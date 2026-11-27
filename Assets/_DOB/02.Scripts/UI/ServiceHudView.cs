// =====================================================================
// ServiceHudView — 영업 중 HUD (시안 B · 스토리보드 ①~⑤)
// 역할   : 왼쪽 위 「N일차 · 영업 중」 · 금화 · 오늘 온 손님 수, 오른쪽 위 줄에 걸린 주문서들,
//          오른쪽 아래 손에 든 접시를 그린다. 왼쪽 아래 Tab · Esc 안내는 고정 글이라 코드가 없다.
//          값은 계산하지 않고 받은 그대로 그린다.
//          움직임: 금화는 바뀌면 세어 오르며 「+N」(줄면 빨간 「−N」)이 떠오르고, 손님 수는 오르면 잠깐 커진다.
//                  주문서는 식별자로 구분해 새로 걸고 · 떼어 내고, 줄 길이를 넘으면 겹쳐 건다.
// 호출자 : ServiceHudPresenter (Show, Hide, SetDay, SetGold, SetGuests, BeginTickets, ShowTicket, EndTickets, SetHeld)
// 상태   : 걸린 주문서(식별자 → 주문서), 떼어 낸 주문서 묶음, 보이는 금화 값, 강조 남은 시간들
//
// 의사코드
// [보이기/숨기기] CanvasGroup 투명도만 바꾼다 (주문서 목록 · 움직임은 유지)
// [일차]          글이 바뀔 때만 다시 쓴다
// [금화]          처음 한 번은 그대로 쓴다 → 바뀌면 보이는 값을 새 값까지 세어 올리고, 차이를 「+N」/「−N」으로 띄우고, 숫자를 잠깐 키운다
// [손님 수]       바뀔 때만 쓰고, 오르면 숫자를 1.35배에서 1배로 줄인다
// [주문서]        BeginTickets → ShowTicket(식별자, ...)를 먼저 온 순서대로 → EndTickets
//                 처음 보는 식별자: 묶음에서 꺼내(없으면 견본 복제) 건다 / 이번에 안 불린 식별자: 떼어 낸다
//                 부른 순서대로 앞에서부터 놓고, 줄 길이를 넘으면 간격을 줄여(음수) 겹쳐 건다
// [손에 든 접시]  그림이 없으면 접시 칸을 숨긴다
// =====================================================================
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>영업 중 HUD.</summary>
    public sealed class ServiceHudView : MonoBehaviour
    {
        [Header("왼쪽 위")]
        [SerializeField] private TMP_Text dayText;
        [SerializeField] private string dayFormat = "{0}일차";
        [SerializeField] private TMP_Text guestsText;
        /// <summary>손님 수가 오를 때 키우는 대상 (숫자 묶음).</summary>
        [SerializeField] private RectTransform guestsBump;

        [Header("금화")]
        [SerializeField] private TMP_Text goldText;
        /// <summary>금화가 바뀔 때 키우는 대상.</summary>
        [SerializeField] private RectTransform goldBump;
        /// <summary>「+N」 · 「−N」 글. 평소 투명.</summary>
        [SerializeField] private TMP_Text goldDelta;
        [SerializeField] private float countSeconds = 0.5f;
        [SerializeField] private float deltaSeconds = 0.9f;
        [SerializeField] private float deltaRise = 44f;
        [SerializeField] private Color gainColor = new Color32(255, 224, 138, 255);
        [SerializeField] private Color lossColor = new Color32(235, 110, 90, 255);

        [Header("숫자 강조")]
        [SerializeField] private float bumpSeconds = 0.35f;
        [SerializeField] private float bumpScale = 1.35f;

        [Header("오른쪽 위 주문서")]
        /// <summary>주문서 견본. 평소 비활성. 복제본은 같은 부모(가로 정렬) 아래에 붙는다.</summary>
        [SerializeField] private ServiceTicketView ticketTemplate;
        /// <summary>주문서마다 돌려 거는 기울기 (도). 걸린 순서대로 반복한다.</summary>
        [SerializeField] private float[] ticketTilts = { -4f, 2f, -1.5f, 3f };
        /// <summary>여유가 있을 때 주문서 사이 간격. 줄이 모자라면 이보다 줄여(음수) 겹쳐 건다.</summary>
        [SerializeField] private float ticketSpacing = 18f;
        /// <summary>겹쳐 걸 때 가장 많이 겹치는 정도 (음수 간격의 하한).</summary>
        [SerializeField] private float minTicketSpacing = -130f;

        [Header("오른쪽 아래 손에 든 접시")]
        [SerializeField] private GameObject heldRoot;
        [SerializeField] private Image heldDish;

        private readonly Dictionary<string, ServiceTicketView> hung = new Dictionary<string, ServiceTicketView>();
        private readonly Stack<ServiceTicketView> spare = new Stack<ServiceTicketView>();
        private readonly HashSet<string> seen = new HashSet<string>();
        private readonly List<string> stale = new List<string>();
        private CanvasGroup group;
        private HorizontalLayoutGroup row;
        private int hangCount, order;
        private int lastGuests = -1, gold = int.MinValue;
        private float shownGold, countFrom, countT = -1f, deltaT = -1f;
        private float guestsBumpLeft, goldBumpLeft;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
            if (ticketTemplate)
            {
                ticketTemplate.gameObject.SetActive(false);
                row = ticketTemplate.transform.parent.GetComponent<HorizontalLayoutGroup>();
            }
            if (goldDelta) goldDelta.alpha = 0f;
        }

        private void Update()
        {
            float dt = Mathf.Min(Time.unscaledDeltaTime, 0.05f); // 멈칫한 프레임에 짧은 연출이 통째로 건너뛰지 않게
            Bump(guestsBump, ref guestsBumpLeft, dt);
            Bump(goldBump, ref goldBumpLeft, dt);

            if (countT >= 0f)
            {
                countT = Mathf.Min(countT + dt, countSeconds);
                float t = countT / countSeconds;
                shownGold = Mathf.Lerp(countFrom, gold, 1f - (1f - t) * (1f - t));
                SetText(goldText, Mathf.RoundToInt(shownGold).ToString());
                if (countT >= countSeconds) countT = -1f;
            }
            if (deltaT >= 0f && goldDelta)
            {
                deltaT += dt;
                float t = Mathf.Clamp01(deltaT / deltaSeconds);
                goldDelta.rectTransform.anchoredPosition = new Vector2(goldDelta.rectTransform.anchoredPosition.x, deltaRise * (1f - (1f - t) * (1f - t)));
                goldDelta.alpha = t < 0.6f ? 1f : 1f - (t - 0.6f) / 0.4f;
                if (t >= 1f) deltaT = -1f;
            }
        }

        public void Show() => SetVisible(true);
        public void Hide() => SetVisible(false);

        private void SetVisible(bool visible)
        {
            float alpha = visible ? 1f : 0f;
            if (group && group.alpha != alpha) group.alpha = alpha;
        }

        public void SetDay(int day) => SetText(dayText, string.Format(dayFormat, day));

        /// <summary>금화. 바뀌면 세어 오르며 차이를 띄운다 (처음 한 번은 그대로 쓴다).</summary>
        public void SetGold(int value)
        {
            if (value == gold) return;
            if (gold == int.MinValue)
            {
                gold = value; shownGold = value;
                SetText(goldText, value.ToString());
                return;
            }
            int diff = value - gold;
            countFrom = shownGold; countT = 0f;
            gold = value;
            goldBumpLeft = bumpSeconds;
            if (goldDelta)
            {
                goldDelta.text = diff > 0 ? "+" + diff : "−" + (-diff);
                goldDelta.color = diff > 0 ? gainColor : lossColor;
                goldDelta.alpha = 1f;
                deltaT = 0f;
            }
        }

        /// <summary>오늘 온 손님 수. 오르면 숫자를 잠깐 키운다 (처음 한 번은 키우지 않는다).</summary>
        public void SetGuests(int count)
        {
            if (count == lastGuests) return;
            if (lastGuests >= 0 && count > lastGuests) guestsBumpLeft = bumpSeconds;
            lastGuests = count;
            SetText(guestsText, count.ToString());
        }

        /// <summary>이번 프레임 주문서 맞추기를 시작한다.</summary>
        public void BeginTickets()
        {
            seen.Clear();
            order = 0;
        }

        /// <summary>주문서 하나를 그린다. 처음 보는 식별자면 새로 건다. 먼저 온 순서대로 부른다.</summary>
        public void ShowTicket(string orderId, Sprite portrait, string guestName, Sprite dishIcon, OrderTicketState state, bool held)
        {
            if (string.IsNullOrEmpty(orderId) || !ticketTemplate || !seen.Add(orderId)) return;
            if (!hung.TryGetValue(orderId, out var ticket))
            {
                ticket = spare.Count > 0 ? spare.Pop() : Instantiate(ticketTemplate, ticketTemplate.transform.parent);
                ticket.Hang(orderId, ticketTilts.Length > 0 ? ticketTilts[hangCount++ % ticketTilts.Length] : 0f);
                hung[orderId] = ticket;
            }
            ticket.transform.SetSiblingIndex(order++ + 1); // 0번은 견본
            ticket.Show(portrait, guestName, dishIcon, state, held);
        }

        /// <summary>이번에 안 불린 주문서를 떼어 내고, 줄 길이에 맞춰 간격(겹침)을 정한다.</summary>
        public void EndTickets()
        {
            stale.Clear();
            foreach (var pair in hung)
                if (!seen.Contains(pair.Key)) stale.Add(pair.Key);
            foreach (var id in stale)
            {
                hung[id].TakeDown(t => spare.Push(t));
                hung.Remove(id);
            }
            if (row)
            {
                float width = ((RectTransform)row.transform).rect.width;
                float each = ((RectTransform)ticketTemplate.transform).rect.width;
                int n = hung.Count;
                float fit = n > 1 ? (width - n * each) / (n - 1) : ticketSpacing;
                float spacing = Mathf.Clamp(fit, minTicketSpacing, ticketSpacing);
                if (!Mathf.Approximately(row.spacing, spacing)) row.spacing = spacing;
            }
        }

        /// <summary>손에 든 접시. 없으면(null) 접시 칸을 숨긴다.</summary>
        public void SetHeld(Sprite dishIcon)
        {
            bool has = dishIcon;
            if (heldRoot.activeSelf != has) heldRoot.SetActive(has);
            if (has && heldDish.sprite != dishIcon) heldDish.sprite = dishIcon;
        }

        // 남은 시간이 있으면 크기를 bumpScale에서 1로 줄인다
        private void Bump(RectTransform target, ref float left, float dt)
        {
            if (left <= 0f || !target) return;
            left = Mathf.Max(0f, left - dt);
            float t = 1f - left / bumpSeconds;
            target.localScale = Vector3.one * Mathf.Lerp(bumpScale, 1f, t * t);
        }

        private static void SetText(TMP_Text label, string text)
        {
            if (label && label.text != text) label.text = text;
        }
    }
}
