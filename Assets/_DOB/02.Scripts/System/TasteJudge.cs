// =====================================================================
// TasteJudge — 방문 호감의 입맛 점수 계산기 (순수 C#, 상태 없음)
// 호출자 : GameManager(서빙 알림 때 그 방문의 입맛 점수를 정해 명단에 기록)
// 의사코드:
//   DishFlavors(기준, 레시피, 양념들) — 레시피의 맛과 넣은 양념의 맛을 합친 집합.
//   Score(손님 입맛, 접시 맛) — 손님 입맛 태그마다 접시 맛에 있으면 +1, 없으면 −1을 더한다. 태그 두 개면 −2·0·+2.
// 「맞으면」을 「그 맛이 접시에 들어 있다」로 읽는다 (기획 4-8). 💡 송예찬 확인 전.
// 금화에 쓰는 맛 점수 0~4와 다른 값이다 (기획 4-6).
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>손님 입맛과 접시 맛으로 방문의 입맛 점수를 낸다.</summary>
    public static class TasteJudge
    {
        /// <summary>레시피와 넣은 양념의 맛을 합친 집합.</summary>
        public static HashSet<string> DishFlavors(TasteRules rules, string recipe, IEnumerable<string> seasonings)
        {
            if (!rules) throw new ArgumentNullException(nameof(rules));
            var flavors = new HashSet<string>(rules.FlavorsOf(recipe));
            if (seasonings != null)
                foreach (string seasoning in seasonings)
                    flavors.UnionWith(rules.FlavorsOf(seasoning));
            return flavors;
        }

        /// <summary>입맛 태그마다 접시에 있으면 +1, 없으면 −1을 더한 값.</summary>
        public static int Score(IEnumerable<string> guestTastes, ICollection<string> dishFlavors)
        {
            if (guestTastes == null) throw new ArgumentNullException(nameof(guestTastes));
            if (dishFlavors == null) throw new ArgumentNullException(nameof(dishFlavors));
            int score = 0;
            foreach (string taste in guestTastes)
                score += dishFlavors.Contains(taste) ? 1 : -1;
            return score;
        }
    }
}
