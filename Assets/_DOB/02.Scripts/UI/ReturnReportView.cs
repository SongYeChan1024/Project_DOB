// =====================================================================
// ReturnReportView — 아침 귀환 보고 화면 (MorningReportView 교체본)
// 역할   : 돌아온 손님을 꼬리표로 늘어놓고, 고른 손님의 보고 한 장(결과 도장 · 한마디 · 위험 막기 ·
//          가져온 재료 · 수첩 기록 · 호감)을 그린다. 왼쪽에는 그날 점수 등급에 맞는 일러스트를 보인다.
//          값은 계산하지 않고 받은 그대로 그린다. 손님 사이 넘기기만 화면이 직접 한다.
// 호출자 : DayFlowPresenter (Show(MorningReportData), Hide, Confirmed 구독) — BasementHud 「하루 진행 화면」 칸
//          ReturnReportPresenter (Opened 구독 → ShowReport로 손님 보고 채움)
// 호출   : HoverInfo (가져온 재료에 마우스를 올리면 이름), StampPressView (손님 보고를 그릴 때마다 도장 「쾅」)
// 비고   : 어제 호감 비율은 받지 않으므로 「단골까지 N」의 N과 오늘 비율 · 증가량으로 거꾸로 계산한다 (Reveal 참고)
// 상태   : 받은 보고, 지금 보는 손님 번호, 이미 본 손님 표시, 복사해 만든 칸 목록
//
// 의사코드
// [열기]      일차 제목을 쓰고 화면을 켠 뒤 Opened(일차)를 알린다 → Presenter가 ShowReport로 보고를 채운다
// [보고 받기] 첫 손님을 고르고 꼬리표 · 일러스트 · 보고 한 장을 그린다
// [넘기기]    꼬리표 · ◀ 이전 · 다음 ▶ 으로 손님을 바꾼다. 본 손님은 꼬리표의 빨간 점이 사라진다
//             그리기 직전 GuestShowing(손님)을 알린다 → Presenter가 보고 확정을 요청하고 수첩 기록 칸을 채운다
//             보고 한 장을 그릴 때마다 결과 도장을 그 색으로 「쾅」 찍는다 (간신히 · 부상은 더 크게 흔들림)
//             다른 손님으로 넘기면 종이가 왼쪽으로 밀려나고 새 종이가 오른쪽에서 들어온다 (시안 19 ⑧)
//             도장 다음 순서로 「신규」 뱃지가 톡 튀어나오고, 호감 숫자가 +0부터 오르며 게이지가 어제 위치에서 차오른다 (시안 13)
// [최종 정산] SettlementOpening을 알린 뒤(Presenter가 정산 값을 갱신) 전날 정산 창을 띄운다 (시안 27). 결재란을 누르면 도장이 찍히고 확인을 알린다 (시안 28)
//             안 본 보고가 남아도 확인할 수 있다 (보상은 백엔드가 이미 한 번씩 반영). 아래 「전날 정산 창」 의사코드 참고
// [닫기]      DayFlowPresenter가 Hide를 부른다. 도장 연출 중이면 연출이 끝난 뒤 닫는다
// 비고   : [최종 정산] 단추 이름은 OpenSettlement, 결재란 단추 이름은 ConfirmMorningReport (팀 테스트가 이 순서로 누른다).
// =====================================================================
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>아침 귀환 보고 화면.</summary>
    public sealed class ReturnReportView : MorningReportView
    {
        private static readonly string[] Risks = { "독", "열", "냉기", "어둠", "체력", "함정" };

        [SerializeField] private GameObject content;
        [SerializeField] private TMP_Text titleText;

        [Header("돌아온 손님 꼬리표")]
        [SerializeField] private RectTransform guestRow;
        [SerializeField] private ScrollRect guestScroll;
        [Tooltip("자식: Border, Dot, Name, Result, Glow, Unseen. 루트에 Button")]
        [SerializeField] private GameObject guestChipTemplate;
        [SerializeField] private Color chipColor = new Color32(226, 208, 176, 255);
        [SerializeField] private Color chipSelectedColor = new Color32(250, 240, 215, 255);

        [Header("그날 일러스트 (1번 = 가장 높은 점수 ~ 5번 = 가장 낮은 점수)")]
        [SerializeField] private Image illustration;
        [Tooltip("일러스트가 없을 때만 보이는 빈 칸 테두리")]
        [SerializeField] private GameObject illustrationFrame;
        [SerializeField] private Sprite[] dayIllustrations = new Sprite[5];

        [Header("보고 한 장")]
        [SerializeField] private GameObject paperContent;
        [SerializeField] private GameObject emptyText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text whoText;
        [SerializeField] private Image stampRing;
        [SerializeField] private TMP_Text stampText;
        /// <summary>손님을 넘길 때마다 도장을 「쾅」 찍는 연출. 비워 두면 연출 없이 바로 보인다.</summary>
        [SerializeField] private StampPressView stampPress;
        [SerializeField] private TMP_Text lineText;

        [Header("위험 막기")]
        [SerializeField] private RectTransform axisList;
        [SerializeField] private ScrollRect axisScroll;
        [Tooltip("자식: Icon, Name, Result, Risk, Effect")]
        [SerializeField] private GameObject axisRowTemplate;
        [Tooltip("독 · 열 · 냉기 · 어둠 · 체력 · 함정 순서")]
        [SerializeField] private Sprite[] riskIcons = new Sprite[6];
        [SerializeField] private Color heldColor = new Color32(90, 130, 50, 255);
        [SerializeField] private Color shortColor = new Color32(170, 70, 50, 255);

        [Header("가져온 재료 · 수첩")]
        [SerializeField] private RectTransform lootList;
        [Tooltip("자식: Icon, New. 루트에 HoverInfo")]
        [SerializeField] private GameObject lootTemplate;
        [SerializeField] private GameObject lootEmpty;
        [SerializeField] private TMP_Text noteFloorText;
        [SerializeField] private RectTransform noteList;
        [Tooltip("자식: Icon, Text")]
        [SerializeField] private GameObject noteTemplate;
        [SerializeField] private GameObject noteEmpty;

        [Header("호감")]
        [SerializeField] private TMP_Text affectionDelta;
        [SerializeField] private TMP_Text affectionWhy;
        [SerializeField] private TMP_Text affectionNext;
        [Tooltip("채움 막대. 오른쪽 끝(anchorMax.x)을 비율로 맞춘다")]
        [SerializeField] private RectTransform affectionFill;

        [Header("드러내기 순서 (초)")]
        /// <summary>도장이 찍힌 뒤 「신규」 뱃지가 튀어나오기까지.</summary>
        [SerializeField] private float badgeDelay = 0.45f;
        [SerializeField] private float badgeSeconds = 0.3f;
        /// <summary>뱃지 다음 호감이 오르기 시작하기까지.</summary>
        [SerializeField] private float affectionDelay = 0.15f;
        [SerializeField] private float affectionSeconds = 0.8f;
        /// <summary>손님을 넘길 때 지금 종이가 밀려나는 시간 · 새 종이가 들어오는 시간 (시안 19 ⑧).</summary>
        [SerializeField] private float turnOutSeconds = 0.12f;
        [SerializeField] private float turnInSeconds = 0.13f;

        [Header("단추")]
        [SerializeField] private Button prevButton;
        [SerializeField] private Button nextButton;
        /// <summary>[최종 정산] 단추 (이름 OpenSettlement). 정산 창을 연다. 정산 데이터가 없으면 바로 확인한다.</summary>
        [SerializeField] private Button confirmButton;

        [Header("전날 정산 창 (시안 27 · 28)")]
        [SerializeField] private GameObject settlementRoot;
        [SerializeField] private RectTransform settlementPaper;
        [SerializeField] private TMP_Text settleTitle;
        [SerializeField] private TMP_Text settleSummary;
        /// <summary>대성공 · 성공 · 간신히 · 부상 손님 수 글자.</summary>
        [SerializeField] private TMP_Text[] settleGrades = new TMP_Text[4];
        [SerializeField] private TMP_Text settleLoot;
        [SerializeField] private TMP_Text settleAffection;
        [SerializeField] private TMP_Text settleAffectionMax;
        /// <summary>호감 막대 바탕 · 채움 · 눈금 견본 (견본을 복사해 눈금마다 놓는다).</summary>
        [SerializeField] private RectTransform settleBar;
        [SerializeField] private RectTransform settleBarFill;
        [SerializeField] private RectTransform settleTickTemplate;
        [SerializeField] private TMP_Text settleNewGuests;
        [SerializeField] private TMP_Text settleNews;
        [SerializeField] private GameObject settleGoldBox;
        [SerializeField] private TMP_Text settleGoldEarned;
        [SerializeField] private TMP_Text settleGoldNow;
        /// <summary>결재란 단추. 이름은 ConfirmMorningReport (팀 테스트가 이 이름으로 누른다) — 누르면 보고 확인을 알린다.</summary>
        [SerializeField] private Button stampButton;
        [SerializeField] private GameObject stampGuide;
        [SerializeField] private Image stampSeal;
        [SerializeField] private float sealSlamSeconds = 0.12f;
        [SerializeField] private float closeDelay = 0.4f;
        [SerializeField] private float closeSeconds = 0.35f;

        private readonly List<GameObject> chips = new List<GameObject>();
        private readonly List<GameObject> spawned = new List<GameObject>();
        private ReturnReportViewData report = new ReturnReportViewData();
        private bool[] seen = Array.Empty<bool>();
        private int index;
        private Coroutine reveal;
        private int drawnIndex = -1;  // 지금 종이에 그려진 손님 (-1: 아직 없음)
        private Coroutine pageTurn;
        private CanvasGroup paperGroup;
        private Vector2 paperHome;

        /// <summary>화면이 열렸다 (일차). Presenter가 그날 보고를 ShowReport로 채운다.</summary>
        public event Action<int> Opened;
        /// <summary>손님 보고 한 장을 그리기 직전 (그 손님). Presenter가 보고 확정을 요청하고 수첩 기록 칸을 채울 수 있다.
        /// 같은 손님을 다시 그려도 또 알린다 (확정은 백엔드가 한 번만 처리).</summary>
        public event Action<ReturnGuestData> GuestShowing;
        /// <summary>전날 정산 창을 채우기 직전. Presenter가 남은 보고를 확정하고 정산 값(층 개방 등)을 갱신할 수 있다.</summary>
        public event Action SettlementOpening;

        private void Awake()
        {
            foreach (var t in new[] { guestChipTemplate, axisRowTemplate, lootTemplate, noteTemplate })
                t.SetActive(false);
            prevButton.onClick.AddListener(() => Select(index - 1));
            nextButton.onClick.AddListener(() => Select(index + 1));
            confirmButton.onClick.AddListener(OpenSettlement);
            if (stampButton) stampButton.onClick.AddListener(OnStamp);
            if (settleTickTemplate) settleTickTemplate.gameObject.SetActive(false);
            if (settlementPaper) settlementHome = settlementPaper.anchoredPosition;
            CloseSettlement();
        }

        // =================================================================
        // 전날 정산 창 (시안 27 · 28)
        // 의사코드
        // [최종 정산]  정산 데이터가 없으면 바로 확인. 있으면 창을 채우고 아래에서 떠오르며 연다 (결재란은 「이곳을 눌러 날인」)
        // [결재란]     한 번만: 도장 자국이 1.3배로 쾅 나타나 0.12초에 제자리 크기, 종이가 살짝 흔들림 (기울기 −8~−14°)
        //              누르는 즉시 보고 확인을 알린다 (준비 단계로 넘어가는 판정은 바로) — 화면은 연출을 마저 하고 닫힌다
        //              0.4초 뒤 종이가 위로 걷히며 사라지고, 그사이 Hide가 왔으면 연출이 끝난 뒤 보고 화면을 끈다
        // =================================================================

        private Vector2 settlementHome;
        private Coroutine stamping;
        private bool hidePending;
        private readonly List<GameObject> ticks = new List<GameObject>();

        private void OpenSettlement()
        {
            SettlementOpening?.Invoke();
            var s = report.settlement;
            if (s == null || !settlementRoot) { RaiseConfirmed(); return; }
            settleTitle.text = $"{s.day}일차 영업 정산";
            settleSummary.text = s.summary;
            for (int i = 0; i < settleGrades.Length && i < s.grades.Length; i++) settleGrades[i].text = s.grades[i].ToString();
            settleLoot.text = string.IsNullOrEmpty(s.loot) ? "가져온 재료가 없어요" : "가져온 재료  " + s.loot;
            settleAffection.text = (s.affectionSum > 0 ? "+" : "") + s.affectionSum;
            settleAffectionMax.text = s.affectionMax > 0 ? $"/ {s.affectionMax}점" : "점";
            settleBar.gameObject.SetActive(s.affectionMax > 0);
            if (s.affectionMax > 0) DrawBar(Mathf.Clamp01((float)s.affectionSum / s.affectionMax), s.affectionSteps);
            settleNewGuests.gameObject.SetActive(!string.IsNullOrEmpty(s.newGuests));
            settleNewGuests.text = $"오늘 새 손님 <color=#2F7A3A>{s.newGuests}</color>";
            settleNews.text = s.news.Length > 0 ? string.Join("\n", s.news) : "<color=#8A7055>새로 알게 된 것이 없어요</color>";
            settleGoldBox.SetActive(s.goldEarned != null || s.goldNow != null);
            settleGoldEarned.transform.parent.gameObject.SetActive(s.goldEarned != null);
            settleGoldEarned.text = s.goldEarned;
            settleGoldNow.transform.parent.gameObject.SetActive(s.goldNow != null);
            settleGoldNow.text = s.goldNow;

            stampGuide.SetActive(true);
            stampSeal.gameObject.SetActive(false);
            stampButton.interactable = true;
            settlementRoot.SetActive(true);
            settlementRoot.transform.SetAsLastSibling();
            if (isActiveAndEnabled) StartCoroutine(RiseIn());
        }

        // 막대: 채움 비율 + 눈금마다 견본 복사 (눈금을 넘었으면 빨갛게)
        private void DrawBar(float ratio, float[] steps)
        {
            settleBarFill.anchorMax = new Vector2(ratio, 1f);
            foreach (var t in ticks) Destroy(t);
            ticks.Clear();
            foreach (float step in steps)
            {
                var tick = Instantiate(settleTickTemplate, settleBar);
                tick.gameObject.SetActive(true);
                tick.anchorMin = tick.anchorMax = new Vector2(Mathf.Clamp01(step), 0.5f);
                var label = tick.GetComponentInChildren<TMP_Text>(true);
                if (label) label.text = Mathf.RoundToInt(step * 100) + "%";
                ticks.Add(tick.gameObject);
            }
        }

        private System.Collections.IEnumerator RiseIn()
        {
            var group = Group(settlementRoot);
            for (float t = 0f; t < 0.2f; t += Mathf.Min(Time.unscaledDeltaTime, 0.05f))
            {
                float e = 1f - (1f - t / 0.2f) * (1f - t / 0.2f);
                group.alpha = e;
                settlementPaper.anchoredPosition = settlementHome + Vector2.down * (24f * (1f - e));
                settlementPaper.localScale = Vector3.one * Mathf.Lerp(0.96f, 1f, e);
                yield return null;
            }
            group.alpha = 1f;
            settlementPaper.anchoredPosition = settlementHome;
            settlementPaper.localScale = Vector3.one;
        }

        private void OnStamp()
        {
            if (stamping != null) return;
            stampButton.interactable = false;
            stamping = StartCoroutine(Stamp());
            RaiseConfirmed(); // 판정은 바로 (준비 단계로). 화면은 연출을 마치고 닫힌다 (Hide 참고)
        }

        private System.Collections.IEnumerator Stamp()
        {
            stampGuide.SetActive(false);
            var seal = stampSeal.rectTransform;
            stampSeal.gameObject.SetActive(true);
            seal.localRotation = Quaternion.Euler(0f, 0f, -UnityEngine.Random.Range(8f, 14f));
            // 쾅: 1.3배 · 연하게 → 제자리 크기 · 진하게, 종이는 잠깐 흔들림
            for (float t = 0f; t < sealSlamSeconds; t += Mathf.Min(Time.unscaledDeltaTime, 0.05f))
            {
                float k = t / sealSlamSeconds;
                seal.localScale = Vector3.one * Mathf.Lerp(1.3f, 1f, k * k);
                stampSeal.color = new Color(1f, 1f, 1f, Mathf.Lerp(0.6f, 1f, k));
                yield return null;
            }
            seal.localScale = Vector3.one;
            stampSeal.color = Color.white;
            for (float t = 0f; t < 0.15f; t += Mathf.Min(Time.unscaledDeltaTime, 0.05f))
            {
                settlementPaper.anchoredPosition = settlementHome + new Vector2(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-4f, 4f)) * (1f - t / 0.15f);
                yield return null;
            }
            settlementPaper.anchoredPosition = settlementHome;

            for (float t = 0f; t < closeDelay; t += Time.unscaledDeltaTime) yield return null;

            // 종이가 위로 걷히며 사라짐
            var group = Group(settlementRoot);
            for (float t = 0f; t < closeSeconds; t += Mathf.Min(Time.unscaledDeltaTime, 0.05f))
            {
                float k = t / closeSeconds, e = k * k;
                settlementPaper.anchoredPosition = settlementHome + Vector2.up * (700f * e);
                settlementPaper.localRotation = Quaternion.Euler(0f, 0f, -3f * e);
                group.alpha = 1f - e;
                yield return null;
            }
            stamping = null;
            CloseSettlement();
            if (hidePending) { hidePending = false; if (content) content.SetActive(false); }
        }

        private void CloseSettlement()
        {
            if (!settlementRoot) return;
            settlementRoot.SetActive(false);
            if (settlementPaper)
            {
                settlementPaper.anchoredPosition = settlementHome;
                settlementPaper.localRotation = Quaternion.identity;
                settlementPaper.localScale = Vector3.one;
            }
            Group(settlementRoot).alpha = 1f;
        }

        private static CanvasGroup Group(GameObject go)
        {
            var group = go.GetComponent<CanvasGroup>();
            return group ? group : go.AddComponent<CanvasGroup>();
        }

        // =================================================================
        // MorningReportView (DayFlowPresenter가 부름)
        // =================================================================

        public override void Show(MorningReportData data)
        {
            titleText.text = $"{data.Day}일차 아침 · 귀환 보고";
            if (stamping != null) { StopCoroutine(stamping); stamping = null; }
            hidePending = false;
            CloseSettlement();
            content.SetActive(true);
            ShowReport(new ReturnReportViewData()); // 보고가 오기 전엔 빈 상태
            Opened?.Invoke(data.Day);
        }

        public override void Hide()
        {
            // 도장을 찍고 창이 걷히는 중이면 연출이 끝난 뒤 끈다 (Stamp 끝에서)
            if (stamping != null) { hidePending = true; return; }
            CloseSettlement();
            if (content) content.SetActive(false);
        }

        // =================================================================
        // 보고 그리기 (ReturnReportPresenter가 부름)
        // =================================================================

        public void ShowReport(ReturnReportViewData data)
        {
            report = data ?? new ReturnReportViewData();
            seen = new bool[report.guests.Length];
            drawnIndex = -1; // 새 보고는 넘김 없이 바로 그린다
            DrawIllustration();
            DrawChips();
            Select(0);
        }

        private void Select(int i)
        {
            bool any = report.guests.Length > 0;
            paperContent.SetActive(any);
            emptyText.SetActive(!any);
            prevButton.interactable = any && i > 0;
            nextButton.interactable = any && i < report.guests.Length - 1;
            if (!any) return;

            index = Mathf.Clamp(i, 0, report.guests.Length - 1);
            seen[index] = true;
            // 그리기 전에 알린다 → Presenter가 같은 데이터에 수첩 기록을 채우면 이번에 바로 보인다
            GuestShowing?.Invoke(report.guests[index]);
            prevButton.interactable = index > 0;
            nextButton.interactable = index < report.guests.Length - 1;
            RefreshChips();

            // 다른 손님으로 넘길 때만 종이를 넘긴다 (처음 열 때 · 같은 손님은 바로 그림)
            bool turn = drawnIndex >= 0 && drawnIndex != index && isActiveAndEnabled;
            drawnIndex = index;
            if (pageTurn != null) { StopCoroutine(pageTurn); pageTurn = null; }
            if (turn) pageTurn = StartCoroutine(TurnPage(report.guests[index]));
            else { PosePaper(0f, 0f, 1f); DrawGuest(report.guests[index]); }
        }

        // 의사코드 (시안 19 ⑧): 지금 종이가 왼쪽으로 밀려나며 기울고 흐려짐 → 새 손님으로 다시 그림(도장 「쾅」 시작)
        //   → 새 종이가 오른쪽에서 들어와 제자리. 넘기는 중에 또 넘기면 앞 넘김을 멈추고 새로 시작한다
        private System.Collections.IEnumerator TurnPage(ReturnGuestData next)
        {
            for (float t = 0f; t < turnOutSeconds; t += Step())
            {
                float e = t / turnOutSeconds;
                PosePaper(-60f * e, -4f * e, 1f - e);
                yield return null;
            }
            DrawGuest(next);
            for (float t = 0f; t < turnInSeconds; t += Step())
            {
                float e = 1f - t / turnInSeconds;
                e = e * e; // 끝에서 부드럽게 멈추게
                PosePaper(40f * e, 2f * e, 1f - e);
                yield return null;
            }
            PosePaper(0f, 0f, 1f);
            pageTurn = null;
        }

        private void PosePaper(float x, float degrees, float alpha)
        {
            var rect = (RectTransform)paperContent.transform;
            if (!paperGroup)
            {
                paperHome = rect.anchoredPosition;
                paperGroup = paperContent.GetComponent<CanvasGroup>();
                if (!paperGroup) paperGroup = paperContent.AddComponent<CanvasGroup>();
            }
            rect.anchoredPosition = paperHome + new Vector2(x, 0f);
            rect.localRotation = Quaternion.Euler(0f, 0f, degrees);
            paperGroup.alpha = alpha;
        }

        private void DrawIllustration()
        {
            int tier = report.illustrationTier;
            var sprite = tier >= 1 && tier <= dayIllustrations.Length ? dayIllustrations[tier - 1] : null;
            illustration.sprite = sprite;
            illustration.enabled = sprite;
            illustrationFrame.SetActive(!sprite);
        }

        private void DrawChips()
        {
            foreach (var go in chips) Destroy(go);
            chips.Clear();
            for (int i = 0; i < report.guests.Length; i++)
            {
                var guest = report.guests[i];
                var chip = Instantiate(guestChipTemplate, guestRow);
                chip.SetActive(true);
                var color = ResultColor(guest.result);
                chip.transform.Find("Border").GetComponent<Image>().color = color;
                chip.transform.Find("Dot").GetComponent<Image>().color = color;
                chip.transform.Find("Name").GetComponent<TMP_Text>().text = guest.name;
                chip.transform.Find("Result").GetComponent<TMP_Text>().text = ResultWord(guest.result);
                int n = i; // 람다가 반복 변수 대신 고정된 번호를 쓰도록 복사
                chip.GetComponent<Button>().onClick.AddListener(() => Select(n));
                chips.Add(chip);
            }
            if (guestScroll) guestScroll.horizontalNormalizedPosition = 0f;
        }

        // 지금 손님: 밝은 종이 + 모서리 빛 / 아직 안 본 손님: 빨간 점
        private void RefreshChips()
        {
            for (int i = 0; i < chips.Count; i++)
            {
                bool selected = i == index;
                chips[i].GetComponent<Image>().color = selected ? chipSelectedColor : chipColor;
                chips[i].transform.Find("Glow").gameObject.SetActive(selected);
                chips[i].transform.Find("Unseen").gameObject.SetActive(!seen[i]);
            }
        }

        private void DrawGuest(ReturnGuestData g)
        {
            foreach (var go in spawned) Destroy(go);
            spawned.Clear();

            nameText.text = g.name;
            whoText.text = string.IsNullOrEmpty(g.trip) ? g.raceJob : $"{g.raceJob}  ·  {g.trip}";
            var color = ResultColor(g.result);
            stampRing.color = color;
            stampText.color = color;
            stampText.text = ResultWord(g.result);
            if (stampPress) stampPress.Press(color, g.result == ReturnResult.Failure || g.result == ReturnResult.GreatFailure);
            lineText.text = string.IsNullOrEmpty(g.line) ? string.Empty : $"“{g.line}”";

            // 위험 막기
            foreach (var axis in g.axes) DrawAxis(axis);
            if (axisScroll) axisScroll.verticalNormalizedPosition = 1f;

            var badges = new List<Transform>();
            // 가져온 재료
            foreach (var item in g.loot)
            {
                var go = Spawn(lootTemplate, lootList);
                var icon = go.transform.Find("Icon").GetComponent<Image>();
                icon.sprite = item.icon;
                icon.enabled = item.icon;
                go.transform.Find("New").gameObject.SetActive(item.isNew);
                if (item.isNew) badges.Add(go.transform.Find("New"));
                var hover = go.GetComponent<HoverInfo>();
                if (hover) hover.Set(item.name, item.isNew ? "신규 재료" : null);
            }
            lootEmpty.SetActive(g.loot.Length == 0);

            // 수첩에 적었어요
            noteFloorText.text = g.noteFloor;
            foreach (var note in g.noteAxes)
            {
                var go = Spawn(noteTemplate, noteList);
                var icon = go.transform.Find("Icon").GetComponent<Image>();
                icon.sprite = RiskIcon(note.risk);
                icon.enabled = icon.sprite;
                var text = go.transform.Find("Text").GetComponent<TMP_Text>();
                text.text = $"{note.risk} {new string('■', Mathf.Max(0, note.amount))}";
                text.color = PrepPalette.For(note.risk);
            }
            noteEmpty.SetActive(g.noteAxes.Length == 0);

            // 호감
            // 내역이 없으면 호감 기록이 없는 것 → "+0" 대신 비운다
            bool hasAffection = !string.IsNullOrEmpty(g.affectionWhy);
            affectionDelta.text = !hasAffection ? "" : g.affectionDelta >= 0 ? "+" + g.affectionDelta : g.affectionDelta.ToString();
            affectionWhy.text = g.affectionWhy;
            affectionNext.text = g.affectionNext;
            var max = affectionFill.anchorMax;
            max.x = Mathf.Clamp01(g.affectionRatio);
            affectionFill.anchorMax = max;

            if (reveal != null) StopCoroutine(reveal);
            reveal = isActiveAndEnabled ? StartCoroutine(Reveal(badges, g, hasAffection)) : null;
        }

        // 도장 → 「신규」 뱃지 톡 → 호감 숫자 · 게이지 차오름. 끝나면 DrawGuest가 그린 최종 값과 같다
        private System.Collections.IEnumerator Reveal(List<Transform> badges, ReturnGuestData g, bool hasAffection)
        {
            float to = Mathf.Clamp01(g.affectionRatio);
            float from = YesterdayRatio(g);
            foreach (var b in badges) b.localScale = Vector3.zero;
            if (hasAffection) affectionDelta.text = "+0";
            SetFill(from);

            yield return Wait(badgeDelay);
            for (float t = 0f; t < badgeSeconds; t += Step())
            {
                float k = t / badgeSeconds;
                // 0 → 1.3 → 1 로 톡 튀고, 기울었다 제자리로
                float s = k < 0.6f ? Mathf.Lerp(0f, 1.3f, k / 0.6f) : Mathf.Lerp(1.3f, 1f, (k - 0.6f) / 0.4f);
                foreach (var b in badges) { b.localScale = Vector3.one * s; b.localRotation = Quaternion.Euler(0f, 0f, -8f * (1f - k)); }
                yield return null;
            }
            foreach (var b in badges) { b.localScale = Vector3.one; b.localRotation = Quaternion.identity; }

            yield return Wait(affectionDelay);
            for (float t = 0f; t < affectionSeconds; t += Step())
            {
                float k = t / affectionSeconds;
                float e = 1f - (1f - k) * (1f - k);
                if (hasAffection && g.affectionDelta > 0) affectionDelta.text = "+" + Mathf.RoundToInt(g.affectionDelta * e);
                SetFill(Mathf.Lerp(from, to, e));
                yield return null;
            }
            SetFill(to);
            if (hasAffection) affectionDelta.text = g.affectionDelta >= 0 ? "+" + g.affectionDelta : g.affectionDelta.ToString();

            // 끝 숫자를 한 번 크게 했다가 돌려놓는다
            var r = affectionDelta.rectTransform;
            for (float t = 0f; t < 0.2f; t += Step())
            {
                r.localScale = Vector3.one * (1f + 0.3f * Mathf.Sin(Mathf.PI * t / 0.2f));
                yield return null;
            }
            r.localScale = Vector3.one;
            reveal = null;
        }

        // shortcut: 어제 비율을 「단골까지 N」 글자에서 거꾸로 구한다 (전체 = N ÷ (1 − 오늘 비율)).
        //           백엔드가 어제 비율(또는 전체 칸 수)을 보고 데이터에 주면 그 값으로 바꾼다
        private static float YesterdayRatio(ReturnGuestData g)
        {
            float to = Mathf.Clamp01(g.affectionRatio);
            var m = System.Text.RegularExpressions.Regex.Match(g.affectionNext ?? "", @"\d+");
            if (!m.Success || to >= 1f || g.affectionDelta <= 0) return to;
            float total = int.Parse(m.Value) / (1f - to);
            return total > 0f ? Mathf.Clamp01(to - g.affectionDelta / total) : to;
        }

        private void SetFill(float ratio)
        {
            var max = affectionFill.anchorMax;
            max.x = ratio;
            affectionFill.anchorMax = max;
        }

        // 일시 정지와 무관하게, 멈칫한 프레임에 연출이 통째로 건너뛰지 않게
        private static float Step() => Mathf.Min(Time.unscaledDeltaTime, 0.05f);

        private static System.Collections.IEnumerator Wait(float seconds)
        {
            for (float t = 0f; t < seconds; t += Step()) yield return null;
        }

        private void DrawAxis(AxisResultData axis)
        {
            var row = Spawn(axisRowTemplate, axisList);
            var color = PrepPalette.For(axis.risk);
            row.GetComponent<Image>().color = color;
            var icon = row.transform.Find("Icon").GetComponent<Image>();
            icon.sprite = RiskIcon(axis.risk);
            icon.enabled = icon.sprite;
            var name = row.transform.Find("Name").GetComponent<TMP_Text>();
            name.text = axis.risk;
            name.color = color;

            bool surprise = !string.IsNullOrEmpty(axis.surprise);
            bool held = surprise ? axis.surpriseHeld : axis.shortfall <= 0;
            var result = row.transform.Find("Result").GetComponent<TMP_Text>();
            result.text = surprise ? axis.surprise : held ? "막아냈다" : $"{axis.shortfall}칸 모자랐다";
            result.color = held ? heldColor : shortColor;

            // 위험 · 요리 칸은 값을 받았을 때만 (백엔드가 부족 칸만 줄 때는 숨김)
            var risk = row.transform.Find("Risk").GetComponent<TMP_Text>();
            risk.text = axis.need >= 0 ? Pips(axis.need) : string.Empty;
            risk.color = color;
            var effect = row.transform.Find("Effect").GetComponent<TMP_Text>();
            effect.text = axis.have >= 0 ? Pips(axis.have) : string.Empty;
            effect.color = color;
        }

        // =================================================================
        // 도우미
        // =================================================================

        // 칸 그림: 채운 칸 ■ + 빈 칸 □ (최소 3칸)
        private static string Pips(int filled)
        {
            filled = Mathf.Max(0, filled);
            return new string('■', filled) + new string('□', Mathf.Max(0, 3 - filled));
        }

        private Sprite RiskIcon(string risk)
        {
            int i = Array.IndexOf(Risks, risk);
            return i >= 0 && i < riskIcons.Length ? riskIcons[i] : null;
        }

        private static string ResultWord(ReturnResult result)
        {
            switch (result)
            {
                case ReturnResult.GreatSuccess: return "대성공";
                case ReturnResult.Success: return "성공";
                case ReturnResult.Failure: return "간신히";
                default: return "부상";
            }
        }

        // 도장 · 꼬리표 색 (기획에 정한 색 없음 — 시안 기준)
        private static Color ResultColor(ReturnResult result)
        {
            switch (result)
            {
                case ReturnResult.GreatSuccess: return new Color32(205, 150, 30, 255);
                case ReturnResult.Success: return new Color32(85, 135, 55, 255);
                case ReturnResult.Failure: return new Color32(200, 110, 40, 255);
                default: return new Color32(175, 55, 45, 255);
            }
        }

        private GameObject Spawn(GameObject template, Transform parent)
        {
            var go = Instantiate(template, parent);
            go.SetActive(true);
            spawned.Add(go);
            return go;
        }
    }
}
