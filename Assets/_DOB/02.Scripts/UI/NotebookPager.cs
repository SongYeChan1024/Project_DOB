// =====================================================================
// NotebookPager — 수첩 쪽 넘기기 (두 쪽씩 펼친 책)
// 역할   : 수첩의 여러 쪽(위험·단서 사전·도감·손님 기록·층별 위험…)을 두 쪽씩 보여 주고,
//          책 그림의 왼쪽 아래 / 오른쪽 아래 말린 모서리를 누르면 실제 책장처럼 한 장을 넘긴다.
// 호출자 : GuestDialogueView (ShowSpread) / 모서리 버튼
// 호출   : PageCurlGraphic (SetAngle)
// 상태   : 지금 펼친 면(spread) 번호, 넘기는 중 여부, 안내용(가만히 둔 시간 · 띄운 횟수 · 이번에 넘긴 적 있는지).
//
// 의사코드 (구현 전 작성 · 2026-11-23 홍준우와 합의)
// [면]       pages[0,1] = 첫 면(왼쪽·오른쪽), pages[2,3] = 둘째 면 … 홀수 개면 마지막 오른쪽은 빈 쪽
// [다음 장]  오른쪽 아래 모서리 → 오른쪽 장이 바깥 끝부터 둥글게 들리며 책등을 넘어 왼쪽에 내려앉는다
//            장 앞면에는 지금 오른쪽 쪽 내용, 뒷면에는 다음 면의 왼쪽 쪽 내용이 붙어 함께 움직인다
//            장이 들리면 그 아래로 다음 면의 오른쪽 쪽이 드러난다
// [앞 장]    왼쪽 아래 모서리 → 위와 좌우 반대
// [막기]     넘기는 중에는 다른 넘기기 무시. 첫 면이면 앞 장 모서리, 마지막 면이면 다음 장 모서리를 숨긴다
// [바로가기] ShowSpread(번호, 연출 여부) — 응대 화면을 열 때는 연출 없이 첫 면으로
//
// 넘기기 안내 (2026-11-25 홍준우 요청 · 시안 12 v3 A안): 마우스를 올리지 않아도 넘길 수 있다는 걸 알게
// [가끔]     수첩이 펼쳐져 있고(HintsEnabled) 이번에 한 번도 넘기지 않았으면, 가만히 둔 지 hintDelay 뒤 ·
//            그다음엔 hintInterval마다 「넘기기 ▶」(마지막 면이면 「◀ 넘기기」)를 천천히 띄웠다 지운다. 펼칠 때마다 최대 maxAutoHints번
// [올림]     모서리에 마우스를 올리면 그쪽 안내를 띄우고, 벗어나면 지운다 (어두워지는 색 변화 대신)
// [넘기기]   한 번 넘기면 다시 펼칠 때까지 저절로는 안 띄운다. 넘기는 동안은 안내를 지운다
// [반짝임]   펼쳐져 있는 동안 말린 모서리 부분만 glowPeriod마다 짧게 밝아졌다 돌아온다 (누를 수 있다는 표시, 넘긴 뒤에도 계속)
// 비고       장을 휘어 들썩이는 안은 글자가 종이를 따라가지 않아 어색해서, 모서리 그림을 키우는 안은 네모 이음새가 보여서 뺐다
// =====================================================================
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>두 쪽씩 펼친 수첩의 쪽 넘기기. 표시만 하고 게임 상태는 모른다.</summary>
    public sealed class NotebookPager : MonoBehaviour
    {
        /// <summary>쪽 내용 순서대로. 짝수 번호는 왼쪽 자리, 홀수 번호는 오른쪽 자리에 둔다.</summary>
        [Header("쪽"), SerializeField] private GameObject[] pages;
        [SerializeField] private RectTransform leftSlot;
        [SerializeField] private RectTransform rightSlot;

        /// <summary>휘어지며 넘어가는 종이 한 장. 넘길 때만 켠다.</summary>
        [Header("넘어가는 장"), SerializeField] private PageCurlGraphic curl;
        [SerializeField] private float flipSeconds = 0.6f;

        /// <summary>책 그림의 말린 모서리 위에 겹친 버튼.</summary>
        [Header("모서리 버튼"), SerializeField] private Button prevCorner;
        [SerializeField] private Button nextCorner;

        [Header("넘기기 안내")]
        /// <summary>다음 장 모서리 옆 「넘기기 ▶」.</summary>
        [SerializeField] private TMP_Text nextHint;
        /// <summary>앞 장 모서리 옆 「◀ 넘기기」.</summary>
        [SerializeField] private TMP_Text prevHint;
        /// <summary>펼친 뒤 처음 안내까지 (초, 실제 시간).</summary>
        [SerializeField, Min(0f)] private float hintDelay = 3f;
        /// <summary>두 번째부터 안내 사이 간격 (초).</summary>
        [SerializeField, Min(0.5f)] private float hintInterval = 8f;
        /// <summary>나타나기 · 사라지기에 걸리는 시간 (초).</summary>
        [SerializeField, Min(0.05f)] private float hintFadeSeconds = 0.6f;
        /// <summary>다 나타난 채 머무는 시간 (초).</summary>
        [SerializeField, Min(0f)] private float hintHoldSeconds = 2f;
        /// <summary>펼칠 때마다 저절로 띄우는 최대 횟수 (마우스 올림은 따로).</summary>
        [SerializeField, Min(0)] private int maxAutoHints = 2;

        [Header("모서리 반짝임")]
        /// <summary>앞 · 다음 장 말린 모서리 위의 빛 (말린 부분 모양 그림, 모서리 버튼의 자식).</summary>
        [SerializeField] private Graphic prevGlow;
        [SerializeField] private Graphic nextGlow;
        /// <summary>한 번 반짝이는 주기 (초).</summary>
        [SerializeField, Min(0.2f)] private float glowPeriod = 2.4f;
        /// <summary>가장 밝을 때의 빛 투명도 (0~1).</summary>
        [SerializeField, Range(0f, 1f)] private float glowMax = 0.8f;

        /// <summary>수첩이 펼쳐져 보이는 동안만 켠다 (GuestDialogueView). 켤 때마다 넘긴 기록 · 안내 횟수를 지운다.</summary>
        public bool HintsEnabled
        {
            get => hintsEnabled;
            set
            {
                if (value && !hintsEnabled) { turnedSinceShown = false; idle = 0f; autoHints = 0; }
                hintsEnabled = value;
                if (!value) HideHints();
            }
        }

        private int spread;
        private bool isFlipping;
        private bool hintsEnabled;
        private bool turnedSinceShown;
        private float idle;
        private int autoHints;
        private Coroutine hinting;
        private RectTransform frontHolder; // 장 앞면(오른쪽을 향할 때) 내용 자리
        private RectTransform backHolder;  // 장 뒷면(왼쪽을 향할 때) 내용 자리
        private CanvasGroup frontGroup;
        private CanvasGroup backGroup;

        public int SpreadCount => (pages.Length + 1) / 2;

        private void Awake()
        {
            prevCorner.onClick.AddListener(() => { turnedSinceShown = true; ShowSpread(spread - 1, true); });
            nextCorner.onClick.AddListener(() => { turnedSinceShown = true; ShowSpread(spread + 1, true); });
            // 마우스를 올리면 어두워지던 색 변화 대신 넘기기 안내를 띄운다
            prevCorner.transition = Selectable.Transition.None;
            nextCorner.transition = Selectable.Transition.None;
            OnHover(prevCorner, () => prevHint);
            OnHover(nextCorner, () => nextHint);
            SetupLeaf();
            curl.gameObject.SetActive(false);
            HideHints();
            Apply(0);
        }

        private void OnDisable() => HideHints(); // 꺼질 때 반쯤 보이는 안내가 남지 않게

        private void Update()
        {
            // 펼쳐져 있는 동안 말린 모서리가 짧게 반짝였다 쉰다 (누를 수 있는 곳이라는 표시). 숨은 모서리는 자식인 빛도 같이 숨는다
            float k = hintsEnabled ? Mathf.Pow(Mathf.Max(0f, Mathf.Sin(2f * Mathf.PI * Time.unscaledTime / glowPeriod)), 3f) : 0f;
            if (prevGlow) prevGlow.canvasRenderer.SetAlpha(glowMax * k);
            if (nextGlow) nextGlow.canvasRenderer.SetAlpha(glowMax * k);

            if (!hintsEnabled || turnedSinceShown || isFlipping || hinting != null || SpreadCount < 2 || autoHints >= maxAutoHints)
                return;
            idle += Time.unscaledDeltaTime;
            if (idle < (autoHints == 0 ? hintDelay : hintInterval)) return;
            idle = 0f;
            autoHints++;
            hinting = StartCoroutine(Blink(spread < SpreadCount - 1 ? nextHint : prevHint));
        }

        private void OnHover(Button corner, System.Func<TMP_Text> hint)
        {
            var trigger = corner.gameObject.AddComponent<EventTrigger>();
            var enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            enter.callback.AddListener(_ => { if (!isFlipping) FadeTo(hint(), 1f); });
            var exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            exit.callback.AddListener(_ => FadeTo(hint(), 0f));
            trigger.triggers.Add(enter);
            trigger.triggers.Add(exit);
        }

        // 천천히 나타나 잠시 머물다 사라진다
        private IEnumerator Blink(TMP_Text hint)
        {
            yield return Fade(hint, 1f);
            for (float t = 0f; t < hintHoldSeconds; t += Time.unscaledDeltaTime) yield return null;
            yield return Fade(hint, 0f);
            hinting = null;
        }

        private void FadeTo(TMP_Text hint, float target)
        {
            if (!hint || !isActiveAndEnabled) return;
            StopHint();
            hinting = StartCoroutine(FadeOnly(hint, target));
        }

        private IEnumerator FadeOnly(TMP_Text hint, float target)
        {
            yield return Fade(hint, target);
            hinting = null;
        }

        private IEnumerator Fade(TMP_Text hint, float target)
        {
            if (!hint) yield break;
            float start = hint.alpha;
            float seconds = hintFadeSeconds * Mathf.Abs(target - start);
            for (float t = 0f; t < seconds; t += Time.unscaledDeltaTime)
            {
                hint.alpha = Mathf.Lerp(start, target, t / seconds);
                yield return null;
            }
            hint.alpha = target;
        }

        private void StopHint()
        {
            if (hinting != null) StopCoroutine(hinting);
            hinting = null;
        }

        private void HideHints()
        {
            StopHint();
            if (nextHint) nextHint.alpha = 0f;
            if (prevHint) prevHint.alpha = 0f;
        }

        /// <summary>몇 번째 면(0부터)을 펼친다. animate면 책장 넘기는 연출을 한다.</summary>
        public void ShowSpread(int index, bool animate)
        {
            index = Mathf.Clamp(index, 0, SpreadCount - 1);
            if (isFlipping || index == spread)
                return;
            HideHints();
            if (animate && isActiveAndEnabled)
                StartCoroutine(Flip(index));
            else
                Apply(index);
        }

        // 연출 없이 바로 그 면을 보여 준다.
        private void Apply(int index)
        {
            spread = index;
            for (int i = 0; i < pages.Length; i++)
            {
                if (pages[i] == null) continue;
                pages[i].transform.SetParent(i % 2 == 0 ? leftSlot : rightSlot, false);
                pages[i].SetActive(i / 2 == index);
            }
            prevCorner.gameObject.SetActive(index > 0);
            nextCorner.gameObject.SetActive(index < SpreadCount - 1);
        }

        private GameObject Page(int i) => i >= 0 && i < pages.Length ? pages[i] : null;

        // 넘어가는 장을 두 쪽 전체에 겹치고(가운데 = 책등), 앞·뒷면 내용 자리를 만든다.
        private void SetupLeaf()
        {
            var leaf = curl.rectTransform;
            leaf.anchorMin = leaf.anchorMax = rightSlot.anchorMin;
            leaf.pivot = new Vector2(0.5f, 0.5f);
            float spineX = rightSlot.anchoredPosition.x - rightSlot.sizeDelta.x * 0.5f;
            leaf.anchoredPosition = new Vector2(spineX, rightSlot.anchoredPosition.y);
            leaf.sizeDelta = new Vector2(rightSlot.sizeDelta.x * 2f, rightSlot.sizeDelta.y);

            frontHolder = MakeHolder("FrontContent", new Vector2(0f, 0.5f), out frontGroup);
            backHolder = MakeHolder("BackContent", new Vector2(1f, 0.5f), out backGroup);
        }

        private RectTransform MakeHolder(string holderName, Vector2 pivot, out CanvasGroup group)
        {
            var go = new GameObject(holderName, typeof(RectTransform), typeof(CanvasGroup));
            var rt = (RectTransform)go.transform;
            rt.SetParent(curl.rectTransform, false);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f); // 책등
            rt.pivot = pivot;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = rightSlot.sizeDelta;
            group = go.GetComponent<CanvasGroup>();
            group.blocksRaycasts = false;
            return rt;
        }

        private IEnumerator Flip(int target)
        {
            isFlipping = true;
            prevCorner.gameObject.SetActive(false);
            nextCorner.gameObject.SetActive(false);

            bool forward = target > spread;
            // 장 앞면 = 오른쪽을 향한 면, 뒷면 = 왼쪽을 향한 면
            GameObject front = Page(forward ? spread * 2 + 1 : target * 2 + 1);
            GameObject back = Page(forward ? target * 2 : spread * 2);
            GameObject under = Page(forward ? target * 2 + 1 : target * 2);   // 장이 들리면 드러나는 쪽
            GameObject covered = Page(forward ? spread * 2 : spread * 2 + 1); // 장이 내려앉으면 가려지는 쪽

            Attach(front, frontHolder);
            Attach(back, backHolder);
            if (under != null) { under.transform.SetParent(forward ? rightSlot : leftSlot, false); under.SetActive(true); }
            curl.gameObject.SetActive(true);
            curl.transform.SetAsLastSibling();
            prevCorner.transform.SetAsLastSibling();
            nextCorner.transform.SetAsLastSibling();

            float from = forward ? 0f : Mathf.PI;
            float to = forward ? Mathf.PI : 0f;
            float dir = forward ? 1f : -1f;
            for (float t = 0f; t < flipSeconds; t += Time.unscaledDeltaTime)
            {
                SetLeaf(Mathf.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t / flipSeconds)), dir);
                yield return null;
            }
            SetLeaf(to, dir);

            if (covered != null) covered.SetActive(false);
            curl.gameObject.SetActive(false);
            isFlipping = false;
            Apply(target); // 앞·뒷면에 붙였던 쪽을 제자리로 돌려놓고 새 면을 보여 준다
        }


        private static void Attach(GameObject page, RectTransform holder)
        {
            if (page == null) return;
            page.transform.SetParent(holder, false);
            page.SetActive(true);
        }

        // 종이 모양을 바꾸고, 앞·뒷면 내용을 종이 너비에 맞춰 눌러 준다.
        private void SetLeaf(float angle, float dir)
        {
            curl.SetAngle(angle, dir);
            curl.Rebuild(CanvasUpdate.PreRender); // EdgeX를 바로 계산
            float pageW = rightSlot.sizeDelta.x;
            float frontScale = Mathf.Clamp01(curl.EdgeX / pageW);
            float backScale = Mathf.Clamp01(-curl.EdgeX / pageW);
            frontHolder.localScale = new Vector3(frontScale, 1f, 1f);
            backHolder.localScale = new Vector3(backScale, 1f, 1f);
            // 종이가 세워질수록 글씨는 흐려지게 (납작하게 눌린 글씨가 덜 어색하도록)
            frontGroup.alpha = Mathf.SmoothStep(0f, 1f, frontScale * 1.4f);
            backGroup.alpha = Mathf.SmoothStep(0f, 1f, backScale * 1.4f);
        }
    }
}
