// =====================================================================
// HoverUnderline — 글자 버튼에 마우스를 올리거나 선택하면 밑줄 · 불빛을 보여 준다 (시안 14 ⑦)
// 역할   : 타이틀 가로 메뉴처럼 판 없는 글자 버튼의 강조 표시. 표시만 하고 입력을 처리하지 않는다.
//          밑줄은 가운데에서 양옆으로 펼쳐지고, 불빛(있으면)은 글자 뒤에서 서서히 번진다.
// 호출자 : EventSystem (마우스 올림·벗어남, 키보드 선택·해제)
// 상태   : 강조 정도 (0 = 꺼짐 ~ 1 = 다 펼쳐짐)
//
// 의사코드
// [올림·선택]   누를 수 있는 버튼이면 목표를 1, 아니면 0
// [매 프레임]   강조 정도를 목표로 옮긴다 → 밑줄 가로 크기 · 불빛 투명도에 반영. 0이면 밑줄을 끈다
// [켜질 때]     강조를 0으로 시작 (다시 켤 때 펼쳐진 채로 남지 않게)
// =====================================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>버튼 위에 마우스가 있거나 선택됐을 때 밑줄을 펼치고 불빛을 켠다.</summary>
    public sealed class HoverUnderline : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private GameObject underline;
        /// <summary>잠긴(누를 수 없는) 버튼이면 밑줄을 켜지 않는다.</summary>
        [SerializeField] private Selectable target;
        /// <summary>글자 뒤 불빛. 비워 두면 밑줄만.</summary>
        [SerializeField] private Graphic glow;
        /// <summary>다 펼쳐지는 데 걸리는 시간 (초).</summary>
        [SerializeField] private float seconds = 0.15f;

        private float shown, goal;

        private void OnEnable()
        {
            shown = goal = 0f;
            Apply();
        }

        public void OnPointerEnter(PointerEventData eventData) => Show(true);
        public void OnPointerExit(PointerEventData eventData) => Show(false);
        public void OnSelect(BaseEventData eventData) => Show(true);
        public void OnDeselect(BaseEventData eventData) => Show(false);

        private void Show(bool on) => goal = on && (target == null || target.IsInteractable()) ? 1f : 0f;

        private void Update()
        {
            if (target != null && !target.IsInteractable()) goal = 0f; // 로딩 중 잠기면 접는다
            if (Mathf.Approximately(shown, goal)) return;
            shown = Mathf.MoveTowards(shown, goal, Time.unscaledDeltaTime / Mathf.Max(0.01f, seconds));
            Apply();
        }

        private void Apply()
        {
            underline.SetActive(shown > 0.001f);
            float e = shown * shown * (3f - 2f * shown);
            var s = underline.transform.localScale;
            underline.transform.localScale = new Vector3(e, s.y, s.z);
            if (glow) glow.canvasRenderer.SetAlpha(e);
        }
    }
}
