// OrderBookTests — 방문별 주문, 화구 두 칸, 던전 재료·양념 소비, 들고 있는 접시 하나, 떠난 방문의 취소를 검증한다.
// 의사코드: 실제 주방 기준(SO_Kitchen: 조리 6초·시작 1칸·최대 2칸)과 화구 두 칸을 산 장부, 재료·양념 재고로 장부를 열고 kitchen-burners 명세의 시나리오를 따라간다.
//   구운 빵(밀가루 2, 기본 재료)과 숲버섯 수프(숲버섯 2 + 감자 1)를 쓴다. 대금은 PriceJudge가 정한다(EconomyTests).
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;

namespace DOB.Tests
{
    public sealed class OrderBookTests
    {
        private const string A = "visit-a";
        private const string B = "visit-b";
        private const string C = "visit-c";

        private static KitchenRules Rules => KitchenRules.Load();
        private static RecipeEntry Recipe(string id) { Rules.TryGet(id, out var r); return r; }
        private static RecipeEntry Bread => Recipe("Bread");
        private static RecipeEntry Soup => Recipe("MushroomSoup");

        private static OrderBook Book(int garlic = 2, int mushrooms = 0) => Book(out _, out _, garlic, mushrooms);

        private static OrderBook Book(out MaterialStock materials, out MaterialStock spices, int garlic = 2, int mushrooms = 0)
        {
            materials = new MaterialStock(new[] { new MaterialCount("숲버섯", mushrooms) });
            spices = new MaterialStock(new[] { new MaterialCount("garlic", garlic) });
            return new OrderBook(Rules, materials, spices, () => 2);
        }

        private static void CookAndCarry(OrderBook book, string visit, string spice = "garlic")
        {
            Assert.That(book.TryStart(visit, Bread, spice), Is.EqualTo(OrderStartResult.Started), "조리 시작 " + visit);
            book.Advance(6f);
            Assert.That(book.TryPickUp(visit), Is.True, "픽업 " + visit);
        }

        [Test]
        public void TheKitchenAssetIsValidAndStartsWithOneBurner()
        {
            Assert.That(Rules, Is.Not.Null);
            Assert.That(Rules.Validate(UnityEngine.Resources.Load<TasteRules>(TasteRules.ResourceName)), Is.Null);
            Assert.That((Rules.StartBurners, Rules.MaxBurners, Rules.BurnerPrice), Is.EqualTo((1, 2, 60)));
        }

        [Test]
        public void WithTheStartingBurnerASecondCookWaits()
        {
            var book = new OrderBook(Rules, new MaterialStock(), new MaterialStock(new[] { new MaterialCount("garlic", 2) }));
            Assert.That(book.BurnerCount, Is.EqualTo(1));
            book.TryOpen(A); book.TryOpen(B);
            Assert.That(book.TryStart(A, Bread, null), Is.EqualTo(OrderStartResult.Started));
            Assert.That(book.TryStart(B, Bread, null), Is.EqualTo(OrderStartResult.BurnerBusy));
            book.Advance(6f);
            Assert.That(book.TryPickUp(A), Is.True);
            Assert.That(book.TryStart(B, Bread, null), Is.EqualTo(OrderStartResult.Started), "접시를 들면 칸이 빈다");
        }

        [Test]
        public void EachSeatedVisitOpensOneOrder()
        {
            var book = Book();
            Assert.That(book.TryOpen(A), Is.True);
            Assert.That(book.TryOpen(B), Is.True);
            Assert.That(book.TryOpen(A), Is.False, "같은 방문의 중복 주문");
            Assert.That(book.TryOpen(""), Is.False);
            Assert.That(book.TryOpen(null), Is.False);
            Assert.That(book.Orders.Count, Is.EqualTo(2));
        }

        [Test]
        public void UnspecifiedCookingStartsTheFirstOpenedOrder()
        {
            var book = Book();
            book.TryOpen(A);
            book.TryOpen(B);
            Assert.That(book.TryStart(null, Bread, "garlic"), Is.EqualTo(OrderStartResult.Started));
            Assert.That(book.Cooking.VisitId, Is.EqualTo(A));
            Assert.That(book.Cooking.BurnerIndex, Is.Zero);
            Assert.That(book.NextWaiting.VisitId, Is.EqualTo(B));
        }

        [Test]
        public void TwoBurnersCookInParallelAndAThirdIsRejectedWithoutConsuming()
        {
            var book = Book(out _, out var spices, garlic: 3);
            book.TryOpen(A); book.TryOpen(B); book.TryOpen(C);
            Assert.That(book.TryStart(A, Bread, "garlic"), Is.EqualTo(OrderStartResult.Started));
            book.Advance(2f);
            Assert.That(book.TryStart(B, Bread, "garlic"), Is.EqualTo(OrderStartResult.Started));
            Assert.That(book.Find(B).BurnerIndex, Is.EqualTo(1));
            Assert.That(book.FreeBurner, Is.EqualTo(-1));
            Assert.That(book.TryStart(C, Bread, "garlic"), Is.EqualTo(OrderStartResult.BurnerBusy));
            Assert.That(spices.Count("garlic"), Is.EqualTo(1), "거절된 조리는 양념을 빼지 않는다");

            int finished = 0;
            book.Finished += _ => finished++;
            book.Advance(4f);
            Assert.That(book.Find(A).State, Is.EqualTo(FirstGuestOrderState.Ready));
            Assert.That(book.Find(B).RemainingSeconds, Is.EqualTo(2f), "두 칸이 같은 조리 시계로 함께 줄어든다");
            Assert.That(finished, Is.EqualTo(1));
            Assert.That(book.FreeBurner, Is.EqualTo(-1), "완성 접시도 칸을 차지한다");
            Assert.That(book.TryPickUp(A), Is.True);
            Assert.That(book.FreeBurner, Is.Zero, "픽업하면 그 칸이 빈다");
        }

