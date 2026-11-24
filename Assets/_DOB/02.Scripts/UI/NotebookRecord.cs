// =====================================================================
// NotebookRecord — 수첩에 적힌 단서 기록 (찾은 단서 · 도감)
// 역할   : 플레이어가 찾은 단서를 손님별로 기억하고,
//          종족 · 직업 · 층별 도감에 무엇이 적혔는지 모아 둔다.
// 호출자 : GuestDialogueView (단서를 찾을 때 Add, 화면을 그릴 때 읽기)
// 상태   : 이번 플레이 동안만 기억한다. (게임을 껐다 켜면 비어 있음)
// 비고   : 임시 기록 보관소. 💡 구성안(Docs/개발/FeatureManagerMap.md)에서는 수첩 기록을
//          RelationshipManager 아래 NotebookRecords System이 갖는다 → 생기면 이 클래스는 화면용 읽기 창구로 줄인다.
//          저장(세이브)은 SaveManager 데이터 계약(홍준우 · 김언호 공동)에 맞춰 붙인다.
// =====================================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>단서를 어디서 찾았는지.</summary>
    public enum ClueSource
    {
        /// <summary>손님 대사 속 키워드</summary>
        Line,
        /// <summary>초상화의 한 부분</summary>
        Portrait,
    }

    /// <summary>단서를 찾았을 때 도감의 어느 곳에도 적을지.</summary>
    public enum ClueRecordTarget
    {
        /// <summary>도감에는 안 적음 (단서 사전 · 위험 칸에만)</summary>
        None,
        /// <summary>종족 도감 (예: 인간)</summary>
        Race,
        /// <summary>직업 도감 (예: 마법사)</summary>
        Job,
        /// <summary>층별 위험 (예: 1층 슬라임 숲)</summary>
        Floor,
    }

    /// <summary>찾은 단서 하나.</summary>
    [Serializable]
    public sealed class FoundClue
    {
        /// <summary>손님 안에서 단서를 구분하는 이름 (예: "line:슬라임 점액", "portrait:0").</summary>
        public string key;
        /// <summary>태그에 보일 글자 (예: 슬라임 점액).</summary>
        public string tag;
        /// <summary>위험 이름 (독 · 열 · 냉기 · 어둠 · 체력 · 함정). 모르면 비어 있음.</summary>
        public string risk;
        public ClueSource source;
        public ClueRecordTarget recordTo;
    }

    /// <summary>도감 한 줄 (예: 인간 → [젖은 장화](냉기)).</summary>
    public sealed class CodexEntry
    {
        public string Name { get; }
        public List<FoundClue> Clues { get; } = new List<FoundClue>();

        public CodexEntry(string name) => Name = name;
    }

    /// <summary>수첩 기록. 손님별 찾은 단서와 도감 세 가지를 가진다.</summary>
    public sealed class NotebookRecord
    {
        // ---------------- 이번 플레이 동안 함께 쓰는 기록 ----------------

        /// <summary>손님 응대 화면이 따로 기록을 받지 못했을 때 쓰는 공용 기록.</summary>
        public static NotebookRecord Session { get; private set; } = new NotebookRecord();

        // Play를 누를 때마다 빈 수첩으로 시작한다.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetSession() => Session = new NotebookRecord();

        // ---------------- 기록 ----------------

        private readonly Dictionary<string, List<FoundClue>> cluesByGuest = new Dictionary<string, List<FoundClue>>();
        private readonly List<CodexEntry> races = new List<CodexEntry>();
        private readonly List<CodexEntry> jobs = new List<CodexEntry>();
        private readonly List<CodexEntry> floors = new List<CodexEntry>();

        public IReadOnlyList<CodexEntry> Races => races;
        public IReadOnlyList<CodexEntry> Jobs => jobs;
        public IReadOnlyList<CodexEntry> Floors => floors;

        /// <summary>이 손님의 이 단서를 이미 찾았는지.</summary>
        public bool Has(string guestId, string key)
        {
            return cluesByGuest.TryGetValue(guestId ?? string.Empty, out var list)
                   && list.Exists(c => c.key == key);
        }

        /// <summary>이 손님에게서 찾은 단서들 (찾은 순서대로).</summary>
        public IReadOnlyList<FoundClue> CluesOf(string guestId)
        {
            return cluesByGuest.TryGetValue(guestId ?? string.Empty, out var list)
                ? list
                : (IReadOnlyList<FoundClue>)Array.Empty<FoundClue>();
        }

        /// <summary>
        /// 모든 손님에게서 찾은 단서 (준비 화면 수첩의 단서 사전용).
        /// 다른 손님에게서 같은 단서(같은 곳 · 태그 · 위험)를 또 찾았으면 한 번만 준다.
        /// </summary>
        public IEnumerable<FoundClue> AllClues()
        {
            var seen = new HashSet<string>();
            foreach (var list in cluesByGuest.Values)
                foreach (var clue in list)
                    if (seen.Add(clue.source + "|" + clue.tag + "|" + clue.risk))
                        yield return clue;
        }

        /// <summary>
        /// 단서를 적는다. 이미 찾은 단서면 아무것도 안 하고 false.
        /// recordTo에 따라 종족 · 직업 · 층별 도감에도 같이 적는다.
        /// </summary>
        public bool Add(string guestId, FoundClue clue, string race, string job, string floor)
        {
            if (clue == null || Has(guestId, clue.key))
                return false;

            // 1) 손님별 기록
            string id = guestId ?? string.Empty;
            if (!cluesByGuest.TryGetValue(id, out var list))
                cluesByGuest[id] = list = new List<FoundClue>();
            list.Add(clue);

            // 2) 도감
            switch (clue.recordTo)
            {
                case ClueRecordTarget.Race:  AddToCodex(races, race, clue); break;
                case ClueRecordTarget.Job:   AddToCodex(jobs, job, clue); break;
                case ClueRecordTarget.Floor: AddToCodex(floors, floor, clue); break;
            }
            return true;
        }

        // 도감에서 이름(예: 인간)에 해당하는 줄을 찾거나 새로 만들고 단서를 붙인다.
        // 다른 손님에게서 같은 단서를 또 찾으면 한 번만 적는다.
        private static void AddToCodex(List<CodexEntry> codex, string name, FoundClue clue)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            var entry = codex.Find(e => e.Name == name);
            if (entry == null)
                codex.Add(entry = new CodexEntry(name));

            bool sameExists = entry.Clues.Exists(c => c.tag == clue.tag && c.risk == clue.risk);
            if (!sameExists)
                entry.Clues.Add(clue);
        }
    }
}
