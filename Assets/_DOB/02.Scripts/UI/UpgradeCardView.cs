// =====================================================================
// UpgradeCardView — 가게 개량 카드 하나
// 역할   : 개량 그림 · 이름 · 효과 · 단계 점 · [개량 N금] 버튼을 보여 주고, 버튼 누름을 알린다.
//          살 수 있는지는 데이터(canBuy)를 그대로 따른다. 다 올렸으면 '완료'.
// 호출자 : UpgradeView (Bind)
// 상태   : 없음
// =====================================================================
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>가게 개량 카드.</summary>
    public sealed class UpgradeCardView : MonoBehaviour
    {
        [SerializeField] private Image picture;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text effectText;
        [SerializeField] private TMP_Text detailText;
        /// <summary>단계 점 (최대 3개). 단계 수만큼만 켜고, 올린 단계는 금색.</summary>
        [SerializeField] private Image[] pips;
        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text buyLabel;
        /// <summary>금화가 모자랄 때 버튼 아래 안내.</summary>
        [SerializeField] private TMP_Text lackText;

        private static readonly Color PipOn = new Color32(240, 196, 110, 255);
        private static readonly Color PipOff = new Color32(215, 195, 160, 255);

        public event Action Clicked;

        private void Awake() => buyButton.onClick.AddListener(() => Clicked?.Invoke());

        public void Bind(UpgradeItemData upgrade)
        {
            picture.sprite = upgrade.picture;
            picture.enabled = upgrade.picture != null;
            nameText.text = upgrade.name;
            effectText.text = upgrade.effect;
            detailText.text = upgrade.detail ?? string.Empty;

            for (int i = 0; i < pips.Length; i++)
            {
                pips[i].gameObject.SetActive(i < upgrade.maxLevel);
                pips[i].color = i < upgrade.level ? PipOn : PipOff;
            }

            bool maxed = upgrade.level >= upgrade.maxLevel;
            buyLabel.text = maxed ? "완료" : $"개량 · 금화 {upgrade.price}개";
            buyButton.interactable = !maxed && upgrade.canBuy;
            lackText.gameObject.SetActive(!maxed && !upgrade.canBuy);
        }
    }
}
