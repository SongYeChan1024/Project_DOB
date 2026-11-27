// =====================================================================
// SettlementManager — 탐험 판정 결과·귀환 보고·층 진행의 총괄 (FeatureManagerMap F7·F9)
// 역할   : 오늘 서빙한 방문의 결과 기록(VisitResultLog), 오늘 아침의 귀환 보고(ReturnReportLog), 층 진행(FloorProgress)을 만들고 요청을 전달한다.
//          규칙·상태는 세 System이 소유하며 여기서 다시 계산하지 않는다.
// 호출자 : GameManager (BeginDay, RecordVisit, ConfirmAllReports, 정산 저장 때 명부 갱신 뒤 SettledResults)
//          귀환 보고 화면의 Presenter (Reports, IsReportConfirmed, ConfirmReport) — 화면 연결은 프론트엔드 인계 항목
// 호출   : VisitResultLog, ReturnReportLog, FloorProgress, ExpeditionRewards(08.Data), FloorProgressRules(Resources/SO_FloorProgress)
// 상태   : 없음 (System 두 개의 참조만 가진다)
// 의사코드:
//   BeginDay          — 보상·층 기준이 없으면 false. 일차 시작 스냅샷의 미처리 보고와 재료 재고로 보고를 열고,
//                       저장된 층 진행(없으면 1층만 열림)과 오늘의 결과 기록을 새로 만든다.
//   RecordVisit       — 서빙이 성공한 방문의 결과를 확정한다. 하루를 열기 전이거나 중복 방문이면 false.
//   SettledResults    — 마감 정산이 명부를 갱신한 뒤, 누적 호감 자격으로 전리품을 정한 오늘의 결과(다음 아침의 미처리 보고).
//   ConfirmReport     — 보고 하나를 확정해 전리품을 재고에 한 번 더하고, 처음 확정이면 층 진행에 반영한다(공개 축·새로 연 층).
//                       다시 확정해도 층 진행은 바뀌지 않는다.
//   ConfirmAllReports — 아침 보고가 끝날 때 남은 보고를 모두 같은 방법으로 확정한다.
//   FloorOutcomeOf    — 그 보고가 공개한 축과 연 층(보고 화면 표시용). 화면 연결은 프론트엔드 인계 항목.
// 손님의 목적지 층 선택은 🟡 미정이라 손님은 아직 1층으로만 간다.
// =====================================================================
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>방문 결과 기록과 귀환 보고를 하루 단위로 연다. 실제 상태는 아래 System이 소유한다.</summary>
    [DisallowMultipleComponent]
    public sealed class SettlementManager : MonoBehaviour
    {
        /// <summary>전리품 자격 호감·개수와 층별 희귀 재료(08.Data/SO_ExpeditionRewards).</summary>
        [SerializeField] private ExpeditionRewards rewards;

        private VisitResultLog results;
        private ReturnReportLog reports;
        private FloorProgress floors;

        /// <summary>오늘 확정된 방문 결과. 전리품은 비어 있다(정산 때 SettledResults로 정한다).</summary>
        public IReadOnlyList<VisitResult> TodayResults => results != null ? results.Results : System.Array.Empty<VisitResult>();
        /// <summary>오늘 아침의 귀환 보고(전날 방문 결과). 조회는 상태를 바꾸지 않는다.</summary>
        public IReadOnlyList<VisitResult> Reports => reports != null ? reports.Reports : System.Array.Empty<VisitResult>();

        /// <summary>하루를 연다. 보상·층 기준이나 재고가 없으면 아무것도 바꾸지 않고 false.</summary>
        /// <param name="savedFloors">일차 시작 스냅샷의 층 진행. 없으면 1층만 열림.</param>
        public bool BeginDay(IEnumerable<VisitResult> pendingReports, MaterialStock stock, FloorProgressState savedFloors = null)
        {
            var floorRules = FloorProgressRules.Load();
            if (!rewards || stock == null || !floorRules || floorRules.Validate() != null)
                return false;
            floors = new FloorProgress(floorRules, savedFloors);
            reports = new ReturnReportLog(pendingReports, stock);
            results = new VisitResultLog();
            return true;
        }

        /// <summary>서빙이 성공한 방문의 결과를 확정한다. 축별 부족은 RiskAxes.All 순서.</summary>
        public bool RecordVisit(string guestId, string visitId, string guestName, int floor, string dish, bool usedSeasoning,
            IReadOnlyList<int> shortfalls, IReadOnlyList<int> risks = null, IReadOnlyList<int> protections = null) =>
            results != null && results.TryRecord(guestId, visitId, guestName, floor, dish, usedSeasoning, shortfalls, risks, protections, out _);

        /// <summary>정산 뒤 명부로 전리품·선물을 정한 오늘의 결과. 마감 정산이 명부를 갱신한 다음에 부른다. 하루를 열기 전이면 빈 목록.</summary>
        /// <param name="findGuest">손님 ID로 정산 뒤 명부 기록을 찾는다 (RelationshipManager.FindGuest).</param>
        public IReadOnlyList<VisitResult> SettledResults(System.Func<string, GuestRecord> findGuest) =>
            results != null ? results.WithGifts(findGuest, rewards.GiftAffinityMin, rewards.RareMaterial, rewards.GiftCount)
                : System.Array.Empty<VisitResult>();

        /// <summary>오늘 아침 보고에서 처음 얻는 재료인지 (보고 화면의 "신규" 표시용). 하루를 열기 전이면 false.</summary>
        public bool IsNewMaterial(string material) => reports != null && reports.IsNewMaterial(material);

        /// <summary>이 방문의 귀환 보고를 이미 처리했는지.</summary>
        public bool IsReportConfirmed(string visitId) => reports != null && reports.IsConfirmed(visitId);

        /// <summary>귀환 보고 하나를 확정한다. 처음 확정이면 층 진행에 반영한다. 하루를 열기 전이면 NotFound.</summary>
        public ReportConfirmResult ConfirmReport(string visitId)
        {
            var result = reports != null ? reports.Confirm(visitId) : ReportConfirmResult.NotFound;
            if (result == ReportConfirmResult.Confirmed)
            {
                floors.Apply(reports.Find(visitId));
                ReportConfirmed?.Invoke(visitId);
            }
            return result;
        }

        /// <summary>귀환 보고를 처음 확정했을 때 한 번 알린다(전리품이 재고에 들어간 뒤). 레시피 해금·효과음용.</summary>
        public event System.Action<string> ReportConfirmed;

        /// <summary>그 보고가 공개한 위험 축과 새로 연 층. 아직 확정하지 않았으면 null.</summary>
        public FloorOutcome FloorOutcomeOf(string visitId) => floors?.OutcomeOf(visitId);
        /// <summary>열린 층 수. 하루를 열기 전이면 1.</summary>
        public int OpenFloors => floors?.OpenFloors ?? 1;
        /// <summary>그 층에서 공개된 위험 축인지.</summary>
        public bool IsRiskRevealed(int floor, string axis) => floors != null && floors.IsRevealed(floor, axis);
        /// <summary>저장할 층 진행. 하루를 열기 전이면 null.</summary>
        public FloorProgressState FloorSnapshot => floors?.Snapshot();

        /// <summary>이미 확정한 귀환 보고의 방문 ID. 수동 저장이 진행으로 남긴다.</summary>
        public IEnumerable<string> ConfirmedReports => reports != null ? reports.ConfirmedIds : System.Array.Empty<string>();

        /// <summary>수동 저장을 불러올 때 오늘 확정했던 방문 결과를 다시 넣는다. 이미 있는 방문은 건너뛴다.</summary>
        public void RestoreResults(IEnumerable<VisitResult> saved) => results?.Restore(saved);

        /// <summary>남은 귀환 보고를 모두 확정하고 이번에 처리한 건수를 돌려준다.</summary>
        public int ConfirmAllReports()
        {
            int count = 0;
            foreach (var report in Reports)
                if (ConfirmReport(report.VisitId) == ReportConfirmResult.Confirmed)
                    count++;
            return count;
        }
    }
}
