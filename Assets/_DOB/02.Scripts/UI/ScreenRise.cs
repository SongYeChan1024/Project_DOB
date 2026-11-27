// =====================================================================
// ScreenRise — 화면 창이 바탕과 함께 떠오르기 (시안 12 ⑤)
// 역할   : 창(시장 판 · 수첩 · 문패 · 방명록 책 · 보고 창)에 붙인다. 위쪽 CanvasGroup의 투명도를
//          열림 정도로 보고, 덜 열렸을수록 조금 아래 · 조금 작게 둔다. 열 때는 아래에서 올라오며 커지고,
//          닫을 때(투명도가 줄 때)는 반대로 내려가며 작아진다.
// 호출자 : 없음 (매 프레임 스스로 투명도를 읽는다. 페이드는 각 화면이 이미 한다)
// 상태   : 원래 위치 · 크기, 따라갈 CanvasGroup
// =====================================================================
using UnityEngine;

namespace DOB.UI
{
    /// <summary>위쪽 CanvasGroup 투명도에 맞춰 창을 올리고 키운다.</summary>
    public sealed class ScreenRise : MonoBehaviour
    {
        /// <summary>다 닫혔을 때 아래로 내려가 있는 거리 (캔버스 단위).</summary>
        [SerializeField] private float drop = 40f;
        /// <summary>다 닫혔을 때 크기 (배율).</summary>
        [SerializeField] private float startScale = 0.94f;

        private RectTransform rect;
        private CanvasGroup group;
        private Vector2 homePos;
        private Vector3 homeScale;

        private void Awake()
        {
            rect = (RectTransform)transform;
            homePos = rect.anchoredPosition;
            homeScale = rect.localScale;
            // 자기 자신이 아닌 위쪽 페이드를 따른다 (창 안의 작은 CanvasGroup은 무시)
            group = transform.parent ? transform.parent.GetComponentInParent<CanvasGroup>(true) : null;
        }

        private void LateUpdate()
        {
            if (!group) return;
            float a = Mathf.Clamp01(group.alpha);
            float e = 1f - (1f - a) * (1f - a); // 끝에서 부드럽게 멈추게
            rect.anchoredPosition = homePos + new Vector2(0f, -drop * (1f - e));
            rect.localScale = homeScale * Mathf.Lerp(startScale, 1f, e);
        }
    }
}
