using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;

namespace DOB.Tests
{
    public sealed class FirstServiceSaveStoreTests
    {
        private string directory;
        private FirstServiceSaveStore store;

        [SetUp]
        public void SetUp()
        {
            directory = Path.Combine(Path.GetTempPath(), "DOB-SaveTest-" + Guid.NewGuid().ToString("N"));
            store = new FirstServiceSaveStore(directory);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
        }

        [Test]
        public void SlotsAreIndependentAndLatestCheckpointSurvivesReopening()
        {
            Assert.That(store.TryLoadLatest(out _), Is.False);
            Assert.That(Directory.Exists(directory), Is.False);
            var auto = store.SavePreparation(FirstServiceSaveSlot.Auto, 1, 50, 2);
            var manual = store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, 1);
            var reopened = new FirstServiceSaveStore(directory);
            Assert.That(reopened.TryLoadLatest(out var latest), Is.True);
            Assert.That(latest.Slot, Is.EqualTo(FirstServiceSaveSlot.Manual));
            Assert.That(latest.SavedAtUtc, Is.EqualTo(manual.SavedAtUtc));
            Assert.That(latest.SavedAtUtc.Kind, Is.EqualTo(DateTimeKind.Utc));
            Assert.That(latest.SavedAtUtc, Is.GreaterThan(auto.SavedAtUtc));
            Assert.That(latest.SchemaVersion, Is.EqualTo(2));
            Assert.That(latest.Stage, Is.EqualTo("Preparation"));
            Assert.That(latest.Gold, Is.EqualTo(59));
            Assert.That(latest.GarlicStock, Is.EqualTo(1));
            Assert.That(reopened.TryLoad(FirstServiceSaveSlot.Auto, out var first), Is.True);
            Assert.That(first.Day, Is.EqualTo(1));
            Assert.That(first.Gold, Is.EqualTo(50));
        }

        [Test]
        public void RepeatedOrConcurrentSettlementKeepsTheFirstPaidResult()
        {
            store.SavePreparation(FirstServiceSaveSlot.Auto, 1, 50, 2);
            var sameDirectory = new FirstServiceSaveStore(directory);
            FirstServiceSaveData left = null, right = null;
            Parallel.Invoke(() => left = store.SettleDay(1, 59, 1), () => right = sameDirectory.SettleDay(1, 59, 1));
            Assert.That(left.Day, Is.EqualTo(2));
            Assert.That(left.SavedAtUtc, Is.EqualTo(right.SavedAtUtc));

            var duplicate = sameDirectory.SettleDay(1, 68, 0);
            Assert.That(duplicate.Gold, Is.EqualTo(59));
            Assert.That(duplicate.GarlicStock, Is.EqualTo(1));
            Assert.That(duplicate.SavedAtUtc, Is.EqualTo(left.SavedAtUtc));
            var next = store.SettleDay(2, 69, 0);
            Assert.That(next.Day, Is.EqualTo(3));
            Assert.That(next.Gold, Is.EqualTo(69));
            Assert.That(sameDirectory.SettleDay(1, 68, 0).Day, Is.EqualTo(3));
        }

