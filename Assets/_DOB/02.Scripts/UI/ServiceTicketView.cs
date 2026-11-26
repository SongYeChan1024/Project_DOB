// =====================================================================
// ServiceTicketView — 영업 HUD 오른쪽 위 줄에 걸린 주문서 하나 (시안 B)
// 역할   : 얼굴 · 이름 · 요리 그림(주문 전에는 "?") · 상태 한 단어를 그린다. 완성이면 종이가 초록빛으로 빛난다.
//          대기 시간 · 조리 남은 초는 그리지 않는다 (손님 말풍선 · 화구 표시가 맡는다).
// 호출자 : ServiceHudView (목록 관리), ServiceHudPresenter (Show)
// 상태   : 없음
// =====================================================================
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>주문서에 보일 주문 상태.</summary>
    public enum OrderTicketState
    {
        /// <summary>자리에 앉아 주문을 정하기 전 (요리 "?")</summary>
        Waiting,
        /// <summary>화구에서 조리 중</summary>
        Cooking,
        /// <summary>완성 — 서빙할 수 있음</summary>
        Ready,
    }

    /// <summary>걸린 주문서 하나.</summary>
    public sealed class ServiceTicketView : MonoBehaviour
    {
        [SerializeField] private Image face;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Image dish;
        /// <summary>주문 전 "?" 표시.</summary>
        [SerializeField] private GameObject unknownDish;
        /// <summary>상태 글자 뒤 둥근 바탕. 대기일 때는 옅게.</summary>
        [SerializeField] private Image stateChip;
        [SerializeField] private TMP_Text stateText;
        /// <summary>완성일 때만 켜는 종이 뒤 초록 빛.</summary>
        [SerializeField] private GameObject readyGlow;

        [Header("상태별 (대기 · 조리 중 · 완성 순서) 💡 문구 가안")]
        [SerializeField] private string[] stateWords = { "주문 대기", "조리 중", "완성!" };
        [SerializeField] private Color[] chipColors = { new Color32(138, 106, 74, 40), new Color32(214, 136, 40, 255), new Color32(92, 150, 50, 255) };
        [SerializeField] private Color[] wordColors = { new Color32(122, 90, 58, 255), Color.white, Color.white };

        /// <summary>주문서를 그린다. dishIcon이 없으면 "?"를 보인다.</summary>
        public void Show(Sprite portrait, string guestName, Sprite dishIcon, OrderTicketState state)
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            face.sprite = portrait;
            face.enabled = portrait;
            if (nameText.text != guestName) nameText.text = guestName;
            dish.sprite = dishIcon;
            dish.enabled = dishIcon;
            unknownDish.SetActive(!dishIcon);

            int s = (int)state;
            if (stateText.text != stateWords[s]) stateText.text = stateWords[s];
            stateText.color = wordColors[s];
            stateChip.color = chipColors[s];
            readyGlow.SetActive(state == OrderTicketState.Ready);
        }

        public void Hide()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }
    }
}
