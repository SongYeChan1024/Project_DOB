// SoundManager — BGM·효과음 재생, 음량, 씬 전환 시 BGM 중복 방지를 총괄한다 (sound-playback 명세).
// 호출자: GameManager(성공 결과마다 Play), Unity(첫 씬이 열릴 때 스스로 생성, 씬이 열릴 때마다 BGM 선택).
// 호출  : SoundCatalog(클립·곡·일시 정지 비율), GameSettings(BGM·효과음 음량 — 전체 음량은 GameSettings가 AudioListener에 적용).
// 상태  : 하나뿐인 인스턴스, BGM·효과음 AudioSource 하나씩, 지금 곡 id.
// 의사코드:
//   생성 — 첫 씬이 열리면 인스턴스가 없을 때만 만들고 씬을 넘어 유지한다. 두 번째 인스턴스는 스스로 없앤다(씬을 고치지 않는다).
//   Play(신호) — 인스턴스가 없으면 무시. 있으면 Played로 알리고, 클립이 있을 때만 효과음 음원에서 한 번 낸다.
//   씬이 열림 — 카탈로그의 「씬 → 곡」에 있으면 PlayBgm. Additive로 함께 열리는 다른 씬(표에 없음)은 지금 곡을 그대로 둔다.
//   PlayBgm(곡) — 지금 곡과 같으면 처음부터 다시 틀지 않는다. 클립이 없으면 BGM을 멈추고 곡 id만 바꾼다.
//   매 프레임 — BGM 음량 = 설정 BGM 음량 × (ESC 일시 정지 중이면 💡 0.4), 효과음 음량 = 설정 효과음 음량. 대화 중에는 바꾸지 않는다(💡).
using System;
using DOB.Define;
using DOB.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DOB.Manager
{
    public sealed class SoundManager : MonoBehaviour
    {
        private static SoundManager instance;
        private AudioSource bgm;
        private AudioSource sfx;
        private SoundCatalog catalog;

        /// <summary>효과음을 요청할 때마다 알린다(클립이 없어도). 테스트가 재생 횟수를 센다.</summary>
        public static event Action<SoundCue> Played;
        /// <summary>지금 곡 id. 인스턴스가 없거나 곡이 없으면 null.</summary>
        public static string CurrentTrack { get; private set; }
        /// <summary>지금 BGM 음원의 음량. 인스턴스가 없으면 0.</summary>
        public static float BgmVolume => instance ? instance.bgm.volume : 0f;
        /// <summary>지금 효과음 음원의 음량. 인스턴스가 없으면 0.</summary>
        public static float SfxVolume => instance ? instance.sfx.volume : 0f;
        /// <summary>살아 있는 BGM 음원 수(중복 방지 확인용).</summary>
        public static int BgmSourceCount => instance ? 1 : 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateOnce()
        {
            if (!instance)
                new GameObject(nameof(SoundManager)).AddComponent<SoundManager>();
        }

        /// <summary>성공 결과의 효과음. 인스턴스가 없으면 아무것도 하지 않는다.</summary>
        public static void Play(SoundCue cue)
        {
            if (!instance)
                return;
            Played?.Invoke(cue);
            float volume = 0f;
            var clip = instance.catalog ? instance.catalog.ClipOf(cue, out volume) : null;
            if (clip)
                instance.sfx.PlayOneShot(clip, volume);
        }

        /// <summary>곡을 튼다. 지금 곡과 같으면 이어서 튼다.</summary>
        public static void PlayBgm(string trackId)
        {
            if (!instance || trackId == CurrentTrack)
                return;
            CurrentTrack = trackId;
            var clip = instance.catalog ? instance.catalog.TrackClip(trackId) : null;
            instance.bgm.clip = clip;
            if (clip) instance.bgm.Play();
            else instance.bgm.Stop();
        }

        private void Awake()
        {
            if (instance && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            catalog = SoundCatalog.Load();
            if (catalog && catalog.Validate() is string error)
                Debug.LogWarning("[DOB] 사운드 목록을 확인하세요. " + error, catalog);
            bgm = gameObject.AddComponent<AudioSource>();
            bgm.loop = true;
            bgm.playOnAwake = false;
            sfx = gameObject.AddComponent<AudioSource>();
            sfx.playOnAwake = false;
            SceneManager.sceneLoaded += OnSceneLoaded;
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            Update();
        }

        private void OnDestroy()
        {
            if (instance != this)
                return;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
            CurrentTrack = null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string track = catalog ? catalog.TrackForScene(scene.name) : null;
            if (track != null)
                PlayBgm(track);
        }

        private void Update()
        {
            float pause = Time.timeScale <= 0f && catalog ? catalog.PauseBgmScale : 1f;
            bgm.volume = GameSettings.GetVolume(GameSettings.Volume.Bgm) * pause;
            sfx.volume = GameSettings.GetVolume(GameSettings.Volume.Sfx);
        }
    }
}
