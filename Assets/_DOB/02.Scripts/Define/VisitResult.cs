// 서빙 때 확정한 한 방문의 탐험 결과. VisitResultLog가 만들고, 저장(FirstServiceSaveData)과 귀환 보고(ReturnReportLog),
// 보고 화면이 같은 데이터를 읽는다. 만든 뒤에는 값을 바꾸지 않는다.
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DOB.Define
{
    /// <summary>귀환 보고 확정 요청의 결과.</summary>
    public enum ReportConfirmResult
    {
        /// <summary>전리품을 재고에 반영하고 처리됨으로 바꿨다.</summary>
        Confirmed,
        /// <summary>이미 처리한 보고라 아무것도 바꾸지 않았다.</summary>
        AlreadyConfirmed,
        /// <summary>현재 일차의 보고 목록에 없는 방문이다.</summary>
        NotFound
    }

    /// <summary>한 방문의 처방과 탐험 결과. 축별 부족은 RiskAxes.All 순서다.</summary>
    [DataContract]
    public sealed class VisitResult
    {
        [DataMember(Name = "guestId")] public string GuestId { get; private set; }
        [DataMember(Name = "visitId")] public string VisitId { get; private set; }
        [DataMember(Name = "guestName")] public string GuestName { get; private set; }
        /// <summary>목적지 층. 1부터.</summary>
        [DataMember(Name = "floor")] public int Floor { get; private set; }
        /// <summary>낸 요리의 이름.</summary>
        [DataMember(Name = "dish")] public string Dish { get; private set; }
        [DataMember(Name = "usedSeasoning")] public bool UsedSeasoning { get; private set; }
        [DataMember(Name = "shortfalls")] private int[] shortfalls;
        // Unity의 Mono JSON 직렬화기에서 enum 표현이 달라지지 않도록 숫자로 저장한다.
        [DataMember(Name = "grade")] private int grade;
        [DataMember(Name = "loot")] private string[] loot;
        // 이 두 항목이 없던 저장도 읽을 수 있게 필수로 두지 않는다. 없으면 null이다.
        [DataMember(Name = "risks")] private int[] risks;
        [DataMember(Name = "protections")] private int[] protections;

        /// <summary>RiskAxes.All 순서의 축별 부족 칸 수.</summary>
        public IReadOnlyList<int> Shortfalls => shortfalls;
        /// <summary>RiskAxes.All 순서의 그날 실제 위험 칸 수. 기록되지 않은 방문이면 null.</summary>
        public IReadOnlyList<int> Risks => risks;
        /// <summary>RiskAxes.All 순서의 요리·양념이 막은 칸 수. 기록되지 않은 방문이면 null.</summary>
        public IReadOnlyList<int> Protections => protections;
        public ExpeditionGrade Grade => (ExpeditionGrade)grade;
        /// <summary>전리품으로 받을 던전 재료 이름. 같은 재료가 여러 번 들어갈 수 있다.</summary>
        public IReadOnlyList<string> Loot => loot;

        public VisitResult(string guestId, string visitId, string guestName, int floor, string dish, bool usedSeasoning,
            int[] shortfalls, ExpeditionGrade grade, string[] loot, int[] risks = null, int[] protections = null)
        {
            this.risks = risks;
            this.protections = protections;
            GuestId = guestId;
            VisitId = visitId;
            GuestName = guestName;
            Floor = floor;
            Dish = dish;
            UsedSeasoning = usedSeasoning;
            this.shortfalls = shortfalls;
            this.grade = (int)grade;
            this.loot = loot;
        }
    }
}
