using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;

namespace DOB.Tests
{
    public sealed class DayCycleTests
    {
        [Test]
        public void FirstDayStartsAtServiceAndLaterDaysAtMorningReport()
        {
            var cycle = new DayCycle();
            cycle.Begin(1);
            Assert.That(cycle.Phase, Is.EqualTo(DayPhase.Service));
            cycle.Begin(2);
            Assert.That(cycle.Phase, Is.EqualTo(DayPhase.MorningReport));
            Assert.That(cycle.Day, Is.EqualTo(2));
        }

        [Test]
        public void PhasesAdvanceInOrderAndDuplicateRequestsDoNotSkip()
        {
            var cycle = new DayCycle();
            var seen = new List<DayPhase>();
            cycle.PhaseChanged += seen.Add;
            cycle.Begin(2);

            Assert.That(cycle.TryComplete(DayPhase.Preparation, out _), Is.False, "아침보고 중 준비 완료 요청은 거절");
            Assert.That(cycle.TryComplete(DayPhase.MorningReport, out _), Is.True);
            Assert.That(cycle.TryComplete(DayPhase.MorningReport, out _), Is.False, "같은 확인 두 번은 한 번만");
            Assert.That(cycle.TryComplete(DayPhase.Preparation, out _), Is.True);
            Assert.That(cycle.TryComplete(DayPhase.Preparation, out _), Is.False, "문 열기 연속 클릭은 한 번만");
            Assert.That(cycle.TryComplete(DayPhase.Service, out _), Is.False, "영업은 화면 확인으로 끝나지 않음");
            Assert.That(cycle.TrySettle(true, out _), Is.True);
            CollectionAssert.AreEqual(new[] { DayPhase.MorningReport, DayPhase.Preparation, DayPhase.Service, DayPhase.Closing }, seen);
        }

        [Test]
        public void ServiceHasNoClockAndSettlesOnceWhenAllVisitsEnd()
        {
            var cycle = new DayCycle();
            cycle.Begin(2);
            Assert.That(cycle.TrySettle(true, out _), Is.False, "영업 전에는 정산 불가");
            cycle.TryComplete(DayPhase.MorningReport, out _);
            cycle.TryComplete(DayPhase.Preparation, out _);
            Assert.That(cycle.TrySettle(false, out _), Is.False, "남은 방문이 있으면 영업 계속");
            Assert.That(cycle.Phase, Is.EqualTo(DayPhase.Service));
            Assert.That(cycle.TrySettle(true, out _), Is.True, "마지막 방문이 끝나면 바로 마감·정산");
            Assert.That(cycle.Phase, Is.EqualTo(DayPhase.Closing));
            Assert.That(cycle.TrySettle(true, out _), Is.False, "두 번째 정산은 거절");
            Assert.That(cycle.IsSettled, Is.True);
        }

        [Test]
        public void GuestbookCompletesOnceAndIgnoresDuplicateVisits()
        {
            var log = new GuestbookLog(3f);
            int completed = 0;
            log.EntryCompleted += _ => completed++;
            Assert.That(log.AllComplete, Is.True, "글이 없으면(돌려보냄) 막지 않음");
            Assert.That(log.TryBegin("haru", "visit-1", "하루", "맛있어요"), Is.True);
            Assert.That(log.TryBegin("haru", "visit-1", "하루", "맛있어요"), Is.False);
            Assert.That(log.TryBegin("", "visit-2", "?", "?"), Is.False);
            Assert.That(log.AllComplete, Is.False);
            log.Tick(2f);
            log.Tick(0f);
            Assert.That(completed, Is.Zero, "정지 중에는 작성 시간이 흐르지 않음");
            log.Tick(1f);
            log.Tick(5f);
            Assert.That(completed, Is.EqualTo(1));
            Assert.That(log.AllComplete, Is.True);
        }
    }
}
