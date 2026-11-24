// =====================================================================
// GameManager — Gameplay 하루 진행의 진입점
// 역할   : 씬 조립이 끝난 뒤 하루를 연다. 하루 단계(DayCycle)·방명록(RelationshipManager)·하루 진행 화면(DayFlowPresenter)을
//          연결하고, 단계에 맞춰 응대·주방을 켜고, 마감 정산과 수동 저장을 처리한다.
//          금화·재고 값은 주방(FirstGuestKitchen), 하루 단계는 DayCycle이 소유하며 여기서 따로 보관하지 않는다.
// 호출자 : BasementComposition (Prepare → 로딩 화면 닫힘 → StartService)
// 호출   : DayCycle, DayFlowPresenter, RelationshipManager, FirstGuestService, FirstGuestKitchen,
//          FirstServiceSaveStore, BasementPauseMenu, BasementComposition.EnterDay
// 상태   : 현재 일차의 DayCycle, 새 진행 여부, 이번 일차의 정산 결과
// 의사코드:
//   Prepare  — 연결이 하나라도 없으면 아무것도 바꾸지 않고 실패. 주방·마감 버튼을 UI에 연결하고 시작 스냅샷으로 주방을 연다.
//              DayCycle·방명록·Presenter를 만들어 구독한다. 자동 슬롯이 없으면 1일차 시작을 저장한다(파일 오류는 경고만).
//   StartService — 로딩 화면이 닫힌 뒤 일시 정지 메뉴를 잇고 DayCycle.Begin(일차). 1일차는 영업, 이후는 아침보고부터.
//   단계 변경 — 영업이 되면 응대·주방을 켠다.
//   Update   — 일시 정지·전환 중이 아니면 방명록 작성 시간을 흘린다. 💡 영업 시계는 없다.
//              영업 중 오늘 손님의 모든 방문이 끝나면 DayCycle.TrySettle로 마감하고 바로 정산을 저장한다.
//              마감 확인 버튼은 마감 단계에서만 누를 수 있다.
//   Settle   — 정산 저장이 아직이면 다시 시도하고, 저장한 정산으로 다음 일차에 진입한다.
//              정산 결과가 이미 있으면 재사용해 금화를 두 번 지급하지 않는다.
//   OnDisable — 구독을 풀고 응대·주방을 끈다.
// =====================================================================
using System;
using System.IO;
using DOB.Define;
using DOB.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.Manager
{
    /// <summary>게임 베이스에서 하루 한 판의 단계·저장·정산을 맡는다. 다른 Manager는 이 클래스를 참조하지 않는다.</summary>
    [DisallowMultipleComponent]
    public sealed class GameManager : MonoBehaviour
    {
        /// <summary>손님 입장·응대·착석을 진행하는 첫 손님 서비스.</summary>
        [SerializeField] private FirstGuestService service;
        /// <summary>주문·조리·서빙과 영업 중 금화·재고를 소유하는 첫 손님 주방.</summary>
        [SerializeField] private FirstGuestKitchen kitchen;
        /// <summary>방명록을 소유하는 관계 총괄.</summary>
        [SerializeField] private RelationshipManager relationship;
        /// <summary>방명록 작성 시간 기준값(08.Data/SO_DayCycle).</summary>
        [SerializeField] private DayCycleSettings settings;

        private FirstServiceSaveStore saves;
        private BasementPauseMenu pauseMenu;
        private Button settleButton;
        private bool newTimeline;
        private FirstServiceSaveData settledCheckpoint;
        private DayCycle cycle;
        private DayFlowPresenter dayFlow;
        private int startDay = 1;

        /// <summary>현재 일차. 1부터 시작한다.</summary>
        public int Day => cycle?.Day ?? startDay;
        /// <summary>현재 하루 단계. StartService 전에는 의미가 없다.</summary>
        public DayPhase Phase => cycle?.Phase ?? DayPhase.MorningReport;
        /// <summary>로딩 화면이 닫히고 하루가 시작된 상태.</summary>
        public bool IsReady { get; private set; }

        // shortcut: 손님이 첫 손님 한 명뿐이라 '돌려보냈거나 식사를 마치고 떠났고 방명록이 완성됨'을 오늘의 모든 방문 종료로 본다.
        // 입소문으로 정한 그날 손님 수가 생기면 GuestManager가 방문 목록으로 판정하도록 바꾼다.
        private bool AllVisitsEnded => service.HasLeft && relationship.Guestbook.AllComplete;

        /// <summary>로딩 화면이 떠 있는 동안 하루를 준비한다. 연결이 빠졌으면 아무것도 바꾸지 않고 false.</summary>
        public bool Prepare(FirstServiceSaveStore store, (int Day, int Gold, int GarlicStock)? start, bool startsNewTimeline, BasementHud hud)
        {
            // 일시 정지 메뉴는 BasementUI의 같은 Canvas에 붙어 있다 (BasementSceneBuilder.CreateHud).
            var menu = hud ? hud.GetComponent<BasementPauseMenu>() : null;
            if (!service || !kitchen || !relationship || !settings || store == null || !menu || !hud.HasDayFlowViews)
                return false;

            saves = store;
            pauseMenu = menu;
            newTimeline = startsNewTimeline;
            startDay = start?.Day ?? 1;

            service.ConnectHud(hud.Guidance, hud.Dialogue);
            kitchen.ConnectHud(hud);
            settleButton = hud.Settle;
            settleButton.interactable = false;
            settleButton.onClick.AddListener(Settle);
            kitchen.StartDay(start?.Gold ?? kitchen.DayStartGold, start?.GarlicStock ?? kitchen.DayStartGarlic);
            kitchen.Served += RecordMeal;

            relationship.BeginDay(settings.GuestbookWriteSeconds);
            cycle = new DayCycle();
            cycle.PhaseChanged += OnPhaseChanged;
            dayFlow = new DayFlowPresenter(cycle, relationship.Guestbook, hud.MorningReport, hud.Preparation, hud.Guestbook,
                () => (kitchen.Gold, kitchen.GarlicStock));
            try
            {
                if (!saves.TryLoad(FirstServiceSaveSlot.Auto, out _))
                    saves.SavePreparation(FirstServiceSaveSlot.Auto, startDay, kitchen.DayStartGold, kitchen.DayStartGarlic);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Debug.LogWarning("[DOB] 첫 자동 저장을 만들 수 없습니다. " + e.Message, this);
            }
            return true;
        }

        /// <summary>로딩 화면이 닫힌 뒤 하루를 시작한다.</summary>
        public void StartService()
        {
            // Begin 전에는 DayCycle.Day가 0이므로 일차를 정한 뒤 메뉴에 넘긴다.
            cycle.Begin(startDay);
            pauseMenu.Initialize(saves, Day);
            pauseMenu.SaveRequested += SaveManual;
            IsReady = true;
        }

        /// <summary>현재 영업을 계속하면서 그 일차의 시작 스냅샷을 수동 슬롯에 기록한다. 파일 오류는 호출자에게 전달한다.</summary>
        public void SaveManual() => saves.SavePreparation(FirstServiceSaveSlot.Manual, Day, kitchen.DayStartGold, kitchen.DayStartGarlic);

        /// <summary>마감 확인. 마감 단계에서만 정산을 저장(이미 했으면 재사용)하고 다음 일차로 진입한다.</summary>
        public void Settle()
        {
            if (!IsReady || BasementComposition.IsEntering || cycle.Phase != DayPhase.Closing || !SaveSettlement())
                return;
            BasementComposition.EnterDay(settledCheckpoint);
        }

        // 다음 씬을 열지 못하거나 저장이 실패해 다시 불려도, 이미 확정한 정산을 그대로 쓴다.
        private bool SaveSettlement()
        {
            if (settledCheckpoint != null)
                return true;
            try
            {
                // 새 게임 또는 수동 슬롯 선택 시, 첫 정산에 한해 이 플레이의 결과가 자동 슬롯을 이어받는다.
                settledCheckpoint = newTimeline
                    ? saves.SavePreparation(FirstServiceSaveSlot.Auto, checked(Day + 1), kitchen.Gold, kitchen.GarlicStock)
                    : saves.SettleDay(Day, kitchen.Gold, kitchen.GarlicStock);
                return true;
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Debug.LogWarning("[DOB] 영업 정산을 저장할 수 없습니다. 마감 확인으로 다시 시도합니다. " + e.Message, this);
                return false;
            }
        }

        private void OnPhaseChanged(DayPhase phase)
        {
            if (phase == DayPhase.Service)
            {
                service.enabled = true;
                kitchen.enabled = true;
            }
        }

        private void RecordMeal(string visitId, string guestName, string line) =>
            relationship.RecordMeal(service.GuestId, visitId, guestName, line);

        private void Update()
        {
            if (!IsReady)
                return;
            // 겹친 정지 사유는 F2에서 넣는다. 지금은 일시 정지 메뉴와 전환만 방명록 작성 시간을 멈춘다.
            bool paused = BasementPauseMenu.IsPaused || BasementComposition.IsEntering;
            relationship.Tick(paused ? 0f : Time.deltaTime);

            // 마지막 방문이 끝난 순간 한 번 마감·정산한다.
            if (cycle.Phase == DayPhase.Service && cycle.TrySettle(AllVisitsEnded, out _))
                SaveSettlement();

            if (settleButton)
                settleButton.interactable = cycle.Phase == DayPhase.Closing && !paused;
        }

        private void OnDisable()
        {
            IsReady = false;
            dayFlow?.Dispose();
            dayFlow = null;
            if (cycle != null) cycle.PhaseChanged -= OnPhaseChanged;
            if (kitchen) kitchen.Served -= RecordMeal;
            if (settleButton) settleButton.onClick.RemoveListener(Settle);
            if (pauseMenu) pauseMenu.SaveRequested -= SaveManual;
            if (kitchen) kitchen.enabled = false;
            if (service) service.enabled = false;
        }
    }
}
