// 방문 호감과 입소문 추가 인원의 기준값. RelationshipManager가 읽어 AffinityJudge에 넘긴다.
// 의사코드: 값은 인스펙터(08.Data/SO_AffinityRules)에서만 정한다. 등급별 점수는 GradePoints로 조회한다.
// Validate가 방문 최대 > 0, 문턱 퍼센트 1~100·엄격한 오름차순, 누적 상한 ≥ 1, 휴식 일수 ≥ 0을 검사하고 첫 오류를 돌려준다. 검사 실패면 GameManager.Prepare가 실패한다.
// 문턱은 부동소수 올림 오차를 피하려고 정수 퍼센트로 둔다 (AffinityJudge.ExtraGuests).
// 💡 시험값(대성공 +2·성공 +1·실패 −1·대실패 −2, 돌려보냄·대기 이탈 −4, 방문 최대 6, 문턱 60·75·85·100%, 돌려보낸 방문 제외)은
// 송예찬 확인 전이다 (FeatureManagerMap F10 「입소문으로 다음 영업의 손님 수 정하기」).
// 누적 호감 상한(💡 36)과 부상 휴식 일수(💡 2)는 GuestLedger가 마감 정산에서 쓴다 (기획 4-7·4-8, 2026-11-25).
using UnityEngine;

namespace DOB.Define
{
    /// <summary>방문 호감·입소문 기준값. 실행 중 기록은 RelationshipManager가 갖는다.</summary>
    [CreateAssetMenu(menuName = "DOB/호감·입소문 기준", fileName = "SO_AffinityRules")]
    public sealed class AffinityRules : ScriptableObject
    {
        /// <summary>탐험 대성공일 때 방문 호감에 더하는 점수(점).</summary>
        [SerializeField] private int greatSuccessPoints = 2;
        /// <summary>탐험 성공일 때 더하는 점수(점).</summary>
        [SerializeField] private int successPoints = 1;
        /// <summary>탐험 실패일 때 더하는 점수(점). 보통 음수.</summary>
        [SerializeField] private int failurePoints = -1;
        /// <summary>탐험 대실패일 때 더하는 점수(점). 보통 음수.</summary>
        [SerializeField] private int greatFailurePoints = -2;
        /// <summary>응대 중 돌려보낸 방문의 호감 변화(점). 다른 점수는 더하지 않는다. 보통 음수.</summary>
        [SerializeField] private int sentHomePoints = -4;
        /// <summary>대기 한도를 넘겨 떠난 방문의 호감 변화(점). 다른 점수는 더하지 않는다. 보통 음수.</summary>
        [SerializeField] private int waitedOutPoints = -4;
        /// <summary>한 방문의 호감 최대 점수(점). 하루 최대 = 이 값 × 손님 수. 1 이상.</summary>
        [SerializeField] private int visitMaxPoints = 6;
        /// <summary>입소문 문턱(하루 최대 대비 %). 1~100, 엄격한 오름차순. 도달한 문턱 하나당 다음 영업 1명 추가.</summary>
        [SerializeField] private int[] thresholdPercents = { 60, 75, 85, 100 };
        /// <summary>돌려보낸 방문을 입소문의 손님 수·합계에 넣는지. 넣으면 N에 1명, S에 감점을 방문당 한 번 더한다 (2026-11-25 확정).</summary>
        [SerializeField] private bool countSentHome = true;
        /// <summary>손님 한 명의 누적 호감 상한(점). 1 이상. 넘는 값은 이 값으로 맞춘다.</summary>
        [SerializeField] private int affinityCap = 36;
        /// <summary>탐험 대실패(부상) 뒤 오지 않는 영업 일수(일). 0 이상. 2이면 D+1·D+2에 쉬고 D+3에 돌아온다.</summary>
        [SerializeField] private int injuryRestDays = 2;

        public int SentHomePoints => sentHomePoints;
        public int WaitedOutPoints => waitedOutPoints;
        public int VisitMaxPoints => visitMaxPoints;
        public System.Collections.Generic.IReadOnlyList<int> ThresholdPercents => thresholdPercents;
        public bool CountSentHome => countSentHome;
        public int AffinityCap => affinityCap;
        public int InjuryRestDays => injuryRestDays;

        /// <summary>탐험 등급의 호감 점수.</summary>
        public int GradePoints(ExpeditionGrade grade)
        {
            switch (grade)
            {
                case ExpeditionGrade.GreatSuccess: return greatSuccessPoints;
                case ExpeditionGrade.Success: return successPoints;
                case ExpeditionGrade.Failure: return failurePoints;
                case ExpeditionGrade.GreatFailure: return greatFailurePoints;
                default: throw new System.ArgumentOutOfRangeException(nameof(grade));
            }
        }

        /// <summary>규칙에 쓰는 값을 검사한다. 문제가 없으면 null, 있으면 첫 오류 설명.</summary>
        public string Validate()
        {
            if (visitMaxPoints < 1) return "방문 최대 점수는 1 이상이어야 합니다.";
            if (affinityCap < 1) return "누적 호감 상한은 1 이상이어야 합니다.";
            if (injuryRestDays < 0) return "부상 휴식 일수는 0 이상이어야 합니다.";
            if (thresholdPercents == null) return "입소문 문턱 목록이 없습니다.";
            int previous = 0;
            foreach (int percent in thresholdPercents)
            {
                if (percent < 1 || percent > 100) return "입소문 문턱은 1~100%여야 합니다.";
                if (percent <= previous) return "입소문 문턱은 오름차순이어야 합니다.";
                previous = percent;
            }
            return null;
        }
    }
}
