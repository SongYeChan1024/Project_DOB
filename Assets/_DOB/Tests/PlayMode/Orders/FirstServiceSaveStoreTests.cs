using System;
using System.IO;
using System.Threading.Tasks;
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
            Assert.That(latest.SchemaVersion, Is.EqualTo(1));
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
            string future = valid.Replace("\"schemaVersion\":1", "\"schemaVersion\":999");
            Assert.That(future, Is.Not.EqualTo(valid));
            File.WriteAllText(primary, future);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out var recovered), Is.True);
            Assert.That(recovered.SchemaVersion, Is.EqualTo(1));
            File.WriteAllText(primary + ".bak", future);
            Assert.That(store.TryLoad(FirstServiceSaveSlot.Manual, out _), Is.False);
        }
    }
}
