// =====================================================================
// GuestBubbleView — 손님 머리 위 말풍선 하나 (시안 10)
// 역할   : [기다림 원 또는 기호] [문구]를 한 줄로 그린다. 너비는 내용에 맞춰 레이아웃이 정한다 (빈칸 없음).
//          기다림 원은 남은 비율만큼 12시부터 채워, 시간이 갈수록 반시계 방향으로 줄어든다.
//          남은 비율 · 점수 구간 · 표정 종류는 받은 값만 그린다. 문구는 💡 가안 (기획 원고 오면 교체).
// 호출자 : GuestBubblePresenter (ShowWaiting, ShowServed, ShowLeaving, Hide)
// 상태   : 없음
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

        private RectTransform rect;
        private RectTransform Rect => rect ? rect : rect = (RectTransform)transform;

        /// <summary>기다림: 남은 비율(1→0)의 원과, 점수 구간(2 · 1 · 0)에 맞는 색 · 불평 문구.</summary>
        public void ShowWaiting(Vector2 canvasPoint, float patienceLeft, int waitScore)
        {
            int stage = Mathf.Clamp(2 - waitScore, 0, 2); // 2점 = 0번 칸
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
        }

        private void Place(Vector2 canvasPoint)
        {
            Rect.anchoredPosition = canvasPoint;
            if (!gameObject.activeSelf) gameObject.SetActive(true);
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
