// =====================================================================
// PrepNotebookView — 준비 단계의 수첩 (Tab)
// 역할   : 응대 화면에서 적은 수첩 기록(NotebookRecord)을 한 권으로 모아 보여 준다.
//          오른쪽 가죽 책갈피를 누르면 그 면으로 바로 넘어간다.
//          칸은 모두 테두리만 그려서 수첩 종이가 그대로 보이게 한다.
// 호출자 : PrepScreensView (Show)
// 호출   : NotebookRecord (읽기만)
// 상태   : 지금 고른 책갈피 · 그 안의 쪽 번호, 복사해 만든 칸 목록.
// 비고   : 탭 구성은 기획서 · FeatureManagerMap F11 "던전 · 단골 · 사전"을 따른다.
//
// 의사코드
// [책갈피 3개] 던전 · 단골 · 사전. 고른 책갈피는 더 길게 빠져나오고 글자가 밝아진다
// [던전]   층별 위험 두 쪽 전체. 층마다 카드 (번호 · 이름 · 위험 칸 · 재료 칸 · 찾은 단서)
//          못 간 층은 흐리게, 모르는 위험 · 못 본 재료는 ?
// [단골]   단골 기록 (나중에: [단골] 태그 · 단계 ●●○ · 남은 호감 · 열린 사연 다시 읽기)
// [사전]   두 면. 쪽 모서리로 넘긴다
//          1면 단서: 왼쪽 대사 단서 / 오른쪽 초상화 단서
//          2면 종족 · 직업: 왼쪽 종족(○에 약함) / 오른쪽 직업(○에 강함). 못 찾은 것은 ??? · ?
// [원본 칸]  빌더가 만든 원본(꺼 둔 것)을 복사한다. 자식 이름으로 부품을 찾는다.
// =====================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>준비 단계 수첩.</summary>
    public sealed class PrepNotebookView : MonoBehaviour
    {
        // 위험 이름 순서 (riskIcons 순서와 같아야 한다)
        private static readonly string[] RiskNames = { "독", "열", "냉기", "어둠", "체력", "함정" };

        // 책갈피마다 펼칠 면 번호 (사전은 두 면: 단서 → 종족 · 직업)
        private static readonly int[][] TabSpreads = { new[] { 0 }, new[] { 1 }, new[] { 2, 3 } };

        [Header("책갈피 (위에서부터 던전 · 단골 · 사전)")]
        [SerializeField] private Button[] bookmarks;
        [SerializeField] private float bookmarkWidth = 300f;
        /// <summary>고른 책갈피는 이만큼 더 길게 빠져나와 글자가 옆으로 밀려 나온다.</summary>
        [SerializeField] private float selectedBookmarkWidth = 380f;
        [SerializeField] private Color bookmarkLabelColor = new Color32(215, 195, 160, 255);
        [SerializeField] private Color selectedBookmarkLabelColor = new Color32(255, 236, 190, 255);

        [Header("면 4개 (던전 · 단골 · 사전-단서 · 사전-종족직업)"), SerializeField] private GameObject[] spreads;
        [Tooltip("면이 여러 개인 책갈피(사전)에서만 보이는 쪽 넘김 모서리")]
        [SerializeField] private Button prevPageButton;
        [SerializeField] private Button nextPageButton;

        [Header("① 단서 사전")]
        [SerializeField] private RectTransform lineList;
        [SerializeField] private RectTransform portraitList;
        [SerializeField] private GameObject lineEmpty;
        [SerializeField] private GameObject portraitEmpty;

        [Header("② 종족 · 직업")]
        [SerializeField] private RectTransform raceList;
        [SerializeField] private RectTransform jobList;

        [Header("③ 층별 위험 (왼쪽 쪽에 몇 층까지)")]
        [SerializeField] private RectTransform floorListLeft;
        [SerializeField] private RectTransform floorListRight;
        [SerializeField] private int floorsOnLeft = 3;

        [Header("원본 (꺼 둔 채로 복사해서 씀)")]
        [Tooltip("자식: Icon, Title, Sub")]
        [SerializeField] private GameObject clueTemplate;
        [Tooltip("자식: Name, RiskIcon, Unknown, RiskText, Tags")]
        [SerializeField] private GameObject codexRowTemplate;
        [Tooltip("자식: Badge/Number, Title, Status, Risks, Ingredients, Tags, NoClue (+CanvasGroup)")]
        [SerializeField] private GameObject floorCardTemplate;
        [Tooltip("자식: Icon, Unknown, Label")]
        [SerializeField] private GameObject riskSlotTemplate;
        [Tooltip("자식: Icon, Unknown")]
        [SerializeField] private GameObject ingredientTemplate;
        [Tooltip("자식: Icon, Label")]
        [SerializeField] private GameObject tagTemplate;

        [Header("그림 · 색")]
        [Tooltip("독 · 열 · 냉기 · 어둠 · 체력 · 함정 순서")]
        [SerializeField] private Sprite[] riskIcons;
        [SerializeField] private Color faintColor = new Color32(140, 112, 82, 255);
        [SerializeField] private Color visitedColor = new Color32(100, 135, 60, 255);
        [SerializeField, Range(0f, 1f)] private float lockedAlpha = 0.55f;

        [SerializeField] private Button closeButton;

        private readonly List<GameObject> spawned = new List<GameObject>();
        private int currentBookmark;
        private int currentPage; // 책갈피 안에서 몇 번째 면인지

        public event Action CloseClicked;

        private void Awake()
        {
            foreach (var t in new[] { clueTemplate, codexRowTemplate, floorCardTemplate, riskSlotTemplate, ingredientTemplate, tagTemplate })
                t.SetActive(false);

            for (int i = 0; i < bookmarks.Length; i++)
            {
                int index = i; // 람다가 반복 변수 대신 고정된 번호를 쓰도록 복사
                bookmarks[i].onClick.AddListener(() => SelectBookmark(index));
            }
            closeButton.onClick.AddListener(() => CloseClicked?.Invoke());
            prevPageButton.onClick.AddListener(() => TurnPage(-1));
            nextPageButton.onClick.AddListener(() => TurnPage(+1));
        }

        /// <summary>기록을 읽어 수첩 전체를 그린다. 마지막에 본 책갈피를 유지한다.</summary>
        public void Show(PrepViewData data)
        {
            var record = data.notebook ?? NotebookRecord.Session;

            foreach (var go in spawned) { go.SetActive(false); Destroy(go); }
            spawned.Clear();

            DrawClues(record);
            DrawCodex(raceList, data.races, record.Races, "에 약함");
            DrawCodex(jobList, data.jobs, record.Jobs, "에 강함");
            DrawFloors(data.floors, record);

            ShowPage();
        }

        /// <summary>책갈피를 골라 그 책갈피의 첫 면을 펼친다.</summary>
        public void SelectBookmark(int index)
        {
            currentBookmark = Mathf.Clamp(index, 0, Mathf.Min(bookmarks.Length, TabSpreads.Length) - 1);
            currentPage = 0;
            ShowPage();
        }

        // 사전처럼 면이 여러 개인 책갈피에서 앞뒤로 넘긴다
        private void TurnPage(int delta)
        {
            int last = TabSpreads[currentBookmark].Length - 1;
            currentPage = Mathf.Clamp(currentPage + delta, 0, last);
            ShowPage();
        }

        private void ShowPage()
        {
            var pages = TabSpreads[currentBookmark];
            int spread = pages[Mathf.Clamp(currentPage, 0, pages.Length - 1)];
            for (int i = 0; i < spreads.Length; i++)
                spreads[i].SetActive(i == spread);

            // 쪽 넘김 모서리: 넘길 쪽이 있을 때만
            prevPageButton.gameObject.SetActive(pages.Length > 1 && currentPage > 0);
            nextPageButton.gameObject.SetActive(pages.Length > 1 && currentPage < pages.Length - 1);

            for (int i = 0; i < bookmarks.Length; i++)
            {
                bool selected = i == currentBookmark;

                // 길이: 고른 것만 책 밖으로 더 빠져나온다 (글자는 책갈피 오른쪽 끝에 붙어 함께 움직임)
                var rect = (RectTransform)bookmarks[i].transform;
                rect.sizeDelta = new Vector2(selected ? selectedBookmarkWidth : bookmarkWidth, rect.sizeDelta.y);

                // 글자: 고른 것은 밝고 굵게
                var label = bookmarks[i].GetComponentInChildren<TMP_Text>(true);
                if (label)
                {
                    label.color = selected ? selectedBookmarkLabelColor : bookmarkLabelColor;
                    label.fontStyle = selected ? FontStyles.Bold : FontStyles.Normal;
                }
            }
        }

        // =================================================================
        // ① 단서 사전
        // =================================================================

        private void DrawClues(NotebookRecord record)
        {
            int lines = 0, portraits = 0;
            foreach (var clue in record.AllClues())
            {
                bool fromLine = clue.source == ClueSource.Line;
                var item = Spawn(clueTemplate, fromLine ? lineList : portraitList);
                SetRiskIcon(Part<Image>(item, "Icon"), clue.risk);
                Part<TMP_Text>(item, "Title").text = clue.tag;
                var sub = Part<TMP_Text>(item, "Sub");
                sub.text = string.IsNullOrEmpty(clue.risk) ? "어떤 위험인지 아직 몰라요" : clue.RiskLabel + " 위험";
                sub.color = string.IsNullOrEmpty(clue.risk) ? faintColor : PrepPalette.For(clue.risk);
                if (fromLine) lines++; else portraits++;
            }
            lineEmpty.SetActive(lines == 0);
            portraitEmpty.SetActive(portraits == 0);
        }

        // =================================================================
        // ② 종족 · 직업
        // =================================================================

        // word: "에 약함"(종족) / "에 강함"(직업) → "열에 약함"
        private void DrawCodex(RectTransform list, CodexItemData[] items, IReadOnlyList<CodexEntry> entries, string word)
        {
            foreach (var item in items)
            {
                var row = Spawn(codexRowTemplate, list);
                var entry = entries.FirstOrDefault(e => e.Name == item.name);

                // 도감처럼: 아직 단서를 못 찾은 종족 · 직업은 이름도 ??? (약점 없는 인간은 처음부터 보임)
                bool discovered = entry != null || item.noWeakness;
                Part<TMP_Text>(row, "Name").text = discovered ? item.name : "???";
                var clues = entry != null ? entry.Clues : new List<FoundClue>();
                string risk = clues.Select(c => c.risk).FirstOrDefault(r => !string.IsNullOrEmpty(r));

                var icon = Part<Image>(row, "RiskIcon");
                var unknown = row.transform.Find("Unknown").gameObject;
                var text = Part<TMP_Text>(row, "RiskText");

                if (item.noWeakness)
                {
                    // 인간: 약점이 없다는 것 자체가 정보
                    icon.gameObject.SetActive(false);
                    unknown.SetActive(false);
                    text.text = "약점 없음";
                    text.color = faintColor;
                }
                else if (risk != null)
                {
                    icon.gameObject.SetActive(true);
                    SetRiskIcon(icon, risk);
                    unknown.SetActive(false);
                    text.text = risk + word;
                    text.color = PrepPalette.For(risk);
                }
                else
                {
                    icon.gameObject.SetActive(false);
                    unknown.SetActive(true);
                    text.text = string.Empty;
                }

                var tags = (RectTransform)row.transform.Find("Tags");
                foreach (var clue in clues)
                    SpawnTag(tags, clue);
            }
        }

        // =================================================================
        // ③ 층별 위험
        // =================================================================

        private void DrawFloors(FloorItemData[] floors, NotebookRecord record)
        {
            for (int i = 0; i < floors.Length; i++)
            {
                var floor = floors[i];
                var card = Spawn(floorCardTemplate, i < floorsOnLeft ? floorListLeft : floorListRight);

                // 머리: 번호 · 이름 · 발견 여부
                Part<TMP_Text>(card, "Badge/Number").text = floor.number.ToString();
                Part<TMP_Text>(card, "Title").text = floor.opened ? $"{floor.number}층 · {floor.name}" : $"{floor.number}층 · ???";
                var status = Part<TMP_Text>(card, "Status");
                status.text = floor.opened ? "발견함" : "아직 못 감";
                status.color = floor.opened ? visitedColor : faintColor;
                card.GetComponent<CanvasGroup>().alpha = floor.opened ? 1f : lockedAlpha;

                // 이 층에 적힌 단서와, 거기서 알게 된 위험 (찾은 순서, 겹치지 않게)
                var entry = floor.opened ? record.Floors.FirstOrDefault(e => e.Name == FloorKey(floor)) : null;
                var clues = entry != null ? entry.Clues : new List<FoundClue>();
                var found = clues.Select(c => c.risk).Where(r => !string.IsNullOrEmpty(r)).Distinct().ToList();

                // 위험 칸: 층의 위험 수만큼. 알게 된 것은 그림 + 이름, 나머지는 ?
                var risks = (RectTransform)card.transform.Find("Risks");
                int slots = Mathf.Max(floor.riskCount, found.Count);
                for (int s = 0; s < slots; s++)
                    SpawnRiskSlot(risks, s < found.Count ? found[s] : null);

                // 재료 칸: 손님이 가져온 재료는 그림, 못 본 재료는 그림자
                var ingredients = (RectTransform)card.transform.Find("Ingredients");
                foreach (var ing in floor.ingredients)
                {
                    // 본 재료는 그림, 못 본 재료는 ?
                    var slot = Spawn(ingredientTemplate, ingredients);
                    var icon = Part<Image>(slot, "Icon");
                    icon.sprite = ing.icon;
                    icon.gameObject.SetActive(ing.seen && ing.icon);
                    slot.transform.Find("Unknown").gameObject.SetActive(!(ing.seen && ing.icon));
                    var hover = slot.GetComponent<HoverInfo>();
                    if (hover)
                    {
                        if (ing.seen) hover.Set(ing.name, ing.description);
                        else hover.Set("???", "아직 못 본 재료");
                    }
                }

                // 찾은 단서 태그
                var tags = (RectTransform)card.transform.Find("Tags");
                foreach (var clue in clues)
                    SpawnTag(tags, clue);
                var noClue = Part<TMP_Text>(card, "NoClue");
                noClue.gameObject.SetActive(floor.opened && clues.Count == 0);
            }
        }

        // 수첩 기록(NotebookRecord)의 층 이름 (예: 1층 슬라임 숲)
        private static string FloorKey(FloorItemData floor) => $"{floor.number}층 {floor.name}";

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

        private static T Part<T>(GameObject root, string path) where T : Component
        {
            return root.transform.Find(path).GetComponent<T>();
        }

        private void SpawnRiskSlot(RectTransform parent, string risk)
        {
            var slot = Spawn(riskSlotTemplate, parent);
            bool known = risk != null;
            var icon = Part<Image>(slot, "Icon");
            icon.gameObject.SetActive(known);
            slot.transform.Find("Unknown").gameObject.SetActive(!known);
            var label = Part<TMP_Text>(slot, "Label");
            label.gameObject.SetActive(known);
            if (known)
            {
                SetRiskIcon(icon, risk);
                label.text = risk;
                label.color = PrepPalette.For(risk);
            }
            // 아는 칸은 이름까지 들어가게 넓게, 모르는 칸은 동그라미만큼
            var layout = slot.GetComponent<LayoutElement>();
            if (layout) layout.preferredWidth = known ? 110f : 52f;
        }

        private void SpawnTag(RectTransform parent, FoundClue clue)
        {
            var tag = Spawn(tagTemplate, parent);
            var color = PrepPalette.For(clue.risk);
            tag.GetComponent<Image>().color = color;
            SetRiskIcon(Part<Image>(tag, "Icon"), clue.risk);
            var label = Part<TMP_Text>(tag, "Label");
            label.text = clue.tag;
            label.color = color;
        }

        private void SetRiskIcon(Image image, string risk)
        {
            int index = Array.IndexOf(RiskNames, risk);
            bool has = index >= 0 && riskIcons != null && index < riskIcons.Length && riskIcons[index];
            image.enabled = has;
            if (has) image.sprite = riskIcons[index];
        }
    }
}
