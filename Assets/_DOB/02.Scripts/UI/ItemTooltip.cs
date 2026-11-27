// =====================================================================
// ItemTooltip — 양념 · 재료에 마우스를 올리면 뜨는 작은 설명 상자
// 역할   : 이름(굵게)과 효과 한두 줄을 보여 준다. 가리킨 그림 바로 위에 뜨고,
//          화면 밖으로 나가면 아래로 내리거나 옆으로 민다.
//          툭 나타나지 않고 살짝 아래에서 떠오르며 서서히 보인다 (시안 22 ④)
// 호출자 : HoverInfo (Show, Hide)
// 상태   : 없음 (보이기 / 숨기기만)
// 비고   : 클릭을 막지 않도록 글자 · 바탕 모두 raycast를 끈다.
// =====================================================================
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>마우스를 올린 물건의 이름 · 효과 상자.</summary>
    public sealed class ItemTooltip : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text bodyText;
        /// <summary>가리킨 그림과 상자 사이 간격.</summary>
        [SerializeField] private float gap = 8f;
        /// <summary>나타날 때 떠오르는 시간 (초) · 아래에서 시작하는 거리 (캔버스 단위).</summary>
        [SerializeField] private float riseSeconds = 0.12f;
        [SerializeField] private float riseDistance = 6f;

        // 처음엔 빌더가 꺼 둔다 (여기서 Awake로 끄면 처음 Show 때 바로 다시 꺼져 버림)

        /// <summary>target 위에 이름 · 설명을 띄운다. 설명이 비면 이름만.</summary>
        public void Show(string title, string body, RectTransform target)
        {
            titleText.text = title;
            bodyText.text = body;
            bodyText.gameObject.SetActive(!string.IsNullOrEmpty(body));
            gameObject.SetActive(true);

            var rect = (RectTransform)transform;
            var parent = (RectTransform)rect.parent;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect); // 글자에 맞춰 크기부터 정한다

            // 가리킨 그림의 위 · 아래 가운데 (부모 좌표)
            var corners = new Vector3[4]; // 0 왼아래, 1 왼위, 2 오른위, 3 오른아래
            target.GetWorldCorners(corners);
            Vector2 top = parent.InverseTransformPoint((corners[1] + corners[2]) / 2f);
            Vector2 bottom = parent.InverseTransformPoint((corners[0] + corners[3]) / 2f);

            Vector2 size = rect.rect.size;
            Rect area = parent.rect;
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);

            // 기본은 위. 위가 모자라면 아래로
            float y = top.y + gap + size.y / 2f;
            if (y + size.y / 2f > area.yMax) y = bottom.y - gap - size.y / 2f;
            // 옆으로 넘치면 안쪽으로 민다
            float x = Mathf.Clamp(top.x, area.xMin + size.x / 2f, area.xMax - size.x / 2f);
            rect.anchoredPosition = new Vector2(x, y);
            rect.SetAsLastSibling(); // 다른 칸보다 위에 그리기

            // 툭 나타나지 않고 살짝 아래에서 떠오르며 보인다 (시안 22 ④)
            if (rise != null) StopCoroutine(rise);
            rise = StartCoroutine(Rise(rect, new Vector2(x, y)));
        }

        private Coroutine rise;

        private System.Collections.IEnumerator Rise(RectTransform rect, Vector2 home)
        {
            var group = GetComponent<CanvasGroup>();
            if (!group) { group = gameObject.AddComponent<CanvasGroup>(); group.blocksRaycasts = false; }
            for (float t = 0f; t < riseSeconds; t += Mathf.Min(Time.unscaledDeltaTime, 0.05f))
            {
                float k = t / riseSeconds;
                float e = 1f - (1f - k) * (1f - k);
                group.alpha = e;
                rect.anchoredPosition = home + new Vector2(0f, -riseDistance * (1f - e));
                yield return null;
            }
            group.alpha = 1f;
            rect.anchoredPosition = home;
            rise = null;
        }

        public void Hide() => gameObject.SetActive(false);
    }
}
