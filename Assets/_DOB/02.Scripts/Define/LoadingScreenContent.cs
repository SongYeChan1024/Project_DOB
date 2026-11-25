// =====================================================================
// LoadingScreenContent — 로딩 화면에 보여 줄 그림·팁 데이터 (ScriptableObject)
// 역할   : 목적지(Title·Restaurant 등 로딩 화면을 여는 쪽이 넘기는 이름)마다 제목·그림 1장·팁 목록을 담는다.
//          팁 문구는 기획(송예찬), 그림은 아트(이기혁)가 에셋에서 직접 채운다. 코드에 문구를 쓰지 않는다.
//          로딩 화면의 최소 표시 시간과 페이드 시간도 기획 값으로 함께 담는다.
// 호출자 : 로딩 화면 Presenter (GetHeadingImage, GetHeading, GetIllustration, GetTips, GetRandomTip,
//          MinimumDisplaySeconds, FadeSeconds)
// 상태   : 없음 (에디터에서 채운 값을 읽기만 한다)
//
// 의사코드
// [팁 하나]  목적지 팁 목록(없으면 공통 팁)에서 무작위로 하나를 고른다. 목록이 비면 빈 문자열
// [시간]     음수 입력은 0으로 다룬다
// =====================================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>목적지별 로딩 그림과 팁 목록.</summary>
    [CreateAssetMenu(menuName = "DOB/로딩 화면 내용", fileName = "LoadingScreenContent")]
    public sealed class LoadingScreenContent : ScriptableObject
    {
        /// <summary>목적지 하나의 그림과 팁.</summary>
        [Serializable]
        public sealed class Entry
        {
            /// <summary>로딩 화면을 여는 쪽이 넘기는 목적지 이름 (Title, Restaurant).</summary>
            [SerializeField] private string destination;
            /// <summary>로딩 화면 위에 크게 보이는 목적지 제목 (예: 층계참 식당).</summary>
            [SerializeField] private string heading;
            /// <summary>제목 글씨 그림 (빛 번짐 포함 PNG). 있으면 글자 대신 이 그림을 보여 준다.</summary>
            [SerializeField] private Sprite headingImage;
            [SerializeField] private Sprite illustration;
            [SerializeField, TextArea(1, 3)] private string[] tips;

            public string Destination => destination;
            public string Heading => heading;
            public Sprite HeadingImage => headingImage;
            public Sprite Illustration => illustration;
            public IReadOnlyList<string> Tips => tips ?? Array.Empty<string>();
        }

        [SerializeField] private Entry[] entries;
        /// <summary>목적지 그림이 없을 때 쓸 그림.</summary>
        [SerializeField] private Sprite fallbackIllustration;
        /// <summary>목적지 팁이 없을 때 쓸 공통 팁.</summary>
        [SerializeField, TextArea(1, 3)] private string[] commonTips;

        /// <summary>로딩 화면이 완전히 보인 뒤 최소한 머무는 시간(초). 로드가 빨라도 그림·팁을 읽을 수 있게 한다.</summary>
        [Header("시간"), SerializeField, Min(0f)] private float minimumDisplaySeconds = 1.5f;
        /// <summary>로딩 화면이 검은 화면에서 나타나거나 검게 사라지는 시간(초).</summary>
        [SerializeField, Min(0f)] private float fadeSeconds = 0.35f;

        /// <summary>로딩 화면 최소 표시 시간(초).</summary>
        public float MinimumDisplaySeconds => Mathf.Max(0f, minimumDisplaySeconds);
        /// <summary>로딩 화면 페이드 시간(초).</summary>
        public float FadeSeconds => Mathf.Max(0f, fadeSeconds);

        /// <summary>목적지 팁 하나를 무작위로 고른다. 팁이 하나도 없으면 빈 문자열.</summary>
        public string GetRandomTip(string destination)
        {
            var tips = GetTips(destination);
            return tips.Count > 0 ? tips[UnityEngine.Random.Range(0, tips.Count)] ?? string.Empty : string.Empty;
        }

        /// <summary>목적지 제목. 없으면 빈 문자열.</summary>
        public string GetHeading(string destination)
        {
            var entry = Find(destination);
            return entry != null ? entry.Heading ?? string.Empty : string.Empty;
        }

        /// <summary>목적지 제목 글씨 그림. 없으면 null (그때는 제목 글자를 쓴다).</summary>
        public Sprite GetHeadingImage(string destination)
        {
            return Find(destination)?.HeadingImage;
        }

        /// <summary>목적지 그림. 없으면 기본 그림.</summary>
        public Sprite GetIllustration(string destination)
        {
            var entry = Find(destination);
            return entry != null && entry.Illustration != null ? entry.Illustration : fallbackIllustration;
        }

        /// <summary>목적지 팁 목록. 없으면 공통 팁.</summary>
        public IReadOnlyList<string> GetTips(string destination)
        {
            var entry = Find(destination);
            if (entry != null && entry.Tips.Count > 0)
                return entry.Tips;
            return commonTips ?? Array.Empty<string>();
        }

        private Entry Find(string destination)
        {
            if (entries == null) return null;
            foreach (var entry in entries)
                if (entry != null && entry.Destination == destination)
                    return entry;
            return null;
        }
    }
}
