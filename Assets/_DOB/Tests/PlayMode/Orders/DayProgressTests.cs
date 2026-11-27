// DayProgressTests — 수동 저장(day-save 명세)의 「영업 진행 상태의 저장과 복구」를 System 단위로 검증한다.
// 의사코드: 같은 시드로 연 두 명단 중 하나를 진행시킨 뒤 상태를 RosterProgress로 옮기고, 다른 하나에 AdmitRestored + Restore로 되살려 비교한다.
//           주문·방명록·결과도 같은 방식으로 저장 → 복원하고, 손상된 진행 상태는 Validate가 거절하는지 본다.
using System.Collections.Generic;
using System.Linq;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;
using UnityEngine;

namespace DOB.Tests
{
    public sealed class DayProgressTests
    {
        private ServiceRosterSettings settings;

        [SetUp]
        public void SetUp()
        {
            settings = ScriptableObject.CreateInstance<ServiceRosterSettings>();
            JsonUtility.FromJsonOverwrite("{\"firstArrivalSeconds\":0,\"baseGuests\":3,\"arrivalMinSeconds\":8,\"arrivalMaxSeconds\":20,\"queueCapacity\":3," +
                "\"waitStartScore\":2,\"scoreStepSeconds\":10,\"leaveAfterSeconds\":30}", settings);
        }

        [TearDown]
        public void TearDown() => Object.DestroyImmediate(settings);

        private ServiceRoster Open(int seed)
        {
            var roster = new ServiceRoster(settings, new System.Random(seed), 3);
            roster.Tick(0f);
            return roster;
        }

        private static RosterProgress Snapshot(ServiceRoster roster) => new RosterProgress(roster.NextArrivalSeconds, roster.ArrivalDue,
            roster.Visits.Select(v => new VisitProgress(v.VisitId, v.GuestId, v.Stage, v.End, v.WaitSeconds, v.WaitScore, v.TasteScore, -1)));

        [Test]
        public void ARestoredRosterMatchesTheSavedOneAndKeepsTheArrivalSchedule()
        {
            var played = Open(7);
            Assert.That(played.Admit("v1", "g1"), Is.True);
            played.Tick(played.NextArrivalSeconds);
            Assert.That(played.Admit("v2", "g2"), Is.True);
            played.TryBeginGreeting("v1");
            played.TryGuide("v1");
            played.TrySeat("v1");
            played.Tick(4f);
            played.TryServe("v1", 2);
            played.TryEnd("v1", ServiceVisitEnd.Dispatched);
            played.TryBeginGreeting("v2");
            var saved = Snapshot(played);

            var restored = Open(7);
            Assert.That(restored.AdmitRestored("v1", "g1") && restored.AdmitRestored("v2", "g2"), Is.True);
            Assert.That(restored.Restore(saved), Is.True);

            Assert.That(restored.NextArrivalSeconds, Is.EqualTo(played.NextArrivalSeconds));
            var first = restored.Find("v1");
            Assert.That((first.End, first.WaitScore, first.TasteScore), Is.EqualTo((ServiceVisitEnd.Dispatched, played.Find("v1").WaitScore, (int?)2)));
            Assert.That(restored.ServedGuestCount, Is.EqualTo(1));
            Assert.That(restored.Find("v2").Stage, Is.EqualTo(ServiceVisitStage.AtEntrance), "응대 중이던 방문은 입구 대기로 돌아간다");
            Assert.That(restored.Find("v2").WaitSeconds, Is.EqualTo(played.Find("v2").WaitSeconds), "대기 시간은 유지");

            // 이후 입장 간격 난수도 같은 순서로 이어진다.
            played.Tick(played.NextArrivalSeconds);
            restored.Tick(restored.NextArrivalSeconds);
            Assert.That(restored.CanAdmit, Is.EqualTo(played.CanAdmit));
        }

        [Test]
        public void ARosterRefusesASaveWhoseVisitsDoNotMatch()
        {
            var played = Open(7);
            played.Admit("v1", "g1");
            var restored = Open(7);
            restored.AdmitRestored("other", "g1");
            Assert.That(restored.Restore(Snapshot(played)), Is.False);
            Assert.That(restored.Find("other").Stage, Is.EqualTo(ServiceVisitStage.AtEntrance));
        }

