// =====================================================================
// GuestDialogueView — 손님 응대 화면 표시와 입력
// 역할   : 손님 표시 데이터를 받아 초상 · 대사 · 수첩을 그리고,
//          숨은 단서(대사 키워드 · 초상화 부분)를 찾으면 수첩에 적는다.
//          버튼 입력은 이벤트로 알린다. 자리 · 판정 · 저장은 모른다.
// 호출자 : Presenter (Open, Close, ShowNotice, ShowDictionaryEntry)
// 호출   : NotebookRecord (찾은 단서 기록), NotebookPager (쪽 넘기기)
// 상태   : 열림 여부, 전환 중 여부, 수첩 펼침 여부, 지금 손님 데이터.
//
// 의사코드 (구현 전 작성 · 2026-11-23 홍준우와 합의)
// [열릴 때]     (기본) 손님 얼굴 줌인이 끝난 화면 위로 응대 화면만 서서히 나타난다
//               (fadeThroughBlack 켬) 검은 화면 → 데이터 표시 → 밝아짐
//               이름판에는 이름만 (종족 · 직업은 숨김 — 미리 보이면 정답 힌트가 됨)
// [대사]        [[ ]] 키워드는 숨겨 둔다 (밑줄 없음, 마우스를 올려도 티 안 남)
//               맞는 키워드를 누르면 → 그 말에 밑줄 + 수첩에 기록. 틀린 곳은 무반응
// [초상화]      숨은 단서 자리를 누르면 → 동그라미 표시 + 수첩에 기록. 틀린 곳은 무반응
// [수첩에 기록] · 위험 쪽: 빈 줄 6개 중 위에서부터 위험 이름이 적힘
//                 같은 위험을 또 찾으면 그 줄의 작은 칸(최대 5칸)이 하나씩 참
//               · 단서 사전: '대사 단서' / '초상화 단서' 줄에 [단서](위험) 태그
//               · 도감: 단서가 가리키는 종족 · 직업 · 층 줄에 태그 (처음엔 ???)
//               기록하면 수첩을 첫 면(위험 + 단서 사전)으로 펼쳐 보여 준다
// [수첩]        탭을 누르면 옆으로 미끄러져 열리고 닫힌다. 쪽 넘기기는 NotebookPager가 맡는다
// [긴 대사]     넘치면 쪽으로 나누고 [다음]으로 넘긴다
// [버튼]        따라오세요 · 닫기 · 돌려보내기(확인창) → 이벤트로 알리기만 한다
// [닫힐 때]     (기본) 응대 화면만 서서히 사라진다 → 그 뒤 줌아웃은 FirstGuestService가 맡는다
//               전환 중에는 FadeOverlay가 투명한 채로 클릭을 막는다 (닫기 끝 신호로도 쓰임)
// =====================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>손님 응대 화면. 표시와 입력 알림만 하고 게임 상태를 바꾸지 않는다.</summary>
    public sealed class GuestDialogueView : MonoBehaviour
    {
        /// <summary>위험 종류 하나의 이름 · 아이콘 · 색.</summary>
        [Serializable]
        public struct RiskKind
        {
            public string name;
            public Sprite icon;
            public Color color;
        }

        private static readonly Regex KeywordMarkup = new Regex(@"\[\[(.+?)\]\]");
        private const string FoundKeywordColor = "#B5651D";

        // =================================================================
        // 인스펙터 연결
        // =================================================================

        /// <summary>페이드를 제외한 응대 화면 전체. 닫혀 있으면 꺼 둔다.</summary>
        [Header("화면"), SerializeField] private GameObject content;
        /// <summary>화면 전환용 검은 덮개 (평소 투명).</summary>
        [SerializeField] private Image fadeOverlay;
        /// <summary>페이드 한 방향 시간(초).</summary>
        [SerializeField] private float fadeSeconds = 0.15f;
        /// <summary>
        /// 켜면 예전처럼 검은 화면을 거쳐 열리고 닫힌다.
        /// 끄면(기본) 손님 얼굴 줌인 화면 위로 응대 화면만 서서히 나타나고 사라진다.
        /// </summary>
        [SerializeField] private bool fadeThroughBlack = false;
        /// <summary>응대 화면이 서서히 나타나거나 사라지는 시간(초).</summary>
        [SerializeField] private float contentFadeSeconds = 0.25f;

        [Header("손님 정보"), SerializeField] private TMP_Text nameText;
        [SerializeField] private GameObject regularTag;
        [SerializeField] private TMP_Text destinationText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private TextLinkClick dialogueLinks;
        /// <summary>긴 대사의 다음 쪽 버튼. 넘길 쪽이 있을 때만 보인다.</summary>
        [SerializeField] private Button nextPageButton;
        /// <summary>요청 거절 이유 등 짧은 안내를 띄우는 대사창 줄.</summary>
        [SerializeField] private TMP_Text noticeText;
        /// <summary>초상화의 숨은 단서 자리 (데이터보다 많으면 남는 것은 숨김).</summary>
        [SerializeField] private ClueTooltip[] clueMarkers;

        [Header("위험 종류 (이름 · 아이콘 · 색)"), SerializeField] private RiskKind[] riskKinds;

        [Header("수첩 · 위험 쪽"), SerializeField] private RiskSlotView[] riskSlots;

        [Header("수첩 · 단서 사전 쪽"), SerializeField] private RectTransform lineTagList;
        [SerializeField] private RectTransform portraitTagList;
        /// <summary>아직 찾은 대사 단서가 없을 때 보이는 안내 글.</summary>
        [SerializeField] private GameObject lineTagEmpty;
        [SerializeField] private GameObject portraitTagEmpty;
        /// <summary>태그 원본 (꺼 둔 채로 두고 복사해서 쓴다).</summary>
        [SerializeField] private ClueTagView tagTemplate;

        [Header("수첩 · 도감 쪽"), SerializeField] private RectTransform raceList;
        [SerializeField] private RectTransform jobList;
        [SerializeField] private RectTransform floorList;
        /// <summary>각 목록 맨 끝의 '???' 줄 (아직 모르는 것이 더 있다는 표시).</summary>
        [SerializeField] private GameObject raceUnknown;
        [SerializeField] private GameObject jobUnknown;
        [SerializeField] private GameObject floorUnknown;
        /// <summary>도감 한 줄 원본 (꺼 둔 채로 두고 복사해서 쓴다).</summary>
        [SerializeField] private CodexRowView rowTemplate;

        [Header("수첩 · 펼치기"), SerializeField] private Button expandTab;
        [SerializeField] private TMP_Text expandLabel;
        /// <summary>수첩 쪽 넘기기 (위험 · 단서 사전 · 도감 · 손님 기록 · 층별 위험).</summary>
        [SerializeField] private NotebookPager pager;
        /// <summary>두 쪽을 다 펼친 수첩. 오른쪽 끝 기준(pivot x=1)으로 옆으로 미끄러진다.</summary>
        [SerializeField] private RectTransform notebookPanel;
        /// <summary>열 때 왼쪽으로 미끄러지는 거리(px). 닫힌 위치는 에디터에서 놓아 둔 자리를 그대로 쓴다.</summary>
        [SerializeField] private float notebookSlideDistance = 530f;
        [SerializeField] private float notebookSlideSeconds = 0.25f;

        [Header("버튼"), SerializeField] private Button followButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button sendHomeButton;
        [SerializeField] private GameObject sendHomeConfirm;
        [SerializeField] private Button confirmYesButton;
        [SerializeField] private Button confirmNoButton;

        // =================================================================
        // 상태
        // =================================================================

        private bool isTransitioning;
        private CanvasGroup contentGroup;   // 응대 화면 전체 투명도
        private bool isNotebookOpen;
        private Coroutine notebookSlide;
        private Vector2 notebookClosedPos; // 에디터에서 놓아 둔 수첩 자리 = 닫힌 자리

        private GuestDialogueData current;   // 지금 응대 중인 손님
        private NotebookRecord record;       // 수첩 기록 (손님이 바뀌어도 이어짐)
        private readonly List<GameObject> spawned = new List<GameObject>(); // 복사해서 만든 태그 · 줄 (다시 그릴 때 지움)

        public bool IsOpen { get; private set; }

        // =================================================================
        // 이벤트 (Presenter가 듣는다)
        // =================================================================

        public event Action FollowClicked;
        public event Action CloseClicked;
        public event Action SendHomeConfirmed;
        /// <summary>대사 키워드를 눌렀다. 값은 키워드 글자. (예전 방식 Presenter 호환용)</summary>
        public event Action<string> ClueClicked;
        /// <summary>새 단서를 찾아 수첩에 적었다. (나중에 저장 연결용)</summary>
        public event Action<FoundClue> ClueFound;

        /// <summary>[예전] 메모 변경. 메모 버튼이 없어져서 더는 알리지 않는다. (구독 코드 호환용)</summary>
        public event Action<int, int> MemoChanged { add { } remove { } }

        // =================================================================
        // 시작
        // =================================================================

        private void Awake()
        {
            // 버튼
            followButton.onClick.AddListener(() => { if (CanAcceptInput()) FollowClicked?.Invoke(); });
            closeButton.onClick.AddListener(() => { if (CanAcceptInput()) CloseClicked?.Invoke(); });
            sendHomeButton.onClick.AddListener(() => { if (CanAcceptInput()) sendHomeConfirm.SetActive(true); });
            confirmYesButton.onClick.AddListener(() =>
            {
                sendHomeConfirm.SetActive(false);
                if (CanAcceptInput()) SendHomeConfirmed?.Invoke();
            });
            confirmNoButton.onClick.AddListener(() => sendHomeConfirm.SetActive(false));
            expandTab.onClick.AddListener(() => { if (CanAcceptInput()) SetNotebookOpen(!isNotebookOpen, true); });
            nextPageButton.onClick.AddListener(() => { if (CanAcceptInput()) ShowDialoguePage(dialogueText.pageToDisplay + 1); });

            // 숨은 단서
            dialogueLinks.LinkClicked += OnKeywordClicked;
            for (int i = 0; i < clueMarkers.Length; i++)
            {
                int index = i; // 람다가 반복 변수 대신 고정된 번호를 쓰도록 복사
                clueMarkers[i].Clicked += () => OnPortraitClueClicked(index);
            }

            dialogueText.overflowMode = TextOverflowModes.Page;
            notebookClosedPos = notebookPanel.anchoredPosition;
            if (tagTemplate != null) tagTemplate.gameObject.SetActive(false);
            if (rowTemplate != null) rowTemplate.gameObject.SetActive(false);
            // 응대 화면 전체를 한 번에 투명하게 하려고 CanvasGroup을 쓴다 (없으면 붙인다)
            contentGroup = content.GetComponent<CanvasGroup>();
            if (contentGroup == null)
                contentGroup = content.AddComponent<CanvasGroup>();
            content.SetActive(false);
            SetFade(0f);
        }

        // =================================================================
        // 바깥에서 부르는 기능
        // =================================================================

        /// <summary>응대 화면을 연다. 이미 열려 있거나 전환 중이면 false (E 연타 무시).</summary>
        public bool Open(GuestDialogueData data)
        {
            if (IsOpen || isTransitioning || data == null)
                return false;
            if (fadeThroughBlack)
                StartCoroutine(Transition(() => { Bind(data); content.SetActive(true); IsOpen = true; }));
            else
                StartCoroutine(FadeContent(true, () => Bind(data)));
            return true;
        }

        /// <summary>응대 화면을 닫는다. 상태 변경은 하지 않는다.</summary>
        public void Close()
        {
            if (!IsOpen || isTransitioning)
                return;
            if (fadeThroughBlack)
                StartCoroutine(Transition(() => { content.SetActive(false); IsOpen = false; }));
            else
                StartCoroutine(FadeContent(false, null));
        }

        /// <summary>대사창에 짧은 안내를 띄운다 (예: "지금은 빈 자리가 없어요.").</summary>
        public void ShowNotice(string message)
        {
            noticeText.text = message;
            noticeText.gameObject.SetActive(!string.IsNullOrEmpty(message));
        }

        /// <summary>
        /// [예전 방식 Presenter 호환] 키워드의 뜻을 받아 수첩에 적는다.
        /// meaning이 위험 이름(독 · 열 …)이면 그 위험으로 적고, 아니면 (?)로 적는다.
        /// 이미 찾은 키워드면 수첩만 펼친다.
        /// </summary>
        public void ShowDictionaryEntry(string phrase, string meaning)
        {
            if (current == null || string.IsNullOrEmpty(phrase))
                return;

            string key = LineKey(phrase);
            if (!record.Has(current.guestId, key))
            {
                Discover(new FoundClue
                {
                    key = key,
                    tag = phrase,
                    risk = FindRiskKind(meaning).HasValue ? meaning : null,
                    source = ClueSource.Line,
                    recordTo = ClueRecordTarget.None,
                });
                return;
            }
            ShowNotebookFirstSpread();
        }

        // =================================================================
        // 화면 채우기
        // =================================================================

        private void Bind(GuestDialogueData data)
        {
            current = data;
            record = data.notebook ?? NotebookRecord.Session;

            // 손님 정보
            // 이름만 보여 준다. 종족 · 직업은 정답 힌트가 되므로 이름판에 쓰지 않는다
            // (단서를 찾으면 도감에만 적힌다).
            nameText.text = data.guestName;
            regularTag.SetActive(data.isRegular);
            destinationText.text = data.destinationLabel;
            RefreshDialogueText(keepPage: false);
            ShowNotice(null);

            // 초상화 숨은 단서 자리
            for (int i = 0; i < clueMarkers.Length; i++)
            {
                if (i < data.clues.Length)
                {
                    bool found = record.Has(data.guestId, PortraitKey(i));
                    clueMarkers[i].Show(data.clues[i].description, data.clues[i].position, found);
                }
                else
                {
                    clueMarkers[i].Hide();
                }
            }

            // 수첩
            RefreshNotebook();
            SetNotebookOpen(false, false);
            pager.ShowSpread(0, false); // 응대를 열 때는 항상 첫 면부터

            // 버튼
            sendHomeConfirm.SetActive(false);
            sendHomeButton.gameObject.SetActive(data.canSendHome);
        }

        // =================================================================
        // 숨은 단서 찾기
        // =================================================================

        // 대사의 [[ ]] 키워드를 눌렀다. (TMP 링크가 걸린 곳만 여기로 온다 = 틀린 곳은 무반응)
        private void OnKeywordClicked(string phrase)
        {
            if (!CanAcceptInput())
                return;

            var keyword = Array.Find(current.keywords ?? Array.Empty<ClueKeywordData>(), k => k != null && k.phrase == phrase);
            if (keyword != null)
            {
                Discover(new FoundClue
                {
                    key = LineKey(phrase),
                    tag = phrase,
                    risk = keyword.risk,
                    source = ClueSource.Line,
                    recordTo = keyword.recordTo,
                });
            }

            // 예전 방식 Presenter는 이걸 듣고 ShowDictionaryEntry로 뜻을 넘겨준다.
            ClueClicked?.Invoke(phrase);
        }

        // 초상화의 숨은 단서 자리를 눌렀다.
        private void OnPortraitClueClicked(int index)
        {
            if (!CanAcceptInput() || index >= current.clues.Length)
                return;

            var clue = current.clues[index];
            Discover(new FoundClue
            {
                key = PortraitKey(index),
                tag = string.IsNullOrEmpty(clue.tagName) ? clue.description : clue.tagName,
                risk = clue.risk,
                source = ClueSource.Portrait,
                recordTo = clue.recordTo,
            });
            clueMarkers[index].SetFound(true);
        }

        // 새 단서를 수첩에 적고, 화면을 다시 그리고, 수첩을 펼쳐 보여 준다.
        private void Discover(FoundClue clue)
        {
            string race = FirstNonEmpty(current.race, SplitRaceJob(current.raceJob, 0));
            string job = FirstNonEmpty(current.job, SplitRaceJob(current.raceJob, 1));

            if (!record.Add(current.guestId, clue, race, job, current.destinationLabel))
                return; // 이미 찾은 단서

            RefreshDialogueText(keepPage: true);
            RefreshNotebook();
            ShowNotebookFirstSpread();
            ClueFound?.Invoke(clue);
        }

        // =================================================================
        // 대사 그리기
        // =================================================================

        // [[키워드]] → 숨은 링크. 찾은 키워드만 밑줄 + 색.
        private void RefreshDialogueText(bool keepPage)
        {
            int page = dialogueText.pageToDisplay;
            dialogueText.text = ToLinkMarkup(current.line);
            ShowDialoguePage(keepPage ? page : 1);
        }

        private string ToLinkMarkup(string line)
        {
            if (string.IsNullOrEmpty(line))
                return string.Empty;

            return KeywordMarkup.Replace(line, m =>
            {
                string phrase = m.Groups[1].Value;
                bool found = record.Has(current.guestId, LineKey(phrase));
                return found
                    ? $"<link=\"{phrase}\"><u><color={FoundKeywordColor}>{phrase}</color></u></link>"
                    : $"<link=\"{phrase}\">{phrase}</link>"; // 겉으로는 보통 글자와 똑같다
            });
        }

        // 대사의 몇 번째 쪽(1부터)을 보여 주고, 뒤에 쪽이 더 있으면 [다음]을 띄운다.
        private void ShowDialoguePage(int page)
        {
            dialogueText.ForceMeshUpdate(); // 쪽 수를 바로 계산하게 한다
            int pageCount = Mathf.Max(1, dialogueText.textInfo.pageCount);
            dialogueText.pageToDisplay = Mathf.Clamp(page, 1, pageCount);
            nextPageButton.gameObject.SetActive(dialogueText.pageToDisplay < pageCount);
        }

        // =================================================================
        // 수첩 그리기
        // =================================================================

        // 기록을 보고 수첩 전체(위험 · 단서 사전 · 도감)를 다시 그린다.
        private void RefreshNotebook()
        {
            // 지난번에 만든 태그 · 줄을 지운다 (바로 꺼서 이번 프레임 배치에서도 빠지게)
            foreach (var go in spawned)
            {
                if (go == null) continue;
                go.SetActive(false);
                Destroy(go);
            }
            spawned.Clear();

            var clues = record.CluesOf(current.guestId);
            DrawRiskSlots(clues);
            DrawDictionary(clues);
            DrawCodex(raceList, raceUnknown, record.Races);
            DrawCodex(jobList, jobUnknown, record.Jobs);
            DrawCodex(floorList, floorUnknown, record.Floors);
        }

        // 위험 쪽: 찾은 위험을 위 줄부터 적는다.
        //          같은 위험의 단서를 또 찾으면 새 줄 대신 그 줄의 작은 칸이 하나 더 찬다.
        private void DrawRiskSlots(IReadOnlyList<FoundClue> clues)
        {
            var risks = clues
                .Where(c => !string.IsNullOrEmpty(c.risk))
                .GroupBy(c => c.risk)          // 찾은 순서를 지키며 위험별로 묶는다
                .ToList();

            for (int i = 0; i < riskSlots.Length; i++)
            {
                if (i >= risks.Count)
                {
                    riskSlots[i].Clear();
                    continue;
                }
                string riskName = risks[i].Key;
                int count = risks[i].Count();   // 이 위험의 단서를 몇 개 찾았는지 → 칸 수 (최대 5)
                var kind = FindRiskKind(riskName);
                riskSlots[i].Show(riskName, kind?.icon, kind?.color ?? Color.gray, count);
            }
        }

        // 단서 사전 쪽: '대사 단서' / '초상화 단서' 줄에 태그를 쌓는다.
        private void DrawDictionary(IReadOnlyList<FoundClue> clues)
        {
            int lineCount = 0, portraitCount = 0;
            foreach (var clue in clues)
            {
                bool fromLine = clue.source == ClueSource.Line;
                SpawnTag(fromLine ? lineTagList : portraitTagList, clue);
                if (fromLine) lineCount++; else portraitCount++;
            }
            lineTagEmpty.SetActive(lineCount == 0);
            portraitTagEmpty.SetActive(portraitCount == 0);
            lineTagEmpty.transform.SetAsLastSibling();
            portraitTagEmpty.transform.SetAsLastSibling();
        }

        // 도감 쪽: 이름 줄마다 태그를 붙이고, 맨 끝에 '???' 줄을 둔다.
        private void DrawCodex(RectTransform list, GameObject unknown, IReadOnlyList<CodexEntry> entries)
        {
            foreach (var entry in entries)
            {
                var row = Instantiate(rowTemplate, list);
                row.gameObject.SetActive(true);
                row.SetName(entry.Name);
                spawned.Add(row.gameObject);
                foreach (var clue in entry.Clues)
                    SpawnTag(row.TagList, clue);
            }
            unknown.SetActive(true);
            unknown.transform.SetAsLastSibling();
        }

        private void SpawnTag(RectTransform parent, FoundClue clue)
        {
            var tagView = Instantiate(tagTemplate, parent);
            tagView.gameObject.SetActive(true);
            var kind = FindRiskKind(clue.risk);
            Color color = kind.HasValue ? Color.Lerp(kind.Value.color, Color.black, 0.3f) : new Color(0.45f, 0.35f, 0.25f);
            tagView.Set(clue.tag, clue.risk, color);
            spawned.Add(tagView.gameObject);
        }

        // =================================================================
        // 수첩 펼치기
        // =================================================================

        // 단서를 찾으면 첫 면(위험 + 단서 사전)을 펼쳐 보여 준다.
        private void ShowNotebookFirstSpread()
        {
            pager.ShowSpread(0, true);
            SetNotebookOpen(true, true);
        }

        // 수첩을 옆으로 밀어 열거나 닫는다. animate가 false면 바로 그 자리로 옮긴다(화면 열 때 초기화용).
        private void SetNotebookOpen(bool open, bool animate)
        {
            if (open == isNotebookOpen && animate)
                return; // 이미 그 상태
            isNotebookOpen = open;
            expandLabel.text = open ? "접기" : "펼치기";
            if (notebookSlide != null)
                StopCoroutine(notebookSlide);
            notebookSlide = null;

            Vector2 target = open ? notebookClosedPos + Vector2.left * notebookSlideDistance : notebookClosedPos;
            if (animate && isActiveAndEnabled)
                notebookSlide = StartCoroutine(SlideNotebook(target));
            else
                notebookPanel.anchoredPosition = target;
        }

        private IEnumerator SlideNotebook(Vector2 target)
        {
            Vector2 start = notebookPanel.anchoredPosition;
            for (float t = 0f; t < notebookSlideSeconds; t += Time.unscaledDeltaTime)
            {
                // 처음엔 빠르고 끝에서 부드럽게 멈추게 (SmoothStep)
                notebookPanel.anchoredPosition = Vector2.Lerp(start, target, Mathf.SmoothStep(0f, 1f, t / notebookSlideSeconds));
                yield return null;
            }
            notebookPanel.anchoredPosition = target;
            notebookSlide = null;
        }

        // =================================================================
        // 화면 전환 (페이드)
        // =================================================================

        private bool CanAcceptInput() => IsOpen && !isTransitioning;

        // 응대 화면만 서서히 나타난다 (show) / 사라진다 (!show). 뒤의 줌인 화면은 그대로 보인다.
        // 전환 중에는 FadeOverlay가 투명한 채로 클릭만 막는다.
        // (FirstGuestDialogue는 FadeOverlay의 클릭 막기가 풀리는 것으로 '닫기 끝'을 안다)
        private IEnumerator FadeContent(bool show, Action beforeShow)
        {
            isTransitioning = true;
            fadeOverlay.raycastTarget = true;
            SetFade(0f);

            if (show)
            {
                beforeShow?.Invoke();
                contentGroup.alpha = 0f;
                content.SetActive(true);
                IsOpen = true;
            }

            float from = show ? 0f : 1f, to = show ? 1f : 0f;
            for (float t = 0f; t < contentFadeSeconds; t += Time.unscaledDeltaTime)
            {
                contentGroup.alpha = Mathf.Lerp(from, to, t / contentFadeSeconds);
                yield return null;
            }
            contentGroup.alpha = to;

            if (!show)
            {
                content.SetActive(false);
                contentGroup.alpha = 1f;
                IsOpen = false;
            }
            fadeOverlay.raycastTarget = false;
            isTransitioning = false;
        }

        // 검은 화면을 거쳐 바뀐다 (예전 방식). fadeThroughBlack을 켜면 쓴다.
        private IEnumerator Transition(Action swap)
        {
            isTransitioning = true;
            fadeOverlay.raycastTarget = true; // 전환 중 클릭 막기
            yield return Fade(0f, 1f);
            swap();
            yield return Fade(1f, 0f);
            fadeOverlay.raycastTarget = false;
            isTransitioning = false;
        }

        private IEnumerator Fade(float from, float to)
        {
            // 일시 정지(timeScale 0)와 무관하게 연출이 끝나도록 실제 시간을 쓴다.
            for (float t = 0f; t < fadeSeconds; t += Time.unscaledDeltaTime)
            {
                SetFade(Mathf.Lerp(from, to, t / fadeSeconds));
                yield return null;
            }
            SetFade(to);
        }

        private void SetFade(float alpha)
        {
            var c = fadeOverlay.color;
            c.a = alpha;
            fadeOverlay.color = c;
        }

        // =================================================================
        // 작은 도우미
        // =================================================================

        private static string LineKey(string phrase) => "line:" + phrase;
        private static string PortraitKey(int index) => "portrait:" + index;

        // 위험 이름(독 · 열 …)으로 아이콘과 색을 찾는다. 없으면 null.
        private RiskKind? FindRiskKind(string riskName)
        {
            if (string.IsNullOrEmpty(riskName) || riskKinds == null)
                return null;
            foreach (var kind in riskKinds)
                if (kind.name == riskName)
                    return kind;
            return null;
        }

        // "인간 · 마법사" → 0번째 "인간", 1번째 "마법사"
        private static string SplitRaceJob(string raceJob, int part)
        {
            if (string.IsNullOrEmpty(raceJob))
                return null;
            var parts = raceJob.Split('·');
            return part < parts.Length ? parts[part].Trim() : null;
        }

        private static string FirstNonEmpty(string a, string b) => string.IsNullOrWhiteSpace(a) ? b : a;
    }
}
