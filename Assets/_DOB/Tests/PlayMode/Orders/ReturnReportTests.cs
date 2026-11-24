using System;
using System.Collections.Generic;
using System.Linq;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;

namespace DOB.Tests
{
    /// <summary>방문 결과 확정 → 귀환 보고 → 던전 재료 재고의 순수 C# 규칙.</summary>
    public sealed class ReturnReportTests
    {
        private static readonly string[] Forest = { "슬라임 젤리", "숲버섯", "들풀", "숲 당근" };
        private static readonly int[] LootByGrade = { 3, 2, 1, 0 };

        private static VisitResultLog NewLog(int seed = 7) =>
            new VisitResultLog(grade => LootByGrade[(int)grade], floor => floor == 1 ? Forest : Array.Empty<string>(), new Random(seed));

        // 독 축에만 부족을 넣어 부족 합을 정한다.
        private static int[] Short(int total) => new[] { total, 0, 0, 0, 0, 0 };

        private static VisitResult Record(VisitResultLog log, string visit, int shortfall, int floor = 1)
        {
            Assert.That(log.TryRecord("guest-" + visit, visit, "하루", floor, "구운 빵", true, Short(shortfall), out var result), Is.True);
            return result;
        }

        [Test]
        public void AServedVisitIsRecordedOnceWithItsGradeAndShortfalls()
        {
            var log = NewLog();
            var result = Record(log, "v1", 1);
            Assert.That(result.Grade, Is.EqualTo(ExpeditionGrade.Success));
            Assert.That(result.Shortfalls, Is.EqualTo(Short(1)));
            Assert.That(result.Floor, Is.EqualTo(1));
            Assert.That(result.Dish, Is.EqualTo("구운 빵"));
            Assert.That(result.UsedSeasoning, Is.True);

            Assert.That(log.TryRecord("guest-v1", "v1", "하루", 1, "구운 빵", true, Short(0), out _), Is.False);
            Assert.That(log.Results.Count, Is.EqualTo(1));
            Assert.That(log.Results[0].Grade, Is.EqualTo(ExpeditionGrade.Success));
        }

        [Test]
        public void InvalidRecordsChangeNothing()
        {
            var log = NewLog();
            Assert.That(log.TryRecord("", "v1", "하루", 1, "구운 빵", false, Short(0), out _), Is.False);
            Assert.That(log.TryRecord("g", "", "하루", 1, "구운 빵", false, Short(0), out _), Is.False);
            Assert.That(log.TryRecord("g", "v1", "하루", 1, "구운 빵", false, new[] { 0, 0 }, out _), Is.False);
            Assert.That(log.TryRecord("g", "v1", "하루", 1, "구운 빵", false, new[] { -1, 0, 0, 0, 0, 0 }, out _), Is.False);
            Assert.That(log.Results, Is.Empty);
        }

        [TestCase(0, 3)]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        [TestCase(3, 0)]
        public void LootCountFollowsTheGradeAndComesFromTheFloor(int shortfall, int expectedCount)
        {
            var result = Record(NewLog(), "v1", shortfall);
            Assert.That(result.Loot.Count, Is.EqualTo(expectedCount));
            Assert.That(result.Loot, Is.SubsetOf(Forest));
        }

        [Test]
        public void LootIsDrawnOnceAndAFloorWithoutMaterialsGivesNone()
        {
            var log = NewLog(3);
            var first = Record(log, "v1", 0);
            var snapshot = first.Loot.ToArray();
            Record(log, "other", 0);
            Assert.That(log.Results[0].Loot, Is.EqualTo(snapshot), "다른 방문을 기록해도 이미 뽑은 전리품은 그대로다");
            Assert.That(Record(NewLog(3), "v1", 0).Loot, Is.EqualTo(snapshot), "같은 난수에서는 같은 전리품");
            Assert.That(Record(NewLog(), "v2", 0, floor: 9).Loot, Is.Empty);
        }

        [Test]
        public void StockCountsMaterialsAndRejectsInvalidAmounts()
        {
            var stock = new MaterialStock(new[] { new MaterialCount("숲버섯", 1) });
            Assert.That(stock.Count("들풀"), Is.Zero);
            Assert.That(stock.TryAdd("숲버섯", 2), Is.True);
            Assert.That(stock.Count("숲버섯"), Is.EqualTo(3));
            Assert.That(stock.TryAdd("숲버섯", 0), Is.False);
            Assert.That(stock.TryAdd("숲버섯", -1), Is.False);
            Assert.That(stock.TryAdd("", 1), Is.False);
            Assert.That(stock.Count("숲버섯"), Is.EqualTo(3));
            Assert.That(stock.All().Single().Count, Is.EqualTo(3));
            Assert.Throws<ArgumentException>(() => new MaterialStock(new[] { new MaterialCount("들풀", -1) }));
        }

        [Test]
        public void ConfirmingAReportAddsItsLootExactlyOnce()
        {
            var log = NewLog();
            var report = Record(log, "v1", 1);
            var stock = new MaterialStock();
            var reports = new ReturnReportLog(log.Results, stock);

            Assert.That(reports.Reports.Single().VisitId, Is.EqualTo("v1"));
            Assert.That(reports.Reports.Single().VisitId, Is.EqualTo("v1"), "조회는 상태를 바꾸지 않는다");
            Assert.That(stock.All(), Is.Empty);
            Assert.That(reports.IsConfirmed("v1"), Is.False);

            Assert.That(reports.Confirm("v1"), Is.EqualTo(ReportConfirmResult.Confirmed));
            Assert.That(reports.IsConfirmed("v1"), Is.True);
            Assert.That(Total(stock), Is.EqualTo(2));
            foreach (var group in report.Loot.GroupBy(id => id))
                Assert.That(stock.Count(group.Key), Is.EqualTo(group.Count()));

            Assert.That(reports.Confirm("v1"), Is.EqualTo(ReportConfirmResult.AlreadyConfirmed));
            Assert.That(reports.Confirm("missing"), Is.EqualTo(ReportConfirmResult.NotFound));
            Assert.That(reports.Confirm(null), Is.EqualTo(ReportConfirmResult.NotFound));
            Assert.That(Total(stock), Is.EqualTo(2));
        }

        [Test]
        public void ConfirmAllPaysOnlyTheReportsStillPending()
        {
            var log = NewLog();
            Record(log, "v1", 0);
            Record(log, "v2", 2);
            var stock = new MaterialStock();
            var reports = new ReturnReportLog(log.Results, stock);

            Assert.That(reports.Confirm("v1"), Is.EqualTo(ReportConfirmResult.Confirmed));
            Assert.That(reports.ConfirmAll(), Is.EqualTo(1));
            Assert.That(reports.ConfirmAll(), Is.Zero);
            Assert.That(Total(stock), Is.EqualTo(3 + 1));
            Assert.That(reports.Reports.Select(r => r.VisitId), Is.EqualTo(new[] { "v1", "v2" }));
        }

        [Test]
        public void ADayWithoutDispatchedGuestsHasNoReports()
        {
            var stock = new MaterialStock();
            var reports = new ReturnReportLog(null, stock);
            Assert.That(reports.Reports, Is.Empty);
            Assert.That(reports.ConfirmAll(), Is.Zero);
            Assert.That(stock.All(), Is.Empty);
        }

        private static int Total(MaterialStock stock) => stock.All().Sum(entry => entry.Count);
    }
}
