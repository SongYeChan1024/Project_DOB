// =====================================================================
// ServiceHudPresenter — 영업 중 HUD를 맞춘다 (시안 B)
// 역할   : IServiceStatus(백엔드)가 알려 주는 일차 · 금화 · 오늘 온 손님 수 · 주문 · 손에 든 접시를
//          ServiceHudView에 옮긴다. 얼굴 · 요리 id를 그림으로 바꾸는 것만 하고 값은 계산하지 않는다.
// 호출자 : 없음 (매 프레임 스스로 갱신)
// 호출   : UIManager.ServiceStatus · IsScreenOpen, ServiceHudView, ServiceTicketView
// 상태   : 없음 (주문서 목록은 View가 갖는다)
//
// 의사코드
// [매 프레임 끝] 연결이 없거나 다른 화면(준비 · 요리 · 방명록)이 열려 있으면 HUD를 숨긴다
//               → 보이기 → 일차 · 금화 · 오늘 온 손님 수
//               → 주문서 맞추기 시작 → 먼저 온 주문부터: 식별자 · 얼굴(T_UI_Portrait_{id}) · 이름 ·
//                 요리(T_UI_Dish{id}, 없으면 "?") · 상태 · 든 접시의 주인인지(식별자가 HeldOrderId와 같은지)
//               → 맞추기 끝(안 불린 주문서는 View가 떼어 낸다) → 손에 든 접시(없으면 칸 숨김)
// =====================================================================
using System;
using System.Linq;
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>영업 중 HUD를 맞춘다.</summary>
    public sealed class ServiceHudPresenter : MonoBehaviour
    {
        [SerializeField] private ServiceHudView view;
        /// <summary>손님 얼굴 (07.UI/Sprites/GuestDialogue의 T_UI_Portrait_○○). 이름으로 찾는다.</summary>
        [SerializeField] private Sprite[] portraits = Array.Empty<Sprite>();
        /// <summary>요리 그림 (07.UI/Sprites/Items의 T_UI_Dish○○). 이름으로 찾는다.</summary>
        [SerializeField] private Sprite[] dishIcons = Array.Empty<Sprite>();

        private void LateUpdate()
        {
            var status = UIManager.ServiceStatus;
            if (status == null || UIManager.IsScreenOpen)
            {
                view.Hide();
                return;
            }

            view.Show();
            view.SetDay(status.Day);
            view.SetGold(status.Gold);
            view.SetGuests(status.GuestsArrived);

            string heldOrder = status.HeldOrderId;
            view.BeginTickets();
            for (int i = 0, count = Mathf.Max(0, status.OrderCount); i < count; i++)
            {
                string id = status.OrderIdAt(i);
                view.ShowTicket(id, Portrait(status.PortraitIdAt(i)), status.GuestNameAt(i), Dish(status.DishIdAt(i)), status.StateAt(i),
                    !string.IsNullOrEmpty(heldOrder) && id == heldOrder);
            }
            view.EndTickets();

            view.SetHeld(Dish(status.HeldDishId));
        }

        private Sprite Portrait(string id) =>
            string.IsNullOrEmpty(id) ? null : portraits.FirstOrDefault(sp => sp && sp.name == "T_UI_Portrait_" + id);

        private Sprite Dish(string id) =>
            string.IsNullOrEmpty(id) ? null : dishIcons.FirstOrDefault(sp => sp && sp.name == "T_UI_Dish" + id);
    }
}
