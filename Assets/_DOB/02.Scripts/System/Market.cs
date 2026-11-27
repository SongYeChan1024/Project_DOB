// =====================================================================
// Market — 시장 장바구니 구매의 검증과 적용 (순수 C#, 상태 없음)
// 호출자 : EconomyManager.Buy (준비 단계인지는 EconomyManager가 먼저 본다)
// 호출   : MarketRules(가격·판매 여부), Wallet.TrySpend, MaterialStock.TryAdd(양념 재고)
// 의사코드:
//   TryBuy(기준, 지갑, 양념 재고, 장바구니)
//     1. 검증 — 장바구니가 비었으면 잘못된 개수. 줄마다 순서대로: 개수 0 이하 → 잘못된 개수,
//        팔지 않는 품목 → 팔지 않는 품목, 양념 표에 없는 id → 모르는 품목. 첫 거절 사유를 돌려준다.
//     2. 합계 = Σ 가격 × 개수. 지갑보다 많으면 금화 부족.
//     3. 통과했을 때만 지갑에서 합계를 빼고 양념을 더한다. 거절 경로는 상태를 건드리지 않는다.
//   결과에는 처리 뒤 금화와 양념 재고가 함께 담긴다.
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>시장 구매 요청의 결과 사유.</summary>
    public enum PurchaseResult
    {
        /// <summary>장바구니 전체를 샀다.</summary>
        Purchased,
        /// <summary>합계가 가진 금화보다 많다.</summary>
        NotEnoughGold,
        /// <summary>양념 표에 없는 품목이 있다.</summary>
        UnknownItem,
        /// <summary>기본 재료·희귀 재료처럼 시장이 팔지 않는 품목이 있다.</summary>
        NotForSale,
        /// <summary>개수가 0 이하인 줄이 있거나 장바구니가 비었다.</summary>
        InvalidCount,
        /// <summary>준비 단계가 아니라 시장이 닫혀 있다.</summary>
        MarketClosed
    }

    /// <summary>구매 결과와 처리 뒤 금화·양념 재고.</summary>
    public readonly struct PurchaseOutcome
    {
        public readonly PurchaseResult Result;
        public readonly int Gold;
        public readonly IReadOnlyList<MaterialCount> Spices;

        public PurchaseOutcome(PurchaseResult result, int gold, IReadOnlyList<MaterialCount> spices)
        {
            Result = result;
            Gold = gold;
            Spices = spices;
        }
    }

    /// <summary>장바구니를 모두 검사한 뒤 전부 사거나 아무것도 사지 않는다.</summary>
    public static class Market
    {
        public static PurchaseOutcome TryBuy(MarketRules rules, Wallet wallet, MaterialStock spices, IReadOnlyDictionary<string, int> cart)
        {
            if (!rules) throw new ArgumentNullException(nameof(rules));
            if (wallet == null) throw new ArgumentNullException(nameof(wallet));
            if (spices == null) throw new ArgumentNullException(nameof(spices));

            var result = Check(rules, cart, out int total);
            if (result == PurchaseResult.Purchased && total > wallet.Gold)
                result = PurchaseResult.NotEnoughGold;
            if (result == PurchaseResult.Purchased)
            {
                wallet.TrySpend(total);
                foreach (var line in cart)
                    spices.TryAdd(line.Key, line.Value);
            }
            return new PurchaseOutcome(result, wallet.Gold, spices.All());
        }

        private static PurchaseResult Check(MarketRules rules, IReadOnlyDictionary<string, int> cart, out int total)
        {
            total = 0;
            if (cart == null || cart.Count == 0)
                return PurchaseResult.InvalidCount;
            foreach (var line in cart)
            {
                if (line.Value <= 0)
                    return PurchaseResult.InvalidCount;
                if (rules.IsNotForSale(line.Key))
                    return PurchaseResult.NotForSale;
                if (!rules.TryGet(line.Key, out var spice))
                    return PurchaseResult.UnknownItem;
                total = checked(total + spice.price * line.Value);
            }
            return PurchaseResult.Purchased;
        }
    }
}
