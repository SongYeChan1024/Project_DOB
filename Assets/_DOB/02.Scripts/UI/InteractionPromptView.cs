// =====================================================================
// InteractionPromptView — 상호작용 대상 위에 뜨는 작은 [E] 동작 표시
// 역할   : 받은 화면 위치에 표시를 옮기고 동작 이름을 적는다. 무엇을 띄울지 · 어디에 띄울지는 계산하지 않는다.
// 움직임 : 나타날 때 아래에서 스르륵 떠오르고, 사라질 때 아래로 가라앉으며 흐려진다.
//          표시된 키를 누르면 눌렸다 밝아지며 돌아온다 (시안 18 ⑥). 입력 처리는 하지 않고 보이기만 한다
// 호출자 : InteractionPromptPresenter (Show, Hide)
// 상태   : 보이는 정도(0~1), 떠오르는 중인지 · 가라앉는 중인지, 눌림 시간
//
// 의사코드
// [보이기]   꺼져 있었으면 켜고 보이는 정도를 0부터 올린다. 가라앉던 중이었으면 다시 올린다
// [숨기기]   바로 끄지 않고 보이는 정도를 0으로 내린다 → 0이 되면 끈다
// [매 프레임] 보이는 정도로 투명도 · 아래로 밀린 거리를 정하고, 표시된 키(E)를 누르면 눌림 시간을 0부터
// =====================================================================
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DOB.UI
{
    /// <summary>상호작용 표시 (키 칸 + 동작 이름).</summary>
    public sealed class InteractionPromptView : MonoBehaviour
    {
        /// <summary>표시 전체 (위치를 옮기는 칩). 평소 비활성.</summary>
        [SerializeField] private RectTransform chip;
        /// <summary>키 글자 (E).</summary>
        [SerializeField] private TMP_Text keyText;
        /// <summary>동작 이름.</summary>
        [SerializeField] private TMP_Text actionText;

        [Header("움직임")]
        /// <summary>떠오르기 · 가라앉기 시간 (초).</summary>
        [SerializeField] private float fadeSeconds = 0.2f;
        /// <summary>안 보일 때 아래로 내려가 있는 거리 (캔버스 단위).</summary>
        [SerializeField] private float sink = 18f;
        [SerializeField] private float pressSeconds = 0.18f;
        [SerializeField] private float pressScale = 0.9f;

        private RectTransform area;
        private CanvasGroup group;
        private Vector2 place;
        private float shown, goal, pressT = -1f;

        private void Awake()
        {
            area = (RectTransform)transform;
            if (chip)
            {
                group = chip.GetComponent<CanvasGroup>();
                if (!group) group = chip.gameObject.AddComponent<CanvasGroup>();
                group.blocksRaycasts = false;
            }
            shown = goal = 0f;
            if (chip) chip.gameObject.SetActive(false);
        }

        /// <summary>화면 좌표(픽셀)에 표시를 띄운다.</summary>
        public void Show(Vector2 screenPoint, string key, string action)
        {
            if (!chip) return;
            // 오버레이 캔버스라 카메라 없이 화면 좌표 → 캔버스 좌표
            RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, null, out place);
            if (keyText) keyText.text = key;
            if (actionText && actionText.text != action) actionText.text = action;
            goal = 1f;
            if (!chip.gameObject.activeSelf) { shown = 0f; chip.gameObject.SetActive(true); }
            Pose();
        }

        public void Hide()
        {
            goal = 0f;
            if (chip && chip.gameObject.activeSelf && shown <= 0f) chip.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!chip || !chip.gameObject.activeSelf) return;
            float dt = Mathf.Min(Time.unscaledDeltaTime, 0.05f);
            shown = Mathf.MoveTowards(shown, goal, dt / Mathf.Max(0.01f, fadeSeconds));
            var k = Keyboard.current;
            if (goal > 0f && k != null && keyText && keyText.text == "E" && k.eKey.wasPressedThisFrame) pressT = 0f;
            if (pressT >= 0f) { pressT += dt; if (pressT >= pressSeconds) pressT = -1f; }
            if (shown <= 0f && goal <= 0f) { chip.gameObject.SetActive(false); return; }
            Pose();
        }

        private void Pose()
        {
            float e = 1f - (1f - shown) * (1f - shown);
            chip.anchoredPosition = place + new Vector2(0f, -sink * (1f - e));
            if (group) group.alpha = e;
            float press = pressT >= 0f ? Mathf.Sin(Mathf.PI * pressT / pressSeconds) : 0f;
            chip.localScale = Vector3.one * Mathf.Lerp(1f, pressScale, press);
        }
    }
}
