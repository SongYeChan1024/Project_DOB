// =====================================================================
// MarketRowView — 시장의 양념 한 줄
// 역할   : 양념 그림 · 이름 · 효과 · 맛 · 가격 · 보유 개수와 살 개수(-/+)를 보여 주고,
//          -/+ 누름을 알린다. 살 개수를 정하는 건 MarketView가 한다. 방금 산 양념은 보유 숫자가 톡 (PopOwned)
// 호출자 : MarketView (Bind, SetQuantity, PopOwned)
// 호출   : EffectChipView
// 상태   : 없음
// =====================================================================
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>양념 한 줄.</summary>
    public sealed class MarketRowView : MonoBehaviour
    {
        [SerializeField] private GameObject stripe;      // 짝수 줄 바탕 (읽기 쉽게)
        [SerializeField] private Image icon;
        /// <summary>그림에 마우스를 올리면 이름 · 효과를 띄운다.</summary>
        [SerializeField] private HoverInfo iconHover;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private EffectChipView effectChip;
        [SerializeField] private TMP_Text flavorText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private TMP_Text ownedText;

        [Header("살 개수"), SerializeField] private Button minusButton;
        [SerializeField] private Button plusButton;
        [SerializeField] private TMP_Text quantityText;
        /// <summary>이 줄의 소계 (예: 4금). 0개면 비운다.</summary>
        [SerializeField] private TMP_Text subtotalText;

        /// <summary>-/+ 를 눌렀다. 값은 -1 또는 +1.</summary>
        public event Action<int> QuantityDelta;

        private static readonly Color NumberColor = new Color32(160, 110, 40, 255);
        private static readonly Color FaintColor = new Color32(140, 112, 82, 255);

        private void Awake()
        {
            minusButton.onClick.AddListener(() => QuantityDelta?.Invoke(-1));
            plusButton.onClick.AddListener(() => QuantityDelta?.Invoke(+1));
        }

        /// <summary>양념 정보를 채운다.</summary>
        public void Bind(SpiceItemData spice, bool striped)
        {
            stripe.SetActive(striped);
            icon.sprite = spice.icon;
            icon.enabled = spice.icon != null;
            nameText.text = spice.name;
            effectChip.Set(spice.effect, 1);
            flavorText.text = spice.flavor;
            priceText.text = $"금화 {spice.price}개";
            ownedText.text = spice.owned.ToString();
            if (iconHover) iconHover.Set(spice.name, $"{spice.effect} +1 · {spice.flavor}");
        }

        /// <summary>방금 산 양념: 보유 숫자를 톡 키웠다 돌려놓는다 (시안 16 ②).</summary>
        public void PopOwned()
        {
            if (isActiveAndEnabled) StartCoroutine(Pop());
        }

        private System.Collections.IEnumerator Pop()
        {
            var r = ownedText.rectTransform;
            const float seconds = 0.35f;
            for (float t = 0f; t < seconds; t += Mathf.Min(Time.unscaledDeltaTime, 0.05f))
            {
                r.localScale = Vector3.one * (1f + 0.35f * Mathf.Sin(Mathf.PI * t / seconds));
                yield return null;
            }
            r.localScale = Vector3.one;
        }

        /// <summary>살 개수와 소계를 보여 준다.</summary>
        public void SetQuantity(int quantity, int price, bool canAddMore)
        {
            quantityText.text = quantity.ToString();
            quantityText.color = quantity > 0 ? NumberColor : FaintColor;
            subtotalText.text = quantity > 0 ? $"금화 {quantity * price}개" : string.Empty;
            minusButton.interactable = quantity > 0;
            plusButton.interactable = canAddMore;
        }
    }
}
