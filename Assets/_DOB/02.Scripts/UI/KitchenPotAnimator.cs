// =====================================================================
// KitchenPotAnimator — 냄비 뚜껑 닫기 · 열기와 조리 중 들썩임
// 역할   : 조리가 시작되면 뚜껑을 옆 자리에서 냄비 위로 옮겨 덮고, 조리 중에는 냄비와 뚜껑을 살짝씩 흔든다.
//          완성되면 뚜껑을 덮은 채 흔들림만 멈추고, 접시를 들어 화구가 비면 옆에 내려놓는다. 연출만 하고 상태는 판단하지 않는다.
// 호출자 : KitchenView (SetState)
// 상태   : 지금 상태, 뚜껑 이동 진행도
// 비고   : 뚜껑 자리 두 곳(lidClosedSpot · lidRestSpot)은 빈 RectTransform 표시다.
//          유니티에서 표시를 옮기면 뚜껑이 그 자리로 간다 (코드 숫자를 고칠 필요 없음).
//          표시 · 뚜껑은 같은 부모 아래에 둔다 (anchoredPosition을 그대로 옮기기 때문).
//          표시의 크기 · 회전 · 배율(Scale)도 따라간다.
// =====================================================================
using UnityEngine;

namespace DOB.UI
{
    /// <summary>냄비 뚜껑 이동과 조리 중 흔들림.</summary>
    public sealed class KitchenPotAnimator : MonoBehaviour
    {
        [SerializeField] private RectTransform pot;
        [SerializeField] private RectTransform lid;
        [Tooltip("뚜껑을 덮었을 때 자리 (빈 표시)")]
        [SerializeField] private RectTransform lidClosedSpot;
        [Tooltip("뚜껑을 옆에 내려놓았을 때 자리 (빈 표시)")]
        [SerializeField] private RectTransform lidRestSpot;
        [SerializeField] private float lidMoveSeconds = 0.35f;
        [Header("조리 중 흔들림")]
        [SerializeField] private float potWobbleDegrees = 1.2f;
        [SerializeField] private float lidHopPixels = 6f;
        [SerializeField] private float wobbleSpeed = 9f;

        private BurnerState state = BurnerState.Empty;
        private float closed; // 0 = 옆에 놓임, 1 = 덮임

        // 조리 중 · 완성일 때 뚜껑을 덮어 둔다 (빈 화구일 때만 열림)
        private bool Covered => state != BurnerState.Empty;

        public void SetState(BurnerState newState, bool instant)
        {
            state = newState;
            if (instant) closed = Covered ? 1f : 0f;
            Apply(0f);
        }

        private void Update()
        {
            float target = Covered ? 1f : 0f;
            // 일시 정지 중에도 화면 연출은 끝나게 실제 시간 사용
            closed = Mathf.MoveTowards(closed, target, Time.unscaledDeltaTime / Mathf.Max(0.01f, lidMoveSeconds));
            Apply(Time.unscaledTime);
        }

        private void Apply(float time)
        {
            if (!lid || !lidClosedSpot || !lidRestSpot) return;
            float eased = Mathf.SmoothStep(0f, 1f, closed);
            lid.anchoredPosition = Vector2.Lerp(lidRestSpot.anchoredPosition, lidClosedSpot.anchoredPosition, eased);
            float angle = Mathf.LerpAngle(lidRestSpot.localEulerAngles.z, lidClosedSpot.localEulerAngles.z, eased);
            lid.sizeDelta = Vector2.Lerp(lidRestSpot.sizeDelta, lidClosedSpot.sizeDelta, eased);
            lid.localScale = Vector3.Lerp(lidRestSpot.localScale, lidClosedSpot.localScale, eased);

            bool cooking = state == BurnerState.Cooking && closed >= 1f;
            float wobble = cooking ? Mathf.Sin(time * wobbleSpeed) : 0f;
            if (pot) pot.localRotation = Quaternion.Euler(0f, 0f, wobble * potWobbleDegrees);
            // 뚜껑은 김이 새듯 위로만 톡톡 (반파 정류)
            float hop = cooking ? Mathf.Abs(Mathf.Sin(time * wobbleSpeed * 1.5f)) * lidHopPixels : 0f;
            lid.anchoredPosition += new Vector2(0f, hop);
            lid.localRotation = Quaternion.Euler(0f, 0f, angle + wobble * potWobbleDegrees * 1.5f);
        }
    }
}
