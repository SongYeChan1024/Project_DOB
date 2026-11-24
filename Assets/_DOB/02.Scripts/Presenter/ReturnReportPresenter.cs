// =====================================================================
// ReturnReportPresenter — 아침 귀환 보고 화면에 그날 손님별 보고를 채운다
// 역할   : 보고 화면이 열리면(Opened) 어젯밤 던전에 간 손님들의 보고 데이터를 만들어 ShowReport로 넘긴다.
//          보고 화면 열기 · 닫기와 [준비하러 가기]는 DayFlowPresenter가 맡는다 (팀 교체 지점 규칙).
// 호출자 : ReturnReportView.Opened
// 호출   : ReturnReportView.ShowReport
// 상태   : 없음. 더미 단계에서는 시안과 같은 손님 3명을 매번 만든다.
// 비고   : TODO(계약) 표시된 곳이 백엔드 값으로 바뀔 자리 (Docs/개발/FeatureManagerMap.md F9 · F10).
//          결과 등급 · 축별 부족 · 전리품 · 층 위험 기록 · 그날 일러스트 등급 → SettlementManager (ReturnResolver · FloorProgress)
//          호감 변화 · 단골까지 남은 값 → RelationshipManager (Affinity)
//          🔴 그날 총합 점수 계산식과 일러스트 1~5 구간은 미정 (송예찬 · 김언호와 정하기)
//          연결 예정 (PR #29, 김언호): SettlementManager.Reports(VisitResult 목록) · IsReportConfirmed · ConfirmReport(visitId)
//            VisitResult → 이 화면: GuestId·VisitId·GuestName 그대로 / Grade(ExpeditionGrade) → ReturnResult (이름 순서 같음)
//            Shortfalls(독·열·냉기·어둠·체력·함정 순서) → 0보다 큰 축만 "N칸 모자랐다" 줄 / Loot(재료 이름) → 그림 · 신규 표시
//            아직 계약에 없는 것: 축별 실제 위험 · 요리 칸(→ 지금은 -1로 칸 숨김), 한마디, 수첩 기록, 호감 변화, 얼굴, 그날 일러스트 등급
//            확정: 화면은 확정하지 않아도 된다 (준비로 넘어갈 때 백엔드가 남은 보고를 모두 한 번씩 확정)
//
// 의사코드
// [켜질 때]   화면의 Opened를 구독한다 / [꺼질 때] 구독을 푼다
// [열림]      그날 보고를 만든다 → 화면에 넘긴다
// =====================================================================
using System;
using System.Linq;
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>귀환 보고 화면에 손님별 보고를 채운다. 판정 · 보상은 계산하지 않는다.</summary>
    public sealed class ReturnReportPresenter : MonoBehaviour
    {
        [SerializeField] private ReturnReportView view;
        /// <summary>재료 그림 (07.UI/Sprites/Items). 파일 이름으로 찾는다 (예: T_UI_IngSlimeJelly).</summary>
        [SerializeField] private Sprite[] itemIcons = Array.Empty<Sprite>();

        private void OnEnable()
        {
            if (view) view.Opened += OnOpened;
        }

        private void OnDisable()
        {
            if (view) view.Opened -= OnOpened;
        }

        private void OnOpened(int day)
        {
            // TODO(계약) SettlementManager에서 이번 아침 보고(전날 파견한 손님별 확정 결과)를 받기
            view.ShowReport(new ReturnReportViewData
            {
                illustrationTier = 2, // TODO(계약) 그날 총합 점수 등급 1~5
                guests = new[] { Haru(), Bori(), Sol() },
            });
        }

        // ---------------- 더미 (기획 4-3 판정 · 4-7 보고 · 4-8 호감 · 10장 값으로 맞춤) ----------------

        // 인간(약점 없음) · 마법사 → 1층 기본 위험 독1 · 체력1. 부족 합 1 = 성공 → 전리품 2개
        private ReturnGuestData Haru() => new ReturnGuestData
        {
            guestId = "test_haru", visitId = "visit_1", name = "하루",
            raceJob = "인간 · 마법사", trip = "1층 슬라임 숲에 다녀왔어요",
            result = ReturnResult.Success,
            line = "점액이 좀 따끔했지만, 덕분에 끄떡없었어요!",
            axes = new[]
            {
                new AxisResultData { risk = "독", need = 1, have = 2 },
                new AxisResultData { risk = "체력", shortfall = 1, need = 1, have = 0 },
                new AxisResultData { risk = "함정", need = 1, have = 0, surprise = "돌발 · 활력 여유로 버텼다", surpriseHeld = true },
            },
            loot = new[] { Loot("SlimeJelly", "슬라임 젤리", false), Loot("ForestMushroom", "숲버섯", true) },
            noteFloor = "1층 슬라임 숲",
            noteAxes = new[] { Note("체력", 1), Note("독", 1) },
            affectionDelta = 3, affectionWhy = "대기 +1 · 맛 +1 · 던전 +1",
            affectionNext = "단골까지 6", affectionRatio = 3f / 9f,
        };

        // 수인(독 약함 +1) · 광부(어둠 강함 −1) → 2층 독3 · 어둠0 · 함정1. 부족 2+1=3 = 부상 → 전리품 없음
        private ReturnGuestData Bori() => new ReturnGuestData
        {
            guestId = "test_bori", visitId = "visit_2", name = "보리",
            raceJob = "수인 · 광부", trip = "2층 곰팡이 동굴에 다녀왔어요",
            result = ReturnResult.GreatFailure,
            line = "포자 때문에 숨이 막혀서… 이틀은 쉬어야겠어요.",
            axes = new[]
            {
                new AxisResultData { risk = "독", shortfall = 2, need = 3, have = 1 },
                new AxisResultData { risk = "함정", shortfall = 1, need = 1, have = 0 },
            },
            noteFloor = "2층 곰팡이 동굴",
            noteAxes = new[] { Note("독", 2), Note("함정", 1) },
            affectionDelta = 1, affectionWhy = "대기 +1 · 맛 0 · 던전 0",
            affectionNext = "단골까지 4", affectionRatio = 5f / 9f,
        };

        // 하플링(체력 약함 +1) · 궁수 → 1층 독1 · 체력2. 모두 막음 = 대성공 → 전리품 3개
        private ReturnGuestData Sol() => new ReturnGuestData
        {
            guestId = "test_sol", visitId = "visit_3", name = "솔",
            raceJob = "하플링 · 궁수", trip = "1층 슬라임 숲에 다녀왔어요",
            result = ReturnResult.GreatSuccess,
            line = "배가 든든하니까 하나도 안 지치던걸요!",
            axes = new[]
            {
                new AxisResultData { risk = "독", need = 1, have = 1 },
                new AxisResultData { risk = "체력", need = 2, have = 2 },
            },
            loot = new[] { Loot("Herb", "들풀", true), Loot("SlimeJelly", "슬라임 젤리", false), Loot("ForestCarrot", "숲 당근", true) },
            affectionDelta = 4, affectionWhy = "대기 +1 · 맛 +1 · 던전 +2",
            affectionNext = "단골까지 1", affectionRatio = 8f / 9f,
        };

        private LootData Loot(string id, string name, bool isNew) => new LootData
        {
            id = id, name = name, isNew = isNew,
            icon = itemIcons.FirstOrDefault(sp => sp && sp.name == "T_UI_Ing" + id),
        };

        private static NoteAxisData Note(string risk, int amount) => new NoteAxisData { risk = risk, amount = amount };
    }
}
