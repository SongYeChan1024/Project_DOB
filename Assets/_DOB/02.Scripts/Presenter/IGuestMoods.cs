// =====================================================================
// IGuestMoods — 손님 머리 위 말풍선(GuestBubblePresenter)이 읽는 약속 (시안 10)
// 역할   : 손님마다 위치 · 지금 상태(기다림 · 서빙 직후 · 퇴장) · 기다린 비율 · 대기 점수 구간 · 맛 소감만 알려 준다.
//          대기 시간 · 점수 · 떠남 판정은 구현하는 쪽(ServiceRoster)만 계산한다. 화면은 기준 초(10 · 20 · 30초)를 모른다.
// 구현   : 💡 FirstGuestService (+ ServiceRoster). 김언호
// 연결   : 구현하는 쪽이 켜질 때 UIManager.ConnectGuestMoods, 꺼질 때 DisconnectGuestMoods
// 비고   : 공동 인터페이스 — 바꿀 때는 김언호 · 홍준우가 서로 리뷰한다.
//          기다린 비율은 계속 흐르므로 화면은 매 프레임 읽는다 (UI 연결 계획 2-2).
// =====================================================================
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>말풍선에 보일 손님 상태.</summary>
    public enum GuestMoodState
    {
        /// <summary>말풍선 없음 (입장 중 · 응대 중 · 식사 중 등)</summary>
        None,
        /// <summary>맞이 · 음식을 기다리는 중: 기다림 원 + 구간별 불평</summary>
        Waiting,
        /// <summary>서빙 직후: 기쁨</summary>
        Served,
        /// <summary>나가는 중: 맛 소감 표정</summary>
        Leaving,
    }

    /// <summary>퇴장 표정 (기획 4-6 · 6-4: 좋아함 · 보통 · 싫어함).</summary>
    public enum TasteReaction { Like, Neutral, Dislike }

    /// <summary>손님들의 말풍선 상태.</summary>
    public interface IGuestMoods
    {
        /// <summary>지금 말풍선을 띄울 수 있는 손님 수.</summary>
        int Count { get; }
        /// <summary>손님 위치 (말풍선 기준). 없으면 null.</summary>
        Transform GuestAt(int index);
        GuestMoodState StateAt(int index);
        /// <summary>떠날 때까지 남은 비율 1 → 0 (기다림 원).</summary>
        float PatienceLeftAt(int index);
        /// <summary>지금 대기 점수 구간 2 · 1 · 0 (원 색과 불평 문구를 고른다).</summary>
        int WaitScoreAt(int index);
        /// <summary>퇴장 표정. Leaving일 때만 읽는다.</summary>
        TasteReaction ReactionAt(int index);
    }
}
