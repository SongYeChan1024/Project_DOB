// GuestLedgerTests — 손님 명부(guest-ledger) 명세의 등록·누적·영구 거절·부상 휴식·재방문 판정 시나리오를 검증한다.
// 의사코드: 💡 시험 기준(누적 상한 36, 휴식 2일)으로 명부를 만들고, 방문 호감 기록과 방문 결과를 직접 넣어 하루씩 정산한다.
using System;
using System.Collections.Generic;
using System.Linq;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;
using UnityEngine;

namespace DOB.Tests
{
    public sealed class GuestLedgerTests
    {
        private AffinityRules rules;

        [SetUp]
        public void SetUp() => rules = ScriptableObject.CreateInstance<AffinityRules>();

        [TearDown]
        public void TearDown() => UnityEngine.Object.DestroyImmediate(rules);

        private static GuestRecord Guest(string id, int affinity = 0, bool rejected = false, int returnDay = 1) =>
            new GuestRecord(id, "이름_" + id, "수인", "사제", "독", affinity, rejected, returnDay);

        // 파견으로 끝난 방문 하나의 호감 기록. 내역은 합계만 쓴다.
        private static VisitAffinity Visit(string guestId, int total, ServiceVisitEnd end = ServiceVisitEnd.Dispatched) =>
            new VisitAffinity(guestId, "visit_" + guestId, end, 0, 0, 0, total);

        private static VisitResult Result(string guestId, ExpeditionGrade grade) =>
            new VisitResult(guestId, "visit_" + guestId, "이름_" + guestId, 1, "Bread", false, new int[6], grade, new string[0]);

        // 명부에 있는 손님 한 명의 하루를 정산하고 그 기록을 돌려준다.
        private GuestRecord SettleOne(GuestLedger ledger, int day, string id, int total,
            ServiceVisitEnd end = ServiceVisitEnd.Dispatched, ExpeditionGrade? grade = null)
        {
            Assert.That(ledger.SettleDay(rules, day, new[] { Guest(id) }, new[] { Visit(id, total, end) },
                grade == null ? null : new[] { Result(id, grade.Value) }), Is.True);
            return ledger.Find(id);
        }

        [Test]
        public void AnEmptyLedgerHasNoGuestsAndDuplicateIdsAreRejected()
        {
            var ledger = new GuestLedger();
            Assert.That(ledger.Records, Is.Empty);
            Assert.That(ledger.Find("g1"), Is.Null);
            Assert.That(ledger.EligibleOn(2), Is.Empty);
            Assert.Throws<ArgumentException>(() => new GuestLedger(new[] { Guest("g1"), Guest("g1") }));
            Assert.Throws<ArgumentException>(() => new GuestLedger(new[] { Guest("") }));
        }

        [Test]
        public void AFirstVisitRegistersTheIdentityAtSettlement()
        {
            var ledger = new GuestLedger();
            Assert.That(ledger.Find("g1"), Is.Null, "정산 전에는 명부에 없다");
            var record = SettleOne(ledger, 1, "g1", 3);
            Assert.That((record.Name, record.Race, record.Job, record.Weakness), Is.EqualTo(("이름_g1", "수인", "사제", "독")));
            Assert.That((record.Affinity, record.Rejected, record.ReturnDay), Is.EqualTo((3, false, 2)));
        }

        [Test]
        public void VisitPointsAddToTheCumulativeAffinityAndStopAtTheCap()
        {
            Assert.That(SettleOne(new GuestLedger(new[] { Guest("g1", 5) }), 1, "g1", -1).Affinity, Is.EqualTo(4));
            Assert.That(SettleOne(new GuestLedger(new[] { Guest("g1", 5) }), 1, "g1", -4, ServiceVisitEnd.SentHome).Affinity,
                Is.EqualTo(1), "돌려보낸 재방문 손님");
            Assert.That(SettleOne(new GuestLedger(new[] { Guest("g1", 34) }), 1, "g1", 4).Affinity, Is.EqualTo(36), "상한에서 멈춤");
        }

