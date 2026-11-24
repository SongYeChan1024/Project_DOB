// =====================================================================
// ReturnReportLog — 오늘 아침의 귀환 보고와 전리품 지급 (순수 C#)
// 호출자 : SettlementManager (일차 시작 때 생성, 화면의 조회·확정 요청과 아침 보고 종료 때의 전체 확정을 전달)
// 호출   : MaterialStock.TryAdd
// 상태   : 전날 방문 결과 목록(보고 순서), 처리한 방문 ID
// 의사코드:
//   생성       — 일차 시작 스냅샷의 미처리 보고를 받는다. 방문 ID가 없거나 겹치는 항목은 예외.
//   Reports    — 조회만 한다. 재고와 처리 여부를 바꾸지 않는다.
//   Confirm    — 목록에 없는 방문이면 NotFound, 이미 처리했으면 AlreadyConfirmed.
//                처음이면 전리품을 재고에 더하고 처리됨으로 표시한다. 금화는 서빙 때 받았으므로 건드리지 않는다.
//   ConfirmAll — 남은 보고를 모두 확정하고 이번에 처리한 건수를 돌려준다.
// 보고 도중에는 저장하지 않는다. 불러오면 일차 시작으로 돌아가 재고와 함께 처음부터 다시 받는다 (2026-11-24 송예찬 확인, PR #29).
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>전날 파견한 방문의 귀환 보고를 제공하고 전리품을 한 번만 지급한다.</summary>
    public sealed class ReturnReportLog
    {
        private readonly List<VisitResult> reports = new List<VisitResult>();
        private readonly HashSet<string> confirmed = new HashSet<string>();
        private readonly MaterialStock stock;

        /// <summary>오늘 아침의 귀환 보고. 전날 결과가 확정된 순서.</summary>
        public IReadOnlyList<VisitResult> Reports => reports;

        public ReturnReportLog(IEnumerable<VisitResult> pending, MaterialStock stock)
        {
            this.stock = stock ?? throw new ArgumentNullException(nameof(stock));
            if (pending == null)
                return;
            var seen = new HashSet<string>();
            foreach (var report in pending)
            {
                if (report == null || string.IsNullOrEmpty(report.VisitId) || !seen.Add(report.VisitId))
                    throw new ArgumentException("귀환 보고마다 서로 다른 방문 ID가 필요합니다.", nameof(pending));
                reports.Add(report);
            }
        }

        /// <summary>이 방문의 보고를 이미 처리했는지.</summary>
        public bool IsConfirmed(string visitId) => visitId != null && confirmed.Contains(visitId);

        /// <summary>보고 하나를 확정해 전리품을 재고에 한 번 더한다.</summary>
        public ReportConfirmResult Confirm(string visitId)
        {
            var report = reports.Find(r => r.VisitId == visitId);
            if (report == null)
                return ReportConfirmResult.NotFound;
            if (!confirmed.Add(visitId))
                return ReportConfirmResult.AlreadyConfirmed;
            if (report.Loot != null)
                foreach (string material in report.Loot)
                    stock.TryAdd(material, 1);
            return ReportConfirmResult.Confirmed;
        }

        /// <summary>남은 보고를 모두 확정한다. 이번 호출로 처리한 건수를 돌려준다.</summary>
        public int ConfirmAll()
        {
            int count = 0;
            foreach (var report in reports)
                if (Confirm(report.VisitId) == ReportConfirmResult.Confirmed)
                    count++;
            return count;
        }
    }
}
