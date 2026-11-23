// 첫 손님의 방문 ID에 주문을 등록 → 재료와 재고를 검사한 뒤 확정과 동시에 마늘을 차감한다.
// 조리 시간이 끝나면 접시 완성 → 같은 방문의 접시를 픽업 → 지정 손님 서빙 시 한 번만 계산한다.
using System;

namespace DOB.Systems
{
    public enum FirstGuestOrderState { Waiting, Cooking, Ready, Carrying, Served }

    /// <summary>하루 한 방문의 구운 빵 주문. 재료·가격·시간은 시작할 때 기획 데이터에서 받는다.</summary>
    public sealed class FirstGuestOrder
    {
        private readonly float cookSeconds;
        private readonly int flourRequired;
        private readonly int garlicRequired;
        private readonly int breadBasePrice;

        public string VisitId { get; }
        public FirstGuestOrderState State { get; private set; } = FirstGuestOrderState.Waiting;
        public int GarlicStock { get; private set; }
        public float RemainingSeconds { get; private set; }

        public FirstGuestOrder(string visitId, float cookSeconds, int startGarlic, int flourRequired,
                               int garlicRequired, int breadBasePrice)
        {
            if (string.IsNullOrWhiteSpace(visitId)) throw new ArgumentException("방문 ID가 필요합니다.", nameof(visitId));
            if (!(cookSeconds > 0f) || float.IsInfinity(cookSeconds)) throw new ArgumentOutOfRangeException(nameof(cookSeconds));
            if (startGarlic < 0) throw new ArgumentOutOfRangeException(nameof(startGarlic));
            if (flourRequired <= 0) throw new ArgumentOutOfRangeException(nameof(flourRequired));
            if (garlicRequired < 0) throw new ArgumentOutOfRangeException(nameof(garlicRequired));
            if (breadBasePrice < 0 || breadBasePrice > int.MaxValue - garlicRequired)
                throw new ArgumentOutOfRangeException(nameof(breadBasePrice));

            VisitId = visitId;
            this.cookSeconds = cookSeconds;
            this.flourRequired = flourRequired;
            this.garlicRequired = garlicRequired;
            this.breadBasePrice = breadBasePrice;
            GarlicStock = startGarlic;
        }

        /// <summary>정해진 튜토리얼 조합을 확정한다. 중복·잘못된 재료·부족한 재고는 그대로 둔다.</summary>
        public bool TryStart(string visitId, int flour, int garlic)
        {
            if (State != FirstGuestOrderState.Waiting || visitId != VisitId ||
                flour != flourRequired || garlic != garlicRequired || GarlicStock < garlic)
                return false;

            GarlicStock -= garlic;
            RemainingSeconds = cookSeconds;
            State = FirstGuestOrderState.Cooking;
            return true;
        }

        /// <summary>게임 쪽 조리 시계가 허용한 경과 시간만 받는다. 응대 중에도 조리는 계속된다.</summary>
        public void Advance(float cookingDeltaSeconds)
        {
            if (State != FirstGuestOrderState.Cooking || !(cookingDeltaSeconds > 0f) || float.IsInfinity(cookingDeltaSeconds))
                return;

            RemainingSeconds = Math.Max(0f, RemainingSeconds - cookingDeltaSeconds);
            if (RemainingSeconds == 0f)
                State = FirstGuestOrderState.Ready;
        }

        public bool TryPickUp(string visitId)
        {
            if (State != FirstGuestOrderState.Ready || visitId != VisitId)
                return false;
            State = FirstGuestOrderState.Carrying;
            return true;
        }

        /// <summary>하루의 고정 맛 점수 2는 가격 배율 1이다. 기본가에 투입한 양념 개수를 더한다.</summary>
        public bool TryServe(string visitId, out int payment)
        {
            payment = 0;
            if (State != FirstGuestOrderState.Carrying || visitId != VisitId)
                return false;
            State = FirstGuestOrderState.Served;
            payment = breadBasePrice + garlicRequired;
            return true;
        }
    }
}
