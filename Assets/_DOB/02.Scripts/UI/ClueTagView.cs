// =====================================================================
// ClueTagView — 단서 태그 하나  예) [슬라임 점액](독)
// 역할   : 단서 이름과 위험 이름을 태그 모양으로 보여 준다. 테두리는 위험 색.
//          단서 이름이 길면 앞부분 + '…'로 줄이고, 마우스를 올리면 전체 이름을 보여 준다.
//          맨 앞에 위험 아이콘을 둔다 (모르면 (?) 동그라미).
// 호출자 : GuestDialogueView (단서 사전 · 도감에 태그를 만들 때)
// 호출   : HoverInfo (줄였을 때만 전체 이름 상자)
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
        /// <summary>단서 이름을 보여 줄 최대 글자 수(공백 포함). 넘으면 앞부분 + '…'로 줄인다.</summary>
        [SerializeField] private int maxTagChars = 9;
        /// <summary>줄인 태그에 마우스를 올리면 전체 이름을 띄운다. 없으면 줄이기만 한다.</summary>
        [SerializeField] private HoverInfo hover;
        /// <summary>태그 맨 앞 위험 아이콘 (시안 11 ⑧). 비워 두면 아이콘 없이 글자만.</summary>
        [SerializeField] private Image icon;
        /// <summary>위험을 모를 때 아이콘 자리에 보이는 (?) 동그라미.</summary>
        [SerializeField] private GameObject unknownIcon;

        // 의사코드: 단서 이름이 maxTagChars보다 길면 앞부분만 남기고 '…'를 붙인다.
        //           위험 이름은 판단에 필요하므로 줄이지 않는다.
        //           줄였으면 마우스 상자에 전체 이름 · 위험을 넣고, 안 줄였으면 상자를 띄우지 않는다.
        //           맨 앞에는 위험 아이콘, 모르면 (?) 동그라미를 둔다.
        /// <summary>태그 글자 · 색 · 아이콘을 정한다. 위험을 모르면 (?)로 보인다. 긴 단서 이름은 줄여 보인다.</summary>
        public void Set(string tag, string risk, Color color, Sprite riskIcon = null)
        {
            string riskText = string.IsNullOrEmpty(risk) ? "?" : risk;
            string hex = ColorUtility.ToHtmlStringRGB(color);
            string shown = Shorten(tag, maxTagChars);
            label.text = $"[{shown}]<color=#{hex}>({riskText})</color>";
            outline.color = color;
            if (hover) hover.Set(shown != tag ? tag : null, shown != tag ? $"({riskText})" : null);
            if (icon)
            {
                icon.sprite = riskIcon;
                icon.gameObject.SetActive(riskIcon);
            }
            if (unknownIcon) unknownIcon.SetActive(icon && !riskIcon);
        }

        // 글자를 너무 줄이면 수첩에서 안 읽혀서, 크기 대신 길이를 줄인다
        private static string Shorten(string text, int max)
        {
            if (string.IsNullOrEmpty(text) || max < 2 || text.Length <= max) return text;
            return text.Substring(0, max - 1).TrimEnd() + "…";
        }
    }
}
