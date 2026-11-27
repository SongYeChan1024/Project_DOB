// OrderBook — 하루 영업의 주문 장부. 방문별 주문, 화구 N칸(칸 수는 KitchenManager가 소유), 들고 있는 접시 하나를 소유한다.
// 호출자: FirstGuestKitchen(착석 시 주문 열기, 레시피 확정, 픽업·서빙, 떠난 방문 취소), 조리 Δ는 GameManager가 넘긴다.
// 상태  : 연 순서의 주문 목록. 주문이 화구 번호를 가져 조리 중·완성 주문이 칸을 차지한다.
//         던전 재료·양념 재고는 경제(EconomyManager)가 소유하고 여기서는 소비만 요청한다. 대금은 주방이 PriceJudge로 정한다.
// 의사코드:
//   TryOpen — 빈 ID·이미 주문이 있는 방문이면 거절하고, 아니면 대기 주문을 하나 추가한다.
//   TryStart(레시피, 양념) — 방문을 지정하지 않으면 가장 먼저 연 대기 주문. 대기 주문이 아니면 주문 없음, 빈 화구가 없으면
//              화구 사용 중, 기본 재료가 아닌 재료나 양념이 하나라도 모자라면 재고 부족으로 거절하고 아무것도 빼지 않는다.
//              모두 있으면 차례로 한 번씩 빼고 번호가 가장 낮은 빈 화구에 넣는다.
//   Advance — 조리 중인 모든 주문에 조리 시계 Δ를 넘기고, 이번에 완성된 주문을 Finished로 한 번 알린다(복원은 알리지 않는다).
//   TryPickUp — 접시를 들고 있지 않을 때 완성된 주문 하나를 든다(지정하지 않으면 먼저 연 완성 주문). 그 화구가 빈다.
//   TryServe — 들고 있는 접시의 방문 ID와 같을 때만 한 번 서빙된다. 다른 방문이면 거절하고 접시를 계속 든다.
//   Cancel — 서빙 전 주문을 취소한다. 그 주문의 화구·들고 있던 접시가 함께 빈다. 재료는 돌려주지 않는다.
//   Restore — 저장된 화구 번호가 비어 있으면 그 칸에, 아니면 가장 낮은 빈 칸에 둔다. 조리 중인데 빈 칸이 없으면 실패,
//             완성 접시는 칸이 없으면 화구 밖(-1) 완성으로 둔다(이전 스키마의 한 칸 장부에서 여러 접시가 완성돼 있던 경우).
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>조리 시작 결과.</summary>
    public enum OrderStartResult { Started, NoOrder, BurnerBusy, NotEnoughStock }

    /// <summary>방문 ID별 주문. 일차마다 새로 만든다.</summary>
    public sealed class OrderBook
    {
        private readonly List<FirstGuestOrder> orders = new List<FirstGuestOrder>();
        private readonly KitchenRules rules;
        private readonly MaterialStock materials;
        private readonly MaterialStock spices;
        private readonly Func<int> burnerCount;

        /// <summary>조리가 끝나 완성된 주문(Advance에서만, 한 번).</summary>
        public event Action<FirstGuestOrder> Finished;

        /// <param name="materials">경제가 소유한 던전 재료 재고.</param>
        /// <param name="spices">경제가 소유한 양념 재고.</param>
        /// <param name="burnerCount">지금 가진 화구 수. null이면 시작 화구 수. 구매로 늘 수 있어 매번 읽는다.</param>
        public OrderBook(KitchenRules rules, MaterialStock materials, MaterialStock spices, Func<int> burnerCount = null)
        {
            this.burnerCount = burnerCount;
            this.rules = rules ? rules : throw new ArgumentNullException(nameof(rules));
            this.materials = materials ?? throw new ArgumentNullException(nameof(materials));
            this.spices = spices ?? throw new ArgumentNullException(nameof(spices));
        }

        public int BurnerCount => Math.Max(1, burnerCount?.Invoke() ?? rules.StartBurners);
        /// <summary>연 순서의 주문 목록(취소·서빙된 주문 포함).</summary>
        public IReadOnlyList<FirstGuestOrder> Orders => orders;
        /// <summary>그 방문의 주문. 없으면 null.</summary>
        public FirstGuestOrder Find(string visitId) => orders.Find(o => o.VisitId == visitId);
        /// <summary>아직 조리하지 않은 가장 먼저 연 주문.</summary>
        public FirstGuestOrder NextWaiting => First(FirstGuestOrderState.Waiting);
        /// <summary>먼저 연 조리 중 주문. 없으면 null.</summary>
        public FirstGuestOrder Cooking => First(FirstGuestOrderState.Cooking);
        /// <summary>먼저 연 픽업 대기 주문.</summary>
        public FirstGuestOrder NextReady => First(FirstGuestOrderState.Ready);
        /// <summary>플레이어가 들고 있는 접시의 주문. 없으면 null.</summary>
        public FirstGuestOrder Carried => First(FirstGuestOrderState.Carrying);
        /// <summary>그 화구에 놓인 조리 중·완성 주문. 비었으면 null.</summary>
        public FirstGuestOrder AtBurner(int index) =>
            index < 0 ? null : orders.Find(o => o.BurnerIndex == index && (o.State == FirstGuestOrderState.Cooking || o.State == FirstGuestOrderState.Ready));
        /// <summary>번호가 가장 낮은 빈 화구. 없으면 -1.</summary>
        public int FreeBurner
        {
            get
            {
                for (int i = 0; i < BurnerCount; i++)
                    if (AtBurner(i) == null) return i;
                return -1;
            }
        }

        /// <summary>착석한 방문의 주문을 하나 연다. 빈 ID·중복은 거절한다.</summary>
        public bool TryOpen(string visitId)
        {
            if (string.IsNullOrWhiteSpace(visitId) || Find(visitId) != null)
                return false;
            orders.Add(new FirstGuestOrder(visitId, rules.CookSeconds));
            return true;
        }

        /// <summary>확정한 레시피·양념으로 조리를 시작한다. visitId가 null이면 먼저 연 대기 주문. 거절이면 재고·주문을 바꾸지 않는다.
        /// 레시피 판정(재료 조합·해금·도구)은 호출하는 쪽이 RecipeJudge로 먼저 한다.</summary>
        public OrderStartResult TryStart(string visitId, RecipeEntry recipe, string spiceId)
        {
            var order = visitId == null ? NextWaiting : Find(visitId);
            if (order == null || order.State != FirstGuestOrderState.Waiting || string.IsNullOrEmpty(recipe.id))
                return OrderStartResult.NoOrder;
            int burner = FreeBurner;
            if (burner < 0)
                return OrderStartResult.BurnerBusy;
            foreach (var item in recipe.ingredients)
                if (!rules.IsBasic(item.id) && materials.Count(item.id) < item.count)
                    return OrderStartResult.NotEnoughStock;
            if (spiceId != null && spices.Count(spiceId) < 1)
                return OrderStartResult.NotEnoughStock;
            foreach (var item in recipe.ingredients)
                if (!rules.IsBasic(item.id))
                    materials.TryConsume(item.id, item.count);
            if (spiceId != null)
                spices.TryConsume(spiceId, 1);
            order.TryStart(recipe.id, spiceId, burner);
            return OrderStartResult.Started;
        }

        /// <summary>조리 시계가 허용한 경과 시간을 조리 중인 모든 주문에 넘긴다.</summary>
        public void Advance(float cookingDeltaSeconds)
        {
            foreach (var order in orders.ToArray())
                if (order.Advance(cookingDeltaSeconds))
                    Finished?.Invoke(order);
        }

        /// <summary>완성된 접시를 든다. visitId가 null이면 먼저 연 완성 주문. 이미 들고 있으면 거절한다.</summary>
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

        /// <summary>저장 복원용. 주문이 하나도 없을 때만 저장된 주문들로 채운다. 이미 주문이 있거나, 방문이 겹치거나,
        /// 모르는 레시피이거나, 조리 중인데 빈 화구가 없으면 false. 재고는 경제가 따로 되살린다.</summary>
        public bool Restore(IEnumerable<OrderProgress> saved)
        {
            if (orders.Count > 0)
                return false;
            var restored = new List<FirstGuestOrder>();
            var seen = new HashSet<string>();
            var taken = new HashSet<int>();
            var pending = new List<(FirstGuestOrder order, OrderProgress entry, FirstGuestOrderState state)>();
            if (saved != null)
                foreach (var entry in saved)
                {
                    if (entry == null || !seen.Add(entry.VisitId))
                        return false;
                    bool started = entry.State != FirstGuestOrderState.Waiting && entry.State != FirstGuestOrderState.Cancelled;
                    if (started && !rules.TryGet(entry.Recipe, out _))
                        return false;
                    // 💡 들고 있던 접시는 저장하지 않는다. 불러오면 완성 접시로 화구에 다시 놓인다.
                    var state = entry.State == FirstGuestOrderState.Carrying ? FirstGuestOrderState.Ready : entry.State;
                    var order = new FirstGuestOrder(entry.VisitId, rules.CookSeconds);
                    restored.Add(order);
                    bool onBurner = state == FirstGuestOrderState.Cooking || state == FirstGuestOrderState.Ready;
                    int burner = entry.Burner ?? -1;
                    if (onBurner && burner >= 0 && burner < BurnerCount && taken.Add(burner))
                        order.Restore(state, entry.RemainingSeconds, entry.Recipe, entry.Spice, burner);
                    else if (onBurner)
                        pending.Add((order, entry, state));
                    else
                        order.Restore(state, entry.RemainingSeconds, started ? entry.Recipe : null, started ? entry.Spice : null, -1);
                }
            // 저장된 칸이 없거나 겹친 주문: 조리 중부터 남은 빈 칸에 둔다.
            pending.Sort((a, b) => (a.state == FirstGuestOrderState.Cooking ? 0 : 1).CompareTo(b.state == FirstGuestOrderState.Cooking ? 0 : 1));
            foreach (var (order, entry, state) in pending)
            {
                int burner = -1;
                for (int i = 0; i < BurnerCount; i++)
                    if (taken.Add(i)) { burner = i; break; }
                if (burner < 0 && state == FirstGuestOrderState.Cooking)
                    return false;
                order.Restore(state, entry.RemainingSeconds, entry.Recipe, entry.Spice, burner);
            }
            orders.AddRange(restored);
            return true;
        }

        /// <summary>떠난 방문의 서빙 전 주문을 취소한다. 주문이 없거나 이미 서빙했으면 false.</summary>
        public bool Cancel(string visitId) => Find(visitId)?.TryCancel() == true;

        private FirstGuestOrder First(FirstGuestOrderState state) => orders.Find(o => o.State == state);
    }
}
