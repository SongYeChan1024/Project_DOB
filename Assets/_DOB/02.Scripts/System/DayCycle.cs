// =====================================================================
// DayCycle — 하루 단계의 유일한 소유자 (순수 C#)
// 호출자 : GameManager (Begin, TrySettle), DayFlowPresenter (TryComplete)
// 상태   : 현재 일차, 현재 단계, 이번 일차 정산 여부
// 의사코드:
//   Begin(일차)   — 1일차는 튜토리얼이라 영업부터, 2일차부터는 아침보고부터 시작한다. 정산 여부를 초기화한다.
//   TryComplete(단계) — 요청한 단계가 지금 단계일 때만 아침보고→준비, 준비→영업으로 넘긴다. 중복·다른 단계는 거절.
//   TrySettle(모든 방문 종료) — 영업 단계에서 오늘 손님의 모든 방문(방명록 완성·돌려보냄·떠남)이 끝나면
//                마감으로 넘기고 정산을 한 번만 확정한다.
//   단계가 바뀔 때마다 PhaseChanged로 알린다.
// 💡 영업 시간 제한은 없다 (CurrentDecisions 「영업 시간 없음」, 팀 합의 전). 마감은 방문 종료로만 일어난다.
// 정산 뒤 다음 일차는 저장된 시작 스냅샷으로 베이스를 다시 열어 Begin(일차+1)으로 시작한다.
// =====================================================================
using System;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>하루 단계 전환과 정산 1회를 관리한다. 화면·저장은 모른다.</summary>
    public sealed class DayCycle
    {
        /// <summary>현재 일차. 1부터 시작한다.</summary>
        public int Day { get; private set; }
        /// <summary>현재 하루 단계.</summary>
        public DayPhase Phase { get; private set; }
        /// <summary>이번 일차의 정산이 확정됐는지.</summary>
        public bool IsSettled { get; private set; }
        /// <summary>Begin 이후 단계가 바뀔 때마다 새 단계를 알린다.</summary>
        public event Action<DayPhase> PhaseChanged;

        /// <summary>일차를 시작한다. 1일차는 영업, 그 뒤는 아침보고부터.</summary>
        public void Begin(int day)
        {
            if (day < 1)
                throw new ArgumentOutOfRangeException(nameof(day));
            Day = day;
            IsSettled = false;
            Enter(day == 1 ? DayPhase.Service : DayPhase.MorningReport);
        }

        /// <summary>화면에서 끝낸 단계를 다음으로 넘긴다. 지금 단계와 다르면 거절 사유를 돌려준다.</summary>
        public bool TryComplete(DayPhase phase, out string reason)
        {
            if (phase != Phase)
            {
                reason = $"지금은 {Phase} 단계입니다.";
                return false;
            }
            switch (phase)
            {
                case DayPhase.MorningReport:
                    Enter(DayPhase.Preparation);
                    break;
                case DayPhase.Preparation:
                    Enter(DayPhase.Service);
                    break;
                default:
                    // 영업은 방문 종료(정산)로만 끝나고, 마감은 다음 일차 진입으로 끝난다.
                    reason = $"{phase} 단계는 화면 확인으로 끝나지 않습니다.";
                    return false;
            }
            reason = null;
            return true;
        }

        /// <summary>영업 중 오늘의 모든 방문이 끝났으면 마감으로 넘기고 정산을 한 번 확정한다.</summary>
        public bool TrySettle(bool allVisitsEnded, out string reason)
        {
            if (IsSettled)
                reason = "이미 정산한 일차입니다.";
            else if (Phase != DayPhase.Service)
                reason = "영업 중에만 정산합니다.";
            else if (!allVisitsEnded)
                reason = "아직 끝나지 않은 방문이 있습니다.";
            else
            {
                IsSettled = true;
                reason = null;
                Enter(DayPhase.Closing);
                return true;
            }
            return false;
        }

        private void Enter(DayPhase phase)
        {
            Phase = phase;
            PhaseChanged?.Invoke(phase);
        }
    }
}
