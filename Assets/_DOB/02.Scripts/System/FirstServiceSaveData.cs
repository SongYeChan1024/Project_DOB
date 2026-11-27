// 일차 시작 스냅샷: 일차·금화·마늘에 더해 던전 재료 재고와 그날 아침의 미처리 귀환 보고를 저장한다.
// 읽을 때 슬롯·스키마·단계·UTC 시각과 재료 수·보고 내용이 올바른지 검증한다.
// 스키마 3에서 전날 방문 호감 기록과 이 일차의 입소문 추가 인원을 더했다.
// 스키마 4에서 손님 명부(신원·누적 호감·영구 거절·복귀 일차)와 이 일차의 난수 시드를 더했다 (💡 홍준우와 저장 계약 합의 전).
// 스키마 5에서 수동 저장을 누른 시점의 진행(DayProgress)을 선택 항목으로 더했다 (F13, 2026-11-25. 💡 홍준우와 저장 계약 합의 전).
// 진행이 없으면 일차 시작 스냅샷이고, 있으면 시작 스냅샷으로 하루를 연 뒤 그 진행으로 복원한다. 손님 명부 줄에는 입맛 태그가 더해졌다.
// 스키마 1(재료·보고 없음)·2(호감·입소문 없음)·3(명부·시드 없음)·4(진행 없음)도 읽는다. 없는 항목은 빈 목록·0·시드 없음·진행 없음으로 본다.
// 새로 쓰는 스냅샷은 항상 최신 스키마이다. 시작 스냅샷의 보고는 항상 미처리이고, 확정한 보고는 진행의 확정 ID로 남긴다.
// 데모 완료 슬롯은 엔딩을 본 일차의 시작 스냅샷을 보관한다(진행 없음).
// 스키마 6에서 마늘 개수(garlicStock) 대신 양념 재고 목록(spices)과 층 진행(floors)을 더했다 (F8·F9, 💡 홍준우와 저장 계약 합의 전).
// 스키마 1~5의 garlicStock은 마늘 재고로 읽고 다른 양념은 0, 층 진행이 없으면 1층만 열림·성공 0·공개 없음으로 본다.
// 음수 재고·겹치는 양념·시장 기준(SO_Market)에 없는 양념 id가 든 슬롯은 유효하지 않다.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using DOB.Define;

namespace DOB.Systems
{
    public enum FirstServiceSaveSlot { Auto, Manual, DemoComplete }

    [DataContract]
    public sealed class FirstServiceSaveData
    {
        public const int CurrentSchemaVersion = 6;
        // 스키마 5까지 저장하던 유일한 양념. 스키마 6의 시장 id와 같다.
        private const string LegacyGarlicId = "garlic";
        private const int OldestReadableSchemaVersion = 1;
        public const string PreparationStage = "Preparation";

        [DataMember(Name = "schemaVersion", IsRequired = true)] public int SchemaVersion { get; private set; }
        [DataMember(Name = "day", IsRequired = true)] public int Day { get; private set; }
        [DataMember(Name = "gold", IsRequired = true)] public int Gold { get; private set; }
        // 스키마 1~5에서만 쓴다. 스키마 6은 쓰지 않고 spices를 쓴다.
        [DataMember(Name = "garlicStock", EmitDefaultValue = false)] private int garlicStock;
        [DataMember(Name = "stage", IsRequired = true)] public string Stage { get; private set; }
        [DataMember(Name = "savedAtUtc", IsRequired = true)] private string savedAtUtc;
        [DataMember(Name = "slot", IsRequired = true)] private string slot;
        // 스키마 1에는 없는 항목이라 필수로 두지 않는다.
        [DataMember(Name = "materials")] private MaterialCount[] materials;
        [DataMember(Name = "pendingReports")] private VisitResult[] pendingReports;
        // 스키마 1·2에는 없는 항목이라 필수로 두지 않는다.
        [DataMember(Name = "visitAffinities")] private VisitAffinity[] visitAffinities;
        [DataMember(Name = "wordOfMouthBonus")] private int wordOfMouthBonus;
        // 스키마 1~3에는 없는 항목이라 필수로 두지 않는다.
        [DataMember(Name = "guests")] private GuestRecord[] guests;
        [DataMember(Name = "daySeed")] private int? daySeed;
        // 스키마 1~4에는 없고, 스키마 5에서도 일차 시작 스냅샷이면 없다.
        [DataMember(Name = "progress")] private DayProgress progress;
        // 스키마 1~5에는 없다.
        [DataMember(Name = "spices")] private MaterialCount[] spices;
        [DataMember(Name = "floors")] private FloorProgressState floors;

