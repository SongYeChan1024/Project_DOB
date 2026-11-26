// =====================================================================
// StampPressView — 귀환 보고 결과 도장을 「쾅」 찍는 연출 (스토리보드 ③)
// 역할   : 도장 묶음(테두리 · 글자)을 크게 · 반투명 · 기울인 채 띄웠다가 눌러 찍고 자리잡게 한다.
//          찍는 순간 잉크 번짐 고리가 퍼지고, 종이 부스러기가 튀고, 보고 종이가 살짝 흔들린다.
//          도장 색 · 글자는 정하지 않는다 (ReturnReportView가 정한다). 번짐 고리 색만 같은 색으로 맞춘다.
// 호출자 : ReturnReportView (Press — 손님 보고를 그릴 때마다)
// 상태   : 찍은 뒤 지난 시간, 도장 · 종이의 원래 자리 · 기울기
//
// 의사코드
// [찍기]     시간을 0부터, 번짐 고리 색을 도장 색으로, heavy(간신히 · 부상)면 흔들림을 크게
// [매 프레임] 0~들기 시간: 크기 1.9 → 0.9, 투명 0.25 → 1, 기울기 원래 −18° → 원래
//            들기 끝(쾅): 종이 흔들림 시작 · 번짐 고리 · 부스러기 시작
//            ~끝: 크기 0.9 → 1 (살짝 튕김), 번짐 고리는 커지며 사라짐, 부스러기는 밖으로 날아가며 사라짐
//            끝나면 모두 원래대로
// 비고   : 💡 효과음 자리 — 쾅 순간에 Pressed를 알린다 (사운드 연결은 사운드 목록 확정 뒤).
// =====================================================================
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>결과 도장 「쾅」 연출.</summary>
    public sealed class StampPressView : MonoBehaviour
    {
        /// <summary>찍히는 도장 묶음 (테두리 · 글자). 보통 이 컴포넌트의 오브젝트.</summary>
        [SerializeField] private RectTransform stamp;
        [SerializeField] private CanvasGroup stampGroup;
        /// <summary>찍는 순간 퍼지는 잉크 번짐 고리.</summary>
        [SerializeField] private Image ink;
        /// <summary>튀는 종이 부스러기들.</summary>
        [SerializeField] private RectTransform[] dust = Array.Empty<RectTransform>();
        /// <summary>찍을 때 흔들리는 보고 종이.</summary>
        [SerializeField] private RectTransform paper;

        [Header("시간 (초)")]
        [SerializeField] private float dropSeconds = 0.15f;
        [SerializeField] private float settleSeconds = 0.3f;

        [Header("모양")]
        [SerializeField] private float startScale = 1.9f;
        [SerializeField] private float pressScale = 0.9f;
        [SerializeField] private float startTilt = -18f;
        [SerializeField] private float inkScale = 1.45f;
        [SerializeField] private float dustDistance = 46f;
        /// <summary>종이 흔들림 (캔버스 단위). 간신히 · 부상은 heavyShake.</summary>
        [SerializeField] private float shake = 4f;
        [SerializeField] private float heavyShake = 9f;

        /// <summary>도장이 종이에 닿은 순간 (💡 효과음 자리).</summary>
        public event Action Pressed;

        private float t = -1f, shakeNow;
        private bool homeSaved;
        private Vector2 paperHome;
        private Quaternion stampHome;
        private Vector2[] dustHome = Array.Empty<Vector2>();

        /// <summary>도장을 찍는다. color는 번짐 고리 색, heavy면 종이를 더 크게 흔든다.</summary>
        public void Press(Color color, bool heavy)
        {
            SaveHome();
            if (ink) ink.color = new Color(color.r, color.g, color.b, 0f);
            shakeNow = heavy ? heavyShake : shake;
            t = 0f;
            Pose();
        }

        private void SaveHome()
        {
            if (homeSaved) return;
            homeSaved = true;
            stampHome = stamp.localRotation;
            if (paper) paperHome = paper.anchoredPosition;
            dustHome = new Vector2[dust.Length];
            for (int i = 0; i < dust.Length; i++) if (dust[i]) dustHome[i] = dust[i].anchoredPosition;
        }

        private void OnDisable()
        {
            if (t < 0f) return;
            t = dropSeconds + settleSeconds; // 닫히면 끝 모양으로
            Pose();
            t = -1f;
        }

        private void Update()
        {
            if (t < 0f) return;
            float before = t;
            t += Mathf.Min(Time.unscaledDeltaTime, 0.05f); // 멈칫한 프레임에 「쾅」이 건너뛰지 않게
            if (before < dropSeconds && t >= dropSeconds) Pressed?.Invoke();
            Pose();
            if (t >= dropSeconds + settleSeconds) t = -1f;
        }

        private void Pose()
        {
            float total = dropSeconds + settleSeconds;
            float k = Mathf.Min(t, total);
            float scale, alpha = 1f, tilt = 0f;
            if (k < dropSeconds)
            {
                float d = k / dropSeconds;
                float e = d * d; // 점점 빨라지며 떨어진다
                scale = Mathf.Lerp(startScale, pressScale, e);
                alpha = Mathf.Lerp(0.25f, 1f, e);
                tilt = Mathf.Lerp(startTilt, 0f, e);
            }
            else
            {
                float s = (k - dropSeconds) / settleSeconds;
                scale = Mathf.Lerp(pressScale, 1f, 1f - (1f - s) * (1f - s)) + 0.04f * Mathf.Sin(s * Mathf.PI) ; // 살짝 넘쳤다 돌아온다
            }
            stamp.localScale = Vector3.one * scale;
            stamp.localRotation = stampHome * Quaternion.Euler(0f, 0f, tilt);
            if (stampGroup) stampGroup.alpha = alpha;

            // 쾅 이후: 번짐 고리 · 부스러기 · 종이 흔들림
            float after = k < dropSeconds ? -1f : (k - dropSeconds) / settleSeconds;
            if (ink)
            {
                ink.rectTransform.localScale = Vector3.one * (after < 0f ? 1f : Mathf.Lerp(1f, inkScale, after));
                var c = ink.color; c.a = after < 0f ? 0f : 0.45f * (1f - after); ink.color = c;
            }
            for (int i = 0; i < dust.Length; i++)
            {
                if (!dust[i]) continue;
                bool on = after >= 0f && after < 1f;
                if (dust[i].gameObject.activeSelf != on) dust[i].gameObject.SetActive(on);
                if (!on) { dust[i].anchoredPosition = dustHome[i]; continue; }
                dust[i].anchoredPosition = dustHome[i] + dustHome[i].normalized * dustDistance * after;
                dust[i].localScale = Vector3.one * (1f - after);
            }
            if (paper)
            {
                float w = after < 0f ? 0f : shakeNow * (1f - after) * Mathf.Sin(after * 40f);
                paper.anchoredPosition = paperHome + new Vector2(w, -Mathf.Abs(w) * 0.5f);
            }
        }
    }
}
