// =====================================================================
// GuestbookLog — 그날의 방명록 글 (순수 C#)
// 호출자 : RelationshipManager (TryBegin, Tick), GameManager·DayFlowPresenter (AllComplete, Entries, EntryCompleted)
// 상태   : 방문별 글(손님 ID·방문 ID·이름·소감·남은 작성 시간·완성 여부)
// 의사코드:
//   TryBegin — 서빙이 끝난 방문의 글을 '작성 중'으로 등록한다. 같은 방문은 두 번 등록하지 않는다.
//   Tick(초) — 작성 중인 글의 남은 시간을 줄이고, 0이 되면 완성으로 바꿔 한 번 알린다.
//   AllComplete — 등록된 글이 모두 완성됐는지. 돌려보낸 손님은 글이 없으므로 막지 않는다.
// =====================================================================
using System;
using System.Collections.Generic;

namespace DOB.Systems
{
    /// <summary>방명록 글 한 건. 같은 이름이어도 GuestId로 구분한다.</summary>
    public sealed class GuestbookEntry
    {
        public string GuestId { get; }
        public string VisitId { get; }
        public string GuestName { get; }
        /// <summary>그 식사의 맛 소감 한 줄.</summary>
        public string Line { get; }
        public bool IsComplete => RemainingSeconds <= 0f;
        internal float RemainingSeconds { get; set; }

        internal GuestbookEntry(string guestId, string visitId, string guestName, string line, float writeSeconds)
        {
            GuestId = guestId;
            VisitId = visitId;
            GuestName = guestName;
            Line = line;
            RemainingSeconds = writeSeconds;
        }
    }

    /// <summary>방명록 작성과 완성 여부를 관리한다.</summary>
    public sealed class GuestbookLog
    {
        private readonly float writeSeconds;
        private readonly List<GuestbookEntry> entries = new List<GuestbookEntry>();

        /// <summary>방문 순서대로 등록된 글.</summary>
        public IReadOnlyList<GuestbookEntry> Entries => entries;
        /// <summary>글이 완성될 때 한 번 알린다.</summary>
        public event Action<GuestbookEntry> EntryCompleted;

        public bool AllComplete
        {
            get
            {
                foreach (var entry in entries)
                    if (!entry.IsComplete) return false;
                return true;
            }
        }

        public GuestbookLog(float writeSeconds)
        {
            if (writeSeconds < 0f)
                throw new ArgumentOutOfRangeException(nameof(writeSeconds));
            this.writeSeconds = writeSeconds;
        }

        /// <summary>서빙이 끝난 방문의 글을 등록한다. ID가 비었거나 이미 등록한 방문이면 false.</summary>
        public bool TryBegin(string guestId, string visitId, string guestName, string line)
        {
            if (string.IsNullOrEmpty(guestId) || string.IsNullOrEmpty(visitId))
                return false;
            foreach (var entry in entries)
                if (entry.VisitId == visitId) return false;
            var added = new GuestbookEntry(guestId, visitId, guestName, line, writeSeconds);
            entries.Add(added);
            // 작성 시간이 0이면 등록과 동시에 완성된다.
            if (added.IsComplete)
                EntryCompleted?.Invoke(added);
            return true;
        }

        /// <summary>작성 중인 글의 시간을 흘린다. 정지 중이면 0을 넘긴다.</summary>
        public void Tick(float deltaSeconds)
        {
            if (deltaSeconds <= 0f)
                return;
            foreach (var entry in entries)
            {
                if (entry.IsComplete)
                    continue;
                entry.RemainingSeconds -= deltaSeconds;
                if (entry.IsComplete)
                    EntryCompleted?.Invoke(entry);
            }
        }
    }
}
