// OrderBookTests — 방문별 주문, 공유 마늘 재고, 조리 칸 하나, 들고 있는 접시 하나, 떠난 방문의 취소를 검증한다.
// 의사코드: 시험 레시피(조리 6초, 밀가루 2)와 마늘 재고로 장부를 열고 order-book 명세의 시나리오를 그대로 따라간다. 대금은 PriceJudge가 정한다(EconomyTests).
using DOB.Systems;
using NUnit.Framework;

namespace DOB.Tests
{
    public sealed class OrderBookTests
    {
        private const string A = "visit-a";
        private const string B = "visit-b";

        private static OrderBook Book(int garlic = 2) => new OrderBook(new MaterialStock(new[] { new MaterialCount("garlic", garlic) }), "garlic", 6f, 2);

        private static void CookAndCarry(OrderBook book, string visit, int garlic)
        {
            Assert.That(book.TryStart(visit, 2, garlic), Is.True, "조리 시작 " + visit);
            book.Advance(6f);
            Assert.That(book.TryPickUp(visit), Is.True, "픽업 " + visit);
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
            Assert.That(book.Find(A).VisitId, Is.EqualTo(A));
            Assert.That(book.Find(B).VisitId, Is.EqualTo(B));
        }

        [Test]
        public void UnspecifiedCookingStartsTheFirstOpenedOrder()
        {
            var book = Book();
            book.TryOpen(A);
            book.TryOpen(B);
            Assert.That(book.TryStart(null, 2, 1), Is.True);
            Assert.That(book.Cooking.VisitId, Is.EqualTo(A));
            Assert.That(book.NextWaiting.VisitId, Is.EqualTo(B));
        }

        [Test]
        public void OneSlotRejectsASecondCookAndKeepsStock()
        {
            var book = Book();
            book.TryOpen(A);
            book.TryOpen(B);
            Assert.That(book.TryStart(A, 2, 1), Is.True);
            Assert.That(book.TryStart(B, 2, 1), Is.False, "조리 중에는 다른 조리를 거절");
            Assert.That(book.GarlicStock, Is.EqualTo(1));
            Assert.That(book.Find(B).State, Is.EqualTo(FirstGuestOrderState.Waiting));
        }

        [Test]
        public void TwoOrdersShareTheGarlicStock()
        {
            var book = Book(2);
            book.TryOpen(A);
            book.TryOpen(B);
            CookAndCarry(book, A, 1);
            Assert.That(book.TryServe(A), Is.True);
            CookAndCarry(book, B, 1);
            Assert.That(book.GarlicStock, Is.Zero);
        }

        [Test]
        public void InvalidStartLeavesStockAndClockUntouched()
        {
            var book = Book(0);
            book.TryOpen(A);
            Assert.That(book.TryStart(A, 2, 1), Is.False, "재고 부족");
            Assert.That(book.TryStart(A, 1, 0), Is.False, "잘못된 밀가루");
            Assert.That(book.TryStart(B, 2, 0), Is.False, "주문 없는 방문");
            Assert.That(book.GarlicStock, Is.Zero);
            Assert.That(book.Find(A).State, Is.EqualTo(FirstGuestOrderState.Waiting));
            Assert.That(book.Find(A).RemainingSeconds, Is.Zero);
            Assert.That(book.TryStart(A, 2, 0), Is.True, "양념 없이 굽기");
        }

        [Test]
        public void CookingBecomesReadyOnlyAfterTheCookTime()
        {
            var book = Book();
            book.TryOpen(A);
            book.TryStart(A, 2, 1);
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
        public void ADishCanOnlyBeServedToItsOwnVisitAndPaysOnce()
        {
            var book = Book();
            book.TryOpen(A);
            book.TryOpen(B);
            Assert.That(book.TryServe(A), Is.False, "접시 없이 서빙");
            CookAndCarry(book, A, 1);
            Assert.That(book.TryServe(B), Is.False, "다른 손님에게 서빙");
            Assert.That(book.Carried.VisitId, Is.EqualTo(A), "접시는 여전히 A의 것");

            Assert.That(book.TryServe(A), Is.True);
            Assert.That(book.Find(A).UsedGarlic, Is.EqualTo(1), "대금 계산에 쓰는 넣은 양념 수");
            Assert.That(book.TryServe(A), Is.False, "같은 주문 두 번째 서빙");
        }

        [Test]
        public void OnlyOnePlateCanBeCarried()
        {
            var book = Book();
            book.TryOpen(A);
            book.TryOpen(B);
            CookAndCarry(book, A, 1);
            Assert.That(book.TryStart(B, 2, 1), Is.True);
            book.Advance(6f);
            Assert.That(book.TryPickUp(), Is.False, "접시를 든 채 두 번째 픽업");
            Assert.That(book.TryServe(A), Is.True);
            Assert.That(book.TryPickUp(), Is.True);
            Assert.That(book.Carried.VisitId, Is.EqualTo(B));
        }

        [Test]
        public void CancellingWhileCookingFreesTheSlotWithoutRefund()
        {
            var book = Book();
            book.TryOpen(A);
            book.TryOpen(B);
            book.TryStart(A, 2, 1);
            Assert.That(book.Cancel(A), Is.True);
            Assert.That(book.Cooking, Is.Null);
            Assert.That(book.GarlicStock, Is.EqualTo(1), "넣은 양념은 돌려주지 않는다");
            Assert.That(book.TryStart(A, 2, 1), Is.False, "취소된 주문은 조리할 수 없다");
            Assert.That(book.TryStart(B, 2, 1), Is.True, "다른 대기 주문의 조리를 시작할 수 있다");
        }

        [Test]
        public void CancellingDropsTheCarriedPlate()
        {
            var book = Book();
            book.TryOpen(A);
            CookAndCarry(book, A, 1);
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
            CookAndCarry(book, A, 1);
            book.TryServe(A);
            Assert.That(book.Cancel(A), Is.False);
            Assert.That(book.Find(A).State, Is.EqualTo(FirstGuestOrderState.Served));
        }
    }
}
