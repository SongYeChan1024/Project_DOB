// =====================================================================
// TitleAmbienceView — 타이틀 배경을 은은하게 살아 있게 한다 (스토리보드 ④)
// 역할   : 배경을 조금 키워 두고 아주 천천히 좌우로 흐르게 하고, 굴뚝에서 연기 덩어리를 피워 올리고,
//          창문 · 등불 빛을 촛불처럼 서로 다른 박자로 일렁이게 한다. 로고 · 메뉴는 건드리지 않는다.
// 호출자 : 없음 (켜져 있는 동안 스스로 움직인다)
// 상태   : 연기 덩어리 묶음과 각자 나이, 다음 연기까지 남은 시간, 빛마다 박자
//
// 의사코드
// [켜짐]       배경을 panScale배로 키운다 → 빛마다 서로 다른 박자를 정한다
// [매 프레임]  배경: 키운 여유 안에서 panSeconds 주기로 좌우(조금 위아래)로 오간다
//              연기: 시간이 되면 굴뚝 자리에서 하나 띄운다 → 오른쪽 위로 오르며 커지고, 나타났다 흐려져 사라진다
//              빛: 기본 밝기 × (0.7 ~ 1.0 사이를 부드럽게 오가는 값)
// [꺼짐]       배경 크기 · 위치를 되돌린다
// 비고   : 연기 · 빛은 배경의 자식이라 배경과 함께 움직인다. 시간은 일시 정지와 무관하게 흐른다.
// =====================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>타이틀 배경 움직임.</summary>
    public sealed class TitleAmbienceView : MonoBehaviour
    {
        [Header("느린 화면 흐름")]
        /// <summary>흐르게 할 배경 (보통 이 오브젝트).</summary>
        [SerializeField] private RectTransform background;
        /// <summary>배경을 키우는 정도. 키운 여유만큼만 움직여 가장자리가 드러나지 않는다. 1이면 화면 흐름 없음 (Title 씬은 1: 정신없다는 의견).</summary>
        [SerializeField] private float panScale = 1.04f;
        /// <summary>한 번 오가는 데 걸리는 시간 (초).</summary>
        [SerializeField] private float panSeconds = 20f;

        [Header("굴뚝 연기")]
        /// <summary>연기 견본 (부드러운 덩어리). 평소 비활성. 복제본은 같은 부모 아래에 붙는다.</summary>
        [SerializeField] private Image puffTemplate;
        /// <summary>연기가 나오는 굴뚝 자리 (배경 안 비율 0~1).</summary>
        [SerializeField] private Vector2 chimney = new Vector2(0.756f, 0.85f);
        [SerializeField] private float puffEverySeconds = 3.5f;
        [SerializeField] private float puffLifeSeconds = 5.5f;
        /// <summary>한 덩어리가 사는 동안 움직이는 거리 (배경 크기 비율).</summary>
        [SerializeField] private Vector2 puffDrift = new Vector2(0.06f, 0.16f);
        [SerializeField] private float puffStartSize = 60f;
        [SerializeField] private float puffEndSize = 230f;
        [SerializeField, Range(0f, 1f)] private float puffAlpha = 0.45f;

        [Header("창문 · 등불 빛")]
        /// <summary>빛 그림들. 가장 밝을 때의 밝기는 각 그림의 색 투명도로 정한다.</summary>
        [SerializeField] private Graphic[] glows = Array.Empty<Graphic>();
        /// <summary>빛 전체 세기 (그림 색 투명도에 곱한다).</summary>
        [SerializeField, Range(0f, 1f)] private float glowAlpha = 1f;
        /// <summary>일렁이는 빠르기 (초당).</summary>
        [SerializeField] private float glowSpeed = 1.3f;

        private struct Puff { public Image image; public float age; public float sway; }
        private readonly List<Puff> puffs = new List<Puff>();
        private float nextPuff, time;
        private float[] glowPhase = Array.Empty<float>();
        private Vector3 homeScale;
        private Vector2 homePos;

        private void OnEnable()
        {
            if (background) { homeScale = background.localScale; homePos = background.anchoredPosition; background.localScale = homeScale * panScale; }
            if (puffTemplate) puffTemplate.gameObject.SetActive(false);
            glowPhase = new float[glows.Length];
            for (int i = 0; i < glows.Length; i++) glowPhase[i] = i * 7.31f; // 서로 다른 박자
            nextPuff = 0.5f;
        }

        private void OnDisable()
        {
            if (background) { background.localScale = homeScale; background.anchoredPosition = homePos; }
        }

        private void Update()
        {
            float dt = Time.unscaledDeltaTime;
            time += dt;
            Pan();
            Smoke(dt);
            for (int i = 0; i < glows.Length; i++)
            {
                if (!glows[i]) continue;
                float n = Mathf.PerlinNoise(glowPhase[i], time * glowSpeed);
                glows[i].canvasRenderer.SetAlpha(glowAlpha * Mathf.Lerp(0.7f, 1f, n));
            }
        }

        // 키운 여유(가장자리가 안 보이는 범위)의 90% 안에서 좌우로, 그 절반만큼 위아래로
        private void Pan()
        {
            if (!background) return;
            var size = background.rect.size;
            float room = (panScale - 1f) * 0.5f * 0.9f;
            float a = time / panSeconds * Mathf.PI * 2f;
            background.anchoredPosition = homePos + new Vector2(Mathf.Sin(a) * size.x * room, Mathf.Sin(a * 0.5f) * size.y * room * 0.5f);
        }

        private void Smoke(float dt)
        {
            if (!puffTemplate) return;
            nextPuff -= dt;
            if (nextPuff <= 0f)
            {
                nextPuff = puffEverySeconds * UnityEngine.Random.Range(0.8f, 1.2f);
                int free = puffs.FindIndex(p => p.age < 0f);
                var puff = new Puff { age = 0f, sway = UnityEngine.Random.Range(-1f, 1f) };
                if (free >= 0) { puff.image = puffs[free].image; puffs[free] = puff; }
                else { puff.image = Instantiate(puffTemplate, puffTemplate.transform.parent); puffs.Add(puff); }
                puff.image.gameObject.SetActive(true);
            }

            var area = ((RectTransform)puffTemplate.transform.parent).rect.size;
            for (int i = 0; i < puffs.Count; i++)
            {
                var p = puffs[i];
                if (p.age < 0f) continue;
                p.age += dt;
                float t = p.age / puffLifeSeconds;
                if (t >= 1f) { p.age = -1f; p.image.gameObject.SetActive(false); puffs[i] = p; continue; }
                var rt = p.image.rectTransform;
                rt.anchorMin = rt.anchorMax = chimney;
                float ease = 1f - (1f - t) * (1f - t);
                rt.anchoredPosition = new Vector2((puffDrift.x * ease + 0.012f * p.sway * Mathf.Sin(t * 6f)) * area.x, puffDrift.y * ease * area.y);
                rt.sizeDelta = Vector2.one * Mathf.Lerp(puffStartSize, puffEndSize, ease);
                float fade = t < 0.2f ? t / 0.2f : 1f - (t - 0.2f) / 0.8f;
                p.image.canvasRenderer.SetAlpha(puffAlpha * fade);
                puffs[i] = p;
            }
        }
    }
}
