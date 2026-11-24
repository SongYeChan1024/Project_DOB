// ESC → 영업 시간 정지 → 저장 일차 확인, 슬롯 복원, 음량 조정 또는 타이틀 복귀.
// 취소하면 파일과 진행 상태를 보존하고, 닫히거나 씬이 종료되면 열기 전 시간 배율과 포커스를 복원한다.
using System;
using System.IO;
using DOB.Systems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DOB.Manager
{
    [DisallowMultipleComponent]
    public sealed class BasementPauseMenu : MonoBehaviour
    {
        [SerializeField] private Font font;
        // ESC 메뉴는 BasementComposition.Current를 읽지 않는다. 저장소·일차·저장 요청은 GameManager가 Initialize·이벤트로 잇는다.
        private const string VolumeKey = "DOB.Basement.MasterVolume";
        private enum Page { Main, Save, Load, ConfirmLoad, Settings, Exit }
        private Page page;
        private BasementPauseView view;
        private GameObject previousFocus;
        private float previousScale;
        private static BasementPauseMenu openMenu;
        private FirstServiceSaveStore saves;
        private int day;
        public static bool IsPaused => openMenu;
        /// <summary>수동 저장 확인을 눌렀을 때. 파일 오류는 이 호출로 그대로 올라와 메뉴에 안내된다.</summary>
        public event Action SaveRequested;

        /// <summary>영업이 시작될 때 GameManager가 저장소와 현재 일차를 넘긴다. 그 전에는 ESC를 무시한다.</summary>
        public void Initialize(FirstServiceSaveStore store, int currentDay)
        {
            saves = store;
            day = currentDay;
        }

        private void Awake()
        {
            view = new BasementPauseView(transform, font ? font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"));
            if (PlayerPrefs.HasKey(VolumeKey))
                AudioListener.volume = Mathf.Clamp01(PlayerPrefs.GetFloat(VolumeKey));
        }

        public void HandleEscape()
        {
            if (!isActiveAndEnabled || saves == null)
                return;
            if (!IsPaused)
            {
                previousFocus = EventSystem.current ? EventSystem.current.currentSelectedGameObject : null;
                previousScale = Time.timeScale;
                Time.timeScale = 0;
                openMenu = this;
                Main();
            }
            else if (openMenu == this)
            {
                if (page == Page.Main) Close();
                else if (page == Page.ConfirmLoad) LoadSlots();
                else Main();
            }
        }

        public void Close()
        {
            if (openMenu != this) return;
            openMenu = null;
            Time.timeScale = previousScale;
            view.Hide();
            if (EventSystem.current)
                EventSystem.current.SetSelectedGameObject(previousFocus && previousFocus.activeInHierarchy ? previousFocus : null);
            previousFocus = null;
        }

        private void OnDisable() => Close();

        private void Main(string note = null)
        {
            page = Page.Main;
            view.Begin("일시정지");
            string recent = "최근 자동 저장 없음";
            TryIo(() =>
            {
                if (saves.TryLoad(FirstServiceSaveSlot.Auto, out var saved))
                    recent = "최근 자동 저장  " + Format(saved);
            }, error => recent = error);
            view.Action("Resume", "이어하기\n" + recent, Close);
            view.Action("Save", "저장", ConfirmSave);
            view.Action("Load", "불러오기", LoadSlots);
            view.Action("Settings", "환경설정", Settings);
            view.Action("Exit", "끝내기", ConfirmExit);
            if (!string.IsNullOrEmpty(note)) view.Label(note);
            view.Focus();
        }

        private void ConfirmSave()
        {
            page = Page.Save;
            view.Begin("수동 저장");
            view.Label($"불러오면 {day}일차 시작 시점부터 다시 시작합니다. 저장하시겠습니까?", 24, 76);
            view.Action("ConfirmSave", "저장", () => TryIo(() =>
            {
                SaveRequested?.Invoke();
                Main("수동 저장했습니다. 현재 영업을 계속할 수 있어요.");
            }, Main));
            view.Action("CancelSave", "취소", () => Main());
            view.Focus();
        }

        private void LoadSlots()
        {
            page = Page.Load;
            view.Begin("불러오기");
            AddSlot(FirstServiceSaveSlot.Auto, "자동 저장");
            AddSlot(FirstServiceSaveSlot.Manual, "수동 저장");
            view.Action("Back", "뒤로", () => Main());
            view.Focus();
        }

        private void AddSlot(FirstServiceSaveSlot slot, string title)
        {
            FirstServiceSaveData saved = null;
            string details = "비어 있음";
            TryIo(() =>
            {
                if (saves.TryLoad(slot, out saved))
                    details = Format(saved);
            }, error => details = error);
            view.Action(slot.ToString(), title + "\n" + details, () => ConfirmLoad(saved), saved != null);
        }

        private void ConfirmLoad(FirstServiceSaveData saved)
        {
            page = Page.ConfirmLoad;
            view.Begin("저장한 일차 불러오기");
            view.Label($"현재 영업을 마치고 {saved.Day}일차 시작 시점으로 돌아갑니까?\n이번 영업에서 조리하거나 서빙한 내용은 사라집니다.", 23, 88);
            view.Action("ConfirmLoad", "불러오기", () =>
            {
                if (BasementComposition.EnterDay(saved)) Close();
                else Main("저장한 일차의 씬을 열 수 없어요.");
            });
            view.Action("CancelLoad", "취소", LoadSlots);
            view.Focus();
        }

        private void Settings()
        {
            page = Page.Settings;
            view.Begin("환경설정");
            var label = view.Label($"전체 음량 {Mathf.RoundToInt(AudioListener.volume * 100f)}%");
            view.Volume(AudioListener.volume, volume =>
            {
                AudioListener.volume = volume;
                PlayerPrefs.SetFloat(VolumeKey, volume);
                label.text = $"전체 음량 {Mathf.RoundToInt(volume * 100f)}%";
            });
            view.Action("Back", "뒤로", () => Main());
            view.Focus();
        }

        private void ConfirmExit()
        {
            page = Page.Exit;
            view.Begin("타이틀로 돌아가기");
            view.Label("영업을 끝내고 타이틀로 돌아갑니까?\n저장한 일차 시작 시점에서 다시 플레이할 수 있어요.", 23, 80);
            view.Action("ConfirmExit", "타이틀로", () =>
            {
                if (BasementComposition.GoToTitle()) Close();
                else Main("타이틀 씬을 열 수 없어요.");
            });
            view.Action("CancelExit", "취소", () => Main());
            view.Focus();
        }

        internal static string Format(FirstServiceSaveData saved) =>
            $"{saved.Day}일차 시작  ·  {saved.SavedAtUtc.ToLocalTime():yyyy.MM.dd HH:mm}";

        internal static void TryIo(Action readOrWrite, Action<string> showError)
        {
            try { readOrWrite(); }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Debug.LogWarning("[DOB] 저장 파일을 열 수 없습니다. " + e.Message);
                showError("저장 파일을 열 수 없어요. 폴더의 쓰기 권한을 확인해 주세요.");
            }
        }
    }
}
