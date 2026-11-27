// =====================================================================
// IKitchenOrders — 요리 화면(KitchenPresenter)이 실제 주방 주문을 읽고 요청하는 약속
// 역할   : 화면은 이 약속으로 주문 · 조리 상태 · 재고 · 효과 · 가격을 읽고, 조리 시작 · 접시 들기만 요청한다.
//          판정 · 재고 차감 · 조리 시간 · 결제는 구현하는 쪽(지금은 FirstGuestKitchen)이 한다.
// 구현   : FirstGuestKitchen. 나중에 KitchenManager가 이어받는다.
// 레시피  : 2026-11-27 김언호 — 재료·양념·도구 조합으로 미리보기·조리 시작을 요청하는 멤버를 더했다(첫 플레이 레시피 8개).
//          기존 빵 전용 멤버(FlourRequired·GarlicRequired·Protection·Price·TryStartCooking(id, 밀가루, 마늘))는 화면이 옮겨 갈 때까지
//          구운 빵 기준으로 남긴다. 💡 홍준우 리뷰 전.
using System.Collections.Generic;
// 연결   : 구현하는 쪽이 켜질 때 UIManager.ConnectKitchenOrders, 꺼질 때 DisconnectKitchenOrders
// =====================================================================
using DOB.Systems;

namespace DOB.Manager
{
    /// <summary>조리 시작 요청의 결과. 💡 초안 — 백엔드 구현 때 확정 (2026-11-25 김언호).</summary>
    public enum CookStartResult { Started, NoOrder, NotSeated, NotNearKitchen, Paused, BurnerBusy, WrongIngredients, RecipeLocked, WrongTool, NotEnoughStock }

    /// <summary>재료·양념·도구 조합의 미리보기. 실제 조리 시작·서빙과 같은 계산이다.</summary>
    public readonly struct DishPreview
    {
        /// <summary>지금 확정하면 나올 결과. Started면 조리할 수 있다.</summary>
        public readonly CookStartResult Check;
        /// <summary>맞는 레시피 id(그림 T_UI_Dish{id}). 재료가 어떤 레시피와도 맞지 않으면 null.</summary>
        public readonly string RecipeId;
        public readonly string RecipeName;
        /// <summary>그 레시피의 조리도구.</summary>
        public readonly string Tool;
        /// <summary>RiskAxes.All 순서의 축별 보호 칸(레시피 + 양념). 레시피가 없으면 빈 목록.</summary>
        public readonly IReadOnlyList<int> Protections;
        /// <summary>맛 점수(0~4)와 서빙하면 받을 금화. 레시피가 없으면 0.</summary>
        public readonly int Taste;
        public readonly int Price;

        public DishPreview(CookStartResult check, string recipeId, string recipeName, string tool, IReadOnlyList<int> protections, int taste, int price)
        {
            Check = check;
            RecipeId = recipeId;
            RecipeName = recipeName;
            Tool = tool;
            Protections = protections ?? new int[0];
            Taste = taste;
            Price = price;
        }
    }

    /// <summary>접시 들기 요청의 결과. 💡 초안 — 백엔드 구현 때 확정.</summary>
    public enum PickUpResult { PickedUp, NotCooked, NotNearKitchen, AlreadyHolding }

    /// <summary>요리 화면이 읽고 요청하는 실제 주문.</summary>
    public interface IKitchenOrders
    {
        /// <summary>주문 손님 이름. 주문이 없으면 null.</summary>
        string GuestName { get; }
        /// <summary>주문 식별자. 요청에 그대로 돌려준다. 주문이 없으면 null.</summary>
        string OrderId { get; }
        /// <summary>주문 상태. 주문이 없으면 null.</summary>
        FirstGuestOrderState? State { get; }
        float CookSeconds { get; }
        float RemainingSeconds { get; }
        /// <summary>구운 빵에 넣을 밀가루 수.</summary>
        int FlourRequired { get; }
        /// <summary>넣어야 하는 마늘 수. 마늘이 모자란 날은 0 (양념 없이 굽기).</summary>
        int GarlicRequired { get; }
        int GarlicStock { get; }
        /// <summary>이 요리가 한 위험 축을 막는 칸 수.</summary>
        int Protection(string axis, bool garlic);
        /// <summary>서빙하면 받을 금화.</summary>
        int Price(bool garlic);
        /// <summary>손님에게서 찾아 둔 위험 칸 수. 못 찾았으면 null.</summary>
        int? KnownRisk(string axis);
        /// <summary>그 주문의 재료를 확정하고 조리를 시작한다. 재료 인자는 재료 id 통일 때 바꾼다.</summary>
        CookStartResult TryStartCooking(string orderId, int flour, int garlic);
        /// <summary>열린 레시피 id(레시피 목록 순서).</summary>
        IReadOnlyList<string> UnlockedRecipes { get; }
        /// <summary>재료·양념 재고. 기본 재료(밀가루·감자·고기)는 int.MaxValue. 재료 id는 한글 이름, 양념은 시장 id(garlic 등).</summary>
        int StockOf(string id);
        /// <summary>그 주문에 이 재료·양념(없으면 null)·도구로 확정하면 어떻게 되는지. 상태는 바꾸지 않는다.</summary>
        DishPreview Preview(string orderId, IReadOnlyDictionary<string, int> ingredients, string spiceId, string toolId);
        /// <summary>재료·양념·도구로 레시피를 확정하고 조리를 시작한다. 거절이면 재고·주문을 바꾸지 않는다.</summary>
        CookStartResult TryStartCooking(string orderId, IReadOnlyDictionary<string, int> ingredients, string spiceId, string toolId);
        /// <summary>그 주문의 완성된 접시를 든다.</summary>
        PickUpResult TryPickUp(string orderId);
    }
}
