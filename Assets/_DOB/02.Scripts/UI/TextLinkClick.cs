// =====================================================================
// TextLinkClick — TMP 글자의 <link> 부분 클릭 감지
// 역할   : 대사에서 숨은 표현을 클릭하면 원고에 넣은 표현 ID를 알린다.
// 호출자 : 같은 오브젝트의 TMP_Text (Raycast Target 켜져 있어야 함)
// 호출   : 없음 (LinkClicked 이벤트로만 알림)
// =====================================================================
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DOB.UI
{
    /// <summary>TMP 링크 클릭을 단서 문구로 바꿔 알린다.</summary>
    [RequireComponent(typeof(TMP_Text))]
    public sealed class TextLinkClick : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text text;

        /// <summary>클릭한 링크의 원문 표현 ID(예: "슬라임 점액").</summary>
        public event Action<string> LinkClicked;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            int index = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, eventData.pressEventCamera);
            if (index < 0)
                return;
            var link = text.textInfo.linkInfo[index];
            // 긴 대사를 쪽으로 나눈 경우, 지금 보이는 쪽에 있는 링크만 받는다.
            if (text.overflowMode == TextOverflowModes.Page &&
                text.textInfo.characterInfo[link.linkTextfirstCharacterIndex].pageNumber != text.pageToDisplay - 1)
                return;
            // 표시 글자가 폰트에서 대체되어도 생성기가 붙인 원문 ID로 단서를 찾는다.
            LinkClicked?.Invoke(link.GetLinkID());
        }
    }
}
