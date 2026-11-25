// =====================================================================
// PrepScreensView — 준비 화면 프리팹 하나의 겉 (PF_Shop · PF_Notebook · PF_DoorSign)
// 역할   : 이 프리팹에 들어 있는 화면 중 하나만 켜서 보여 주고, 뒤를 어둡게 가려 세계 조작을 막는다.
//          PF_Shop: 시장 · 가게 개량 (위쪽 탭으로 바꿈) / PF_Notebook: 수첩 (레시피는 수첩 책갈피) / PF_DoorSign: 입구 문패
//          열고 닫을 때 살짝 페이드한다. 표시만 하고 금화 · 재고는 바꾸지 않는다.
// 호출자 : PrepScreenPresenter (Open, Refresh, Close)
// 호출   : MarketView, PrepNotebookView, UpgradeView, DoorSignView (프리팹에 없는 화면은 비워 둠)
// 상태   : 지금 열린 화면, 페이드 중 여부.
//
// 의사코드
// [열기]    다른 화면이 열려 있으면 바꿔 끼운다 → 데이터 표시 → 서서히 나타남
// [탭]      상점의 시장 · 가게 개량 탭을 누르면 마지막 데이터로 그 화면을 그린다
// [갱신]    구매 · 개량 결과로 받은 새 데이터로 지금 화면만 다시 그린다
// [닫기]    서서히 사라짐 → 전부 끔
// =====================================================================
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>준비 화면 묶음.</summary>
    public sealed class PrepScreensView : MonoBehaviour
    {
        /// <summary>화면 전체 (어두운 덮개 + 화면들). 닫혀 있으면 꺼 둔다.</summary>
        [SerializeField] private CanvasGroup content;
        [SerializeField] private float fadeSeconds = 0.15f;

        [Header("화면 (이 프리팹에 없는 것은 비워 둠)"), SerializeField] private MarketView market;
        [SerializeField] private PrepNotebookView notebook;
        [SerializeField] private UpgradeView upgrade;
        [SerializeField] private DoorSignView doorSign;

        [Header("상점 탭 (PF_Shop만)"), SerializeField] private Button marketTab;
        [SerializeField] private Button upgradeTab;
        [SerializeField] private Color tabLabelColor = new Color32(190, 170, 135, 255);
        [SerializeField] private Color selectedTabLabelColor = new Color32(240, 196, 110, 255);
        [SerializeField, Range(0f, 1f)] private float tabDim = 0.6f;

        private Coroutine fade;
        private PrepViewData last;

        public MarketView Market => market;
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
            if (marketTab) marketTab.onClick.AddListener(() => SwitchTab(PrepScreen.Market));
            if (upgradeTab) upgradeTab.onClick.AddListener(() => SwitchTab(PrepScreen.Upgrade));
        }

        /// <summary>이 프리팹이 그 화면을 가지고 있는지. 레시피는 수첩 안에 있다.</summary>
        public bool Has(PrepScreen screen)
        {
            switch (screen)
            {
                case PrepScreen.Market: return market;
                case PrepScreen.Upgrade: return upgrade;
                case PrepScreen.Notebook:
                case PrepScreen.RecipeBook: return notebook;
                case PrepScreen.DoorSign: return doorSign;
                default: return false;
            }
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
            // 레시피로 열면 수첩의 레시피 책갈피를 펼친다
            if (screen == PrepScreen.RecipeBook) notebook.SelectBookmark(PrepNotebookView.RecipeBookmark);
            if (!wasOpen) StartFade(1f, false);
        }

        /// <summary>지금 열린 화면을 새 데이터로 다시 그린다.</summary>
        public void Refresh(PrepViewData data)
        {
            last = data;
            switch (Current)
            {
                case PrepScreen.Market: market.Show(data); break;
                case PrepScreen.RecipeBook:
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

        // 상점 탭: 같은 창 안에서 시장 ↔ 가게 개량
        private void SwitchTab(PrepScreen screen)
        {
            if (!IsOpen || Current == screen || last == null) return;
            Current = screen;
            ShowOnly(screen);
            Refresh(last);
        }

        private void ShowOnly(PrepScreen screen)
        {
            if (market) market.gameObject.SetActive(screen == PrepScreen.Market);
            if (notebook) notebook.gameObject.SetActive(screen == PrepScreen.Notebook || screen == PrepScreen.RecipeBook);
            if (upgrade) upgrade.gameObject.SetActive(screen == PrepScreen.Upgrade);
            if (doorSign) doorSign.gameObject.SetActive(screen == PrepScreen.DoorSign);
            PaintTab(marketTab, screen == PrepScreen.Market);
            PaintTab(upgradeTab, screen == PrepScreen.Upgrade);
        }

        // 고른 탭은 밝은 판 · 금색 글자, 나머지는 어둡게
        private void PaintTab(Button tab, bool selected)
        {
            if (!tab) return;
            float v = selected ? 1f : tabDim;
            tab.targetGraphic.color = new Color(v, v, v, 1f);
            var label = tab.GetComponentInChildren<TMP_Text>(true);
            if (label) label.color = selected ? selectedTabLabelColor : tabLabelColor;
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
