// AffinityJudgeTests — 방문 호감(visit-affinity)과 입소문(word-of-mouth) 명세의 계산 시나리오와 기준값 검사를 검증한다.
// 의사코드: JsonUtility로 💡 시험 기준(+2·+1·−1·−2, −4, −4, 최대 6, 60·75·85·100%)을 만들고, 실제 ServiceRoster로
// 방문을 끝낸 뒤 AffinityJudge에 넘긴다. 문턱 올림은 기획 검산표(3명·4명)를 그대로 따라간다.
using System;
using System.Collections.Generic;
using System.Linq;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;
using UnityEngine;

namespace DOB.Tests
{
    public sealed class AffinityJudgeTests
    {
        private readonly List<ScriptableObject> created = new List<ScriptableObject>();

        [TearDown]
        public void TearDown()
        {
            foreach (var asset in created)
                UnityEngine.Object.DestroyImmediate(asset);
            created.Clear();
        }

        private AffinityRules Rules(string json = "")
        {
            var rules = ScriptableObject.CreateInstance<AffinityRules>();
            if (json.Length > 0)
                JsonUtility.FromJsonOverwrite(json, rules);
            created.Add(rules);
            return rules;
        }

        // 입장 1초 간격, 넉넉한 줄·대기 한도의 명단. 방문 ID는 v1, v2 … 순서다.
        private ServiceRoster Roster(int guests)
        {
            var settings = ScriptableObject.CreateInstance<ServiceRosterSettings>();
            JsonUtility.FromJsonOverwrite(
                $"{{\"firstArrivalSeconds\":0,\"baseGuests\":{guests},\"arrivalMinSeconds\":1,\"arrivalMaxSeconds\":1,\"queueCapacity\":{guests}," +
                "\"waitStartScore\":2,\"scoreStepSeconds\":10,\"leaveAfterSeconds\":300}", settings);
            created.Add(settings);
            var roster = new ServiceRoster(settings, new System.Random(1));
            for (int i = 1; i <= guests; i++)
            {
                roster.Tick(i == 1 ? 0f : 1f);
                Assert.That(roster.Admit("v" + i, "g" + i), Is.True);
            }
            return roster;
        }

        private static void Dispatch(ServiceRoster roster, string visit)
        {
            Assert.That(roster.TryBeginGreeting(visit) && roster.TryGuide(visit) && roster.TrySeat(visit) && roster.TryServe(visit), Is.True);
            Assert.That(roster.TryEnd(visit, ServiceVisitEnd.Dispatched), Is.True);
        }

        private static void SendHome(ServiceRoster roster, string visit) =>
            Assert.That(roster.TryBeginGreeting(visit) && roster.TryEnd(visit, ServiceVisitEnd.SentHome), Is.True);

        private static VisitResult Result(string visit, ExpeditionGrade grade) =>
            new VisitResult("g" + visit.Substring(1), visit, "하루", 1, "구운 빵", false, new int[6], grade, new string[0]);

        private static VisitAffinity Record(ServiceVisitEnd end, int total) => new VisitAffinity("g", Guid.NewGuid().ToString("N"), end, 0, 0, 0, total);

        // ── 2.1 방문 하나 ──

