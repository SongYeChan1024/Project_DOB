// =====================================================================
// ClueTooltip — 초상화의 숨은 단서 자리
// 역할   : 초상화 위의 보이지 않는 클릭 영역.
//          · 찾기 전: 아무 표시도 없다. 누르면 "찾았다"를 알린다.
//          · 찾은 뒤: 작은 동그라미가 남고, 마우스를 올리면 생김새 설명이 보인다.
// 호출자 : GuestDialogueView (Show, SetFound, Hide)
// 호출   : 없음 (Clicked 이벤트로만 알림)
// 상태   : 찾았는지 여부만 가진다.
// =====================================================================
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DOB.UI
{
    /// <summary>초상화의 숨은 단서 자리. 뜻 · 수치는 보여 주지 않고 생김새만 보여 준다.</summary>
    public sealed class ClueTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        /// <summary>생김새 설명 말풍선 (찾은 뒤에만 보임).</summary>
        [SerializeField] private GameObject tooltip;
        [SerializeField] private TMP_Text tooltipText;
        /// <summary>찾은 뒤 남는 작은 동그라미 표시.</summary>
        [SerializeField] private GameObject foundMark;

        private bool isFound;

        /// <summary>찾기 전에 눌렸다.</summary>
        public event Action Clicked;

        /// <summary>설명과 위치를 정하고 켠다. 찾은 적이 없는 상태로 시작한다.</summary>
        public void Show(string description, Vector2 position) => Show(description, position, false);

        /// <summary>설명과 위치를 정하고 켠다. found면 찾은 상태로 보여 준다.</summary>
        public void Show(string description, Vector2 position, bool found)
        {
            tooltipText.text = description;
            ((RectTransform)transform).anchoredPosition = position;
            tooltip.SetActive(false);
            SetFound(found);
            gameObject.SetActive(true);
        }

        /// <summary>찾은 상태로 바꾼다 (동그라미 표시).</summary>
        public void SetFound(bool found)
        {
            isFound = found;
            if (foundMark != null)
                foundMark.SetActive(found);
        }

        public void Hide()
        {
            tooltip.SetActive(false);
            gameObject.SetActive(false);
        }

        // 찾기 전에는 마우스를 올려도 티가 나지 않는다.
        public void OnPointerEnter(PointerEventData eventData) => tooltip.SetActive(isFound);
        public void OnPointerExit(PointerEventData eventData) => tooltip.SetActive(false);

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isFound)
                Clicked?.Invoke();
        }
    }
}