        /// <summary>일차 시작 때의 던전 재료 재고.</summary>
        public IReadOnlyList<MaterialCount> Materials => materials ?? Array.Empty<MaterialCount>();
        /// <summary>이 일차 아침에 받을 귀환 보고(전날 방문 결과). 모두 미처리 상태다.</summary>
        public IReadOnlyList<VisitResult> PendingReports => pendingReports ?? Array.Empty<VisitResult>();
        /// <summary>전날 마감 정산에서 확정한 방문 호감 기록(정산 순서). 없으면 빈 목록.</summary>
        public IReadOnlyList<VisitAffinity> VisitAffinities => visitAffinities ?? Array.Empty<VisitAffinity>();
        /// <summary>이 일차 영업의 입소문 추가 인원(명). 0 이상, 없으면 0.</summary>
        public int WordOfMouthBonus => wordOfMouthBonus;
        /// <summary>손님 명부(등록 순서). 없으면 빈 목록.</summary>
        public IReadOnlyList<GuestRecord> Guests => guests ?? Array.Empty<GuestRecord>();
        /// <summary>이 일차의 난수 시드. 없으면 null이고 영업을 열 때 새로 정한다.</summary>
        public int? DaySeed => daySeed;
        /// <summary>수동 저장을 누른 시점의 진행. 일차 시작 스냅샷이면 null.</summary>
        public DayProgress Progress => progress;
        /// <summary>일차 시작 양념 재고. 스키마 1~5는 마늘만 있다.</summary>
        public IReadOnlyList<MaterialCount> Spices => SchemaVersion < 6 ? LegacyGarlic(garlicStock) : (IReadOnlyList<MaterialCount>)(spices ?? Array.Empty<MaterialCount>());
        /// <summary>일차 시작 마늘 재고(양념 재고에서 읽는다).</summary>
        public int GarlicStock
        {
            get
            {
                foreach (var spice in Spices)
                    if (spice.Id == LegacyGarlicId) return spice.Count;
                return 0;
            }
        }
        /// <summary>일차 시작 층 진행. 이전 스키마면 null(1층만 열림·성공 0·공개 없음).</summary>
        public FloorProgressState Floors => floors;

