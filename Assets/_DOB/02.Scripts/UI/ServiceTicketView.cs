// =====================================================================
// ServiceTicketView — 영업 HUD 오른쪽 위 줄에 걸린 주문서 하나 (시안 B · 스토리보드 ①②④)
// 역할   : 얼굴 · 이름 · 요리 그림(주문 전에는 "?") · 상태 한 단어를 그린다. 완성이면 종이가 초록빛으로 빛난다.
//          대기 시간 · 조리 남은 초는 그리지 않는다 (손님 말풍선 · 화구 표시가 맡는다).
//          움직임: 새로 걸리면 위에서 떨어져 흔들리다 멈춤 / 든 접시의 주인이면 금빛으로 들어 올림 /
//                  앞 주문서가 빠져 자리가 바뀌면 미끄러져 옮김 / 서빙되면 기울며 떨어져 사라짐 /
//                  「완성!」이 되는 순간 위로 톡 튀며 커졌다 돌아옴 (시안 14 ④).
// 호출자 : ServiceHudView (Hang, Show, TakeDown)
// 상태   : 주문 식별자, 기본 기울기, 지난 상태, 움직임 시간들(걸림 · 떼어 냄 · 완성 튐 · 들어 올림 정도 · 미끄러짐 남은 거리)
//
// 의사코드
// [걸기]      식별자 · 기본 기울기를 받고 걸림 시간을 0부터 → 위에서 떨어지며 감쇠 진동으로 기본 기울기에 멈춘다
// [그리기]    받은 값만 그린다. held면 들어 올림 정도를 1로, 아니면 0으로 부드럽게 옮긴다
//             걸린 뒤 상태가 「완성!」으로 바뀐 순간에만 튐 시간을 0부터 (걸릴 때부터 완성이면 튀지 않음)
// [미끄러짐]  가로 정렬이 자리를 옮기면 옮긴 거리만큼 반대로 밀어 두고 0으로 줄인다 (빈자리로 당겨지는 느낌)
// [떼어 내기] 정렬에서 빼고 그 자리에서 기울며 떨어지며 흐려진다 → 끝나면 알린다
// 비고   : 자리는 부모의 HorizontalLayoutGroup이 정하므로, 움직임은 안쪽 Swing에만 준다.
// =====================================================================
using System;
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
        /// <summary>실제로 움직이는 안쪽 묶음 (빨래집게 부근이 중심).</summary>
        [SerializeField] private RectTransform swing;
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
        /// <summary>든 접시의 주인일 때 들어 올린 정도만큼 보이는 금빛.</summary>
        [SerializeField] private Graphic heldGlow;

        [Header("상태별 (대기 · 조리 중 · 완성 순서) 💡 문구 가안")]
        [SerializeField] private string[] stateWords = { "주문 대기", "조리 중", "완성!" };
        [SerializeField] private Color[] chipColors = { new Color32(138, 106, 74, 40), new Color32(214, 136, 40, 255), new Color32(92, 150, 50, 255) };
        [SerializeField] private Color[] wordColors = { new Color32(122, 90, 58, 255), Color.white, Color.white };

        [Header("걸림 · 흔들림")]
        [SerializeField] private float hangSeconds = 0.7f;
        /// <summary>떨어지기 시작하는 높이 (캔버스 단위).</summary>
        [SerializeField] private float dropHeight = 140f;
        /// <summary>처음 흔들림 크기 (도).</summary>
        [SerializeField] private float swingDegrees = 14f;

        [Header("든 접시의 주인")]
        [SerializeField] private float liftHeight = 14f;
        [SerializeField] private float liftScale = 1.06f;
        /// <summary>들어 올림이 1까지 가는 빠르기 (초당).</summary>
        [SerializeField] private float liftSpeed = 6f;

        [Header("떼어 내기 · 미끄러짐")]
        [SerializeField] private float takeDownSeconds = 0.5f;
        [SerializeField] private float fallDistance = 150f;
        [SerializeField] private float fallDegrees = 26f;
        /// <summary>미끄러짐 남은 거리가 줄어드는 빠르기 (초당 비율).</summary>
        [SerializeField] private float slideSpeed = 10f;

        [Header("「완성!」 튐")]
        [SerializeField] private float readyPopSeconds = 0.4f;
        [SerializeField] private float readyPopHeight = 16f;
        /// <summary>튈 때 가장 커지는 크기 (배율).</summary>
        [SerializeField] private float readyPopScale = 1.1f;
        [SerializeField] private float readyPopDegrees = 5f;

        public string OrderId { get; private set; }
        public bool IsLeaving => leaveT >= 0f;

        private RectTransform rect;
        private CanvasGroup group;
        private LayoutElement layout;
        private float baseTilt;
        private float hangT = -1f, leaveT = -1f, popT = -1f;
        private int lastState = -1; // -1: 걸린 뒤 아직 안 그림
        private float lift, liftTarget;
        private float slide, lastX;
        private bool hasLastX;
        private Vector2 swingHome;
        private Action<ServiceTicketView> onGone;

        private void Awake()
        {
            rect = (RectTransform)transform;
            group = GetComponent<CanvasGroup>();
            layout = GetComponent<LayoutElement>();
            if (swing) swingHome = swing.anchoredPosition;
        }

        /// <summary>새 주문서로 건다. 위에서 떨어져 흔들리다 기본 기울기로 멈춘다.</summary>
        public void Hang(string orderId, float tilt)
        {
            OrderId = orderId;
            baseTilt = tilt;
            hangT = 0f; leaveT = -1f; popT = -1f; lastState = -1;
            lift = liftTarget = 0f; slide = 0f; hasLastX = false;
            if (layout) layout.ignoreLayout = false;
            if (group) group.alpha = 1f;
            gameObject.SetActive(true);
            Pose();
        }

        /// <summary>주문서를 그린다. dishIcon이 없으면 "?"를 보인다. held면 금빛으로 들어 올린다.</summary>
        public void Show(Sprite portrait, string guestName, Sprite dishIcon, OrderTicketState state, bool held)
        {
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
            if (s == (int)OrderTicketState.Ready && lastState >= 0 && lastState != s) popT = 0f;
            lastState = s;
            liftTarget = held ? 1f : 0f;
        }

        /// <summary>서빙된 주문서를 뗀다. 정렬에서 빠진 뒤 그 자리에서 떨어지며 사라지고, 끝나면 gone을 부른다.</summary>
        public void TakeDown(Action<ServiceTicketView> gone)
        {
            if (IsLeaving) return;
            onGone = gone;
            leaveT = 0f;
            liftTarget = 0f;
            if (layout) layout.ignoreLayout = true; // 남은 주문서가 바로 빈자리를 메운다
        }

        private void Update()
        {
            float dt = Mathf.Min(Time.unscaledDeltaTime, 0.05f); // 멈칫한 프레임에 짧은 연출이 통째로 건너뛰지 않게
            if (hangT >= 0f) { hangT += dt; if (hangT >= hangSeconds) hangT = -1f; }
            if (popT >= 0f) { popT += dt; if (popT >= readyPopSeconds) popT = -1f; }
            lift = Mathf.MoveTowards(lift, liftTarget, liftSpeed * dt);

            // 정렬이 자리를 옮겼으면 그만큼 반대로 밀어 두었다가 0으로 줄인다
            float x = rect.anchoredPosition.x;
            if (hasLastX && !IsLeaving && Mathf.Abs(x - lastX) > 0.5f) slide += lastX - x;
            lastX = x; hasLastX = true;
            slide = Mathf.Lerp(slide, 0f, 1f - Mathf.Exp(-slideSpeed * dt));

            if (IsLeaving)
            {
                leaveT += dt;
                float t = Mathf.Clamp01(leaveT / takeDownSeconds);
                if (group) group.alpha = 1f - t;
                if (t >= 1f)
                {
                    leaveT = -1f;
                    gameObject.SetActive(false);
                    var gone = onGone; onGone = null;
                    gone?.Invoke(this);
                    return;
                }
            }
            Pose();
        }

        // 지금 시간들로 Swing의 위치 · 기울기 · 크기와 금빛을 맞춘다
        private void Pose()
        {
            if (!swing) return;
            float y = 0f, angle = baseTilt;
            if (hangT >= 0f)
            {
                float t = hangT / hangSeconds;
                y = dropHeight * Mathf.Pow(1f - Mathf.Clamp01(t * 2.2f), 2f); // 앞쪽 45%에서 떨어지고
                angle += swingDegrees * Mathf.Exp(-4.5f * t) * Mathf.Cos(t * 19f); // 남은 동안 흔들리다 멈춘다
            }
            if (IsLeaving)
            {
                float t = Mathf.Clamp01(leaveT / takeDownSeconds);
                y -= fallDistance * t * t;
                angle += fallDegrees * t;
            }
            // 완성 튐: 위로 올랐다 내려오는 한 번의 혹 (sin 반주기)
            float pop = popT >= 0f ? Mathf.Sin(Mathf.PI * popT / readyPopSeconds) : 0f;
            y += readyPopHeight * pop;
            angle += readyPopDegrees * pop * Mathf.Sin(popT * 30f);
            float ease = lift * lift * (3f - 2f * lift);
            y += liftHeight * ease;
            angle = Mathf.Lerp(angle, 0f, ease); // 들어 올리면 똑바로 선다
            swing.anchoredPosition = swingHome + new Vector2(slide, y);
            swing.localRotation = Quaternion.Euler(0f, 0f, angle);
            swing.localScale = Vector3.one * Mathf.Lerp(1f, liftScale, ease) * Mathf.Lerp(1f, readyPopScale, pop);
            if (heldGlow) heldGlow.canvasRenderer.SetAlpha(ease);
        }
    }
}
