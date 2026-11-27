// 한 방문의 구운 빵 주문 하나. 재고 확인·차감과 조리 칸·들고 있는 접시 수 제한은 OrderBook이 맡는다.
// 의사코드: 대기 주문만 정해진 밀가루로 조리를 시작하고 넣은 양념 수를 기억한다. 조리 시간이 끝나면 완성 →
// 같은 방문의 접시를 픽업 → 같은 방문에 서빙할 때 한 번만 계산한다(기본가 + 넣은 양념 수). 취소된 주문은 아무것도 못 한다.
using System;

namespace DOB.Systems
{
    public enum FirstGuestOrderState { Waiting, Cooking, Ready, Carrying, Served, Cancelled }

    /// <summary>한 방문의 구운 빵 주문. 재료·가격·시간은 시작할 때 기획 데이터에서 받는다.</summary>
    public sealed class FirstGuestOrder
    {
        private readonly float cookSeconds;
        private readonly int flourRequired;
        private readonly int breadBasePrice;

        public string VisitId { get; }
        public FirstGuestOrderState State { get; private set; } = FirstGuestOrderState.Waiting;
        public float RemainingSeconds { get; private set; }
        /// <summary>조리를 시작할 때 넣은 양념 수. 시작 전에는 0.</summary>
        public int UsedGarlic { get; private set; }

        public FirstGuestOrder(string visitId, float cookSeconds, int flourRequired, int breadBasePrice)
        {
            if (string.IsNullOrWhiteSpace(visitId)) throw new ArgumentException("방문 ID가 필요합니다.", nameof(visitId));
            if (!(cookSeconds > 0f) || float.IsInfinity(cookSeconds)) throw new ArgumentOutOfRangeException(nameof(cookSeconds));
            if (flourRequired <= 0) throw new ArgumentOutOfRangeException(nameof(flourRequired));
            if (breadBasePrice < 0) throw new ArgumentOutOfRangeException(nameof(breadBasePrice));

            VisitId = visitId;
            this.cookSeconds = cookSeconds;
            this.flourRequired = flourRequired;
            this.breadBasePrice = breadBasePrice;
        }

        /// <summary>대기 주문의 조리를 시작한다. 재고는 OrderBook이 먼저 확인한다. 잘못된 재료·중복은 그대로 둔다.</summary>
        internal bool TryStart(int flour, int garlic)
        {
            if (State != FirstGuestOrderState.Waiting || flour != flourRequired || garlic < 0 || breadBasePrice > int.MaxValue - garlic)
                return false;
            UsedGarlic = garlic;
            RemainingSeconds = cookSeconds;
            State = FirstGuestOrderState.Cooking;
            return true;
        }

        /// <summary>게임 쪽 조리 시계가 허용한 경과 시간만 받는다. 응대 중에도 조리는 계속된다.</summary>
        internal void Advance(float cookingDeltaSeconds)
        {
            if (State != FirstGuestOrderState.Cooking || !(cookingDeltaSeconds > 0f) || float.IsInfinity(cookingDeltaSeconds))
                return;

            RemainingSeconds = Math.Max(0f, RemainingSeconds - cookingDeltaSeconds);
            if (RemainingSeconds == 0f)
                State = FirstGuestOrderState.Ready;
        }

        internal bool TryPickUp()
        {
            if (State != FirstGuestOrderState.Ready)
                return false;
            State = FirstGuestOrderState.Carrying;
            return true;
        }

        /// <summary>하루의 고정 맛 점수 2는 가격 배율 1이다. 기본가에 투입한 양념 개수를 더한다.</summary>
        internal bool TryServe(string visitId, out int payment)
        {
            payment = 0;
            if (State != FirstGuestOrderState.Carrying || visitId != VisitId)
                return false;
            State = FirstGuestOrderState.Served;
            payment = breadBasePrice + UsedGarlic;
            return true;
        }

        /// <summary>서빙 전이면 취소한다. 넣은 양념은 돌려주지 않는다.</summary>
        /// <summary>저장 복원용. 저장된 상태·남은 시간·넣은 양념을 그대로 둔다.</summary>
        internal void Restore(FirstGuestOrderState state, float remainingSeconds, int usedGarlic)
        {
            State = state;
            RemainingSeconds = remainingSeconds;
            UsedGarlic = usedGarlic;
        }

        internal bool TryCancel()
        {
            if (State == FirstGuestOrderState.Served || State == FirstGuestOrderState.Cancelled)
                return false;
            State = FirstGuestOrderState.Cancelled;
            return true;
        }
    }
}
