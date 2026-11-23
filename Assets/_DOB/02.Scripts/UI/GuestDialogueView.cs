// =====================================================================
// GuestDialogueView — 손님 응대 화면 표시와 입력
// 역할   : 손님 표시 데이터를 받아 초상·대사·밑줄 단서·관찰 표식·위험 6칸·메모를 그리고,
//          버튼과 단서 클릭을 이벤트로 알린다. 자리·판정·저장은 모른다.
// 호출자 : GuestDialoguePresenter (Open, Close, ShowNotice, ShowDictionaryEntry)
// 호출   : 없음 (이벤트로만 알림)
// 상태   : 열림 여부와 전환 중 여부, 수첩 펼침 여부만 가진다.
//
// 의사코드 (구현 전 작성 · 2026-11-23 홍준우와 합의)
// [열릴 때]   전환 중이거나 이미 열려 있으면 무시 → 페이드 아웃 → 데이터 표시 → 페이드 인
//             이름([단골]), 종족·직업, 목적지, 대사([[ ]]→밑줄 링크), 단서 표식(생김새만),
//             위험 6칸(기록값 칸 / 모름 '?' + 메모)
// [수첩]      두 쪽을 다 펼친 책. 에디터에서 놓아 둔 자리가 닫힌 자리(오른쪽 쪽은 화면 밖, 왼쪽 쪽 위험만 보임).
//             탭을 누르면 옆으로 미끄러져 열리고, 다시 누르면 닫힌다. 밑줄 단서를 누르면 열린다
//             쪽 넘기기는 NotebookPager가 맡는다 (모서리를 눌러 앞 장·뒷장)
// [긴 대사]   대화창 크기는 고정. 넘치면 쪽으로 나누고 [다음]으로 넘긴다. 마지막 쪽이면 [다음]을 숨긴다
// [밑줄 클릭] 단서 문구를 알린다 (Presenter가 사전 항목을 넘겨주면 수첩을 더 펼쳐 보여 줌)
// [메모]      칸마다 비움 → 있음 → 없음 순환, 바뀐 값을 알린다 (판정 영향 없음)
// [따라오세요] 요청만 알린다. 거절이면 Presenter가 ShowNotice로 대사창에 이유를 띄움
// [닫기]      상태를 바꾸지 않고 닫기를 알린다
// [돌려보내기] 확인창 → 확인 시 알린다. 튜토리얼 등에서는 버튼을 숨길 수 있다
// [닫힐 때]   페이드 아웃 → 화면 숨김 → 페이드 인
// =====================================================================
using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>손님 응대 화면. 표시와 입력 알림만 하고 게임 상태를 바꾸지 않는다.</summary>
    public sealed class GuestDialogueView : MonoBehaviour
    {
        private static readonly Regex ClueMarkup = new Regex(@"\[\[(.+?)\]\]");

        /// <summary>페이드를 제외한 응대 화면 전체. 닫혀 있으면 꺼 둔다.</summary>
        [Header("화면"), SerializeField] private GameObject content;
        /// <summary>화면 전환용 검은 덮개 (평소 투명).</summary>
        [SerializeField] private Image fadeOverlay;
        /// <summary>페이드 한 방향 시간(초).</summary>
        [SerializeField] private float fadeSeconds = 0.15f;

        [Header("손님 정보"), SerializeField] private TMP_Text nameText;
        [SerializeField] private GameObject regularTag;
        [SerializeField] private TMP_Text destinationText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private TextLinkClick dialogueLinks;
        /// <summary>긴 대사의 다음 쪽 버튼. 넘길 쪽이 있을 때만 보인다.</summary>
        [SerializeField] private Button nextPageButton;
        /// <summary>요청 거절 이유 등 짧은 안내를 띄우는 대사창 줄.</summary>
        [SerializeField] private TMP_Text noticeText;
        /// <summary>초상 위 관찰 단서 표식 (데이터보다 많으면 남는 것은 숨김).</summary>
        [SerializeField] private ClueTooltip[] clueMarkers;

        [Header("수첩"), SerializeField] private RiskRowView[] riskRows;
        [SerializeField] private Button expandTab;
        [SerializeField] private TMP_Text expandLabel;
        /// <summary>단서 사전 쪽 본문 (첫 면 오른쪽 쪽).</summary>
        [SerializeField] private TMP_Text extraBody;
        /// <summary>수첩 쪽 넘기기 (위험·단서 사전·도감·손님 기록·층별 위험).</summary>
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

        private bool isTransitioning;
        private bool isNotebookOpen;
        private Coroutine notebookSlide;
        private Vector2 notebookClosedPos; // 에디터에서 놓아 둔 수첩 자리 = 닫힌 자리

        public bool IsOpen { get; private set; }

        public event Action FollowClicked;
        public event Action CloseClicked;
        public event Action SendHomeConfirmed;
        /// <summary>밑줄 단서 클릭. 값은 단서 문구.</summary>
        public event Action<string> ClueClicked;
        /// <summary>메모 변경. (축 번호 0~5, 메모 값 0 비움 · 1 있음 · 2 없음)</summary>
        public event Action<int, int> MemoChanged;

        private void Awake()
        {
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
            dialogueLinks.LinkClicked += phrase => { if (CanAcceptInput()) ClueClicked?.Invoke(phrase); };
            nextPageButton.onClick.AddListener(() => { if (CanAcceptInput()) ShowDialoguePage(dialogueText.pageToDisplay + 1); });
            dialogueText.overflowMode = TextOverflowModes.Page;

            for (int i = 0; i < riskRows.Length; i++)
            {
                int axis = i; // 람다가 반복 변수 대신 고정된 축 번호를 쓰도록 복사
                riskRows[i].MemoChanged += memo => MemoChanged?.Invoke(axis, memo);
            }

            notebookClosedPos = notebookPanel.anchoredPosition;
            content.SetActive(false);
            SetFade(0f);
        }

        /// <summary>응대 화면을 연다. 이미 열려 있거나 전환 중이면 false (E 연타 무시).</summary>
        public bool Open(GuestDialogueData data)
        {
            if (IsOpen || isTransitioning || data == null)
                return false;
            StartCoroutine(Transition(() => { Bind(data); content.SetActive(true); IsOpen = true; }));
            return true;
        }

        /// <summary>응대 화면을 닫는다. 상태 변경은 하지 않는다.</summary>
        public void Close()
        {
            if (!IsOpen || isTransitioning)
                return;
            StartCoroutine(Transition(() => { content.SetActive(false); IsOpen = false; }));
        }

        /// <summary>대사창에 짧은 안내를 띄운다 (예: "지금은 빈 자리가 없어요.").</summary>
        public void ShowNotice(string message)
        {
            noticeText.text = message;
            noticeText.gameObject.SetActive(!string.IsNullOrEmpty(message));
        }

        /// <summary>수첩을 더 펼쳐 단서 사전 항목을 보여 준다. 뜻을 모르면 '?'.</summary>
        public void ShowDictionaryEntry(string phrase, string meaning)
        {
            extraBody.text = $"{phrase}\n→ {(string.IsNullOrEmpty(meaning) ? "?" : meaning)}";
            pager.ShowSpread(0, true); // 단서 사전은 첫 면 오른쪽 쪽
            SetNotebookOpen(true, true);
        }

        private void Bind(GuestDialogueData data)
        {
            nameText.text = $"{data.guestName}  <size=70%>{data.raceJob}</size>";
            regularTag.SetActive(data.isRegular);
            destinationText.text = data.destinationLabel;
            dialogueText.text = ToLinkMarkup(data.line);
            ShowDialoguePage(1);
            ShowNotice(null);

            for (int i = 0; i < clueMarkers.Length; i++)
            {
                if (i < data.clues.Length) clueMarkers[i].Show(data.clues[i].description, data.clues[i].position);
                else clueMarkers[i].Hide();
            }

            for (int i = 0; i < riskRows.Length; i++)
            {
                int known = i < data.knownRisks.Length ? data.knownRisks[i] : -1;
                int memo = i < data.memos.Length ? data.memos[i] : 0;
                riskRows[i].Set(known, memo);
            }

            SetNotebookOpen(false, false);
            pager.ShowSpread(0, false); // 응대를 열 때는 항상 위험 쪽부터
            sendHomeConfirm.SetActive(false);
            sendHomeButton.gameObject.SetActive(data.canSendHome);
        }

        // 수첩을 옆으로 밀어 열거나 닫는다. animate가 false면 바로 그 자리로 옮긴다(화면 열 때 초기화용).
        private void SetNotebookOpen(bool open, bool animate)
        {
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

        // 대사의 몇 번째 쪽(1부터)을 보여 주고, 뒤에 쪽이 더 있으면 [다음]을 띄운다.
        private void ShowDialoguePage(int page)
        {
            dialogueText.ForceMeshUpdate(); // 쪽 수를 바로 계산하게 한다
            int pageCount = Mathf.Max(1, dialogueText.textInfo.pageCount);
            dialogueText.pageToDisplay = Mathf.Clamp(page, 1, pageCount);
            nextPageButton.gameObject.SetActive(dialogueText.pageToDisplay < pageCount);
        }

        // [[슬라임 점액]] → 클릭 가능한 밑줄 링크
        private static string ToLinkMarkup(string line)
        {
            if (string.IsNullOrEmpty(line))
                return string.Empty;
            return ClueMarkup.Replace(line, m =>
                $"<link=\"{m.Groups[1].Value}\"><u><color=#B5651D>{m.Groups[1].Value}</color></u></link>");
        }

        private bool CanAcceptInput() => IsOpen && !isTransitioning;

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
    }
}
