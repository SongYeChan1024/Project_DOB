// =====================================================================
// ServiceHudView — 영업 중 HUD (시안 B)
// 역할   : 왼쪽 위 「N일차 · 영업 중」 · 금화 · 오늘 온 손님 수, 오른쪽 위 줄에 걸린 주문서들,
//          오른쪽 아래 손에 든 접시를 그린다. 왼쪽 아래 Tab · Esc 안내는 고정 글이라 코드가 없다.
//          값은 계산하지 않고 받은 그대로 그린다. 손님 수가 오르면 숫자를 잠깐 키웠다 줄인다.
// 호출자 : ServiceHudPresenter (Show, Hide, SetDay, SetGold, SetGuests, Ticket, HideTicketsFrom, SetHeld)
// 상태   : 복제한 주문서 목록, 마지막 손님 수, 손님 수 강조 남은 시간
//
// 의사코드
// [보이기/숨기기] CanvasGroup 투명도만 바꾼다 (주문서 목록은 유지)
// [일차 · 금화]   글이 바뀔 때만 다시 쓴다
// [손님 수]       바뀔 때만 쓰고, 오르면 강조 시간을 처음부터 → 매 프레임 크기 1.35배에서 1배로 줄인다
// [주문서 i]      견본에서 모자란 만큼 복제한다 → 순서대로 살짝 다른 각도로 기울여 걸린 느낌을 낸다
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
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text guestsText;
        /// <summary>손님 수가 오를 때 키우는 대상 (숫자 묶음).</summary>
        [SerializeField] private RectTransform guestsBump;
        [SerializeField] private float bumpSeconds = 0.35f;
        [SerializeField] private float bumpScale = 1.35f;

        [Header("오른쪽 위 주문서")]
        /// <summary>주문서 견본. 평소 비활성. 복제본은 같은 부모(가로 정렬) 아래에 붙는다.</summary>
        [SerializeField] private ServiceTicketView ticketTemplate;
        /// <summary>주문서마다 돌려 거는 기울기 (도). 순서대로 반복한다.</summary>
        [SerializeField] private float[] ticketTilts = { -4f, 2f, -1.5f, 3f };

        [Header("오른쪽 아래 손에 든 접시")]
        [SerializeField] private GameObject heldRoot;
        [SerializeField] private Image heldDish;

        private readonly List<ServiceTicketView> tickets = new List<ServiceTicketView>();
        private CanvasGroup group;
        private int lastGuests = -1;
        private float bumpLeft;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
            if (ticketTemplate) ticketTemplate.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (bumpLeft <= 0f || !guestsBump) return;
            bumpLeft = Mathf.Max(0f, bumpLeft - Time.unscaledDeltaTime);
            float t = 1f - bumpLeft / bumpSeconds;
            guestsBump.localScale = Vector3.one * Mathf.Lerp(bumpScale, 1f, t * t);
        }

        public void Show() => SetVisible(true);
        public void Hide() => SetVisible(false);

        private void SetVisible(bool visible)
        {
            float alpha = visible ? 1f : 0f;
            if (group && group.alpha != alpha) group.alpha = alpha;
        }

        public void SetDay(int day) => SetText(dayText, string.Format(dayFormat, day));
        public void SetGold(int gold) => SetText(goldText, gold.ToString());

        /// <summary>오늘 온 손님 수. 오르면 숫자를 잠깐 키운다 (처음 한 번은 키우지 않는다).</summary>
        public void SetGuests(int count)
        {
            if (count == lastGuests) return;
            if (lastGuests >= 0 && count > lastGuests) bumpLeft = bumpSeconds;
            lastGuests = count;
            SetText(guestsText, count.ToString());
        }

        /// <summary>i번째 주문서. 모자라면 견본에서 복제한다.</summary>
        public ServiceTicketView Ticket(int index)
        {
            while (tickets.Count <= index)
            {
                var copy = Instantiate(ticketTemplate, ticketTemplate.transform.parent);
                copy.transform.localRotation = Quaternion.Euler(0f, 0f, ticketTilts.Length > 0 ? ticketTilts[tickets.Count % ticketTilts.Length] : 0f);
                tickets.Add(copy);
            }
            return tickets[index];
        }

        /// <summary>start번째부터 뒤의 주문서를 숨긴다.</summary>
        public void HideTicketsFrom(int start)
        {
            for (int i = start; i < tickets.Count; i++)
                tickets[i].Hide();
        }

        /// <summary>손에 든 접시. 없으면(null) 접시 칸을 숨긴다.</summary>
        public void SetHeld(Sprite dishIcon)
        {
            bool has = dishIcon;
            if (heldRoot.activeSelf != has) heldRoot.SetActive(has);
            if (has && heldDish.sprite != dishIcon) heldDish.sprite = dishIcon;
        }

        private static void SetText(TMP_Text label, string text)
        {
            if (label && label.text != text) label.text = text;
        }
    }
}
