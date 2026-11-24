// =====================================================================
// ClueTagView — 단서 태그 하나  예) [슬라임 점액](독)
// 역할   : 단서 이름과 위험 이름을 태그 모양으로 보여 준다. 테두리는 위험 색.
// 호출자 : GuestDialogueView (단서 사전 · 도감에 태그를 만들 때)
// 상태   : 없음
// =====================================================================
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>[단서](위험) 태그 하나.</summary>
    public sealed class ClueTagView : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        /// <summary>태그 테두리. 위험 색으로 칠한다.</summary>
        [SerializeField] private Image outline;

        /// <summary>태그 글자와 색을 정한다. 위험을 모르면 (?)로 보인다.</summary>
        public void Set(string tag, string risk, Color color)
        {
            string riskText = string.IsNullOrEmpty(risk) ? "?" : risk;
            string hex = ColorUtility.ToHtmlStringRGB(color);
            label.text = $"[{tag}]<color=#{hex}>({riskText})</color>";
            outline.color = color;
        }
    }
}