        [Test]
        public void ACorruptOrWrongSlotPrimaryRecoversTheLastValidBackup()
        {
            store.SavePreparation(FirstServiceSaveSlot.Auto, 1, 50, 2);
            string primary = Path.Combine(directory, "autosave.json");
            File.WriteAllText(primary, "{");
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var initialBackup), Is.True);
            Assert.That(initialBackup.Day, Is.EqualTo(1));
            store.SettleDay(1, 59, 1);
            store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, 1);

            // 유효한 JSON이라도 슬롯이 다르면 원본으로 쓸 수 없다.
            File.Copy(Path.Combine(directory, "manual.json"), primary, true);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var recovered), Is.True);
            Assert.That(recovered.Day, Is.EqualTo(1));
            Assert.That(recovered.Gold, Is.EqualTo(50));

            File.WriteAllText(primary + ".bak", "null");
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out _), Is.False);
            Assert.That(store.TryLoadLatest(out var latest), Is.True);
            Assert.That(latest.Slot, Is.EqualTo(FirstServiceSaveSlot.Manual));
        }

        [Test]
        public void UnsupportedSchemaAndInvalidBalancesDoNotReplaceAValidRecord()
        {
            store.SavePreparation(FirstServiceSaveSlot.Manual, 1, 0, 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 2, -1, 0));
            string primary = Path.Combine(directory, "manual.json");
            string valid = File.ReadAllText(primary);
            File.WriteAllText(primary, valid.Replace("\"gold\":0", "\"gold\":99999999999999999999"));
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out var oldBalance), Is.True);
            Assert.That(oldBalance.Gold, Is.Zero);
            File.WriteAllText(primary, valid + "unexpected");
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out _), Is.True);
            string future = valid.Replace("\"schemaVersion\":2", "\"schemaVersion\":999");
            Assert.That(future, Is.Not.EqualTo(valid));
            File.WriteAllText(primary, future);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out var recovered), Is.True);
            Assert.That(recovered.SchemaVersion, Is.EqualTo(2));
            File.WriteAllText(primary + ".bak", future);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out _), Is.False);
        }

        private static VisitResult Report(string visit, ExpeditionGrade grade, params string[] loot) =>
            new VisitResult("guest-" + visit, visit, "하루", 1, "구운 빵", true, new[] { (int)grade, 0, 0, 0, 0, 0 }, grade, loot);

        [Test]
        public void MaterialsAndPendingReportsSurviveReopening()
        {
            store.SavePreparation(FirstServiceSaveSlot.Auto, 1, 50, 2);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var empty), Is.True);
            Assert.That(empty.Materials, Is.Empty);
            Assert.That(empty.PendingReports, Is.Empty);

            store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, 1, new[] { new MaterialCount("숲버섯", 2) },
                new[] { Report("v1", ExpeditionGrade.Success, "들풀", "숲 당근") });
            Assert.That(new FirstServiceSaveStore(directory).TryLoad(FirstServiceSaveSlot.Manual, out var saved), Is.True);
            Assert.That(saved.SchemaVersion, Is.EqualTo(2));
            Assert.That(saved.Day, Is.EqualTo(2));
            Assert.That(saved.Gold, Is.EqualTo(59));
            Assert.That(saved.GarlicStock, Is.EqualTo(1));
            Assert.That(saved.Materials.Single().Id, Is.EqualTo("숲버섯"));
            Assert.That(saved.Materials.Single().Count, Is.EqualTo(2));
            var report = saved.PendingReports.Single();
            Assert.That(report.GuestId, Is.EqualTo("guest-v1"));
            Assert.That(report.VisitId, Is.EqualTo("v1"));
            Assert.That(report.GuestName, Is.EqualTo("하루"));
            Assert.That(report.Floor, Is.EqualTo(1));
            Assert.That(report.Dish, Is.EqualTo("구운 빵"));
            Assert.That(report.UsedSeasoning, Is.True);
            Assert.That(report.Grade, Is.EqualTo(ExpeditionGrade.Success));
            Assert.That(report.Shortfalls, Is.EqualTo(new[] { 1, 0, 0, 0, 0, 0 }));
            Assert.That(report.Loot, Is.EqualTo(new[] { "들풀", "숲 당근" }));
        }

        [Test]
        public void ReportRisksAndProtectionsSurviveReopeningAndOlderReportsLoadWithoutThem()
        {
            int[] risks = { 2, 0, 0, 0, 1, 1 }, protections = { 1, 0, 0, 0, 1, 1 };
            var withCells = new VisitResult("guest-v1", "v1", "하루", 1, "구운 빵", true, new[] { 1, 0, 0, 0, 0, 0 },
                ExpeditionGrade.Success, new[] { "들풀" }, risks, protections);
            store.SavePreparation(FirstServiceSaveSlot.Auto, 2, 59, 1, null, new[] { withCells, Report("v2", ExpeditionGrade.GreatSuccess) });
            Assert.That(new FirstServiceSaveStore(directory).TryLoad(FirstServiceSaveSlot.Auto, out var saved), Is.True);
            Assert.That(saved.PendingReports[0].Risks, Is.EqualTo(risks));
            Assert.That(saved.PendingReports[0].Protections, Is.EqualTo(protections));
            Assert.That(saved.PendingReports[1].Risks, Is.Null, "위험·효과 칸이 없던 보고는 null로 읽힌다");
            Assert.That(saved.PendingReports[1].Protections, Is.Null);

            // 축 수가 맞지 않거나 음수인 칸이 든 슬롯은 거절한다.
            string primary = Path.Combine(directory, "autosave.json");
            string valid = File.ReadAllText(primary);
            foreach (var (from, to) in new[] { ("\"risks\":[2,0,0,0,1,1]", "\"risks\":[2,0]"), ("\"protections\":[1,0,0,0,1,1]", "\"protections\":[1,0,0,0,1,-1]") })
            {
                string broken = valid.Replace(from, to);
                Assert.That(broken, Is.Not.EqualTo(valid), from);
                File.WriteAllText(primary, broken);
                File.WriteAllText(primary + ".bak", broken);
                Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out _), Is.False, from);
            }
        }

        [Test]
        public void SettlementCarriesStockAndTodaysVisitsIntoTheNextDayOnce()
        {
            store.SavePreparation(FirstServiceSaveSlot.Auto, 1, 50, 2);
            var first = store.SettleDay(1, 59, 1, null, new[] { Report("v1", ExpeditionGrade.GreatSuccess, "들풀", "들풀", "숲버섯") });
            Assert.That(first.Day, Is.EqualTo(2));
            Assert.That(first.PendingReports.Single().VisitId, Is.EqualTo("v1"));

            var duplicate = store.SettleDay(1, 68, 0, new[] { new MaterialCount("들풀", 9) }, new[] { Report("v2", ExpeditionGrade.Success, "들풀") });
            Assert.That(duplicate.SavedAtUtc, Is.EqualTo(first.SavedAtUtc));
            Assert.That(duplicate.Materials, Is.Empty);
            Assert.That(duplicate.PendingReports.Single().VisitId, Is.EqualTo("v1"));

            // 2일차 아침에 받은 전리품은 2일차 정산 때 3일차 시작 재고로 넘어간다.
            var next = store.SettleDay(2, 68, 1, new[] { new MaterialCount("들풀", 2), new MaterialCount("숲버섯", 1) });
            Assert.That(next.Day, Is.EqualTo(3));
            Assert.That(next.Materials.Single(m => m.Id == "들풀").Count, Is.EqualTo(2));
            Assert.That(next.PendingReports, Is.Empty);
        }

        [Test]
        public void ASchemaOneSlotIsReadWithEmptyStockAndReportsThenRewrittenAsSchemaTwo()
        {
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "autosave.json"),
                "{\"day\":2,\"garlicStock\":1,\"gold\":59,\"savedAtUtc\":\"2026-11-24T00:00:00.0000000Z\"," +
                "\"schemaVersion\":1,\"slot\":\"autosave\",\"stage\":\"Preparation\"}");
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var old), Is.True);
            Assert.That(old.SchemaVersion, Is.EqualTo(1));
            Assert.That(old.Day, Is.EqualTo(2));
            Assert.That(old.Gold, Is.EqualTo(59));
            Assert.That(old.GarlicStock, Is.EqualTo(1));
            Assert.That(old.Materials, Is.Empty);
            Assert.That(old.PendingReports, Is.Empty);

            Assert.That(store.SettleDay(2, 68, 0).SchemaVersion, Is.EqualTo(2));
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var rewritten), Is.True);
            Assert.That(rewritten.SchemaVersion, Is.EqualTo(2));
            Assert.That(rewritten.Day, Is.EqualTo(3));
        }

        [Test]
        public void NegativeStockAndReportsWithoutAVisitDoNotReplaceAValidRecord()
        {
            store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, 1, new[] { new MaterialCount("숲버섯", 2) },
                new[] { Report("v1", ExpeditionGrade.Success, "들풀") });
            string primary = Path.Combine(directory, "manual.json");
            string valid = File.ReadAllText(primary);

            foreach (var (from, to) in new[] { ("\"count\":2", "\"count\":-2"), ("\"visitId\":\"v1\"", "\"visitId\":\"\""),
                         ("\"grade\":1", "\"grade\":9"), ("\"schemaVersion\":2", "\"schemaVersion\":3") })
            {
                string broken = valid.Replace(from, to);
                Assert.That(broken, Is.Not.EqualTo(valid), from);
                File.WriteAllText(primary, broken);
                Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out var recovered), Is.True, from);
                Assert.That(recovered.Materials.Single().Count, Is.EqualTo(2), "유효한 백업을 읽는다: " + from);
                File.WriteAllText(primary + ".bak", broken);
                Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out _), Is.False, from);
                File.WriteAllText(primary + ".bak", valid);
            }

            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, 1,
                new[] { new MaterialCount("숲버섯", -1) }));
        }
    }
}
