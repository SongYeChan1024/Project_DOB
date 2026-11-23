// =====================================================================
// HoverUnderline — 글자 버튼에 마우스를 올리거나 선택하면 밑줄을 보여 준다
// 역할   : 타이틀 가로 메뉴처럼 판 없는 글자 버튼의 강조 표시. 표시만 하고 입력을 처리하지 않는다.
// 호출자 : EventSystem (마우스 올림·벗어남, 키보드 선택·해제)
// 상태   : 없음 (밑줄 오브젝트를 켜고 끈다)
// =====================================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>버튼 위에 마우스가 있거나 선택됐을 때 밑줄을 켠다.</summary>
    public sealed class HoverUnderline : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private GameObject underline;
        /// <summary>잠긴(누를 수 없는) 버튼이면 밑줄을 켜지 않는다.</summary>
        [SerializeField] private Selectable target;

        private void OnEnable() => underline.SetActive(false);

        public void OnPointerEnter(PointerEventData eventData) => Show(true);
        public void OnPointerExit(PointerEventData eventData) => Show(false);
        public void OnSelect(BaseEventData eventData) => Show(true);
        public void OnDeselect(BaseEventData eventData) => Show(false);

        private void Show(bool on) => underline.SetActive(on && (target == null || target.IsInteractable()));
    }
}
