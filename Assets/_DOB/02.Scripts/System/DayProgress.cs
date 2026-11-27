// 수동 저장에 일차 시작 스냅샷과 함께 넣는 「누른 시점」의 진행 상태 (F13, 2026-11-25 결정).
// 저장(FirstServiceSaveData 스키마 5)이 담고, 불러올 때 GameManager가 일차 시작으로 하루를 연 뒤 각 System에 복원을 요청한다.
// 재고는 따로 저장하지 않는다. 「시작 재고 + 확정한 보고의 전리품」으로 다시 계산되므로 같은 보고를 두 번 받지 않는다(FeatureManagerMap A·B 사례).
// 의사코드: Validate — 단계가 아침 보고·준비·영업 중 하나, 금화·마늘 ≥ 0, 확정 보고 ID 중복 없음.
//           영업 단계면 명단이 있고, 방문 ID가 겹치지 않으며 대기·남은 시간이 0 이상, 주문·방명록·결과의 방문 ID가 모두 명단에 있어야 한다.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>명단 안 방문 하나의 저장 상태.</summary>
    [DataContract]
    public sealed class VisitProgress
    {
        [DataMember(Name = "visitId")] public string VisitId { get; private set; }
        [DataMember(Name = "guestId")] public string GuestId { get; private set; }
        // Mono JSON 직렬화기에서 enum 표현이 달라지지 않도록 숫자로 저장한다.
        [DataMember(Name = "stage")] private int stage;
        [DataMember(Name = "end")] private int end;
        [DataMember(Name = "waitSeconds")] public float WaitSeconds { get; private set; }
        [DataMember(Name = "waitScore")] public int? WaitScore { get; private set; }
        [DataMember(Name = "tasteScore")] public int? TasteScore { get; private set; }
        /// <summary>앉은 좌석 번호. 앉지 않았으면 -1.</summary>
        [DataMember(Name = "seat")] public int Seat { get; private set; }

        public ServiceVisitStage Stage => (ServiceVisitStage)stage;
        public ServiceVisitEnd End => (ServiceVisitEnd)end;

        public VisitProgress(string visitId, string guestId, ServiceVisitStage stage, ServiceVisitEnd end, float waitSeconds,
            int? waitScore, int? tasteScore, int seat)
        {
            VisitId = visitId;
            GuestId = guestId;
            this.stage = (int)stage;
            this.end = (int)end;
            WaitSeconds = waitSeconds;
            WaitScore = waitScore;
            TasteScore = tasteScore;
            Seat = seat;
        }
    }

    /// <summary>그날 명단의 저장 상태.</summary>
    [DataContract]
    public sealed class RosterProgress
    {
        [DataMember(Name = "nextArrivalSeconds")] public float NextArrivalSeconds { get; private set; }
        [DataMember(Name = "arrivalDue")] public bool ArrivalDue { get; private set; }
        [DataMember(Name = "visits")] private VisitProgress[] visits;

        /// <summary>입장한 순서의 방문.</summary>
        public IReadOnlyList<VisitProgress> Visits => visits ?? Array.Empty<VisitProgress>();

        public RosterProgress(float nextArrivalSeconds, bool arrivalDue, IEnumerable<VisitProgress> visits)
        {
            NextArrivalSeconds = nextArrivalSeconds;
            ArrivalDue = arrivalDue;
            this.visits = visits == null ? Array.Empty<VisitProgress>() : new List<VisitProgress>(visits).ToArray();
        }
    }

    /// <summary>주문 하나의 저장 상태.</summary>
    [DataContract]
    public sealed class OrderProgress
    {
        [DataMember(Name = "visitId")] public string VisitId { get; private set; }
        [DataMember(Name = "state")] private int state;
        [DataMember(Name = "remainingSeconds")] public float RemainingSeconds { get; private set; }
        [DataMember(Name = "usedGarlic")] public int UsedGarlic { get; private set; }

        public FirstGuestOrderState State => (FirstGuestOrderState)state;

        public OrderProgress(string visitId, FirstGuestOrderState state, float remainingSeconds, int usedGarlic)
        {
            VisitId = visitId;
            this.state = (int)state;
            RemainingSeconds = remainingSeconds;
            UsedGarlic = usedGarlic;
        }
    }

    /// <summary>방명록 글 하나의 저장 상태.</summary>
    [DataContract]
    public sealed class GuestbookProgress
    {
        [DataMember(Name = "guestId")] public string GuestId { get; private set; }
        [DataMember(Name = "visitId")] public string VisitId { get; private set; }
        [DataMember(Name = "guestName")] public string GuestName { get; private set; }
        [DataMember(Name = "line")] public string Line { get; private set; }
        [DataMember(Name = "remainingSeconds")] public float RemainingSeconds { get; private set; }

        public GuestbookProgress(string guestId, string visitId, string guestName, string line, float remainingSeconds)
        {
            GuestId = guestId;
            VisitId = visitId;
            GuestName = guestName;
            Line = line;
            RemainingSeconds = remainingSeconds;
        }
    }

    /// <summary>수동 저장을 누른 시점의 하루 진행.</summary>
    [DataContract]
    public sealed class DayProgress
    {
        [DataMember(Name = "phase")] private int phase;
        [DataMember(Name = "confirmedReports")] private string[] confirmedReports;
        [DataMember(Name = "gold")] public int Gold { get; private set; }
        // 스키마 5는 마늘 개수만 저장했다. 스키마 6부터는 쓰지 않고 spices를 쓴다.
        [DataMember(Name = "garlic", EmitDefaultValue = false)] private int garlic;
        [DataMember(Name = "spices")] private MaterialCount[] spices;
        // 아래는 영업 단계에서만 쓴다.
        [DataMember(Name = "roster")] public RosterProgress Roster { get; private set; }
        [DataMember(Name = "orders")] private OrderProgress[] orders;
        [DataMember(Name = "guestbook")] private GuestbookProgress[] guestbook;
        [DataMember(Name = "results")] private VisitResult[] results;

        public DayPhase Phase => (DayPhase)phase;
        /// <summary>누른 시점의 양념 재고. 스키마 5 진행이면 마늘만 있다.</summary>
        public IReadOnlyList<MaterialCount> Spices => spices ?? FirstServiceSaveData.LegacyGarlic(garlic);
        /// <summary>이미 확정한 귀환 보고의 방문 ID.</summary>
        public IReadOnlyList<string> ConfirmedReports => confirmedReports ?? Array.Empty<string>();
        public IReadOnlyList<OrderProgress> Orders => orders ?? Array.Empty<OrderProgress>();
        public IReadOnlyList<GuestbookProgress> Guestbook => guestbook ?? Array.Empty<GuestbookProgress>();
        /// <summary>오늘 서빙해 확정한 방문 결과(전리품은 비어 있다).</summary>
        public IReadOnlyList<VisitResult> Results => results ?? Array.Empty<VisitResult>();

        public DayProgress(DayPhase phase, IEnumerable<string> confirmedReports, int gold, IEnumerable<MaterialCount> spices, RosterProgress roster = null,
            IEnumerable<OrderProgress> orders = null, IEnumerable<GuestbookProgress> guestbook = null, IEnumerable<VisitResult> results = null)
        {
            this.phase = (int)phase;
            this.confirmedReports = confirmedReports == null ? null : new List<string>(confirmedReports).ToArray();
            Gold = gold;
            this.spices = new List<MaterialCount>(spices ?? Array.Empty<MaterialCount>()).ToArray();
            Roster = roster;
            this.orders = orders == null ? null : new List<OrderProgress>(orders).ToArray();
            this.guestbook = guestbook == null ? null : new List<GuestbookProgress>(guestbook).ToArray();
            this.results = results == null ? null : new List<VisitResult>(results).ToArray();
        }

        /// <summary>진행 상태끼리 맞는지. 손상된 저장이 명단·주문·재고에 들어가지 않게 읽을 때와 쓸 때 같은 기준으로 검사한다.</summary>
        public bool Validate()
        {
            if (Phase != DayPhase.MorningReport && Phase != DayPhase.Preparation && Phase != DayPhase.Service)
                return false;
            if (Gold < 0 || garlic < 0 || !FirstServiceSaveData.SpicesAreValid(Spices))
                return false;
            var confirmed = new HashSet<string>();
            foreach (string id in ConfirmedReports)
                if (string.IsNullOrEmpty(id) || !confirmed.Add(id))
                    return false;
            if (Phase != DayPhase.Service)
                return Roster == null && Orders.Count == 0 && Guestbook.Count == 0 && Results.Count == 0;
            if (Roster == null || Roster.NextArrivalSeconds < 0f)
                return false;

            var visitIds = new HashSet<string>();
            foreach (var visit in Roster.Visits)
                if (visit == null || string.IsNullOrEmpty(visit.VisitId) || string.IsNullOrEmpty(visit.GuestId) || !visitIds.Add(visit.VisitId)
                    || visit.WaitSeconds < 0f || !Enum.IsDefined(typeof(ServiceVisitStage), visit.Stage)
                    || !Enum.IsDefined(typeof(ServiceVisitEnd), visit.End) || (visit.Stage == ServiceVisitStage.Ended) != (visit.End != ServiceVisitEnd.None))
                    return false;
            var orderIds = new HashSet<string>();
            foreach (var order in Orders)
                if (order == null || !visitIds.Contains(order.VisitId) || !orderIds.Add(order.VisitId) || order.RemainingSeconds < 0f
                    || order.UsedGarlic < 0 || !Enum.IsDefined(typeof(FirstGuestOrderState), order.State))
                    return false;
            foreach (var entry in Guestbook)
                if (entry == null || !visitIds.Contains(entry.VisitId) || float.IsNaN(entry.RemainingSeconds) || float.IsInfinity(entry.RemainingSeconds))
                    return false;
            foreach (var result in Results)
                if (result == null || !visitIds.Contains(result.VisitId))
                    return false;
            return true;
        }
    }
}
