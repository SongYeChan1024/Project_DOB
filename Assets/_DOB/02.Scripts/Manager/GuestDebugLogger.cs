// GuestDebugLogger — 개발용. 손님별 대기 시간·남은 인내심·대기 점수·입맛 점수와 방문 호감을 Console에 남긴다.
// 호출자: 없음 (Basement의 Systems에 붙여 두는 개발용 컴포넌트. 인스펙터에서 끄거나 간격을 바꾼다)
// 호출  : GuestManager(명단 조회, VisitEnded 구독), RelationshipManager(호감 미리보기, 전날 기록·입소문 근거),
//         SettlementManager(오늘의 방문 결과)
// 상태  : 다음 진행 로그까지 남은 시간, 전날 기록을 남긴 방명록(일차 구분용).
// 의사코드:
//   OnEnable — 개발 빌드(에디터 포함)가 아니거나 참조가 빠졌으면 경고 후 스스로 끈다. 손님 종료 알림을 구독한다.
//   Update — 관계 총괄이 새 일차를 열었으면(방명록이 바뀌면) 전날 방문 호감 기록과 S·N·입소문 추가 인원을 한 번 남긴다.
//            명단이 열려 있으면 간격(실시간 초)마다 아직 끝나지 않은 방문의 단계·대기 시간·남은 인내심·
//            지금 서빙하면 받을 대기 점수·입맛 점수를 한 줄씩 남긴다.
//   방문 종료 — 종료 방식과 호감 미리보기(정산과 같은 계산)를 남긴다. 결과가 아직 없는 파견이면 그 사실을 남긴다.
//   OnDisable — 구독을 푼다.
// 모든 수치는 System(ServiceRoster·AffinityJudge)이 계산한 값을 읽기만 하고 상태를 바꾸지 않는다.
using System.Text;
using DOB.Define;
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>개발용 손님 상태 로그. 게임 상태를 바꾸지 않는다.</summary>
    [DisallowMultipleComponent]
    public sealed class GuestDebugLogger : MonoBehaviour
    {
        /// <summary>그날 명단을 소유하는 손님 총괄.</summary>
        [SerializeField] private GuestManager guests;
        /// <summary>방문 호감·입소문을 소유하는 관계 총괄.</summary>
        [SerializeField] private RelationshipManager relationship;
        /// <summary>오늘의 방문 결과(탐험 등급)를 소유하는 정산 총괄.</summary>
        [SerializeField] private SettlementManager settlement;
        /// <summary>진행 중 방문 로그 간격(실시간 초). 디버그 설정이라 기획 수치가 아니다.</summary>
        [SerializeField, Min(0.2f)] private float intervalSeconds = 2f;

        private float untilNextLog;
        private GuestbookLog loggedDay;

        private void OnEnable()
        {
            if (!Debug.isDebugBuild || !guests || !relationship || !settlement)
            {
                if (Debug.isDebugBuild) Debug.LogWarning("[DOB 디버그] 손님·관계·정산 총괄 연결을 확인하세요.", this);
                enabled = false;
                return;
            }
            guests.VisitEnded += OnVisitEnded;
        }

        private void OnDisable()
        {
            if (guests) guests.VisitEnded -= OnVisitEnded;
        }

        private void Update()
        {
            if (relationship.Guestbook != null && relationship.Guestbook != loggedDay)
            {
                loggedDay = relationship.Guestbook;
                LogYesterday();
            }

            var roster = guests.Roster;
            // 일시 정지(timeScale 0) 중에도 상태를 볼 수 있게 실시간으로 센다.
            untilNextLog -= Time.unscaledDeltaTime;
            if (roster == null || untilNextLog > 0f)
                return;
            untilNextLog = intervalSeconds;

            var text = new StringBuilder($"[DOB 디버그] 손님 {roster.Entered}/{roster.Planned}명 입장");
            foreach (var visit in roster.Visits)
            {
                if (visit.Stage == ServiceVisitStage.Ended)
                    continue;
                text.Append($"\n  {NameOf(visit.VisitId)} {visit.Stage} 대기 {visit.WaitSeconds:0.0}초 · 인내심 {roster.PatienceLeft(visit):0.0}초 남음" +
                    $" · 대기 점수 {roster.WaitScoreNow(visit)} · 입맛 {RelationshipManager.TasteScore}");
            }
            Debug.Log(text.ToString(), this);
        }

        private void LogYesterday()
        {
            var (sum, count) = relationship.YesterdayTally;
            var text = new StringBuilder($"[DOB 디버그] 전날 방문 호감 {relationship.YesterdayAffinities.Count}건 · S {sum} · N {count}" +
                $" → 오늘 입소문 +{relationship.WordOfMouthBonus}명");
            foreach (var a in relationship.YesterdayAffinities)
                text.Append($"\n  {Short(a.VisitId)} {a.End} 대기 {a.Wait} + 입맛 {a.Taste} + 탐험 {a.Expedition} = {a.Total}");
            Debug.Log(text.ToString(), this);
        }

        private void OnVisitEnded(string visitId, ServiceVisitEnd end)
        {
            var visit = guests.Roster?.Find(visitId);
            var affinity = relationship.PreviewAffinity(visit, settlement.TodayResults);
            Debug.Log(affinity == null
                ? $"[DOB 디버그] {NameOf(visitId)} {end} — 탐험 결과가 아직 없어 호감을 계산하지 못했습니다."
                : $"[DOB 디버그] {NameOf(visitId)} {end} — 호감 대기 {affinity.Wait} + 입맛 {affinity.Taste} + 탐험 {affinity.Expedition} = {affinity.Total}" +
                  $" (대기 {visit.WaitSeconds:0.0}초)", this);
        }

        private string NameOf(string visitId)
        {
            var agent = guests.Find(visitId);
            string name = agent && agent.Visit != null ? agent.Visit.GuestName : "?";
            return $"{name}({Short(visitId)})";
        }

        // 로그를 읽기 쉽게 방문 ID 앞 6자리만 쓴다.
        private static string Short(string id) => string.IsNullOrEmpty(id) || id.Length <= 6 ? id : id.Substring(0, 6);
    }
}
