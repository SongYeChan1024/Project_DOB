// =====================================================================
// KitchenView — 화구 하나의 요리 화면 (재료 상자 · 냄비 · 결과 카드 · 주문서 · 시계)
// 역할   : 표시와 입력만 한다. 재료를 누르면 "넣어 달라", 냄비 접시를 누르면 "빼 달라"를 알리고,
//          Presenter가 준 데이터를 그대로 그린다. 레시피 판정 · 재고 · 조리 완료는 계산하지 않는다.
// 호출자 : KitchenPresenter (Open, Show, SetTimer, Close)
// 호출   : KitchenPotAnimator (뚜껑 · 흔들림), HoverInfo (마우스 설명)
// 상태   : 복사해 만든 칸 목록, 열림 여부
//
// 의사코드
// [열기]    화면을 켜고 서서히 나타남 → Show로 전체 그림
// [Show]    주문서 · 재료 상자 · 양념 상자 · 냄비 접시 · 결과 카드 · 단추를 새 데이터로 다시 그림
//           조리 중 · 완성이면 재료 상자를 흐리게 하고 못 누르게 한다
// [넣기]    (시안 16 ①) 누른 재료가 실제로 냄비에 들어가면(다음 Show에서 냄비 칸 수가 늘면) 그 그림이 곡선으로 냄비에 톡 떨어지고
//           냄비가 출렁 + 김 한 번 + 그 냄비 칸이 톡. 거절돼 칸 수가 그대로면 아무 연출도 없다
// [시계]    SetTimer로 남은 시간만 자주 갱신 (전체를 다시 그리지 않음)
// [닫기]    입력을 바로 막고 서서히 사라짐 (닫히는 중 두 번 눌림 방지)
// [자리]    모든 부품은 프리팹 안에서 손으로 옮길 수 있다. 코드는 위치를 바꾸지 않는다 (뚜껑만 표시 자리 사이를 움직임)
// =====================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>화구 하나의 요리 화면.</summary>
    public sealed class KitchenView : MonoBehaviour
    {
        // 효과 → 막는 위험 (카드에서 손님 위험과 나란히 비교)
        private static readonly string[] Effects = { "해독", "내열", "방한", "야광", "활력", "감각" };
        private static readonly string[] Risks = { "독", "열", "냉기", "어둠", "체력", "함정" };

        [SerializeField] private CanvasGroup content;
        [SerializeField] private float fadeSeconds = 0.15f;
        [SerializeField] private TMP_Text burnerTitle;

        [Header("주문서 (위 줄)")]
        [SerializeField] private RectTransform orderRail;
        [Tooltip("자식: Name, Status, Selected(모서리 빛)")]
        [SerializeField] private GameObject ticketTemplate;

        [Header("시계")]
        [SerializeField] private Image timerFill;
        [SerializeField] private TMP_Text timerText;

        [Header("재료 · 양념 상자 (넘치면 위아래로 넘겨 봄)")]
        [SerializeField] private CanvasGroup pantry;
        [SerializeField] private RectTransform ingredientGrid;
        [SerializeField] private RectTransform spiceGrid;
        [SerializeField] private ScrollRect ingredientScroll;
        [SerializeField] private ScrollRect spiceScroll;
        [Tooltip("자식: Icon, Coin/Count. 루트에 Button · HoverInfo")]
        [SerializeField] private GameObject tileTemplate;
        [SerializeField, Range(0f, 1f)] private float pantryDimAlpha = 0.45f;

        [Header("냄비")]
        [SerializeField] private KitchenPotAnimator pot;
        [Tooltip("열린 냄비 안에 보이는 재료 그림 줄")]
        [SerializeField] private RectTransform potInside;
        [SerializeField] private Image potInsideTemplate;
        [Tooltip("냄비 안에 보이는 양념 그림 (재료 위에 얹힘). 비워 두면 안 보임")]
        [SerializeField] private Image potSpiceIcon;
        [Tooltip("냄비 앞 나무 접시 (재료 칸). 자식: Icon")]
        [SerializeField] private Button[] potSlots;
        [Tooltip("양념 접시. 자식: Icon")]
        [SerializeField] private Button spiceSlot;

        [Header("결과 카드")]
        [SerializeField] private TMP_Text cardTitle;
        [SerializeField] private Image cardPicture;
        [SerializeField] private TMP_Text needText;
        [SerializeField] private RectTransform compareList;
        [Tooltip("자식: Icon, Name, Risk, Effect, Verdict")]
        [SerializeField] private GameObject compareRowTemplate;
        [SerializeField] private Image[] stars;
        [SerializeField] private Color starOn = new Color32(240, 180, 60, 255);
        [SerializeField] private Color starOff = new Color32(205, 190, 165, 255);
        [SerializeField] private TMP_Text flavorText;
        [SerializeField] private TMP_Text priceText;
        [Tooltip("독 · 열 · 냉기 · 어둠 · 체력 · 함정 순서")]
        [SerializeField] private Sprite[] riskIcons;
        [SerializeField] private Color okColor = new Color32(90, 130, 50, 255);
        [SerializeField] private Color shortColor = new Color32(170, 70, 50, 255);

        [Header("단추")]
        [SerializeField] private Button cookButton;
        [SerializeField] private TMP_Text cookLabel;
        [SerializeField] private Image cookIcon;
        [SerializeField] private Button closeButton;

        private readonly List<GameObject> spawned = new List<GameObject>();
        private Coroutine fade;
        private Image selectedGlow; // 고른 주문서의 모서리 빛 (천천히 밝아졌다 어두워짐)
        // 냄비 안 재료를 조금씩 기울여 막 넣은 느낌을 준다
        private static readonly float[] InsideTilt = { -8f, 6f, -4f, 9f };

        public bool IsOpen { get; private set; }

        [Header("재료 넣기 손맛 (시안 16 ①)")]
        /// <summary>냄비 위로 피어오르는 김 그림. 비워 두면 김 없이 출렁이기만.</summary>
        [SerializeField] private Sprite steamSprite;
        [SerializeField] private float dropSeconds = 0.3f;
        [SerializeField] private float wobbleSeconds = 0.45f;
        /// <summary>출렁일 때 처음 기울기 (도).</summary>
        [SerializeField] private float wobbleDegrees = 3f;

        private bool dropPending, hadSpice;
        private Sprite dropSprite;
        private Vector3 dropFrom;
        private int lastPotCount;

        // ---------------- 입력 알림 ----------------
        public event Action<string> OrderClicked;
        public event Action<string> IngredientClicked;
        public event Action<string> SpiceClicked;
        public event Action<int> PotSlotClicked;
        public event Action SpiceSlotClicked;
        /// <summary>조리 시작 (비어 있을 때) 또는 접시 들기 (완성일 때).</summary>
        public event Action CookClicked;
        public event Action CloseClicked;

        private void Awake()
        {
            foreach (var t in new[] { ticketTemplate, tileTemplate, compareRowTemplate, potInsideTemplate.gameObject })
                t.SetActive(false);
            for (int i = 0; i < potSlots.Length; i++)
            {
                int index = i; // 람다가 반복 변수 대신 고정된 번호를 쓰도록 복사
                potSlots[i].onClick.AddListener(() => PotSlotClicked?.Invoke(index));
            }
            spiceSlot.onClick.AddListener(() => SpiceSlotClicked?.Invoke());
            cookButton.onClick.AddListener(() => CookClicked?.Invoke());
            closeButton.onClick.AddListener(() => CloseClicked?.Invoke());
            content.alpha = 0f;
            content.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!selectedGlow) return;
            var c = selectedGlow.color;
            c.a = 0.65f + 0.35f * Mathf.Sin(Time.unscaledTime * 3f);
            selectedGlow.color = c;
        }

        // =================================================================
        // 열기 · 닫기
        // =================================================================

        public void Open(KitchenViewData data)
        {
            bool wasOpen = IsOpen;
            IsOpen = true;
            content.gameObject.SetActive(true);
            SetInput(true);
            Show(data, !wasOpen);
            if (!wasOpen) StartFade(1f, false);
        }

        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            SetInput(false); // 닫히는 페이드 동안 단추가 또 눌리지 않게
            StartFade(0f, true);
        }

        // =================================================================
        // 그리기
        // =================================================================

        /// <summary>전체를 새 데이터로 다시 그린다. instant면 뚜껑을 애니메이션 없이 제자리에 둔다.</summary>
        public void Show(KitchenViewData data, bool instant = false)
        {
            foreach (var go in spawned) Destroy(go);
            spawned.Clear();

            burnerTitle.text = data.burnerName;
            bool editing = data.state == BurnerState.Empty;

            DrawOrders(data);
            DrawPantry(ingredientGrid, data.ingredients, IngredientClicked);
            DrawPantry(spiceGrid, data.spices, SpiceClicked);
            pantry.alpha = editing ? 1f : pantryDimAlpha;
            pantry.interactable = editing;
            pantry.blocksRaycasts = editing;

            DrawPot(data, editing);
            DrawCard(data.preview, data.risks);

            // 단추: 비어 있음 → 조리 시작 / 조리 중 → 막힘 / 완성 → 접시 들기
            cookButton.interactable = data.state == BurnerState.Done || (editing && data.preview.canCook);
            cookLabel.text = data.state == BurnerState.Cooking ? "조리 중…"
                           : data.state == BurnerState.Done ? "접시 들기"
                           : "조리 시작";
            cookIcon.enabled = data.state != BurnerState.Cooking;

            pot.SetState(data.state, instant);
            SetTimer(data.state == BurnerState.Empty ? data.cookSeconds : data.remainingSeconds, data.cookSeconds, data.state);

            // 방금 누른 재료 · 양념이 실제로 냄비에 들어갔으면 날아 들어가는 연출 (시안 16 ①)
            int potCount = data.potIngredients.Count(p => p != null) + (data.potSpice != null ? 1 : 0);
            bool spiceAdded = data.potSpice != null && !hadSpice;
            if (dropPending && editing && potCount > lastPotCount && isActiveAndEnabled)
                StartCoroutine(DropIn(dropSprite, dropFrom, spiceAdded ? spiceSlot : LastFilledSlot(data)));
            dropPending = false;
            lastPotCount = potCount;
            hadSpice = data.potSpice != null;
        }

        // =================================================================
        // 재료 넣기 손맛 (시안 16 ①)
        // =================================================================

        // 누른 칸의 그림 · 자리를 기억한다. 넣기가 받아들여졌는지는 다음 Show에서 냄비 칸 수로 판단한다
        private void MarkDrop(Image icon)
        {
            dropPending = true;
            dropSprite = icon.sprite;
            dropFrom = icon.rectTransform.position;
        }

        private Button LastFilledSlot(KitchenViewData data)
        {
            for (int i = Mathf.Min(potSlots.Length, data.potIngredients.Length) - 1; i >= 0; i--)
                if (data.potIngredients[i] != null) return potSlots[i];
            return null;
        }

        // 의사코드: 맨 위 캔버스에 그림 복사본을 띄워 누른 칸에서 냄비 안까지 곡선으로 날림
        //   → 도착하면 냄비 묶음(PotArea)이 좌우로 출렁, 김이 한 번 피어오름, 그 재료가 들어간 냄비 칸이 톡
        //   도중에 화면이 다시 그려져 칸이 사라져도 오류 없이 끝낸다
        private System.Collections.IEnumerator DropIn(Sprite sprite, Vector3 from, Button slot)
        {
            var canvas = GetComponentInParent<Canvas>().rootCanvas;
            var ghost = new GameObject("DropGhost", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            ghost.transform.SetParent(canvas.transform, false);
            var gi = ghost.GetComponent<Image>();
            gi.sprite = sprite; gi.preserveAspect = true; gi.raycastTarget = false;
            var gr = (RectTransform)ghost.transform;
            gr.sizeDelta = new Vector2(96f, 96f);
            gr.SetAsLastSibling();
            Vector3 to = potInside.position;
            Vector3 mid = (from + to) * 0.5f + canvas.transform.up * (Vector3.Distance(from, to) * 0.35f);
            for (float t = 0f; t < dropSeconds; t += Step())
            {
                float k = t / dropSeconds;
                float e = k * k; // 떨어지듯 점점 빠르게
                gr.position = Vector3.Lerp(Vector3.Lerp(from, mid, e), Vector3.Lerp(mid, to, e), e);
                gr.localScale = Vector3.one * Mathf.Lerp(1f, 0.6f, e);
                yield return null;
            }
            Destroy(ghost);

            // 김: 냄비 위에서 피어오르며 흐려진다
            Image steam = null;
            if (steamSprite)
            {
                var go = new GameObject("DropSteam", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
                go.transform.SetParent(potInside.parent, false);
                steam = go.GetComponent<Image>();
                steam.sprite = steamSprite; steam.raycastTarget = false;
                ((RectTransform)go.transform).sizeDelta = new Vector2(220f, 160f);
                go.transform.position = potInside.position;
            }
            var area = pot.transform;
            var st = slot ? slot.transform : null;
            Vector3 steamFrom = steam ? steam.transform.localPosition : Vector3.zero;
            for (float t = 0f; t < wobbleSeconds; t += Step())
            {
                float k = t / wobbleSeconds;
                if (area) area.localRotation = Quaternion.Euler(0f, 0f, wobbleDegrees * (1f - k) * Mathf.Sin(t * 40f));
                if (st) st.localScale = Vector3.one * (1f + 0.2f * Mathf.Sin(Mathf.PI * Mathf.Min(1f, k * 1.4f)));
                if (steam)
                {
                    steam.transform.localPosition = steamFrom + new Vector3(0f, 90f * k, 0f);
                    steam.transform.localScale = Vector3.one * Mathf.Lerp(0.8f, 1.4f, k);
                    steam.color = new Color(1f, 1f, 1f, 0.8f * (1f - k));
                }
                yield return null;
            }
            if (area) area.localRotation = Quaternion.identity;
            if (st) st.localScale = Vector3.one;
            if (steam) Destroy(steam.gameObject);
        }

        // 일시 정지와 무관하게, 멈칫한 프레임에 연출이 통째로 건너뛰지 않게
        private static float Step() => Mathf.Min(Time.unscaledDeltaTime, 0.05f);

        /// <summary>시계만 갱신한다. 남은 시간이 줄어드는 띠와 초 글자.</summary>
        public void SetTimer(float remaining, float total, BurnerState state)
        {
            float ratio = total > 0f ? Mathf.Clamp01(remaining / total) : 0f;
            timerFill.fillAmount = state == BurnerState.Done ? 0f : ratio;
            timerText.text = state == BurnerState.Done ? "완성" : Mathf.CeilToInt(remaining) + "초";
        }

        private void DrawOrders(KitchenViewData data)
        {
            selectedGlow = null;
            foreach (var order in data.orders)
            {
                var ticket = Spawn(ticketTemplate, orderRail);
                ticket.transform.Find("Name").GetComponent<TMP_Text>().text = order.guestName;
                ticket.transform.Find("Status").GetComponent<TMP_Text>().text = $"{order.seatLabel} · {StatusText(order.status)}";
                bool selected = order.id == data.selectedOrderId;
                var glow = ticket.transform.Find("Selected").GetComponent<Image>();
                glow.gameObject.SetActive(selected);
                if (selected) selectedGlow = glow;
                var button = ticket.GetComponent<Button>();
                string id = order.id;
                button.onClick.AddListener(() => OrderClicked?.Invoke(id));
            }
        }

        private static string StatusText(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Waiting: return "대기";
                case OrderStatus.Cooking: return "조리 중";
                case OrderStatus.Done: return "완성";
                case OrderStatus.Carrying: return "들고 있음";
                default: return "다 먹음";
            }
        }

        private void DrawPantry(RectTransform grid, PantryItemData[] items, Action<string> clicked)
        {
            foreach (var item in items)
            {
                var tile = Spawn(tileTemplate, grid);
                var icon = tile.transform.Find("Icon").GetComponent<Image>();
                icon.sprite = item.icon;
                icon.enabled = item.icon;
                tile.transform.Find("Coin/Count").GetComponent<TMP_Text>().text = item.infinite ? "∞" : item.count.ToString();
                var button = tile.GetComponent<Button>();
                button.interactable = item.infinite || item.count > 0;
                string id = item.id;
                button.onClick.AddListener(() => { MarkDrop(icon); clicked?.Invoke(id); });
                var hover = tile.GetComponent<HoverInfo>();
                if (hover) hover.Set(item.name, item.description);
            }
            // 칸이 바뀌면 맨 위부터 보이게
            var scroll = grid == ingredientGrid ? ingredientScroll : spiceScroll;
            if (scroll) scroll.verticalNormalizedPosition = 1f;
        }

        private void DrawPot(KitchenViewData data, bool editing)
        {
            for (int i = 0; i < potSlots.Length; i++)
            {
                var item = i < data.potIngredients.Length ? data.potIngredients[i] : null;
                SetSlot(potSlots[i], item, editing);
            }
            SetSlot(spiceSlot, data.potSpice, editing);

            // 열린 냄비 안에도 넣은 재료 · 양념을 보여 준다 (뚜껑이 닫히면 숨김)
            potInside.gameObject.SetActive(editing);
            if (potSpiceIcon)
            {
                bool showSpice = editing && data.potSpice != null && data.potSpice.icon;
                potSpiceIcon.gameObject.SetActive(showSpice);
                if (showSpice) potSpiceIcon.sprite = data.potSpice.icon;
            }
            if (!editing) return;
            int n = 0;
            foreach (var item in data.potIngredients)
            {
                if (item == null || !item.icon) continue;
                var img = Instantiate(potInsideTemplate, potInside);
                img.gameObject.SetActive(true);
                img.sprite = item.icon;
                img.rectTransform.localRotation = Quaternion.Euler(0f, 0f, InsideTilt[n++ % InsideTilt.Length]);
                spawned.Add(img.gameObject);
            }
        }

        private static void SetSlot(Button slot, PantryItemData item, bool editing)
        {
            var icon = slot.transform.Find("Icon").GetComponent<Image>();
            icon.sprite = item != null ? item.icon : null;
            icon.enabled = item != null && item.icon;
            slot.interactable = editing && item != null;
            var hover = slot.GetComponent<HoverInfo>();
            if (hover) hover.Set(item?.name, item?.description);
        }

        // 결과 카드: 요리 그림 · 재료 진행 · 손님 위험 ↔ 요리 효과 비교 · 맛 별 · 예상 금화
        private void DrawCard(RecipePreviewData preview, GuestRiskData[] risks)
        {
            cardTitle.text = preview.title;
            cardPicture.sprite = preview.picture;
            cardPicture.enabled = preview.found && preview.picture;
            needText.text = preview.needLine;

            // 비교할 축: 손님에게서 찾은 위험 + 요리 효과가 있는 축 (겹치면 한 줄)
            var rows = new List<int>();
            foreach (var r in risks) AddAxis(rows, Array.IndexOf(Risks, r.risk));
            foreach (var e in preview.effects) AddAxis(rows, Array.IndexOf(Effects, e.name));
            foreach (int axis in rows)
            {
                int need = 0, have = 0;
                foreach (var r in risks) if (r.risk == Risks[axis]) need += r.amount;
                foreach (var e in preview.effects) if (e.name == Effects[axis]) have += e.value;
                DrawCompareRow(axis, need, have);
            }

            for (int i = 0; i < stars.Length; i++)
                stars[i].color = i < preview.taste ? starOn : starOff;
            flavorText.text = preview.flavor;
            priceText.text = preview.priceText;
        }

        private static void AddAxis(List<int> rows, int axis)
        {
            if (axis >= 0 && !rows.Contains(axis)) rows.Add(axis);
        }

        private void DrawCompareRow(int axis, int need, int have)
        {
            var row = Spawn(compareRowTemplate, compareList);
            var color = PrepPalette.For(Risks[axis]);
            row.GetComponent<Image>().color = color; // 테두리만 있는 줄이라 위험 색 그대로
            var icon = row.transform.Find("Icon").GetComponent<Image>();
            icon.sprite = riskIcons != null && axis < riskIcons.Length ? riskIcons[axis] : null;
            icon.enabled = icon.sprite;
            var name = row.transform.Find("Name").GetComponent<TMP_Text>();
            name.text = Risks[axis];
            name.color = color;
            // 이 축의 손님 위험을 아직 모르면 칸 대신 - 표시
            row.transform.Find("Risk").GetComponent<TMP_Text>().text = need == 0 ? "-" : Pips(need);
            var effect = row.transform.Find("Effect").GetComponent<TMP_Text>();
            effect.text = Pips(have);
            effect.color = color;
            var verdict = row.transform.Find("Verdict").GetComponent<TMP_Text>();
            // 손님 위험을 모르면(0) 판정 글자를 비워 둔다
            verdict.text = need == 0 ? string.Empty : have >= need ? "충분" : $"{need - have}칸 부족";
            verdict.color = have >= need ? okColor : shortColor;
        }

        // 칸 그림: 채운 칸 ■ + 빈 칸 □ (최소 3칸)
        private static string Pips(int filled)
        {
            int total = Mathf.Max(3, filled);
            return new string('■', filled) + new string('□', total - filled);
        }

        // =================================================================
        // 도우미
        // =================================================================

        private GameObject Spawn(GameObject template, Transform parent)
        {
            var go = Instantiate(template, parent);
            go.SetActive(true);
            spawned.Add(go);
            return go;
        }

        private void SetInput(bool on)
        {
            content.interactable = on;
            content.blocksRaycasts = on;
        }

        private void StartFade(float to, bool hideAtEnd)
        {
            if (fade != null) StopCoroutine(fade);
            fade = isActiveAndEnabled ? StartCoroutine(Fade(to, hideAtEnd)) : null;
            if (fade == null)
            {
                content.alpha = to;
                if (hideAtEnd) content.gameObject.SetActive(false);
            }
        }

        private IEnumerator Fade(float to, bool hideAtEnd)
        {
            float from = content.alpha;
            // 일시 정지(timeScale 0)와 무관하게 끝나도록 실제 시간을 쓴다.
            for (float t = 0f; t < fadeSeconds; t += Time.unscaledDeltaTime)
            {
                content.alpha = Mathf.Lerp(from, to, t / fadeSeconds);
                yield return null;
            }
            content.alpha = to;
            if (hideAtEnd) content.gameObject.SetActive(false);
            fade = null;
        }
    }
}
