// =====================================================================
// BurnerBadgeView — 화구 하나 위에 뜨는 조리 표시 (시안 09)
// 역할   : 시계 틀 안에 요리 그림, 둘레 링에 남은 비율, 아래 이름표에 남은 초 또는 "완성!"을 그린다.
//          남은 시간 · 비율은 계산하지 않고 받은 값만 그린다.
// 움직임 : 조리 중이던 것이 완성으로 바뀌는 순간 톡 커지며 금빛으로 반짝, 이후 은은한 금빛이 남는다 (시안 17 ④)
// 호출자 : BurnerBadgePresenter (Show, Hide)
// 상태   : 지난번 완성 여부, 반짝 시간
//
// 의사코드
// [그리기]  받은 값을 그린다. 지난번이 조리 중이었고 이번이 완성이면 반짝 시간을 0부터
//           (처음부터 완성으로 뜬 배지는 튀지 않는다)
// [매 프레임] 반짝 중이면 크기 1 → 1.18 → 1, 금빛 = 1 → 머무는 밝기. 완성이 아니면 금빛을 끈다
// [숨기기]  지난 상태를 잊는다
// =====================================================================
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>화구 위 조리 표시 하나.</summary>
    public sealed class BurnerBadgeView : MonoBehaviour
    {
        [SerializeField] private Image dish;
        /// <summary>남은 시간 링 (Filled · Radial360).</summary>
        [SerializeField] private Image ring;
        [SerializeField] private TMP_Text label;
        [SerializeField] private string doneText = "완성!";
        [SerializeField] private Color cookingColor = new Color32(245, 232, 205, 255);
        [SerializeField] private Color doneColor = new Color32(150, 230, 120, 255);

        [Header("완성 반짝")]
        /// <summary>틀 뒤 금빛. 비워 두면 크기만 튄다.</summary>
        [SerializeField] private Graphic doneGlow;
        [SerializeField] private float popSeconds = 0.4f;
        [SerializeField] private float popScale = 1.18f;
        /// <summary>반짝이 끝난 뒤 완성 동안 남는 금빛 밝기 (0~1).</summary>
        [SerializeField] private float holdGlow = 0.45f;

        private RectTransform rect;
        private RectTransform Rect => rect ? rect : rect = (RectTransform)transform;
        private int lastDone = -1; // -1 모름 · 0 조리 중 · 1 완성
        private float popT = -1f;

        /// <summary>캔버스 좌표에 띄운다. done이면 링을 숨기고 "완성!", 아니면 남은 비율(0~1)과 남은 초 글을 보인다.</summary>
        public void Show(Vector2 canvasPoint, Sprite dishIcon, bool done, float remainingRatio, string secondsText)
        {
            Rect.anchoredPosition = canvasPoint;
            if (dish)
            {
                dish.sprite = dishIcon;
                dish.enabled = dishIcon;
            }
            if (ring)
            {
                ring.enabled = !done;
                ring.fillAmount = Mathf.Clamp01(remainingRatio);
            }
            if (label)
            {
                string text = done ? doneText : secondsText;
                if (label.text != text) label.text = text;
                label.color = done ? doneColor : cookingColor;
            }
            if (done && lastDone == 0) popT = 0f;
            lastDone = done ? 1 : 0;
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            Pose();
        }

        public void Hide()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
            lastDone = -1;
            popT = -1f;
        }

        private void Update()
        {
            if (popT < 0f) return;
            popT += Mathf.Min(Time.unscaledDeltaTime, 0.05f); // 멈칫한 프레임에 짧은 연출이 통째로 건너뛰지 않게
            if (popT >= popSeconds) popT = -1f;
            Pose();
        }

        private void Pose()
        {
            float bump = popT >= 0f ? Mathf.Sin(Mathf.PI * popT / popSeconds) : 0f;
            Rect.localScale = Vector3.one * Mathf.Lerp(1f, popScale, bump);
            if (!doneGlow) return;
            bool done = lastDone == 1;
            doneGlow.enabled = done;
            if (done) doneGlow.canvasRenderer.SetAlpha(popT >= 0f ? Mathf.Lerp(1f, holdGlow, popT / popSeconds) : holdGlow);
        }
    }
}
