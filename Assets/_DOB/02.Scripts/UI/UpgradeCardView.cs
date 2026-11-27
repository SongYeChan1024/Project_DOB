// =====================================================================
// UpgradeCardView — 가게 개량 카드 하나
// 역할   : 개량 그림 · 이름 · 효과 · 단계 점 · [개량 N금] 버튼을 보여 주고, 버튼 누름을 알린다.
//          살 수 있는지는 데이터(canBuy)를 그대로 따른다. 다 올렸으면 '완료'.
//          방금 개량됐으면 뒤집히며 「완료」 도장 + 새 단계 점이 톡 (PlayUpgraded)
// 호출자 : UpgradeView (Bind, PlayUpgraded)
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

        /// <summary>방금 개량됨: 카드가 뒤집히듯 좁아졌다 펴지고 「완료」 도장 + 새로 찬 단계 점이 톡 (시안 21 ②).</summary>
        public void PlayUpgraded(int oldLevel)
        {
            if (isActiveAndEnabled) StartCoroutine(Upgraded(oldLevel));
        }

        // 의사코드: 가로 크기 1 → 0.1 → 1 (뒤집힘) → 이름 글자를 복사해 빨간 「완료」 도장을 크게 찍었다 제자리
        //   + 새로 찬 단계 점이 톡 → 잠깐 머문 뒤 도장이 흐려지며 사라진다
        private System.Collections.IEnumerator Upgraded(int oldLevel)
        {
            var card = transform;
            const float half = 0.12f;
            for (float t = 0f; t < half * 2f; t += Step())
            {
                float k = t < half ? t / half : 1f - (t - half) / half;
                card.localScale = new Vector3(Mathf.Lerp(1f, 0.1f, k * k), 1f, 1f);
                yield return null;
            }
            card.localScale = Vector3.one;

            var stamp = Instantiate(nameText, transform);
            stamp.text = "완료";
            stamp.color = new Color32(179, 64, 46, 255);
            stamp.fontSize = nameText.fontSize * 1.6f;
            stamp.raycastTarget = false;
            var sr = stamp.rectTransform;
            sr.anchorMin = sr.anchorMax = new Vector2(0.5f, 0.55f);
            sr.anchoredPosition = Vector2.zero;
            sr.localRotation = Quaternion.Euler(0f, 0f, -12f);
            var pip = oldLevel >= 0 && oldLevel < pips.Length ? pips[oldLevel].transform : null;
            for (float t = 0f; t < 0.2f; t += Step())
            {
                float k = t / 0.2f;
                sr.localScale = Vector3.one * Mathf.Lerp(1.8f, 1f, k * k);
                if (pip) pip.localScale = Vector3.one * (1f + 0.6f * Mathf.Sin(Mathf.PI * k));
                yield return null;
            }
            sr.localScale = Vector3.one;
            if (pip) pip.localScale = Vector3.one;
            for (float t = 0f; t < 0.5f; t += Step()) yield return null;
            for (float t = 0f; t < 0.3f; t += Step())
            {
                stamp.alpha = 1f - t / 0.3f;
                yield return null;
            }
            Destroy(stamp.gameObject);
        }

        // 일시 정지와 무관하게, 멈칫한 프레임에 연출이 통째로 건너뛰지 않게
        private static float Step() => Mathf.Min(Time.unscaledDeltaTime, 0.05f);
    }
}
