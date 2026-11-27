// =====================================================================
// GameManager — Gameplay 하루 진행의 진입점
// 역할   : 씬 조립이 끝난 뒤 하루를 연다. 하루 단계(DayCycle)·방명록(RelationshipManager)·하루 진행 화면(DayFlowPresenter)을
//          연결하고, 단계에 맞춰 응대·주방을 켜고, 마감 정산과 수동 저장을 처리한다.
//          금화·양념·던전 재료는 EconomyManager, 방문 결과·귀환 보고·층 진행은 SettlementManager, 마감 정산 저장은 SaveManager,
//          레시피 기준·열린 레시피는 KitchenManager, 하루 단계는 DayCycle이 소유하며 여기서 따로 보관하지 않는다.
// 호출자 : BasementComposition (Prepare → 로딩 화면 닫힘 → StartService)
// 호출   : DayCycle, DayFlowPresenter, RelationshipManager, SettlementManager, EconomyManager, FirstGuestService,
//          FirstGuestKitchen, GuestManager(명단), ServicePauseReasons, SaveManager, FirstServiceSaveStore, BasementPauseMenu,
//          BasementComposition.EnterDay
// 상태   : 현재 일차의 DayCycle, 일차 시작 스냅샷, 새 진행 여부, 영업 정지 사유 (정산 결과는 SaveManager가 보관)
// 의사코드:
//   Prepare  — 연결이 하나라도 없으면 아무것도 바꾸지 않고 실패. 주방·마감 버튼을 UI에 연결하고 시작 스냅샷으로 주방을 연다.
//              스냅샷의 금화·양념·던전 재료로 경제를(새 게임은 시장 기준의 시작 값), 미처리 귀환 보고·층 진행으로 오늘 아침 보고를 연다.
//              시장은 준비 단계에서만 열리게 연결한다.
//              귀환 보고 화면의 Presenter가 있으면 보고 조회와 신규 재료 판별을 넘겨 준다.
//              호감·입소문 기준이 잘못돼도 실패한다. 스냅샷의 전날 방문 호감·입소문 추가 인원·손님 명부를 관계 총괄에 넘긴다.
//              관계 총괄이 판정한 「오늘 올 수 있는 손님」과 입소문 신규 인원, 그 일차의 난수 시드를 손님 총괄에 넘긴다.
//              스냅샷에 시드가 없으면(새 게임·이전 스키마) 여기서 새로 정한다.
//              DayCycle·방명록·Presenter를 만들어 구독한다. 자동 슬롯이 없으면 1일차 시작을 저장한다(파일 오류는 경고만).
//   StartService — 로딩 화면이 닫힌 뒤 일시 정지 메뉴를 잇고 DayCycle.Begin(일차). 1일차는 영업, 이후는 아침보고부터.
//              스냅샷에 수동 저장의 진행이 있으면 저장된 확정 보고를 다시 확정하고(전리품은 시작 재고에 한 번씩 더해진다) 저장된 단계까지 넘긴다.
//              영업 단계였으면 식당이 준비돼 명단이 열린 첫 Update에서 명단·좌석·주문·금화·마늘·방명록·방문 결과를 되살린다. 그 전에는 시계를 흘리지 않는다.
//   단계 변경 — 준비가 되면 남은 귀환 보고를 모두 확정해 전리품을 재고에 한 번씩 반영한다(화면이 일부를 먼저 확정했어도 안전).
//              영업이 되면 응대·주방을 켠다.
//   성공 알림 — 입장·조리 완성·서빙·금화·보고 확정을 SoundManager 효과음으로 잇는다(불러오기 복원 중의 보고 재확정은 소리 없음).
//              보고 확정 뒤에는 KitchenManager가 새로 들어온 재료로 레시피를 연다.
//   서빙 알림 — 그 방문의 손님으로 방명록 글을 등록하고, 주방이 계산한 축별 부족으로 탐험 결과를 확정한다.
//              손님 입맛과 접시(요리·양념)로 입맛 점수를 정해 명단에 서빙(대기·입맛 점수 기록)과 파견 종료를 요청한다.
//              손님은 명단의 종료 알림으로 계단을 향한다.
//   Update   — 일시 정지·전환으로 정지 사유를 갱신한다. 응대·수첩·재료 선택은 정지 사유가 아니다(F2 노션 확정: ESC만 정지).
//              같은 Δ(사유가 하나라도 있으면 0)로 방명록 작성·조리 시간과, 영업 중이면 명단의 입장·대기를 흘린다.
//              💡 영업 시간 제한은 없다.
//              영업 중 명단의 예정 방문이 모두 끝나고 방명록이 완성되면 DayCycle.TrySettle로 마감하고 바로 정산을 저장한다.
//              마감 확인 버튼은 마감 단계에서만 누를 수 있다.
//   SaveManual — 누른 시점을 수동 슬롯에 쓴다(F13, 2026-11-25). 일차 시작 스냅샷(금화·마늘·재료·미처리 보고·전날 호감·입소문 인원·명부·시드)에
//              지금 단계·확정한 보고·금화·마늘과, 영업 중이면 명단(좌석 포함)·주문·방명록·방문 결과를 진행으로 함께 넣는다.
//              마감·엔딩 단계면 정산한 다음 일차 시작을 쓴다(💡 송예찬 확인 전). 불러온 진행을 아직 되살리기 전이면 그 진행을 그대로 다시 쓴다.
//   Settle   — 정산 저장이 아직이면 다시 시도한다. 마감한 순간에 적어 둔 그날 서빙 인원이 데모 엔딩 기준(08.Data/SO_DayCycle)
//              이상이면 DayCycle.TryEndDemo로 엔딩 단계에 들어가 임시 엔딩 창을 띄우고, 미달이면 저장한 정산으로 다음 일차에 진입한다.
//              돌려보낸 손님·대기 이탈은 명단이 서빙 인원에 넣지 않는다. 엔딩 창의 [타이틀로]는 타이틀 복귀만 요청한다.
//              엔딩에 들어가면 그 일차의 시작 스냅샷(진행 없음)을 데모 완료 슬롯에 쓴다. 정산은 평소처럼 다음 일차 시작을 자동 저장한다 (F13).
//              데모 완료 슬롯으로 연 하루(다시 플레이)는 첫 자동 저장·정산 저장·수동 저장을 하지 않고, 마감 확인에서 서빙 인원과 관계없이 엔딩으로 끝낸다.
//              수동 저장 요청은 IOException으로 거절한다(💡 전용 안내 문구는 홍준우 인계).
//              정산에는 지금의 금화·마늘·던전 재료와 오늘의 방문 결과(다음 아침의 미처리 보고)가 들어간다.
//              스냅샷을 만들기 직전에 관계 총괄이 명단과 방문 결과로 방문 호감·다음 영업 추가 인원을 한 번 확정해 함께 넣는다.
//              같은 때 그날 손님 신원으로 명부(누적 호감·영구 거절·부상 휴식)를 한 번 확정하고, 다음 일차의 시드를 새로 정해 함께 넣는다.
//              명부를 확정한 뒤 SettlementManager가 누적 호감 9 이상(거절 아님) 손님의 방문에만 전리품을 담아 미처리 보고로 넘긴다.
//              정산 결과가 이미 있으면 재사용해 금화·전리품을 두 번 지급하지 않는다.
// 보고 도중에는 자동 저장하지 않는다. 자동 슬롯을 불러오면 일차 시작으로 돌아가 아침 보고를 처음부터 다시 받는다.
// 자동 저장은 당일 정산 뒤 다음 일차 시작 상태로만 갱신한다 (2026-11-24 송예찬 확인, PR #29).
// 💡 저장하지 않는 것: 응대 대화·동행(입구 대기로 돌아간다), 들고 있던 접시(픽업대로 돌아간다), 손님 걷기 위치.
//   OnDisable — 구독을 풀고 응대·주방을 끈다.
// =====================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        // 통합 테스트가 데모 엔딩 기준을 줄이려고 씬 기준값 대신 넣는다. 게임 코드는 쓰지 않는다.
        internal static DayCycleSettings SettingsOverride;
        private FirstServiceSaveStore saves;
        private BasementPauseMenu pauseMenu;
        private Button settleButton;
        private bool newTimeline;
        // 마감 정산 저장과 그 결과를 소유한다. 씬에 없으면 하루를 열 때 붙인다.
        private SaveManager saveManager;
        // 레시피 기준과 열린 레시피를 소유한다. 씬에 없으면 하루를 열 때 붙인다.
        private KitchenManager kitchenManager;
        // 일차 시작의 열린 레시피(수동 저장이 일차 시작 스냅샷에 함께 쓴다).
        private IReadOnlyList<string> dayStartRecipes;
        // 일차 시작의 화구 수(수동 저장의 일차 시작 스냅샷용). 준비 단계에서 산 화구는 진행(DayProgress)에 남는다.
        private int dayStartBurners;
        // 수동 저장을 불러와 확정 보고를 다시 반영하는 동안에는 효과음을 내지 않는다.
        private bool restoring;
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
        // 데모 완료 슬롯으로 연 하루. 어떤 슬롯도 덮어쓰지 않고 다음 일차를 열지 않는다.
        private bool replayingDemo;
        // 불러온 수동 저장의 진행. 영업 진행은 식당이 준비된 뒤 되살리므로 그때까지 보관한다.
        private DayProgress pendingProgress;
        private readonly ServicePauseReasons pauses = new ServicePauseReasons();

        /// <summary>현재 일차. 1부터 시작한다.</summary>
        public int Day => cycle?.Day ?? startDay;
        /// <summary>현재 하루 단계. StartService 전에는 의미가 없다.</summary>
        public DayPhase Phase => cycle?.Phase ?? DayPhase.MorningReport;
        /// <summary>로딩 화면이 닫히고 하루가 시작된 상태.</summary>
        public bool IsReady { get; private set; }

        private DayCycleSettings Settings => SettingsOverride ? SettingsOverride : settings;
        private bool AllVisitsEnded => guests.Roster != null && guests.Roster.AllEnded && relationship.Guestbook.AllComplete;

        /// <summary>로딩 화면이 떠 있는 동안 하루를 준비한다. 연결이 빠졌으면 아무것도 바꾸지 않고 false.</summary>
        public bool Prepare(FirstServiceSaveStore store, FirstServiceSaveData start, bool startsNewTimeline, BasementHud hud)
        {
            // 일시 정지 메뉴는 BasementUI의 같은 Canvas에 붙어 있다 (BasementSceneBuilder.CreateHud).
            var menu = hud ? hud.GetComponent<BasementPauseMenu>() : null;
            if (!service || !kitchen || !guests || !relationship || !settlement || !economy || !Settings || store == null || !menu
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
            if (economy.SetupError is string marketError)
            {
                Debug.LogError("[DOB] 시장 기준을 확인하세요. " + marketError, economy);
                return false;
            }
            // 재고를 먼저 열어야 귀환 보고가 전리품을 넣을 곳이 생긴다. 보상·층 기준이 빠졌으면 여기서 실패한다.
            economy.BeginDay(start?.Gold, start?.Spices, start?.Materials);
            if (!settlement.BeginDay(start?.PendingReports, economy.Materials, start?.Floors))
                return false;
            kitchenManager = GetComponent<KitchenManager>();
            if (!kitchenManager) kitchenManager = gameObject.AddComponent<KitchenManager>();
            kitchenManager.BeginDay(start?.UnlockedRecipes, economy.Materials, start?.Burners);
            if (kitchenManager.SetupError is string kitchenError)
            {
                Debug.LogError("[DOB] 주방 기준을 확인하세요. " + kitchenError, kitchenManager);
                return false;
            }
            dayStartRecipes = kitchenManager.Unlocked;
            dayStartBurners = kitchenManager.OwnedBurners;
            saveManager = GetComponent<SaveManager>();
            if (!saveManager) saveManager = gameObject.AddComponent<SaveManager>();
            saveManager.Begin(store);

            saves = store;
            pauseMenu = menu;
            newTimeline = startsNewTimeline;
            dayStart = start;
            pendingProgress = start?.Progress;
            replayingDemo = start?.Slot == FirstServiceSaveSlot.DemoComplete;
            startDay = start?.Day ?? 1;

            service.ConnectHud(hud.Guidance, hud.Dialogue, startDay);
            dayHud = hud;
            kitchen.ConnectHud(hud);
            settleButton = hud.Settle;
            settleButton.interactable = false;
            settleButton.onClick.AddListener(Settle);
            kitchen.StartDay(economy, kitchenManager);
            kitchen.Served += RecordMeal;
            ConnectSignals(true);

            relationship.BeginDay(Settings.GuestbookWriteSeconds, start?.VisitAffinities, start?.WordOfMouthBonus ?? 0, start?.Guests, startDay);
            daySeed = start?.DaySeed ?? Guid.NewGuid().GetHashCode();
            guests.PrepareDay(startDay, start?.WordOfMouthBonus ?? 0, relationship.GuestsComingToday, relationship.KnownGuestNames, daySeed);
            // 보고 화면은 UI 씬에 있어 정산 총괄을 직접 참조할 수 없다. 화면과 같은 프리팹에 있는 Presenter에 조회 함수를 넘겨 준다.
            returnReport = hud.MorningReport.transform.root.GetComponentInChildren<ReturnReportPresenter>(true);
            if (returnReport) returnReport.Bind(() => settlement.Reports, settlement.IsNewMaterial);
            cycle = new DayCycle();
            cycle.PhaseChanged += OnPhaseChanged;
            // 시장은 준비 단계에서만 연다.
            economy.MarketOpen = () => cycle != null && cycle.Phase == DayPhase.Preparation;
            dayFlow = new DayFlowPresenter(cycle, relationship.Guestbook, hud.MorningReport, hud.Preparation, hud.Guestbook,
                () => (kitchen.Gold, kitchen.GarlicStock));
            try
            {
                if (!replayingDemo && !saves.TryLoad(FirstServiceSaveSlot.Auto, out _))
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
            if (pendingProgress != null)
                ResumePhase(pendingProgress);
            pauseMenu.Initialize(saves, Day);
            pauseMenu.SaveRequested += SaveManual;
            IsReady = true;
        }

        /// <summary>가진 화구 수. 하루를 열기 전이면 0.</summary>
        public int OwnedBurners => kitchenManager ? kitchenManager.OwnedBurners : 0;
        /// <summary>다음 화구 값. 다 샀으면 null.</summary>
        public int? NextBurnerPrice => kitchenManager ? kitchenManager.NextBurnerPrice : null;
        /// <summary>준비 화면의 화구 구매 요청(화면 연결은 홍준우 인계). 준비 단계에서만 금화를 쓰고 화구를 하나 늘린다.</summary>
        public PurchaseResult TryBuyBurner() => kitchenManager ? kitchenManager.TryBuyBurner(economy) : PurchaseResult.MarketClosed;

        /// <summary>현재 영업을 계속하면서 누른 시점의 진행을 수동 슬롯에 기록한다. 파일 오류는 호출자에게 전달한다.</summary>
        public void SaveManual()
        {
            if (replayingDemo)
                throw new IOException("데모 완료 슬롯으로 다시 플레이하는 하루는 저장하지 않습니다.");
            if (cycle != null && (cycle.Phase == DayPhase.Closing || cycle.Phase == DayPhase.DemoEnding))
            {
                if (!SaveSettlement())
                    throw new IOException("영업 정산을 저장하지 못했습니다.");
                var next = saveManager.Settled;
                saves.SavePreparation(FirstServiceSaveSlot.Manual, next.Day, next.Gold, next.Spices, next.Materials, next.PendingReports,
                    next.VisitAffinities, next.WordOfMouthBonus, next.Guests, next.DaySeed, null, next.Floors, next.UnlockedRecipes, next.Burners);
                return;
            }
            SaveDayStart(FirstServiceSaveSlot.Manual, CaptureProgress());
        }

        // 금화·양념·재료·미처리 보고·전날 호감·입소문 인원·층 진행은 일차 시작 스냅샷의 값을 쓴다. 아침에 이미 받은 전리품은 진행의 확정 보고로 남겨,
        // 불러올 때 시작 재고에 한 번씩 다시 더한다. 지금 재고를 그대로 쓰면 미처리 보고와 겹쳐 두 번 받게 된다 (FeatureManagerMap A·B).
        private void SaveDayStart(FirstServiceSaveSlot slot, DayProgress progress = null) => saves.SavePreparation(slot, startDay, economy.DayStartGold,
            economy.DayStartSpices, dayStart?.Materials, dayStart?.PendingReports, dayStart?.VisitAffinities, dayStart?.WordOfMouthBonus ?? 0,
            dayStart?.Guests, daySeed, progress, dayStart?.Floors, dayStartRecipes, dayStartBurners);

        private DayProgress CaptureProgress()
        {
            if (pendingProgress != null)
                return pendingProgress;
            if (cycle == null)
                return null;
            var confirmed = settlement.ConfirmedReports;
            if (cycle.Phase != DayPhase.Service)
                return new DayProgress(cycle.Phase, confirmed, economy.Gold, economy.Spices.All(), burners: kitchenManager.OwnedBurners);
            var roster = guests.Roster;
            // 식당이 아직 준비되지 않았으면 영업에서 바뀐 것이 없으므로 일차 시작으로 충분하다.
            if (roster == null)
                return null;
            var visits = roster.Visits.Select(v => new VisitProgress(v.VisitId, v.GuestId, v.Stage, v.End, v.WaitSeconds, v.WaitScore,
                v.TasteScore, guests.Find(v.VisitId) is GuestAgent agent ? agent.SeatIndex : -1));
            var orders = kitchen.Orders.Select(o => new OrderProgress(o.VisitId, o.State, o.RemainingSeconds, o.RecipeId, o.Spice, o.BurnerIndex));
            return new DayProgress(DayPhase.Service, confirmed, economy.Gold, economy.Spices.All(),
                new RosterProgress(roster.NextArrivalSeconds, roster.ArrivalDue, visits), orders, relationship.Guestbook.Snapshot(),
                settlement.TodayResults, kitchenManager.OwnedBurners);
        }

        // 저장된 확정 보고를 다시 확정하고(전리품·층 진행이 한 번씩 반영된다) 누른 시점의 금화·양념으로 바꾼 뒤 저장된 단계까지 넘긴다.
        // 영업 진행은 식당이 준비된 뒤 ResumeService가 되살린다.
        private void ResumePhase(DayProgress progress)
        {
            restoring = true;
            foreach (string visitId in progress.ConfirmedReports)
                settlement.ConfirmReport(visitId);
            restoring = false;
            // 영업 단계로 넘어가면 주방이 켜지며 양념 재고를 잡으므로, 그 전에 바꾼다.
            if (!economy.Restore(progress.Gold, progress.Spices))
                Debug.LogError("[DOB] 수동 저장의 금화·양념을 되살리지 못했습니다.", this);
            kitchenManager.RestoreBurners(progress.Burners);
            while (cycle.Phase != progress.Phase && cycle.TryComplete(cycle.Phase, out _)) { }
            if (cycle.Phase != DayPhase.Service)
                pendingProgress = null;
        }

        private void ResumeService(DayProgress progress)
        {
            bool restored = guests.RestoreProgress(progress.Roster) && kitchen.RestoreProgress(progress.Orders);
            settlement.RestoreResults(progress.Results);
            foreach (var entry in progress.Guestbook)
                restored &= relationship.Guestbook.Restore(entry.GuestId, entry.VisitId, entry.GuestName, entry.Line, entry.RemainingSeconds);
            foreach (var visit in progress.Roster.Visits)
                if (visit.Stage == ServiceVisitStage.Seated)
                    restored &= service.RestoreSeated(visit.VisitId, visit.Seat);
            if (!restored)
                Debug.LogError("[DOB] 수동 저장의 영업 진행을 모두 되살리지 못했습니다. 손님 후보·좌석 배치가 저장 때와 같은지 확인하세요.", this);
        }

        /// <summary>마감 확인. 마감 단계에서만 정산을 저장(이미 했으면 재사용)하고, 서빙 인원이 기준 이상이면 데모 엔딩, 아니면 다음 일차로 진입한다.</summary>
        public void Settle()
        {
            if (!IsReady || BasementComposition.IsEntering || cycle.Phase != DayPhase.Closing)
                return;
            if (replayingDemo)
            {
                // 다시 플레이한 하루는 인원과 관계없이 끝낸다. 기준 1명으로 엔딩 단계에 넣는다.
                if (cycle.TryEndDemo(Math.Max(1, servedToday), 1, out _))
                    ShowDemoEnding();
                return;
            }
            if (!SaveSettlement())
                return;
            if (cycle.TryEndDemo(servedToday, Settings.DemoEndingServedGuests, out _))
            {
                SaveDemoComplete();
                ShowDemoEnding();
            }
            else
                BasementComposition.EnterDay(saveManager.Settled);
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

        // 엔딩을 본 일차의 시작을 보관한다. 실패해도 엔딩은 보여 준다(정산은 이미 자동 저장됐다).
        private void SaveDemoComplete()
        {
            try { SaveDayStart(FirstServiceSaveSlot.DemoComplete); }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Debug.LogWarning("[DOB] 데모 완료 슬롯을 저장할 수 없습니다. " + e.Message, this);
            }
        }

        // 다음 씬을 열지 못하거나 저장이 실패해 다시 불려도, 이미 확정한 정산을 그대로 쓴다.
        private bool SaveSettlement()
        {
            if (saveManager.Settled != null)
                return true;
            try
            {
                // 새 게임 또는 수동 슬롯 선택 시, 첫 정산에 한해 이 플레이의 결과가 자동 슬롯을 이어받는다.
                var materials = economy.Materials.All();
                var visits = settlement.TodayResults;
                // 명단은 식당 씬이 내려갈 때 비워진다. 그 전에 한 번 확정하고, 저장 재시도는 보관한 값을 다시 받는다.
                var (affinities, bonus) = relationship.SettleDay(guests.Roster?.Visits ?? Array.Empty<ServiceVisit>(), visits, guests.TodaysGuests);
                var ledger = relationship.Guests;
                // 전리품 자격은 그날 호감을 더한 누적 호감·거절로 정하므로 명부를 갱신한 뒤에 정한다.
                var reports = settlement.SettledResults(relationship.FindGuest);
                nextDaySeed = nextDaySeed ?? Guid.NewGuid().GetHashCode();
                saveManager.Settle(newTimeline, Day, economy.Gold, economy.Spices.All(), materials, reports, affinities, bonus, ledger,
                    nextDaySeed, settlement.FloorSnapshot, kitchenManager.Unlocked, kitchenManager.OwnedBurners);
                return true;
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Debug.LogWarning("[DOB] 영업 정산을 저장할 수 없습니다. 마감 확인으로 다시 시도합니다. " + e.Message, this);
                return false;
            }
        }

        // 성공 결과에만 효과음을 낸다(sound-playback). 귀환 보고 확정은 전리품이 들어온 뒤라 레시피 해금도 여기서 한다.
        private void ConnectSignals(bool on)
        {
            if (on)
            {
                guests.GuestEntered += OnGuestEntered;
                kitchen.CookFinished += OnCookFinished;
                kitchen.Served += OnServedSound;
                economy.GoldEarned += OnGoldEarned;
                settlement.ReportConfirmed += OnReportConfirmed;
                return;
            }
            if (guests) guests.GuestEntered -= OnGuestEntered;
            if (kitchen) { kitchen.CookFinished -= OnCookFinished; kitchen.Served -= OnServedSound; }
            if (economy) economy.GoldEarned -= OnGoldEarned;
            if (settlement) settlement.ReportConfirmed -= OnReportConfirmed;
        }

        private void OnGuestEntered(string visitId) => SoundManager.Play(SoundCue.GuestEntered);
        private void OnCookFinished(string visitId) => SoundManager.Play(SoundCue.CookFinished);
        private void OnServedSound(FirstGuestKitchen.ServedMeal meal) => SoundManager.Play(SoundCue.Served);
        private void OnGoldEarned(int gold) => SoundManager.Play(SoundCue.GoldEarned);

        private void OnReportConfirmed(string visitId)
        {
            kitchenManager.Refresh(economy.Materials);
            if (!restoring)
                SoundManager.Play(SoundCue.ReportStamped);
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
            // 서빙한 순간의 대기·입맛 점수를 남기고 파견으로 끝낸다. 손님 이동은 명단의 종료 알림을 받은 FirstGuestService가 한다.
            int? taste = visit != null ? guests.TasteScoreFor(visit.GuestId, meal.Dish, meal.Seasonings) : null;
            guests.Roster?.TryServe(meal.VisitId, taste);
            guests.Roster?.TryEnd(meal.VisitId, ServiceVisitEnd.Dispatched);
        }

        private void Update()
        {
            if (!IsReady)
                return;
            if (pendingProgress != null)
            {
                // 식당이 준비돼 명단이 열리기 전에는 아무 시계도 흘리지 않는다.
                if (guests.Roster == null || !kitchen.enabled)
                    return;
                ResumeService(pendingProgress);
                pendingProgress = null;
            }
            pauses.Set(ServicePauseReason.PauseMenu, BasementPauseMenu.IsPaused);
            pauses.Set(ServicePauseReason.Transition, BasementComposition.IsEntering || SceneLoadService.IsLoading);
            bool paused = pauses.IsStopped;
            // 응대·수첩·재료 선택 중에도 입장·대기·조리·방명록 작성은 흐른다. 일시 정지·전환에만 모두 멈춘다 (F2 노션 확정).
            float clock = paused ? 0f : Time.deltaTime;
            relationship.Tick(clock);
            if (cycle.Phase == DayPhase.Service)
            {
                kitchen.Tick(clock);
                guests.Tick(clock);
            }

            // 마지막 방문이 끝난 순간 한 번 마감·정산한다.
            if (cycle.Phase == DayPhase.Service && cycle.TrySettle(AllVisitsEnded, out _))
            {
                servedToday = guests.Roster.ServedGuestCount;
                if (!replayingDemo)
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
            ConnectSignals(false);
            if (returnReport) returnReport.Bind(null, null);
            returnReport = null;
            if (settleButton) settleButton.onClick.RemoveListener(Settle);
            if (pauseMenu) pauseMenu.SaveRequested -= SaveManual;
            if (kitchen) kitchen.enabled = false;
            if (service) service.enabled = false;
        }
    }
}
