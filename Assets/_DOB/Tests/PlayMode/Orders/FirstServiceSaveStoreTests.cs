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

        private static MaterialCount[] Garlic(int count) => new[] { new MaterialCount("garlic", count) };

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
            var auto = store.SavePreparation(FirstServiceSaveSlot.Auto, 1, 50, Garlic(2));
            var manual = store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, Garlic(1));
            var reopened = new FirstServiceSaveStore(directory);
            Assert.That(reopened.TryLoadLatest(out var latest), Is.True);
            Assert.That(latest.Slot, Is.EqualTo(FirstServiceSaveSlot.Manual));
            Assert.That(latest.SavedAtUtc, Is.EqualTo(manual.SavedAtUtc));
            Assert.That(latest.SavedAtUtc.Kind, Is.EqualTo(DateTimeKind.Utc));
            Assert.That(latest.SavedAtUtc, Is.GreaterThan(auto.SavedAtUtc));
            Assert.That(latest.SchemaVersion, Is.EqualTo(FirstServiceSaveData.CurrentSchemaVersion));
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
            store.SavePreparation(FirstServiceSaveSlot.Auto, 1, 50, Garlic(2));
            var sameDirectory = new FirstServiceSaveStore(directory);
            FirstServiceSaveData left = null, right = null;
            Parallel.Invoke(() => left = store.SettleDay(1, 59, Garlic(1)), () => right = sameDirectory.SettleDay(1, 59, Garlic(1)));
            Assert.That(left.Day, Is.EqualTo(2));
            Assert.That(left.SavedAtUtc, Is.EqualTo(right.SavedAtUtc));

            var duplicate = sameDirectory.SettleDay(1, 68, Garlic(0));
            Assert.That(duplicate.Gold, Is.EqualTo(59));
            Assert.That(duplicate.GarlicStock, Is.EqualTo(1));
            Assert.That(duplicate.SavedAtUtc, Is.EqualTo(left.SavedAtUtc));
            var next = store.SettleDay(2, 69, Garlic(0));
            Assert.That(next.Day, Is.EqualTo(3));
            Assert.That(next.Gold, Is.EqualTo(69));
            Assert.That(sameDirectory.SettleDay(1, 68, Garlic(0)).Day, Is.EqualTo(3));
        }

        [Test]
        public void ACorruptOrWrongSlotPrimaryRecoversTheLastValidBackup()
        {
            store.SavePreparation(FirstServiceSaveSlot.Auto, 1, 50, Garlic(2));
            string primary = Path.Combine(directory, "autosave.json");
            File.WriteAllText(primary, "{");
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var initialBackup), Is.True);
            Assert.That(initialBackup.Day, Is.EqualTo(1));
            store.SettleDay(1, 59, Garlic(1));
            store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, Garlic(1));

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
            store.SavePreparation(FirstServiceSaveSlot.Manual, 1, 0, Garlic(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 2, -1, Garlic(0)));
            string primary = Path.Combine(directory, "manual.json");
            string valid = File.ReadAllText(primary);
            File.WriteAllText(primary, valid.Replace("\"gold\":0", "\"gold\":99999999999999999999"));
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out var oldBalance), Is.True);
            Assert.That(oldBalance.Gold, Is.Zero);
            File.WriteAllText(primary, valid + "unexpected");
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out _), Is.True);
            string future = valid.Replace("\"schemaVersion\":" + FirstServiceSaveData.CurrentSchemaVersion, "\"schemaVersion\":999");
            Assert.That(future, Is.Not.EqualTo(valid));
            File.WriteAllText(primary, future);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out var recovered), Is.True);
            Assert.That(recovered.SchemaVersion, Is.EqualTo(FirstServiceSaveData.CurrentSchemaVersion));
            File.WriteAllText(primary + ".bak", future);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out _), Is.False);
        }

        private static VisitResult Report(string visit, ExpeditionGrade grade, params string[] loot) =>
            new VisitResult("guest-" + visit, visit, "하루", 1, "구운 빵", true, new[] { (int)grade, 0, 0, 0, 0, 0 }, grade, loot);

        [Test]
        public void MaterialsAndPendingReportsSurviveReopening()
        {
            store.SavePreparation(FirstServiceSaveSlot.Auto, 1, 50, Garlic(2));
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var empty), Is.True);
            Assert.That(empty.Materials, Is.Empty);
            Assert.That(empty.PendingReports, Is.Empty);

            store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, Garlic(1), new[] { new MaterialCount("숲버섯", 2) },
                new[] { Report("v1", ExpeditionGrade.Success, "들풀", "숲 당근") });
            Assert.That(new FirstServiceSaveStore(directory).TryLoad(FirstServiceSaveSlot.Manual, out var saved), Is.True);
            Assert.That(saved.SchemaVersion, Is.EqualTo(FirstServiceSaveData.CurrentSchemaVersion));
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
            store.SavePreparation(FirstServiceSaveSlot.Auto, 2, 59, Garlic(1), null, new[] { withCells, Report("v2", ExpeditionGrade.GreatSuccess) });
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
            store.SavePreparation(FirstServiceSaveSlot.Auto, 1, 50, Garlic(2));
            var first = store.SettleDay(1, 59, Garlic(1), null, new[] { Report("v1", ExpeditionGrade.GreatSuccess, "들풀", "들풀", "숲버섯") });
            Assert.That(first.Day, Is.EqualTo(2));
            Assert.That(first.PendingReports.Single().VisitId, Is.EqualTo("v1"));

            var duplicate = store.SettleDay(1, 68, Garlic(0), new[] { new MaterialCount("들풀", 9) }, new[] { Report("v2", ExpeditionGrade.Success, "들풀") });
            Assert.That(duplicate.SavedAtUtc, Is.EqualTo(first.SavedAtUtc));
            Assert.That(duplicate.Materials, Is.Empty);
            Assert.That(duplicate.PendingReports.Single().VisitId, Is.EqualTo("v1"));

            // 2일차 아침에 받은 전리품은 2일차 정산 때 3일차 시작 재고로 넘어간다.
            var next = store.SettleDay(2, 68, Garlic(1), new[] { new MaterialCount("들풀", 2), new MaterialCount("숲버섯", 1) });
            Assert.That(next.Day, Is.EqualTo(3));
            Assert.That(next.Materials.Single(m => m.Id == "들풀").Count, Is.EqualTo(2));
            Assert.That(next.PendingReports, Is.Empty);
        }

        [Test]
        public void ASchemaOneSlotIsReadWithEmptyStockAndReportsThenRewrittenAsTheCurrentSchema()
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
            Assert.That(old.Spices.Select(x => (x.Id, x.Count)), Is.EqualTo(new[] { ("garlic", 1) }), "이전 스키마는 마늘만 있다");
            Assert.That(old.Floors, Is.Null, "이전 스키마는 층 진행이 없다(1층만 열림)");
            Assert.That(old.Materials, Is.Empty);
            Assert.That(old.PendingReports, Is.Empty);

            Assert.That(store.SettleDay(2, 68, Garlic(0)).SchemaVersion, Is.EqualTo(FirstServiceSaveData.CurrentSchemaVersion));
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var rewritten), Is.True);
            Assert.That(rewritten.SchemaVersion, Is.EqualTo(FirstServiceSaveData.CurrentSchemaVersion));
            Assert.That(rewritten.Day, Is.EqualTo(3));
        }

        [Test]
        public void NegativeStockAndReportsWithoutAVisitDoNotReplaceAValidRecord()
        {
            store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, Garlic(1), new[] { new MaterialCount("숲버섯", 2) },
                new[] { Report("v1", ExpeditionGrade.Success, "들풀") });
            string primary = Path.Combine(directory, "manual.json");
            string valid = File.ReadAllText(primary);

            foreach (var (from, to) in new[] { ("\"count\":2", "\"count\":-2"), ("\"visitId\":\"v1\"", "\"visitId\":\"\""),
                         ("\"grade\":1", "\"grade\":9"), ("\"schemaVersion\":" + FirstServiceSaveData.CurrentSchemaVersion, "\"schemaVersion\":" + (FirstServiceSaveData.CurrentSchemaVersion + 1)) })
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

            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, Garlic(1),
                new[] { new MaterialCount("숲버섯", -1) }));
        }

        // ── 스키마 3: 전날 방문 호감과 입소문 추가 인원 ──

        private static VisitAffinity[] Affinities() => new[]
        {
            new VisitAffinity("g1", "v1", ServiceVisitEnd.Dispatched, 2, 0, 1, 3),
            new VisitAffinity("g2", "v2", ServiceVisitEnd.WaitedOut, 0, 0, 0, -4),
            new VisitAffinity("g3", "v3", ServiceVisitEnd.SentHome, 0, 0, 0, -4)
        };

        [Test]
        public void VisitAffinitiesAndWordOfMouthSurviveReopeningInOrder()
        {
            store.SavePreparation(FirstServiceSaveSlot.Auto, 2, 59, Garlic(1), null, null, Affinities(), 1);
            Assert.That(new FirstServiceSaveStore(directory).TryLoad(FirstServiceSaveSlot.Auto, out var saved), Is.True);
            Assert.That(saved.SchemaVersion, Is.EqualTo(FirstServiceSaveData.CurrentSchemaVersion));
            Assert.That(saved.WordOfMouthBonus, Is.EqualTo(1));
            Assert.That(saved.VisitAffinities.Select(a => (a.GuestId, a.VisitId, a.End, a.Wait, a.Taste, a.Expedition, a.Total)),
                Is.EqualTo(Affinities().Select(a => (a.GuestId, a.VisitId, a.End, a.Wait, a.Taste, a.Expedition, a.Total))));
        }

        [Test]
        public void SchemaOneAndTwoSlotsLoadWithNoAffinitiesAndNoBonus()
        {
            Directory.CreateDirectory(directory);
            foreach (int schema in new[] { 1, 2 })
            {
                File.WriteAllText(Path.Combine(directory, "autosave.json"),
                    "{\"day\":2,\"garlicStock\":1,\"gold\":59,\"savedAtUtc\":\"2026-11-24T00:00:00.0000000Z\"," +
                    $"\"schemaVersion\":{schema},\"slot\":\"autosave\",\"stage\":\"Preparation\",\"materials\":[],\"pendingReports\":[]}}");
                Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var old), Is.True, "스키마 " + schema);
                Assert.That(old.VisitAffinities, Is.Empty);
                Assert.That(old.WordOfMouthBonus, Is.Zero);
            }
        }

        [Test]
        public void BrokenAffinitiesOrANegativeBonusAreRejected()
        {
            store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, Garlic(1), null, null, Affinities(), 1);
            string primary = Path.Combine(directory, "manual.json");
            string valid = File.ReadAllText(primary);
            foreach (var (from, to) in new[] { ("\"visitId\":\"v2\"", "\"visitId\":\"v1\""), ("\"visitId\":\"v2\"", "\"visitId\":\"\""),
                         ("\"end\":2", "\"end\":9"), ("\"end\":2", "\"end\":0"), ("\"wordOfMouthBonus\":1", "\"wordOfMouthBonus\":-1") })
            {
                string broken = valid.Replace(from, to);
                Assert.That(broken, Is.Not.EqualTo(valid), from);
                File.WriteAllText(primary, broken);
                File.WriteAllText(primary + ".bak", broken);
                Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out _), Is.False, from);
            }

            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, Garlic(1), null, null, null, -1));
            var duplicate = new[] { Affinities()[0], Affinities()[0] };
            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 2, 59, Garlic(1), null, null, duplicate));
        }

        [Test]
        public void ASecondSettlementOfTheSameDayKeepsTheFirstAffinities()
        {
            store.SavePreparation(FirstServiceSaveSlot.Auto, 1, 50, Garlic(2));
            var first = store.SettleDay(1, 59, Garlic(1), null, null, Affinities(), 1);
            var again = store.SettleDay(1, 59, Garlic(1), null, null, Affinities().Take(1), 4);
            Assert.That(again.SavedAtUtc, Is.EqualTo(first.SavedAtUtc));
            Assert.That(again.VisitAffinities.Count, Is.EqualTo(3));
            Assert.That(again.WordOfMouthBonus, Is.EqualTo(1));
        }

        // ── 스키마 4: 손님 명부와 일차 시드 ──

        private static GuestRecord[] Ledger() => new[]
        {
            new GuestRecord("g1", "하루", "수인", "사제", "독", 7, false, 3),
            new GuestRecord("g2", "보리", "인간", "전사", null, 0, true, 3),
            new GuestRecord("g3", "솔", "엘프", "궁수", null, 36, false, 5)
        };

        [Test]
        public void TheGuestLedgerAndDaySeedSurviveReopening()
        {
            var saved = store.SavePreparation(FirstServiceSaveSlot.Auto, 3, 59, Garlic(1), null, null, null, 0, Ledger(), 12345);
            Assert.That(saved.SchemaVersion, Is.EqualTo(FirstServiceSaveData.CurrentSchemaVersion));
            Assert.That(new FirstServiceSaveStore(directory).TryLoad(FirstServiceSaveSlot.Auto, out var loaded), Is.True);
            Assert.That(loaded.DaySeed, Is.EqualTo(12345));
            CollectionAssert.AreEqual(
                new[] { ("g1", "하루", "수인", "사제", "독", 7, false, 3), ("g2", "보리", "인간", "전사", (string)null, 0, true, 3),
                    ("g3", "솔", "엘프", "궁수", (string)null, 36, false, 5) },
                loaded.Guests.Select(g => (g.GuestId, g.Name, g.Race, g.Job, g.Weakness, g.Affinity, g.Rejected, g.ReturnDay)));

            var settled = store.SettleDay(3, 70, Garlic(0), null, null, null, 2, Ledger().Take(1), -7);
            Assert.That((settled.Day, settled.Guests.Count, settled.DaySeed), Is.EqualTo((4, 1, (int?)-7)), "정산도 명부와 음수 시드를 그대로 쓴다");
        }

        [Test]
        public void ASchemaThreeSlotLoadsWithAnEmptyLedgerAndNoSeed()
        {
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "autosave.json"),
                "{\"day\":2,\"garlicStock\":1,\"gold\":59,\"savedAtUtc\":\"2026-11-24T00:00:00.0000000Z\"," +
                "\"schemaVersion\":3,\"slot\":\"autosave\",\"stage\":\"Preparation\",\"materials\":[],\"pendingReports\":[]," +
                "\"visitAffinities\":[],\"wordOfMouthBonus\":1}");
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var old), Is.True);
            Assert.That(old.Guests, Is.Empty);
            Assert.That(old.DaySeed, Is.Null);
            Assert.That(old.WordOfMouthBonus, Is.EqualTo(1), "다른 항목은 그대로 읽는다");
        }

        [Test]
        public void ABrokenLedgerIsRejected()
        {
            store.SavePreparation(FirstServiceSaveSlot.Manual, 3, 59, Garlic(1), null, null, null, 0, Ledger(), 1);
            string primary = Path.Combine(directory, "manual.json");
            string valid = File.ReadAllText(primary);
            foreach (var (from, to) in new[] { ("\"guestId\":\"g2\"", "\"guestId\":\"g1\""), ("\"guestId\":\"g2\"", "\"guestId\":\"\""),
                         ("\"name\":\"보리\"", "\"name\":\"\""), ("\"affinity\":7", "\"affinity\":-1"), ("\"returnDay\":5", "\"returnDay\":0") })
            {
                string broken = valid.Replace(from, to);
                Assert.That(broken, Is.Not.EqualTo(valid), from);
                File.WriteAllText(primary, broken);
                File.WriteAllText(primary + ".bak", broken);
                Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out _), Is.False, from);
            }

            var duplicate = new[] { Ledger()[0], Ledger()[0] };
            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 3, 59, Garlic(1), null, null, null, 0, duplicate));
        }

        [Test]
        public void TheDemoCompleteSlotIsReadOnlyOnPurpose()
        {
            Assert.That(store.TryLoad(FirstServiceSaveSlot.DemoComplete, out _), Is.False, "엔딩을 본 적이 없으면 슬롯 없음");
            store.SavePreparation(FirstServiceSaveSlot.Auto, 4, 50, Garlic(2));
            store.SavePreparation(FirstServiceSaveSlot.DemoComplete, 5, 70, Garlic(1));
            Assert.That(store.TryLoadLatest(out var latest), Is.True);
            Assert.That(latest.Slot, Is.EqualTo(FirstServiceSaveSlot.Auto), "이어하기는 데모 완료 슬롯을 고르지 않는다");
            Assert.That(store.TryLoad(FirstServiceSaveSlot.DemoComplete, out var demo), Is.True);
            Assert.That((demo.Day, demo.Slot), Is.EqualTo((5, FirstServiceSaveSlot.DemoComplete)));
        }

        [Test]
        public void SpicesAndFloorProgressRoundTripAndBadSpicesAreRejected()
        {
            var spices = new[] { new MaterialCount("garlic", 1), new MaterialCount("salt", 4) };
            var floors = new FloorProgressState(2, 1, new[] { new FloorReveal(1, new[] { "독", "체력" }) });
            store.SavePreparation(FirstServiceSaveSlot.Auto, 3, 40, spices, floors: floors);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var loaded), Is.True);
            Assert.That(loaded.Spices.Select(x => (x.Id, x.Count)), Is.EqualTo(new[] { ("garlic", 1), ("salt", 4) }));
            Assert.That((loaded.Floors.OpenFloors, loaded.Floors.DeepestWins), Is.EqualTo((2, 1)));
            Assert.That(loaded.Floors.Revealed.Single().Axes, Is.EqualTo(new[] { "독", "체력" }));
            Assert.That(File.ReadAllText(Path.Combine(directory, "autosave.json")), Does.Not.Contain("garlicStock"), "스키마 6은 마늘 개수를 따로 쓰지 않는다");

            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 3, 40, new[] { new MaterialCount("salt", -1) }), "음수 재고");
            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 3, 40, new[] { new MaterialCount("truffle", 1) }), "모르는 양념");
            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 3, 40, spices, floors: new FloorProgressState(0, 0, null)), "열린 층 0");

            // 손상된 파일의 모르는 양념은 읽지 않고 백업으로 돌아간다.
            store.SavePreparation(FirstServiceSaveSlot.Auto, 3, 40, spices, floors: floors);
            string path = Path.Combine(directory, "autosave.json");
            File.WriteAllText(path, File.ReadAllText(path).Replace("\"salt\"", "\"truffle\""));
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var fallback), Is.True);
            Assert.That(fallback.Spices.Any(x => x.Id == "truffle"), Is.False);
        }

        [Test]
        public void AServiceProgressSurvivesTheFileAndAMismatchedOneFallsBackToTheBackup()
        {
            var roster = new RosterProgress(3f, false, new[] { new VisitProgress("v1", "g1", ServiceVisitStage.Seated, ServiceVisitEnd.None, 2f, null, null, 1) });
            var progress = new DayProgress(DayPhase.Service, null, 60, Garlic(1), roster, new[] { new OrderProgress("v1", FirstGuestOrderState.Cooking, 4f, "Bread", "garlic", 0) });
            store.SavePreparation(FirstServiceSaveSlot.Manual, 1, 50, Garlic(2), progress: progress);
            // 두 번째 쓰기에서 첫 파일이 백업이 된다.
            store.SavePreparation(FirstServiceSaveSlot.Manual, 1, 50, Garlic(2), progress: progress);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out var loaded), Is.True);
            Assert.That(loaded.SchemaVersion, Is.EqualTo(FirstServiceSaveData.CurrentSchemaVersion));
            Assert.That(loaded.Progress.Phase, Is.EqualTo(DayPhase.Service));
            Assert.That(loaded.Progress.Orders.Single().RemainingSeconds, Is.EqualTo(4f));
            Assert.That(loaded.Progress.Roster.Visits.Single().Seat, Is.EqualTo(1));

            string path = Path.Combine(directory, "manual.json");
            string text = File.ReadAllText(path);
            Assert.That(text, Does.Contain("\"spice\":\"garlic\",\"state\":1,\"visitId\":\"v1\""));
            Assert.That(text, Does.Not.Contain("usedGarlic"), "스키마 7은 레시피·양념 id로 쓴다");
            File.WriteAllText(path, text.Replace("\"state\":1,\"visitId\":\"v1\"", "\"state\":1,\"visitId\":\"v9\""));
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out var fallback), Is.True, "명단에 없는 주문이면 백업을 읽는다");
            Assert.That(fallback.Progress.Orders.Single().VisitId, Is.EqualTo("v1"));

            var orphan = new DayProgress(DayPhase.Service, null, 60, Garlic(1), roster, new[] { new OrderProgress("v9", FirstGuestOrderState.Waiting, 6f, null, null, -1) });
            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 1, 50, Garlic(2), progress: orphan));
        }

        [Test]
        public void UnlockedRecipesRoundTripAndOlderSavesHaveNone()
        {
            var saved = store.SavePreparation(FirstServiceSaveSlot.Auto, 3, 40, Garlic(1), unlockedRecipes: new[] { "Bread", "MushroomSoup" });
            Assert.That(saved.SchemaVersion, Is.EqualTo(7));
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var loaded), Is.True);
            Assert.That(loaded.UnlockedRecipes, Is.EqualTo(new[] { "Bread", "MushroomSoup" }));

            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 3, 40, Garlic(1), unlockedRecipes: new[] { "Pizza" }), "모르는 레시피");
            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 3, 40, Garlic(1), unlockedRecipes: new[] { "Bread", "Bread" }), "겹친 레시피");

            // 스키마 6 파일에는 열린 레시피가 없다. 불러오는 쪽이 시작 레시피 + 재고로 다시 연다.
            store.SavePreparation(FirstServiceSaveSlot.Manual, 3, 40, Garlic(1));
            string path = Path.Combine(directory, "manual.json");
            File.WriteAllText(path, File.ReadAllText(path).Replace("\"schemaVersion\":7", "\"schemaVersion\":6"));
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out var legacy), Is.True);
            Assert.That(legacy.UnlockedRecipes, Is.Null);
            Assert.That(legacy.Burners, Is.Null, "이전 저장은 시작 화구 수로 연다");
        }

        [Test]
        public void OwnedBurnersRoundTripAndZeroIsRejected()
        {
            store.SavePreparation(FirstServiceSaveSlot.Auto, 3, 40, Garlic(1), burners: 2);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Auto, out var loaded), Is.True);
            Assert.That(loaded.Burners, Is.EqualTo(2));
            Assert.Throws<ArgumentException>(() => store.SavePreparation(FirstServiceSaveSlot.Manual, 3, 40, Garlic(1), burners: 0));
            var progress = new DayProgress(DayPhase.Preparation, null, 10, Garlic(1), burners: 2);
            store.SavePreparation(FirstServiceSaveSlot.Manual, 3, 40, Garlic(1), progress: progress, burners: 1);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out var manual), Is.True);
            Assert.That((manual.Burners, manual.Progress.Burners), Is.EqualTo(((int?)1, (int?)2)), "일차 시작 1칸, 누른 시점 2칸");
        }
    }
}
