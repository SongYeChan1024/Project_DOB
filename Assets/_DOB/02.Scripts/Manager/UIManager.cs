// =====================================================================
// UIManager — 여러 씬에 걸친 공용 화면 처리를 맡는 중계자
// 역할   : 로더(SceneLoadService·BasementComposition)의 로딩 열기·진행률·닫기·실패 요청을
//          등록된 로딩 화면 Presenter에 전달한다. 실패 안내 버튼 응답(타이틀로·종료)을 처리하고,
//          버튼을 누를 수 있게 EventSystem이 없으면 임시로 만든다.
// 호출자 : SceneLoadService, BasementComposition (OpenLoading, SetLoadingProgress, CloseLoading, ShowLoadingFailure)
//          LoadingScreenPresenter (RegisterLoadingScreen, UnregisterLoadingScreen, 실패 버튼 응답)
// 호출   : LoadingScreenPresenter, SceneLoadService.RequestReturnToTitle
// 상태   : 등록된 로딩 화면, 로딩 화면이 다 보인 시각, 여는 중/열림 여부.
//          게임 상태(금화·주문·저장)는 갖지 않고, 씬에 배치된 화면을 만들거나 없애지 않는다.
//          직접 만들고 없애는 것은 실패 안내용 임시 EventSystem 하나뿐이며, 그것도 Loading 씬과 함께 사라진다.
//
// 의사코드
// [인스턴스]  처음 쓸 때 하나 만들고 씬이 바뀌어도 유지한다. 플레이 시작마다 초기화한다
// [등록]      로딩 화면 Presenter가 켜지면 등록, 꺼지면 해제한다 (Find* 대신)
// [열기]      화면 등록을 몇 프레임 기다린다 → 없으면 경고하고 표시 없이 진행
//             → 목적지 내용 표시(검은 화면) → 페이드 인 끝까지 대기 → 다 보인 시각 기록
// [진행률]    등록된 화면에 그대로 전달한다
// [닫기]      여는 중이면 끝날 때까지 기다린다 → 진행 바를 1로
//             → 최소 표시 시간 경과와 진행 바 채움을 기다린다(채움은 상한 시간까지만) → 검은 페이드 끝까지 대기
// [실패]      화면이 없으면 false를 돌려 호출자가 정리하게 한다
//             → 있으면 EventSystem을 보장하고 실패 안내를 띄운다
// [버튼 응답] 타이틀로: SceneLoadService에 실패 정리 후 타이틀 전환을 요청 / 종료: 게임 종료
//
// ── 준비 화면 (홍준우 추가 · 2026-11-24) ──────────────────────────────
// 호출자 : 식당 물건 상호작용(E) · Tab 입력 쪽 (OpenPrepScreen, ClosePrepScreen)
//          PrepScreenPresenter (RegisterPrepScreen, UnregisterPrepScreen, HandleStartBusiness)
// 호출   : PrepScreenPresenter
// 상태   : 등록된 준비 화면 Presenter 하나.
// [등록]      준비 화면 Presenter가 켜지면 등록, 꺼지면 해제한다 (로딩 화면과 같은 방식)
// [열기]      등록된 화면이 없으면 경고하고 false → 있으면 그 화면(시장 · 레시피 책 · 수첩 · 가게 개량 · 입구 문패)을 연다
// [영업 시작] 입구 문패의 [문 열기]를 StartBusinessRequested로 알린다 (듣는 쪽: GameManager · 없으면 경고)
//
// ── 요리 화면 (홍준우 추가 · 2026-11-24) ──────────────────────────────
// 호출자 : 식당의 화구 상호작용(E) 쪽 (OpenKitchen(화구 번호), CloseKitchen, IsKitchenOpen)
//          KitchenPresenter (RegisterKitchen, UnregisterKitchen)
// 호출   : KitchenPresenter
// 상태   : 등록된 요리 화면 Presenter 하나.
// [열기]      등록된 화면이 없으면 경고하고 false → 있으면 그 화구의 요리 화면을 연다
// [열림 여부] 재료 고르는 중에는 손님 대기 시간을 멈춘다(기획 4-1). 손님 쪽이 IsKitchenOpen을 읽는다
// [실제 주문] 주방(FirstGuestKitchen)이 ConnectKitchenOrders로 IKitchenOrders를 붙이면 요리 화면이 그 주문을 읽는다. 없으면 더미
// [손님 말풍선] 손님 쪽이 ConnectGuestMoods로 IGuestMoods를 붙이면 손님 머리 위에 기다림 원 · 불평 · 기쁨 · 퇴장 표정이 뜬다
// [화구 표시] 주방이 ConnectKitchenBurners로 IKitchenBurners를 붙이면 조리 중인 화구 위에 요리 그림 · 남은 초가 뜬다
// [영업 HUD] 영업 쪽이 ConnectServiceStatus로 IServiceStatus를 붙이면 일차 · 금화 · 손님 수 · 주문서 · 든 접시가 뜬다
// [상호작용 표시] 상호작용하는 쪽이 ConnectInteractionPrompt로 IInteractionPrompt를 붙이면 대상 위에 [E] 동작 표시가 뜬다
// [하루 연결] (홍준우 2026-11-28) GameManager가 하루를 열 때 ConnectDay(정산 · 호감 · 손님 Manager)로 잇는다.
//             귀환 보고 화면은 보고 확정 · 층 공개/개방 · 입소문 인원을, 준비 화면은 층 진행 · 오늘 손님 수를 읽는다. 연결 전이면 더미
// [Esc 순서] IsScreenOpen: 준비 · 요리 · 방명록 화면이 열려 있으면 Esc는 그 화면을 닫는 데 쓰고 ESC 메뉴는 열지 않는다 (BasementPauseInput)
// =====================================================================
using System;
using System.Collections.Generic;
using System.Collections;
using DOB.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace DOB.Manager
{
    /// <summary>씬을 넘어 유지되는 공용 화면 중계자. 로딩 화면 · 실패 안내 입력 · 준비 화면 · 요리 화면을 맡는다.</summary>
    [DisallowMultipleComponent]
    public sealed class UIManager : MonoBehaviour
    {
        // 씬 활성화 직후 Presenter의 OnEnable이 끝나지 않은 경우를 대비해 기다리는 프레임 수.
        private const int RegistrationWaitFrames = 10;
        // 화면이 꺼져 진행 바가 멈춰도 전환이 영원히 막히지 않게 하는 추가 대기 상한(초).
        private const float CatchUpTimeoutSeconds = 3f;

        private static UIManager instance;

        private LoadingScreenPresenter loadingScreen;
        private float loadingShownAt = -1f;
        private bool loadingOpening;
        private bool loadingOpen;

        /// <summary>로딩 화면이 열려 있거나 여는 중인지. 닫기가 끝나거나 화면이 사라지면 거짓이다.</summary>
        public static bool IsLoadingOpen => instance && (instance.loadingOpen || instance.loadingOpening);

        // ---------------- 준비 화면 ----------------
        // 프리팹마다 하나씩 (PF_Shop · PF_Notebook · PF_DoorSign)
        private readonly List<PrepScreenPresenter> prepScreens = new List<PrepScreenPresenter>();
        // ---------------- 요리 화면 ----------------
        private KitchenPresenter kitchen;

        /// <summary>화구 요리 화면이 열려 있는지 (재료 고르는 중).</summary>
        public static bool IsKitchenOpen => instance && instance.kitchen && instance.kitchen.IsOpen;

        /// <summary>요리 화면이 씬에 있는지. 있으면 주방 E가 이 화면을 연다 (없으면 기존 레시피 창).</summary>
        public static bool HasKitchenScreen => instance && instance.kitchen;

        // 실제 주방 주문 (FirstGuestKitchen). 없으면 요리 화면은 더미로 그린다
        private IKitchenOrders kitchenOrders;
        internal static IKitchenOrders KitchenOrders => instance ? instance.kitchenOrders : null;
        private EconomyManager economy;
        private KitchenManager kitchenRules;
        /// <summary>실제 금화 · 양념 · 시장. 연결 전(테스트 씬 등)이면 null이고 준비 화면은 더미 값을 쓴다.</summary>
        internal static EconomyManager Economy => instance ? instance.economy : null;
        /// <summary>실제 화구 수 · 화구 구매. Economy와 함께 연결된다. 없으면 null.</summary>
        internal static KitchenManager KitchenRules => instance ? instance.kitchenRules : null;

        // 귀환 보고 · 층 진행 · 호감 · 오늘 명단 (하루를 열 때 GameManager가 ConnectDay로 잇는다). 없으면 화면은 더미 · 빈 칸
        private SettlementManager settlement;
        private RelationshipManager relationship;
        private GuestManager guests;
        /// <summary>귀환 보고 확정 · 층 공개/개방 조회. 연결 전이면 null.</summary>
        internal static SettlementManager Settlement => instance ? instance.settlement : null;
        /// <summary>전날 방문 호감 · 입소문 신규 인원. 연결 전이면 null.</summary>
        internal static RelationshipManager Relationship => instance ? instance.relationship : null;
        /// <summary>오늘 올 손님 명단. 연결 전이면 null.</summary>
        internal static GuestManager Guests => instance ? instance.guests : null;

        // 지금 상호작용할 대상 (FirstGuestService 💡). 없으면 상호작용 표시를 숨긴다
        private IInteractionPrompt interactionPrompt;
        internal static IInteractionPrompt InteractionPrompt => instance ? instance.interactionPrompt : null;

        /// <summary>상호작용 대상을 알려 줄 곳을 잇는다 (상호작용하는 쪽이 켜질 때).</summary>
        public static void ConnectInteractionPrompt(IInteractionPrompt prompt)
        {
            if (prompt != null)
                Instance.interactionPrompt = prompt;
        }

        /// <summary>상호작용하는 쪽이 꺼질 때 연결을 푼다. 종료 중에 새 인스턴스를 만들지 않는다.</summary>
        public static void DisconnectInteractionPrompt(IInteractionPrompt prompt)
        {
            if (instance && instance.interactionPrompt == prompt)
                instance.interactionPrompt = null;
        }

        // 손님 말풍선 상태 (FirstGuestService 💡). 없으면 말풍선을 숨긴다
        private IGuestMoods guestMoods;
        internal static IGuestMoods GuestMoods => instance ? instance.guestMoods : null;

        /// <summary>손님 말풍선 상태를 알려 줄 곳을 잇는다 (손님 쪽이 켜질 때).</summary>
        public static void ConnectGuestMoods(IGuestMoods moods)
        {
            if (moods != null)
                Instance.guestMoods = moods;
        }

        /// <summary>손님 쪽이 꺼질 때 연결을 푼다. 종료 중에 새 인스턴스를 만들지 않는다.</summary>
        public static void DisconnectGuestMoods(IGuestMoods moods)
        {
            if (instance && instance.guestMoods == moods)
                instance.guestMoods = null;
        }

        // 화구들의 조리 상태 (FirstGuestKitchen 💡). 없으면 화구 위 표시를 숨긴다
        private IKitchenBurners kitchenBurners;
        internal static IKitchenBurners KitchenBurners => instance ? instance.kitchenBurners : null;

        /// <summary>화구 조리 상태를 알려 줄 곳을 잇는다 (주방이 켜질 때).</summary>
        public static void ConnectKitchenBurners(IKitchenBurners burners)
        {
            if (burners != null)
                Instance.kitchenBurners = burners;
        }

        /// <summary>주방이 꺼질 때 연결을 푼다. 종료 중에 새 인스턴스를 만들지 않는다.</summary>
        public static void DisconnectKitchenBurners(IKitchenBurners burners)
        {
            if (instance && instance.kitchenBurners == burners)
                instance.kitchenBurners = null;
        }

        // 영업 HUD 상태 (💡 FirstGuestService · FirstGuestKitchen). 없으면 HUD를 숨긴다
        private IServiceStatus serviceStatus;
        internal static IServiceStatus ServiceStatus => instance ? instance.serviceStatus : null;

        /// <summary>영업 HUD 상태를 알려 줄 곳을 잇는다 (영업 쪽이 켜질 때).</summary>
        public static void ConnectServiceStatus(IServiceStatus status)
        {
            if (status != null)
                Instance.serviceStatus = status;
        }

        /// <summary>영업 쪽이 꺼질 때 연결을 푼다. 종료 중에 새 인스턴스를 만들지 않는다.</summary>
        public static void DisconnectServiceStatus(IServiceStatus status)
        {
            if (instance && instance.serviceStatus == status)
                instance.serviceStatus = null;
        }

        /// <summary>Esc로 닫히는 화면(준비 · 요리 · 방명록)이 열려 있는지. 열려 있으면 Esc는 그 화면만 닫고 ESC 메뉴를 열지 않는다.</summary>
        public static bool IsScreenOpen => IsPrepScreenOpen || IsKitchenOpen || GuestbookBookView.IsAnyOpen;

        /// <summary>준비 화면(시장 · 레시피 책 · 수첩 · 가게 개량 · 입구 문패) 중 하나가 열려 있는지.</summary>
        public static bool IsPrepScreenOpen => instance && instance.prepScreens.Exists(p => p && p.IsOpen);

        /// <summary>입구 문패에서 [문 열기]를 눌렀다. GameManager(DayCycle, 💡 구성안)가 구독해 준비 → 영업으로 넘긴다.</summary>
        public static event Action StartBusinessRequested;

        private static UIManager Instance
        {
            get
            {
                if (!instance)
                {
                    var host = new GameObject(nameof(UIManager));
                    DontDestroyOnLoad(host);
                    instance = host.AddComponent<UIManager>();
                }
                return instance;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetInstance()
        {
            instance = null;
            StartBusinessRequested = null;
        }

        // =================================================================
        // 준비 화면 (홍준우)
        // =================================================================

        /// <summary>준비 화면 Presenter가 켜질 때 등록한다.</summary>
        internal static void RegisterPrepScreen(PrepScreenPresenter screen)
        {
            if (screen && !Instance.prepScreens.Contains(screen))
                Instance.prepScreens.Add(screen);
        }

        /// <summary>준비 화면 Presenter가 꺼질 때 등록을 푼다. 종료 중에 새 인스턴스를 만들지 않는다.</summary>
        internal static void UnregisterPrepScreen(PrepScreenPresenter screen)
        {
            if (instance)
                instance.prepScreens.Remove(screen);
        }

        /// <summary>준비 화면 하나를 연다. 등록된 화면이 없으면 경고하고 false.</summary>
        public static bool OpenPrepScreen(PrepScreen screen)
        {
            var owner = instance ? instance.prepScreens.Find(p => p && p.Has(screen)) : null;
            if (!owner)
            {
                Debug.LogWarning("[DOB] 준비 화면이 등록되지 않아 열 수 없습니다: " + screen);
                return false;
            }
            // 다른 프리팹의 준비 화면이 열려 있으면 닫고 바꿔 끼운다
            foreach (var other in instance.prepScreens)
                if (other && other != owner) other.Close();
            return owner.Open(screen);
        }

        /// <summary>열린 준비 화면을 닫는다. 없으면 무시한다.</summary>
        public static void ClosePrepScreen()
        {
            if (!instance) return;
            foreach (var screen in instance.prepScreens)
                if (screen) screen.Close();
        }

        /// <summary>입구 문패 [문 열기] 응답.</summary>
        internal static void HandleStartBusiness()
        {
            if (StartBusinessRequested == null)
                Debug.LogWarning("[DOB] 영업 시작 요청을 받을 곳이 없습니다 (StartBusinessRequested 구독 없음).");
            StartBusinessRequested?.Invoke();
        }

        // =================================================================
        // 요리 화면 (홍준우)
        // =================================================================

        /// <summary>요리 화면 Presenter가 켜질 때 등록한다.</summary>
        internal static void RegisterKitchen(KitchenPresenter screen)
        {
            if (screen)
                Instance.kitchen = screen;
        }

        /// <summary>요리 화면 Presenter가 꺼질 때 등록을 푼다. 종료 중에 새 인스턴스를 만들지 않는다.</summary>
        internal static void UnregisterKitchen(KitchenPresenter screen)
        {
            if (!instance || instance.kitchen != screen)
                return;
            instance.kitchen = null;
        }

        /// <summary>실제 주방 주문을 요리 화면에 잇는다 (주방이 켜질 때).</summary>
        internal static void ConnectKitchenOrders(IKitchenOrders orders)
        {
            if (orders != null)
                Instance.kitchenOrders = orders;
        }

        /// <summary>주방이 꺼질 때 연결을 푼다. 종료 중에 새 인스턴스를 만들지 않는다.</summary>
        internal static void DisconnectKitchenOrders(IKitchenOrders orders)
        {
            if (instance && instance.kitchenOrders == orders)
                instance.kitchenOrders = null;
        }

        /// <summary>실제 금화 · 양념 · 시장과 화구 구매를 준비 화면에 잇는다 (하루를 열 때, 예: GameManager가 두 Manager를 준비한 뒤).</summary>
        internal static void ConnectEconomy(EconomyManager economy, KitchenManager kitchen = null)
        {
            if (!economy) return;
            Instance.economy = economy;
            Instance.kitchenRules = kitchen;
        }

        /// <summary>연결을 푼다 (꺼질 때). 종료 중에 새 인스턴스를 만들지 않는다.</summary>
        internal static void DisconnectEconomy(EconomyManager economy)
        {
            if (!instance || instance.economy != economy) return;
            instance.economy = null;
            instance.kitchenRules = null;
        }

        /// <summary>귀환 보고 · 층 진행 · 호감 · 오늘 명단을 보고 화면과 준비 화면에 잇는다 (하루를 열 때, 세 Manager의 BeginDay · PrepareDay 뒤).
        /// 화면은 조회와 보고 확정 요청만 하고 값을 바꾸거나 다시 계산하지 않는다.</summary>
        internal static void ConnectDay(SettlementManager settlement, RelationshipManager relationship, GuestManager guests)
        {
            Instance.settlement = settlement;
            Instance.relationship = relationship;
            Instance.guests = guests;
        }

        /// <summary>ConnectDay로 이은 것을 푼다 (꺼질 때). 다른 하루가 이미 이었으면 건드리지 않는다. 종료 중에 새 인스턴스를 만들지 않는다.</summary>
        internal static void DisconnectDay(SettlementManager settlement)
        {
            if (!instance || instance.settlement != settlement) return;
            instance.settlement = null;
            instance.relationship = null;
            instance.guests = null;
        }

        /// <summary>화구 하나(0부터)의 요리 화면을 연다. 등록된 화면이 없으면 경고하고 false.</summary>
        public static bool OpenKitchen(int burner)
        {
            if (!instance || !instance.kitchen)
            {
                Debug.LogWarning("[DOB] 요리 화면이 등록되지 않아 열 수 없습니다: 화구 " + burner);
                return false;
            }
            return instance.kitchen.Open(burner);
        }

        /// <summary>열린 요리 화면을 닫는다. 없으면 무시한다.</summary>
        public static void CloseKitchen()
        {
            if (instance && instance.kitchen)
                instance.kitchen.Close();
        }

        /// <summary>Loading 씬의 로딩 화면이 켜질 때 등록한다.</summary>
        internal static void RegisterLoadingScreen(LoadingScreenPresenter screen)
        {
            if (screen)
                Instance.loadingScreen = screen;
        }

        /// <summary>로딩 화면이 꺼질 때 등록을 푼다. 종료 중에 새 인스턴스를 만들지 않는다.</summary>
        internal static void UnregisterLoadingScreen(LoadingScreenPresenter screen)
        {
            if (!instance || instance.loadingScreen != screen)
                return;
            instance.loadingScreen = null;
            instance.loadingOpen = false;
            instance.loadingShownAt = -1f;
        }

        /// <summary>목적지의 로딩 화면을 보이고 페이드 인한다. 끝까지 기다리려면 반환값을 yield return 한다.</summary>
        public static Coroutine OpenLoading(string destination)
        {
            var manager = Instance;
            return manager.StartCoroutine(manager.Open(destination));
        }

        /// <summary>실제 진행률(0~1)을 로딩 화면에 전달한다. 화면이 없으면 무시한다.</summary>
        public static void SetLoadingProgress(float progress)
        {
            if (instance && instance.loadingScreen)
                instance.loadingScreen.SetProgress(progress);
        }

        /// <summary>최소 표시 시간·진행 바 채움·페이드 아웃이 모두 끝날 때까지 기다린다. 반환값을 yield return 한다.</summary>
        public static Coroutine CloseLoading()
        {
            var manager = Instance;
            return manager.StartCoroutine(manager.Close());
        }

        /// <summary>로딩 실패 안내를 띄운다. 띄울 화면이 없으면 false를 돌려 호출자가 정리하게 한다.</summary>
        public static bool ShowLoadingFailure(bool canReturnToTitle)
        {
            if (!instance || !instance.loadingScreen)
                return false;
            var screen = instance.loadingScreen;
            EnsureEventSystem(screen.gameObject.scene);
            screen.ShowFailure(canReturnToTitle);
            return true;
        }

        /// <summary>실패 안내의 [타이틀로] 응답.</summary>
        internal static void HandleLoadingReturnToTitle()
        {
            if (!SceneLoadService.RequestReturnToTitle())
                Debug.LogError("[DOB] 로딩 실패 후 타이틀로 돌아갈 수 없습니다.");
        }

        /// <summary>실패 안내의 [게임 종료] 응답.</summary>
        internal static void HandleLoadingQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private IEnumerator Open(string destination)
        {
            loadingOpening = true;
            loadingShownAt = -1f;
            try
            {
                for (var frame = 0; !loadingScreen && frame < RegistrationWaitFrames; frame++)
                    yield return null;
                if (!loadingScreen)
                {
                    Debug.LogWarning("[DOB] 로딩 화면이 등록되지 않아 표시 없이 전환합니다.");
                    yield break;
                }

                var screen = loadingScreen;
                screen.Show(destination);
                loadingOpen = true;
                yield return screen.FadeIn();
                if (screen && screen == loadingScreen)
                    loadingShownAt = Time.realtimeSinceStartup;
            }
            finally
            {
                loadingOpening = false;
            }
        }

        private IEnumerator Close()
        {
            while (loadingOpening)
                yield return null;
            var screen = loadingScreen;
            if (!screen)
                yield break;

            screen.SetProgress(1f);
            var shownAt = loadingShownAt >= 0f ? loadingShownAt : Time.realtimeSinceStartup;
            var minimum = screen.MinimumDisplaySeconds;
            while (screen && Time.realtimeSinceStartup - shownAt < minimum)
                yield return null;
            var catchUpDeadline = Time.realtimeSinceStartup + CatchUpTimeoutSeconds;
            while (screen && !screen.HasCaughtUp && Time.realtimeSinceStartup < catchUpDeadline)
                yield return null;
            if (screen)
                yield return screen.FadeOut();
            if (screen == loadingScreen)
                loadingOpen = false;
        }

        private static void EnsureEventSystem(Scene owner)
        {
            if (EventSystem.current)
                return;
            // Loading 씬은 평소 EventSystem을 두지 않는다(목적지 씬과 겹치지 않게).
            // 실패 안내 때만 같은 씬에 임시로 만들어, Loading이 닫히면 함께 사라지게 한다.
            var host = new GameObject("TemporaryEventSystem");
            if (owner.IsValid() && owner.isLoaded)
                SceneManager.MoveGameObjectToScene(host, owner);
            host.AddComponent<EventSystem>();
            host.AddComponent<InputSystemUIInputModule>().AssignDefaultActions();
        }
    }
}
