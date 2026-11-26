// =====================================================================
// ScreenFadeIn — 스스로 페이드하지 않는 화면을 켜질 때 서서히 보이게 (시안 12 ⑤)
// 역할   : 방명록 · 귀환 보고처럼 SetActive로만 열리는 화면 판에 붙인다. 켜지는 순간 투명도를 0에서 1로 올린다.
//          닫기는 바로 꺼진다 (열림 여부를 activeInHierarchy로 보는 코드 · 테스트가 있어서 지연하지 않는다).
// 호출자 : 없음 (OnEnable)
// 상태   : 같은 오브젝트의 CanvasGroup, 진행 시간
// =====================================================================
using UnityEngine;

namespace DOB.UI
{
    /// <summary>켜질 때 CanvasGroup 투명도를 0 → 1로 올린다.</summary>
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class ScreenFadeIn : MonoBehaviour
    {
        [SerializeField] private float seconds = 0.25f;

        private CanvasGroup group;
        private float t = -1f;

        private void Awake() => group = GetComponent<CanvasGroup>();

        private void OnEnable()
        {
            t = 0f;
            group.alpha = 0f;
        }

        private void Update()
        {
            if (t < 0f) return;
            t += Mathf.Min(Time.unscaledDeltaTime, 0.05f); // 멈칫한 첫 프레임에 연출이 통째로 건너뛰지 않게
            group.alpha = Mathf.Clamp01(t / seconds);
            if (t >= seconds) t = -1f;
        }
    }
}
