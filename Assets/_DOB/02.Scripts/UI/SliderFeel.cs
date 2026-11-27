// =====================================================================
// SliderFeel — 음량 막대 손맛 (시안 23 ⑧)
// 역할   : 막대를 잡고 있는 동안 손잡이가 커지고 금빛이 돌며, 값이 바뀔 때마다 퍼센트 글자가 톡 커졌다 돌아온다.
//          값 · 글자 내용은 SettingsView가 정하고, 여기서는 크기 · 색만 바꾼다.
// 호출자 : EventSystem (누름 · 뗌), Slider.onValueChanged
// 상태   : 잡고 있는지, 글자 톡 시간
//
// 의사코드
// [누름/뗌]   잡고 있음 표시를 바꾼다
// [값 바뀜]   잡고 있을 때만 글자 톡 시간을 0부터 (창을 열며 값을 채울 때는 가만히)
// [매 프레임] 잡고 있으면 손잡이 크기 → grabScale, 색 → grabColor로 부드럽게. 글자는 톡 시간 동안 1 → 1.25 → 1
// =====================================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>음량 막대를 잡을 때 손잡이 · 퍼센트 글자 반응.</summary>
    [RequireComponent(typeof(Slider))]
    public sealed class SliderFeel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>잡고 있을 때 손잡이 크기 (배율).</summary>
        [SerializeField] private float grabScale = 1.35f;
        /// <summary>잡고 있을 때 손잡이 색 (원래 색에 곱해짐).</summary>
        [SerializeField] private Color grabColor = new Color(1.25f, 1.1f, 0.75f, 1f);
        /// <summary>값이 바뀌면 톡 커질 퍼센트 글자. 비워 두면 손잡이만.</summary>
        [SerializeField] private Graphic valueLabel;
        [SerializeField] private float speed = 20f;

        private Slider slider;
        private Graphic handle;
        private bool held;
        private float popT = -1f;
        private const float PopSeconds = 0.18f;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            handle = slider.handleRect ? slider.handleRect.GetComponent<Graphic>() : null;
            slider.onValueChanged.AddListener(_ => { if (held) popT = 0f; }); // 창을 열 때 값을 채우는 것은 톡 하지 않는다
        }

        private void OnDisable()
        {
            held = false;
            popT = -1f;
            if (handle) { handle.transform.localScale = Vector3.one; handle.canvasRenderer.SetColor(Color.white); }
            if (valueLabel) valueLabel.transform.localScale = Vector3.one;
        }

        public void OnPointerDown(PointerEventData eventData) => held = true;
        public void OnPointerUp(PointerEventData eventData) => held = false;

        private void Update()
        {
            float dt = Time.unscaledDeltaTime; // 일시 정지 중인 설정 창에서도 반응
            if (handle)
            {
                float follow = 1f - Mathf.Exp(-speed * dt);
                var t = handle.transform;
                t.localScale = Vector3.Lerp(t.localScale, Vector3.one * (held ? grabScale : 1f), follow);
                handle.canvasRenderer.SetColor(Color.Lerp(handle.canvasRenderer.GetColor(), held ? grabColor : Color.white, follow));
            }
            if (valueLabel && popT >= 0f)
            {
                popT += Mathf.Min(dt, 0.05f);
                float k = Mathf.Clamp01(popT / PopSeconds);
                valueLabel.transform.localScale = Vector3.one * (1f + 0.25f * Mathf.Sin(Mathf.PI * k));
                if (k >= 1f) popT = -1f;
            }
        }
    }
}
