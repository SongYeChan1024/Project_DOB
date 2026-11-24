// =====================================================================
// UpgradeView — 가게 개량 (가게 장부)
// 역할   : 개량 카드들을 보여 주고, [개량]을 누르면 어떤 개량인지 알린다.
// 호출자 : PrepScreensView (Show)
// 호출   : UpgradeCardView
// 상태   : 없음
// =====================================================================
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>가게 개량 화면.</summary>
    public sealed class UpgradeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text goldText;
        /// <summary>카드 자리 (데이터보다 많으면 남는 카드는 숨김).</summary>
        [SerializeField] private UpgradeCardView[] cards;
        [SerializeField] private Button closeButton;

        private PrepViewData data;

        /// <summary>[개량]을 눌렀다. 값은 개량 id.</summary>
        public event Action<string> UpgradeRequested;
        public event Action CloseClicked;

        private void Awake()
        {
            for (int i = 0; i < cards.Length; i++)
            {
                int index = i; // 람다가 반복 변수 대신 고정된 번호를 쓰도록 복사
                cards[i].Clicked += () =>
                {
                    if (data != null && index < data.upgrades.Length)
                        UpgradeRequested?.Invoke(data.upgrades[index].id);
                };
            }
            closeButton.onClick.AddListener(() => CloseClicked?.Invoke());
        }

        public void Show(PrepViewData viewData)
        {
            data = viewData;
            goldText.text = data.gold.ToString();
            for (int i = 0; i < cards.Length; i++)
            {
                bool has = i < data.upgrades.Length;
                cards[i].gameObject.SetActive(has);
                if (has) cards[i].Bind(data.upgrades[i]);
            }
        }
    }
}
