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
        private const string Rare = "숲 송로";
        private const int GiftMin = 9;

        private static VisitResultLog NewLog() => new VisitResultLog();

        private static string FloorGift(int floor) => floor == 1 ? Rare : null;

        // 손님 ID별 정산 뒤 누적 호감으로 전리품을 정한 보고 목록. 명부에 없는 손님은 null.
        private static IReadOnlyList<VisitResult> Gifted(VisitResultLog log, int affinity, bool rejected = false, int count = 1) =>
            log.WithGifts(id => new GuestRecord(id, "하루", "인간", "전사", null, affinity, rejected), GiftMin, FloorGift, count);

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

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ServingRecordsNoLootYetWhateverTheGrade(int shortfall)
        {
            // 전리품 자격은 그날 정산 뒤 누적 호감으로 정하므로 서빙 시점에는 비어 있다.
            Assert.That(Record(NewLog(), "v1", shortfall).Loot, Is.Empty);
        }

        [TestCase(0)]
        [TestCase(3)]
        public void AGuestWithAffinityNineGetsOneRareMaterialOfTheFloorWhateverTheGrade(int shortfall)
        {
            var log = NewLog();
            Record(log, "v1", shortfall);
            Assert.That(Gifted(log, 9).Single().Loot, Is.EqualTo(new[] { Rare }), "정확히 9부터 자격, 부상 귀환도 받는다");
            Assert.That(Gifted(log, 36).Single().Loot, Is.EqualTo(new[] { Rare }));
            Assert.That(Gifted(log, 9, count: 2).Single().Loot, Is.EqualTo(new[] { Rare, Rare }), "개수는 기획 데이터를 따른다");
            Assert.That(log.Results.Single().Loot, Is.Empty, "기록 자체는 바꾸지 않는다");
        }

        [Test]
        public void NoGiftBelowNineForRejectedOrUnknownGuestsOrAFloorWithoutRareMaterial()
        {
            var log = NewLog();
            Record(log, "v1", 0);
            Assert.That(Gifted(log, 8).Single().Loot, Is.Empty, "8은 자격이 없다");
            Assert.That(Gifted(log, 0).Single().Loot, Is.Empty);
            Assert.That(Gifted(log, 9, rejected: true).Single().Loot, Is.Empty, "영구 거절된 방문의 전리품은 폐기한다");
            Assert.That(log.WithGifts(_ => null, GiftMin, FloorGift, 1).Single().Loot, Is.Empty, "명부에 없는 손님");
            var deep = NewLog();
            Record(deep, "v9", 0, floor: 9);
            Assert.That(Gifted(deep, 9).Single().Loot, Is.Empty, "희귀 재료가 등록되지 않은 층");
        }

        [Test]
        public void GiftsKeepTheRestOfTheResultAndRejectBadArguments()
        {
            var log = NewLog();
            int[] risks = { 2, 0, 0, 0, 1, 1 }, protections = { 1, 0, 0, 0, 1, 3 }, shortfalls = { 1, 0, 0, 0, 0, 0 };
            Assert.That(log.TryRecord("g1", "v1", "하루", 1, "구운 빵", true, shortfalls, risks, protections, out _), Is.True);
            var gifted = Gifted(log, 9).Single();
            Assert.That((gifted.GuestId, gifted.VisitId, gifted.GuestName, gifted.Floor, gifted.Dish, gifted.UsedSeasoning, gifted.Grade),
                Is.EqualTo(("g1", "v1", "하루", 1, "구운 빵", true, ExpeditionGrade.Success)));
            Assert.That(gifted.Shortfalls, Is.EqualTo(shortfalls));
            Assert.That(gifted.Risks, Is.EqualTo(risks));
            Assert.That(gifted.Protections, Is.EqualTo(protections));
            Assert.Throws<ArgumentNullException>(() => log.WithGifts(null, GiftMin, FloorGift, 1));
            Assert.Throws<ArgumentNullException>(() => log.WithGifts(_ => null, GiftMin, null, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => log.WithGifts(_ => null, GiftMin, FloorGift, -1));
        }

        [Test]
        public void AGiftGoesIntoStockOnceLikeAnyOtherLoot()
        {
            var log = NewLog();
            Record(log, "v1", 0);
            var stock = new MaterialStock();
            var reports = new ReturnReportLog(Gifted(log, 9), stock);
            Assert.That(reports.Confirm("v1"), Is.EqualTo(ReportConfirmResult.Confirmed));
            Assert.That(reports.Confirm("v1"), Is.EqualTo(ReportConfirmResult.AlreadyConfirmed));
            Assert.That(stock.Count(Rare), Is.EqualTo(1));
            Assert.That(Total(stock), Is.EqualTo(1));
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
        public void RisksAndProtectionsAreKeptForTheReportAndMustAgreeWithTheShortfalls()
        {
            var log = NewLog();
            int[] risks = { 2, 0, 0, 0, 1, 1 }, protections = { 1, 0, 0, 0, 1, 3 }, shortfalls = { 1, 0, 0, 0, 0, 0 };
            Assert.That(log.TryRecord("g1", "v1", "하루", 1, "구운 빵", true, shortfalls, risks, protections, out var result), Is.True);
            Assert.That(result.Risks, Is.EqualTo(risks));
            Assert.That(result.Protections, Is.EqualTo(protections));
            Assert.That(result.Grade, Is.EqualTo(ExpeditionGrade.Success));
            risks[0] = 9;
            Assert.That(result.Risks[0], Is.EqualTo(2), "넘긴 배열을 나중에 바꿔도 기록은 그대로다");

            // 위험·효과를 남기지 않은 기록은 null로 구분한다.
            Assert.That(Record(log, "v2", 1).Risks, Is.Null);
            Assert.That(log.Results[1].Protections, Is.Null);

            // 한쪽만 주거나, 축 수가 다르거나, 음수거나, 부족과 맞지 않으면 아무것도 남기지 않는다.
            int[] ok = { 2, 0, 0, 0, 1, 1 };
            Assert.That(log.TryRecord("g", "v3", "하루", 1, "구운 빵", true, shortfalls, ok, null, out _), Is.False);
            Assert.That(log.TryRecord("g", "v3", "하루", 1, "구운 빵", true, shortfalls, new[] { 2, 0 }, protections, out _), Is.False);
            Assert.That(log.TryRecord("g", "v3", "하루", 1, "구운 빵", true, shortfalls, new[] { 2, 0, 0, 0, 1, -1 }, protections, out _), Is.False);
            Assert.That(log.TryRecord("g", "v3", "하루", 1, "구운 빵", true, Short(0), ok, protections, out _), Is.False);
            Assert.That(log.Results.Count, Is.EqualTo(2));
        }

        [Test]
        public void AMaterialIsNewForTheWholeMorningWhenItWasNotOwnedAtTheStartOfTheDay()
        {
            var log = NewLog();
            Record(log, "v1", 0);
            var stock = new MaterialStock(new[] { new MaterialCount("숲버섯", 2), new MaterialCount("들풀", 0) });
            var reports = new ReturnReportLog(Gifted(log, 9), stock);
            Assert.That(reports.IsNewMaterial("숲버섯"), Is.False);
            Assert.That(reports.IsNewMaterial("들풀"), Is.True, "보유 수가 0이면 아직 얻지 못한 재료다");
            Assert.That(reports.IsNewMaterial(Rare), Is.True);
            Assert.That(reports.IsNewMaterial(""), Is.False);
            Assert.That(reports.IsNewMaterial(null), Is.False);

            reports.ConfirmAll();
            Assert.That(reports.IsNewMaterial(Rare), Is.True, "보고를 확정한 뒤 화면을 다시 열어도 그날은 신규로 남는다");
            Assert.That(reports.IsNewMaterial("숲버섯"), Is.False);
        }

        [Test]
        public void ConfirmingAReportAddsItsLootExactlyOnce()
        {
            var log = NewLog();
            Record(log, "v1", 1);
            var report = Gifted(log, 9, count: 2).Single();
            var stock = new MaterialStock();
            var reports = new ReturnReportLog(new[] { report }, stock);

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
            var reports = new ReturnReportLog(Gifted(log, 9, count: 2), stock);

            Assert.That(reports.Confirm("v1"), Is.EqualTo(ReportConfirmResult.Confirmed));
            Assert.That(reports.ConfirmAll(), Is.EqualTo(1));
            Assert.That(reports.ConfirmAll(), Is.Zero);
            Assert.That(Total(stock), Is.EqualTo(2 + 2));
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
