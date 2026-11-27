// =====================================================================
// GuestPortraitCatalog — 응대 화면에 쓰는 손님 초상 그림 목록
// 역할   : 손님 이름별 기본 초상과 컨디션 레이어(개인 취약점 축 · 지침)를 묶어 둔다. 고르는 규칙은 없다.
//          컨디션 레이어는 기본 초상과 같은 크기의 투명 그림이라 기본 초상 위 같은 자리에 겹쳐 쓴다.
// 호출자 : FirstGuestDialogue가 Resources에서 이름(ResourceName)으로 읽어 초상을 고른다.
// 상태   : 없음 (인스펙터에서 넣은 그림 목록만 가진다)
// =====================================================================
using System;
using UnityEngine;

namespace DOB.Define
{
    [CreateAssetMenu(menuName = "DOB/손님 초상 목록", fileName = ResourceName)]
    public sealed class GuestPortraitCatalog : ScriptableObject
    {
        public const string ResourceName = "SO_GuestPortraitCatalog";
        /// <summary>지침 레이어의 상태 이름. 개인 취약점은 규칙 자산의 축 이름(독 · 열 · 냉기 · 어둠 · 함정)을 그대로 쓴다.</summary>
        public const string TiredCondition = "지침";

        /// <summary>상태 하나에 대응하는 컨디션 레이어.</summary>
        [Serializable]
        public struct ConditionLayer
        {
            /// <summary>개인 취약점 축 이름 또는 TiredCondition.</summary>
            public string condition;
            /// <summary>기본 초상과 같은 크기의 투명 레이어 그림.</summary>
            public Sprite sprite;
        }

        /// <summary>손님 한 명의 초상 묶음.</summary>
        [Serializable]
        public struct Entry
        {
            /// <summary>표시 이름과 같은 손님 이름 (예: 하루).</summary>
            public string guestName;
            /// <summary>항상 맨 아래에 보이는 기본 초상.</summary>
            public Sprite basePortrait;
            public ConditionLayer[] conditions;
        }

        [SerializeField] private Entry[] entries = Array.Empty<Entry>();

        public Entry[] Entries => entries;
    }
}
