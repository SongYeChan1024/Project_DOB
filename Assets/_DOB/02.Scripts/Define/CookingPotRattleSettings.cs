// 냄비 달그락 연출(CookingPotRattle)이 읽는 연출 기준값. 조리 규칙·시간과는 무관한 외형 값이다.
using UnityEngine;

namespace DOB.Define
{
    /// <summary>조리 중 냄비 뚜껑이 들썩이는 간격·크기와 완성 튐의 기준값. 실행 중 상태는 갖지 않는다.</summary>
    [CreateAssetMenu(menuName = "DOB/냄비 달그락 연출", fileName = "SO_CookingPotRattle")]
    public sealed class CookingPotRattleSettings : ScriptableObject
    {
        [Header("쉬는 간격(초) — 진행도 0 → 1로 갈수록 짧아짐")]
        [SerializeField] private Vector2 restSecondsAtStart;
        [SerializeField] private Vector2 restSecondsAtEnd;
        [SerializeField, Min(0.01f)] private float burstGapSeconds;
        [SerializeField, Min(1)] private int burstCountMax;

        [Header("한 번의 들썩임")]
        [SerializeField, Min(0.01f)] private float liftSeconds;
        [SerializeField, Min(0.01f)] private float dropSeconds;
        [SerializeField, Min(0f)] private float liftHeight;
        [SerializeField, Min(0f)] private float tiltDegrees;
        [SerializeField, Range(0f, 1f)] private float bounce;
        [SerializeField, Range(0f, 1f)] private float startIntensity;
        [SerializeField, Range(0f, 1f)] private float randomness;

        [Header("착지 흔들림·돌기")]
        [SerializeField, Min(0f)] private float wobbleDegrees;
        [SerializeField, Min(0.1f)] private float wobbleFrequency;
        [SerializeField, Min(0.01f)] private float wobbleSeconds;
        [SerializeField, Min(0f)] private float spinDegrees;

        [Header("끓는 떨림·막바지")]
        [SerializeField, Min(0f)] private float trembleAmount;
        [SerializeField, Min(0.1f)] private float trembleFrequency;
        [SerializeField, Range(0f, 1f)] private float climaxFrom;

        [Header("몸통 눌림·통째 들썩")]
        [SerializeField, Range(0f, 0.3f)] private float bodySquash;
        [SerializeField, Min(0f)] private float bodyHopHeight;
        [SerializeField, Min(0.01f)] private float squashSeconds;

        [Header("완성 튐")]
        [SerializeField, Min(0f)] private float finishLiftHeight;
        [SerializeField, Min(0f)] private float finishTiltDegrees;
        [SerializeField, Min(0.01f)] private float finishSeconds;

        [Header("김(입자 수)")]
        [SerializeField, Min(0)] private int steamPerRattle;
        [SerializeField, Min(0)] private int steamOnFinish;

        /// <summary>진행도 0일 때 다음 묶음까지 쉬는 시간 범위(초, x=최소·y=최대).</summary>
        public Vector2 RestSecondsAtStart => restSecondsAtStart;
        /// <summary>진행도 1일 때 다음 묶음까지 쉬는 시간 범위(초, x=최소·y=최대).</summary>
        public Vector2 RestSecondsAtEnd => restSecondsAtEnd;
        /// <summary>한 묶음 안에서 연달아 달그락거릴 때의 간격(초).</summary>
        public float BurstGapSeconds => burstGapSeconds;
        /// <summary>한 묶음의 최대 달그락 횟수. 진행도가 높을수록 최대에 가까워진다.</summary>
        public int BurstCountMax => burstCountMax;
        /// <summary>뚜껑이 올라가는 시간(초).</summary>
        public float LiftSeconds => liftSeconds;
        /// <summary>뚜껑이 떨어져 튕김까지 끝나는 시간(초).</summary>
        public float DropSeconds => dropSeconds;
        /// <summary>진행도 1에서 뚜껑이 올라가는 최대 높이(미터).</summary>
        public float LiftHeight => liftHeight;
        /// <summary>진행도 1에서 뚜껑이 한쪽 가장자리를 축으로 기우는 최대 각도(도).</summary>
        public float TiltDegrees => tiltDegrees;
        /// <summary>떨어진 뒤 다시 튀는 높이의 비율(0=튕김 없음).</summary>
        public float Bounce => bounce;
        /// <summary>진행도 0에서의 들썩임 크기 비율(진행도 1은 1).</summary>
        public float StartIntensity => startIntensity;
        /// <summary>들썩임마다 크기를 흔드는 무작위 비율(0~1).</summary>
        public float Randomness => randomness;
        /// <summary>착지 뒤 양쪽 가장자리를 오가며 흔들리는 처음 각도(도). 진행도 1 기준.</summary>
        public float WobbleDegrees => wobbleDegrees;
        /// <summary>착지 흔들림의 진동수(초당 왕복 횟수).</summary>
        public float WobbleFrequency => wobbleFrequency;
        /// <summary>착지 흔들림이 거의 멎을 때까지의 시간(초).</summary>
        public float WobbleSeconds => wobbleSeconds;
        /// <summary>들썩일 때마다 뚜껑이 위 축으로 돌아가는 최대 각도(도, ±).</summary>
        public float SpinDegrees => spinDegrees;
        /// <summary>조리 중 몸통이 끓듯 떨리는 최대 폭(미터). 진행도 1 기준.</summary>
        public float TrembleAmount => trembleAmount;
        /// <summary>끓는 떨림의 빠르기(노이즈 진행 속도).</summary>
        public float TrembleFrequency => trembleFrequency;
        /// <summary>이 진행도 이상이면 쉬지 않고 연달아 덜컹거리는 막바지로 본다.</summary>
        public float ClimaxFrom => climaxFrom;
        /// <summary>뚜껑이 닿을 때 몸통이 세로로 눌리는 비율.</summary>
        public float BodySquash => bodySquash;
        /// <summary>들썩일 때 냄비가 통째로 떠오르는 높이(미터, 냄비 자기 크기 기준). 진행도 1 기준, 완성은 1.5배.</summary>
        public float BodyHopHeight => bodyHopHeight;
        /// <summary>몸통 눌림이 원래대로 돌아오는 시간(초).</summary>
        public float SquashSeconds => squashSeconds;
        /// <summary>완성 때 뚜껑이 튀는 높이(미터).</summary>
        public float FinishLiftHeight => finishLiftHeight;
        /// <summary>완성 때 뚜껑이 기우는 각도(도).</summary>
        public float FinishTiltDegrees => finishTiltDegrees;
        /// <summary>완성 튐 전체 시간(초).</summary>
        public float FinishSeconds => finishSeconds;
        /// <summary>달그락 한 번에 내보내는 김 입자 수.</summary>
        public int SteamPerRattle => steamPerRattle;
        /// <summary>완성 때 내보내는 김 입자 수.</summary>
        public int SteamOnFinish => steamOnFinish;
    }
}
