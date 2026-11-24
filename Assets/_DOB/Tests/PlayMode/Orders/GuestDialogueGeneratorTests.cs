using System;
using System.Linq;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

namespace DOB.Tests
{
    public sealed class GuestDialogueGeneratorTests
    {
        private GuestDialogueRules rules;
        private GuestVisitInput input;

        // 원고 후보의 처음/끝을 고정해 사전 표시와 실제 선택 문장을 함께 비교한다.
        private sealed class EdgeRandom : Random
        {
            private readonly bool last;
            public EdgeRandom(bool last = false) => this.last = last;
            public override int Next(int maxValue) => last ? maxValue - 1 : 0;
        }

        [SetUp]
        public void SetUp()
        {
            rules = Resources.Load<GuestDialogueRules>("SO_GuestDialogueRules");
            Assert.That(rules, Is.Not.Null);
            input = new GuestVisitInput { GuestId = "g1", VisitId = "v1", GuestName = "시험 손님",
                Day = 1, Floor = 1, Race = "수인", Job = "사제" };
        }

        private GuestDialogueVisit Generate(bool last = false) =>
            GuestDialogueGenerator.Generate(rules, input, new EdgeRandom(last));

        [Test]
        public void ShippedTableHas37DistinctCausesAndAllPositiveCausesHaveClickableLines()
        {
            Assert.That(rules.Causes.Count, Is.EqualTo(37));
            Assert.That(rules.Causes.Select(r => r.causeId).Distinct().Count(), Is.EqualTo(37));
            Assert.That(rules.Causes.Select(r => r.axis).Distinct().Count(), Is.EqualTo(6));
            foreach (var r in rules.Causes.Where(r => r.delta > 0))
                Assert.That(r.line, Does.Contain("[["), r.causeId);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void AliasesAndSignedCausesAreIndependentOfDiscoveryOrder(bool blessingFirst)
        {
            var visit = Generate();
            var slime = visit.Clues.Single(c => c.Text == "슬라임 점액");
            var sting = visit.Clues.Single(c => c.Text == "따끔");
            var nose = visit.Clues.Single(c => c.CauseId == "race.beastfolk.poison");
            var blessing = visit.Clues.Single(c => c.CauseId == "job.priest.poison");
            Assert.That(visit.ActualRisk("독"), Is.EqualTo(1));
            Assert.That(visit.NotebookRisk("독"), Is.Null);
            if (blessingFirst)
            {
                Assert.That(visit.Discover(blessing.Key), Is.True);
                Assert.That(visit.NotebookRisk("독"), Is.EqualTo(0));
            }
            Assert.That(visit.Discover(slime.Key), Is.True);
            Assert.That(visit.Discover(slime.Key), Is.False);
            Assert.That(visit.Discover(sting.Key), Is.True);
            Assert.That(visit.IsDiscovered(slime.Key) && visit.IsDiscovered(sting.Key), Is.True);
            Assert.That(visit.NotebookRisk("독"), Is.EqualTo(blessingFirst ? 0 : 1));
            Assert.That(visit.Discover(nose.Key), Is.True);
            Assert.That(visit.NotebookRisk("독"), Is.EqualTo(blessingFirst ? 1 : 2));
            if (!blessingFirst) visit.Discover(blessing.Key);
            Assert.That(visit.NotebookRisk("독"), Is.EqualTo(1));
            Assert.That(visit.ActualRisk("독"), Is.EqualTo(1));
            Assert.That(visit.Discover("다른 방문 원인"), Is.False);
        }

        [Test]
        public void VisitKeepsItsChosenWordsAndDoesNotLeakDiscoveriesIntoAnotherVisit()
        {
            input.Race = "인간";
            input.Job = "전사";
            var first = Generate();
            first.Discover(first.Clues.First(c => c.Text == "슬라임 점액").Key);
            input.VisitId = "v2";
            var second = Generate(last: true);
            Assert.That(first.VisitId, Is.EqualTo("v1"));
            Assert.That(second.VisitId, Is.EqualTo("v2"));
            Assert.That(first.Line, Does.Contain("[[슬라임 점액]]"));
            Assert.That(second.Line, Is.Not.EqualTo(first.Line));
            Assert.That(second.NotebookRisk("독"), Is.Null);
            foreach (var clue in second.Clues.Where(c => c.Channel == DialogueClueChannel.Dialogue))
                Assert.That(second.Line, Does.Contain("[[" + clue.Text + "]]"));
            Assert.That(second.Clues.Any(c => c.Text == "슬라임 점액"), Is.False);
        }

        [Test]
        public void TiesCanChooseEitherHighestAxisAndRegularCanGetASecondCause()
        {
            Assert.That(Generate().Line, Does.Contain("점액"));
            Assert.That(Generate(last: true).Clues.Any(c => c.Channel == DialogueClueChannel.Dialogue && c.Axis == "체력"), Is.True);
            input.Affinity = rules.SecondClueAffinity - 1;
            Assert.That(Generate().Clues.Where(c => c.Channel == DialogueClueChannel.Dialogue)
                .Select(c => c.CauseId).Distinct().Count(), Is.EqualTo(1));
            input.Affinity++;
            var clues = Generate().Clues.Where(c => c.Channel == DialogueClueChannel.Dialogue).ToArray();
            Assert.That(clues.Select(c => c.Axis).Distinct().Count(), Is.EqualTo(2));
            input.Job = "전사"; // 체력이 0인 수인에게는 독의 서로 다른 두 원인을 제시한다.
            clues = Generate().Clues.Where(c => c.Channel == DialogueClueChannel.Dialogue).ToArray();
            Assert.That(clues.Select(c => c.CauseId).Distinct().Count(), Is.EqualTo(2));
            Assert.That(clues.All(c => c.Axis == "독"), Is.True);
        }

        [Test]
        public void DayAndFloorGateNewsFatigueAndWeakness()
        {
            input.EventAxis = "열";
            input.EventFloor = 2;
            input.IsTired = true;
            input.PersonalWeakness = "열";
            input.Race = "엘프";
            var firstDay = Generate();
            Assert.That(firstDay.ActualRisk("열"), Is.EqualTo(1));
            Assert.That(firstDay.ActualRisk("체력"), Is.EqualTo(1));
            input.Day = 2;
            var otherFloor = Generate();
            Assert.That(otherFloor.ActualRisk("열"), Is.EqualTo(2));
            Assert.That(otherFloor.Clues.Any(c => c.Channel == DialogueClueChannel.News), Is.False);
            input.Floor = 2;
            var visit = Generate();
            Assert.That(visit.ActualRisk("열"), Is.EqualTo(3));
            var spoken = visit.Clues.First(c => c.Channel == DialogueClueChannel.Dialogue);
            var news = visit.Clues.Single(c => c.Channel == DialogueClueChannel.News);
            Assert.That(spoken.Axis, Is.EqualTo("열")); // 최고 축은 2층의 기본 위험 바깥에도 있을 수 있다.
            Assert.That(spoken.CauseId, Is.EqualTo(news.CauseId));
            visit.Discover(news.Key);
            visit.Discover(spoken.Key);
            Assert.That(visit.NotebookRisk("열"), Is.EqualTo(1));
            Assert.That(visit.ActualRisk("체력"), Is.EqualTo(1));
        }

        [Test]
        public void BreadTasteAndDepartureUseTheirOwnInputs()
        {
            var visit = Generate();
            Assert.That(visit.TasteLine(true), Is.EqualTo("고소한 빵에 마늘 향이 나네."));
            Assert.That(visit.TasteLine(false), Is.EqualTo("따뜻한 빵이 담백하네."));
            Assert.That(visit.DepartureLine(0), Is.EqualTo("이 정도면 끝까지 버틸 수 있겠어."));
            Assert.That(visit.DepartureLine(1), Is.EqualTo("한 군데만 더 조심하면 되겠어."));
            Assert.That(visit.DepartureLine(2), Is.EqualTo("내려가는 길에 조심할 게 많겠어."));
            Assert.That(visit.DepartureLine(5), Is.EqualTo(visit.DepartureLine(2)));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void NormalGuestsAlwaysSpeakAboutAHighestRiskAcrossFloors(int floor)
        {
            input.Day = 2;
            input.Floor = floor;
            input.EventFloor = floor;
            input.EventAxis = "함정";
            input.PersonalWeakness = "열";
            input.IsTired = true;
            var axes = rules.Causes.Select(r => r.axis).Distinct().ToArray();
            var races = rules.Causes.Where(r => r.condition == DialogueCondition.Race).Select(r => r.qualifier)
                .Concat(new[] { "인간" }).ToArray();
            var jobs = rules.Causes.Where(r => r.condition == DialogueCondition.Job).Select(r => r.qualifier);
            var random = new Random(floor);
            foreach (string race in races)
            foreach (string job in jobs)
            {
                input.Race = race;
                input.Job = job;
                var visit = GuestDialogueGenerator.Generate(rules, input, random);
                int largest = axes.Max(visit.ActualRisk);
                var spoken = visit.Clues.Where(c => c.Channel == DialogueClueChannel.Dialogue).ToArray();
                Assert.That(spoken.Length, Is.GreaterThan(0));
                Assert.That(spoken.All(c => visit.ActualRisk(c.Axis) == largest), Is.True, race + " " + job);
            }
        }

        [Test]
        public void AllZeroRiskUsesNeutralLineWithoutDialogueLinks()
        {
            // 시험용 단일 층에서 직업 보정으로 위험이 모두 사라지는 경우를 재현한다.
            var fixture = ScriptableObject.CreateInstance<GuestDialogueRules>();
            try
            {
                JsonUtility.FromJsonOverwrite(@"{""calmLine"":""별일 없을 거야"",""secondClueAffinity"":21,""causes"":[
                    {""condition"":0,""causeId"":""floor.test"",""floor"":1,""axis"":""독"",""delta"":1,""line"":""[[점액]]""},
                    {""condition"":3,""causeId"":""job.priest"",""qualifier"":""사제"",""axis"":""독"",""delta"":-1}]}", fixture);
                var visit = GuestDialogueGenerator.Generate(fixture, input, new Random(1));
                Assert.That(visit.Line, Is.EqualTo(fixture.CalmLine));
                Assert.That(visit.ActualRisk("독"), Is.Zero);
                Assert.That(visit.Clues.Any(c => c.Channel == DialogueClueChannel.Dialogue), Is.False);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(fixture);
            }
        }
    }
}
