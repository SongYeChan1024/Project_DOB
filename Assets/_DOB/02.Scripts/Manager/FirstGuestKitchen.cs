// 착석한 방문에 주문 하나를 연다. 주방에서 레시피를 확인하고 확정하면 월드 입력을 돌려준다.
// 조리 시간은 대화 상태와 독립적으로 흐른다. 접시는 주방에서 들고 해당 방문의 자리에서만 서빙한다.
// 서빙이 성공하면 축별 부족을 한 번 계산해 출발 한마디를 고르고, 같은 값을 Served로 알려
// GameManager가 방명록 글 등록과 방문 결과 확정을 요청하게 한다.
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.Manager
{
    /// <summary>첫 손님의 주문을 주방 버튼, 식당 상호작용 및 영업 중 지갑에 연결한다.</summary>
    public sealed class FirstGuestKitchen : MonoBehaviour
    {
        [SerializeField] private FirstGuestService service;
        [SerializeField] private FirstServiceRecipe recipe;
        [SerializeField] private GuestDialogueProfile guest;
        private GameObject recipePanel;
        private Text ingredients;
        private Text receipt;
        private Button confirm;
        private Button cancel;

        private FirstGuestOrder order;
        private GuestDialogueVisit orderGuest;
        private string GuestName => orderGuest?.GuestName ?? guest.GuestName;
        private int gold;
        private int? startingGold;
        private int? startingGarlic;
        public int DayStartGold => startingGold ?? recipe.StartGold;
        public int DayStartGarlic => startingGarlic ?? recipe.StartGarlic;
        public int Gold => gold;
        public int GarlicStock => order?.GarlicStock ?? DayStartGarlic;
        public bool HasServed => order != null && order.State == FirstGuestOrderState.Served;
        public bool IsCarrying => order != null && order.State == FirstGuestOrderState.Carrying;
        /// <summary>서빙이 성공한 한 끼. 축별 부족은 RiskAxes.All 순서이며, 생성된 방문이 없으면 null이다.</summary>
        public readonly struct ServedMeal
        {
            public readonly string VisitId;
            public readonly string GuestName;
            public readonly string Line;
            public readonly string Dish;
            public readonly bool UsedSeasoning;
            public readonly IReadOnlyList<int> Shortfalls;

            public ServedMeal(string visitId, string guestName, string line, string dish, bool usedSeasoning, IReadOnlyList<int> shortfalls)
            {
                VisitId = visitId;
                GuestName = guestName;
                Line = line;
                Dish = dish;
                UsedSeasoning = usedSeasoning;
                Shortfalls = shortfalls;
            }
        }

        private const string Dish = "구운 빵";
        /// <summary>서빙이 성공할 때 한 번 알린다 (방명록 등록과 방문 결과 확정용).</summary>
        public event System.Action<ServedMeal> Served;
        // 양념이 부족한 날에도 무료 기본 재료로 영업을 진행한다 (첫 플레이 E11).
        private int GarlicForBread => DayStartGarlic >= recipe.GarlicRequired ? recipe.GarlicRequired : 0;

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

        private void OnEnable()
        {
            recipePanel.SetActive(false);
            receipt.text = string.Empty;
            gold = DayStartGold;
            service.GuestSeated += OpenOrder;
            service.MealInteraction = Interact;
            service.MealGuidance = GetGuidance;
            service.KitchenIsOpen = () => recipePanel.activeSelf;
            confirm.onClick.AddListener(Confirm);
            cancel.onClick.AddListener(ClosePanel);
            if (service.IsSeated)
                OpenOrder(service.VisitId, service.SeatedAt);
        }

        private void OnDisable()
        {
            service.GuestSeated -= OpenOrder;
            service.MealInteraction = null;
            service.MealGuidance = null;
            service.KitchenIsOpen = null;
            if (confirm) confirm.onClick.RemoveListener(Confirm);
            if (cancel) cancel.onClick.RemoveListener(ClosePanel);
            if (recipePanel) ClosePanel();
        }

        private void OpenOrder(string visitId, Transform seat)
        {
            if (order != null && order.VisitId == visitId)
                return;
            orderGuest = service.CurrentVisit;
            order = new FirstGuestOrder(visitId, recipe.CookSeconds, DayStartGarlic,
                recipe.FlourRequired, GarlicForBread, recipe.BreadBasePrice);
        }

        private void Update()
        {
            if (!SceneLoadService.IsLoading && order != null && order.State == FirstGuestOrderState.Cooking)
                order.Advance(Time.deltaTime);
        }

        private string GetGuidance()
        {
            if (order == null)
                return null;
            switch (order.State)
            {
                case FirstGuestOrderState.Waiting:
                    return service.IsNearPlayer(service.Kitchen) ? "E  주방에서 재료 확인" : $"오른쪽 주방에서 {GuestName}의 구운 빵을 준비하세요.";
                case FirstGuestOrderState.Cooking:
                    return $"구운 빵을 굽고 있어요.  {Mathf.CeilToInt(order.RemainingSeconds)}초";
                case FirstGuestOrderState.Ready:
                    return service.IsNearPlayer(service.Kitchen) ? $"E  {GuestName}의 접시 들기" : "빵이 완성됐어요. 주방에서 접시를 들어 주세요.";
                case FirstGuestOrderState.Carrying:
                    return service.IsNearPlayer(service.SeatedAt) ? $"E  {GuestName}에게 서빙" : $"접시를 들었어요. {GuestName}의 자리로 돌아가세요.";
                default:
                    return null;
            }
        }

        private bool Interact()
        {
            if (order == null || !service.IsSeated || recipePanel.activeSelf)
                return false;

            if (order.State == FirstGuestOrderState.Carrying)
            {
                if (!service.IsNearPlayer(service.SeatedAt) || !order.TryServe(service.VisitId, out int payment))
                    return false;
                gold += payment;
                bool hasGarlic = GarlicForBread > 0;
                string line;
                string resolve = string.Empty;
                int[] shortfalls = null;
                if (orderGuest != null)
                {
                    line = orderGuest.TasteLine(hasGarlic);
                    // 출발 한마디와 방문 결과의 등급이 어긋나지 않도록 부족을 한 번만 계산해 함께 쓴다.
                    shortfalls = ExpeditionJudge.Shortfalls(orderGuest.ActualRisk, axis => recipe.Protection(axis, hasGarlic));
                    resolve = $"\n“{orderGuest.DepartureLine(ExpeditionJudge.Total(shortfalls))}”";
                }
                else
                    line = hasGarlic ? guest.ServeLine : guest.PlainBreadLine;
                receipt.text = $"{GuestName}   “{line}”{resolve}\n서빙 +{payment}금화   ·   보유 {gold}금화   ·   남은 마늘 {order.GarlicStock}개";
                if (receipt.preferredHeight > receipt.rectTransform.rect.height)
                    receipt.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, receipt.preferredHeight);
                string servedVisit = order.VisitId;
                Served?.Invoke(new ServedMeal(servedVisit, GuestName, line, Dish, hasGarlic, shortfalls));
                service.TryDepartForDungeon(servedVisit);
                return true;
            }

            if (!service.IsNearPlayer(service.Kitchen))
                return false;
            if (order.State == FirstGuestOrderState.Ready)
                return order.TryPickUp(service.VisitId);
            if (order.State != FirstGuestOrderState.Waiting)
                return false;
            string seasoning = GarlicForBread > 0 ? $"  +  마늘 {GarlicForBread}개" : "  ·  양념 없이 굽기";
            ingredients.text = $"{GuestName}의 구운 빵\n밀가루 {recipe.FlourRequired}개{seasoning}\n마늘 재고 {order.GarlicStock}개    ·    조리 {recipe.CookSeconds:0.#}초";
            recipePanel.SetActive(true);
            return true;
        }

        private void Confirm()
        {
            if (Time.timeScale > 0f && recipePanel.activeSelf && service.IsSeated && service.IsNearPlayer(service.Kitchen)
                && order.TryStart(service.VisitId, recipe.FlourRequired, GarlicForBread))
                ClosePanel();
        }

        private void ClosePanel() => recipePanel.SetActive(false);
    }
}
