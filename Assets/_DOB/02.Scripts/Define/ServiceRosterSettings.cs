// 본영업 하루의 손님 수·입장 간격·입구 줄·대기 점수 기준값. GuestManager가 읽어 ServiceRoster를 만든다.
// 의사코드: 값은 인스펙터(08.Data/SO_ServiceRoster)에서만 정한다. Validate가 1일차 인원 ≥ 1, 최소 인원 ≥ 0, 첫 입장 대기 ≥ 0, 0 < 최소 ≤ 최대, 줄 ≥ 1,
// 점수 간격·이탈 한도 > 0, 시작 점수 ≥ 0을 검사하고 첫 오류를 돌려준다. 검사 실패면 GameManager.Prepare가 실패한다.
// 그날 인원은 재방문 손님 + 입소문 신규 손님으로 정한다(DailyRoster). 별도의 성장표는 없다 (2026-11-25 송예찬 결정).
// 💡 최소 인원(1명)은 손님이 0명이 되는 날을 막는 임시 규칙이다. 0이면 꺼진다 (기획 🔴 「손님 0명 엣지 케이스」, 송예찬 확인 전).
// 💡 시험값(3명, 첫 입장 3초, 8~20초, 줄 2명, 2점, 10초, 30초)은 송예찬 확인 전이다 (FeatureManagerMap F1·F2·F10 시험안).
using UnityEngine;

namespace DOB.Define
{
    /// <summary>하루 영업의 손님 명단 기준값. 실행 중 방문 상태는 ServiceRoster가 갖는다.</summary>
    [CreateAssetMenu(menuName = "DOB/영업 손님 기준", fileName = "SO_ServiceRoster")]
    public sealed class ServiceRosterSettings : ScriptableObject
    {
        /// <summary>1일차에 받는 신규 손님 수(명). 1 이상. 2일차부터는 재방문 손님 + 입소문 신규 손님으로 정한다.</summary>
        [SerializeField] private int baseGuests = 3;
        /// <summary>그날 예정 인원이 이보다 적으면 신규 손님으로 채우는 최소 인원(명). 0 이상. 0이면 채우지 않는다.</summary>
        [SerializeField] private int minGuests = 1;
        /// <summary>영업 입장 시계가 흐르기 시작한 뒤 첫 손님이 들어오기까지의 시간(초). 0 이상.</summary>
        [SerializeField] private float firstArrivalSeconds = 3f;
        /// <summary>다음 입장까지의 최소 간격(초). 0보다 크다.</summary>
        [SerializeField] private float arrivalMinSeconds = 8f;
        /// <summary>다음 입장까지의 최대 간격(초). 최소 이상.</summary>
        [SerializeField] private float arrivalMaxSeconds = 20f;
        /// <summary>입구에서 맞이를 기다릴 수 있는 최대 인원(명). 1 이상.</summary>
        [SerializeField] private int queueCapacity = 2;
        /// <summary>대기 점수의 시작값(점). 0 이상.</summary>
        [SerializeField] private int waitStartScore = 2;
        /// <summary>대기 점수가 1점 내려가는 대기 시간(초). 0보다 크다.</summary>
        [SerializeField] private float scoreStepSeconds = 10f;
        /// <summary>서빙을 받지 못하고 기다릴 수 있는 한도(초). 이에 이르면 대기 이탈. 0보다 크다.</summary>
        [SerializeField] private float leaveAfterSeconds = 30f;
        /// <summary>입구 줄에서 손님 사이의 간격(m). 배치 값이라 규칙에는 쓰지 않는다.</summary>
        [SerializeField, Min(0.1f)] private float queueSpacing = 0.9f;
        /// <summary>서빙 직후 손님 말풍선에 기쁨을 보이는 시간(초). 💡 표시용 값이며 규칙에는 쓰지 않는다.</summary>
        [SerializeField, Min(0f)] private float servedMoodSeconds = 1.5f;

        public int BaseGuests => baseGuests;
        public int MinGuests => minGuests;
        public float FirstArrivalSeconds => firstArrivalSeconds;
        public float ArrivalMinSeconds => arrivalMinSeconds;
        public float ArrivalMaxSeconds => arrivalMaxSeconds;
        public int QueueCapacity => queueCapacity;
        public int WaitStartScore => waitStartScore;
        public float ScoreStepSeconds => scoreStepSeconds;
        public float LeaveAfterSeconds => leaveAfterSeconds;
        public float QueueSpacing => queueSpacing;
        public float ServedMoodSeconds => servedMoodSeconds;

        /// <summary>규칙에 쓰는 값을 검사한다. 문제가 없으면 null, 있으면 첫 오류 설명.</summary>
        public string Validate()
        {
            if (baseGuests < 1) return "기본 인원은 1명 이상이어야 합니다.";
            if (minGuests < 0) return "최소 인원은 0 이상이어야 합니다.";
            if (!(firstArrivalSeconds >= 0f) || float.IsInfinity(firstArrivalSeconds)) return "첫 입장 대기는 0 이상이어야 합니다.";
            if (!(arrivalMinSeconds > 0f) || !(arrivalMaxSeconds >= arrivalMinSeconds) || float.IsInfinity(arrivalMaxSeconds))
                return "입장 간격은 0 < 최소 ≤ 최대여야 합니다.";
            if (queueCapacity < 1) return "입구 줄 인원은 1명 이상이어야 합니다.";
            if (waitStartScore < 0) return "대기 시작 점수는 0 이상이어야 합니다.";
            if (!(scoreStepSeconds > 0f) || float.IsInfinity(scoreStepSeconds)) return "대기 점수 간격은 0보다 커야 합니다.";
            if (!(leaveAfterSeconds > 0f) || float.IsInfinity(leaveAfterSeconds)) return "대기 이탈 한도는 0보다 커야 합니다.";
            return null;
        }
    }
}