        [Test]
        public void BelowZeroIsRejectedForGoodAndExactlyZeroIsNot()
        {
            var negative = SettleOne(new GuestLedger(new[] { Guest("g1", 1) }), 1, "g1", -2);
            Assert.That((negative.Rejected, negative.Affinity), Is.EqualTo((true, 0)), "1 − 2 → 거절, 표시값은 0");
            var zero = SettleOne(new GuestLedger(new[] { Guest("g1", 2) }), 1, "g1", -2);
            Assert.That((zero.Rejected, zero.Affinity), Is.EqualTo((false, 0)), "2 − 2 → 0은 거절이 아니다");
            Assert.That(SettleOne(new GuestLedger(), 1, "g1", -4, ServiceVisitEnd.SentHome).Rejected, Is.True, "처음 온 손님을 돌려보냄");

            var ledger = new GuestLedger(new[] { Guest("g1", 1) });
            SettleOne(ledger, 1, "g1", -2);
            for (int day = 2; day <= 6; day++)
                Assert.That(ledger.EligibleOn(day), Is.Empty, day + "일차에도 돌아오지 않는다");
        }

        [Test]
        public void EligibilityFollowsRejectionAndTheReturnDay()
        {
            var ledger = new GuestLedger(new[]
            {
                Guest("positive", 3, false, 2), Guest("zero", 0, false, 2),
                Guest("rejected", 0, true, 2), Guest("resting", 6, false, 4)
            });
            CollectionAssert.AreEqual(new[] { "positive", "zero" }, ledger.EligibleOn(2).Select(r => r.GuestId), "0인 손님도 다음 날 온다");
            CollectionAssert.AreEqual(new[] { "positive", "zero", "resting" }, ledger.EligibleOn(4).Select(r => r.GuestId));
            CollectionAssert.AreEqual(ledger.EligibleOn(2).Select(r => r.GuestId), ledger.EligibleOn(2).Select(r => r.GuestId));
            Assert.That(ledger.Records.Count, Is.EqualTo(4), "조회는 명부를 바꾸지 않는다");
        }

        [Test]
        public void AnInjuredGuestRestsTwoDaysAndRejectionWins()
        {
            var ledger = new GuestLedger(new[] { Guest("g1", 6) });
            SettleOne(ledger, 1, "g1", 0, grade: ExpeditionGrade.GreatFailure);
            Assert.That(ledger.Find("g1").ReturnDay, Is.EqualTo(4));
            Assert.That((ledger.EligibleOn(2).Count, ledger.EligibleOn(3).Count, ledger.EligibleOn(4).Count), Is.EqualTo((0, 0, 1)));

            var both = SettleOne(new GuestLedger(), 1, "g1", -2, grade: ExpeditionGrade.GreatFailure);
            Assert.That(both.Rejected, Is.True, "부상과 거절이 겹치면 거절이 우선");
            Assert.That(both.CanVisitOn(4), Is.False);

            var failure = SettleOne(new GuestLedger(new[] { Guest("g1", 3) }), 1, "g1", -1, grade: ExpeditionGrade.Failure);
            Assert.That(failure.CanVisitOn(2), Is.True, "대실패가 아니면 쉬지 않는다");

            JsonUtility.FromJsonOverwrite("{\"injuryRestDays\":0}", rules);
            var noRest = SettleOne(new GuestLedger(new[] { Guest("g1", 6) }), 1, "g1", 0, grade: ExpeditionGrade.GreatFailure);
            Assert.That(noRest.CanVisitOn(2), Is.True, "휴식 0일이면 다음 날 온다");
        }

        [Test]
        public void SettlingTheSameDayTwiceKeepsTheFirstResult()
        {
            var ledger = new GuestLedger(new[] { Guest("g1", 5) });
            var guests = new[] { Guest("g1") };
            var visits = new[] { Visit("g1", 3) };
            Assert.That(ledger.SettleDay(rules, 1, guests, visits, null), Is.True);
            Assert.That(ledger.SettleDay(rules, 1, guests, visits, null), Is.False, "정산 재요청");
            var record = ledger.Find("g1");
            Assert.That((record.Affinity, record.Rejected, record.ReturnDay), Is.EqualTo((8, false, 2)));
        }

        [Test]
        public void GuestsWhoDidNotVisitAndVisitsWithoutAnIdentityAreLeftAlone()
        {
            var ledger = new GuestLedger(new[] { Guest("stayed", 4, false, 2) });
            Assert.That(ledger.SettleDay(rules, 2, new GuestRecord[0], new[] { Visit("unknown", 3) }, null), Is.True);
            Assert.That(ledger.Find("unknown"), Is.Null, "신원을 모르는 손님은 등록하지 않는다");
            var stayed = ledger.Find("stayed");
            Assert.That((stayed.Affinity, stayed.ReturnDay), Is.EqualTo((4, 2)));
        }
    }
}
