using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;

namespace DOB.Tests
{
    public sealed class ExpeditionJudgeTests
    {
        private static int[] Judge(Dictionary<string, int> risk, Dictionary<string, int> protection) =>
            ExpeditionJudge.Shortfalls(axis => risk.TryGetValue(axis, out int n) ? n : 0,
                axis => protection.TryGetValue(axis, out int n) ? n : 0);

        private static int At(int[] shortfalls, string axis) => shortfalls[new List<string>(RiskAxes.All).IndexOf(axis)];

        [Test]
        public void EachAxisIsJudgedOnItsOwnAndSurplusDoesNotCoverAnotherAxis()
        {
            var equal = Judge(new Dictionary<string, int> { ["독"] = 1 }, new Dictionary<string, int> { ["독"] = 1 });
            Assert.That(At(equal, "독"), Is.Zero);

            var smaller = Judge(new Dictionary<string, int> { ["독"] = 2 }, new Dictionary<string, int> { ["독"] = 1 });
            Assert.That(At(smaller, "독"), Is.EqualTo(1));

            var surplus = Judge(new Dictionary<string, int> { ["함정"] = 1 }, new Dictionary<string, int> { ["체력"] = 2 });
            Assert.That(At(surplus, "체력"), Is.Zero);
            Assert.That(At(surplus, "함정"), Is.EqualTo(1));

            var uncovered = Judge(new Dictionary<string, int> { ["열"] = 1 }, new Dictionary<string, int>());
            Assert.That(At(uncovered, "열"), Is.EqualTo(1));
            Assert.That(ExpeditionJudge.Total(uncovered), Is.EqualTo(1));
            Assert.That(uncovered.Length, Is.EqualTo(6));
        }

        [TestCase(0, ExpeditionGrade.GreatSuccess)]
        [TestCase(1, ExpeditionGrade.Success)]
        [TestCase(2, ExpeditionGrade.Failure)]
        [TestCase(3, ExpeditionGrade.GreatFailure)]
        [TestCase(4, ExpeditionGrade.GreatFailure)]
        public void TotalShortfallDecidesTheGrade(int total, ExpeditionGrade expected) =>
            Assert.That(ExpeditionJudge.Grade(total), Is.EqualTo(expected));
    }
}
