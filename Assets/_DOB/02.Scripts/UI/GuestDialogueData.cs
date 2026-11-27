// =====================================================================
// GuestDialogueData — 손님 응대 화면에 보여 줄 표시 데이터
// 역할   : 응대 화면이 그릴 값만 담는다. 게임 규칙 · 판정 값은 담지 않는다.
// 호출자 : Presenter가 채워서 GuestDialogueView.Open에 넘긴다.
// 상태   : 없음 (읽기 전용 값 묶음)
// 비고   : 김언호와 표시 데이터 계약(GuestView DTO)을 정하면 DOB.Define의 계약 타입으로 옮긴다.
// =====================================================================
using System;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>응대 화면 한 번에 필요한 표시 값.</summary>
    [Serializable]
    public sealed class GuestDialogueData
    {
        // ---------------- 손님 ----------------

        /// <summary>판정 · 저장 연결용 손님 식별자. 화면은 그대로 돌려보내기만 한다.</summary>
        public string guestId;
        /// <summary>표시 이름. 단골이면 [단골] 태그를 붙인다.</summary>
        public string guestName;
        /// <summary>종족 · 직업 표시 문구 (예: "인간 · 마법사").</summary>
        public string raceJob;
        /// <summary>종족 도감에 적을 이름 (예: 인간). 비어 있으면 raceJob의 앞부분을 쓴다.</summary>
        public string race;
        /// <summary>직업 도감에 적을 이름 (예: 마법사). 비어 있으면 raceJob의 뒷부분을 쓴다.</summary>
        public string job;
        public bool isRegular;
        /// <summary>목적지 간판 문구 (예: "1층 슬라임 숲"). 층별 위험 도감의 이름으로도 쓴다.</summary>
        public string destinationLabel;
        /// <summary>기본 초상 그림. null이면 프리팹의 「그림 대기」 상자를 그대로 보여 준다.</summary>
        public Sprite portrait;
        /// <summary>기본 초상 위 같은 자리에 겹칠 컨디션 레이어 (앞쪽이 아래). 기본 초상이 없으면 쓰지 않는다.</summary>
        public Sprite[] portraitLayers = Array.Empty<Sprite>();

        // ---------------- 대사 · 단서 ----------------

        /// <summary>주문 대사. [[ ]]로 감싼 부분이 숨은 키워드가 된다 (밑줄은 찾은 뒤에만 보임).</summary>
        public string line;
        /// <summary>대사 키워드의 정답 정보 (어떤 위험인지, 도감 어디에 적을지).</summary>
        public ClueKeywordData[] keywords = Array.Empty<ClueKeywordData>();
        /// <summary>초상화의 숨은 단서 자리.</summary>
        public ClueMarkerData[] clues = Array.Empty<ClueMarkerData>();

        /// <summary>
        /// 수첩 기록. 비워 두면 이번 플레이 동안 함께 쓰는 공용 기록(NotebookRecord.Session)을 쓴다.
        /// </summary>
        [NonSerialized] public NotebookRecord notebook;
        // 생성된 방문에서만 제공한다. 같은 원인의 위험 보정은 방문에서 한 번만 적용한다.
        [NonSerialized] public Func<string, bool> discover;
        [NonSerialized] public Func<string, int?> notebookRisk;

        /// <summary>튜토리얼 등에서 돌려보내기 버튼을 숨길 때 false.</summary>
        public bool canSendHome = true;

        // ---------------- 예전 방식 (호환용, 지금 화면은 안 씀) ----------------

        /// <summary>[예전] 위험 6축 기록 값. 지금은 찾은 단서로 위험 칸을 채운다.</summary>
        public int[] knownRisks = { -1, -1, -1, -1, -1, -1 };
        /// <summary>[예전] 메모 버튼 값. 메모 버튼은 없어졌다.</summary>
        public int[] memos = new int[6];
    }

    /// <summary>대사 속 숨은 키워드 하나의 정답.</summary>
    [Serializable]
    public sealed class ClueKeywordData
    {
        public string key;
        /// <summary>원인ID. 같은 원인을 가리키는 표현끼리 같은 값 (수첩에서 첫 표현 아래에 묶는다). 비면 묶지 않음.</summary>
        public string cause;
        public int delta;
        /// <summary>대사의 [[ ]] 안 글자와 똑같아야 한다 (예: 슬라임 점액).</summary>
        public string phrase;
        /// <summary>위험 이름 (독 · 열 · 냉기 · 어둠 · 체력 · 함정).</summary>
        public string risk;
        /// <summary>찾았을 때 도감 어디에도 적을지.</summary>
        public ClueRecordTarget recordTo;
    }

    /// <summary>초상화의 숨은 단서 자리 하나. 위치는 초상 영역 기준 픽셀 좌표.</summary>
    [Serializable]
    public sealed class ClueMarkerData
    {
        public string key;
        /// <summary>원인ID. 같은 원인을 가리키는 표현끼리 같은 값 (수첩에서 첫 표현 아래에 묶는다). 비면 묶지 않음.</summary>
        public string cause;
        public int delta;
        /// <summary>찾은 뒤 마우스를 올리면 보이는 생김새 설명.</summary>
        public string description;
        public Vector2 position;
        /// <summary>태그에 보일 짧은 이름 (예: 젖은 장화). 비어 있으면 description을 쓴다.</summary>
        public string tagName;
        /// <summary>위험 이름 (독 · 열 · 냉기 · 어둠 · 체력 · 함정).</summary>
        public string risk;
        /// <summary>찾았을 때 도감 어디에도 적을지.</summary>
        public ClueRecordTarget recordTo;
    }
}
