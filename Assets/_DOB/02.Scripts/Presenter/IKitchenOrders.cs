// =====================================================================
// IKitchenOrders — 요리 화면(KitchenPresenter)이 실제 주방 주문을 읽고 요청하는 약속
// 역할   : 화면은 이 약속으로 주문 · 조리 상태 · 재고 · 효과 · 가격을 읽고, 조리 시작 · 접시 들기만 요청한다.
//          판정 · 재고 차감 · 조리 시간 · 결제는 구현하는 쪽(지금은 FirstGuestKitchen)이 한다.
// 구현   : FirstGuestKitchen (첫 영업 구운 빵 주문). 나중에 KitchenManager가 이어받는다.
// 연결   : 구현하는 쪽이 켜질 때 UIManager.ConnectKitchenOrders, 꺼질 때 DisconnectKitchenOrders
// =====================================================================
using DOB.Systems;

namespace DOB.Manager
{
    /// <summary>조리 시작 요청의 결과. 💡 초안 — 백엔드 구현 때 확정 (2026-11-25 김언호).</summary>
    public enum CookStartResult { Started, NoOrder, NotSeated, NotNearKitchen, Paused, BurnerBusy, WrongIngredients }

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
        /// <summary>그 주문의 완성된 접시를 든다.</summary>
        PickUpResult TryPickUp(string orderId);
    }
}
