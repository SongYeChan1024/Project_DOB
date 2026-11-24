// =====================================================================
// SettingsView — 설정 창 (화면 · 소리 · 조작 안내)
// 역할   : GameSettings의 지금 값을 보여 주고, 누른 값을 GameSettings에 넘긴다 (바로 적용).
//          [뒤로]를 누르면 닫고 Closed를 알린다. 조작 안내는 그림 · 글만 있는 칸이라 코드가 없다.
// 호출자 : PauseMenuView (ESC 메뉴의 설정), TitleView (타이틀 설정)
// 호출   : GameSettings
// 상태   : 없음 (값은 GameSettings가 가진다)
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

        private void Awake()
        {
            prevResolution.onClick.AddListener(() => GameSettings.StepResolution(-1));
            nextResolution.onClick.AddListener(() => GameSettings.StepResolution(1));
            for (int i = 0; i < modeButtons.Length; i++)
            {
                var mode = (GameSettings.Mode)i; // 람다가 반복 변수 대신 고정된 값을 쓰도록 복사
                modeButtons[i].onClick.AddListener(() => GameSettings.SetMode(mode));
            }
            vsyncButtons[0].onClick.AddListener(() => GameSettings.SetVSync(true));
            vsyncButtons[1].onClick.AddListener(() => GameSettings.SetVSync(false));
            for (int i = 0; i < volumeSliders.Length; i++)
            {
                var kind = (GameSettings.Volume)i;
                volumeSliders[i].onValueChanged.AddListener(v => GameSettings.SetVolume(kind, v));
            }
            resetButton.onClick.AddListener(GameSettings.ResetDefaults);
            backButton.onClick.AddListener(Close);
        }

        private void OnEnable()
        {
            GameSettings.Changed += Refresh;
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
            int index = GameSettings.ResolutionIndex;
            var r = list[index];
            resolutionText.text = $"{r.x} × {r.y}";
            prevResolution.interactable = index > 0;
            nextResolution.interactable = index < list.Count - 1;

            for (int i = 0; i < modeButtons.Length; i++)
                Mark(modeButtons[i], i == (int)GameSettings.DisplayMode);
            Mark(vsyncButtons[0], GameSettings.VSync);
            Mark(vsyncButtons[1], !GameSettings.VSync);

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