        [Test]
        public void DispatchedVisitAddsWaitTasteAndExpedition()
        {
            var rules = Rules();
            var success = AffinityJudge.Score(rules, "g", "v", ServiceVisitEnd.Dispatched, 2, 0, ExpeditionGrade.Success);
            Assert.That((success.Total, success.Wait, success.Taste, success.Expedition), Is.EqualTo((3, 2, 0, 1)));
            Assert.That(AffinityJudge.Score(rules, "g", "v", ServiceVisitEnd.Dispatched, 0, 0, ExpeditionGrade.GreatFailure).Total, Is.EqualTo(-2));
            Assert.That(AffinityJudge.Score(rules, "g", "v", ServiceVisitEnd.Dispatched, 2, 2, ExpeditionGrade.GreatSuccess).Total, Is.EqualTo(6));
            Assert.That(AffinityJudge.Score(rules, "g", "v", ServiceVisitEnd.Dispatched, 1, 0, ExpeditionGrade.Failure).Total, Is.Zero);
            Assert.Throws<ArgumentNullException>(() => AffinityJudge.Score(rules, "g", "v", ServiceVisitEnd.Dispatched, 2, 0, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => AffinityJudge.Score(rules, "g", "v", ServiceVisitEnd.None, 2, 0, null));
        }

        [Test]
        public void SentHomeAndWaitedOutGetOnlyTheirPenalty()
        {
            var rules = Rules();
            foreach (var end in new[] { ServiceVisitEnd.SentHome, ServiceVisitEnd.WaitedOut })
            {
                var record = AffinityJudge.Score(rules, "g", "v", end, 2, 2, ExpeditionGrade.GreatSuccess);
                Assert.That((record.Total, record.Wait, record.Taste, record.Expedition), Is.EqualTo((-4, 0, 0, 0)), end.ToString());
                Assert.That(record.End, Is.EqualTo(end));
            }
        }

        // ── 2.2 하루 정산 ──

        [Test]
        public void SettleDayKeepsOneRecordPerEndedVisitInArrivalOrder()
        {
            var roster = Roster(4);
            Dispatch(roster, "v1");
            SendHome(roster, "v2");
            Assert.That(roster.TryEnd("v3", ServiceVisitEnd.WaitedOut), Is.True);
            // v4는 아직 입구에 있다.
            var results = new[] { Result("v1", ExpeditionGrade.Success), Result("v1", ExpeditionGrade.GreatFailure) };

            var records = AffinityJudge.SettleDay(Rules(), roster.Visits.Concat(roster.Visits), results, 0, out string error);
            Assert.That(error, Is.Null);
            Assert.That(records.Select(r => r.VisitId), Is.EqualTo(new[] { "v1", "v2", "v3" }), "중복 입력·끝나지 않은 방문은 기록하지 않는다");
            Assert.That(records[0].GuestId, Is.EqualTo("g1"));
            Assert.That((records[0].Wait, records[0].Expedition, records[0].Total), Is.EqualTo((2, 1, 3)), "결과가 겹치면 처음 등급을 쓴다");
            Assert.That(records.Select(r => r.Total), Is.EqualTo(new[] { 3, -4, -4 }));
        }

        [Test]
        public void ADispatchedVisitWithoutAResultIsLeftOutAndReported()
        {
            var roster = Roster(2);
            Dispatch(roster, "v1");
            Dispatch(roster, "v2");
            var records = AffinityJudge.SettleDay(Rules(), roster.Visits, new[] { Result("v2", ExpeditionGrade.GreatSuccess) }, 0, out string error);
            Assert.That(records.Single().VisitId, Is.EqualTo("v2"));
            Assert.That(error, Does.Contain("v1"));
        }

        [Test]
        public void TasteScoreIsPassedByTheCaller()
        {
            var roster = Roster(1);
            Dispatch(roster, "v1");
            var record = AffinityJudge.SettleDay(Rules(), roster.Visits, new[] { Result("v1", ExpeditionGrade.Success) }, 2, out _).Single();
            Assert.That((record.Taste, record.Total), Is.EqualTo((2, 5)));
        }

        // ── 2.3 집계와 추가 인원 ──

        [TestCase(3, 10, 0)]
        [TestCase(3, 11, 1)]
        [TestCase(3, 13, 1)]
        [TestCase(3, 14, 2)]
        [TestCase(3, 16, 3)]
        [TestCase(3, 18, 4)]
        [TestCase(4, 14, 0)]
        [TestCase(4, 15, 1)]
        [TestCase(4, 20, 2)]
        [TestCase(4, 24, 4)]
        [TestCase(3, -8, 0)]
        [TestCase(0, 18, 0)]
        public void ExtraGuestsCountReachedThresholdsWithIntegerCeiling(int count, int sum, int expected)
        {
            Assert.That(AffinityJudge.ExtraGuests(Rules(), sum, count), Is.EqualTo(expected));
        }

        [Test]
        public void TallyCountsWaitedOutAndFollowsTheSentHomeSwitch()
        {
            var dispatched = Enumerable.Range(0, 3).Select(_ => Record(ServiceVisitEnd.Dispatched, 6)).ToList();
            Assert.That(AffinityJudge.Tally(Rules(), dispatched.Append(Record(ServiceVisitEnd.WaitedOut, -4))), Is.EqualTo((14, 4)));

            var withSentHome = dispatched.Append(Record(ServiceVisitEnd.SentHome, -4)).ToList();
            Assert.That(AffinityJudge.Tally(Rules(), withSentHome), Is.EqualTo((14, 4)), "기본: 돌려보낸 방문도 N에 1명·S에 −4로 넣는다 (2026-11-25 확정)");
            Assert.That(AffinityJudge.Tally(Rules("{\"countSentHome\":false}"), withSentHome), Is.EqualTo((18, 3)), "끄면 뺀다");
        }

        // ── 2.4 기준값 검사 ──

        [Test]
        public void ValidateRejectsBadMaximumAndThresholds()
        {
            Assert.That(Rules().Validate(), Is.Null, "💡 시험값");
            foreach (string json in new[] { "{\"visitMaxPoints\":0}", "{\"thresholdPercents\":[0,75]}",
                         "{\"thresholdPercents\":[60,101]}", "{\"thresholdPercents\":[75,60]}",
                         "{\"affinityCap\":0}", "{\"injuryRestDays\":-1}" })
                Assert.That(Rules(json).Validate(), Is.Not.Null, json);
        }
    }
}
