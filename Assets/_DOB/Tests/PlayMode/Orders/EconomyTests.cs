// EconomyTests — 지갑·양념 재고·시장 구매·맛 점수·판매가와 기준 에셋을 검증한다 (shop-economy 명세).
// 의사코드: 명세 시나리오의 값(금화 10에서 소금 2개·당근즙 1개 등)을 그대로 쓰고, 거절이면 금화·재고가 그대로인지 함께 본다.
using System.Collections.Generic;
using System.Linq;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;
using UnityEngine;

namespace DOB.Tests
{
    public sealed class EconomyTests
    {
        private static MarketRules Rules() => MarketRules.Load();

        private static (Wallet wallet, MaterialStock spices) Shop(int gold) =>
            (new Wallet(gold), new MaterialStock(new[] { new MaterialCount("garlic", 1) }));

        private static (int, int, int, int) Stock(MaterialStock spices) =>
            (spices.Count("garlic"), spices.Count("salt"), spices.Count("carrot"), spices.All().Count);

        [Test]
        public void TheMarketAndFloorAssetsAreValid()
        {
            Assert.That(Rules(), Is.Not.Null);
            Assert.That(Rules().Validate(), Is.Null);
            Assert.That(FloorProgressRules.Load(), Is.Not.Null);
            Assert.That(FloorProgressRules.Load().Validate(), Is.Null);
            Assert.That(Rules().TryGet("garlic", out var garlic) && garlic.name == "마늘" && garlic.startStock == 2, Is.True);
        }

        [Test]
        public void TheWalletRejectsOverspendingAndNegativeAmounts()
        {
            var wallet = new Wallet(5);
            Assert.That(wallet.TrySpend(6), Is.False);
            Assert.That(wallet.TrySpend(-1), Is.False);
            Assert.That(wallet.Earn(-1), Is.False);
            Assert.That(wallet.Gold, Is.EqualTo(5));
            Assert.That(wallet.TrySpend(5) && wallet.Gold == 0, Is.True);
            Assert.That(wallet.Earn(9) && wallet.Gold == 9, Is.True);
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new Wallet(-1));
        }

        [Test]
        public void ConsumingASpiceTakesOnlyWhatIsThere()
        {
            var stock = new MaterialStock(new[] { new MaterialCount("garlic", 2) });
            Assert.That(stock.TryConsume("garlic", 1) && stock.Count("garlic") == 1, Is.True, "마늘 2개에서 1개를 쓰면 1");
            Assert.That(stock.TryConsume("garlic", 2), Is.False, "부족");
            Assert.That(stock.TryConsume("garlic", -1), Is.False, "음수");
            Assert.That(stock.TryConsume("salt", 0), Is.True, "0개는 바꾸지 않고 통과");
            Assert.That(stock.Count("garlic"), Is.EqualTo(1));
        }

        [Test]
        public void ABasketIsBoughtWhole()
        {
            var (wallet, spices) = Shop(10);
            var outcome = Market.TryBuy(Rules(), wallet, spices, new Dictionary<string, int> { ["salt"] = 2, ["carrot"] = 1 });
            Assert.That(outcome.Result, Is.EqualTo(PurchaseResult.Purchased));
            Assert.That((outcome.Gold, wallet.Gold), Is.EqualTo((3, 3)));
            Assert.That(Stock(spices), Is.EqualTo((1, 2, 1, 3)));
            Assert.That(outcome.Spices.Single(x => x.Id == "salt").Count, Is.EqualTo(2), "결과에 처리 뒤 재고가 담긴다");
        }

        [TestCase(5, "salt", 2, "carrot", 1, PurchaseResult.NotEnoughGold, TestName = "금화 부족")]
        [TestCase(10, "garlic", 1, "truffle", 1, PurchaseResult.UnknownItem, TestName = "하나라도 모르는 품목")]
        [TestCase(10, "salt", 0, "garlic", 1, PurchaseResult.InvalidCount, TestName = "0개")]
        [TestCase(10, "salt", -2, "garlic", 1, PurchaseResult.InvalidCount, TestName = "음수 개수")]
        [TestCase(99, "숲 송로", 1, "garlic", 1, PurchaseResult.NotForSale, TestName = "희귀 재료")]
        [TestCase(99, "밀가루", 1, "garlic", 1, PurchaseResult.NotForSale, TestName = "기본 재료")]
        public void ARejectedBasketChangesNothing(int gold, string a, int countA, string b, int countB, PurchaseResult expected)
        {
            var (wallet, spices) = Shop(gold);
            var before = Stock(spices);
            var outcome = Market.TryBuy(Rules(), wallet, spices, new Dictionary<string, int> { [a] = countA, [b] = countB });
            Assert.That(outcome.Result, Is.EqualTo(expected));
            Assert.That(wallet.Gold, Is.EqualTo(gold));
            Assert.That(Stock(spices), Is.EqualTo(before));
        }

        [Test]
        public void FlavorScoreCountsTheGuestTastesInTheDish()
        {
            var rules = Resources.Load<TasteRules>(TasteRules.ResourceName);
            var garlicBread = TasteJudge.DishFlavors(rules, "구운 빵", new[] { "마늘" });
            Assert.That(PriceJudge.FlavorScore(Rules(), new[] { "고소", "향긋" }, garlicBread), Is.EqualTo(4));
            Assert.That(PriceJudge.FlavorScore(Rules(), new[] { "진한", "달콤" }, TasteJudge.DishFlavors(rules, "구운 빵", null)), Is.EqualTo(2));
            Assert.That(PriceJudge.FlavorScore(Rules(), new string[0], garlicBread), Is.EqualTo(2), "입맛이 없으면 기본 점수");
        }

        [TestCase(8, 2, 1, ExpectedResult = 9, TestName = "맛 2 마늘 빵 9금")]
        [TestCase(8, 4, 0, ExpectedResult = 11, TestName = "맛 4 양념 없음 11금(11.2 내림)")]
        [TestCase(8, 3, 0, ExpectedResult = 9, TestName = "맛 3 9금(9.6 내림)")]
        public int PaymentFollowsBasePriceTasteAndSpices(int basePrice, int taste, int spices) =>
            PriceJudge.Payment(Rules(), basePrice, taste, spices);
    }
}
