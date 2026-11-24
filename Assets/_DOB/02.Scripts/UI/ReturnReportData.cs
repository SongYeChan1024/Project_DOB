// =====================================================================
// ReturnReportData — 아침 귀환 보고 화면에 보여 줄 표시 데이터
// 역할   : 화면이 그릴 값만 담는다. 결과 등급 · 부족 칸 · 전리품 · 호감 · 수첩 기록 · 그날 일러스트 등급은
//          모두 백엔드(SettlementManager · RelationshipManager)가 정해서 넘긴다. 화면은 계산하지 않는다.
// 호출자 : ReturnReportPresenter가 채워서 ReturnReportView.ShowReport에 넘긴다.
// 상태   : 없음 (읽기 전용 값 묶음)
// =====================================================================
using System;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>귀환 결과 등급 (기획 4-3: 부족 합 0 · 1 · 2 · 3 이상 = 대성공 · 성공 · 간신히 · 부상).
    /// 이름 순서는 백엔드 ExpeditionGrade(GreatSuccess · Success · Failure · GreatFailure)와 같게 둔다.</summary>
    public enum ReturnResult { GreatSuccess, Success, Failure, GreatFailure }

    /// <summary>아침 귀환 보고 화면 전체.</summary>
    [Serializable]
    public sealed class ReturnReportViewData
    {
        /// <summary>그날 총합 점수 등급 1~5 (1이 가장 높음). 왼쪽 큰 일러스트를 고른다. 0이면 일러스트 없음.</summary>
        public int illustrationTier;
        /// <summary>돌아온 손님 (돌아온 순서). 비면 "어젯밤 던전에 간 손님이 없어요".</summary>
        public ReturnGuestData[] guests = Array.Empty<ReturnGuestData>();
    }

    /// <summary>손님 한 명의 보고 (기획 4-7).</summary>
    [Serializable]
    public sealed class ReturnGuestData
    {
        public string guestId;
        public string visitId;
        public string name;
        /// <summary>얼굴 일러스트. 없으면 빈 칸 테두리만.</summary>
        public Sprite face;
        /// <summary>예: 인간 · 마법사</summary>
        public string raceJob;
        /// <summary>예: 1층 슬라임 숲에 다녀왔어요</summary>
        public string trip;
        public ReturnResult result;
        /// <summary>손님 한마디 (가장 모자랐던 축을 손님 말로).</summary>
        public string line;
        /// <summary>위험 축마다 결과. 돌발 줄도 여기에 함께 (surprise).</summary>
        public AxisResultData[] axes = Array.Empty<AxisResultData>();
        /// <summary>가져온 재료 (결과 등급만큼). 부상이면 비움.</summary>
        public LootData[] loot = Array.Empty<LootData>();
        /// <summary>수첩에 적은 층 이름 (예: 1층 슬라임 숲). 새로 적은 게 없으면 비움.</summary>
        public string noteFloor;
        /// <summary>수첩에 새로 적은 층 위험 (최대 두 축, 모자랐던 축 먼저).</summary>
        public NoteAxisData[] noteAxes = Array.Empty<NoteAxisData>();
        /// <summary>이번 방문 호감 변화 (예: +3).</summary>
        public int affectionDelta;
        /// <summary>예: 대기 +1 · 맛 +1 · 던전 +1</summary>
        public string affectionWhy;
        /// <summary>예: 단골까지 6</summary>
        public string affectionNext;
        /// <summary>다음 목표까지 채운 비율 0~1 (막대).</summary>
        public float affectionRatio;
    }

    /// <summary>위험 한 축의 결과 줄.</summary>
    [Serializable]
    public sealed class AxisResultData
    {
        /// <summary>독 · 열 · 냉기 · 어둠 · 체력 · 함정</summary>
        public string risk;
        /// <summary>모자란 칸 수 (백엔드 VisitResult.Shortfalls). 0이면 "막아냈다".</summary>
        public int shortfall;
        /// <summary>그날 실제 위험 칸 수 · 요리로 막은 칸 수. 모르면 -1 (칸 그림을 숨김).</summary>
        public int need = -1;
        public int have = -1;
        /// <summary>돌발 줄이면 설명 (예: 돌발 · 활력 여유로 버텼다). 보통 줄은 비움.</summary>
        public string surprise;
        /// <summary>돌발 줄에서 막아냈는지 (보통 줄은 shortfall로 판단).</summary>
        public bool surpriseHeld;
    }

    /// <summary>가져온 재료 하나.</summary>
    [Serializable]
    public sealed class LootData
    {
        public string id;
        public string name;
        public Sprite icon;
        /// <summary>처음 얻은 재료면 "신규" 표시.</summary>
        public bool isNew;
    }

    /// <summary>수첩에 새로 적은 층 위험 하나.</summary>
    [Serializable]
    public sealed class NoteAxisData
    {
        public string risk;
        public int amount;
    }
}
