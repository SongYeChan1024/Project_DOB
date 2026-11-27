// =====================================================================
// UpgradeView — 가게 개량 (가게 장부)
// 역할   : 개량 카드들을 보여 주고, [개량]을 누르면 어떤 개량인지 알린다.
//          개량 뒤 다시 그려질 때 단계가 오른 카드는 뒤집히며 「완료」 도장, 지갑은 카운트다운 (시안 21 ②)
// 호출자 : PrepScreensView (Show)
// 호출   : UpgradeCardView, GoldCounter
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
        /// <summary>개량 뒤 금화가 줄어드는 시간 (초).</summary>
        [SerializeField] private float paySeconds = 0.6f;

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
            // 개량 뒤 다시 그려질 때: 단계가 오른 카드는 뒤집히며 「완료」 도장, 금화가 줄었으면 지갑 카운트다운 (시안 21 ②)
            var before = new System.Collections.Generic.Dictionary<string, int>();
            if (data != null) foreach (var u in data.upgrades) before[u.id] = u.level;
            int paidFrom = data != null && viewData != null && viewData.gold < data.gold ? data.gold : -1;

            data = viewData;
            goldText.text = data.gold.ToString();
            for (int i = 0; i < cards.Length; i++)
            {
                bool has = i < data.upgrades.Length;
                cards[i].gameObject.SetActive(has);
                if (!has) continue;
                cards[i].Bind(data.upgrades[i]);
                if (before.TryGetValue(data.upgrades[i].id, out int level) && data.upgrades[i].level > level) cards[i].PlayUpgraded(level);
            }
            if (paidFrom >= 0 && isActiveAndEnabled) StartCoroutine(GoldCounter.PayOut(goldText, paidFrom, data.gold, paySeconds));
        }
    }
}
