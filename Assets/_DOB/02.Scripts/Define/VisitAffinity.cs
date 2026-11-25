// 마감 정산에서 확정한 한 방문의 호감 변화. AffinityJudge가 만들고, 저장(FirstServiceSaveData)과
// RelationshipManager의 전날 기록 조회가 같은 데이터를 읽는다. 만든 뒤에는 값을 바꾸지 않는다.
using System.Runtime.Serialization;

namespace DOB.Define
{
    /// <summary>한 방문의 호감 내역과 합계. 감점으로 끝난 방문은 대기·입맛·탐험이 0이고 합계가 감점이다.</summary>
    [DataContract]
    public sealed class VisitAffinity
    {
        [DataMember(Name = "guestId")] public string GuestId { get; private set; }
        [DataMember(Name = "visitId")] public string VisitId { get; private set; }
        // Unity의 Mono JSON 직렬화기에서 enum 표현이 달라지지 않도록 숫자로 저장한다.
        [DataMember(Name = "end")] private int end;
        /// <summary>서빙 때의 대기 점수(점).</summary>
        [DataMember(Name = "wait")] public int Wait { get; private set; }
        /// <summary>입맛 점수(점). 💡 입맛 채점 전까지 항상 0.</summary>
        [DataMember(Name = "taste")] public int Taste { get; private set; }
        /// <summary>탐험 등급 점수(점).</summary>
        [DataMember(Name = "expedition")] public int Expedition { get; private set; }
        /// <summary>이 방문의 호감 변화(점).</summary>
        [DataMember(Name = "total")] public int Total { get; private set; }

        public ServiceVisitEnd End => (ServiceVisitEnd)end;

        public VisitAffinity(string guestId, string visitId, ServiceVisitEnd end, int wait, int taste, int expedition, int total)
        {
            GuestId = guestId;
            VisitId = visitId;
            this.end = (int)end;
            Wait = wait;
            Taste = taste;
            Expedition = expedition;
            Total = total;
        }
    }
}
