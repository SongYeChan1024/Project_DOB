// 화구 판 위 냄비 밑에서 불이 올라오는 느낌을 빛으로 보여주는 연출. 판 위로 주황 빛이 번지고, 냄비 둘레에서
// 흐린 빛 덩어리가 피어오르며, 주황 조명이 일렁인다. 불꽃 모양 자체는 보이지 않아도 되도록 빛 위주로 만든다.
// 호출: 조리 연결 담당이 Ignite·SetProgress·Flare·Simmer·Extinguish를 부른다(냄비 연출과 같은 신호). 화구마다 하나씩 가진다.
//       시간은 기본적으로 게임 시간으로 흐르고, 조리 시계로 돌리려면 driveWithGameTime을 끄고 Tick(초)을 넘겨준다.
// 호출하는 것: 빛 입자 ParticleSystem의 방출량, 바닥 빛 번짐 Renderer의 색, 불빛 Light의 밝기. 가진 상태: 불 상태·진행도·현재 세기·순간 배율.
using DOB.Define;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>조리 상태를 받아 냄비 밑 불꽃과 불빛을 보여준다. 조리 결과를 바꾸지 않는다.</summary>
    public sealed class StoveFire : MonoBehaviour
    {
        private enum Mode { Off, Burning, Ember }

        /// <summary>연출 기준값(SO_StoveFire).</summary>
        [SerializeField] private StoveFireSettings settings;
        /// <summary>냄비 둘레에서 피어오르는 빛 입자. 방출량을 이 컴포넌트가 정한다.</summary>
        [SerializeField] private ParticleSystem flames;
        /// <summary>불빛 조명. 비어 있으면 조명 없이 입자만 보인다.</summary>
        [SerializeField] private Light fireLight;
        /// <summary>화구 판 위 납작한 빛 고리(DOB/ToonFireRing 재질). 비어 있으면 고리 없이 보인다.</summary>
        [SerializeField] private Renderer glow;
        /// <summary>빛 번짐의 기본 색(알파는 이 컴포넌트가 세기에 맞춰 바꾼다).</summary>
        [SerializeField] private Color glowColor = new Color(1f, 0.5f, 0.15f, 1f);
        /// <summary>0보다 크면 플레이 중 점화→완성→잔불→끄기를 이 시간(초)마다 반복하는 시험 모드. 실제 연결 후에는 0.</summary>
        [SerializeField, Min(0f)] private float previewLoopSeconds;
        /// <summary>켜면 게임 시간으로 스스로 진행한다. 끄면 연결 담당이 Tick으로 조리 시계를 넘겨준다.</summary>
        [SerializeField] private bool driveWithGameTime = true;

        private Mode mode;
        private float progress;
        private float level;
        private float boost = 1f, boostFrom = 1f, boostTime, boostSeconds = 1f;
        private float flickerTime, previewTime;
        private MaterialPropertyBlock glowBlock;
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int IntensityId = Shader.PropertyToID("_Intensity");
        private static readonly int FireTimeId = Shader.PropertyToID("_FireTime");
        private float fireTime;

        /// <summary>불을 켠다. 꺼져 있었으면 "화륵" 크게 피었다가 안정된다.</summary>
        public void Ignite()
        {
            if (!settings) return;
            mode = Mode.Burning;
            progress = 0f;
            Kick(settings.IgniteBoost, settings.IgniteSeconds);
        }

        /// <summary>조리 진행도(0~1). 높을수록 불이 조금씩 세진다.</summary>
        public void SetProgress(float value) => progress = Mathf.Clamp01(value);

        /// <summary>완성 순간 한 번 화르륵 커진 뒤 잔불로 줄어든다.</summary>
        public void Flare()
        {
            if (!settings) return;
            Kick(settings.FlareBoost, settings.FlareSeconds);
            mode = Mode.Ember;
        }

        /// <summary>약한 잔불로 둔다.</summary>
        public void Simmer() => mode = Mode.Ember;

        /// <summary>천천히 끈다.</summary>
        public void Extinguish() => mode = Mode.Off;

        private void Kick(float to, float seconds)
        {
            boostFrom = Mathf.Max(boost, to);
            boostTime = 0f;
            boostSeconds = seconds;
        }

        private void Update()
        {
            if (driveWithGameTime) Tick(Time.deltaTime);
        }

        /// <summary>연출 시간을 deltaSeconds만큼 진행한다. 조리 시계로 직접 돌리거나 에디터에서 미리볼 때 쓴다.</summary>
        public void Tick(float deltaSeconds)
        {
            if (!settings || !flames) return;
            float dt = Mathf.Max(0f, deltaSeconds);
            if (previewLoopSeconds > 0f) RunPreview(dt);

            float target = mode switch
            {
                Mode.Burning => Mathf.Lerp(settings.RateLow, settings.RateHigh, progress),
                Mode.Ember => settings.RateEmber,
                _ => 0f,
            };
            float seconds = mode == Mode.Off ? settings.FadeOutSeconds : settings.BlendSeconds;
            level = Mathf.MoveTowards(level, target, Mathf.Max(settings.RateHigh, 1f) / seconds * dt);

            // 순간 배율은 시작값에서 1로 ease-out으로 돌아온다.
            boostTime += dt;
            float u = Mathf.Clamp01(boostTime / boostSeconds);
            boost = Mathf.Lerp(boostFrom, 1f, 1f - (1f - u) * (1f - u));

            var em = flames.emission;
            em.rateOverTime = level * boost;
            if (!flames.isPlaying && level > 0f) flames.Play();

            flickerTime += dt * settings.FlickerSpeed;
            fireTime += dt;
            float flicker = 1f + (Mathf.PerlinNoise(flickerTime, 0.5f) * 2f - 1f) * settings.FlickerAmount;
            float ratio = settings.RateHigh > 0f ? level / settings.RateHigh : 0f;
            if (fireLight)
            {
                fireLight.intensity = settings.LightIntensity * ratio * boost * flicker;
                fireLight.enabled = fireLight.intensity > 0.001f;
            }
            if (glow)
            {
                // 재질을 복제하지 않도록 프로퍼티 블록으로 색만 바꾼다. 조명과 다른 위상으로 일렁여 단조롭지 않게 한다.
                float glowFlicker = 1f + (Mathf.PerlinNoise(0.3f, flickerTime * 0.7f) * 2f - 1f) * settings.FlickerAmount;
                float a = Mathf.Clamp01(settings.GlowAlpha * ratio * boost * glowFlicker);
                glowBlock ??= new MaterialPropertyBlock();
                glow.GetPropertyBlock(glowBlock);
                // 빛 고리 셰이더(DOB/ToonFireRing)는 세기·불 시간을, 일반 입자 재질은 색 알파를 쓴다. 둘 다 넣어 어느 재질이든 동작하게 한다.
                glowBlock.SetColor(BaseColorId, new Color(glowColor.r, glowColor.g, glowColor.b, a));
                glowBlock.SetFloat(IntensityId, a);
                glowBlock.SetFloat(FireTimeId, fireTime);
                glow.SetPropertyBlock(glowBlock);
                glow.enabled = a > 0.001f;
            }
        }

        // 시험 반복: 앞 85%는 조리(진행도 0→1), 완성 화르륵, 1초 잔불, 이어서 끄고 다시.
        private void RunPreview(float dt)
        {
            if (previewTime <= 0f) Ignite();
            previewTime += dt;
            float cook = previewLoopSeconds * 0.85f;
            if (previewTime < cook) SetProgress(previewTime / cook);
            else if (mode == Mode.Burning) Flare();
            if (previewTime >= previewLoopSeconds + 0.5f && mode == Mode.Ember) Extinguish();
            if (previewTime >= previewLoopSeconds + 1.5f) previewTime = 0f;
        }
    }
}
