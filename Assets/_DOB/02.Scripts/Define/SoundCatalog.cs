// =====================================================================
// SoundCatalog — 효과음 신호 → 클립·음량 비율, 씬 → BGM 곡, 일시 정지 중 BGM 비율 (08.Data/Resources/SO_SoundCatalog)
// 호출자 : SoundManager(효과음 재생, 씬이 열릴 때 BGM 선택, 음량 계산)
// 의사코드: 값은 인스펙터에서만 정한다. 클립이 비어 있으면 그 신호·곡은 소리 없이 넘어간다(음원 수급 전 임시 검증).
//   ClipOf(cue)        — 그 신호의 클립과 음량 비율. 없으면 null.
//   TrackForScene(씬)  — 그 씬이 열릴 때 틀 곡 id. 표에 없는 씬이면 null(지금 곡 유지).
//   Validate           — 곡 id가 비거나 겹치지 않고, 비율이 0~1인지.
// 💡 일시 정지 중 BGM 비율 0.4, 대화 중 BGM 유지는 기획 미정(🔴) 항목에 정한 임의값이다(송예찬 확인 전).
// =====================================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>실제 성공 결과에만 울리는 효과음 신호.</summary>
    public enum SoundCue { GuestEntered, CookFinished, Served, GoldEarned, ReportStamped }

    [Serializable]
    public struct CueClip
    {
        public SoundCue cue;
        public AudioClip clip;
        /// <summary>효과음 음량에 곱하는 비율(0~1).</summary>
        [Range(0f, 1f)] public float volume;
    }

    [Serializable]
    public struct BgmTrack
    {
        public string id;
        public AudioClip clip;
        /// <summary>이 곡을 트는 씬 이름. Additive로 함께 열리는 씬은 하나만 적는다.</summary>
        public string[] scenes;
    }

    [CreateAssetMenu(menuName = "DOB/사운드 목록", fileName = ResourceName)]
    public sealed class SoundCatalog : ScriptableObject
    {
        public const string ResourceName = "SO_SoundCatalog";

        [SerializeField] private CueClip[] cues = Array.Empty<CueClip>();
        [SerializeField] private BgmTrack[] tracks = Array.Empty<BgmTrack>();
        /// <summary>ESC 일시 정지 중 BGM 음량에 곱하는 비율.</summary>
        [SerializeField, Range(0f, 1f)] private float pauseBgmScale = 0.4f;

        public float PauseBgmScale => pauseBgmScale;
        public IReadOnlyList<BgmTrack> Tracks => tracks;

        public static SoundCatalog Load() => Resources.Load<SoundCatalog>(ResourceName);

        /// <summary>그 신호의 클립. 없으면 null이고 volume은 0.</summary>
        public AudioClip ClipOf(SoundCue cue, out float volume)
        {
            foreach (var entry in cues)
                if (entry.cue == cue && entry.clip)
                {
                    volume = entry.volume;
                    return entry.clip;
                }
            volume = 0f;
            return null;
        }

        /// <summary>그 씬이 열릴 때 틀 곡 id. 없으면 null.</summary>
        public string TrackForScene(string sceneName)
        {
            foreach (var track in tracks)
                if (track.scenes != null && Array.IndexOf(track.scenes, sceneName) >= 0)
                    return track.id;
            return null;
        }

        /// <summary>곡 id의 클립. 없으면 null.</summary>
        public AudioClip TrackClip(string id)
        {
            foreach (var track in tracks)
                if (track.id == id)
                    return track.clip;
            return null;
        }

        /// <summary>기준값 오류. 문제가 없으면 null. 클립이 비어 있는 것은 오류가 아니다.</summary>
        public string Validate()
        {
            var ids = new HashSet<string>();
            foreach (var track in tracks)
                if (string.IsNullOrWhiteSpace(track.id) || !ids.Add(track.id)) return "BGM 곡 id가 비었거나 겹칩니다.";
            foreach (var entry in cues)
                if (entry.volume < 0f || entry.volume > 1f) return $"{entry.cue}의 음량 비율은 0~1이어야 합니다.";
            return pauseBgmScale < 0f || pauseBgmScale > 1f ? "일시 정지 BGM 비율은 0~1이어야 합니다." : null;
        }
    }
}
