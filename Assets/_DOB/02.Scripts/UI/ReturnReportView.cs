// =====================================================================
// ReturnReportView — 아침 귀환 보고 화면 (MorningReportView 교체본)
// 역할   : 돌아온 손님을 꼬리표로 늘어놓고, 고른 손님의 보고 한 장(결과 도장 · 한마디 · 위험 막기 ·
//          가져온 재료 · 수첩 기록 · 호감)을 그린다. 왼쪽에는 그날 점수 등급에 맞는 일러스트를 보인다.
//          값은 계산하지 않고 받은 그대로 그린다. 손님 사이 넘기기만 화면이 직접 한다.
// 호출자 : DayFlowPresenter (Show(MorningReportData), Hide, Confirmed 구독) — BasementHud 「하루 진행 화면」 칸
//          ReturnReportPresenter (Opened 구독 → ShowReport로 손님 보고 채움)
// 호출   : HoverInfo (가져온 재료에 마우스를 올리면 이름)
// 상태   : 받은 보고, 지금 보는 손님 번호, 이미 본 손님 표시, 복사해 만든 칸 목록
//
// 의사코드
// [열기]      일차 제목을 쓰고 화면을 켠 뒤 Opened(일차)를 알린다 → Presenter가 ShowReport로 보고를 채운다
// [보고 받기] 첫 손님을 고르고 꼬리표 · 일러스트 · 보고 한 장을 그린다
// [넘기기]    꼬리표 · ◀ 이전 · 다음 ▶ 으로 손님을 바꾼다. 본 손님은 꼬리표의 빨간 점이 사라진다
// [준비하러 가기] 안 본 보고가 남아도 바로 확인을 알린다 (보상은 백엔드가 이미 한 번씩 반영)
// [닫기]      DayFlowPresenter가 Hide를 부른다
// 비고   : [준비하러 가기] 단추 이름은 ConfirmMorningReport (BasementFirstDayTests가 이 이름으로 누른다).
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

        [Header("단추")]
        [SerializeField] private Button prevButton;
        [SerializeField] private Button nextButton;
        [Tooltip("이름은 ConfirmMorningReport 로 둔다 (테스트가 이름으로 누름)")]
        [SerializeField] private Button confirmButton;

        private readonly List<GameObject> chips = new List<GameObject>();
        private readonly List<GameObject> spawned = new List<GameObject>();
        private ReturnReportViewData report = new ReturnReportViewData();
        private bool[] seen = Array.Empty<bool>();
        private int index;

        /// <summary>화면이 열렸다 (일차). Presenter가 그날 보고를 ShowReport로 채운다.</summary>
        public event Action<int> Opened;

        private void Awake()
        {
            foreach (var t in new[] { guestChipTemplate, axisRowTemplate, lootTemplate, noteTemplate })
                t.SetActive(false);
            prevButton.onClick.AddListener(() => Select(index - 1));
            nextButton.onClick.AddListener(() => Select(index + 1));
            confirmButton.onClick.AddListener(RaiseConfirmed);
        }

        // =================================================================
        // MorningReportView (DayFlowPresenter가 부름)
        // =================================================================

        public override void Show(MorningReportData data)
        {
            titleText.text = $"{data.Day}일차 아침 · 귀환 보고";
            content.SetActive(true);
            ShowReport(new ReturnReportViewData()); // 보고가 오기 전엔 빈 상태
            Opened?.Invoke(data.Day);
        }

        public override void Hide()
        {
            if (content) content.SetActive(false);
        }

        // =================================================================
        // 보고 그리기 (ReturnReportPresenter가 부름)
        // =================================================================

        public void ShowReport(ReturnReportViewData data)
        {
            report = data ?? new ReturnReportViewData();
            seen = new bool[report.guests.Length];
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
            prevButton.interactable = index > 0;
            nextButton.interactable = index < report.guests.Length - 1;
            RefreshChips();
            DrawGuest(report.guests[index]);
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
            lineText.text = string.IsNullOrEmpty(g.line) ? string.Empty : $"“{g.line}”";

            // 위험 막기
            foreach (var axis in g.axes) DrawAxis(axis);
            if (axisScroll) axisScroll.verticalNormalizedPosition = 1f;

            // 가져온 재료
            foreach (var item in g.loot)
            {
                var go = Spawn(lootTemplate, lootList);
                var icon = go.transform.Find("Icon").GetComponent<Image>();
                icon.sprite = item.icon;
                icon.enabled = item.icon;
                go.transform.Find("New").gameObject.SetActive(item.isNew);
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
