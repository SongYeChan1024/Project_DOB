// =====================================================================
// PrepScreensView — 준비 화면 묶음 (시장 · 레시피 책 · 수첩 · 가게 개량 · 입구 문패)
// 역할   : 다섯 화면 중 하나만 켜서 보여 주고, 뒤를 어둡게 가려 세계 조작을 막는다.
//          열고 닫을 때 살짝 페이드한다. 표시만 하고 금화 · 재고는 바꾸지 않는다.
// 호출자 : PrepScreenPresenter (Open, Refresh, Close)
// 호출   : MarketView, RecipeBookView, PrepNotebookView, UpgradeView, DoorSignView
// 상태   : 지금 열린 화면, 페이드 중 여부.
//
// 의사코드
// [열기]    다른 화면이 열려 있으면 바꿔 끼운다 → 데이터 표시 → 서서히 나타남
// [갱신]    구매 · 개량 결과로 받은 새 데이터로 지금 화면만 다시 그린다
// [닫기]    서서히 사라짐 → 전부 끔
// =====================================================================
using System.Collections;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>준비 화면 묶음.</summary>
    public sealed class PrepScreensView : MonoBehaviour
    {
        /// <summary>화면 전체 (어두운 덮개 + 다섯 화면). 닫혀 있으면 꺼 둔다.</summary>
        [SerializeField] private CanvasGroup content;
        [SerializeField] private float fadeSeconds = 0.15f;

        [Header("화면"), SerializeField] private MarketView market;
        [SerializeField] private RecipeBookView recipeBook;
        [SerializeField] private PrepNotebookView notebook;
        [SerializeField] private UpgradeView upgrade;
        [SerializeField] private DoorSignView doorSign;

        private Coroutine fade;

        public MarketView Market => market;
        public RecipeBookView RecipeBook => recipeBook;
        public PrepNotebookView Notebook => notebook;
        public UpgradeView Upgrade => upgrade;
        public DoorSignView DoorSign => doorSign;

        /// <summary>지금 열린 화면. 닫혀 있으면 None.</summary>
        public PrepScreen Current { get; private set; } = PrepScreen.None;
        public bool IsOpen => Current != PrepScreen.None;

        private void Awake()
        {
            content.alpha = 0f;
            content.gameObject.SetActive(false);
        }

        /// <summary>화면 하나를 연다. 이미 다른 화면이 열려 있으면 바꿔 끼운다.</summary>
        public void Open(PrepScreen screen, PrepViewData data)
        {
            if (screen == PrepScreen.None || data == null)
                return;
            bool wasOpen = IsOpen;
            Current = screen;
            SetInput(true);
            // 먼저 켜야 각 화면의 Awake(버튼 연결)가 그리기 전에 끝난다
            content.gameObject.SetActive(true);
            ShowOnly(screen);
            Refresh(data);
            if (!wasOpen) StartFade(1f, false);
        }

        /// <summary>지금 열린 화면을 새 데이터로 다시 그린다.</summary>
        public void Refresh(PrepViewData data)
        {
            switch (Current)
            {
                case PrepScreen.Market: market.Show(data); break;
                case PrepScreen.RecipeBook: recipeBook.Show(data); break;
                case PrepScreen.Notebook: notebook.Show(data); break;
                case PrepScreen.Upgrade: upgrade.Show(data); break;
                case PrepScreen.DoorSign: doorSign.Show(data); break;
            }
        }

        /// <summary>닫는다.</summary>
        public void Close()
        {
            if (!IsOpen) return;
            Current = PrepScreen.None;
            // 닫히는 페이드 동안 버튼이 또 눌리지 않게 바로 입력을 막는다 (PR #20 버그: 문 열기 두 번)
            SetInput(false);
            StartFade(0f, true);
        }

        private void ShowOnly(PrepScreen screen)
        {
            market.gameObject.SetActive(screen == PrepScreen.Market);
            recipeBook.gameObject.SetActive(screen == PrepScreen.RecipeBook);
            notebook.gameObject.SetActive(screen == PrepScreen.Notebook);
            upgrade.gameObject.SetActive(screen == PrepScreen.Upgrade);
            doorSign.gameObject.SetActive(screen == PrepScreen.DoorSign);
        }

        private void StartFade(float to, bool hideAtEnd)
        {
            if (fade != null) StopCoroutine(fade);
            fade = isActiveAndEnabled ? StartCoroutine(Fade(to, hideAtEnd)) : null;
            if (fade == null)
            {
                content.alpha = to;
                if (hideAtEnd) content.gameObject.SetActive(false);
            }
        }

        private void SetInput(bool on)
        {
            content.interactable = on;
            content.blocksRaycasts = on;
        }

        private IEnumerator Fade(float to, bool hideAtEnd)
        {
            float from = content.alpha;
            // 일시 정지(timeScale 0)와 무관하게 끝나도록 실제 시간을 쓴다.
            for (float t = 0f; t < fadeSeconds; t += Time.unscaledDeltaTime)
            {
                content.alpha = Mathf.Lerp(from, to, t / fadeSeconds);
                yield return null;
            }
            content.alpha = to;
            if (hideAtEnd) content.gameObject.SetActive(false);
            fade = null;
        }
    }
}
