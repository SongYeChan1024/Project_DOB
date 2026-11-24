// 하루 진행 기준값. GameManager가 읽어 GuestbookLog(방명록 작성 시간)를 만든다.
// 💡 영업 시간 제한은 없다 (CurrentDecisions 「영업 시간 없음」, 팀 합의 전). 그날 손님을 모두 처리하면 마감한다.
using UnityEngine;

namespace DOB.Define
{
    /// <summary>하루 진행의 시간 기준값. 실행 중 남은 시간은 GuestbookLog가 갖는다.</summary>
    [CreateAssetMenu(menuName = "DOB/하루 진행", fileName = "SO_DayCycle")]
    public sealed class DayCycleSettings : ScriptableObject
    {
        [SerializeField, Min(0f)] private float guestbookWriteSeconds = 3f;

        /// <summary>서빙 뒤 손님이 방명록 글을 완성하기까지의 시간(초).</summary>
        public float GuestbookWriteSeconds => guestbookWriteSeconds;
    }
}
