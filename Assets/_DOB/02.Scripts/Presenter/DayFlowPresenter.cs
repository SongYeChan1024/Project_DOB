// =====================================================================
// DayFlowPresenter — 하루 단계와 아침보고·준비·방명록 화면을 잇는다 (순수 C#, GameManager가 만들고 Dispose)
// 호출자 : GameManager (생성, Dispose)
// 호출   : DayCycle.TryComplete, MorningReportView·PreparationView·GuestbookView (Show, Hide)
// 상태   : 연결된 DayCycle·GuestbookLog·화면과 표시값 조회 함수. 게임 상태는 갖지 않는다.
// 의사코드:
//   생성     — 단계 변경·화면 입력·방명록 완성·입구 문패 [문 열기]를 구독한다.
//   단계 변경 — 아침보고면 보고 화면, 준비면 준비 화면을 보이고 나머지는 숨긴다.
//   보고 확인 — DayCycle에 아침보고 완료를 요청한다. 중복 클릭은 DayCycle이 거절한다.
//   영업 시작 — 준비 화면 버튼이나 입구 문패 [문 열기] 모두 준비 완료로 요청한다.
//   글 완성   — 완성된 방명록 글을 방문 순서대로 방명록 화면에 보인다. 닫기를 누르면 숨긴다.
//   Dispose  — 구독을 모두 풀고 화면을 숨긴다. 화면을 다시 열어도 중복 구독하지 않는다.
// 화면 교체: 각 View의 상속 클래스를 바꿔 BasementHud에 연결하면 이 클래스는 그대로 쓴다.
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using DOB.UI;

namespace DOB.Manager
{
    /// <summary>하루 진행 입력을 DayCycle 요청으로 옮기고 결과를 화면에 표시한다. 규칙은 계산하지 않는다.</summary>
    public sealed class DayFlowPresenter : IDisposable
    {
        private readonly DayCycle cycle;
        private readonly GuestbookLog guestbook;
        private readonly MorningReportView report;
        private readonly PreparationView preparation;
        private readonly GuestbookView guestbookView;
        private readonly Func<(int Gold, int GarlicStock)> stock;
        private readonly List<GuestbookLineData> lines = new List<GuestbookLineData>();

        /// <param name="stock">화면에 보일 현재 금화·재고. 값의 소유자(주방)에게서 읽는다.</param>
        public DayFlowPresenter(DayCycle cycle, GuestbookLog guestbook, MorningReportView report, PreparationView preparation,
            GuestbookView guestbookView, Func<(int Gold, int GarlicStock)> stock)
        {
            this.cycle = cycle ?? throw new ArgumentNullException(nameof(cycle));
            this.guestbook = guestbook ?? throw new ArgumentNullException(nameof(guestbook));
            this.report = report;
            this.preparation = preparation;
            this.guestbookView = guestbookView;
            this.stock = stock ?? throw new ArgumentNullException(nameof(stock));

            cycle.PhaseChanged += OnPhaseChanged;
            guestbook.EntryCompleted += OnEntryCompleted;
            report.Confirmed += OnReportConfirmed;
            preparation.StartRequested += OnStartRequested;
            guestbookView.Closed += guestbookView.Hide;
            UIManager.StartBusinessRequested += OnStartRequested;
            report.Hide();
            preparation.Hide();
            guestbookView.Hide();
        }

        private void OnPhaseChanged(DayPhase phase)
        {
            var (gold, garlic) = stock();
            if (phase == DayPhase.MorningReport) report.Show(new MorningReportData(cycle.Day, gold, garlic));
            else report.Hide();
            if (phase == DayPhase.Preparation) preparation.Show(new PreparationData(cycle.Day, gold, garlic));
            else preparation.Hide();
        }

        // 중복 클릭·다른 단계의 요청은 DayCycle이 거절하므로 결과를 따로 처리하지 않는다.
        private void OnReportConfirmed() => cycle.TryComplete(DayPhase.MorningReport, out _);
        private void OnStartRequested() => cycle.TryComplete(DayPhase.Preparation, out _);

        private void OnEntryCompleted(GuestbookEntry _)
        {
            lines.Clear();
            foreach (var entry in guestbook.Entries)
                if (entry.IsComplete)
                    lines.Add(new GuestbookLineData(entry.GuestName, entry.Line));
            guestbookView.Show(lines);
        }

        public void Dispose()
        {
            cycle.PhaseChanged -= OnPhaseChanged;
            guestbook.EntryCompleted -= OnEntryCompleted;
            UIManager.StartBusinessRequested -= OnStartRequested;
            // 씬 종료 중이면 화면이 먼저 파괴됐을 수 있다.
            if (report) { report.Confirmed -= OnReportConfirmed; report.Hide(); }
            if (preparation) { preparation.StartRequested -= OnStartRequested; preparation.Hide(); }
            if (guestbookView) { guestbookView.Closed -= guestbookView.Hide; guestbookView.Hide(); }
        }
    }
}
