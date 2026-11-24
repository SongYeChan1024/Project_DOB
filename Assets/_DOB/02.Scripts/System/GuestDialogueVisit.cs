using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>방문 소유자가 정한 손님 정보. 축·종족·직업은 규칙 자산의 한글 이름과 대응한다.</summary>
    public sealed class GuestVisitInput
    {
        public string GuestId, VisitId, GuestName, Race, Job;
        public int Day, Floor, Affinity, EventFloor;
        public bool IsTired;
        public string PersonalWeakness, EventAxis;
    }

    public sealed class GuestVisitClue
    {
        public string Key { get; }
        public string CauseId { get; }
        public DialogueCondition Condition { get; }
        public string Axis { get; }
        public int Delta { get; }
        public string Text { get; }
        public DialogueClueChannel Channel { get; }

        internal GuestVisitClue(DialogueCauseRule cause, DialogueClueChannel channel, string text)
        {
            CauseId = cause.causeId;
            Condition = cause.condition;
            Axis = cause.axis;
            Delta = cause.delta;
            Channel = channel;
            Text = text;
            Key = cause.causeId + "/" + channel + "/" + text;
        }
    }

    /// <summary>한 방문에서 고른 원고와 발견 기록. UI를 다시 열 때 같은 인스턴스를 읽는다.</summary>
    public sealed class GuestDialogueVisit
    {
        public string GuestId { get; }
        public string VisitId { get; }
        public string GuestName { get; }
        public string Race { get; }
        public string Job { get; }
        public int Floor { get; }
        public string Line { get; }
        public IReadOnlyList<GuestVisitClue> Clues { get; }

        private readonly Dictionary<string, int> actualRisk;
        private readonly BreadDialogueLines breadLines;
        private readonly Dictionary<string, GuestVisitClue> cluesByKey = new Dictionary<string, GuestVisitClue>();
        private readonly HashSet<string> foundKeys = new HashSet<string>();
        private readonly HashSet<string> foundCauses = new HashSet<string>();
        private readonly Dictionary<string, int> knownRisk = new Dictionary<string, int>();

        internal GuestDialogueVisit(GuestVisitInput input, string line, List<GuestVisitClue> clues,
            Dictionary<string, int> actualRisk, BreadDialogueLines breadLines)
        {
            GuestId = input.GuestId;
            VisitId = input.VisitId;
            GuestName = input.GuestName;
            Race = input.Race;
            Job = input.Job;
            Floor = input.Floor;
            Line = line;
            Clues = clues.AsReadOnly();
            this.actualRisk = actualRisk;
            this.breadLines = breadLines;
            foreach (var clue in clues) cluesByKey.Add(clue.Key, clue);
        }

        public int ActualRisk(string axis) => actualRisk.TryGetValue(axis, out int n) ? Math.Max(0, n) : 0;
        public bool IsDiscovered(string key) => foundKeys.Contains(key);
        public string TasteLine(bool hasGarlic) => hasGarlic ? breadLines.garlicTaste : breadLines.plainTaste;

        public string DepartureLine(int shortfall)
        {
            if (shortfall < 0) throw new ArgumentOutOfRangeException(nameof(shortfall));
            if (shortfall == 0) return breadLines.fullyPrepared;
            return shortfall == 1 ? breadLines.oneShort : breadLines.severalShort;
        }

        /// <summary>미발견은 null. 성표를 먼저 찾은 음수 보정은 내부에 남기고 표시만 0으로 제한한다.</summary>
        public int? NotebookRisk(string axis) =>
            knownRisk.TryGetValue(axis, out int n) ? Math.Max(0, n) : (int?)null;

        /// <summary>새 표현·관찰이면 true. 같은 원인을 다른 표현으로 찾아도 위험 증감은 한 번뿐이다.</summary>
        public bool Discover(string key)
        {
            if (key == null || !cluesByKey.TryGetValue(key, out var clue) || !foundKeys.Add(key))
                return false;
            if (foundCauses.Add(clue.CauseId))
            {
                knownRisk.TryGetValue(clue.Axis, out int n);
                knownRisk[clue.Axis] = n + clue.Delta;
            }
            return true;
        }
    }
}
