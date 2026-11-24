// =====================================================================
// EffectChipView — 효과 칩 하나  예) [해독 +1]
// 역할   : 효과 이름과 칸 수를 작은 칩으로 보여 준다. 테두리와 글자는 효과 색.
// 호출자 : MarketRowView, RecipeBookView
// 상태   : 없음
// =====================================================================
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>[효과 +N] 칩.</summary>
    public sealed class EffectChipView : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        /// <summary>칩 테두리 (효과 색으로 칠함).</summary>
        [SerializeField] private Image outline;

        public void Set(string effect, int value)
        {
            Color color = PrepPalette.For(effect);
            label.text = $"{effect} +{value}";
            label.color = color;
            outline.color = color;
        }
    }
}
