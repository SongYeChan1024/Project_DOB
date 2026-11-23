// =====================================================================
// ClueTooltip — 초상 위 돋보기 표식과 생김새 말풍선
// 역할   : 마우스를 올리면 생김새 설명 말풍선을 보이고, 벗어나면 숨긴다.
// 호출자 : GuestDialogueView (설명 문구·위치 설정)
// 상태   : 표시 문구만 가진다.
// =====================================================================
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DOB.UI
{
    /// <summary>관찰 단서 표식. 뜻·수치는 보여 주지 않고 생김새만 보여 준다.</summary>
    public sealed class ClueTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject tooltip;
        [SerializeField] private TMP_Text tooltipText;

        /// <summary>설명과 초상 기준 위치를 정하고 표식을 켠다.</summary>
        public void Show(string description, Vector2 position)
        {
            tooltipText.text = description;
            ((RectTransform)transform).anchoredPosition = position;
            tooltip.SetActive(false);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            tooltip.SetActive(false);
            gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData) => tooltip.SetActive(true);
        public void OnPointerExit(PointerEventData eventData) => tooltip.SetActive(false);
    }
}
