// 손님 명부의 한 줄. 손님 ID에 고정된 신원과 날을 넘겨 보관하는 누적 호감·영구 거절·복귀 일차를 담는다.
// GuestLedger가 마감 정산에서 만들고 바꾸며, 저장(FirstServiceSaveData)과 그날 명단 구성(DailyRoster)이 같은 데이터를 읽는다.
// 만든 뒤에는 값을 바꾸지 않는다. 바뀌면 GuestLedger가 새 줄로 갈아 끼운다.
using System.Runtime.Serialization;

namespace DOB.Define
{
    /// <summary>손님 한 명의 고정 신원과 장기 상태.</summary>
    [DataContract]
    public sealed class GuestRecord
    {
        [DataMember(Name = "guestId")] public string GuestId { get; private set; }
        [DataMember(Name = "name")] public string Name { get; private set; }
        /// <summary>종족. 대사 규칙 에셋의 한글 이름과 대응한다.</summary>
        [DataMember(Name = "race")] public string Race { get; private set; }
        /// <summary>직업. 대사 규칙 에셋의 한글 이름과 대응한다.</summary>
        [DataMember(Name = "job")] public string Job { get; private set; }
        /// <summary>개인 취약점 축. 없으면 null.</summary>
        [DataMember(Name = "weakness")] public string Weakness { get; private set; }
        /// <summary>누적 호감(점). 0 이상 상한 이하.</summary>
        [DataMember(Name = "affinity")] public int Affinity { get; private set; }
        /// <summary>영구 거절 여부. 참이면 다시 오지 않는다.</summary>
        [DataMember(Name = "rejected")] public bool Rejected { get; private set; }
        /// <summary>다시 올 수 있는 첫 일차. 1 이상.</summary>
        [DataMember(Name = "returnDay")] public int ReturnDay { get; private set; }

        public GuestRecord(string guestId, string name, string race, string job, string weakness,
            int affinity = 0, bool rejected = false, int returnDay = 1)
        {
            GuestId = guestId;
            Name = name;
            Race = race;
            Job = job;
            Weakness = weakness;
            Affinity = affinity;
            Rejected = rejected;
            ReturnDay = returnDay;
        }

        /// <summary>그 일차에 올 수 있는지. 영구 거절이 아니고 복귀 일차가 됐으면 참.</summary>
        public bool CanVisitOn(int day) => !Rejected && ReturnDay <= day;
    }
}