        [Test]
        public void OrdersComeBackWithTheirStateTimeAndGarlicAndAHeldPlateReturnsToTheCounter()
        {
            var book = new OrderBook(2, 6f, 2, 8);
            book.TryOpen("v1"); book.TryOpen("v2"); book.TryOpen("v3");
            book.TryStart("v1", 2, 1); book.Advance(6f); book.TryPickUp("v1");
            book.TryStart("v2", 2, 0); book.Advance(2f);
            var saved = book.Orders.Select(o => new OrderProgress(o.VisitId, o.State, o.RemainingSeconds, o.UsedGarlic)).ToList();

            var restored = new OrderBook(5, 6f, 2, 8);
            Assert.That(restored.Restore(book.GarlicStock, saved), Is.True);
            Assert.That(restored.GarlicStock, Is.EqualTo(1));
            Assert.That(restored.Find("v1").State, Is.EqualTo(FirstGuestOrderState.Ready));
            Assert.That(restored.Find("v1").UsedGarlic, Is.EqualTo(1));
            Assert.That(restored.Cooking.VisitId, Is.EqualTo("v2"));
            Assert.That(restored.Cooking.RemainingSeconds, Is.EqualTo(4f));
            Assert.That(restored.NextWaiting.VisitId, Is.EqualTo("v3"));
            Assert.That(restored.Restore(1, saved), Is.False, "이미 주문이 있으면 다시 채우지 않는다");
        }

        [Test]
        public void GuestbookAndResultsComeBackWithoutDuplicates()
        {
            var log = new GuestbookLog(10f);
            log.TryBegin("g1", "v1", "하루", "맛있었다");
            log.Tick(4f);
            var restored = new GuestbookLog(10f);
            int completed = 0;
            restored.EntryCompleted += _ => completed++;
            foreach (var e in log.Snapshot())
                Assert.That(restored.Restore(e.GuestId, e.VisitId, e.GuestName, e.Line, e.RemainingSeconds), Is.True);
            Assert.That(restored.Restore("g1", "v1", "하루", "맛있었다", 1f), Is.False);
            Assert.That(restored.Snapshot()[0].RemainingSeconds, Is.EqualTo(6f));
            restored.Tick(6f);
            Assert.That(completed, Is.EqualTo(1));
        }

        [Test]
        public void AProgressOutsideServiceCannotCarryARoster()
        {
            var roster = new RosterProgress(0f, false, null);
            Assert.That(Valid(new DayProgress(DayPhase.Preparation, null, 10, 1, roster)), Is.False);
            Assert.That(Valid(new DayProgress(DayPhase.Preparation, new[] { "v1" }, 10, 1)), Is.True);
            Assert.That(Valid(new DayProgress(DayPhase.MorningReport, new[] { "v1", "v1" }, 10, 1)), Is.False, "같은 보고 두 번");
        }

        [Test]
        public void AServiceProgressMustPointOnlyAtItsOwnVisits()
        {
            var roster = new RosterProgress(3f, false, new[] { new VisitProgress("v1", "g1", ServiceVisitStage.Seated, ServiceVisitEnd.None, 2f, null, null, 0) });
            Assert.That(Valid(new DayProgress(DayPhase.Service, null, 0, 0, roster,
                new[] { new OrderProgress("v1", FirstGuestOrderState.Waiting, 6f, 0) })), Is.True);
            Assert.That(Valid(new DayProgress(DayPhase.Service, null, 0, 0, roster,
                new[] { new OrderProgress("v9", FirstGuestOrderState.Waiting, 6f, 0) })), Is.False);
            var ended = new RosterProgress(3f, false, new[] { new VisitProgress("v1", "g1", ServiceVisitStage.Seated, ServiceVisitEnd.Dispatched, 2f, 2, 0, 0) });
            Assert.That(Valid(new DayProgress(DayPhase.Service, null, 0, 0, ended)), Is.False, "끝나지 않은 단계에 종료 방식");
        }

        private static bool Valid(DayProgress progress) => progress.Validate();
    }
}
