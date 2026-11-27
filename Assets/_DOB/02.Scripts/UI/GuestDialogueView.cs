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
//               (시안 20 ①) 새 손님 · 다음 쪽 대사는 한 글자씩 나온다. 클릭하면 바로 다 보인다 (키워드를 누르면 다 보이면서 찾음)
//               맞는 키워드를 누르면 → 그 말에 밑줄 + 수첩에 기록. 틀린 곳은 무반응
// [초상화]      데이터에 기본 초상이 있으면 그 그림을, 없으면 프리팹의 「그림 대기」 상자를 보여 준다 (2026-11-27 추가)
//               컨디션 레이어는 기본 초상과 같은 크기로 그 위에 겹친다. 숨은 단서 표시는 레이어보다 위에 남는다
//               숨은 단서 자리를 누르면 → 동그라미 표시 + 수첩에 기록. 틀린 곳은 무반응
// [수첩에 기록] · 위험 쪽: 빈 줄 6개 중 위에서부터 위험 이름이 적힘
//                 같은 위험을 또 찾으면 그 줄의 작은 칸(최대 5칸)이 하나씩 참
//               · 단서 사전: '대사 단서' / '초상화 단서' 줄에 [단서](위험) 태그
//               · 도감: 단서가 가리키는 종족 · 직업 · 층 줄에 태그 (처음엔 ???)
//               기록하면 수첩을 첫 면(위험 + 단서 사전)으로 펼쳐 보여 준다
//               (시안 11 ①) 누른 자리에서 태그 카드가 떠올라 곡선으로 단서 사전 자리까지 날아가 '착' 붙고,
//               같은 위험 줄이 한 번 톡 커진다. 날아가는 동안 진짜 태그는 투명하게 기다린다
//               (시안 15 A안) 같은 원인을 가리키는 다른 표현은 첫 표현 아래에 「같은 원인」으로 묶고, 위험 줄은 톡 커지지 않는다
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
        /// <summary>손님 초상 그림 칸. 그림이 없으면 프리팹의 처음 모양(회색 상자)으로 둔다.</summary>
        [SerializeField] private Image portraitImage;
        /// <summary>그림이 없을 때 보이는 「그림 대기」 안내.</summary>
        [SerializeField] private GameObject portraitPlaceholder;

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

        [Header("단서 찾기 손맛 (시안 11 ①)")]
        [SerializeField] private float tagFlySeconds = 0.35f;
        /// <summary>날아갈 때 곡선이 위로 부푸는 정도 (거리 대비 비율).</summary>
        [SerializeField] private float tagFlyArc = 0.3f;
        [SerializeField] private float tagLandSeconds = 0.25f;

        [Header("대사 타이핑 (시안 20 ①)")]
        /// <summary>대사가 나오는 빠르기 (초당 글자 수).</summary>
        [SerializeField] private float typeCharsPerSecond = 30f;

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
        private Sprite emptyPortraitSprite; // 그림이 없을 때 되돌릴 프리팹의 처음 초상 칸 모양
        private Color emptyPortraitColor;
        private bool emptyPortraitPreserve;
        private Image.Type emptyPortraitType;
        private readonly List<Image> portraitLayers = new List<Image>(); // 기본 초상 위에 겹친 컨디션 레이어 (재사용)

        private GuestDialogueData current;   // 지금 응대 중인 손님
        private NotebookRecord record;       // 수첩 기록 (손님이 바뀌어도 이어짐)
        private readonly List<GameObject> spawned = new List<GameObject>(); // 복사해서 만든 태그 · 줄 (다시 그릴 때 지움)
        private string flyKey, flyRisk;      // 방금 찾은 단서 (다시 그릴 때 그 태그 · 위험 줄을 기억)
        private ClueTagView flyTag;
        private RiskSlotView flySlot;
        private bool typing;                // 대사가 아직 써지는 중
        private float typed;
        private int typeFrom, typeTo;

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
            nextPageButton.onClick.AddListener(() => { if (CanAcceptInput()) { ShowDialoguePage(dialogueText.pageToDisplay + 1); StartTyping(); } });

            // 숨은 단서
            dialogueLinks.LinkClicked += OnKeywordClicked;
            for (int i = 0; i < clueMarkers.Length; i++)
                BindMarker(i);

            dialogueText.overflowMode = TextOverflowModes.Page;
            notebookClosedPos = notebookPanel.anchoredPosition;
            if (portraitImage != null)
            {
                emptyPortraitSprite = portraitImage.sprite;
                emptyPortraitColor = portraitImage.color;
                emptyPortraitPreserve = portraitImage.preserveAspect;
                emptyPortraitType = portraitImage.type;
            }
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
            ShowPortrait(data.portrait, data.portraitLayers);
            RefreshDialogueText(keepPage: false);
            ShowNotice(null);

            // 동시에 드러날 외형 단서가 프리팹의 기본 자리 수를 넘으면 같은 입력 영역을 추가한다.
            EnsureMarkerCount(data.clues.Length);
            for (int i = 0; i < clueMarkers.Length; i++)
            {
                if (i < data.clues.Length)
                {
                    bool found = record.Has(data.guestId, MarkerKey(i));
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

        // 화면을 다시 열 때 이전 손님 그림이 남지 않도록 매번 그림 또는 처음 모양으로 덮어쓴다.
        private void ShowPortrait(Sprite portrait, Sprite[] layers)
        {
            if (portraitImage == null)
                return;
            bool hasPortrait = portrait != null;
            ShowPortraitLayers(hasPortrait ? layers : null);
            portraitImage.sprite = hasPortrait ? portrait : emptyPortraitSprite;
            portraitImage.color = hasPortrait ? Color.white : emptyPortraitColor;
            // 그림 비율이 칸과 달라도 얼굴이 늘어나지 않게 한다. 비율 유지는 Simple 방식에서만 적용된다
            // (빈 상자는 Sliced라 그대로 두면 그림이 칸에 맞춰 늘어난다).
            portraitImage.type = hasPortrait ? Image.Type.Simple : emptyPortraitType;
            portraitImage.preserveAspect = hasPortrait || emptyPortraitPreserve;
            if (portraitPlaceholder != null)
                portraitPlaceholder.SetActive(!hasPortrait);
        }

        // 레이어는 기본 초상과 같은 크기 · 같은 비율 유지로 두어야 같은 자리에 정확히 겹친다.
        // 초상 칸 자식의 맨 앞에 두어 기본 초상 위, 숨은 단서 표시 아래에 그려지게 한다.
        private void ShowPortraitLayers(Sprite[] layers)
        {
            int count = layers?.Length ?? 0;
            for (int i = 0; i < count; i++)
            {
                if (i == portraitLayers.Count)
                {
                    var go = new GameObject("PortraitLayer", typeof(RectTransform), typeof(Image));
                    var rect = (RectTransform)go.transform;
                    rect.SetParent(portraitImage.transform, false);
                    rect.anchorMin = Vector2.zero;
                    rect.anchorMax = Vector2.one;
                    rect.offsetMin = rect.offsetMax = Vector2.zero;
                    var image = go.GetComponent<Image>();
                    image.preserveAspect = true;
                    image.raycastTarget = false; // 숨은 단서 클릭을 막지 않는다
                    portraitLayers.Add(image);
                }
                var layer = portraitLayers[i];
                layer.transform.SetSiblingIndex(i);
                layer.sprite = layers[i];
                layer.gameObject.SetActive(layers[i] != null);
            }
            for (int i = count; i < portraitLayers.Count; i++)
                portraitLayers[i].gameObject.SetActive(false);
        }

        private void BindMarker(int index) => clueMarkers[index].Clicked += () => OnPortraitClueClicked(index);

        private void EnsureMarkerCount(int required)
        {
            int previous = clueMarkers.Length;
            if (previous == 0 || previous >= required) return;
            var template = clueMarkers[0];
            Array.Resize(ref clueMarkers, required);
            for (int i = previous; i < required; i++)
            {
                clueMarkers[i] = Instantiate(template, template.transform.parent);
                BindMarker(i);
            }
        }

        private ClueKeywordData FindKeyword(string phrase) =>
            Array.Find(current.keywords ?? Array.Empty<ClueKeywordData>(), k => k != null && k.phrase == phrase);

        private string MarkerKey(int index) => FirstNonEmpty(current.clues[index].key, PortraitKey(index));

        // 대사의 [[ ]] 키워드를 눌렀다. (TMP 링크가 걸린 곳만 여기로 온다 = 틀린 곳은 무반응)
        private void OnKeywordClicked(string phrase)
        {
            if (!CanAcceptInput())
                return;
            FinishTyping(); // 써지는 중에 키워드를 누르면 대사를 다 보이고, 그 키워드도 찾은 것으로 친다

            var keyword = FindKeyword(phrase);
            if (keyword != null)
            {
                Discover(new FoundClue
                {
                    key = FirstNonEmpty(keyword.key, LineKey(phrase)),
                    cause = keyword.cause,
                    tag = phrase,
                    risk = keyword.risk,
                    delta = keyword.delta,
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
                key = MarkerKey(index),
                cause = clue.cause,
                tag = string.IsNullOrEmpty(clue.tagName) ? clue.description : clue.tagName,
                risk = clue.risk,
                delta = clue.delta,
                source = ClueSource.Portrait,
                recordTo = clue.recordTo,
            });
            clueMarkers[index].SetFound(true);
            clueMarkers[index].PlayFound();
        }

        // 새 단서를 수첩에 적고, 화면을 다시 그리고, 수첩을 펼쳐 보여 준다.
        private void Discover(FoundClue clue)
        {
            string race = FirstNonEmpty(current.race, SplitRaceJob(current.raceJob, 0));
            string job = FirstNonEmpty(current.job, SplitRaceJob(current.raceJob, 1));

            // 같은 원인을 이미 찾았으면 위험 칸은 그대로다 → 날아간 뒤 위험 줄을 톡 키우지 않는다
            bool sameCause = !string.IsNullOrEmpty(clue.cause) && record.CluesOf(current.guestId).Any(c => c.cause == clue.cause);
            current.discover?.Invoke(clue.key);
            if (!record.Add(current.guestId, clue, race, job, current.destinationLabel))
                return; // 이미 찾은 단서

            RefreshDialogueText(keepPage: true);
            flyKey = clue.key; flyRisk = sameCause ? null : clue.risk;
            RefreshNotebook();
            flyKey = flyRisk = null;
            // 초상화 단서는 수첩을 저절로 펼치지 않는다 (초상화를 계속 살필 수 있게). 태그는 「펼치기」 탭으로 날아간다
            if (clue.source == ClueSource.Portrait) pager.ShowSpread(0, true);
            else ShowNotebookFirstSpread();
            if (flyTag && isActiveAndEnabled) StartCoroutine(FlyTagIn(flyTag, flySlot, PointerPosition()));
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
            if (!keepPage) StartTyping(); // 새 손님 대사만 써 나간다 (단서를 찾아 다시 그릴 때는 그대로)
        }

        // =================================================================
        // 대사 타이핑 (시안 20 ①)
        // =================================================================

        // 의사코드: 지금 쪽의 첫 글자 ~ 끝 글자를 정하고, 보이는 글자 수를 첫 글자부터 초당 typeCharsPerSecond씩 늘린다
        //   어디든 클릭하면 바로 다 보인다. 써지는 중에 키워드를 누르면 다 보이면서 그 키워드도 찾는다 (OnKeywordClicked)
        // 화면이 막 켜지는 중이면 글자 배치가 아직 없으므로, 쪽 범위는 실제로 보일 때(Update) 잰다
        private void StartTyping()
        {
            typed = 0f;
            typing = !string.IsNullOrEmpty(dialogueText.text);
            typeTo = -1; // 아직 안 잼
            dialogueText.maxVisibleCharacters = typing ? 0 : 99999;
        }

        // 지금 쪽의 첫 글자 ~ 끝 글자. 배치가 아직 없으면 false
        private bool MeasurePage()
        {
            if (!dialogueText.gameObject.activeInHierarchy) return false;
            dialogueText.ForceMeshUpdate();
            var info = dialogueText.textInfo;
            if (info.characterCount == 0) return false;
            int page = Mathf.Clamp(dialogueText.pageToDisplay - 1, 0, Mathf.Max(0, info.pageCount - 1));
            typeFrom = info.pageCount > 0 ? info.pageInfo[page].firstCharacterIndex : 0;
            typeTo = info.pageCount > 0 ? info.pageInfo[page].lastCharacterIndex + 1 : info.characterCount;
            return true;
        }

        private void FinishTyping()
        {
            if (!typing) return;
            typing = false;
            dialogueText.maxVisibleCharacters = 99999;
        }

        private void Update()
        {
            if (!typing) return;
            var mouse = UnityEngine.InputSystem.Mouse.current;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame) { FinishTyping(); return; }
            if (typeTo < 0 && !MeasurePage()) return;
            typed += Mathf.Min(Time.unscaledDeltaTime, 0.05f) * typeCharsPerSecond;
            int shown = typeFrom + Mathf.FloorToInt(typed);
            if (shown >= typeTo) FinishTyping();
            else dialogueText.maxVisibleCharacters = shown;
        }

        private string ToLinkMarkup(string line)
        {
            if (string.IsNullOrEmpty(line))
                return string.Empty;

            return KeywordMarkup.Replace(line, m =>
            {
                string phrase = m.Groups[1].Value;
                bool found = record.Has(current.guestId, FirstNonEmpty(FindKeyword(phrase)?.key, LineKey(phrase)));
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
            flyTag = null; flySlot = null;

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
                // 백엔드 값이 없으면 원인별로 한 번씩 센다 (같은 원인의 다른 표현은 한 칸)
                int count = current.notebookRisk != null ? current.notebookRisk(riskName).GetValueOrDefault()
                    : risks[i].Select(c => string.IsNullOrEmpty(c.cause) ? c.key : c.cause).Distinct().Count();
                var kind = FindRiskKind(riskName);
                string label = current.notebookRisk == null ? riskName : $"{riskName} {count}";
                riskSlots[i].Show(label, kind?.icon, kind?.color ?? Color.gray, count);
                if (riskName == flyRisk) flySlot = riskSlots[i];
            }
        }

        // 단서 사전 쪽: '대사 단서' / '초상화 단서' 줄에 태그를 쌓는다.
        private void DrawDictionary(IReadOnlyList<FoundClue> clues)
        {
            var line = clues.Where(c => c.source == ClueSource.Line).ToList();
            var portrait = clues.Where(c => c.source != ClueSource.Line).ToList();
            foreach (var (clue, same) in GroupByCause(line, clues))
            {
                var tag = SpawnTag(lineTagList, clue, same);
                if (clue.key == flyKey) flyTag = tag;
            }
            foreach (var (clue, same) in GroupByCause(portrait, clues))
            {
                var tag = SpawnTag(portraitTagList, clue, same);
                if (clue.key == flyKey) flyTag = tag;
            }
            lineTagEmpty.SetActive(line.Count == 0);
            portraitTagEmpty.SetActive(portrait.Count == 0);
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
                foreach (var (clue, same) in GroupByCause(entry.Clues, entry.Clues))
                    SpawnTag(row.TagList, clue, same);
            }
            unknown.SetActive(true);
            unknown.transform.SetAsLastSibling();
        }

        // sameCause면 앞서 찾은 같은 원인 단서 아래에 매다는 작은 태그로 그린다 (위험 수치 없음)
        private ClueTagView SpawnTag(RectTransform parent, FoundClue clue, bool sameCause = false)
        {
            var tagView = Instantiate(tagTemplate, parent);
            tagView.gameObject.SetActive(true);
            var kind = FindRiskKind(clue.risk);
            Color color = kind.HasValue ? Color.Lerp(kind.Value.color, Color.black, 0.3f) : new Color(0.45f, 0.35f, 0.25f);
            if (sameCause) tagView.SetSameCause(clue.tag, color);
            else tagView.Set(clue.tag, clue.RiskLabel, color, kind?.icon);
            spawned.Add(tagView.gameObject);
            return tagView;
        }

        // 의사코드 (시안 15 A안): 원인마다 처음 찾은 단서를 정하고, section을 찾은 순서대로 돌며
        //   · 처음 찾은 단서 → 그대로 내보내고, 같은 section의 같은 원인 단서를 바로 뒤에 「같은 원인」으로 붙인다
        //   · 같은 원인인데 처음 것이 다른 section(대사 ↔ 초상화)에 있으면 제자리에서 「같은 원인」으로 내보낸다
        //   · 원인ID가 없으면 묶지 않는다
        private static List<(FoundClue clue, bool same)> GroupByCause(IReadOnlyList<FoundClue> section, IEnumerable<FoundClue> all)
        {
            var first = new Dictionary<string, string>();
            foreach (var c in all)
                if (!string.IsNullOrEmpty(c.cause) && !first.ContainsKey(c.cause)) first[c.cause] = c.key;
            bool Same(FoundClue c) => !string.IsNullOrEmpty(c.cause) && first[c.cause] != c.key;

            var result = new List<(FoundClue, bool)>();
            var done = new HashSet<FoundClue>();
            foreach (var c in section)
            {
                if (done.Contains(c)) continue;
                if (Same(c) && section.Any(p => p.key == first[c.cause])) continue; // 처음 것이 이 section에서 끌어온다
                done.Add(c);
                result.Add((c, Same(c)));
                if (Same(c) || string.IsNullOrEmpty(c.cause)) continue;
                foreach (var d in section)
                    if (!done.Contains(d) && d.cause == c.cause) { done.Add(d); result.Add((d, true)); }
            }
            return result;
        }

        // =================================================================
        // 단서 찾기 손맛 (시안 11 ①)
        // =================================================================

        // 마우스 자리 (마우스가 없으면 null → 대사창 가운데에서 출발)
        private static Vector2? PointerPosition()
        {
            var mouse = UnityEngine.InputSystem.Mouse.current;
            return mouse != null ? mouse.position.ReadValue() : (Vector2?)null;
        }

        // 의사코드: 진짜 태그를 투명하게 → 한 프레임 기다려 배치가 끝난 자리를 얻음 → 맨 위 캔버스에 복사본을 띄워
        //           누른 자리에서 진짜 태그 자리까지 곡선으로 날림 (수첩이 미끄러져도 매 프레임 따라감)
        //           → 복사본을 지우고 진짜 태그를 보이며 톡 눌렸다 커짐, 같은 위험 줄도 함께 톡
        //           도중에 다시 그려져 태그가 사라지면 복사본만 지우고 끝낸다
        private IEnumerator FlyTagIn(ClueTagView target, RiskSlotView slot, Vector2? screenFrom)
        {
            var group = target.GetComponent<CanvasGroup>();
            if (!group) group = target.gameObject.AddComponent<CanvasGroup>();
            group.alpha = 0f;
            yield return null;
            if (!target) yield break;

            var canvas = GetComponentInParent<Canvas>().rootCanvas;
            var tr = (RectTransform)target.transform;
            var ghost = Instantiate(target.gameObject, canvas.transform);
            var gr = (RectTransform)ghost.transform;
            gr.anchorMin = gr.anchorMax = new Vector2(0.5f, 0.5f);
            gr.pivot = tr.pivot;
            gr.sizeDelta = tr.rect.size;
            gr.SetAsLastSibling();
            var ghostGroup = ghost.GetComponent<CanvasGroup>();
            ghostGroup.alpha = 1f;
            ghostGroup.blocksRaycasts = false;

            Vector3 from = dialogueText.rectTransform.position;
            var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            if (screenFrom.HasValue)
                RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)canvas.transform, screenFrom.Value, cam, out from);

            for (float t = 0f; t < tagFlySeconds && target; t += Mathf.Min(Time.unscaledDeltaTime, 0.05f))
            {
                float k = t / tagFlySeconds;
                float e = k * k * (3f - 2f * k);
                Vector3 to = isNotebookOpen ? tr.position : expandTab.transform.position; // 접혀 있으면 탭으로
                Vector3 mid = (from + to) * 0.5f + canvas.transform.up * (Vector3.Distance(from, to) * tagFlyArc);
                gr.position = Vector3.Lerp(Vector3.Lerp(from, mid, e), Vector3.Lerp(mid, to, e), e);
                gr.localScale = Vector3.one * Mathf.Lerp(1.3f, 1f, e);
                gr.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(-6f, 0f, e));
                yield return null;
            }
            Destroy(ghost);
            if (!target) yield break;
            group.alpha = 1f;

            // 착 붙기: 살짝 눌렸다(0.9) 커졌다(1.12) 제자리
            Transform land = isNotebookOpen ? tr : expandTab.transform; // 접혀 있으면 탭이 톡
            var st = slot && isNotebookOpen ? slot.transform : null;
            for (float t = 0f; t < tagLandSeconds && target; t += Mathf.Min(Time.unscaledDeltaTime, 0.05f))
            {
                float k = t / tagLandSeconds;
                float s = k < 0.3f ? Mathf.Lerp(1f, 0.9f, k / 0.3f) : 1f + 0.12f * Mathf.Sin(Mathf.PI * (k - 0.3f) / 0.7f);
                land.localScale = Vector3.one * s;
                if (st) st.localScale = Vector3.one * (1f + 0.08f * Mathf.Sin(Mathf.PI * k));
                yield return null;
            }
            if (land) land.localScale = Vector3.one;
            if (st) st.localScale = Vector3.one;
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
            pager.HintsEnabled = open; // 펼쳐져 보일 때만 「넘기기」 안내를 가끔 띄운다
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
