// 화구 위 냄비의 "요리 중" 외형 연출. 냄비가 통째로 들썩이며 뚜껑이 불규칙하게 달그락거리고(착지 후 덜컹 흔들림 포함)
// 몸통이 끓듯 떨리며 김을 낸다. 쿼터뷰(약 13m)에서도 읽히도록 통째 들썩임·큰 김으로 덩어리 움직임을 만든다.
// 호출: 조리 연결 담당(Presenter/Manager)이 StartCooking·SetProgress·Finish·Stop을 부른다. 시간은 기본적으로 게임 시간으로 흐르고,
//       조리 시계로 돌리려면 driveWithGameTime을 끄고 Tick(초)을 넘겨준다. 아직 연결 전이라 인스펙터의 시험 반복으로 확인한다.
// 호출하는 것: 뚜껑·몸통 Transform, 김 ParticleSystem.Emit, Rattled/Finished 이벤트(효과음 연결용).
// 가진 상태: 연출 상태·진행도·다음 달그락까지 남은 시간·진행 중인 들썩임·몸통 눌림·떨림. 조리 규칙·시간은 갖지 않는다.
using System;
using DOB.Define;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>조리 상태를 받아 냄비 뚜껑 달그락·몸통 눌림·끓는 떨림·김을 보여준다. 조리 결과를 바꾸지 않는다.</summary>
    public sealed class CookingPotRattle : MonoBehaviour
    {
        private enum Mode { Idle, Cooking, Ready }

        /// <summary>연출 기준값(SO_CookingPotRattle).</summary>
        [SerializeField] private CookingPotRattleSettings settings;
        /// <summary>달그락거릴 뚜껑. 원점은 뚜껑 바닥 가운데.</summary>
        [SerializeField] private Transform lid;
        /// <summary>눌림·떨림을 줄 냄비 몸통. 원점은 바닥 가운데.</summary>
        [SerializeField] private Transform body;
        /// <summary>김 입자. 비어 있으면 김을 내지 않는다.</summary>
        [SerializeField] private ParticleSystem steam;
        /// <summary>0보다 크면 플레이 중 시작→완성을 이 시간(초)마다 반복하는 시험 모드. 실제 연결 후에는 0.</summary>
        [SerializeField, Min(0f)] private float previewLoopSeconds;
        /// <summary>켜면 게임 시간(Time.deltaTime)으로 스스로 진행한다. 끄면 연결 담당이 Tick으로 조리 시계를 넘겨준다.</summary>
        [SerializeField] private bool driveWithGameTime = true;

        /// <summary>뚜껑이 내려와 닿는 순간마다 한 번. 효과음 연결용.</summary>
        public event Action Rattled;
        /// <summary>완성 튐이 시작될 때 한 번. 효과음 연결용.</summary>
        public event Action Finished;

        private Mode mode;
        private float progress;
        private float waitLeft;
        private int burstLeft;
        private bool inBurst;

        // 진행 중인 들썩임 한 번(올라감 → 떨어짐 → 튕김 → 착지 흔들림)
        private bool hopping;
        private bool hopLanded;
        private float hopTime, landTime;
        private float hopLift, hopDrop, hopHeight, hopTilt, hopWobble, hopBody;
        private Vector3 hopDir;

        private float lidYaw;
        private float squashTime = -1f;
        private float tremble, noiseTime;
        private float previewTime;

        private Vector3 lidPos, bodyPos, bodyScale;
        private Quaternion lidRot;
        private float lidRadius;
        private bool ready;

        /// <summary>조리를 시작한다. 진행도를 0으로 두고 곧 첫 달그락을 준비한다.</summary>
        public void StartCooking()
        {
            Init();
            if (!ready) return;
            mode = Mode.Cooking;
            progress = 0f;
            burstLeft = 0;
            waitLeft = NextRest() * 0.5f;
        }

        /// <summary>조리 진행도(0~1). 높을수록 자주·크게 달그락거린다.</summary>
        public void SetProgress(float value) => progress = Mathf.Clamp01(value);

        /// <summary>완성: 뚜껑을 크게 한 번 튀우고 김을 많이 낸다. 이미 완성이면 무시한다.</summary>
        public void Finish()
        {
            Init();
            if (!ready || mode == Mode.Ready) return;
            mode = Mode.Ready;
            progress = 1f;
            BeginHop(RandomDir(), settings.FinishLiftHeight, settings.FinishTiltDegrees,
                settings.FinishSeconds * 0.4f, settings.FinishSeconds * 0.6f, settings.WobbleDegrees * 1.6f, settings.BodyHopHeight * 1.5f);
            Emit(settings.SteamOnFinish);
            Finished?.Invoke();
        }

        /// <summary>연출을 멈춘다. 진행 중인 들썩임은 끝까지 보여준 뒤 쉰다.</summary>
        public void Stop() => mode = Mode.Idle;

        private void Awake() => Init();

        // 기준 자세를 한 번만 기록한다. 에디터에서 Tick으로 미리볼 때는 Awake가 없으므로 여기서 준비한다.
        private void Init()
        {
            if (ready) return;
            ready = settings && lid && body;
            if (!ready) return;
            lidPos = lid.localPosition;
            lidRot = lid.localRotation;
            bodyPos = body.localPosition;
            bodyScale = body.localScale;
            var r = lid.GetComponentInChildren<Renderer>();
            // 기울기 축이 될 가장자리까지의 거리. 렌더러가 없으면 냄비 크기 정도로 둔다.
            lidRadius = r ? Mathf.Max(r.bounds.extents.x, r.bounds.extents.z) / Mathf.Max(lid.lossyScale.x, 0.0001f) : 0.18f;
        }

        private void OnDisable()
        {
            if (!ready) return;
            hopping = false;
            squashTime = -1f;
            tremble = 0f;
            lidYaw = 0f;
            lid.localPosition = lidPos;
            lid.localRotation = lidRot;
            body.localPosition = bodyPos;
            body.localScale = bodyScale;
        }

        private void Update()
        {
            if (driveWithGameTime) Tick(Time.deltaTime);
        }

        /// <summary>연출 시간을 deltaSeconds만큼 진행한다. 조리 시계로 직접 돌리거나 에디터에서 미리볼 때 쓴다.</summary>
        public void Tick(float deltaSeconds)
        {
            Init();
            if (!ready) return;
            float dt = Mathf.Max(0f, deltaSeconds);
            if (previewLoopSeconds > 0f) RunPreview(dt);

            if (mode == Mode.Cooking && !hopping)
            {
                waitLeft -= dt;
                if (waitLeft <= 0f) StartRattle();
            }
            float squash = UpdateSquash(dt);
            Vector3 shake = UpdateTremble(dt);
            UpdateHop(dt, squash, shake);
        }

        // 시험 반복: 앞 85%는 조리(진행도 0→1), 이어서 완성, 나머지는 쉼.
        private void RunPreview(float dt)
        {
            if (previewTime <= 0f) StartCooking();
            previewTime += dt;
            float cook = previewLoopSeconds * 0.85f;
            if (previewTime < cook) SetProgress(previewTime / cook);
            else if (mode == Mode.Cooking) Finish();
            if (previewTime >= previewLoopSeconds + settings.FinishSeconds + 1f) { previewTime = 0f; mode = Mode.Idle; }
        }

        private bool Climax => progress >= settings.ClimaxFrom;

        private void StartRattle()
        {
            if (burstLeft <= 0)
            {
                // 진행도가 높을수록 한 묶음에 여러 번 연달아 달그락거린다. 막바지에는 항상 최대.
                burstLeft = Climax ? settings.BurstCountMax
                    : 1 + Mathf.FloorToInt(UnityEngine.Random.value * (settings.BurstCountMax * (0.35f + 0.65f * progress)));
                burstLeft = Mathf.Clamp(burstLeft, 1, settings.BurstCountMax);
                inBurst = false;
            }
            // 같은 묶음 안에서는 직전의 반대쪽 근처를 들어 "달그락달그락" 번갈아 덜컹거린다.
            Vector3 dir = inBurst ? Quaternion.Euler(0f, 180f + UnityEngine.Random.Range(-30f, 30f), 0f) * hopDir : RandomDir();
            inBurst = true;
            burstLeft--;
            float k = Mathf.Lerp(settings.StartIntensity, 1f, progress) * (1f + (UnityEngine.Random.value * 2f - 1f) * settings.Randomness);
            BeginHop(dir, settings.LiftHeight * k, settings.TiltDegrees * k, settings.LiftSeconds, settings.DropSeconds, settings.WobbleDegrees * k, settings.BodyHopHeight * k);
            lidYaw += UnityEngine.Random.Range(-settings.SpinDegrees, settings.SpinDegrees);
            waitLeft = burstLeft > 0 || Climax ? settings.BurstGapSeconds : NextRest();
        }

        private float NextRest()
        {
            var a = settings.RestSecondsAtStart; var b = settings.RestSecondsAtEnd;
            float min = Mathf.Lerp(a.x, b.x, progress), max = Mathf.Lerp(a.y, b.y, progress);
            return UnityEngine.Random.Range(min, Mathf.Max(min, max));
        }

        private static Vector3 RandomDir()
        {
            float a = UnityEngine.Random.value * Mathf.PI * 2f;
            return new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a));
        }

        // 무작위는 들썩임을 시작할 때만 뽑는다. 매 프레임 무작위로 흔들면 떨림처럼 보이기 때문.
        private void BeginHop(Vector3 dir, float height, float tilt, float lift, float drop, float wobble, float bodyHop)
        {
            hopDir = dir;
            hopHeight = height; hopTilt = tilt; hopLift = lift; hopDrop = drop; hopWobble = wobble; hopBody = bodyHop;
            hopTime = 0f; landTime = 0f; hopping = true; hopLanded = false;
        }

        // squash: 몸통이 눌린 비율(뚜껑은 입구 높이만큼 같이 내려간다). shake: 몸통 떨림 위치(뚜껑도 따라간다).
        // 몸통 통째 들썩임은 뚜껑과 같은 높이 곡선을 쓰고, 뚜껑은 그 위에서 기운다.
        private void UpdateHop(float dt, float squash, Vector3 shake)
        {
            Quaternion yaw = Quaternion.AngleAxis(lidYaw, Vector3.up);
            float tilt = 0f, lift = 0f, bodyLift = 0f;
            if (hopping)
            {
                hopTime += dt;
                float h = HopCurve(hopTime, out bool landed, out bool bounceDone);
                if (landed && !hopLanded)
                {
                    hopLanded = true;
                    squashTime = 0f;
                    EmitFromGap();
                    Rattled?.Invoke();
                }
                if (hopLanded) landTime += dt;
                // 착지 뒤 감쇠 진동: 부호가 바뀌면 반대쪽 가장자리를 축으로 삼아 양쪽을 오가며 덜컹인다.
                float wob = 0f;
                if (hopLanded && landTime < settings.WobbleSeconds)
                {
                    float decay = 3f / settings.WobbleSeconds;
                    wob = hopWobble * Mathf.Exp(-decay * landTime) * Mathf.Sin(2f * Mathf.PI * settings.WobbleFrequency * landTime);
                }
                if (bounceDone && landTime >= settings.WobbleSeconds) { hopping = false; h = 0f; wob = 0f; }
                tilt = hopTilt * h + wob;
                lift = hopHeight * h;
                bodyLift = hopBody * h;
            }
            body.localPosition = bodyPos + shake + Vector3.up * bodyLift;
            Vector3 follow = Vector3.down * (lidPos.y * squash) + shake + Vector3.up * bodyLift;
            // tilt > 0: hopDir 쪽이 들리고 반대쪽 가장자리가 축. tilt < 0: 반대쪽이 들리고 hopDir 쪽 가장자리가 축.
            Vector3 pivot = lidPos - hopDir * (lidRadius * Mathf.Sign(tilt == 0f ? 1f : tilt));
            Vector3 axis = Vector3.Cross(Vector3.up, hopDir);
            Quaternion q = Quaternion.AngleAxis(-tilt, axis);
            lid.localPosition = pivot + q * (lidPos - pivot) + Vector3.up * lift + follow;
            lid.localRotation = q * yaw * lidRot;
        }

        // 0→1 올라감(ease-out), 1→0 떨어짐(ease-in), 마지막 30% 동안 bounce 높이로 한 번 더 튐.
        private float HopCurve(float t, out bool landed, out bool done)
        {
            landed = false; done = false;
            if (t < hopLift) { float u = t / hopLift; return 1f - (1f - u) * (1f - u); }
            float d = t - hopLift;
            float fall = hopDrop * 0.7f;
            if (d < fall) { float u = d / fall; return 1f - u * u; }
            landed = true;
            float b = d - fall, bt = hopDrop * 0.3f;
            if (b < bt) { float u = b / bt; return settings.Bounce * 4f * u * (1f - u); }
            done = true;
            return 0f;
        }

        // 세로는 줄고 가로는 늘려 부피감을 유지한다. 시간이 지나며 원래대로. 현재 눌림 비율을 돌려준다.
        private float UpdateSquash(float dt)
        {
            if (squashTime < 0f) return 0f;
            squashTime += dt;
            float u = squashTime / settings.SquashSeconds;
            if (u >= 1f) { squashTime = -1f; body.localScale = bodyScale; return 0f; }
            float s = settings.BodySquash * (1f - u) * (1f - u);
            body.localScale = Vector3.Scale(bodyScale, new Vector3(1f + s * 0.5f, 1f - s, 1f + s * 0.5f));
            return s;
        }

        // 끓는 떨림: 조리 중에만 부드러운 노이즈로 아주 작게 흔든다. 세기는 목표로 서서히 따라간다.
        private Vector3 UpdateTremble(float dt)
        {
            float target = mode == Mode.Cooking ? settings.TrembleAmount * Mathf.Lerp(0.3f, 1f, progress) : 0f;
            tremble = Mathf.MoveTowards(tremble, target, settings.TrembleAmount * 2f * dt);
            if (tremble <= 0f) return Vector3.zero;
            noiseTime += dt * settings.TrembleFrequency;
            float x = Mathf.PerlinNoise(noiseTime, 0.37f) * 2f - 1f;
            float z = Mathf.PerlinNoise(0.71f, noiseTime) * 2f - 1f;
            return new Vector3(x, 0f, z) * tremble;
        }

        // 뚜껑이 들렸던 쪽 가장자리에서 바깥·위로 김이 새어 나온다.
        private void EmitFromGap()
        {
            if (!steam || settings.SteamPerRattle <= 0) return;
            Vector3 edge = transform.TransformPoint(lidPos + hopDir * lidRadius);
            Vector3 outward = transform.TransformDirection(hopDir);
            var p = new ParticleSystem.EmitParams { applyShapeToPosition = false };
            for (int i = 0; i < settings.SteamPerRattle; i++)
            {
                p.position = edge + UnityEngine.Random.insideUnitSphere * 0.02f;
                p.velocity = outward * UnityEngine.Random.Range(0.12f, 0.22f) + Vector3.up * UnityEngine.Random.Range(0.15f, 0.25f);
                steam.Emit(p, 1);
            }
        }

        private void Emit(int count)
        {
            if (steam && count > 0) steam.Emit(count);
        }

#if UNITY_EDITOR
        // 에디터 확인용: 인스펙터 우클릭 메뉴로 한 순간의 자세를 멈춰 본다. 실행 중 연출과는 무관하다.
        [ContextMenu("시험/달그락 최고점 자세")]
        private void PreviewRattlePeak() => PreviewPeak(settings ? settings.LiftHeight : 0f, settings ? settings.TiltDegrees : 0f, settings ? settings.LiftSeconds : 1f);

        [ContextMenu("시험/완성 튐 최고점 자세")]
        private void PreviewFinishPeak() => PreviewPeak(settings ? settings.FinishLiftHeight : 0f, settings ? settings.FinishTiltDegrees : 0f, settings ? settings.FinishSeconds * 0.4f : 1f);

        [ContextMenu("시험/원래 자세")]
        private void PreviewReset() { if (ready) OnDisable(); }

        private void PreviewPeak(float height, float tilt, float lift)
        {
            Init();
            if (!ready) return;
            BeginHop(RandomDir(), height, tilt, lift, 1f, 0f, 0f);
            hopTime = lift;
            UpdateHop(0f, 0f, Vector3.zero);
            hopping = false;
        }
#endif
    }
}
