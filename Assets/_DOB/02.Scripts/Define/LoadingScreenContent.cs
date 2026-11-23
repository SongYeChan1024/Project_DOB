// =====================================================================
// LoadingScreenContent — 로딩 화면에 보여 줄 그림·팁 데이터 (ScriptableObject)
// 역할   : 목적지(Title·Restaurant·Tutorial·ReturnReport)마다 그림 1장과 팁 목록을 담는다.
//          팁 문구는 기획(송예찬), 그림은 아트(이기혁)가 에셋에서 직접 채운다. 코드에 문구를 쓰지 않는다.
// 호출자 : 로딩 화면 Presenter (GetIllustration, GetTips)
// 상태   : 없음 (에디터에서 채운 값을 읽기만 한다)
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
            /// <summary>SceneComposition의 진입 경로 이름 (Title, Restaurant, Tutorial, ReturnReport).</summary>
            [SerializeField] private string destination;
            [SerializeField] private Sprite illustration;
            [SerializeField, TextArea(1, 3)] private string[] tips;

            public string Destination => destination;
            public Sprite Illustration => illustration;
            public IReadOnlyList<string> Tips => tips ?? Array.Empty<string>();
        }

        [SerializeField] private Entry[] entries;
        /// <summary>목적지 그림이 없을 때 쓸 그림.</summary>
        [SerializeField] private Sprite fallbackIllustration;
        /// <summary>목적지 팁이 없을 때 쓸 공통 팁.</summary>
        [SerializeField, TextArea(1, 3)] private string[] commonTips;

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
