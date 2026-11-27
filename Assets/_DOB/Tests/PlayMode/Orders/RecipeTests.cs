// RecipeTests — 재료 조합 → 레시피 판정(정확 일치·잠김·도구), 레시피 보호 칸, 해금 장부를 검증한다 (kitchen-recipes 명세).
// 의사코드: 실제 주방 기준(SO_Kitchen)과 시장 기준(SO_Market)으로 판정하고, 해금은 던전 재료 재고로 연다.
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;

namespace DOB.Tests
{
    public sealed class RecipeTests
    {
        private static KitchenRules Rules => KitchenRules.Load();
        private static Dictionary<string, int> Items(params (string id, int count)[] items)
        {
            var map = new Dictionary<string, int>();
            foreach (var (id, count) in items) map[id] = count;
            return map;
        }

        private static RecipeMatch Identify(Dictionary<string, int> items, string tool, out RecipeEntry recipe, params string[] unlocked) =>
            RecipeJudge.Identify(Rules, id => System.Array.IndexOf(unlocked, id) >= 0, items, tool, out recipe);

        [Test]
        public void OnlyAnExactIngredientMatchNamesARecipe()
        {
            Assert.That(Identify(Items(("밀가루", 2)), "오븐", out var bread, "Bread"), Is.EqualTo(RecipeMatch.Matched));
            Assert.That(bread.id, Is.EqualTo("Bread"));
            Assert.That(Identify(Items(("밀가루", 3)), "오븐", out _, "Bread"), Is.EqualTo(RecipeMatch.WrongIngredients), "개수가 다름");
            Assert.That(Identify(Items(("밀가루", 2), ("감자", 1)), "오븐", out _, "Bread"), Is.EqualTo(RecipeMatch.WrongIngredients), "재료가 남음");
            Assert.That(Identify(Items(), "오븐", out _, "Bread"), Is.EqualTo(RecipeMatch.WrongIngredients));
        }

        [Test]
        public void ALockedRecipeIsRejectedBeforeTheTool()
        {
            var soup = Items(("숲버섯", 2), ("감자", 1));
            Assert.That(Identify(soup, "프라이팬", out _, "Bread"), Is.EqualTo(RecipeMatch.Locked));
            Assert.That(Identify(soup, "프라이팬", out _, "MushroomSoup"), Is.EqualTo(RecipeMatch.WrongTool));
            Assert.That(Identify(soup, "냄비", out var recipe, "MushroomSoup"), Is.EqualTo(RecipeMatch.Matched));
            Assert.That(recipe.name, Is.EqualTo("숲버섯 수프"));
            Assert.That(Identify(soup, null, out _, "MushroomSoup"), Is.EqualTo(RecipeMatch.Matched), "도구를 고르지 않는 옛 화면은 도구를 보지 않는다");
        }

        [Test]
        public void ProtectionIsTheRecipeEffectsPlusOneForAMatchingSpice()
        {
            Rules.TryGet("TruffleSoup", out var truffle);
            var market = MarketRules.Load();
            string poison = RiskAxes.AxisOfEffect("해독");
            Assert.That(RecipeJudge.Protection(truffle, market, null, poison), Is.EqualTo(3));
            Assert.That(RecipeJudge.Protection(truffle, market, null, RiskAxes.AxisOfEffect("활력")), Is.EqualTo(1));
            Assert.That(RecipeJudge.Protection(truffle, market, null, RiskAxes.AxisOfEffect("방한")), Is.Zero);
            Assert.That(market.TryGet("garlic", out var garlic), Is.True);
            string garlicAxis = RiskAxes.AxisOfEffect(garlic.effect);
            Assert.That(RecipeJudge.Protection(truffle, market, "garlic", garlicAxis),
                Is.EqualTo(RecipeJudge.Protection(truffle, market, null, garlicAxis) + 1));
        }

        [Test]
        public void TheBookStartsWithStarterRecipesAndOnlyGrows()
        {
            var book = new RecipeBook(Rules);
            Assert.That(book.Unlocked, Is.EqualTo(new[] { "Bread", "PotatoStew", "GrilledMeat" }));
            var stock = new MaterialStock(new[] { new MaterialCount("숲버섯", 1), new MaterialCount("들풀", 1) });
            Assert.That(book.RefreshFrom(stock), Is.EquivalentTo(new[] { "HerbPie", "MushroomSoup", "SlimeSalad" }), "여는 재료 중 하나만 있어도 열린다");
            Assert.That(book.RefreshFrom(stock), Is.Empty, "이미 열린 레시피는 다시 알리지 않는다");
            Assert.That(book.RefreshFrom(new MaterialStock()), Is.Empty);
            Assert.That(book.IsUnlocked("MushroomSoup"), Is.True, "재료를 다 써도 닫히지 않는다");
            Assert.That(book.IsUnlocked("TruffleSoup"), Is.False);

            var restored = new RecipeBook(Rules, new[] { "TruffleSoup", "Pizza" });
            Assert.That(restored.IsUnlocked("TruffleSoup"), Is.True);
            Assert.That(restored.IsUnlocked("Pizza"), Is.False, "모르는 저장 id는 버린다");
            Assert.That(restored.IsUnlocked("Bread"), Is.True);
        }
    }
}
