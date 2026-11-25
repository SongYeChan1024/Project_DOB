// 화구 불 연출(StoveFire)이 읽는 연출 기준값. 조리 규칙·시간과는 무관한 외형 값이다.
using UnityEngine;

namespace DOB.Define
{
    /// <summary>냄비 밑 불꽃의 세기·점화/완성 화르륵·잔불·조명 깜빡임 기준값. 실행 중 상태는 갖지 않는다.</summary>
    [CreateAssetMenu(menuName = "DOB/화구 불 연출", fileName = "SO_StoveFire")]
    public sealed class StoveFireSettings : ScriptableObject
    {
        [Header("세기(초당 불꽃 입자 수)")]
        [SerializeField, Min(0f)] private float rateLow;
        [SerializeField, Min(0f)] private float rateHigh;
        [SerializeField, Min(0f)] private float rateEmber;
        [SerializeField, Min(0.01f)] private float blendSeconds;
        [SerializeField, Min(0.01f)] private float fadeOutSeconds;

        [Header("순간 화르륵")]
        [SerializeField, Min(1f)] private float igniteBoost;
        [SerializeField, Min(0.01f)] private float igniteSeconds;
        [SerializeField, Min(1f)] private float flareBoost;
        [SerializeField, Min(0.01f)] private float flareSeconds;

        [Header("조명")]
        [SerializeField, Min(0f)] private float lightIntensity;
        [SerializeField, Range(0f, 1f)] private float flickerAmount;
        [SerializeField, Min(0.1f)] private float flickerSpeed;

        [Header("바닥 빛 번짐")]
        [SerializeField, Range(0f, 1f)] private float glowAlpha;

        /// <summary>진행도 0일 때 초당 불꽃 수.</summary>
        public float RateLow => rateLow;
        /// <summary>진행도 1일 때 초당 불꽃 수.</summary>
        public float RateHigh => rateHigh;
        /// <summary>완성 후 잔불의 초당 불꽃 수.</summary>
        public float RateEmber => rateEmber;
        /// <summary>세기가 목표값으로 따라가는 데 걸리는 시간(초).</summary>
        public float BlendSeconds => blendSeconds;
        /// <summary>끌 때 0까지 줄어드는 시간(초).</summary>
        public float FadeOutSeconds => fadeOutSeconds;
        /// <summary>점화 순간 세기 배율(1=변화 없음).</summary>
        public float IgniteBoost => igniteBoost;
        /// <summary>점화 배율이 1로 돌아오는 시간(초).</summary>
        public float IgniteSeconds => igniteSeconds;
        /// <summary>완성 순간 세기 배율.</summary>
        public float FlareBoost => flareBoost;
        /// <summary>완성 배율이 1로 돌아오는 시간(초).</summary>
        public float FlareSeconds => flareSeconds;
        /// <summary>최대 세기일 때 불빛 조명 밝기.</summary>
        public float LightIntensity => lightIntensity;
        /// <summary>조명 깜빡임 폭(밝기 대비 비율).</summary>
        public float FlickerAmount => flickerAmount;
        /// <summary>조명 깜빡임 빠르기.</summary>
        public float FlickerSpeed => flickerSpeed;
        /// <summary>최대 세기일 때 화구 판 빛 번짐의 불투명도(0~1).</summary>
        public float GlowAlpha => glowAlpha;
    }
}
