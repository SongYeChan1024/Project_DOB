// 준비 시점의 경제와 일차를 저장한다. 읽을 때 슬롯·스키마·단계·UTC 시각이 일치하는지 검증한다.
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace DOB.Systems
{
    public enum FirstServiceSaveSlot { Auto, Manual }

    [DataContract]
    public sealed class FirstServiceSaveData
    {
        public const int CurrentSchemaVersion = 1;
        public const string PreparationStage = "Preparation";

        [DataMember(Name = "schemaVersion", IsRequired = true)] public int SchemaVersion { get; private set; }
        [DataMember(Name = "day", IsRequired = true)] public int Day { get; private set; }
        [DataMember(Name = "gold", IsRequired = true)] public int Gold { get; private set; }
        [DataMember(Name = "garlicStock", IsRequired = true)] public int GarlicStock { get; private set; }
        [DataMember(Name = "stage", IsRequired = true)] public string Stage { get; private set; }
        [DataMember(Name = "savedAtUtc", IsRequired = true)] private string savedAtUtc;
        [DataMember(Name = "slot", IsRequired = true)] private string slot;

        public DateTime SavedAtUtc => DateTime.ParseExact(savedAtUtc, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        public FirstServiceSaveSlot Slot => slot == "autosave" ? FirstServiceSaveSlot.Auto : FirstServiceSaveSlot.Manual;

        internal FirstServiceSaveData(FirstServiceSaveSlot destination, int day, int gold, int garlicStock, DateTime timestamp)
        {
            if (day < 1) throw new ArgumentOutOfRangeException(nameof(day));
            if (gold < 0) throw new ArgumentOutOfRangeException(nameof(gold));
            if (garlicStock < 0) throw new ArgumentOutOfRangeException(nameof(garlicStock));
            SchemaVersion = CurrentSchemaVersion;
            Stage = PreparationStage;
            Day = day;
            Gold = gold;
            GarlicStock = garlicStock;
            savedAtUtc = timestamp.ToString("O", CultureInfo.InvariantCulture);
            slot = SlotName(destination);
        }

        internal bool IsValidFor(FirstServiceSaveSlot expectedSlot)
        {
            return SchemaVersion == CurrentSchemaVersion && Stage == PreparationStage && Day >= 1 &&
                Gold >= 0 && GarlicStock >= 0 && slot == SlotName(expectedSlot) &&
                DateTime.TryParseExact(savedAtUtc, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var time) &&
                time.Kind == DateTimeKind.Utc;
        }

        internal static string SlotName(FirstServiceSaveSlot value)
        {
            switch (value)
            {
                case FirstServiceSaveSlot.Auto: return "autosave";
                case FirstServiceSaveSlot.Manual: return "manual";
                default: throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }
}
