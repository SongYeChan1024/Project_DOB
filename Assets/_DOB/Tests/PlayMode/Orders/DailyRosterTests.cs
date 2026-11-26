// DailyRosterTests — 그날 명단(daily-roster) 명세의 인원 계산·최소 인원·이름 중복 제외·입장 순서·시드 재현을 검증한다.
// 의사코드: JsonUtility로 💡 시험 기준(1일차 3명, 최소 1명)과 작은 이름 후보를 만들고, 시드가 정해진 난수로 명단을 만든다.
using System;
using System.Collections.Generic;
using System.Linq;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;
using UnityEngine;

namespace DOB.Tests
{
    public sealed class DailyRosterTests
    {
        private readonly List<ScriptableObject> created = new List<ScriptableObject>();

        [TearDown]
        public void TearDown()
        {
            foreach (var asset in created)
                UnityEngine.Object.DestroyImmediate(asset);
            created.Clear();
        }

        private ServiceRosterSettings Settings(int first = 3, int min = 1)
        {
            var settings = ScriptableObject.CreateInstance<ServiceRosterSettings>();
            JsonUtility.FromJsonOverwrite($"{{\"baseGuests\":{first},\"minGuests\":{min}}}", settings);
            created.Add(settings);
            return settings;
        }

        private GuestArrivalPool Pool(params string[] names)
        {
            if (names.Length == 0)
                names = Enumerable.Range(1, 30).Select(i => "이름" + i).ToArray();
            var pool = ScriptableObject.CreateInstance<GuestArrivalPool>();
            JsonUtility.FromJsonOverwrite("{\"names\":[" + string.Join(",", names.Select(n => "\"" + n + "\"")) +
                "],\"races\":[\"인간\",\"수인\"],\"jobs\":[\"전사\",\"사제\"],\"weaknessAxes\":[\"독\"],\"tiredChance\":0.3,\"weaknessChance\":0.3}", pool);
            created.Add(pool);
            return pool;
        }

        private static GuestRecord Returning(string id, string name = null) =>
            new GuestRecord(id, name ?? "단골_" + id, "수인", "사제", null, 3, false, 2);

        private List<GuestRecord> Compose(int day, IReadOnlyList<GuestRecord> eligible, int bonus, int seed = 1,
            ServiceRosterSettings settings = null, GuestArrivalPool pool = null, IEnumerable<string> taken = null) =>
            DailyRoster.Compose(settings ?? Settings(), pool ?? Pool(), day, eligible, taken, bonus, new System.Random(seed));

        [Test]
        public void TheCountIsReturningGuestsPlusWordOfMouthNewGuests()
        {
            var eligible = new[] { Returning("a"), Returning("b"), Returning("c") };
            var roster = Compose(2, eligible, 1);
            Assert.That(roster.Count, Is.EqualTo(4));
            CollectionAssert.IsSubsetOf(new[] { "a", "b", "c" }, roster.Select(g => g.GuestId));
            Assert.That(Compose(5, eligible, 0).Count, Is.EqualTo(3), "일차만 지나서는 늘지 않는다");
            Assert.That(Compose(2, new[] { Returning("a"), Returning("a") }, 0).Count, Is.EqualTo(1), "같은 손님은 한 번만 센다");
        }

        [Test]
        public void TheFirstDayIsAllNewGuestsAndIgnoresWordOfMouth()
        {
            var roster = Compose(1, new GuestRecord[0], 4);
            Assert.That(roster.Count, Is.EqualTo(3));
            Assert.That(roster.All(g => g.Affinity == 0 && !g.Rejected && g.Weakness == null), Is.True, "첫날 신규는 취약점이 없다");
        }

        [Test]
        public void AbsencesAreNotRefilledButTheMinimumIsKept()
        {
            // 전날 네 명 가운데 거절 1·휴식 1이면 올 수 있는 손님은 2명이다.
            Assert.That(Compose(2, new[] { Returning("a"), Returning("b") }, 0).Count, Is.EqualTo(2), "결석 자리는 채우지 않는다");
            var lonely = Compose(2, new GuestRecord[0], 0);
            Assert.That(lonely.Count, Is.EqualTo(1), "전원 거절된 다음 날은 최소 인원");
            Assert.That(lonely[0].Affinity, Is.Zero);
            Assert.That(Compose(2, new GuestRecord[0], 0, settings: Settings(min: 0)).Count, Is.Zero, "최소 인원을 끄면 0명");
            Assert.That(Compose(2, new[] { Returning("a"), Returning("b") }, 1).Count, Is.EqualTo(3), "이미 충분하면 더하지 않는다");
        }

        [Test]
        public void NewNamesAvoidTheLedgerAndTheSameDayUntilTheCandidatesRunOut()
        {
            var pool = Pool("하루", "보리", "솔", "다온");
            for (int seed = 1; seed <= 20; seed++)
            {
                var roster = Compose(2, new[] { Returning("a", "하루") }, 2, seed, pool: pool, taken: new[] { "보리" });
                var fresh = roster.Where(g => g.GuestId != "a").Select(g => g.Name).ToArray();
                CollectionAssert.AreEquivalent(new[] { "솔", "다온" }, fresh, "명부·재방문·같은 날 이름을 피한다 (시드 " + seed + ")");
            }
            var exhausted = Compose(2, new GuestRecord[0], 1, pool: Pool("하루", "보리"), taken: new[] { "하루", "보리" });
            CollectionAssert.Contains(new[] { "하루", "보리" }, exhausted.Single().Name, "후보를 다 쓰면 전체 이름을 다시 허용");
        }

        [Test]
        public void TheSameSeedGivesTheSameRosterAndReturningGuestsAreShuffledIn()
        {
            var eligible = new[] { Returning("a"), Returning("b"), Returning("c") };
            string Key(List<GuestRecord> roster) => string.Join("|", roster.Select(g => g.GuestId + ":" + g.Name + ":" + g.Race + ":" + g.Job));
            Assert.That(Key(Compose(2, eligible, 2, 7)), Is.EqualTo(Key(Compose(2, eligible, 2, 7))), "같은 시드면 같은 신원·같은 순서");

            bool newGuestCameFirst = false;
            for (int seed = 1; seed <= 30; seed++)
            {
                var roster = Compose(2, eligible, 2, seed);
                Assert.That(roster.Select(g => g.GuestId).Distinct().Count(), Is.EqualTo(5), "손님 ID가 겹치지 않는다");
                newGuestCameFirst |= !eligible.Any(g => g.GuestId == roster[0].GuestId);
            }
            Assert.That(newGuestCameFirst, Is.True, "재방문 손님이 항상 앞에만 오지는 않는다");
        }

        [Test]
        public void BadInputsAreRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Compose(0, null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => Compose(2, null, -1));
            Assert.Throws<ArgumentNullException>(() => DailyRoster.Compose(Settings(), Pool(), 2, null, null, 0, null));
        }
    }
}
