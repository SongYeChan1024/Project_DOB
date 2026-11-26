// =====================================================================
// ButtonFeel — 버튼 손맛 (시안 12 ③)
// 역할   : 마우스를 올리면 살짝 커지고, 누르면 눌려 들어갔다가, 떼면 돌아온다. 못 누르는 버튼은 반응하지 않는다.
//          밝기 · 흐림은 Button의 색 전환(ColorTint)이 맡고, 여기서는 크기만 바꾼다.
// 호출자 : EventSystem (마우스 올림 · 벗어남 · 누름 · 뗌)
// 상태   : 마우스가 위에 있는지, 눌려 있는지, 원래 크기
//
// 의사코드
// [들어옴/나감] 위에 있음 표시를 바꾼다
// [누름/뗌]     눌림 표시를 바꾼다 (효과음 요청은 사운드 시스템이 생기면 누름에서 보낸다)
// [매 프레임]   누를 수 있으면 눌림 > 위에 있음 > 기본 순으로 목표 크기를 정하고, 실제 시간으로 부드럽게 따라간다
// [꺼짐]        표시를 지우고 원래 크기로 돌려 둔다 (다시 켤 때 커진 채로 남지 않게)
// =====================================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>마우스를 올리면 커지고 누르면 눌리는 버튼 반응.</summary>
    public sealed class ButtonFeel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>마우스를 올렸을 때 크기 (배율).</summary>
        [SerializeField] private float hoverScale = 1.06f;
        /// <summary>눌렀을 때 크기 (배율).</summary>
        [SerializeField] private float pressScale = 0.95f;
        /// <summary>목표 크기를 따라가는 빠르기 (클수록 빠름).</summary>
        [SerializeField] private float speed = 24f;

        private Selectable target;
        private Vector3 home;
        private bool over, down;

        private void Awake()
        {
            target = GetComponent<Selectable>();
            home = transform.localScale;
        }

        private void OnDisable()
        {
            over = down = false;
            transform.localScale = home;
        }

        public void OnPointerEnter(PointerEventData eventData) => over = true;
        public void OnPointerExit(PointerEventData eventData) => over = false;
        public void OnPointerDown(PointerEventData eventData) => down = true;
        public void OnPointerUp(PointerEventData eventData) => down = false;

        private void Update()
        {
            bool live = target == null || target.IsInteractable();
            float s = !live ? 1f : down ? pressScale : over ? hoverScale : 1f;
            var goal = home * s;
            // 일시 정지(timeScale 0) 중인 메뉴에서도 반응하도록 실제 시간을 쓴다
            transform.localScale = Vector3.Lerp(transform.localScale, goal, 1f - Mathf.Exp(-speed * Time.unscaledDeltaTime));
        }
    }
}
