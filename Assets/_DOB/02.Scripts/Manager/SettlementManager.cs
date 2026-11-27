// =====================================================================
// SettlementManager — 탐험 판정 결과와 귀환 보고의 총괄 (FeatureManagerMap F7·F9)
// 역할   : 오늘 서빙한 방문의 결과 기록(VisitResultLog)과 오늘 아침의 귀환 보고(ReturnReportLog)를 만들고 요청을 전달한다.
//          규칙·상태는 두 System이 소유하며 여기서 다시 계산하지 않는다.
// 호출자 : GameManager (BeginDay, RecordVisit, ConfirmAllReports, 정산 저장 때 명부 갱신 뒤 SettledResults)
//          귀환 보고 화면의 Presenter (Reports, IsReportConfirmed, ConfirmReport) — 화면 연결은 프론트엔드 인계 항목
// 호출   : VisitResultLog, ReturnReportLog, ExpeditionRewards(08.Data)
// 상태   : 없음 (System 두 개의 참조만 가진다)
// 의사코드:
//   BeginDay          — 보상 기준이 연결되지 않았으면 false. 일차 시작 스냅샷의 미처리 보고와 재료 재고로 보고를 열고,
//                       오늘의 결과 기록을 새로 만든다.
//   RecordVisit       — 서빙이 성공한 방문의 결과를 확정한다. 하루를 열기 전이거나 중복 방문이면 false.
//   SettledResults    — 마감 정산이 명부를 갱신한 뒤, 누적 호감 자격으로 전리품을 정한 오늘의 결과(다음 아침의 미처리 보고).
//   ConfirmReport     — 보고 하나를 확정해 전리품을 재고에 한 번 더한다.
//   ConfirmAllReports — 아침 보고가 끝날 때 남은 보고를 모두 확정한다.
// 층 개방·수첩의 층 위험 공개는 후속이다.
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

        /// <summary>오늘 확정된 방문 결과. 전리품은 비어 있다(정산 때 SettledResults로 정한다).</summary>
        public IReadOnlyList<VisitResult> TodayResults => results != null ? results.Results : System.Array.Empty<VisitResult>();
        /// <summary>오늘 아침의 귀환 보고(전날 방문 결과). 조회는 상태를 바꾸지 않는다.</summary>
        public IReadOnlyList<VisitResult> Reports => reports != null ? reports.Reports : System.Array.Empty<VisitResult>();

        /// <summary>하루를 연다. 보상 기준이나 재고가 없으면 아무것도 바꾸지 않고 false.</summary>
        public bool BeginDay(IEnumerable<VisitResult> pendingReports, MaterialStock stock)
        {
            if (!rewards || stock == null)
                return false;
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

        /// <summary>귀환 보고 하나를 확정한다. 하루를 열기 전이면 NotFound.</summary>
        public ReportConfirmResult ConfirmReport(string visitId) =>
            reports != null ? reports.Confirm(visitId) : ReportConfirmResult.NotFound;

        /// <summary>남은 귀환 보고를 모두 확정하고 이번에 처리한 건수를 돌려준다.</summary>
        public int ConfirmAllReports() => reports != null ? reports.ConfirmAll() : 0;
    }
}
