using System.Collections.Generic;
using DOB.Define;
using NUnit.Framework;
using UnityEngine;

namespace DOB.Tests
{
    public sealed class GuestArrivalPoolTests
    {
        [Test]
        public void FirstVisitsUseDifferentNamesAndTraitsWithoutLaterDayConditions()
        {
            var pool = Resources.Load<GuestArrivalPool>(GuestArrivalPool.ResourceName);
            Assert.That(pool, Is.Not.Null);
            var random = new System.Random(42);
            var names = new HashSet<string>();
            var races = new HashSet<string>();
            var jobs = new HashSet<string>();
            for (int i = 0; i < 100; i++)
            {
                var guest = pool.DrawIdentity("g" + i, 1, random);
                names.Add(guest.Name);
                races.Add(guest.Race);
                jobs.Add(guest.Job);
                Assert.That(pool.DrawTired(1, random), Is.False);
                Assert.That(guest.Weakness, Is.Null);
            }

            Assert.That(names.Count, Is.GreaterThan(1));
            Assert.That(races.Count, Is.GreaterThan(1));
            Assert.That(jobs.Count, Is.GreaterThan(1));
        }

        [Test]
        public void IdentitiesAvoidExcludedNamesUntilNoCandidateIsLeftAndTirednessIsDrawnPerVisit()
        {
            var pool = Resources.Load<GuestArrivalPool>(GuestArrivalPool.ResourceName);
            var random = new System.Random(7);
            var excluded = new HashSet<string>();
            for (int i = 0; i < 40; i++)
            {
                var guest = pool.DrawIdentity("g" + i, 1, random, excluded);
                Assert.That(excluded.Add(guest.Name), Is.True, "제외한 이름은 다시 나오지 않는다: " + guest.Name);
                Assert.That((guest.GuestId, guest.Affinity, guest.Rejected, guest.Weakness), Is.EqualTo(("g" + i, 0, false, (string)null)));
            }

            var all = new HashSet<string>();
            for (int i = 0; i < 2000; i++)
                all.Add(pool.DrawIdentity("x", 1, random).Name);
            Assert.That(pool.DrawIdentity("last", 1, random, all).Name, Is.Not.Empty, "후보를 다 쓰면 전체 이름을 다시 허용");

            Assert.That(pool.DrawTired(1, random), Is.False, "첫날에는 피로가 없다");
            int tired = 0;
            for (int i = 0; i < 400; i++)
                if (pool.DrawTired(2, random)) tired++;
            Assert.That(tired, Is.InRange(1, 399), "2일차부터 방문마다 추첨한다");
        }
    }
}
