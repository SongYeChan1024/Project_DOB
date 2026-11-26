// =====================================================================
// SettingsView — 설정 창 (화면 · 소리 · 조작 안내)
// 역할   : GameSettings의 지금 값을 보여 주고, 누른 값을 GameSettings에 넘긴다.
//          화면(해상도 · 화면 모드 · 수직 동기화)은 고른 값을 들고만 있다가 [화면 적용]에서 한 번에 넘긴다
//          (화면이 고를 때마다 바뀌지 않게). 음량은 바로 넘긴다.
//          [뒤로]를 누르면 닫고 Closed를 알린다. 조작 안내는 그림 · 글만 있는 칸이라 코드가 없다.
// 호출자 : PauseMenuView (ESC 메뉴의 설정), TitleView (타이틀 설정)
// 호출   : GameSettings
// 상태   : 적용 전 화면 값 (열 때마다 저장된 값으로 되돌림)
//
// 의사코드
// [열기]        고른 화면 값 ← 저장된 값 → 다시 그리기
// [화면 칸]     고른 값만 바꾼다 → 다시 그리기 (저장된 값과 다르면 [화면 적용] 활성, 같으면 반투명 · 잠김)
// [화면 적용]   고른 값을 GameSettings.ApplyDisplay로 넘긴다
// [기본값으로]  GameSettings.ResetDefaults → 고른 값도 기본값으로
// [뒤로] · Esc  적용하지 않은 화면 값은 버린다 (다음에 열면 저장된 값)
// 비고   : Esc로 닫기는 여는 쪽이 맡는다 (ESC 메뉴는 한 단계 뒤로, 타이틀은 창 닫기).
// =====================================================================
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>설정 창.</summary>
    public sealed class SettingsView : MonoBehaviour
    {
        [Header("화면")]
        [SerializeField] private TMP_Text resolutionText;
        [SerializeField] private Button prevResolution;
        [SerializeField] private Button nextResolution;
        [Tooltip("전체화면 · 창 모드 · 테두리 없음 순서. 자식: Selected, Label")]
        [SerializeField] private Button[] modeButtons = new Button[3];
        [Tooltip("켜기 · 끄기 순서. 자식: Selected, Label")]
        [SerializeField] private Button[] vsyncButtons = new Button[2];
        /// <summary>[화면 적용]. 바꾼 화면 값이 있을 때만 누를 수 있다.</summary>
        [SerializeField] private Button applyDisplayButton;
        /// <summary>[화면 적용]이 잠겼을 때의 투명도.</summary>
        [SerializeField, Range(0f, 1f)] private float applyDisabledAlpha = 0.4f;

        [Header("소리 (전체 · 배경음 · 효과음 순서)")]
        [SerializeField] private Slider[] volumeSliders = new Slider[3];
        [SerializeField] private TMP_Text[] volumeTexts = new TMP_Text[3];

        [Header("단추")]
        [SerializeField] private Button resetButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Color selectedText = new Color32(245, 232, 205, 255);
        [SerializeField] private Color normalText = new Color32(70, 45, 25, 255);

        /// <summary>[뒤로]로 닫혔다.</summary>
        public event Action Closed;

        public bool IsOpen => gameObject.activeSelf;

        // 적용 전 화면 값
        private int pendingResolution;
        private GameSettings.Mode pendingMode;
        private bool pendingVSync;

        private bool HasPendingDisplay =>
            pendingResolution != GameSettings.ResolutionIndex || pendingMode != GameSettings.DisplayMode || pendingVSync != GameSettings.VSync;

        private void Awake()
        {
            prevResolution.onClick.AddListener(() => StepResolution(-1));
            nextResolution.onClick.AddListener(() => StepResolution(1));
            for (int i = 0; i < modeButtons.Length; i++)
            {
                var mode = (GameSettings.Mode)i; // 람다가 반복 변수 대신 고정된 값을 쓰도록 복사
                modeButtons[i].onClick.AddListener(() => { pendingMode = mode; Refresh(); });
            }
            vsyncButtons[0].onClick.AddListener(() => { pendingVSync = true; Refresh(); });
            vsyncButtons[1].onClick.AddListener(() => { pendingVSync = false; Refresh(); });
            if (applyDisplayButton)
                applyDisplayButton.onClick.AddListener(() => GameSettings.ApplyDisplay(pendingResolution, pendingMode, pendingVSync));
            for (int i = 0; i < volumeSliders.Length; i++)
            {
                var kind = (GameSettings.Volume)i;
                volumeSliders[i].onValueChanged.AddListener(v => GameSettings.SetVolume(kind, v));
            }
            resetButton.onClick.AddListener(() => { GameSettings.ResetDefaults(); LoadSavedDisplay(); Refresh(); });
            backButton.onClick.AddListener(Close);
        }

        private void OnEnable()
        {
            GameSettings.Changed += Refresh;
            LoadSavedDisplay(); // 지난번에 적용하지 않고 닫은 값은 버린다
            Refresh();
        }

        private void LoadSavedDisplay()
        {
            pendingResolution = GameSettings.ResolutionIndex;
            pendingMode = GameSettings.DisplayMode;
            pendingVSync = GameSettings.VSync;
        }

        private void StepResolution(int direction)
        {
            pendingResolution = Mathf.Clamp(pendingResolution + direction, 0, GameSettings.Resolutions.Count - 1);
            Refresh();
        }

        private void OnDisable() => GameSettings.Changed -= Refresh;

        public void Open() => gameObject.SetActive(true);

        /// <summary>닫고 Closed를 알린다.</summary>
        public void Close()
        {
            if (!IsOpen) return;
            gameObject.SetActive(false);
            Closed?.Invoke();
        }

        /// <summary>알림 없이 숨긴다 (여는 쪽이 통째로 닫을 때).</summary>
        public void Hide() => gameObject.SetActive(false);

        // 지금 값으로 다시 그린다 (슬라이더는 알림 없이 값만)
        private void Refresh()
        {
            var list = GameSettings.Resolutions;
            int index = Mathf.Clamp(pendingResolution, 0, list.Count - 1);
            var r = list[index];
            resolutionText.text = $"{r.x} × {r.y}";
            prevResolution.interactable = index > 0;
            nextResolution.interactable = index < list.Count - 1;

            for (int i = 0; i < modeButtons.Length; i++)
                Mark(modeButtons[i], i == (int)pendingMode);
            Mark(vsyncButtons[0], pendingVSync);
            Mark(vsyncButtons[1], !pendingVSync);

            if (applyDisplayButton)
            {
                bool pending = HasPendingDisplay;
                applyDisplayButton.interactable = pending;
                var group = applyDisplayButton.GetComponent<CanvasGroup>();
                if (group) group.alpha = pending ? 1f : applyDisabledAlpha;
            }

            for (int i = 0; i < volumeSliders.Length; i++)
            {
                float v = GameSettings.GetVolume((GameSettings.Volume)i);
                volumeSliders[i].SetValueWithoutNotify(v);
                volumeTexts[i].text = Mathf.RoundToInt(v * 100f) + "%";
            }
        }

        // 고른 칸: 금색으로 채우고 글자를 밝게
        private void Mark(Button button, bool selected)
        {
            button.transform.Find("Selected").gameObject.SetActive(selected);
            button.transform.Find("Label").GetComponent<TMP_Text>().color = selected ? selectedText : normalText;
        }
    }
}
