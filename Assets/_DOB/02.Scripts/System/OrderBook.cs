// OrderBook — 하루 영업의 주문 장부. 방문별 주문, 조리 칸 하나, 들고 있는 접시 하나를 소유한다.
// 호출자: FirstGuestKitchen(착석 시 주문 열기, 레시피 확정, 픽업·서빙, 떠난 방문 취소), 조리 Δ는 GameManager가 넘긴다.
// 상태  : 연 순서의 주문 목록. 양념 재고는 경제(EconomyManager.Spices)가 소유하고 여기서는 소비만 요청한다.
//         대금은 맛 점수가 필요해 주방이 PriceJudge로 정한다.
// 의사코드:
//   TryOpen — 빈 ID·이미 주문이 있는 방문이면 거절하고, 아니면 대기 주문을 하나 추가한다.
//   TryStart — 방문을 지정하지 않으면 가장 먼저 연 대기 주문을 고른다. 조리 중인 주문이 있거나 양념이 재고보다 많으면
//              거절하고 재고를 그대로 둔다. 성공하면 경제의 양념 재고에서 한 번 소비한다.
//   Advance — 조리 중인 주문에만 조리 시계 Δ를 넘긴다.
//   TryPickUp — 접시를 들고 있지 않을 때 완성된 주문 하나를 든다(지정하지 않으면 먼저 완성된 것).
//   TryServe — 들고 있는 접시의 방문 ID와 같을 때만 한 번 서빙된다. 다른 방문이면 거절하고 접시를 계속 든다.
//   Cancel — 서빙 전 주문을 취소한다. 조리 칸·들고 있던 접시는 그 주문의 상태로 정해지므로 함께 빈다. 양념은 돌려주지 않는다.
using System;
using System.Collections.Generic;

namespace DOB.Systems
{
    /// <summary>방문 ID별 주문. 일차마다 새로 만든다.</summary>
    public sealed class OrderBook
    {
        private readonly List<FirstGuestOrder> orders = new List<FirstGuestOrder>();
        private readonly float cookSeconds;
        private readonly int flourRequired;
        private readonly MaterialStock spices;
        private readonly string garlicId;

        /// <param name="spices">경제가 소유한 양념 재고.</param>
        /// <param name="garlicId">이 레시피가 넣는 양념의 id.</param>
        public OrderBook(MaterialStock spices, string garlicId, float cookSeconds, int flourRequired)
        {
            this.spices = spices ?? throw new ArgumentNullException(nameof(spices));
            if (string.IsNullOrEmpty(garlicId)) throw new ArgumentException("양념 id가 필요합니다.", nameof(garlicId));
            this.garlicId = garlicId;
            if (!(cookSeconds > 0f) || float.IsInfinity(cookSeconds)) throw new ArgumentOutOfRangeException(nameof(cookSeconds));
            if (flourRequired <= 0) throw new ArgumentOutOfRangeException(nameof(flourRequired));
            this.cookSeconds = cookSeconds;
            this.flourRequired = flourRequired;
        }

        /// <summary>주문 사이에서 공유하는 마늘 재고(경제의 양념 재고에서 읽는다).</summary>
        public int GarlicStock => spices.Count(garlicId);
        /// <summary>연 순서의 주문 목록(취소·서빙된 주문 포함).</summary>
        public IReadOnlyList<FirstGuestOrder> Orders => orders;
        /// <summary>그 방문의 주문. 없으면 null.</summary>
        public FirstGuestOrder Find(string visitId) => orders.Find(o => o.VisitId == visitId);
        /// <summary>아직 조리하지 않은 가장 먼저 연 주문.</summary>
        public FirstGuestOrder NextWaiting => First(FirstGuestOrderState.Waiting);
        // shortcut: 조리 칸이 하나라 조리 중인 주문도 하나다. 두 화구 병행(F3)이 들어오면 칸 배열로 바꾼다.
        /// <summary>조리 중인 주문. 없으면 null.</summary>
        public FirstGuestOrder Cooking => First(FirstGuestOrderState.Cooking);
        /// <summary>먼저 완성된 픽업 대기 주문.</summary>
        public FirstGuestOrder NextReady => First(FirstGuestOrderState.Ready);
        /// <summary>플레이어가 들고 있는 접시의 주문. 없으면 null.</summary>
        public FirstGuestOrder Carried => First(FirstGuestOrderState.Carrying);

