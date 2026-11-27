// GuestManager — 하루 영업의 손님 명단(ServiceRoster)과 그날 올 손님 목록, 식당 안 손님 오브젝트를 소유한다.
// 호출자: GameManager(PrepareDay로 그날 입력 전달, 입장·대기 Δ로 Tick, 마감 판정, 정산 때 TodaysGuests),
//         FirstGuestService(식당이 준비되면 BeginService, 끝나면 EndService), FirstGuestKitchen(VisitEnded로 주문 취소).
// 호출  : DailyRoster(그날 명단 구성), ServiceRoster, GuestArrivalPool·GuestDialogueGenerator(Resources의 손님 후보·대사 규칙), GuestAgent.
// 상태  : 기준값·손님 템플릿(직렬화), 그날 입력(일차·입소문 신규 인원·올 수 있는 손님·쓰는 이름·시드), 그날 난수,
//         입장 순서대로의 그날 손님 신원, 입구 위치, 명단, 입장한 손님 에이전트 목록.
// 의사코드:
//   PrepareDay  — GameManager가 일차 시작 스냅샷에서 읽은 그날 입력을 보관한다. 명부(누가 올 수 있는지)는 RelationshipManager가 소유한다.
//   BeginService — 이미 열렸거나 기준값·후보가 없으면 거절. 준비된 입력이 그 일차 것이 아니면 재방문 없음·새 시드로 연다.
//                  그날 시드로 난수를 만들고 DailyRoster로 재방문 + 신규 손님의 신원과 입장 순서를 한 번에 정한다.
//                  예정 수(= 그날 손님 수)로 새 명단을 열고 Tick(0)을 한 번 돌린다. 첫 손님은 첫 입장 대기 뒤 들어온다.
//   Tick(Δ)     — 명단에 입장·대기 Δ를 넘긴다(대기 이탈은 명단이 확정하고 VisitEnded로 알린다).
//                  명단이 입장을 허락하는 동안 그날 손님 목록의 다음 신원으로 방문 원고를 만들어 등록하고 에이전트를 입구에 세운다.
//                  재방문 손님은 명부의 신원과 누적 호감을, 처음 온 손님은 호감 0을 대사 생성기에 넘긴다. 피로는 방문마다 뽑는다.
//                  첫 손님은 씬의 템플릿을 그대로 쓰고 이후 손님은 템플릿을 복제한다.
//   EndService  — 구독을 풀고 복제한 손님을 파괴하고 템플릿을 끈다. 명단을 비운다. 그날 손님 목록은 정산이 읽도록 남긴다.
// 손님·방문 ID, 입장 간격, 대사 추첨이 모두 그날 난수를 쓰므로 같은 일차 시작 스냅샷으로 열면 같은 손님이 같은 순서로 온다.
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
        /// <summary>복제할 손님 오브젝트(Basement의 PF_Guest). 첫 손님은 이 오브젝트 자체다.</summary>
        // shortcut: 씬의 손님 인스턴스를 복제한다. 손님 외형 프리팹을 따로 두게 되면 프리팹 직렬화 참조로 바꾼다.
        [SerializeField] private Transform template;

        // 통합 테스트가 손님 수·간격을 줄이려고 씬 기준값 대신 넣는다. 게임 코드는 쓰지 않는다.
        internal static ServiceRosterSettings SettingsOverride;
        private readonly List<GuestAgent> agents = new List<GuestAgent>();
        private List<GuestRecord> todaysGuests = new List<GuestRecord>();
        private System.Random random;
        private int day;
        private Transform entrance;
        // PrepareDay가 넘긴 그날 입력. 그 일차의 것이 아니면 쓰지 않는다.
        private int preparedDay;
        private int wordOfMouthBonus;
        private IReadOnlyList<GuestRecord> returning;
        private IEnumerable<string> takenNames;
        private int daySeed;

        private ServiceRosterSettings Settings => SettingsOverride ? SettingsOverride : settings;
        /// <summary>오늘 명단. 영업 전·후에는 null.</summary>
        public ServiceRoster Roster { get; private set; }
        /// <summary>입장한 순서의 손님 오브젝트.</summary>
        public IReadOnlyList<GuestAgent> Agents => agents;
        /// <summary>입장 순서대로의 그날 손님 신원(재방문은 명부 기록, 신규는 그날 뽑은 신원). 마감 정산이 명부 등록에 쓴다.</summary>
        public IReadOnlyList<GuestRecord> TodaysGuests => todaysGuests;
        /// <summary>서빙을 받지 못하고 기다릴 수 있는 한도(초). 말풍선의 남은 비율에 쓴다.</summary>
        public float LeaveAfterSeconds => Settings.LeaveAfterSeconds;
        /// <summary>서빙 직후 기쁨 표정을 보이는 시간(초).</summary>
        public float ServedMoodSeconds => Settings.ServedMoodSeconds;
        /// <summary>입구 줄 간격(m).</summary>
        public float QueueSpacing => Settings.QueueSpacing;
        /// <summary>기준값·템플릿 연결 오류. 문제가 없으면 null.</summary>
        public string SetupError => !template ? "손님 템플릿이 없습니다." : !Settings ? "영업 손님 기준값이 없습니다." : Settings.Validate();
        /// <summary>방문 ID, 종료 방식. 명단이 방문을 끝낸 순간 한 번 알린다.</summary>
        public event Action<string, ServiceVisitEnd> VisitEnded;

        /// <summary>오늘 손님의 입맛 태그. 오늘 손님이 아니거나 입맛 기준이 없어 정하지 못했으면 빈 목록.</summary>
        public IReadOnlyList<string> TastesOf(string guestId) =>
            todaysGuests.Find(g => g.GuestId == guestId)?.Tastes ?? (IReadOnlyList<string>)Array.Empty<string>();

        /// <summary>서빙한 접시로 그 손님의 입맛 점수(−2·0·+2)를 낸다. 입맛 기준이나 손님 입맛이 없으면 null.</summary>
        public int? TasteScoreFor(string guestId, string dish, IEnumerable<string> seasonings)
        {
            var rules = Resources.Load<TasteRules>(TasteRules.ResourceName);
            var tastes = TastesOf(guestId);
            if (!rules || tastes.Count == 0)
                return null;
            return TasteJudge.Score(tastes, TasteJudge.DishFlavors(rules, dish, seasonings));
        }

        /// <summary>그 방문의 손님 오브젝트. 없으면 null.</summary>
        public GuestAgent Find(string visitId) => agents.Find(a => a.VisitId == visitId);

        /// <summary>그 일차 영업의 입력을 받는다. 명단은 BeginService에서 만든다.</summary>
        /// <param name="comingToday">명부에서 그 일차에 올 수 있는 손님.</param>
        /// <param name="knownNames">신규 손님 이름에서 뺄 이름(영구 거절되지 않은 명부 손님).</param>
        /// <param name="seed">그 일차의 난수 시드. 같은 시드면 같은 손님이 같은 순서로 온다.</param>
        public void PrepareDay(int serviceDay, int wordOfMouthNewGuests, IReadOnlyList<GuestRecord> comingToday,
            IEnumerable<string> knownNames, int seed)
        {
            preparedDay = serviceDay;
            wordOfMouthBonus = Mathf.Max(0, wordOfMouthNewGuests);
            returning = comingToday;
            takenNames = knownNames;
            daySeed = seed;
        }

        /// <summary>식당이 준비되면 그날 손님 목록과 명단을 연다. 첫 손님은 첫 입장 대기 뒤 Tick에서 입구에 선다.</summary>
        public bool BeginService(int serviceDay, Transform guestEntrance)
        {
            string error = SetupError;
            var pool = Resources.Load<GuestArrivalPool>(GuestArrivalPool.ResourceName);
            if (error == null && !pool) error = "Resources에서 손님 후보를 찾을 수 없습니다.";
            if (Roster != null || !guestEntrance || error != null)
            {
                if (error != null) Debug.LogError("[DOB] " + error, this);
                return false;
            }
            day = serviceDay;
            entrance = guestEntrance;
            bool prepared = preparedDay == serviceDay;
            // 준비된 입력이 없으면(씬만 따로 실행) 재방문 없이 새 시드로 연다.
            random = new System.Random(prepared ? daySeed : Guid.NewGuid().GetHashCode());
            todaysGuests = DailyRoster.Compose(Settings, pool, day, prepared ? returning : null, prepared ? takenNames : null,
                prepared ? wordOfMouthBonus : 0, random, Resources.Load<TasteRules>(TasteRules.ResourceName));
            Roster = new ServiceRoster(Settings, random, todaysGuests.Count);
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
                var guest = todaysGuests[Roster.Entered];
                var visit = CreateVisit(guest);
                if (visit == null || !Roster.Admit(visit.VisitId, visit.GuestId))
                    return;
                Spawn(visit);
            }
        }

        /// <summary>수동 저장을 불러올 때 저장된 입장 수만큼 같은 난수 순서로 방문을 다시 만들고 단계·대기·점수를 덮어쓴다.
        /// 끝난 방문은 손님 오브젝트를 두지 않는다. 방문 ID가 저장과 다르면 false(그때까지 만든 방문은 남는다).</summary>
        public bool RestoreProgress(RosterProgress saved)
        {
            if (Roster == null || saved == null || saved.Visits.Count > todaysGuests.Count || Roster.Entered > saved.Visits.Count)
                return false;
            var created = new List<GuestDialogueVisit>();
            // 첫 입장 대기가 0이면 BeginService가 이미 앞 손님을 들였으므로 그 다음부터 만든다.
            for (int i = Roster.Entered; i < saved.Visits.Count; i++)
            {
                var visit = CreateVisit(todaysGuests[i]);
                if (visit == null || visit.VisitId != saved.Visits[i].VisitId || !Roster.AdmitRestored(visit.VisitId, visit.GuestId))
                    return false;
                created.Add(visit);
            }
            if (!Roster.Restore(saved))
                return false;
            foreach (var agent in agents)
                if (Roster.Find(agent.VisitId).Stage == ServiceVisitStage.Ended)
                {
                    agent.Move = GuestAgent.Movement.Gone;
                    agent.gameObject.SetActive(false);
                }
            foreach (var visit in created)
                if (Roster.Find(visit.VisitId).Stage != ServiceVisitStage.Ended)
                    Spawn(visit);
            return true;
        }

        private void Spawn(GuestDialogueVisit visit)
        {
            var body = agents.Count == 0 ? template : Instantiate(template, template.parent);
            if (agents.Count > 0) body.name = template.name + "_" + (agents.Count + 1);
            // 캐릭터 프리팹을 고치지 않도록 이동 컴포넌트는 실행 중에 붙인다.
            var agent = body.TryGetComponent<GuestAgent>(out var found) ? found : body.gameObject.AddComponent<GuestAgent>();
            agent.Begin(visit, entrance);
            agents.Add(agent);
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

        private GuestDialogueVisit CreateVisit(GuestRecord guest)
        {
            var pool = Resources.Load<GuestArrivalPool>(GuestArrivalPool.ResourceName);
            var rules = Resources.Load<GuestDialogueRules>("SO_GuestDialogueRules");
            if (!pool || !rules)
            {
                Debug.LogError("[DOB] Resources에서 손님 후보 또는 대사 규칙을 찾을 수 없습니다.");
                return null;
            }

            var input = new GuestVisitInput
            {
                GuestId = guest.GuestId,
                VisitId = DailyRoster.NewId(random),
                GuestName = guest.Name,
                Race = guest.Race,
                Job = guest.Job,
                Day = day,
                Floor = 1,
                Affinity = guest.Affinity,
                IsTired = pool.DrawTired(day, random),
                PersonalWeakness = guest.Weakness
            };
            return GuestDialogueGenerator.Generate(rules, input, random);
        }
    }
}
