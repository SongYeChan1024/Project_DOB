// =====================================================================
// GameManager — Gameplay 하루 진행의 진입점
// 역할   : 씬 조립이 끝난 뒤 하루를 연다. 하루 단계(DayCycle)·방명록(RelationshipManager)·하루 진행 화면(DayFlowPresenter)을
//          연결하고, 단계에 맞춰 응대·주방을 켜고, 마감 정산과 수동 저장을 처리한다.
//          금화·마늘 값은 주방(FirstGuestKitchen), 던전 재료는 EconomyManager, 방문 결과와 귀환 보고는 SettlementManager,
//          하루 단계는 DayCycle이 소유하며 여기서 따로 보관하지 않는다.
// 호출자 : BasementComposition (Prepare → 로딩 화면 닫힘 → StartService)
// 호출   : DayCycle, DayFlowPresenter, RelationshipManager, SettlementManager, EconomyManager, FirstGuestService,
//          FirstGuestKitchen, GuestManager(명단), ServicePauseReasons, FirstServiceSaveStore, BasementPauseMenu,
//          BasementComposition.EnterDay
// 상태   : 현재 일차의 DayCycle, 일차 시작 스냅샷, 새 진행 여부, 이번 일차의 정산 결과, 영업 정지 사유
// 의사코드:
//   Prepare  — 연결이 하나라도 없으면 아무것도 바꾸지 않고 실패. 주방·마감 버튼을 UI에 연결하고 시작 스냅샷으로 주방을 연다.
//              스냅샷의 던전 재료로 재고를, 미처리 귀환 보고로 오늘 아침 보고를 연다(새 게임은 둘 다 비어 있다).
//              귀환 보고 화면의 Presenter가 있으면 보고 조회와 신규 재료 판별을 넘겨 준다.
//              호감·입소문 기준이 잘못돼도 실패한다. 스냅샷의 전날 방문 호감·입소문 추가 인원·손님 명부를 관계 총괄에 넘긴다.
//              관계 총괄이 판정한 「오늘 올 수 있는 손님」과 입소문 신규 인원, 그 일차의 난수 시드를 손님 총괄에 넘긴다.
//              스냅샷에 시드가 없으면(새 게임·이전 스키마) 여기서 새로 정한다.
//              DayCycle·방명록·Presenter를 만들어 구독한다. 자동 슬롯이 없으면 1일차 시작을 저장한다(파일 오류는 경고만).
//   StartService — 로딩 화면이 닫힌 뒤 일시 정지 메뉴를 잇고 DayCycle.Begin(일차). 1일차는 영업, 이후는 아침보고부터.
//   단계 변경 — 준비가 되면 남은 귀환 보고를 모두 확정해 전리품을 재고에 한 번씩 반영한다(화면이 일부를 먼저 확정했어도 안전).
//              영업이 되면 응대·주방을 켠다.
//   서빙 알림 — 그 방문의 손님으로 방명록 글을 등록하고, 주방이 계산한 축별 부족으로 탐험 결과를 확정한다.
//              명단에 서빙(대기 점수 기록)과 파견 종료를 요청한다. 손님은 명단의 종료 알림으로 계단을 향한다.
//   Update   — 응대 화면·재료 선택(레시피 패널·조리 화면)·일시 정지·전환으로 정지 사유를 갱신한다.
//              조리 Δ(일시 정지·전환에만 멈춤)로 방명록 작성·조리 시간을, 영업 중이면 입장·대기 Δ(사유가 하나라도 있으면 멈춤)로
//              명단을 흘린다. 💡 영업 시간 제한은 없다. 수첩 사유는 화면이 생기면 연결한다(홍준우 인계).
//              영업 중 명단의 예정 방문이 모두 끝나고 방명록이 완성되면 DayCycle.TrySettle로 마감하고 바로 정산을 저장한다.
//              마감 확인 버튼은 마감 단계에서만 누를 수 있다.
//   SaveManual — 현재 진행이 아니라 보관해 둔 일차 시작 스냅샷(금화·마늘·재료·미처리 보고·전날 호감·입소문 인원·명부·시드)을 수동 슬롯에 쓴다.
//   Settle   — 정산 저장이 아직이면 다시 시도한다. 마감한 순간에 적어 둔 그날 서빙 인원이 데모 엔딩 기준(08.Data/SO_DayCycle)
//              이상이면 DayCycle.TryEndDemo로 엔딩 단계에 들어가 임시 엔딩 창을 띄우고, 미달이면 저장한 정산으로 다음 일차에 진입한다.
//              돌려보낸 손님·대기 이탈은 명단이 서빙 인원에 넣지 않는다. 엔딩 창의 [타이틀로]는 타이틀 복귀만 요청한다.
//              🔴 데모 완료 슬롯(엔딩을 본 일차의 시작을 보관)은 아직 없다. 정산은 평소처럼 다음 일차 시작을 자동 저장한다 (F13 후속).
//              정산에는 지금의 금화·마늘·던전 재료와 오늘의 방문 결과(다음 아침의 미처리 보고)가 들어간다.
//              스냅샷을 만들기 직전에 관계 총괄이 명단과 방문 결과로 방문 호감·다음 영업 추가 인원을 한 번 확정해 함께 넣는다.
//              같은 때 그날 손님 신원으로 명부(누적 호감·영구 거절·부상 휴식)를 한 번 확정하고, 다음 일차의 시드를 새로 정해 함께 넣는다.
//              정산 결과가 이미 있으면 재사용해 금화·전리품을 두 번 지급하지 않는다.
// 보고 도중에는 저장하지 않는다. 불러오면 일차 시작으로 돌아가 아침 보고를 처음부터 다시 받는다.
// 자동 저장은 당일 정산 뒤 다음 일차 시작 상태로만 갱신한다 (2026-11-24 송예찬 확인, PR #29).
// 🔴 수동 저장을 저장한 시점으로 할지 그 일차의 시작으로 할지는 기획 검토 중이다. 지금은 일차 시작이다.
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
        /// <summary>그날 손님 명단을 소유하는 손님 총괄.</summary>
        [SerializeField] private GuestManager guests;
        /// <summary>방명록과 방문 호감·입소문을 소유하는 관계 총괄.</summary>
        [SerializeField] private RelationshipManager relationship;
        /// <summary>방문 결과 기록과 귀환 보고를 소유하는 정산 총괄.</summary>
        [SerializeField] private SettlementManager settlement;
        /// <summary>던전 재료 재고를 소유하는 경제 총괄.</summary>
        [SerializeField] private EconomyManager economy;
        /// <summary>방명록 작성 시간 기준값(08.Data/SO_DayCycle).</summary>
        [SerializeField] private DayCycleSettings settings;

        private FirstServiceSaveStore saves;
        private BasementPauseMenu pauseMenu;
        private Button settleButton;
        private bool newTimeline;
        private FirstServiceSaveData settledCheckpoint;
        // 수동 저장이 일차 시작 상태를 그대로 다시 쓰도록 보관한다. 새 게임이면 null.
        private FirstServiceSaveData dayStart;
        private DayCycle cycle;
        private DayFlowPresenter dayFlow;
        // 귀환 보고 화면의 Presenter. 임시 보고 화면이 연결돼 있으면 null이다.
        private ReturnReportPresenter returnReport;
        private int startDay = 1;
        // 이 일차의 난수 시드. 같은 스냅샷으로 열면 같은 손님이 같은 순서로 온다.
        private int daySeed;
        // 다음 일차의 시드. 정산 저장을 다시 시도해도 같은 값을 쓴다.
        private int? nextDaySeed;
        // 마감한 순간의 서빙 인원. 명단은 식당 씬이 내려가면 비워지므로 따로 적어 둔다.
        private int servedToday;
        private BasementHud dayHud;
        private FirstGuestDialogue dialogue;
        private readonly ServicePauseReasons pauses = new ServicePauseReasons();

        /// <summary>현재 일차. 1부터 시작한다.</summary>
        public int Day => cycle?.Day ?? startDay;
        /// <summary>현재 하루 단계. StartService 전에는 의미가 없다.</summary>
        public DayPhase Phase => cycle?.Phase ?? DayPhase.MorningReport;
        /// <summary>로딩 화면이 닫히고 하루가 시작된 상태.</summary>
        public bool IsReady { get; private set; }

        private bool AllVisitsEnded => guests.Roster != null && guests.Roster.AllEnded && relationship.Guestbook.AllComplete;

        /// <summary>로딩 화면이 떠 있는 동안 하루를 준비한다. 연결이 빠졌으면 아무것도 바꾸지 않고 false.</summary>
        public bool Prepare(FirstServiceSaveStore store, FirstServiceSaveData start, bool startsNewTimeline, BasementHud hud)
        {
            // 일시 정지 메뉴는 BasementUI의 같은 Canvas에 붙어 있다 (BasementSceneBuilder.CreateHud).
            var menu = hud ? hud.GetComponent<BasementPauseMenu>() : null;
            if (!service || !kitchen || !guests || !relationship || !settlement || !economy || !settings || store == null || !menu
                || !hud.HasDayFlowViews)
                return false;
            if (guests.SetupError is string error)
            {
                Debug.LogError("[DOB] 영업 손님 기준을 확인하세요. " + error, guests);
                return false;
            }
            if (relationship.SetupError is string affinityError)
            {
                Debug.LogError("[DOB] 호감·입소문 기준을 확인하세요. " + affinityError, relationship);
                return false;
            }
            // 재고를 먼저 열어야 귀환 보고가 전리품을 넣을 곳이 생긴다. 보상 기준이 빠졌으면 여기서 실패한다.
            economy.BeginDay(start?.Materials);
            if (!settlement.BeginDay(start?.PendingReports, economy.Materials))
                return false;

            saves = store;
            pauseMenu = menu;
            newTimeline = startsNewTimeline;
            dayStart = start;
            startDay = start?.Day ?? 1;

            service.ConnectHud(hud.Guidance, hud.Dialogue, startDay);
            dialogue = hud.Dialogue;
            dayHud = hud;
            kitchen.ConnectHud(hud);
            settleButton = hud.Settle;
            settleButton.interactable = false;
            settleButton.onClick.AddListener(Settle);
            kitchen.StartDay(start?.Gold ?? kitchen.DayStartGold, start?.GarlicStock ?? kitchen.DayStartGarlic);
            kitchen.Served += RecordMeal;

            relationship.BeginDay(settings.GuestbookWriteSeconds, start?.VisitAffinities, start?.WordOfMouthBonus ?? 0, start?.Guests, startDay);
            daySeed = start?.DaySeed ?? Guid.NewGuid().GetHashCode();
            guests.PrepareDay(startDay, start?.WordOfMouthBonus ?? 0, relationship.GuestsComingToday, relationship.KnownGuestNames, daySeed);
            // 보고 화면은 UI 씬에 있어 정산 총괄을 직접 참조할 수 없다. 화면과 같은 프리팹에 있는 Presenter에 조회 함수를 넘겨 준다.
            returnReport = hud.MorningReport.transform.root.GetComponentInChildren<ReturnReportPresenter>(true);
            if (returnReport) returnReport.Bind(() => settlement.Reports, settlement.IsNewMaterial);
            cycle = new DayCycle();
            cycle.PhaseChanged += OnPhaseChanged;
            dayFlow = new DayFlowPresenter(cycle, relationship.Guestbook, hud.MorningReport, hud.Preparation, hud.Guestbook,
                () => (kitchen.Gold, kitchen.GarlicStock));
            try
            {
                if (!saves.TryLoad(FirstServiceSaveSlot.Auto, out _))
                    SaveDayStart(FirstServiceSaveSlot.Auto);
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
        public void SaveManual() => SaveDayStart(FirstServiceSaveSlot.Manual);

        // 재료·미처리 보고·전날 호감·입소문 인원은 일차 시작 스냅샷의 값을 쓴다. 아침에 이미 받은 전리품이 저장에 섞이면 불러온 뒤 두 번 받게 된다.
        private void SaveDayStart(FirstServiceSaveSlot slot) => saves.SavePreparation(slot, startDay, kitchen.DayStartGold,
            kitchen.DayStartGarlic, dayStart?.Materials, dayStart?.PendingReports, dayStart?.VisitAffinities, dayStart?.WordOfMouthBonus ?? 0,
            dayStart?.Guests, daySeed);

        /// <summary>마감 확인. 마감 단계에서만 정산을 저장(이미 했으면 재사용)하고, 서빙 인원이 기준 이상이면 데모 엔딩, 아니면 다음 일차로 진입한다.</summary>
        public void Settle()
        {
            if (!IsReady || BasementComposition.IsEntering || cycle.Phase != DayPhase.Closing || !SaveSettlement())
                return;
            if (cycle.TryEndDemo(servedToday, settings.DemoEndingServedGuests, out _))
                ShowDemoEnding();
            else
                BasementComposition.EnterDay(settledCheckpoint);
        }

        // shortcut: 코드로 만든 임시 창이다. 실제 엔딩 화면이 생기면 DayPhase.DemoEnding을 받는 View로 바꾼다 (홍준우 인계).
        private void ShowDemoEnding()
        {
            var view = new BasementPauseView(dayHud.transform, dayHud.Guidance.font);
            view.Begin("데모 엔딩");
            view.Label($"{Day}일차에 손님 {servedToday}명에게 서빙했습니다.\n금고 {kitchen.Gold}금", 23, 76);
            view.Action("Title", "타이틀로", () => BasementComposition.GoToTitle());
            view.Focus();
        }

        // 다음 씬을 열지 못하거나 저장이 실패해 다시 불려도, 이미 확정한 정산을 그대로 쓴다.
        private bool SaveSettlement()
        {
            if (settledCheckpoint != null)
                return true;
            try
            {
                // 새 게임 또는 수동 슬롯 선택 시, 첫 정산에 한해 이 플레이의 결과가 자동 슬롯을 이어받는다.
                var materials = economy.Materials.All();
                var visits = settlement.TodayResults;
                // 명단은 식당 씬이 내려갈 때 비워진다. 그 전에 한 번 확정하고, 저장 재시도는 보관한 값을 다시 받는다.
                var (affinities, bonus) = relationship.SettleDay(guests.Roster?.Visits ?? Array.Empty<ServiceVisit>(), visits, guests.TodaysGuests);
                var ledger = relationship.Guests;
                nextDaySeed = nextDaySeed ?? Guid.NewGuid().GetHashCode();
                settledCheckpoint = newTimeline
                    ? saves.SavePreparation(FirstServiceSaveSlot.Auto, checked(Day + 1), kitchen.Gold, kitchen.GarlicStock, materials, visits,
                        affinities, bonus, ledger, nextDaySeed)
                    : saves.SettleDay(Day, kitchen.Gold, kitchen.GarlicStock, materials, visits, affinities, bonus, ledger, nextDaySeed);
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
            // 아침 보고가 끝나면 화면이 하나씩 확정하지 않은 보고도 여기서 한 번씩 처리한다.
            if (phase == DayPhase.Preparation)
                settlement.ConfirmAllReports();
            if (phase == DayPhase.Service)
            {
                service.enabled = true;
                kitchen.enabled = true;
            }
        }

        private void RecordMeal(FirstGuestKitchen.ServedMeal meal)
        {
            var agent = guests.Find(meal.VisitId);
            var visit = agent ? agent.Visit : null;
            relationship.RecordMeal(visit?.GuestId, meal.VisitId, meal.GuestName, meal.Line);
            // 생성된 방문이 없으면 실제 위험을 알 수 없어 탐험 결과를 남기지 않는다.
            if (visit != null && meal.Shortfalls != null)
                settlement.RecordVisit(visit.GuestId, meal.VisitId, meal.GuestName, visit.Floor, meal.Dish, meal.UsedSeasoning,
                    meal.Shortfalls, meal.Risks, meal.Protections);
            // 서빙한 순간의 대기 점수를 남기고 파견으로 끝낸다. 손님 이동은 명단의 종료 알림을 받은 FirstGuestService가 한다.
            guests.Roster?.TryServe(meal.VisitId);
            guests.Roster?.TryEnd(meal.VisitId, ServiceVisitEnd.Dispatched);
        }

        private void Update()
        {
            if (!IsReady)
                return;
            pauses.Set(ServicePauseReason.Dialogue, dialogue && dialogue.BlocksWorldInput);
            pauses.Set(ServicePauseReason.Ingredients, kitchen.IsChoosingIngredients || UIManager.IsKitchenOpen);
            pauses.Set(ServicePauseReason.PauseMenu, BasementPauseMenu.IsPaused);
            pauses.Set(ServicePauseReason.Transition, BasementComposition.IsEntering || SceneLoadService.IsLoading);
            bool paused = pauses.StopsCooking;
            // 방명록 작성은 조리와 같은 시계다. 응대·재료 선택 중에도 흐르고 일시 정지·전환에만 멈춘다.
            float clock = paused ? 0f : Time.deltaTime;
            relationship.Tick(clock);
            if (cycle.Phase == DayPhase.Service)
            {
                kitchen.Tick(clock);
                guests.Tick(pauses.StopsArrivalAndWait ? 0f : Time.deltaTime);
            }

            // 마지막 방문이 끝난 순간 한 번 마감·정산한다.
            if (cycle.Phase == DayPhase.Service && cycle.TrySettle(AllVisitsEnded, out _))
            {
                servedToday = guests.Roster.ServedGuestCount;
                SaveSettlement();
            }

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
            if (returnReport) returnReport.Bind(null, null);
            returnReport = null;
            if (settleButton) settleButton.onClick.RemoveListener(Settle);
            if (pauseMenu) pauseMenu.SaveRequested -= SaveManual;
            if (kitchen) kitchen.enabled = false;
            if (service) service.enabled = false;
        }
    }
}