        public DateTime SavedAtUtc => DateTime.ParseExact(savedAtUtc, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        public FirstServiceSaveSlot Slot => slot == "autosave" ? FirstServiceSaveSlot.Auto
            : slot == "democomplete" ? FirstServiceSaveSlot.DemoComplete : FirstServiceSaveSlot.Manual;

        internal FirstServiceSaveData(FirstServiceSaveSlot destination, int day, int gold, IEnumerable<MaterialCount> spices, DateTime timestamp,
            IEnumerable<MaterialCount> materials = null, IEnumerable<VisitResult> pendingReports = null,
            IEnumerable<VisitAffinity> visitAffinities = null, int wordOfMouthBonus = 0,
            IEnumerable<GuestRecord> guests = null, int? daySeed = null, DayProgress progress = null, FloorProgressState floors = null,
            ICollection<string> knownSpices = null)
        {
            if (day < 1) throw new ArgumentOutOfRangeException(nameof(day));
            if (gold < 0) throw new ArgumentOutOfRangeException(nameof(gold));
            SchemaVersion = CurrentSchemaVersion;
            Stage = PreparationStage;
            Day = day;
            Gold = gold;
            this.spices = new List<MaterialCount>(spices ?? Array.Empty<MaterialCount>()).ToArray();
            this.floors = floors;
            savedAtUtc = timestamp.ToString("O", CultureInfo.InvariantCulture);
            slot = SlotName(destination);
            this.materials = materials == null ? null : new List<MaterialCount>(materials).ToArray();
            this.pendingReports = pendingReports == null ? null : new List<VisitResult>(pendingReports).ToArray();
            this.visitAffinities = visitAffinities == null ? null : new List<VisitAffinity>(visitAffinities).ToArray();
            this.wordOfMouthBonus = wordOfMouthBonus;
            this.guests = guests == null ? null : new List<GuestRecord>(guests).ToArray();
            this.daySeed = daySeed;
            this.progress = progress;
            if (!ContentIsValid(knownSpices))
                throw new ArgumentException("양념·재료 수량, 귀환 보고, 방문 호감, 입소문 인원, 손님 명부 또는 층 진행이 올바르지 않습니다.");
        }

        /// <param name="knownSpices">시장 기준의 양념 id. null이면 id 검사를 건너뛴다.</param>
        internal bool IsValidFor(FirstServiceSaveSlot expectedSlot, ICollection<string> knownSpices = null)
        {
            return SchemaVersion >= OldestReadableSchemaVersion && SchemaVersion <= CurrentSchemaVersion &&
                Stage == PreparationStage && Day >= 1 &&
                Gold >= 0 && garlicStock >= 0 && slot == SlotName(expectedSlot) && ContentIsValid(knownSpices) &&
                DateTime.TryParseExact(savedAtUtc, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var time) &&
                time.Kind == DateTimeKind.Utc;
        }

        // 손상된 파일의 재료·보고·호감이 재고, 보고 목록, 다음 영업 인원에 들어가지 않도록 읽을 때와 쓸 때 같은 기준으로 검사한다.
        private bool ContentIsValid(ICollection<string> knownSpices)
        {
            if (wordOfMouthBonus < 0 || progress != null && !progress.Validate() || !SpicesAreValid(Spices, knownSpices) || progress != null && !SpicesAreValid(progress.Spices, knownSpices) || floors != null && !floors.Validate())
                return false;
            var guestIds = new HashSet<string>();
            foreach (var guest in Guests)
                if (guest == null || string.IsNullOrEmpty(guest.GuestId) || !guestIds.Add(guest.GuestId) ||
                    string.IsNullOrEmpty(guest.Name) || guest.Affinity < 0 || guest.ReturnDay < 1)
                    return false;
            var affinityVisits = new HashSet<string>();
            foreach (var affinity in VisitAffinities)
                if (affinity == null || string.IsNullOrEmpty(affinity.VisitId) || !affinityVisits.Add(affinity.VisitId) ||
                    affinity.End == ServiceVisitEnd.None || !Enum.IsDefined(typeof(ServiceVisitEnd), affinity.End))
                    return false;
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
                // 위험·효과 칸은 나중에 추가된 선택 항목이다. 없으면 통과하고, 있으면 여섯 축의 0 이상 값이어야 한다.
                foreach (var cells in new[] { report.Risks, report.Protections })
                {
                    if (cells == null) continue;
                    if (cells.Count != RiskAxes.All.Count) return false;
                    foreach (int cell in cells)
                        if (cell < 0) return false;
                }
                foreach (string loot in report.Loot)
                    if (string.IsNullOrEmpty(loot)) return false;
            }
            return true;
        }

        /// <summary>시장 기준(SO_Market)의 양념 id. 기준이 없으면 null. Resources를 읽으므로 메인 스레드에서만 부른다.</summary>
        public static ICollection<string> LoadKnownSpices()
        {
            var market = MarketRules.Load();
            if (!market) return null;
            var ids = new HashSet<string>();
            foreach (var spice in market.Spices) ids.Add(spice.id);
            return ids;
        }

        // 스키마 5까지의 마늘 개수를 양념 목록으로 바꾼다. 0개면 빈 목록.
        internal static IReadOnlyList<MaterialCount> LegacyGarlic(int count) =>
            count > 0 ? new[] { new MaterialCount(LegacyGarlicId, count) } : Array.Empty<MaterialCount>();

        /// <summary>양념 재고 목록이 올바른지: id가 비거나 겹치지 않고, 0 이상이며, 알려 준 시장 양념 id 안에 있다(null이면 id 검사 생략).</summary>
        internal static bool SpicesAreValid(IEnumerable<MaterialCount> list, ICollection<string> knownSpices = null)
        {
            var seen = new HashSet<string>();
            foreach (var spice in list)
                if (spice == null || string.IsNullOrEmpty(spice.Id) || !seen.Add(spice.Id) || spice.Count < 0 ||
                    knownSpices != null && !knownSpices.Contains(spice.Id))
                    return false;
            return true;
        }

        internal static string SlotName(FirstServiceSaveSlot value)
        {
            switch (value)
            {
                case FirstServiceSaveSlot.Auto: return "autosave";
                case FirstServiceSaveSlot.Manual: return "manual";
                case FirstServiceSaveSlot.DemoComplete: return "democomplete";
                default: throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }
}
