// GuestManager — 하루 영업의 손님 명단(ServiceRoster)과 식당 안 손님 오브젝트를 소유한다.
// 호출자: FirstGuestService(식당이 준비되면 BeginService, 끝나면 EndService), GameManager(입장·대기 Δ로 Tick, 마감 판정),
//         FirstGuestKitchen(VisitEnded로 주문 취소).
// 호출  : ServiceRoster, GuestArrivalPool·GuestDialogueGenerator(Resources의 손님 후보·대사 규칙), GuestAgent.
// 상태  : 기준값·손님 템플릿(직렬화), 현재 일차, 입구 위치, 명단, 입장한 손님 에이전트 목록.
// 의사코드:
//   BeginService — 이미 열렸거나 기준값이 잘못되면 거절. 새 명단을 열고 바로 Tick(0)으로 첫 손님을 입장시킨다.
//   Tick(Δ)     — 명단에 입장·대기 Δ를 넘긴다(대기 이탈은 명단이 확정하고 VisitEnded로 알린다).
//                  명단이 입장을 허락하는 동안 방문 원고를 만들어 등록하고 에이전트를 입구에 세운다.
//                  첫 손님은 씬의 템플릿을 그대로 쓰고 이후 손님은 템플릿을 복제한다.
//   EndService  — 구독을 풀고 복제한 손님을 파괴하고 템플릿을 끈다. 명단을 비운다.
// 방문 원고를 만들 수 없으면(Resources 누락) 오류를 남기고 그 입장을 미룬 채 둔다.
using System;
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>그날 손님 명단과 손님 오브젝트. 방문 단계는 명단이, 걷기는 FirstGuestService가 맡는다.</summary>
    public sealed class GuestManager : MonoBehaviour
    {
        /// <summary>손님 수·입장 간격·대기 기준값(08.Data/SO_ServiceRoster).</summary>
        [SerializeField] private ServiceRosterSettings settings;
        /// <summary>복제할 손님 오브젝트(Gameplay의 PF_Guest). 첫 손님은 이 오브젝트 자체다.</summary>
        // shortcut: 씬의 손님 인스턴스를 복제한다. 손님 외형 프리팹을 따로 두게 되면 프리팹 직렬화 참조로 바꾼다.
        [SerializeField] private Transform template;

        private static readonly System.Random random = new System.Random();
        // 통합 테스트가 손님 수·간격을 줄이려고 씬 기준값 대신 넣는다. 게임 코드는 쓰지 않는다.
        internal static ServiceRosterSettings SettingsOverride;
        private readonly List<GuestAgent> agents = new List<GuestAgent>();
        private int day;
        private Transform entrance;

        private ServiceRosterSettings Settings => SettingsOverride ? SettingsOverride : settings;
        /// <summary>오늘 명단. 영업 전·후에는 null.</summary>
        public ServiceRoster Roster { get; private set; }
        /// <summary>입장한 순서의 손님 오브젝트.</summary>
        public IReadOnlyList<GuestAgent> Agents => agents;
        /// <summary>입구 줄 간격(m).</summary>
        public float QueueSpacing => Settings.QueueSpacing;
        /// <summary>기준값·템플릿 연결 오류. 문제가 없으면 null.</summary>
        public string SetupError => !template ? "손님 템플릿이 없습니다." : !Settings ? "영업 손님 기준값이 없습니다." : Settings.Validate();
        /// <summary>방문 ID, 종료 방식. 명단이 방문을 끝낸 순간 한 번 알린다.</summary>
        public event Action<string, ServiceVisitEnd> VisitEnded;

        /// <summary>그 방문의 손님 오브젝트. 없으면 null.</summary>
        public GuestAgent Find(string visitId) => agents.Find(a => a.VisitId == visitId);

        /// <summary>식당이 준비되면 그날 명단을 열고 첫 손님을 입구에 세운다.</summary>
        public bool BeginService(int serviceDay, Transform guestEntrance)
        {
            string error = SetupError;
            if (Roster != null || !guestEntrance || error != null)
            {
                if (error != null) Debug.LogError("[DOB] " + error, this);
                return false;
            }
            day = serviceDay;
            entrance = guestEntrance;
            Roster = new ServiceRoster(Settings, random);
            Roster.VisitEnded += OnVisitEnded;
            template.gameObject.SetActive(false);
            Tick(0f);
            return true;
        }

        /// <summary>입장·대기 시계가 허용한 경과 시간을 명단에 넘기고, 허락된 입장을 처리한다.</summary>
        public void Tick(float arrivalAndWaitDelta)
        {
            if (Roster == null)
                return;
            Roster.Tick(arrivalAndWaitDelta);
            while (Roster.CanAdmit)
            {
                var visit = CreateVisit(day);
                if (visit == null || !Roster.Admit(visit.VisitId, visit.GuestId))
                    return;
                var body = agents.Count == 0 ? template : Instantiate(template, template.parent);
                if (agents.Count > 0) body.name = template.name + "_" + (agents.Count + 1);
                // 캐릭터 프리팹을 고치지 않도록 이동 컴포넌트는 실행 중에 붙인다.
                var agent = body.TryGetComponent<GuestAgent>(out var found) ? found : body.gameObject.AddComponent<GuestAgent>();
                agent.Begin(visit, entrance);
                agents.Add(agent);
            }
        }

        /// <summary>영업이 끝나거나 식당이 내려가면 손님 오브젝트를 정리한다.</summary>
        public void EndService()
        {
            if (Roster != null)
                Roster.VisitEnded -= OnVisitEnded;
            Roster = null;
            foreach (var agent in agents)
                if (agent && agent.transform != template)
                    Destroy(agent.gameObject);
            agents.Clear();
            if (template)
                template.gameObject.SetActive(false);
        }

        private void OnDestroy() => EndService();

        private void OnVisitEnded(string visitId, ServiceVisitEnd end) => VisitEnded?.Invoke(visitId, end);

        private static GuestDialogueVisit CreateVisit(int day)
        {
            var pool = Resources.Load<GuestArrivalPool>(GuestArrivalPool.ResourceName);
            var rules = Resources.Load<GuestDialogueRules>("SO_GuestDialogueRules");
            if (!pool || !rules)
            {
                Debug.LogError("[DOB] Resources에서 손님 후보 또는 대사 규칙을 찾을 수 없습니다.");
                return null;
            }

            var guest = pool.Draw(day, random);
            var input = new GuestVisitInput
            {
                GuestId = Guid.NewGuid().ToString("N"),
                VisitId = Guid.NewGuid().ToString("N"),
                GuestName = guest.GuestName,
                Race = guest.Race,
                Job = guest.Job,
                Day = day,
                Floor = 1,
                IsTired = guest.IsTired,
                PersonalWeakness = guest.PersonalWeakness
            };
            return GuestDialogueGenerator.Generate(rules, input, random);
        }
    }
}
