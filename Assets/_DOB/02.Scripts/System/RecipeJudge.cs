// RecipeJudge — 넣은 재료 조합으로 레시피를 정하고, 레시피와 양념의 효과로 위험 축별 보호 칸을 계산한다 (kitchen-recipes 명세).
// 호출자: OrderBook(확정 전 검사), FirstGuestKitchen(요리 화면 미리보기·서빙 판정). 미리보기와 확정·서빙이 같은 함수를 쓴다.
// 의사코드:
//   Identify — 넣은 재료가 비었거나 개수가 0 이하면 맞지 않음. 레시피 목록에서 재료 종류·개수가 정확히 같은 줄을 찾는다
//              (양념 id가 섞여 오면 어떤 레시피와도 같지 않아 맞지 않음). 찾은 레시피가 잠겼으면 잠김,
//              도구를 보냈는데 레시피 도구와 다르면 도구 틀림, 아니면 맞음.
//   Protection — 레시피 효과 중 그 축을 막는 칸 수의 합 + 양념 효과가 그 축이면 1.
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>재료 조합 판정 결과.</summary>
    public enum RecipeMatch { Matched, WrongIngredients, Locked, WrongTool }

    public static class RecipeJudge
    {
        /// <summary>넣은 재료와 정확히 같은 레시피. tool이 null이면 도구를 검사하지 않는다(빵 전용 옛 요청).</summary>
        public static RecipeMatch Identify(KitchenRules rules, Func<string, bool> isUnlocked, IReadOnlyDictionary<string, int> ingredients,
            string tool, out RecipeEntry recipe)
        {
            recipe = default;
            if (rules == null || ingredients == null || ingredients.Count == 0)
                return RecipeMatch.WrongIngredients;
            foreach (var pair in ingredients)
                if (pair.Value <= 0)
                    return RecipeMatch.WrongIngredients;
            foreach (var candidate in rules.Recipes)
            {
                if (!SameIngredients(candidate, ingredients))
                    continue;
                recipe = candidate;
                if (isUnlocked != null && !isUnlocked(candidate.id))
                    return RecipeMatch.Locked;
                return tool != null && tool != candidate.tool ? RecipeMatch.WrongTool : RecipeMatch.Matched;
            }
            return RecipeMatch.WrongIngredients;
        }

        /// <summary>레시피 효과 + 양념 효과 1칸이 그 축을 막는 칸 수. spiceId가 null이면 양념 없음.</summary>
        public static int Protection(RecipeEntry recipe, MarketRules market, string spiceId, string axis)
        {
            int total = 0;
            if (recipe.effects != null)
                foreach (var effect in recipe.effects)
                    if (RiskAxes.AxisOfEffect(effect.effect) == axis)
                        total += effect.amount;
            if (spiceId != null && market && market.TryGet(spiceId, out var spice) && RiskAxes.AxisOfEffect(spice.effect) == axis)
                total += 1;
            return total;
        }

        private static bool SameIngredients(RecipeEntry recipe, IReadOnlyDictionary<string, int> ingredients)
        {
            if (recipe.ingredients == null || recipe.ingredients.Length != ingredients.Count)
                return false;
            foreach (var item in recipe.ingredients)
                if (!ingredients.TryGetValue(item.id, out int count) || count != item.count)
                    return false;
            return true;
        }
    }
}
