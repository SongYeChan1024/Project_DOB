// =====================================================================
// BurnerBadgeView — 화구 하나 위에 뜨는 조리 표시 (시안 09)
// 역할   : 시계 틀 안에 요리 그림, 둘레 링에 남은 비율, 아래 이름표에 남은 초 또는 "완성!"을 그린다.
//          남은 시간 · 비율은 계산하지 않고 받은 값만 그린다.
// 호출자 : BurnerBadgePresenter (Show, Hide)
// 상태   : 없음
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

        private RectTransform rect;
        private RectTransform Rect => rect ? rect : rect = (RectTransform)transform;

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
            if (!gameObject.activeSelf) gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }
    }
}
