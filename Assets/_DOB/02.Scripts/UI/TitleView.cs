// =====================================================================
// TitleView — 타이틀 화면 표시와 입력
// 역할   : 메뉴 버튼·로딩 표시·새로 시작 확인창·설정 창(설정 PF_Settings)·실패 안내를 보여 주고,
//          데모 완료 슬롯이 있으면 「엔딩 날 다시 하기」 메뉴와 확인창을 보여 준다 (시안 24, 이어하기 · 확인창을 복제해 만듦)
//          버튼 입력을 이벤트로 알린다. 씬 이동·저장 확인은 하지 않는다.
// 호출자 : TitlePresenter (메뉴 구성, 잠금, 확인창, 실패 안내 요청)
// 호출   : 없음 (이벤트로만 알림)
// 상태   : 화면에 보이는 것만 가진다. 게임 상태는 갖지 않는다.
// 연출   : 전환을 시작하면 화면을 어둡게 덮은 뒤 Presenter에게 넘긴다 (시안 14 ⑦, FadeOutThen)
// =====================================================================
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>타이틀 메뉴를 표시하고 입력을 이벤트로 보낸다. 씬 전환과 저장 판단은 Presenter가 한다.</summary>
    public sealed class TitleView : MonoBehaviour
    {
        /// <summary>새로 시작 버튼.</summary>
        [Header("메뉴"), SerializeField] private Button newGameButton;
        /// <summary>이어하기 버튼. 저장이 없으면 숨긴다.</summary>
        [SerializeField] private Button continueButton;
        [SerializeField] private TMP_Text continueLabel;
        /// <summary>설정 창을 여는 버튼.</summary>
        [SerializeField] private Button settingsButton;
        /// <summary>게임 종료 버튼.</summary>
        [SerializeField] private Button quitButton;

        /// <summary>씬 전환 요청 뒤 보이는 "불러오는 중" 표시.</summary>
        [Header("상태 표시"), SerializeField] private GameObject loadingIndicator;
        /// <summary>씬 전환 요청이 거절됐을 때 보이는 안내.</summary>
        [SerializeField] private GameObject loadFailedMessage;

        /// <summary>저장이 있을 때 새로 시작 전에 띄우는 확인창.</summary>
        [Header("새로 시작 확인창"), SerializeField] private GameObject confirmPopup;
        [SerializeField] private Button confirmYesButton;
        [SerializeField] private Button confirmNoButton;

        /// <summary>설정 창 (ESC 메뉴와 같은 PF_Settings). 비워 두면 처음 열 때 스킨에서 만든다.</summary>
        [Header("설정 창"), SerializeField] private SettingsView settings;

        /// <summary>전환 전에 화면이 어두워지는 시간 (초).</summary>
        [Header("전환 어둡게"), SerializeField] private float fadeOutSeconds = 0.4f;

        private Image fadeCover;
        private Coroutine fadeOut;

        /// <summary>새로 시작을 눌렀다.</summary>
        public event Action NewGameClicked;
        /// <summary>확인창에서 새로 시작을 확정했다.</summary>
        public event Action NewGameConfirmed;
        /// <summary>이어하기를 눌렀다.</summary>
        public event Action ContinueClicked;
        /// <summary>나가기를 눌렀다.</summary>
        public event Action QuitClicked;
        /// <summary>「엔딩 날 다시 하기」를 눌렀다.</summary>
        public event Action DemoReplayClicked;
        /// <summary>엔딩 날 다시 하기 확인창에서 「다시 하기」를 눌렀다.</summary>
        public event Action DemoReplayConfirmed;

        // 「엔딩 날 다시 하기」 메뉴 · 확인창 (시안 24). Title 씬을 고치지 않게 이어하기 · 새로 시작 확인창을 복제해 만든다
        private const string DemoReplayWord = "엔딩 날 다시 하기";
        private static readonly Color DemoGold = new Color(0.965f, 0.831f, 0.541f, 1f);
        private Button demoButton;
        private TMP_Text demoLabel, demoDetail;
        private GameObject demoPopup;
        private TMP_Text demoMessage;

        private void Awake()
        {
            BuildDemoReplay(); // 이어하기에 일차 줄(Detail)이 붙기 전에 복제한다
            newGameButton.onClick.AddListener(() => NewGameClicked?.Invoke());
            continueButton.onClick.AddListener(() => ContinueClicked?.Invoke());
            quitButton.onClick.AddListener(() => QuitClicked?.Invoke());
            confirmYesButton.onClick.AddListener(() => NewGameConfirmed?.Invoke());
            confirmNoButton.onClick.AddListener(() => ShowConfirm(false));

            // 설정 창 열고 닫기는 게임 상태와 무관한 화면 동작이라 View 안에서 처리한다.
            settingsButton.onClick.AddListener(OpenSettings);
            if (settings) settings.Hide();

            loadingIndicator.SetActive(false);
            loadFailedMessage.SetActive(false);
            confirmPopup.SetActive(false);
        }

        // 의사코드: 이어하기 버튼을 복제해 바로 뒤에 두고 글자를 금빛 「엔딩 날 다시 하기」로, 너비를 글자에 맞춘다.
        //           새로 시작 확인창을 복제해 문구 · 단추 글자를 바꾸고, 단추를 이 메뉴의 이벤트에 잇는다. 둘 다 처음엔 숨긴다.
        private void BuildDemoReplay()
        {
            if (!continueButton || !continueLabel) return;
            demoButton = Instantiate(continueButton, continueButton.transform.parent);
            demoButton.name = "DemoReplayButton";
            demoButton.transform.SetSiblingIndex(continueButton.transform.GetSiblingIndex() + 1);
            demoLabel = demoButton.transform.Find(continueLabel.name).GetComponent<TMP_Text>();
            demoLabel.text = DemoReplayWord;
            demoLabel.color = DemoGold;
            var layout = demoButton.GetComponent<LayoutElement>();
            if (layout) layout.preferredWidth = layout.minWidth = Mathf.Ceil(demoLabel.GetPreferredValues(DemoReplayWord).x);
            demoButton.onClick = new Button.ButtonClickedEvent(); // 이어하기에 씬에서 걸어 둔 연결이 있어도 따라오지 않게
            demoButton.onClick.AddListener(() => DemoReplayClicked?.Invoke());
            demoButton.gameObject.SetActive(false);

            if (!confirmPopup) return;
            demoPopup = Instantiate(confirmPopup, confirmPopup.transform.parent);
            demoPopup.name = "DemoConfirmPopup";
            demoMessage = demoPopup.transform.Find("Message")?.GetComponent<TMP_Text>();
            SetChildLabel(demoPopup, "YesButton", "다시 하기").onClick.AddListener(() => DemoReplayConfirmed?.Invoke());
            SetChildLabel(demoPopup, "NoButton", "취소").onClick.AddListener(() => ShowDemoConfirm(false));
            demoPopup.SetActive(false);
        }

        private static Button SetChildLabel(GameObject root, string buttonName, string text)
        {
            var button = root.transform.Find(buttonName).GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent(); // 복제 원본(새로 시작 확인)의 연결을 끊는다
            var label = button.GetComponentInChildren<TMP_Text>(true);
            if (label) label.text = text;
            return button;
        }

        /// <summary>데모 완료 슬롯이 있으면 「엔딩 날 다시 하기」와 그 일차 줄을 보인다. null이면 숨긴다.</summary>
        public void ShowDemoReplay(string checkpointLabel)
        {
            if (!demoButton) return;
            bool show = !string.IsNullOrEmpty(checkpointLabel);
            if (show)
            {
                if (!demoDetail) { demoDetail = MakeDetail(demoButton, demoLabel); demoDetail.color = new Color(0.88f, 0.72f, 0.42f, 1f); }
                demoDetail.text = checkpointLabel;
            }
            demoButton.gameObject.SetActive(show);
        }

        /// <summary>엔딩 날 다시 하기 확인창을 열거나 닫는다. day는 다시 할 일차.</summary>
        public void ShowDemoConfirm(bool show, int day = 0)
        {
            if (!demoPopup) return;
            if (show && demoMessage)
                demoMessage.text = $"엔딩을 본 {day}일차 아침으로 돌아갑니다.\n이 하루를 마감하면 엔딩을 보고 타이틀로 돌아오며,\n이번 플레이는 저장되지 않습니다.";
            demoPopup.SetActive(show);
        }

        private void OpenSettings()
        {
            if (!settings) settings = CreateSettings();
            if (settings) settings.Open();
        }

        // 씬에 연결된 창이 없으면 ESC 메뉴와 같은 PF_Settings(Resources/UI/PauseMenuSkin에 연결)를
        // 타이틀 맨 위 캔버스에 한 번 만든다 (Title 씬을 고치지 않아도 되게)
        private SettingsView CreateSettings()
        {
            var skin = Resources.Load<PauseMenuSkin>("UI/PauseMenuSkin");
            if (!skin || !skin.settingsPrefab)
            {
                Debug.LogWarning("[DOB] Resources/UI/PauseMenuSkin 또는 PF_Settings가 없어 설정 창을 열 수 없습니다.");
                return null;
            }
            var host = new GameObject("SettingsCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            host.transform.SetParent(transform, false);
            var canvas = host.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 900; // 타이틀 연출 위
            var scaler = host.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            var view = Instantiate(skin.settingsPrefab, host.transform);
            view.Hide();
            return view;
        }

        // 타이틀에는 ESC 메뉴가 없으므로 Esc는 열린 설정 창만 닫는다
        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null || !keyboard.escapeKey.wasPressedThisFrame) return;
            if (settings && settings.IsOpen) settings.Close();
        }

        /// <summary>저장 여부에 맞춰 메뉴를 구성한다. 저장이 없으면 이어하기를 숨긴다.</summary>
        public void ShowMenu(bool hasSave, string checkpointLabel = null)
        {
            if (continueLabel)
            {
                continueLabel.text = "이어하기";
                var detail = ContinueDetail();
                detail.text = checkpointLabel ?? "";
                detail.gameObject.SetActive(!string.IsNullOrEmpty(checkpointLabel));
            }
            continueButton.gameObject.SetActive(hasSave);
            SetBusy(false);
        }

        // 저장 일차 줄은 버튼 아래 바깥에 둔다. 버튼 안에 두면 마우스를 올릴 때 나오는 밑줄이 글자를 가린다.
        // 이름은 Detail (테스트가 이 이름으로 읽음). Title 씬을 고치지 않게 이어하기 글자를 복제해 처음 한 번 만든다.
        private TMP_Text continueDetail;
        private TMP_Text ContinueDetail() => continueDetail ? continueDetail : continueDetail = MakeDetail(continueButton, continueLabel);

        private static TMP_Text MakeDetail(Button button, TMP_Text label)
        {
            var detail = Instantiate(label, button.transform);
            detail.name = "Detail";
            detail.raycastTarget = false;
            detail.fontSize = 20;
            detail.enableAutoSizing = false;
            detail.textWrappingMode = TextWrappingModes.NoWrap;
            detail.overflowMode = TextOverflowModes.Overflow;
            detail.alignment = TextAlignmentOptions.Top;
            var rect = detail.rectTransform;
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(0f, 26f);
            return detail;
        }

        /// <summary>전환 요청 중이면 모든 버튼을 잠그고 로딩 표시를 켠다.</summary>
        public void SetBusy(bool busy)
        {
            newGameButton.interactable = !busy;
            continueButton.interactable = !busy;
            if (demoButton) demoButton.interactable = !busy;
            settingsButton.interactable = !busy;
            quitButton.interactable = !busy;
            loadingIndicator.SetActive(busy);
            if (busy)
                loadFailedMessage.SetActive(false);
            // 전환이 실패해 잠금이 풀리면 어둡게 덮은 것도 걷는다
            if (!busy) ClearFade();
        }

        // 의사코드: 덮개를 (없으면 한 번 만들어) 켜고, 실제 시간으로 투명도 0 → 1, 다 덮이면 then을 부른다.
        //           꺼져 있어 코루틴을 못 돌리면 바로 부른다.
        /// <summary>화면을 어둡게 덮은 뒤 then을 부른다. 잠금(SetBusy(false))이 풀리면 덮개도 걷힌다.</summary>
        public void FadeOutThen(Action then)
        {
            if (!isActiveAndEnabled) { then?.Invoke(); return; }
            if (fadeOut != null) StopCoroutine(fadeOut);
            fadeOut = StartCoroutine(FadeOut(then));
        }

        private System.Collections.IEnumerator FadeOut(Action then)
        {
            var cover = FadeCover();
            cover.gameObject.SetActive(true);
            for (float t = 0f; t < fadeOutSeconds; t += Time.unscaledDeltaTime)
            {
                cover.color = new Color(0f, 0f, 0f, t / fadeOutSeconds);
                yield return null;
            }
            cover.color = Color.black;
            fadeOut = null;
            then?.Invoke();
        }

        private void ClearFade()
        {
            if (fadeOut != null) { StopCoroutine(fadeOut); fadeOut = null; }
            if (fadeCover) fadeCover.gameObject.SetActive(false);
        }

        // 설정 창 위 · 화면 전체를 덮는 검은 판. Title 씬을 고치지 않게 처음 한 번 만든다
        private Image FadeCover()
        {
            if (fadeCover) return fadeCover;
            var host = new GameObject("FadeCover", typeof(RectTransform), typeof(Canvas), typeof(Image));
            host.transform.SetParent(transform, false);
            var canvas = host.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 950; // 설정 창(900)보다 위
            fadeCover = host.GetComponent<Image>();
            fadeCover.raycastTarget = true; // 어두워지는 동안 메뉴가 또 눌리지 않게
            fadeCover.color = Color.clear;
            host.SetActive(false);
            return fadeCover;
        }

        /// <summary>새로 시작 확인창을 열거나 닫는다.</summary>
        public void ShowConfirm(bool show)
        {
            confirmPopup.SetActive(show);
        }

        /// <summary>전환 요청이 거절됐음을 알린다. 잠금은 Presenter가 SetBusy(false)로 푼다.</summary>
        public void ShowLoadFailed()
        {
            loadFailedMessage.SetActive(true);
        }
    }
}
