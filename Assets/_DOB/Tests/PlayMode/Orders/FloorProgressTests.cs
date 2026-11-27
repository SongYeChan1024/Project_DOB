// FloorProgressTests — 최심층 성공 두 번이면 다음 층을 열고, 보고마다 그 층의 미확인 위험을 최대 두 축 공개하는지 검증한다 (floor-progress 명세).
// 의사코드: 시험 기준(개방 2, 공개 2, 1층 독·열·체력, 2층 어둠·함정)을 만들어 명세 시나리오를 그대로 따라간다.
using System.Linq;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;
using UnityEngine;

namespace DOB.Tests
{
    public sealed class FloorProgressTests
    {
        private FloorProgressRules rules;

        [SetUp]
        public void SetUp()
        {
            rules = ScriptableObject.CreateInstance<FloorProgressRules>();
            typeof(FloorProgressRules).GetField("floors", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(rules, new[]
                {
                    new FloorEntry { name = "1층", axes = new[] { "독", "열", "체력" } },
                    new FloorEntry { name = "2층", axes = new[] { "어둠", "함정" } }
                });
        }

        [TearDown]
        public void TearDown() => Object.DestroyImmediate(rules);

        // 축 순서: 독·열·냉기·어둠·체력·함정
        private static VisitResult Report(string visit, int floor, ExpeditionGrade grade, params int[] shortfalls) =>
            new VisitResult("g-" + visit, visit, "손님", floor, "구운 빵", false,
                shortfalls.Length == 6 ? shortfalls : new int[6], grade, new string[0]);

        [Test]
        public void TwoSuccessesOnTheFirstFloorOpenTheSecond()
        {
            var progress = new FloorProgress(rules);
            Assert.That(progress.Apply(Report("v1", 1, ExpeditionGrade.Success)).OpenedFloor, Is.Null);
            Assert.That(progress.Apply(Report("v2", 1, ExpeditionGrade.GreatSuccess)).OpenedFloor, Is.EqualTo(2));
            Assert.That((progress.OpenFloors, progress.DeepestWins), Is.EqualTo((2, 0)));
        }

        [Test]
        public void ShallowFloorsAndFailuresDoNotCount()
        {
            var progress = new FloorProgress(rules, new FloorProgressState(2, 1, null));
            progress.Apply(Report("v1", 1, ExpeditionGrade.GreatSuccess));
            Assert.That(progress.DeepestWins, Is.EqualTo(1), "얕은 층 실적");
            progress.Apply(Report("v2", 2, ExpeditionGrade.Failure));
            progress.Apply(Report("v3", 2, ExpeditionGrade.GreatFailure));
            Assert.That((progress.OpenFloors, progress.DeepestWins), Is.EqualTo((2, 1)), "실패·대실패");
            Assert.That(progress.Apply(Report("v4", 2, ExpeditionGrade.Success)).OpenedFloor, Is.Null, "마지막 층에서는 더 열지 않는다");
            Assert.That(progress.OpenFloors, Is.EqualTo(2));
        }

        [Test]
        public void ConfirmingTheSameReportAgainChangesNothing()
        {
            var progress = new FloorProgress(rules);
            var first = progress.Apply(Report("v1", 1, ExpeditionGrade.Success));
            var again = progress.Apply(Report("v1", 1, ExpeditionGrade.Success));
            Assert.That(again, Is.SameAs(first));
            Assert.That(progress.DeepestWins, Is.EqualTo(1));
            Assert.That(progress.Snapshot().Revealed.Single().Axes.Count, Is.EqualTo(2));
        }

        [Test]
        public void ShortAxesAreRevealedFirstThenTheRestInAxisOrder()
        {
            var progress = new FloorProgress(rules);
            var outcome = progress.Apply(Report("v1", 1, ExpeditionGrade.Failure, 1, 1, 0, 0, 0, 0));
            Assert.That(outcome.Revealed, Is.EqualTo(new[] { "독", "열" }), "부족했던 독·열부터");
            Assert.That(progress.IsRevealed(1, "체력"), Is.False);
            Assert.That(progress.Apply(Report("v2", 1, ExpeditionGrade.Success)).Revealed, Is.EqualTo(new[] { "체력" }), "남은 미확인 축으로 채움");
            Assert.That(progress.Apply(Report("v3", 1, ExpeditionGrade.Success)).Revealed, Is.Empty, "모두 공개된 뒤");
        }

        [Test]
        public void TheStateRoundTripsThroughASnapshot()
        {
            var progress = new FloorProgress(rules);
            progress.Apply(Report("v1", 1, ExpeditionGrade.Success, 0, 0, 0, 0, 1, 0));
            var copy = new FloorProgress(rules, progress.Snapshot());
            Assert.That((copy.OpenFloors, copy.DeepestWins), Is.EqualTo((1, 1)));
            Assert.That(copy.IsRevealed(1, "체력") && copy.IsRevealed(1, "독") && !copy.IsRevealed(1, "열"), Is.True);
            Assert.That(new FloorProgressState(0, 0, null).Validate(), Is.False);
            Assert.That(new FloorProgressState(1, 0, new[] { new FloorReveal(1, new[] { "바람" }) }).Validate(), Is.False);
        }
    }
}
