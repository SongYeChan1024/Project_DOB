using DOB.Systems;
using NUnit.Framework;

namespace DOB.Tests
{
    public sealed class FirstGuestOrderTests
    {
        private const string Visit = "day-1-haru";

        private static FirstGuestOrder Haru(int garlicStock = 2) =>
            new FirstGuestOrder(Visit, 6f, garlicStock, 2, 1, 8);

        [Test]
        public void PlainBreadCanBeServedWithNoGarlicLeft()
        {
            var order = new FirstGuestOrder(Visit, 6f, 0, 2, 0, 8);
            Assert.That(order.TryStart(Visit, 2, 0), Is.True);
            order.Advance(6f);
            Assert.That(order.TryPickUp(Visit), Is.True);
            Assert.That(order.TryServe(Visit, out int payment), Is.True);
            Assert.That(payment, Is.EqualTo(8));
            Assert.That(order.GarlicStock, Is.Zero);
        }

        [Test]
        public void InvalidConfirmationLeavesIngredientsAndClockUntouched()
        {
            var order = Haru();
            Assert.That(order.TryStart("previous-visit", 2, 1), Is.False);
            Assert.That(order.TryStart(Visit, 1, 1), Is.False);
            Assert.That(order.TryStart(Visit, 2, 2), Is.False);
            Assert.That(order.TryStart(Visit, 2, 0), Is.False);
            Assert.That(order.State, Is.EqualTo(FirstGuestOrderState.Waiting));
            Assert.That(order.GarlicStock, Is.EqualTo(2));
            Assert.That(order.RemainingSeconds, Is.Zero);

            var emptyStock = Haru(0);
            Assert.That(emptyStock.TryStart(Visit, 2, 1), Is.False);
            Assert.That(emptyStock.GarlicStock, Is.Zero);
            Assert.That(emptyStock.State, Is.EqualTo(FirstGuestOrderState.Waiting));
        }

        [Test]
        public void CookingConsumesOnceAndOnlyBecomesReadyAfterSixSeconds()
        {
            var order = Haru();
            Assert.That(order.TryStart(Visit, 2, 1), Is.True);
            order.Advance(2f);
            Assert.That(order.TryStart(Visit, 2, 1), Is.False);
            Assert.That(order.GarlicStock, Is.EqualTo(1));
            Assert.That(order.RemainingSeconds, Is.EqualTo(4f));

            order.Advance(0f);
            order.Advance(-1f);
            order.Advance(float.NaN);
            Assert.That(order.RemainingSeconds, Is.EqualTo(4f));
            order.Advance(3.99f);
            Assert.That(order.State, Is.EqualTo(FirstGuestOrderState.Cooking));
            Assert.That(order.TryPickUp(Visit), Is.False);
            order.Advance(0.02f);
            Assert.That(order.RemainingSeconds, Is.Zero);
            Assert.That(order.State, Is.EqualTo(FirstGuestOrderState.Ready));
        }

        [Test]
        public void OnlyTheOwningVisitCanReceiveTheDishAndPayOnce()
        {
            var order = Haru();
            Assert.That(order.TryPickUp(Visit), Is.False);
            Assert.That(order.TryServe(Visit, out var payment), Is.False);
            Assert.That(payment, Is.Zero);

            order.TryStart(Visit, 2, 1);
            order.Advance(10f); // 응대 중 조리 시간을 넘겨도 접시 하나만 준비된다.
            Assert.That(order.TryServe(Visit, out payment), Is.False);
            Assert.That(order.TryPickUp("different-visit"), Is.False);
            Assert.That(order.TryPickUp(Visit), Is.True);
            Assert.That(order.TryPickUp(Visit), Is.False);
            Assert.That(order.TryServe("different-visit", out payment), Is.False);
            Assert.That(payment, Is.Zero);
            Assert.That(order.State, Is.EqualTo(FirstGuestOrderState.Carrying));

            Assert.That(order.TryServe(Visit, out payment), Is.True);
            Assert.That(payment, Is.EqualTo(9));
            Assert.That(order.TryServe(Visit, out payment), Is.False);
            Assert.That(payment, Is.Zero);
            Assert.That(order.TryStart(Visit, 2, 1), Is.False);
            Assert.That(order.GarlicStock, Is.EqualTo(1));
            Assert.That(order.State, Is.EqualTo(FirstGuestOrderState.Served));
        }
    }
}