        [Test]
        public void DungeonIngredientsAreConsumedOnlyWhenAllAreInStock()
        {
            var book = Book(out var materials, out _, garlic: 0, mushrooms: 1);
            book.TryOpen(A);
            Assert.That(book.TryStart(A, Soup, null), Is.EqualTo(OrderStartResult.NotEnoughStock));
            Assert.That(book.TryStart(A, Bread, "garlic"), Is.EqualTo(OrderStartResult.NotEnoughStock), "양념 부족");
            Assert.That(materials.Count("숲버섯"), Is.EqualTo(1));
            Assert.That(book.Find(A).State, Is.EqualTo(FirstGuestOrderState.Waiting));
            materials.TryAdd("숲버섯", 1);
            Assert.That(book.TryStart(A, Soup, null), Is.EqualTo(OrderStartResult.Started), "감자는 기본 재료라 재고를 보지 않는다");
            Assert.That(materials.Count("숲버섯"), Is.Zero);
            Assert.That(book.Find(A).RecipeId, Is.EqualTo("MushroomSoup"));
            Assert.That(book.Find(A).UsedSeasoning, Is.False);
        }

        [Test]
        public void InvalidStartLeavesTheOrderUntouched()
        {
            var book = Book();
            book.TryOpen(A);
            Assert.That(book.TryStart(B, Bread, null), Is.EqualTo(OrderStartResult.NoOrder), "주문 없는 방문");
            Assert.That(book.TryStart(A, default, null), Is.EqualTo(OrderStartResult.NoOrder), "레시피 없음");
            Assert.That(book.Find(A).RemainingSeconds, Is.Zero);
            Assert.That(book.TryStart(A, Bread, null), Is.EqualTo(OrderStartResult.Started), "양념 없이 굽기");
            Assert.That(book.TryStart(A, Bread, null), Is.EqualTo(OrderStartResult.NoOrder), "이미 조리 중");
        }

        [Test]
        public void CookingBecomesReadyOnlyAfterTheCookTime()
        {
            var book = Book();
            book.TryOpen(A);
            book.TryStart(A, Bread, "garlic");
            book.Advance(2f);
            book.Advance(0f);
            book.Advance(-1f);
            book.Advance(float.NaN);
            Assert.That(book.Find(A).RemainingSeconds, Is.EqualTo(4f));
            book.Advance(3.99f);
            Assert.That(book.TryPickUp(A), Is.False, "조리 중 픽업");
            book.Advance(0.02f);
            Assert.That(book.Find(A).State, Is.EqualTo(FirstGuestOrderState.Ready));
        }

        [Test]
        public void ADishCanOnlyBeServedToItsOwnVisitOnce()
        {
            var book = Book();
            book.TryOpen(A);
            book.TryOpen(B);
            Assert.That(book.TryServe(A), Is.False, "접시 없이 서빙");
            CookAndCarry(book, A);
            Assert.That(book.TryServe(B), Is.False, "다른 손님에게 서빙");
            Assert.That(book.Carried.VisitId, Is.EqualTo(A), "접시는 여전히 A의 것");
            Assert.That(book.TryServe(A), Is.True);
            Assert.That(book.Find(A).Spice, Is.EqualTo("garlic"), "대금 계산에 쓰는 넣은 양념");
            Assert.That(book.TryServe(A), Is.False, "같은 주문 두 번째 서빙");
        }

        [Test]
        public void OnlyOnePlateCanBeCarried()
        {
            var book = Book();
            book.TryOpen(A);
            book.TryOpen(B);
            CookAndCarry(book, A);
            Assert.That(book.TryStart(B, Bread, "garlic"), Is.EqualTo(OrderStartResult.Started));
            book.Advance(6f);
            Assert.That(book.TryPickUp(), Is.False, "접시를 든 채 두 번째 픽업");
            Assert.That(book.TryServe(A), Is.True);
            Assert.That(book.TryPickUp(), Is.True);
            Assert.That(book.Carried.VisitId, Is.EqualTo(B));
        }

        [Test]
        public void CancellingWhileCookingFreesTheBurnerWithoutRefund()
        {
            var book = Book(out _, out var spices);
            book.TryOpen(A);
            book.TryOpen(B);
            book.TryStart(A, Bread, "garlic");
            Assert.That(book.Cancel(A), Is.True);
            Assert.That(book.Cooking, Is.Null);
            Assert.That(book.FreeBurner, Is.Zero);
            Assert.That(spices.Count("garlic"), Is.EqualTo(1), "넣은 양념은 돌려주지 않는다");
            Assert.That(book.TryStart(A, Bread, "garlic"), Is.EqualTo(OrderStartResult.NoOrder), "취소된 주문은 조리할 수 없다");
            Assert.That(book.TryStart(B, Bread, "garlic"), Is.EqualTo(OrderStartResult.Started));
        }

        [Test]
        public void CancellingDropsTheCarriedPlate()
        {
            var book = Book();
            book.TryOpen(A);
            CookAndCarry(book, A);
            Assert.That(book.Cancel(A), Is.True);
            Assert.That(book.Carried, Is.Null);
            Assert.That(book.TryServe(A), Is.False);
            Assert.That(book.Cancel(A), Is.False, "이미 취소됨");
        }

        [Test]
        public void ServedOrdersCannotBeCancelled()
        {
            var book = Book();
            book.TryOpen(A);
            CookAndCarry(book, A);
            book.TryServe(A);
            Assert.That(book.Cancel(A), Is.False);
            Assert.That(book.Find(A).State, Is.EqualTo(FirstGuestOrderState.Served));
        }
    }
}
