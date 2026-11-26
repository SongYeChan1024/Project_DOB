// =====================================================================
// IInteractionPrompt — 상호작용 표시(InteractionPromptPresenter)가 읽는 약속
// 역할   : 지금 E를 누르면 무엇과 상호작용하는지(대상 위치 · 짧은 동작 이름)만 알려 준다.
//          어떤 대상이 가장 가까운지 · 지금 상호작용할 수 있는지는 구현하는 쪽(백엔드 상호작용)이 판정한다.
// 구현   : 💡 FirstGuestService (좌석 안내 · 손님 맞이 · 주방 · 서빙). 김언호
// 연결   : 구현하는 쪽이 켜질 때 UIManager.ConnectInteractionPrompt, 꺼질 때 DisconnectInteractionPrompt
// 비고   : 공동 인터페이스 — 바꿀 때는 김언호 · 홍준우가 서로 리뷰한다.
//          대상 위치는 계속 움직이므로(손님) 화면은 열려 있는 동안 매 프레임 읽는다.
// =====================================================================
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>지금 상호작용할 수 있는 대상.</summary>
    public interface IInteractionPrompt
    {
        /// <summary>E를 누르면 상호작용할 대상. 없으면 null (표시를 숨긴다).</summary>
        Transform Target { get; }
        /// <summary>짧은 동작 이름 (예: 맞이하기 · 2번 자리 안내 · 재료 확인 · 접시 들기 · 서빙).</summary>
        string Action { get; }
    }
}
