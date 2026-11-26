// =====================================================================
// IServiceStatus — 영업 중 HUD(ServiceHudPresenter)가 읽는 약속 (시안 B)
// 역할   : 일차 · 금화 · 오늘 온 손님 수 · 손님별 주문(이름 · 얼굴 · 요리 · 상태) · 손에 든 접시만 알려 준다.
//          금화 · 주문 · 손님 수의 실제 값은 구현하는 쪽만 바꾼다. 화면은 받은 값만 그린다.
// 구현   : 💡 FirstGuestService · FirstGuestKitchen → 나중에 GameManager · KitchenManager. 김언호
// 연결   : 구현하는 쪽이 켜질 때 UIManager.ConnectServiceStatus, 꺼질 때 DisconnectServiceStatus
// 비고   : 공동 인터페이스 — 바꿀 때는 김언호 · 홍준우가 서로 리뷰한다. 💡 초안 (2026-11-26 홍준우).
//          값이 계속 바뀌므로 화면은 매 프레임 읽는다 (UI 연결 계획 2-2).
// =====================================================================
using DOB.UI;

namespace DOB.Manager
{
    /// <summary>영업 중 HUD가 읽는 지금 영업 상태.</summary>
    public interface IServiceStatus
    {
        /// <summary>지금 일차 (1부터).</summary>
        int Day { get; }
        /// <summary>가진 금화.</summary>
        int Gold { get; }
        /// <summary>오늘 입장한 손님 수. 손님이 들어올 때마다 1씩 오른다 (정해진 전체 수는 보이지 않는다).</summary>
        int GuestsArrived { get; }
        /// <summary>주문표를 걸 손님 수 (자리에 앉아 주문을 기다리는 손님부터 서빙 전까지).</summary>
        int OrderCount { get; }
        string GuestNameAt(int index);
        /// <summary>얼굴 그림 id (그림 T_UI_Portrait_{id}, 예: Haru). 없으면 null → 빈 얼굴.</summary>
        string PortraitIdAt(int index);
        /// <summary>주문한 요리 id (그림 T_UI_Dish{id}, 예: Bread). 주문 전이면 null → "?".</summary>
        string DishIdAt(int index);
        OrderTicketState StateAt(int index);
        /// <summary>플레이어가 손에 든 접시의 요리 id. 빈손이면 null.</summary>
        string HeldDishId { get; }
    }
}
