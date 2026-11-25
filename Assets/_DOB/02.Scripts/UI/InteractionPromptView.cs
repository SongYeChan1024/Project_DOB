// =====================================================================
// InteractionPromptView — 상호작용 대상 위에 뜨는 작은 [E] 동작 표시
// 역할   : 받은 화면 위치에 표시를 옮기고 동작 이름을 적는다. 무엇을 띄울지 · 어디에 띄울지는 계산하지 않는다.
// 호출자 : InteractionPromptPresenter (Show, Hide)
// 상태   : 없음
// =====================================================================
using TMPro;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>상호작용 표시 (키 칸 + 동작 이름).</summary>
    public sealed class InteractionPromptView : MonoBehaviour
    {
        /// <summary>표시 전체 (위치를 옮기는 칩). 평소 비활성.</summary>
        [SerializeField] private RectTransform chip;
        /// <summary>키 글자 (E).</summary>
        [SerializeField] private TMP_Text keyText;
        /// <summary>동작 이름.</summary>
        [SerializeField] private TMP_Text actionText;

        private RectTransform area;

        private void Awake()
        {
            area = (RectTransform)transform;
            Hide();
        }

        /// <summary>화면 좌표(픽셀)에 표시를 띄운다.</summary>
        public void Show(Vector2 screenPoint, string key, string action)
        {
            if (!chip) return;
            // 오버레이 캔버스라 카메라 없이 화면 좌표 → 캔버스 좌표
            RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, null, out var local);
            chip.anchoredPosition = local;
            if (keyText) keyText.text = key;
            if (actionText && actionText.text != action) actionText.text = action;
            if (!chip.gameObject.activeSelf) chip.gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (chip && chip.gameObject.activeSelf) chip.gameObject.SetActive(false);
        }
    }
}
