// =====================================================================
// FloatingMotion — UI를 위아래로 천천히 떠 있게 움직인다
// 역할   : 타이틀 로고처럼 가만히 있으면 허전한 UI에 숨 쉬는 듯한 작은 움직임을 준다. 표시만 한다.
// 호출자 : 없음 (오브젝트에 붙이면 혼자 움직인다)
// 상태   : 처음 위치만 기억한다. 꺼지면 처음 위치로 돌려놓는다.
//
// 의사코드 (구현 전 작성 · 2026-11-23 홍준우와 합의)
// [켜짐]   지금 위치를 기준점으로 기억한다
// [매 프레임] 기준점 + 위아래 흔들림(사인 곡선) 위치로 옮긴다. 일시정지(timeScale 0)에도 움직인다
// [꺼짐]   기준점으로 되돌려 놓는다 (에디터에서 위치가 어긋나지 않게)
// =====================================================================
using UnityEngine;

namespace DOB.UI
{
    /// <summary>UI를 위아래로 천천히 흔든다. RectTransform이 있는 오브젝트에 붙인다.</summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class FloatingMotion : MonoBehaviour
    {
        /// <summary>기준점에서 위아래로 움직이는 거리(픽셀, 1920x1080 기준).</summary>
        [SerializeField, Range(0f, 30f)] private float height = 6f;
        /// <summary>한 번 올라갔다 내려오는 데 걸리는 시간(초).</summary>
        [SerializeField, Range(0.5f, 10f)] private float period = 3f;

        private RectTransform rect;
        private Vector2 basePosition;
        private float startTime;

        private void OnEnable()
        {
            rect = (RectTransform)transform;
            basePosition = rect.anchoredPosition;
            startTime = Time.unscaledTime;
        }

        private void OnDisable()
        {
            if (rect != null)
                rect.anchoredPosition = basePosition;
        }

        private void Update()
        {
            float t = (Time.unscaledTime - startTime) / period;
            float offset = Mathf.Sin(t * Mathf.PI * 2f) * height;
            rect.anchoredPosition = basePosition + new Vector2(0f, offset);
        }
    }
}
