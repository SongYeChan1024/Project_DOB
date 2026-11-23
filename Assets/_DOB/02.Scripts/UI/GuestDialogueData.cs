// =====================================================================
// GuestDialogueData — 손님 응대 화면에 보여 줄 표시 데이터 (임시)
// 역할   : 응대 화면이 그릴 값만 담는다. 게임 규칙·판정 값은 담지 않는다.
// 호출자 : GuestDialoguePresenter가 채워서 GuestDialogueView.Open에 넘긴다.
// 상태   : 없음 (읽기 전용 값 묶음)
// 비고   : 김언호와 표시 데이터 계약(GuestView DTO)을 정하면 DOB.Define의 계약 타입으로 옮긴다.
// =====================================================================
using System;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>응대 화면 한 번에 필요한 표시 값. 알 수 없는 위험 축은 -1로 둔다.</summary>
    [Serializable]
    public sealed class GuestDialogueData
    {
        /// <summary>판정·저장 연결용 손님 식별자. 화면은 그대로 돌려보내기만 한다.</summary>
        public string guestId;
        /// <summary>표시 이름. 단골이면 [단골] 태그를 붙인다.</summary>
        public string guestName;
        /// <summary>종족 · 직업 표시 문구 (예: "인간 · 마법사").</summary>
        public string raceJob;
        public bool isRegular;
        /// <summary>목적지 간판 문구 (예: "1층 슬라임 숲").</summary>
        public string destinationLabel;
        /// <summary>주문 대사. [[ ]]로 감싼 부분이 밑줄 단서가 된다.</summary>
        public string line;
        /// <summary>관찰 단서. 생김새 설명만 담고 뜻·수치는 담지 않는다.</summary>
        public ClueMarkerData[] clues = Array.Empty<ClueMarkerData>();
        /// <summary>위험 6축 순서(독·열·냉기·어둠·체력·함정)의 수첩 기록 값. -1은 모름('?').</summary>
        public int[] knownRisks = { -1, -1, -1, -1, -1, -1 };
        /// <summary>플레이어 메모 상태. 0 비움 · 1 있음 · 2 없음. 판정에 쓰지 않는다.</summary>
        public int[] memos = new int[6];
        /// <summary>튜토리얼 등에서 돌려보내기 버튼을 숨길 때 false.</summary>
        public bool canSendHome = true;
    }

    /// <summary>초상 위 돋보기 표식 하나. 위치는 초상 영역 기준 픽셀 좌표.</summary>
    [Serializable]
    public sealed class ClueMarkerData
    {
        public string description;
        public Vector2 position;
    }
}
