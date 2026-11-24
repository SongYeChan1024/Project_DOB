// FirstGuestKitchen — 주문 장부(OrderBook)를 주방 레시피 패널, 식당 상호작용, 영업 중 지갑에 연결한다.
// 호출자: FirstGuestService(E 입력·안내 위임, 착석 알림), GameManager(StartDay·ConnectHud·조리 Δ Tick, Served 구독).
// 호출  : OrderBook, GuestManager(방문 원고 조회, VisitEnded), ExpeditionJudge.
// 상태  : 영업 시작 금화·마늘, 현재 금화, 하루 주문 장부, 레시피 패널이 가리키는 방문.
// 의사코드:
//   착석 알림마다 그 방문의 주문을 연다. 서빙 전에 방문이 끝나면(돌려보냄·대기 이탈) 그 주문을 취소한다.
//   E(주방 근처): 완성된 접시가 있으면 먼저 완성된 것을 든다. 아니면 조리 칸이 비었을 때 먼저 연 대기 주문의 레시피를 연다.
//   확정하면 공유 재고가 레시피 양만큼 있으면 마늘을 넣고, 없으면 양념 없이 굽는다(첫 플레이 E11).
//   조리 시간은 GameManager가 넘기는 조리 Δ로만 흐른다(응대·재료 선택 중에도 흐르고, 일시 정지·전환에 멈춘다).
//   E(접시를 든 채 손님 자리 근처): 가장 가까운 착석 손님에게 서빙을 요청한다. 접시 주인이 아니면 장부가 거절한다.
//   서빙이 성공하면 축별 부족을 한 번 계산해 출발 한마디를 고르고, 같은 값을 Served로 알려
//   GameManager가 방명록 글 등록·방문 결과 확정·명단의 파견 종료를 요청하게 한다.
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.Manager
{
    /// <summary>그날 손님들의 주문을 주방 버튼, 식당 상호작용 및 영업 중 지갑에 연결한다.</summary>
    public sealed class FirstGuestKitchen : MonoBehaviour
    {
        [SerializeField] private FirstGuestService service;
        [SerializeField] private FirstServiceRecipe recipe;
        private GameObject recipePanel;
        private Text ingredients;
        private Text receipt;
        private Button confirm;
        private Button cancel;

        private OrderBook book;
        private string panelVisit;
        private int gold;
        private int? startingGold;
        private int? startingGarlic;
        public int DayStartGold => startingGold ?? recipe.StartGold;
        public int DayStartGarlic => startingGarlic ?? recipe.StartGarlic;
        public int Gold => gold;
        public int GarlicStock => book?.GarlicStock ?? DayStartGarlic;
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
        /// <summary>레시피(재료 선택) 패널이 열려 있다. 손님 입장·대기 시계를 멈추는 사유다.</summary>
        public bool IsChoosingIngredients => recipePanel && recipePanel.activeSelf;
        /// <summary>서빙이 성공한 한 끼. 축별 부족·실제 위험·요리가 막은 칸은 RiskAxes.All 순서다.</summary>
        public readonly struct ServedMeal
        {
            public readonly string VisitId;
            public readonly string GuestName;
            public readonly string Line;
            public readonly string Dish;
            public readonly bool UsedSeasoning;
            public readonly IReadOnlyList<int> Shortfalls;
            public readonly IReadOnlyList<int> Risks;
            public readonly IReadOnlyList<int> Protections;

            public ServedMeal(string visitId, string guestName, string line, string dish, bool usedSeasoning,
                IReadOnlyList<int> shortfalls, IReadOnlyList<int> risks, IReadOnlyList<int> protections)
            {
                VisitId = visitId;
                GuestName = guestName;
                Line = line;
                Dish = dish;
                UsedSeasoning = usedSeasoning;
                Shortfalls = shortfalls;
                Risks = risks;
                Protections = protections;
            }
        }

        private const string Dish = "구운 빵";
        /// <summary>서빙이 성공할 때 한 번 알린다 (방명록 등록·방문 결과 확정·파견 종료용).</summary>
        public event System.Action<ServedMeal> Served;
        // 양념이 부족하면 무료 기본 재료로 굽는다 (첫 플레이 E11). 재고는 주문 사이에서 공유한다.
        private int GarlicForBread => GarlicStock >= recipe.GarlicRequired ? recipe.GarlicRequired : 0;

        private GuestDialogueVisit VisitOf(string visitId) => service.Guests.Find(visitId)?.Visit;
        private string NameOf(string visitId) => VisitOf(visitId)?.GuestName ?? "손님";

        public void StartDay(int goldAtOpening, int garlicAtOpening)
        {
            startingGold = goldAtOpening;
            startingGarlic = garlicAtOpening;
            // 주방은 영업 단계에서야 켜지므로, 아침보고·준비 화면도 저장된 금화를 보도록 여기서 맞춘다.
            gold = goldAtOpening;
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

        /// <summary>조리 시계가 허용한 경과 시간을 조리 중인 주문에 넘긴다. GameManager가 영업 중에만 부른다.</summary>
        public void Tick(float cookingDeltaSeconds) => book?.Advance(cookingDeltaSeconds);

        private void OnEnable()
        {
            recipePanel.SetActive(false);
            receipt.text = string.Empty;
            gold = DayStartGold;
            book = new OrderBook(DayStartGarlic, recipe.CookSeconds, recipe.FlourRequired, recipe.BreadBasePrice);
            service.GuestSeated += OpenOrder;
            service.Guests.VisitEnded += CancelOrder;
            service.MealInteraction = Interact;
            service.MealGuidance = GetGuidance;
            service.KitchenIsOpen = () => recipePanel.activeSelf;
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
            if (confirm) confirm.onClick.RemoveListener(Confirm);
            if (cancel) cancel.onClick.RemoveListener(ClosePanel);
            if (recipePanel) ClosePanel();
        }

        private void OpenOrder(string visitId, Transform seat) => book.TryOpen(visitId);

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
                return atKitchen ? $"E  {NameOf(ready.VisitId)}의 접시 들기" : "빵이 완성됐어요. 주방에서 접시를 들어 주세요.";
            if (book.Cooking is FirstGuestOrder cooking)
                return $"{NameOf(cooking.VisitId)}의 구운 빵을 굽고 있어요.  {Mathf.CeilToInt(cooking.RemainingSeconds)}초";
            if (book.NextWaiting is FirstGuestOrder waiting)
                return atKitchen ? "E  주방에서 재료 확인" : $"오른쪽 주방에서 {NameOf(waiting.VisitId)}의 구운 빵을 준비하세요.";
            return null;
        }

        private bool Interact()
        {
            if (book == null || recipePanel.activeSelf)
                return false;

            if (book.Carried != null)
            {
                // 접시 주인이 아닌 손님 자리에서는 장부가 거절해 접시를 계속 든다.
                string target = service.NearestSeatedVisit();
                if (target == null || !book.TryServe(target, out int payment))
                    return false;
                Serve(book.Find(target), payment);
                return true;
            }

            if (!service.IsNearPlayer(service.Kitchen))
                return false;
            if (book.NextReady != null)
                return book.TryPickUp();
            var next = book.NextWaiting;
            if (next == null || book.Cooking != null)
                return false;
            panelVisit = next.VisitId;
            string seasoning = GarlicForBread > 0 ? $"  +  마늘 {GarlicForBread}개" : "  ·  양념 없이 굽기";
            ingredients.text = $"{NameOf(panelVisit)}의 구운 빵\n밀가루 {recipe.FlourRequired}개{seasoning}\n마늘 재고 {GarlicStock}개    ·    조리 {recipe.CookSeconds:0.#}초";
            recipePanel.SetActive(true);
            return true;
        }

        private void Serve(FirstGuestOrder order, int payment)
        {
            gold += payment;
            var visit = VisitOf(order.VisitId);
            string name = NameOf(order.VisitId);
            bool hasGarlic = order.UsedGarlic > 0;
            string line = string.Empty, resolve = string.Empty;
            int[] shortfalls = null, risks = null, protections = null;
            if (visit != null)
            {
                line = visit.TasteLine(hasGarlic);
                // 출발 한마디와 방문 결과의 등급이 어긋나지 않도록 부족을 한 번만 계산해 함께 쓴다.
                shortfalls = ExpeditionJudge.Shortfalls(visit.ActualRisk, axis => recipe.Protection(axis, hasGarlic));
                // 보고 화면이 "막아냈다" 줄을 그릴 수 있도록 같은 입력의 위험과 효과 칸도 함께 넘긴다.
                risks = ExpeditionJudge.PerAxis(visit.ActualRisk);
                protections = ExpeditionJudge.PerAxis(axis => recipe.Protection(axis, hasGarlic));
                resolve = $"\n“{visit.DepartureLine(ExpeditionJudge.Total(shortfalls))}”";
            }
            receipt.text = $"{name}   “{line}”{resolve}\n서빙 +{payment}금화   ·   보유 {gold}금화   ·   남은 마늘 {GarlicStock}개";
            if (receipt.preferredHeight > receipt.rectTransform.rect.height)
                receipt.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, receipt.preferredHeight);
            Served?.Invoke(new ServedMeal(order.VisitId, name, line, Dish, hasGarlic, shortfalls, risks, protections));
        }

        private void Confirm()
        {
            if (Time.timeScale > 0f && recipePanel.activeSelf && service.IsNearPlayer(service.Kitchen)
                && book.TryStart(panelVisit, recipe.FlourRequired, GarlicForBread))
                ClosePanel();
        }

        private void ClosePanel() => recipePanel.SetActive(false);
    }
}
