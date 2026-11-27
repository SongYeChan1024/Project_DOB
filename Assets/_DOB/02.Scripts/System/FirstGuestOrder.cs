// 한 방문의 주문 하나. 재고 확인·차감, 레시피 판정과 화구 배정, 들고 있는 접시 수 제한은 OrderBook이 맡는다.
// 의사코드: 대기 주문만 조리를 시작하며, 확정한 레시피 id·양념 id·차지한 화구 번호를 기억한다. 조리 시간이 끝나면 완성 →
// 같은 방문의 접시를 픽업(화구가 빈다) → 같은 방문에 서빙할 때 한 번만 서빙된다(대금은 주방이 PriceJudge로 정한다).
// 취소된 주문은 아무것도 못 하고 화구도 비운다.
using System;

namespace DOB.Systems
{
    public enum FirstGuestOrderState { Waiting, Cooking, Ready, Carrying, Served, Cancelled }

    /// <summary>한 방문의 주문. 레시피는 조리를 확정할 때 정해진다.</summary>
    public sealed class FirstGuestOrder
    {
        private readonly float cookSeconds;

        public string VisitId { get; }
        public FirstGuestOrderState State { get; private set; } = FirstGuestOrderState.Waiting;
        public float RemainingSeconds { get; private set; }
        /// <summary>확정한 레시피 id. 시작 전에는 null.</summary>
        public string RecipeId { get; private set; }
        /// <summary>넣은 양념 id. 넣지 않았거나 시작 전이면 null.</summary>
        public string Spice { get; private set; }
        /// <summary>조리 중·완성 접시가 놓인 화구 번호. 화구에 없으면 -1.</summary>
        public int BurnerIndex { get; private set; } = -1;
        public bool UsedSeasoning => Spice != null;

        public FirstGuestOrder(string visitId, float cookSeconds)
        {
            if (string.IsNullOrWhiteSpace(visitId)) throw new ArgumentException("방문 ID가 필요합니다.", nameof(visitId));
            if (!(cookSeconds > 0f) || float.IsInfinity(cookSeconds)) throw new ArgumentOutOfRangeException(nameof(cookSeconds));
            VisitId = visitId;
            this.cookSeconds = cookSeconds;
        }

        /// <summary>대기 주문의 조리를 시작한다. 재고·화구는 OrderBook이 먼저 확인한다.</summary>
        internal bool TryStart(string recipeId, string spice, int burner)
        {
            if (State != FirstGuestOrderState.Waiting || string.IsNullOrEmpty(recipeId) || burner < 0)
                return false;
            RecipeId = recipeId;
            Spice = spice;
            BurnerIndex = burner;
            RemainingSeconds = cookSeconds;
            State = FirstGuestOrderState.Cooking;
            return true;
        }

        /// <summary>게임 쪽 조리 시계가 허용한 경과 시간만 받는다. 이번 호출로 완성됐으면 true.</summary>
        internal bool Advance(float cookingDeltaSeconds)
        {
            if (State != FirstGuestOrderState.Cooking || !(cookingDeltaSeconds > 0f) || float.IsInfinity(cookingDeltaSeconds))
                return false;
            RemainingSeconds = Math.Max(0f, RemainingSeconds - cookingDeltaSeconds);
            if (RemainingSeconds > 0f)
                return false;
            State = FirstGuestOrderState.Ready;
            return true;
        }

        internal bool TryPickUp()
        {
            if (State != FirstGuestOrderState.Ready)
                return false;
            State = FirstGuestOrderState.Carrying;
            BurnerIndex = -1;
            return true;
        }

        /// <summary>들고 있는 접시를 그 방문에 한 번만 서빙한다.</summary>
        internal bool TryServe(string visitId)
        {
            if (State != FirstGuestOrderState.Carrying || visitId != VisitId)
                return false;
            State = FirstGuestOrderState.Served;
            return true;
        }

        /// <summary>저장 복원용. 저장된 상태·남은 시간·레시피·양념·화구를 그대로 둔다.</summary>
        internal void Restore(FirstGuestOrderState state, float remainingSeconds, string recipeId, string spice, int burner)
        {
            State = state;
            RemainingSeconds = remainingSeconds;
            RecipeId = recipeId;
            Spice = spice;
            BurnerIndex = burner;
        }

        /// <summary>서빙 전이면 취소한다. 넣은 재료·양념은 돌려주지 않는다.</summary>
        internal bool TryCancel()
        {
            if (State == FirstGuestOrderState.Served || State == FirstGuestOrderState.Cancelled)
                return false;
            State = FirstGuestOrderState.Cancelled;
            BurnerIndex = -1;
            return true;
        }
    }
}
