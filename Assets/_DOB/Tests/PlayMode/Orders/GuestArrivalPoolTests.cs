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
                var guest = pool.Draw(1, random);
                names.Add(guest.GuestName);
                races.Add(guest.Race);
                jobs.Add(guest.Job);
                Assert.That(guest.IsTired, Is.False);
                Assert.That(guest.PersonalWeakness, Is.Null);
            }

            Assert.That(names.Count, Is.GreaterThan(1));
            Assert.That(races.Count, Is.GreaterThan(1));
            Assert.That(jobs.Count, Is.GreaterThan(1));
        }
    }
}
