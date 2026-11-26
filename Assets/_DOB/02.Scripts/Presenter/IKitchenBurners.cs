// =====================================================================
// IKitchenBurners — 화구 위 조리 표시(BurnerBadgePresenter)가 읽는 약속
// 역할   : 화구마다 월드 위치 · 상태 · 만드는 요리 · 남은 시간만 알려 준다. 조리 시간은 구현하는 쪽(OrderBook)만 센다.
// 구현   : 💡 FirstGuestKitchen → 나중에 KitchenManager. 김언호
// 연결   : 구현하는 쪽이 켜질 때 UIManager.ConnectKitchenBurners, 꺼질 때 DisconnectKitchenBurners
// 비고   : 공동 인터페이스 — 바꿀 때는 김언호 · 홍준우가 서로 리뷰한다. 화구 수는 Count로 읽는다 (지금 백엔드는 1칸).
//          남은 시간은 계속 흐르므로 화면은 매 프레임 읽는다 (UI 연결 계획 2-2).
// =====================================================================
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>화구들의 지금 조리 상태.</summary>
    public interface IKitchenBurners
    {
        /// <summary>화구 수.</summary>
        int Count { get; }
        /// <summary>화구의 월드 위치 (표시를 띄울 기준). 없으면 null.</summary>
        Transform BurnerAt(int index);
        /// <summary>비어 있음 · 조리 중 · 완성.</summary>
        BurnerState StateAt(int index);
        /// <summary>만드는 요리 id (그림 T_UI_Dish{id}, 예: Bread). 비어 있으면 null.</summary>
        string DishIdAt(int index);
        /// <summary>남은 조리 시간 (초).</summary>
        float RemainingSecondsAt(int index);
        /// <summary>전체 조리 시간 (초). 링 비율에 쓴다.</summary>
        float CookSecondsAt(int index);
    }
}
