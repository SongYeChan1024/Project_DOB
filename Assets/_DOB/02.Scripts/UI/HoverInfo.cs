// =====================================================================
// HoverInfo — 마우스를 올리면 ItemTooltip에 이름 · 효과를 띄우는 표시
// 역할   : 그림(양념 · 재료 칸)에 붙여 둔다. 화면이 Set으로 글을 정해 주면
//          마우스가 들어올 때 보이고, 나가거나 칸이 꺼지면 숨긴다.
// 호출자 : MarketRowView, DoorSignView, PrepNotebookView (Set)
// 호출   : ItemTooltip (같은 캔버스 안에서 찾아 씀)
// 상태   : 띄울 이름 · 설명
// 비고   : 마우스를 받으려면 같은 오브젝트의 Image raycastTarget이 켜져 있어야 한다.
// =====================================================================
using UnityEngine;
using UnityEngine.EventSystems;

namespace DOB.UI
{
    /// <summary>마우스를 올리면 이름 · 효과 상자를 띄운다.</summary>
    public sealed class HoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string title;
        private string body;
        private ItemTooltip tooltip;
        private bool showing;

        /// <summary>띄울 글을 정한다. 이름이 비면 아무것도 안 띄운다.</summary>
        public void Set(string newTitle, string newBody)
        {
            title = newTitle;
            body = newBody;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(title)) return;
            if (!tooltip)
            {
                // 같은 캔버스 맨 위에서 찾는다 (꺼져 있어도 찾기)
                var canvas = GetComponentInParent<Canvas>();
                if (canvas) tooltip = canvas.rootCanvas.GetComponentInChildren<ItemTooltip>(true);
            }
            if (!tooltip) return;
            tooltip.Show(title, body, (RectTransform)transform);
            showing = true;
        }

        public void OnPointerExit(PointerEventData eventData) => HideTooltip();

        // 칸이 사라지거나 화면이 닫히면 상자도 같이 숨긴다
        private void OnDisable() => HideTooltip();

        private void HideTooltip()
        {
            if (showing && tooltip) tooltip.Hide();
            showing = false;
        }
    }
}
