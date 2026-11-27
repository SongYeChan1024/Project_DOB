// FirstGuestKitchen — 주문 장부(OrderBook)를 요리 화면·옛 레시피 패널, 식당 상호작용, 경제(금화·재료·양념), 주방 기준(레시피·화구)에 연결한다.
// 호출자: FirstGuestService(E 입력·안내 위임, 착석 알림), GameManager(StartDay·ConnectHud·조리 Δ Tick, Served·CookFinished 구독).
// 호출  : OrderBook, KitchenManager(레시피 판정·화구 위치), EconomyManager(재고·서빙 대금), RecipeJudge, PriceJudge,
//         GuestManager(방문 원고·입맛 조회, VisitEnded), ExpeditionJudge.
// 상태  : 하루 주문 장부, 옛 레시피 패널이 가리키는 방문. 금화·재고는 경제, 레시피 기준·열린 레시피는 KitchenManager가 소유한다.
// 의사코드:
//   착석 알림마다 그 방문의 주문을 연다. 서빙 전에 방문이 끝나면(돌려보냄·대기 이탈) 그 주문을 취소한다.
//   E(주방 근처): 완성된 접시가 있으면 먼저 연 완성 주문을 든다. 아니면 빈 화구가 있을 때 먼저 연 대기 주문으로 요리 화면을 연다.
//   요리 화면은 재료·양념·도구 조합을 보내고, KitchenManager가 열린 레시피 중 재료가 정확히 같은 것을 고른다(kitchen-recipes).
//   옛 레시피 패널(요리 화면이 없을 때)은 구운 빵만 굽고, 마늘이 있으면 넣는다(첫 플레이 E11).
//   조리 시간은 GameManager가 넘기는 조리 Δ로만 흐른다(응대·재료 선택 중에도 흐르고, 일시 정지·전환에 멈춘다).
//   E(접시를 든 채 손님 자리 근처): 가장 가까운 착석 손님에게 서빙을 요청한다. 접시 주인이 아니면 장부가 거절한다.
//   서빙이 성공하면 대금 = PriceJudge(레시피 기본가, 손님 입맛과 접시 맛의 맛 점수, 넣은 양념 수)을 경제에 한 번 더한다.
//   판정의 보호 칸 = 레시피 효과 + 양념 효과 1칸(RecipeJudge.Protection). 미리보기와 같은 함수다.
//   축별 부족을 한 번 계산해 출발 한마디를 고르고, 같은 값을 Served로 알려
//   GameManager가 방명록 글 등록·방문 결과 확정·명단의 파견 종료를 요청하게 한다.
//   화면 약속(켜질 때 UIManager에 연결, 꺼질 때 해제): IServiceStatus(영업 HUD), IKitchenBurners(화구 표시), IKitchenOrders(요리 화면).
//     값은 모두 OrderBook·명단에서 읽기만 한다. 주문표는 착석해 연 주문부터 서빙 전까지(취소 제외), 들고 있는 접시는 완성으로 보인다.
//     요리 화면이 다룰 주문은 완성 > (빈 화구가 있으면) 대기 > 조리 중 순의 첫 주문이다.
//     조리 시작 요청은 주문 없음 → 착석 안 함 → 주방에서 멂 → 일시 정지 → 화구 사용 중 → 재료 틀림 → 잠긴 레시피 → 도구 틀림
//     → 재고 부족 순으로 거절하고, 통과하면 OrderBook에 맡긴다. 미리보기는 같은 검사를 상태를 바꾸지 않고 한다.
//     접시 들기 요청은 이미 들고 있음 → 주방에서 멂 → 안 익음 순으로 거절한다. 거절이면 상태를 바꾸지 않는다.
//   E(주방, 재료 확인): 요리 화면이 등록돼 있으면 그 화면을 열고, 없으면 옛 재료 패널을 연다. 어느 쪽이든 열린 동안 이동·E를 막는다.
//   MealPrompt — E 처리(Interact)와 같은 조건으로 지금 E의 대상과 동작 이름을 돌려준다([E] 표시용).
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using DOB.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.Manager
{
    /// <summary>그날 손님들의 주문을 주방 버튼, 식당 상호작용 및 영업 중 지갑에 연결한다.</summary>
    public sealed class FirstGuestKitchen : MonoBehaviour, IServiceStatus, IKitchenBurners, IKitchenOrders
    {
        [SerializeField] private FirstGuestService service;
        private GameObject recipePanel;
        private Text ingredients;
        private Text receipt;
        private Button confirm;
        private Button cancel;

        private OrderBook book;
        private string panelVisit;
        private EconomyManager economy;
        private KitchenManager kitchenManager;
        private KitchenRules Rules => kitchenManager.Rules;
        /// <summary>지금 금화(경제의 값).</summary>
        public int Gold => economy ? economy.Gold : 0;
        /// <summary>지금 마늘 재고(경제의 양념 재고).</summary>
        public int GarlicStock => economy ? economy.SpiceCount(GarlicId) : 0;
        /// <summary>오늘 서빙한 주문이 하나라도 있다.</summary>
        public bool HasServed
        {
            get
            {
                if (book != null)
                    foreach (var order in book.Orders)
                        if (order.State == FirstGuestOrderState.Served) return true;
                return false;
            }
        }
        /// <summary>플레이어가 완성된 접시를 들고 있다.</summary>
        public bool IsCarrying => book?.Carried != null;
        /// <summary>서빙이 성공한 한 끼. 축별 부족·실제 위험·요리가 막은 칸은 RiskAxes.All 순서다.</summary>
        public readonly struct ServedMeal
        {
            public readonly string VisitId;
            public readonly string GuestName;
            public readonly string Line;
            public readonly string Dish;
            public readonly bool UsedSeasoning;
            /// <summary>넣은 양념 이름(입맛 기준의 이름과 같다). 없으면 빈 목록.</summary>
            public readonly IReadOnlyList<string> Seasonings;
            public readonly IReadOnlyList<int> Shortfalls;
            public readonly IReadOnlyList<int> Risks;
            public readonly IReadOnlyList<int> Protections;

            public ServedMeal(string visitId, string guestName, string line, string dish, bool usedSeasoning,
                IReadOnlyList<int> shortfalls, IReadOnlyList<int> risks, IReadOnlyList<int> protections,
                IReadOnlyList<string> seasonings = null)
            {
                VisitId = visitId;
                GuestName = guestName;
                Line = line;
                Dish = dish;
                UsedSeasoning = usedSeasoning;
                Seasonings = seasonings ?? System.Array.Empty<string>();
                Shortfalls = shortfalls;
                Risks = risks;
                Protections = protections;
            }
        }

        // 빵 전용 옛 화면 약속·옛 레시피 패널이 다루는 레시피와 양념.
        private const string BreadId = "Bread";
        private const string GarlicId = "garlic";
        private const string Flour = "밀가루";
        private RecipeEntry Bread => Rules.TryGet(BreadId, out var bread) ? bread : default;
        private int BreadFlour => Bread.ingredients != null && Bread.ingredients.Length > 0 ? Bread.ingredients[0].count : 0;
        /// <summary>서빙이 성공할 때 한 번 알린다 (방명록 등록·방문 결과 확정·파견 종료용).</summary>
        public event System.Action<ServedMeal> Served;
        /// <summary>조리가 끝나 접시가 완성될 때 한 번 알린다 (효과음). 불러오기 복원에서는 알리지 않는다.</summary>
        public event System.Action<string> CookFinished;
        // 양념이 부족하면 무료 기본 재료로 굽는다 (첫 플레이 E11). 양념 칸은 하나라 마늘은 한 개다.
        private int GarlicForBread => GarlicStock >= 1 ? 1 : 0;
        // 입맛 기준(SO_TasteRules)의 양념 이름은 시장 표의 이름과 같다.
        private string SpiceName(string spiceId) => spiceId != null && economy.Rules.TryGet(spiceId, out var spice) ? spice.name : spiceId;
        private string RecipeName(string recipeId) => recipeId != null && Rules.TryGet(recipeId, out var recipe) ? recipe.name : "요리";

        private bool ScreenOpen => recipePanel.activeSelf || UIManager.IsKitchenOpen;
        private bool HasRoster => book != null && service.Guests && service.Guests.Roster != null;

        // ---- IServiceStatus: 영업 HUD ----
        int IServiceStatus.Day => service.Day;
        int IServiceStatus.GuestsArrived => HasRoster ? service.Guests.Roster.Entered : 0;
        int IServiceStatus.OrderCount
        {
            get
            {
                int count = 0;
                if (HasRoster)
                    foreach (var order in book.Orders)
                        if (IsTicket(order)) count++;
                return count;
            }
        }
        string IServiceStatus.OrderIdAt(int index) => TicketAt(index)?.VisitId;
        string IServiceStatus.GuestNameAt(int index) => TicketAt(index) is FirstGuestOrder order ? NameOf(order.VisitId) : null;
        // 손님 초상 id 데이터가 아직 없다 (홍준우 인계).
        string IServiceStatus.PortraitIdAt(int index) => null;
        // 대기 주문은 아직 요리가 정해지지 않아 null이다.
        string IServiceStatus.DishIdAt(int index) => TicketAt(index)?.RecipeId;
        OrderTicketState IServiceStatus.StateAt(int index)
        {
            switch (TicketAt(index)?.State)
            {
                case FirstGuestOrderState.Cooking: return OrderTicketState.Cooking;
                case FirstGuestOrderState.Ready:
                case FirstGuestOrderState.Carrying: return OrderTicketState.Ready;
                default: return OrderTicketState.Waiting;
            }
        }
        string IServiceStatus.HeldDishId => HasRoster ? book.Carried?.RecipeId : null;
        string IServiceStatus.HeldOrderId => HasRoster ? book.Carried?.VisitId : null;

        private static bool IsTicket(FirstGuestOrder order) =>
            order.State != FirstGuestOrderState.Served && order.State != FirstGuestOrderState.Cancelled;

        // 주문표 순서 = 착석해 주문을 연 순서. 목록을 새로 만들지 않고 센다.
        private FirstGuestOrder TicketAt(int index)
        {
            if (!HasRoster || index < 0)
                return null;
            foreach (var order in book.Orders)
                if (IsTicket(order) && index-- == 0)
                    return order;
            return null;
        }

        // ---- IKitchenBurners: 화구 표시 ----
        int IKitchenBurners.Count => HasRoster && service.Kitchen ? book.BurnerCount : 0;
        Transform IKitchenBurners.BurnerAt(int index) =>
            HasRoster && index >= 0 && index < book.BurnerCount ? kitchenManager.BurnerAt(index, service.Kitchen) : null;
        BurnerState IKitchenBurners.StateAt(int index) => BurnerStateAt(index);
        string IKitchenBurners.DishIdAt(int index) => HasRoster ? book.AtBurner(index)?.RecipeId : null;
        float IKitchenBurners.RemainingSecondsAt(int index) => HasRoster ? book.AtBurner(index)?.RemainingSeconds ?? 0f : 0f;
        float IKitchenBurners.CookSecondsAt(int index) => Rules.CookSeconds;

        private BurnerState BurnerStateAt(int index)
        {
            var order = HasRoster ? book.AtBurner(index) : null;
            return order == null ? BurnerState.Empty : order.State == FirstGuestOrderState.Cooking ? BurnerState.Cooking : BurnerState.Done;
        }

        // ---- IKitchenOrders: 요리 화면 ----
        private FirstGuestOrder ScreenOrder => !HasRoster ? null
            : book.NextReady ?? (book.FreeBurner >= 0 ? book.NextWaiting : null) ?? book.Cooking ?? book.NextWaiting;
        string IKitchenOrders.GuestName => ScreenOrder is FirstGuestOrder order ? NameOf(order.VisitId) : null;
        string IKitchenOrders.OrderId => ScreenOrder?.VisitId;
        FirstGuestOrderState? IKitchenOrders.State => ScreenOrder?.State;
        float IKitchenOrders.CookSeconds => Rules.CookSeconds;
        float IKitchenOrders.RemainingSeconds => ScreenOrder?.RemainingSeconds ?? 0f;
        int IKitchenOrders.FlourRequired => BreadFlour;
        int IKitchenOrders.GarlicRequired => GarlicForBread;
        int IKitchenOrders.GarlicStock => GarlicStock;
        int IKitchenOrders.Protection(string axis, bool garlic) => RecipeJudge.Protection(Bread, economy.Rules, garlic ? GarlicId : null, axis);
        // 서빙 대금과 같은 식이다. 맛 점수는 화면이 다루는 주문의 손님 입맛으로 정한다.
        int IKitchenOrders.Price(bool garlic) => ScreenOrder is FirstGuestOrder order ? PaymentFor(order.VisitId, Bread, garlic ? GarlicId : null) : 0;
        IReadOnlyList<string> IKitchenOrders.UnlockedRecipes => kitchenManager ? kitchenManager.Unlocked : (IReadOnlyList<string>)System.Array.Empty<string>();
        int IKitchenOrders.StockOf(string id) =>
            id == null || economy == null ? 0 : Rules.IsBasic(id) ? int.MaxValue : economy.Rules.TryGet(id, out _) ? economy.SpiceCount(id) : economy.Materials.Count(id);

        DishPreview IKitchenOrders.Preview(string orderId, IReadOnlyDictionary<string, int> ingredients, string spiceId, string toolId)
        {
            var check = CheckCooking(orderId, ingredients, spiceId, toolId, out var recipe);
            if (string.IsNullOrEmpty(recipe.id))
                return new DishPreview(check, null, null, null, null, 0, 0);
            var protections = ExpeditionJudge.PerAxis(axis => RecipeJudge.Protection(recipe, economy.Rules, spiceId, axis));
            string guestId = VisitOf(orderId)?.GuestId;
            int taste = service.Guests.FlavorScoreFor(economy.Rules, guestId, recipe.name, spiceId != null ? new[] { SpiceName(spiceId) } : null);
            return new DishPreview(check, recipe.id, recipe.name, recipe.tool, protections, taste, PaymentFor(orderId, recipe, spiceId));
        }

        /// <summary>요리 화면의 조리 시작 요청(재료·양념·도구 조합). 거절이면 재고·주문을 바꾸지 않는다.</summary>
        public CookStartResult TryStartCooking(string orderId, IReadOnlyDictionary<string, int> ingredients, string spiceId, string toolId)
        {
            var check = CheckCooking(orderId, ingredients, spiceId, toolId, out var recipe);
            if (check != CookStartResult.Started)
                return check;
            switch (book.TryStart(orderId, recipe, spiceId))
            {
                case OrderStartResult.Started: ClosePanel(); return CookStartResult.Started;
                case OrderStartResult.BurnerBusy: return CookStartResult.BurnerBusy;
                case OrderStartResult.NotEnoughStock: return CookStartResult.NotEnoughStock;
                default: return CookStartResult.NoOrder;
            }
        }

        // 조리 시작과 미리보기가 같은 순서로 검사한다. 재료가 어떤 레시피와 맞으면 거절이어도 recipe를 채운다.
        private CookStartResult CheckCooking(string orderId, IReadOnlyDictionary<string, int> ingredients, string spiceId, string toolId, out RecipeEntry recipe)
        {
            recipe = default;
            var order = HasRoster ? book.Find(orderId) : null;
            if (order == null || order.State == FirstGuestOrderState.Cancelled || order.State == FirstGuestOrderState.Served)
                return CookStartResult.NoOrder;
            var match = kitchenManager.Identify(ingredients, toolId, out recipe);
            if (!service.SeatOf(orderId))
                return CookStartResult.NotSeated;
            if (!service.IsNearPlayer(service.Kitchen))
                return CookStartResult.NotNearKitchen;
            if (Time.timeScale <= 0f)
                return CookStartResult.Paused;
            if (order.State != FirstGuestOrderState.Waiting)
                return CookStartResult.NoOrder;
            if (book.FreeBurner < 0)
                return CookStartResult.BurnerBusy;
            if (match == RecipeMatch.WrongIngredients || spiceId != null && !economy.Rules.TryGet(spiceId, out _))
                return CookStartResult.WrongIngredients;
            if (match == RecipeMatch.Locked)
                return CookStartResult.RecipeLocked;
            if (match == RecipeMatch.WrongTool)
                return CookStartResult.WrongTool;
            foreach (var item in recipe.ingredients)
                if (!Rules.IsBasic(item.id) && economy.Materials.Count(item.id) < item.count)
                    return CookStartResult.NotEnoughStock;
            return spiceId != null && economy.SpiceCount(spiceId) < 1 ? CookStartResult.NotEnoughStock : CookStartResult.Started;
        }
        int? IKitchenOrders.KnownRisk(string axis) => ScreenOrder is FirstGuestOrder order ? VisitOf(order.VisitId)?.NotebookRisk(axis) : null;

        /// <summary>빵 전용 옛 조리 시작 요청(요리 화면이 새 멤버로 옮겨 가면 지운다). 밀가루 수와 마늘 0·1개를 받아 오븐 검사 없이 구운 빵을 굽는다.</summary>
        public CookStartResult TryStartCooking(string orderId, int flour, int garlic)
        {
            if (flour <= 0 || garlic < 0 || garlic > 1)
                return HasRoster && book.Find(orderId) != null ? CookStartResult.WrongIngredients : CookStartResult.NoOrder;
            return TryStartCooking(orderId, new Dictionary<string, int> { [Flour] = flour }, garlic > 0 ? GarlicId : null, null);
        }

        /// <summary>요리 화면의 접시 들기 요청. 거절이면 든 접시를 바꾸지 않는다.</summary>
        public PickUpResult TryPickUp(string orderId)
        {
            if (HasRoster && book.Carried != null)
                return PickUpResult.AlreadyHolding;
            if (!service.IsNearPlayer(service.Kitchen))
                return PickUpResult.NotNearKitchen;
            var order = HasRoster ? book.Find(orderId) : null;
            return order != null && order.State == FirstGuestOrderState.Ready && book.TryPickUp(orderId)
                ? PickUpResult.PickedUp : PickUpResult.NotCooked;
        }

        /// <summary>지금 E를 누르면 주방·서빙에서 처리될 대상과 동작 이름. Interact와 같은 조건이다. 없으면 (null, null).</summary>
        public (Transform Target, string Action) MealPrompt()
        {
            if (book == null || ScreenOpen)
                return (null, null);
            if (book.Carried is FirstGuestOrder carried)
                return service.NearestSeatedVisit() == carried.VisitId ? (service.SeatOf(carried.VisitId), "서빙") : (null, null);
            if (!service.IsNearPlayer(service.Kitchen))
                return (null, null);
            if (book.NextReady != null)
                return (service.Kitchen, "접시 들기");
            return book.NextWaiting != null && book.FreeBurner >= 0 ? (service.Kitchen, "재료 확인") : (null, null);
        }

        private GuestDialogueVisit VisitOf(string visitId) => service.Guests.Find(visitId)?.Visit;
        private string NameOf(string visitId) => VisitOf(visitId)?.GuestName ?? "손님";

        /// <summary>오늘 금화·재고를 소유한 경제와 레시피를 소유한 주방 총괄을 연결한다. GameManager가 하루를 열 때 부른다.</summary>
        public void StartDay(EconomyManager dayEconomy, KitchenManager dayKitchen)
        {
            economy = dayEconomy;
            kitchenManager = dayKitchen;
        }

        // 서빙 대금: 레시피 기본가·맛 점수·넣은 양념 수. 맛 점수는 손님 입맛과 접시 맛(레시피 + 양념)으로 정한다.
        private int PaymentFor(string visitId, RecipeEntry recipe, string spiceId)
        {
            string guestId = VisitOf(visitId)?.GuestId;
            int taste = service.Guests.FlavorScoreFor(economy.Rules, guestId, recipe.name, spiceId != null ? new[] { SpiceName(spiceId) } : null);
            return PriceJudge.Payment(economy.Rules, recipe.basePrice, taste, spiceId != null ? 1 : 0);
        }

        // Additive UI가 로드되면 해당 씬이 소유한 버튼과 표시창을 한 번 연결한다.
        public void ConnectHud(BasementHud hud)
        {
            recipePanel = hud.RecipePanel;
            ingredients = hud.Ingredients;
            receipt = hud.Receipt;
            confirm = hud.Confirm;
            cancel = hud.Cancel;
        }

        /// <summary>연 순서의 주문(취소·서빙 포함). 주방이 꺼져 있으면 빈 목록.</summary>
        public IReadOnlyList<FirstGuestOrder> Orders => book != null ? book.Orders : (IReadOnlyList<FirstGuestOrder>)System.Array.Empty<FirstGuestOrder>();

        /// <summary>수동 저장을 불러올 때 영업 중 주문을 되살린다. 주방이 켜진 뒤에만, 주문이 비어 있을 때만 된다. 착석 알림은 다시 보내지 않는다.
        /// 금화·양념은 경제가 따로 되살린다.</summary>
        public bool RestoreProgress(IEnumerable<OrderProgress> orders) => book != null && book.Restore(orders);

        /// <summary>조리 시계가 허용한 경과 시간을 조리 중인 주문에 넘긴다. GameManager가 영업 중에만 부른다.</summary>
        public void Tick(float cookingDeltaSeconds) => book?.Advance(cookingDeltaSeconds);

        private void OnEnable()
        {
            recipePanel.SetActive(false);
            receipt.text = string.Empty;
            book = new OrderBook(Rules, economy.Materials, economy.Spices, () => kitchenManager.OwnedBurners);
            book.Finished += OnFinished;
            service.GuestSeated += OpenOrder;
            service.Guests.VisitEnded += CancelOrder;
            service.MealInteraction = Interact;
            service.MealGuidance = GetGuidance;
            service.KitchenIsOpen = () => ScreenOpen;
            service.MealPrompt = MealPrompt;
            UIManager.ConnectServiceStatus(this);
            UIManager.ConnectKitchenBurners(this);
            UIManager.ConnectKitchenOrders(this);
            confirm.onClick.AddListener(Confirm);
            cancel.onClick.AddListener(ClosePanel);
        }

        private void OnDisable()
        {
            service.GuestSeated -= OpenOrder;
            if (service.Guests) service.Guests.VisitEnded -= CancelOrder;
            service.MealInteraction = null;
            service.MealGuidance = null;
            service.KitchenIsOpen = null;
            service.MealPrompt = null;
            UIManager.DisconnectServiceStatus(this);
            UIManager.DisconnectKitchenBurners(this);
            UIManager.DisconnectKitchenOrders(this);
            UIManager.CloseKitchen();
            if (confirm) confirm.onClick.RemoveListener(Confirm);
            if (cancel) cancel.onClick.RemoveListener(ClosePanel);
            if (recipePanel) ClosePanel();
        }

        private void OpenOrder(string visitId, Transform seat) => book.TryOpen(visitId);

        private void OnFinished(FirstGuestOrder order) => CookFinished?.Invoke(order.VisitId);

        // 파견은 서빙을 마친 뒤라 취소할 주문이 없다. 돌려보냄·대기 이탈이면 조리 칸과 들고 있던 접시가 함께 빈다.
        private void CancelOrder(string visitId, ServiceVisitEnd end) => book?.Cancel(visitId);

        private string GetGuidance()
        {
            if (book == null)
                return null;
            bool atKitchen = service.IsNearPlayer(service.Kitchen);
            if (book.Carried is FirstGuestOrder carried)
            {
                string name = NameOf(carried.VisitId);
                return service.IsNearPlayer(service.SeatOf(carried.VisitId)) ? $"E  {name}에게 서빙" : $"접시를 들었어요. {name}의 자리로 돌아가세요.";
            }
            if (book.NextReady is FirstGuestOrder ready)
                return atKitchen ? $"E  {NameOf(ready.VisitId)}의 접시 들기" : $"{RecipeName(ready.RecipeId)}이(가) 완성됐어요. 주방에서 접시를 들어 주세요.";
            if (book.NextWaiting is FirstGuestOrder waiting && book.FreeBurner >= 0)
                return atKitchen ? "E  주방에서 재료 확인" : $"오른쪽 주방에서 {NameOf(waiting.VisitId)}의 요리를 준비하세요.";
            if (book.Cooking is FirstGuestOrder cooking)
                return $"{NameOf(cooking.VisitId)}의 {RecipeName(cooking.RecipeId)}을(를) 만들고 있어요.  {Mathf.CeilToInt(cooking.RemainingSeconds)}초";
            return null;
        }

        private bool Interact()
        {
            if (book == null || ScreenOpen)
                return false;

            if (book.Carried != null)
            {
                // 접시 주인이 아닌 손님 자리에서는 장부가 거절해 접시를 계속 든다.
                string target = service.NearestSeatedVisit();
                if (target == null || !book.TryServe(target))
                    return false;
                Serve(book.Find(target));
                return true;
            }

            if (!service.IsNearPlayer(service.Kitchen))
                return false;
            if (book.NextReady != null)
                return book.TryPickUp();
            var next = book.NextWaiting;
            if (next == null || book.FreeBurner < 0)
                return false;
            if (UIManager.HasKitchenScreen)
                return UIManager.OpenKitchen(0);
            panelVisit = next.VisitId;
            string seasoning = GarlicForBread > 0 ? $"  +  {SpiceName(GarlicId)} {GarlicForBread}개" : "  ·  양념 없이 굽기";
            ingredients.text = $"{NameOf(panelVisit)}의 {Bread.name}\n{Flour} {BreadFlour}개{seasoning}\n{SpiceName(GarlicId)} 재고 {GarlicStock}개    ·    조리 {Rules.CookSeconds:0.#}초";
            recipePanel.SetActive(true);
            return true;
        }

        private void Serve(FirstGuestOrder order)
        {
            Rules.TryGet(order.RecipeId, out var recipe);
            string spice = order.Spice;
            int payment = PaymentFor(order.VisitId, recipe, spice);
            economy.Earn(payment);
            var visit = VisitOf(order.VisitId);
            string name = NameOf(order.VisitId);
            string line = string.Empty, resolve = string.Empty;
            int[] shortfalls = null, risks = null, protections = null;
            if (visit != null)
            {
                line = TasteLine(visit, recipe, spice);
                // 출발 한마디와 방문 결과의 등급이 어긋나지 않도록 부족을 한 번만 계산해 함께 쓴다.
                shortfalls = ExpeditionJudge.Shortfalls(visit.ActualRisk, axis => RecipeJudge.Protection(recipe, economy.Rules, spice, axis));
                // 보고 화면이 "막아냈다" 줄을 그릴 수 있도록 같은 입력의 위험과 효과 칸도 함께 넘긴다.
                risks = ExpeditionJudge.PerAxis(visit.ActualRisk);
                protections = ExpeditionJudge.PerAxis(axis => RecipeJudge.Protection(recipe, economy.Rules, spice, axis));
                resolve = $"\n“{visit.DepartureLine(ExpeditionJudge.Total(shortfalls))}”";
            }
            receipt.text = $"{name}   “{line}”{resolve}\n서빙 +{payment}금화   ·   보유 {Gold}금화";
            if (receipt.preferredHeight > receipt.rectTransform.rect.height)
                receipt.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, receipt.preferredHeight);
            Served?.Invoke(new ServedMeal(order.VisitId, name, line, recipe.name, spice != null, shortfalls, risks, protections,
                spice != null ? new[] { SpiceName(spice) } : null));
        }

        // 맛 소감 원고는 구운 빵(양념 없음·마늘)만 있다. 💡 다른 요리·양념은 원고 전까지 임의 문장을 쓴다(송예찬 인계).
        private string TasteLine(GuestDialogueVisit visit, RecipeEntry recipe, string spice)
        {
            if (recipe.id == BreadId && (spice == null || spice == GarlicId))
                return visit.TasteLine(spice == GarlicId);
            return spice != null ? $"{SpiceName(spice)} 넣은 {recipe.name}, 잘 먹었어." : $"{recipe.name}, 잘 먹었어.";
        }

        private void Confirm()
        {
            if (recipePanel.activeSelf)
                TryStartCooking(panelVisit, BreadFlour, GarlicForBread);
        }

        private void ClosePanel() => recipePanel.SetActive(false);
    }
}
