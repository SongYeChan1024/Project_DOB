// =====================================================================
// GameSettings — 환경설정 값(화면 · 소리)을 저장하고 적용한다
// 역할   : 해상도(16:9, 1280×720 ~ 1920×1080) · 화면 모드 · 수직 동기화 · 음량(전체 · 배경음 · 효과음)을
//          PlayerPrefs에 저장하고, 고르는 즉시 적용한다. 게임 시작 때 저장된 값을 한 번 적용한다.
// 호출자 : SettingsView (값 바꾸기 · 기본값), 게임 시작 (ApplySaved 자동)
// 호출   : Screen.SetResolution, QualitySettings, AudioListener
// 상태   : PlayerPrefs (키 이름은 아래 상수). 게임 규칙 값은 갖지 않는다.
// 비고   : 배경음 · 효과음은 값만 저장한다. 실제 적용은 제작 08 SoundManager가 GetVolume으로 읽어 쓴다.
//          전체 음량 키는 기존 ESC 메뉴(BasementPauseMenu)와 같은 키를 이어 쓴다.
//          해상도 · 화면 모드는 에디터 Play에서는 바뀌지 않는다 (빌드에서 확인).
//
// 의사코드
// [시작]      저장된 값을 읽어 화면 · 수직 동기화 · 전체 음량을 적용한다
// [해상도]    모니터보다 크지 않은 16:9 목록에서 앞 · 뒤로 고른다 → 바로 적용 · 저장
// [화면 모드] 전체화면 · 창 모드 · 테두리 없음 → 바로 적용 · 저장
// [음량]      0~1로 저장, 전체 음량은 AudioListener에 바로
// [기본값]    가장 큰 해상도 · 전체화면 · 수직 동기화 켬 · 음량 100%
// =====================================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>환경설정 값 저장 · 적용.</summary>
    public static class GameSettings
    {
        public enum Mode { FullScreen, Windowed, Borderless }
        public enum Volume { Master, Bgm, Sfx }

        // 16:9 해상도 (기획: 최소 1280×720, 최대 1920×1080)
        private static readonly Vector2Int[] All =
        {
            new Vector2Int(1280, 720), new Vector2Int(1366, 768), new Vector2Int(1600, 900), new Vector2Int(1920, 1080),
        };

        private const string ResolutionKey = "DOB.Settings.Resolution";
        private const string ModeKey = "DOB.Settings.Mode";
        private const string VSyncKey = "DOB.Settings.VSync";
        private const string MasterKey = "DOB.Basement.MasterVolume";
        private const string BgmKey = "DOB.Settings.BgmVolume";
        private const string SfxKey = "DOB.Settings.SfxVolume";

        /// <summary>값이 바뀌었다 (화면을 다시 그리게).</summary>
        public static event Action Changed;

        /// <summary>이 모니터에서 고를 수 있는 해상도 (작은 것부터).</summary>
        public static IReadOnlyList<Vector2Int> Resolutions
        {
            get
            {
                var desktop = Screen.currentResolution;
                var list = new List<Vector2Int>();
                foreach (var r in All)
                    if (r.x <= desktop.width && r.y <= desktop.height) list.Add(r);
                if (list.Count == 0) list.Add(All[0]);
                return list;
            }
        }

        /// <summary>지금 해상도의 목록 번호. 저장 값이 목록에 없으면 가장 큰 것.</summary>
        public static int ResolutionIndex
        {
            get
            {
                var list = Resolutions;
                string saved = PlayerPrefs.GetString(ResolutionKey, string.Empty);
                for (int i = 0; i < list.Count; i++)
                    if (saved == Key(list[i])) return i;
                return list.Count - 1;
            }
        }

        public static Mode DisplayMode => (Mode)Mathf.Clamp(PlayerPrefs.GetInt(ModeKey, (int)Mode.FullScreen), 0, 2);
        public static bool VSync => PlayerPrefs.GetInt(VSyncKey, 1) == 1;

        public static float GetVolume(Volume kind) => Mathf.Clamp01(PlayerPrefs.GetFloat(VolumeKey(kind), 1f));

        // 게임 시작 때 저장된 값을 한 번 적용한다
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ApplySaved()
        {
            AudioListener.volume = GetVolume(Volume.Master);
            QualitySettings.vSyncCount = VSync ? 1 : 0;
            ApplyScreen();
        }

        // =================================================================
        // 바꾸기 (고르는 즉시 적용)
        // =================================================================

        /// <summary>해상도를 앞(-1) · 뒤(+1)로 바꾼다. 목록 끝이면 그대로.</summary>
        public static void StepResolution(int direction)
        {
            var list = Resolutions;
            int next = Mathf.Clamp(ResolutionIndex + direction, 0, list.Count - 1);
            PlayerPrefs.SetString(ResolutionKey, Key(list[next]));
            Save();
            ApplyScreen();
        }

        public static void SetMode(Mode mode)
        {
            PlayerPrefs.SetInt(ModeKey, (int)mode);
            Save();
            ApplyScreen();
        }

        public static void SetVSync(bool on)
        {
            PlayerPrefs.SetInt(VSyncKey, on ? 1 : 0);
            QualitySettings.vSyncCount = on ? 1 : 0;
            Save();
        }

        public static void SetVolume(Volume kind, float value)
        {
            value = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(VolumeKey(kind), value);
            if (kind == Volume.Master) AudioListener.volume = value;
            Save();
        }

        /// <summary>가장 큰 해상도 · 전체화면 · 수직 동기화 켬 · 음량 100%.</summary>
        public static void ResetDefaults()
        {
            foreach (var key in new[] { ResolutionKey, ModeKey, VSyncKey, MasterKey, BgmKey, SfxKey })
                PlayerPrefs.DeleteKey(key);
            ApplySaved();
            Save();
        }

        // =================================================================
        // 도우미
        // =================================================================

        private static void ApplyScreen()
        {
            var r = Resolutions[ResolutionIndex];
            Screen.SetResolution(r.x, r.y, ToUnity(DisplayMode));
        }

        private static FullScreenMode ToUnity(Mode mode)
        {
            switch (mode)
            {
                case Mode.Windowed: return FullScreenMode.Windowed;
                case Mode.Borderless: return FullScreenMode.FullScreenWindow;
                default: return FullScreenMode.ExclusiveFullScreen;
            }
        }

        private static string VolumeKey(Volume kind) => kind == Volume.Master ? MasterKey : kind == Volume.Bgm ? BgmKey : SfxKey;

        private static string Key(Vector2Int r) => r.x + "x" + r.y;

        private static void Save()
        {
            PlayerPrefs.Save();
            Changed?.Invoke();
        }
    }
}
