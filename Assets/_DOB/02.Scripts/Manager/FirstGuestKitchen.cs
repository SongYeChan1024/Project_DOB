// 착석한 방문에 주문 하나를 연다. 주방에서 레시피를 확인하고 확정하면 월드 입력을 돌려준다.
// 조리 시간은 대화 상태와 독립적으로 흐른다. 접시는 주방에서 들고 해당 방문의 자리에서만 서빙한다.
// 서빙이 성공하면 Served로 알려 GameManager가 방명록 글을 등록하게 한다.
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
        private int gold;
        private int? startingGold;
        private int? startingGarlic;
        public int DayStartGold => startingGold ?? recipe.StartGold;
        public int DayStartGarlic => startingGarlic ?? recipe.StartGarlic;
        public int Gold => gold;
        public int GarlicStock => order?.GarlicStock ?? DayStartGarlic;
        public bool HasServed => order != null && order.State == FirstGuestOrderState.Served;
        public bool IsCarrying => order != null && order.State == FirstGuestOrderState.Carrying;
        /// <summary>서빙이 성공할 때 방문 ID, 손님 이름, 맛 소감 한 줄을 한 번 알린다 (방명록 등록용).</summary>
        public event System.Action<string, string, string> Served;
        // 양념이 부족한 날에도 무료 기본 재료로 영업을 진행한다 (첫 플레이 E11).
        private int GarlicForBread => DayStartGarlic >= recipe.GarlicRequired ? recipe.GarlicRequired : 0;

        public void StartDay(int goldAtOpening, int garlicAtOpening)
        {
            startingGold = goldAtOpening;
            startingGarlic = garlicAtOpening;
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
                    return service.IsNearPlayer(service.Kitchen) ? "E  주방에서 재료 확인" : "오른쪽 주방에서 하루의 구운 빵을 준비하세요.";
                case FirstGuestOrderState.Cooking:
                    return $"구운 빵을 굽고 있어요.  {Mathf.CeilToInt(order.RemainingSeconds)}초";
                case FirstGuestOrderState.Ready:
                    return service.IsNearPlayer(service.Kitchen) ? "E  하루의 접시 들기" : "빵이 완성됐어요. 주방에서 접시를 들어 주세요.";
                case FirstGuestOrderState.Carrying:
                    return service.IsNearPlayer(service.SeatedAt) ? "E  하루에게 서빙" : "접시를 들었어요. 하루가 앉은 자리로 돌아가세요.";
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
                string line = GarlicForBread > 0 ? guest.ServeLine : guest.PlainBreadLine;
                receipt.text = $"{guest.GuestName}   “{line}”\n서빙 +{payment}금화   ·   보유 {gold}금화   ·   남은 마늘 {order.GarlicStock}개";
                Served?.Invoke(service.VisitId, guest.GuestName, line);
                service.TryDepartForDungeon(service.VisitId);
                return true;
            }

            if (!service.IsNearPlayer(service.Kitchen))
                return false;
            if (order.State == FirstGuestOrderState.Ready)
                return order.TryPickUp(service.VisitId);
            if (order.State != FirstGuestOrderState.Waiting)
                return false;
            string seasoning = GarlicForBread > 0 ? $"  +  마늘 {GarlicForBread}개" : "  ·  양념 없이 굽기";
            ingredients.text = $"구운 빵\n밀가루 {recipe.FlourRequired}개{seasoning}\n마늘 재고 {order.GarlicStock}개    ·    조리 {recipe.CookSeconds:0.#}초";
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
