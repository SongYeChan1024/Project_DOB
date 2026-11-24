// 일차 시작 스냅샷: 일차·금화·마늘에 더해 던전 재료 재고와 그날 아침의 미처리 귀환 보고를 저장한다.
// 읽을 때 슬롯·스키마·단계·UTC 시각과 재료 수·보고 내용이 올바른지 검증한다.
// 스키마 1(재료·보고 없음)도 읽는다. 그때 재료와 보고는 비어 있는 것으로 본다. 새로 쓰는 스냅샷은 항상 스키마 2다.
// 보고 도중에는 저장하지 않으므로 슬롯의 보고는 항상 미처리다 (💡 김언호 선택 2026-11-24, 기획 확인 대기).
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using DOB.Define;

namespace DOB.Systems
{
    public enum FirstServiceSaveSlot { Auto, Manual }

    [DataContract]
    public sealed class FirstServiceSaveData
    {
        public const int CurrentSchemaVersion = 2;
        private const int OldestReadableSchemaVersion = 1;
        public const string PreparationStage = "Preparation";

        [DataMember(Name = "schemaVersion", IsRequired = true)] public int SchemaVersion { get; private set; }
        [DataMember(Name = "day", IsRequired = true)] public int Day { get; private set; }
        [DataMember(Name = "gold", IsRequired = true)] public int Gold { get; private set; }
        [DataMember(Name = "garlicStock", IsRequired = true)] public int GarlicStock { get; private set; }
        [DataMember(Name = "stage", IsRequired = true)] public string Stage { get; private set; }
        [DataMember(Name = "savedAtUtc", IsRequired = true)] private string savedAtUtc;
        [DataMember(Name = "slot", IsRequired = true)] private string slot;
        // 스키마 1에는 없는 항목이라 필수로 두지 않는다.
        [DataMember(Name = "materials")] private MaterialCount[] materials;
        [DataMember(Name = "pendingReports")] private VisitResult[] pendingReports;

        /// <summary>일차 시작 때의 던전 재료 재고.</summary>
        public IReadOnlyList<MaterialCount> Materials => materials ?? Array.Empty<MaterialCount>();
        /// <summary>이 일차 아침에 받을 귀환 보고(전날 방문 결과). 모두 미처리 상태다.</summary>
        public IReadOnlyList<VisitResult> PendingReports => pendingReports ?? Array.Empty<VisitResult>();

        public DateTime SavedAtUtc => DateTime.ParseExact(savedAtUtc, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        public FirstServiceSaveSlot Slot => slot == "autosave" ? FirstServiceSaveSlot.Auto : FirstServiceSaveSlot.Manual;

        internal FirstServiceSaveData(FirstServiceSaveSlot destination, int day, int gold, int garlicStock, DateTime timestamp,
            IEnumerable<MaterialCount> materials = null, IEnumerable<VisitResult> pendingReports = null)
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
            this.materials = materials == null ? null : new List<MaterialCount>(materials).ToArray();
            this.pendingReports = pendingReports == null ? null : new List<VisitResult>(pendingReports).ToArray();
            if (!ContentIsValid())
                throw new ArgumentException("재료 수량 또는 귀환 보고 내용이 올바르지 않습니다.");
        }

        internal bool IsValidFor(FirstServiceSaveSlot expectedSlot)
        {
            return SchemaVersion >= OldestReadableSchemaVersion && SchemaVersion <= CurrentSchemaVersion &&
                Stage == PreparationStage && Day >= 1 &&
                Gold >= 0 && GarlicStock >= 0 && slot == SlotName(expectedSlot) && ContentIsValid() &&
                DateTime.TryParseExact(savedAtUtc, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var time) &&
                time.Kind == DateTimeKind.Utc;
        }

        // 손상된 파일의 재료·보고가 재고나 보고 목록에 들어가지 않도록 읽을 때와 쓸 때 같은 기준으로 검사한다.
        private bool ContentIsValid()
        {
            foreach (var material in Materials)
                if (material == null || string.IsNullOrEmpty(material.Id) || material.Count < 0)
                    return false;
            var visits = new HashSet<string>();
            foreach (var report in PendingReports)
            {
                if (report == null || string.IsNullOrEmpty(report.GuestId) || string.IsNullOrEmpty(report.VisitId) ||
                    !visits.Add(report.VisitId) || !Enum.IsDefined(typeof(ExpeditionGrade), report.Grade) ||
                    report.Shortfalls == null || report.Shortfalls.Count != RiskAxes.All.Count || report.Loot == null)
                    return false;
                foreach (int shortfall in report.Shortfalls)
                    if (shortfall < 0) return false;
                foreach (string loot in report.Loot)
                    if (string.IsNullOrEmpty(loot)) return false;
            }
            return true;
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
