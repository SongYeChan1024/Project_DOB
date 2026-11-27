// =====================================================================
// PriceJudge — 접시 맛 점수와 서빙 판매가 (순수 C#, 상태 없음)
// 호출자 : FirstGuestKitchen(서빙 대금, 요리 화면의 예상 대금)
// 의사코드:
//   FlavorScore(기준, 손님 입맛, 접시 맛) — 기본 맛 점수에서 시작해 접시에 든 입맛 태그 하나당 +1(최대 보너스까지).
//   Payment(기준, 기본가, 맛 점수, 넣은 양념 수) — 내림(기본가 × (기본 배율 + 맛 배율 × 맛 점수)) + 양념 수.
// 방문 호감의 입맛 점수(TasteJudge, −2~+2)와 다른 값이다 (기획 4-6). 💡 내림은 송예찬 확인 전.
// 💡 싫어하는 맛이 정의되기 전까지 맛 점수는 기본~기본+보너스만 나온다.
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>금화에 쓰는 맛 점수와 판매가를 낸다.</summary>
    public static class PriceJudge
    {
        /// <summary>기본 맛 점수 + 접시에 든 손님 입맛 수(최대 보너스까지).</summary>
        public static int FlavorScore(MarketRules rules, IEnumerable<string> guestTastes, ICollection<string> dishFlavors)
        {
            if (!rules) throw new ArgumentNullException(nameof(rules));
            int matched = 0;
            if (guestTastes != null && dishFlavors != null)
                foreach (string taste in guestTastes)
                    if (dishFlavors.Contains(taste)) matched++;
            return rules.BaseTasteScore + Math.Min(rules.MaxTasteBonus, matched);
        }

        /// <summary>서빙 대금(금화). 배율을 곱한 값은 내림하고 넣은 양념 수를 더한다.</summary>
        public static int Payment(MarketRules rules, int basePrice, int tasteScore, int spiceCount)
        {
            if (!rules) throw new ArgumentNullException(nameof(rules));
            if (basePrice < 0) throw new ArgumentOutOfRangeException(nameof(basePrice));
            if (spiceCount < 0) throw new ArgumentOutOfRangeException(nameof(spiceCount));
            double rate = rules.PriceBaseRate + rules.PriceTasteRate * (double)tasteScore;
            // 0.6 + 0.2 × 2 같은 값이 0.99999…로 계산돼 1금화 깎이지 않도록 작은 여유를 둔다.
            return Math.Max(0, (int)Math.Floor(basePrice * rate + 1e-6)) + spiceCount;
        }
    }
}
