// =====================================================================
// GuestBubbleView — 손님 머리 위 말풍선 하나 (시안 10)
// 역할   : [기다림 원 또는 기호] [문구]를 한 줄로 그린다. 너비는 내용에 맞춰 레이아웃이 정한다 (빈칸 없음).
//          기다림 원은 남은 비율만큼 12시부터 채워, 시간이 갈수록 반시계 방향으로 줄어든다.
//          남은 비율 · 점수 구간 · 표정 종류는 받은 값만 그린다. 문구는 💡 가안 (기획 원고 오면 교체).
// 호출자 : GuestBubblePresenter (ShowWaiting, ShowServed, ShowLeaving, Hide)
// 움직임 : 처음 뜰 때 작게 → 톡 커졌다 제자리 (시안 17 ③). 기다림 구간이 나빠지는 순간 좌우로 흔들림
// 상태   : 등장 · 흔들림 시간, 지난 기다림 구간
//
// 의사코드
// [자리 잡기]  꺼져 있다가 켜지면 등장 시간을 0부터 → 크기 0.35 → 1.15 → 1
// [기다림]     구간이 지난번보다 나빠지면 흔들림 시간을 0부터 → 좌우 각도가 줄어들며 멈춤
// [숨기기]     지난 구간을 잊는다 (다음에 뜰 때 처음부터)
// =====================================================================
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>손님 말풍선 하나.</summary>
    public sealed class GuestBubbleView : MonoBehaviour
    {
        [Header("구성")]
        /// <summary>기다림 원 묶음 (바탕 · 채움 · 테두리).</summary>
        [SerializeField] private GameObject pie;
        /// <summary>기다림 원 채움 (Filled · Radial360 · 12시 시작 · 시계 방향 채움).</summary>
        [SerializeField] private Image pieFill;
        /// <summary>앞 기호 (♪ 또는 표정).</summary>
        [SerializeField] private TMP_Text mark;
        [SerializeField] private TMP_Text line;

        [Header("기다림 (대기 점수 2 · 1 · 0 순서)")]
        [SerializeField] private Color[] stageColors = { new Color32(110, 190, 80, 255), new Color32(235, 180, 50, 255), new Color32(220, 75, 55, 255) };
        /// <summary>💡 구간별 불평 문구. 비우면 원만 보인다.</summary>
        [SerializeField] private string[] complaints = { "", "아직인가요?", "너무 오래 걸려요!" };
        [SerializeField] private Color[] complaintColors = { new Color32(70, 45, 25, 255), new Color32(70, 45, 25, 255), new Color32(210, 70, 50, 255) };

        [Header("서빙 직후")]
        [SerializeField] private string servedMark = "♪";
        /// <summary>💡 기쁨 문구.</summary>
        [SerializeField] private string servedLine = "와, 맛있겠다!";
        [SerializeField] private Color servedColor = new Color32(220, 130, 40, 255);

        [Header("퇴장 표정 (좋아함 · 보통 · 싫어함 순서)")]
        [SerializeField] private string[] faces = { "^ㅁ^", "-_-", ">_<" };
        [SerializeField] private Color[] faceColors = { new Color32(220, 130, 40, 255), new Color32(70, 45, 25, 255), new Color32(210, 70, 50, 255) };
        [SerializeField] private Color inkColor = new Color32(70, 45, 25, 255);

        [Header("움직임")]
        [SerializeField] private float popSeconds = 0.25f;
        [SerializeField] private float shakeSeconds = 0.35f;
        /// <summary>흔들림 처음 각도 (도).</summary>
        [SerializeField] private float shakeDegrees = 7f;

        private RectTransform rect;
        private RectTransform Rect => rect ? rect : rect = (RectTransform)transform;
        private float popT = -1f, shakeT = -1f;
        private int lastStage = -1; // -1: 이번에 뜬 뒤 아직 기다림 구간 없음

        /// <summary>기다림: 남은 비율(1→0)의 원과, 점수 구간(2 · 1 · 0)에 맞는 색 · 불평 문구.</summary>
        public void ShowWaiting(Vector2 canvasPoint, float patienceLeft, int waitScore)
        {
            int stage = Mathf.Clamp(2 - waitScore, 0, 2); // 2점 = 0번 칸
            if (lastStage >= 0 && stage > lastStage) shakeT = 0f;
            lastStage = stage;
            pie.SetActive(true);
            pieFill.fillAmount = Mathf.Clamp01(patienceLeft);
            pieFill.color = Pick(stageColors, stage, Color.white);
            SetText(mark, null, inkColor);
            SetText(line, Pick(complaints, stage, ""), Pick(complaintColors, stage, inkColor));
            Place(canvasPoint);
        }

        /// <summary>서빙 직후 기쁨.</summary>
        public void ShowServed(Vector2 canvasPoint)
        {
            pie.SetActive(false);
            SetText(mark, servedMark, servedColor);
            SetText(line, servedLine, inkColor);
            Place(canvasPoint);
        }

        /// <summary>퇴장 표정 (0 좋아함 · 1 보통 · 2 싫어함).</summary>
        public void ShowLeaving(Vector2 canvasPoint, int reaction)
        {
            pie.SetActive(false);
            SetText(mark, Pick(faces, reaction, ""), Pick(faceColors, reaction, inkColor));
            SetText(line, null, inkColor);
            Place(canvasPoint);
        }

        public void Hide()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
            lastStage = -1;
        }

        private void Place(Vector2 canvasPoint)
        {
            Rect.anchoredPosition = canvasPoint;
            if (gameObject.activeSelf) return;
            gameObject.SetActive(true);
            popT = 0f;
            Pose();
        }

        private void Update()
        {
            float dt = Mathf.Min(Time.unscaledDeltaTime, 0.05f); // 멈칫한 프레임에 짧은 연출이 통째로 건너뛰지 않게
            if (popT >= 0f) { popT += dt; if (popT >= popSeconds) popT = -1f; }
            if (shakeT >= 0f) { shakeT += dt; if (shakeT >= shakeSeconds) shakeT = -1f; }
            Pose();
        }

        private void Pose()
        {
            float s = 1f;
            if (popT >= 0f)
            {
                float k = popT / popSeconds;
                s = k < 0.6f ? Mathf.Lerp(0.35f, 1.15f, k / 0.6f) : Mathf.Lerp(1.15f, 1f, (k - 0.6f) / 0.4f);
            }
            float angle = shakeT >= 0f ? shakeDegrees * (1f - shakeT / shakeSeconds) * Mathf.Sin(shakeT * 45f) : 0f;
            Rect.localScale = Vector3.one * s;
            Rect.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        // 글이 없으면 칸을 꺼서 레이아웃이 너비를 줄이게 한다
        private static void SetText(TMP_Text text, string value, Color color)
        {
            if (!text) return;
            bool on = !string.IsNullOrEmpty(value);
            if (text.gameObject.activeSelf != on) text.gameObject.SetActive(on);
            if (!on) return;
            if (text.text != value) text.text = value;
            text.color = color;
        }

        private static T Pick<T>(T[] values, int index, T fallback) =>
            values != null && index >= 0 && index < values.Length ? values[index] : fallback;
    }
}
