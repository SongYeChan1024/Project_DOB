// 하루 진행 기준값. GameManager가 읽어 GuestbookLog(방명록 작성 시간)를 만들고 데모 엔딩 서빙 인원 기준을 DayCycle에 넘긴다.
// 💡 영업 시간 제한은 없다 (CurrentDecisions 「영업 시간 없음」, 팀 합의 전). 그날 손님을 모두 처리하면 마감한다.
using UnityEngine;

namespace DOB.Define
{
    /// <summary>하루 진행의 시간 기준값. 실행 중 남은 시간은 GuestbookLog가 갖는다.</summary>
    [CreateAssetMenu(menuName = "DOB/하루 진행", fileName = "SO_DayCycle")]
    public sealed class DayCycleSettings : ScriptableObject
    {
        [SerializeField, Min(0f)] private float guestbookWriteSeconds = 3f;

        [SerializeField, Min(1)] private int demoEndingServedGuests = 12;

        /// <summary>서빙 뒤 손님이 방명록 글을 완성하기까지의 시간(초).</summary>
        public float GuestbookWriteSeconds => guestbookWriteSeconds;
        /// <summary>하루에 서빙을 마친 서로 다른 손님이 이 수 이상이면 마감 확인 뒤 데모 엔딩으로 간다(명). 1 이상.</summary>
        public int DemoEndingServedGuests => demoEndingServedGuests;
    }
}