        /// <summary>착석한 방문의 주문을 하나 연다. 빈 ID·중복은 거절한다.</summary>
        public bool TryOpen(string visitId)
        {
            if (string.IsNullOrWhiteSpace(visitId) || Find(visitId) != null)
                return false;
            orders.Add(new FirstGuestOrder(visitId, cookSeconds, flourRequired));
            return true;
        }

        /// <summary>조리를 시작한다. visitId가 null이면 먼저 연 대기 주문. 조리 중·재고 부족·잘못된 재료는 거절한다.</summary>
        public bool TryStart(string visitId, int flour, int garlic)
        {
            var order = visitId == null ? NextWaiting : Find(visitId);
            if (order == null || Cooking != null || garlic > GarlicStock || !order.TryStart(flour, garlic))
                return false;
            spices.TryConsume(garlicId, garlic);
            return true;
        }

        /// <summary>조리 시계가 허용한 경과 시간을 조리 중인 주문에 넘긴다.</summary>
        public void Advance(float cookingDeltaSeconds) => Cooking?.Advance(cookingDeltaSeconds);

        /// <summary>완성된 접시를 든다. visitId가 null이면 먼저 완성된 것. 이미 들고 있으면 거절한다.</summary>
        public bool TryPickUp(string visitId = null)
        {
            var order = visitId == null ? NextReady : Find(visitId);
            return order != null && Carried == null && order.TryPickUp();
        }

        /// <summary>들고 있는 접시를 그 방문에 서빙한다. 다른 방문이면 거절한다. 대금은 주방이 정한다.</summary>
        public bool TryServe(string visitId)
        {
            var carried = Carried;
            return carried != null && carried.TryServe(visitId);
        }

        /// <summary>저장 복원용. 주문이 하나도 없을 때만 저장된 주문들로 채운다. 이미 주문이 있거나 같은 방문·조리 중 주문이 겹치면 false.
        /// 양념 재고는 경제가 따로 되살린다.</summary>
        public bool Restore(IEnumerable<OrderProgress> saved)
        {
            if (orders.Count > 0)
                return false;
            var restored = new List<FirstGuestOrder>();
            var seen = new HashSet<string>();
            if (saved != null)
                foreach (var entry in saved)
                {
                    if (entry == null || !seen.Add(entry.VisitId))
                        return false;
                    // 💡 들고 있던 접시는 저장하지 않는다. 불러오면 픽업대에 다시 놓인다.
                    var state = entry.State == FirstGuestOrderState.Carrying ? FirstGuestOrderState.Ready : entry.State;
                    // 조리 칸이 하나라 조리 중인 주문도 하나여야 한다.
                    if (state == FirstGuestOrderState.Cooking && restored.Exists(o => o.State == FirstGuestOrderState.Cooking))
                        return false;
                    var order = new FirstGuestOrder(entry.VisitId, cookSeconds, flourRequired);
                    order.Restore(state, entry.RemainingSeconds, entry.UsedGarlic);
                    restored.Add(order);
                }
            orders.AddRange(restored);
            return true;
        }

        /// <summary>떠난 방문의 서빙 전 주문을 취소한다. 주문이 없거나 이미 서빙했으면 false.</summary>
        public bool Cancel(string visitId) => Find(visitId)?.TryCancel() == true;

        private FirstGuestOrder First(FirstGuestOrderState state) => orders.Find(o => o.State == state);
    }
}
