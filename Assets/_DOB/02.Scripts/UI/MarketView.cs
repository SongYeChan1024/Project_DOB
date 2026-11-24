// =====================================================================
// MarketView — 시장 (행상인에게 양념 사기)
// 역할   : 양념 목록을 보여 주고, 줄마다 살 개수를 고르게 한 뒤 [결제] 한 번으로 구매 요청을 알린다.
//          합계 · 남는 금화는 미리보기일 뿐이고, 실제 차감과 성공 여부는 Presenter(백엔드)가 정한다.
// 호출자 : PrepScreensView (Show), PrepScreenPresenter (이벤트 구독, ShowNotice)
// 호출   : MarketRowView
// 상태   : 고른 개수(장바구니)만 가진다. 화면을 다시 열거나 결제가 끝나면 비운다.
//
// 의사코드
// [보이기]  양념마다 줄을 만들고 살 개수 0으로 시작한다
// [-/+]     0 아래로 내려가지 않는다. 합계가 가진 금화를 넘으면 [+]를 잠근다
// [결제]    고른 게 있을 때만 누를 수 있다. 누르면 (양념 id → 개수) 목록을 알린다
// [닫기]    닫기 요청만 알린다
// =====================================================================
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>시장 화면.</summary>
    public sealed class MarketView : MonoBehaviour
    {
        private const int MaxPerItem = 99;

        [SerializeField] private TMP_Text newsText;
        [SerializeField] private TMP_Text goldText;

        [Header("양념 목록"), SerializeField] private RectTransform rowList;
        /// <summary>줄 원본 (꺼 둔 채로 두고 복사해서 쓴다).</summary>
        [SerializeField] private MarketRowView rowTemplate;

        [Header("결제"), SerializeField] private TMP_Text totalText;
        [SerializeField] private TMP_Text remainText;
        [SerializeField] private Button payButton;
        [SerializeField] private Button closeButton;
        /// <summary>실패 사유 등 짧은 안내 (예: 금화가 모자라요).</summary>
        [SerializeField] private TMP_Text noticeText;

        private readonly List<MarketRowView> rows = new List<MarketRowView>();
        private readonly Dictionary<string, int> cart = new Dictionary<string, int>();
        private PrepViewData data;

        /// <summary>[결제]를 눌렀다. 값은 양념 id → 살 개수.</summary>
        public event Action<IReadOnlyDictionary<string, int>> PurchaseRequested;
        public event Action CloseClicked;

        private void Awake()
        {
            rowTemplate.gameObject.SetActive(false);
            payButton.onClick.AddListener(() => PurchaseRequested?.Invoke(new Dictionary<string, int>(cart)));
            closeButton.onClick.AddListener(() => CloseClicked?.Invoke());
        }

        /// <summary>시장 목록을 새로 그린다. 고른 개수는 비운다.</summary>
        public void Show(PrepViewData viewData)
        {
            data = viewData;
            cart.Clear();
            ShowNotice(null);
            newsText.text = string.IsNullOrEmpty(data.news) ? string.Empty : "오늘의 소식: " + data.news;
            goldText.text = data.gold.ToString();

            foreach (var row in rows)
            {
                row.gameObject.SetActive(false); // 이번 프레임 배치에서도 빠지게
                Destroy(row.gameObject);
            }
            rows.Clear();

            for (int i = 0; i < data.spices.Length; i++)
            {
                var spice = data.spices[i];
                var row = Instantiate(rowTemplate, rowList);
                row.gameObject.SetActive(true);
                row.Bind(spice, i % 2 == 0);
                row.QuantityDelta += delta => ChangeQuantity(spice, delta);
                rows.Add(row);
            }
            RefreshTotals();
        }

        /// <summary>짧은 안내를 띄운다. null이면 숨긴다.</summary>
        public void ShowNotice(string message)
        {
            noticeText.text = message ?? string.Empty;
            noticeText.gameObject.SetActive(!string.IsNullOrEmpty(message));
        }

        private void ChangeQuantity(SpiceItemData spice, int delta)
        {
            cart.TryGetValue(spice.id, out int quantity);
            quantity = Mathf.Clamp(quantity + delta, 0, MaxPerItem);
            if (quantity == 0) cart.Remove(spice.id);
            else cart[spice.id] = quantity;
            RefreshTotals();
        }

        // 합계 · 남는 금화 미리보기와 버튼 잠금을 갱신한다.
        private void RefreshTotals()
        {
            int total = 0;
            foreach (var spice in data.spices)
                if (cart.TryGetValue(spice.id, out int q)) total += q * spice.price;

            for (int i = 0; i < rows.Count; i++)
            {
                var spice = data.spices[i];
                cart.TryGetValue(spice.id, out int q);
                bool canAddMore = q < MaxPerItem && total + spice.price <= data.gold;
                rows[i].SetQuantity(q, spice.price, canAddMore);
            }

            totalText.text = $"금화 {total}개";
            remainText.text = $"결제 후 남는 금화 {data.gold - total}개";
            payButton.interactable = total > 0 && total <= data.gold;
        }
    }
}
