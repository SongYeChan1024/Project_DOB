// =====================================================================
// PageCurlGraphic — 휘어지며 넘어가는 종이 한 장 그리기
// 역할   : 종이를 세로 띠 여러 개로 나눠, 책등을 축으로 둥글게 휘며 넘어가는 모양을 그린다.
//          바깥 끝이 먼저 들리고(휘어짐), 들린 부분은 가까워 보이게 살짝 커지고(원근), 그늘진다.
// 호출자 : NotebookPager (SetAngle)
// 호출   : 없음
// 상태   : 넘김 각도(0 = 오른쪽에 펴짐, π = 왼쪽에 펴짐)와 넘기는 방향만 가진다.
//
// 의사코드
// [모양]  이 오브젝트의 사각형 = 두 쪽 너비, 가운데 세로선 = 책등
//         띠 i의 각도 = 넘김 각도 + 방향 × 휨 × sin(넘김 각도) × (책등에서 거리)^1.5  (0~π로 자름)
//         책등부터 각 띠 방향으로 이어 붙여 x(가로), z(들림)를 구하고, 원근 = f / (f - z)로 크기 보정
// [그림]  띠가 오른쪽을 향하면 앞면(오른쪽 쪽 종이), 왼쪽을 향하면 뒷면(왼쪽 쪽 종이)을 붙인다
//         한 장의 그림(UI_Notebook_Leaf) 왼쪽 = 앞면, 오른쪽 = 뒷면
// [순서]  멀리 있는 띠부터 그려 가까운 띠가 위에 오게 한다
// =====================================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>휘어지며 넘어가는 종이. 표시만 한다.</summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class PageCurlGraphic : MaskableGraphic
    {
        /// <summary>앞면·뒷면 종이를 나란히 붙인 그림.</summary>
        [SerializeField] private Sprite leafSprite;
        /// <summary>그림 안에서 앞면(오른쪽 쪽 종이)이 차지하는 가로 범위(0~1).</summary>
        [SerializeField] private Vector2 frontU = new Vector2(0f, 455f / 919f);
        /// <summary>그림 안에서 뒷면(왼쪽 쪽 종이)이 차지하는 가로 범위(0~1).</summary>
        [SerializeField] private Vector2 backU = new Vector2(463f / 919f, 1f);
        [SerializeField, Range(4, 64)] private int segments = 28;
        /// <summary>휘는 정도(라디안). 클수록 바깥 끝이 먼저 들린다.</summary>
        [SerializeField] private float bend = 0.9f;
        /// <summary>원근 거리(쪽 너비 배수). 작을수록 들린 부분이 크게 보인다.</summary>
        [SerializeField] private float perspective = 3f;
        [SerializeField, Range(0f, 1f)] private float shade = 0.35f;

        private float angle;
        private float direction = 1f;
        private readonly List<int> order = new List<int>();

        /// <summary>바깥 끝이 화면에서 책등으로부터 떨어진 거리(오른쪽 +, 왼쪽 -). 쪽 내용 크기를 맞출 때 쓴다.</summary>
        public float EdgeX { get; private set; }

        public override Texture mainTexture => leafSprite != null ? leafSprite.texture : s_WhiteTexture;

        /// <summary>넘김 각도(0~π)와 방향(앞으로 넘기면 +1, 뒤로 넘기면 -1)을 정한다.</summary>
        public void SetAngle(float radians, float dir)
        {
            angle = Mathf.Clamp(radians, 0f, Mathf.PI);
            direction = dir >= 0f ? 1f : -1f;
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            Rect r = rectTransform.rect;
            float pageW = r.width * 0.5f;
            float halfH = r.height * 0.5f;
            Vector2 spine = r.center;
            int n = Mathf.Max(4, segments);
            float f = perspective * pageW;

            // 1) 띠마다 각도와 위치(x, z)
            var a = new float[n + 1];
            var x = new float[n + 1];
            var z = new float[n + 1];
            float lift = Mathf.Sin(angle);
            for (int i = 0; i <= n; i++)
            {
                float t = (float)i / n;
                a[i] = Mathf.Clamp(angle + direction * bend * lift * Mathf.Pow(t, 1.5f), 0f, Mathf.PI);
            }
            float step = pageW / n;
            for (int i = 1; i <= n; i++)
            {
                float mid = (a[i - 1] + a[i]) * 0.5f;
                x[i] = x[i - 1] + Mathf.Cos(mid) * step;
                z[i] = z[i - 1] + Mathf.Sin(mid) * step;
            }
            EdgeX = x[n] * f / Mathf.Max(1f, f - z[n]);

            // 2) 그림 좌표(UV)
            Rect uvRect = SpriteUv();

            // 3) 먼 띠부터 그리기
            order.Clear();
            for (int i = 0; i < n; i++) order.Add(i);
            order.Sort((p, q) => (z[p] + z[p + 1]).CompareTo(z[q] + z[q + 1]));

            foreach (int i in order)
            {
                float mid = (a[i] + a[i + 1]) * 0.5f;
                bool front = mid < Mathf.PI * 0.5f;
                float t0 = (float)i / n, t1 = (float)(i + 1) / n;
                float u0 = front ? Mathf.Lerp(frontU.x, frontU.y, t0) : Mathf.Lerp(backU.y, backU.x, t0);
                float u1 = front ? Mathf.Lerp(frontU.x, frontU.y, t1) : Mathf.Lerp(backU.y, backU.x, t1);
                u0 = Mathf.Lerp(uvRect.xMin, uvRect.xMax, u0);
                u1 = Mathf.Lerp(uvRect.xMin, uvRect.xMax, u1);

                int start = vh.currentVertCount;
                AddVertex(vh, spine, x[i], z[i], -halfH, f, new Vector2(u0, uvRect.yMin), a[i]);
                AddVertex(vh, spine, x[i], z[i], halfH, f, new Vector2(u0, uvRect.yMax), a[i]);
                AddVertex(vh, spine, x[i + 1], z[i + 1], halfH, f, new Vector2(u1, uvRect.yMax), a[i + 1]);
                AddVertex(vh, spine, x[i + 1], z[i + 1], -halfH, f, new Vector2(u1, uvRect.yMin), a[i + 1]);
                vh.AddTriangle(start, start + 1, start + 2);
                vh.AddTriangle(start + 2, start + 3, start);
            }
        }

        private void AddVertex(VertexHelper vh, Vector2 spine, float px, float pz, float py, float f, Vector2 uv, float a)
        {
            float p = f / Mathf.Max(1f, f - pz); // 들린 곳일수록 크게(가깝게)
            var pos = new Vector3(spine.x + px * p, spine.y + py * p, 0f);
            float s = 1f - shade * Mathf.Sin(a); // 세워질수록 그늘
            Color c = color * new Color(s, s, s, 1f);
            vh.AddVert(pos, c, uv);
        }

        // 스프라이트가 큰 그림(아틀라스)의 일부여도 맞는 좌표를 쓴다.
        private Rect SpriteUv()
        {
            if (leafSprite == null || leafSprite.texture == null)
                return new Rect(0, 0, 1, 1);
            Rect tr = leafSprite.textureRect;
            float w = leafSprite.texture.width, h = leafSprite.texture.height;
            return new Rect(tr.x / w, tr.y / h, tr.width / w, tr.height / h);
        }
    }
}
