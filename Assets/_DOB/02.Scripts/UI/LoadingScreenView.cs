// =====================================================================
// LoadingScreenView — 로딩 화면 표시
// 역할   : 금테 액자 속 목적지 그림·빛나는 목적지 제목·팁 한 줄·금색 진행 선(끝의 불씨)·검은 페이드·실패 안내를 보여 준다.
//          로드를 직접 하지 않고, 진행률과 표시 내용은 Presenter가 넘겨준다.
// 호출자 : LoadingScreenPresenter (Show, SetProgress, FadeTo, ShowFailure)
// 호출   : 없음 (실패 안내 버튼은 이벤트로만 알림)
// 상태   : 화면에 보이는 진행률(부드럽게 따라가는 값)만 가진다.
//
// 의사코드 (구현 전 작성 · 2026-11-23 홍준우와 합의)
// [보이기]   검은 바탕 가운데 금테 액자에 목적지 그림, 위에 목적지 제목, 아래에 팁 한 줄 (로딩 중 바뀌지 않음)
// [진행]     실제 진행률을 받으면 바가 부드럽게 따라 차오르고 절대 줄어들지 않는다
//            불씨는 채워진 끝을 따라 이동한다. 숫자(%)는 보이지 않는다
//            (시안 19 ⑦) 불씨는 따라가며 깜빡이고, 바가 다 차는 순간 바 전체가 한 번 금빛으로 반짝인다
// [페이드]   검은 덮개의 투명도를 정해진 시간 동안 바꾼다 (Presenter가 끝날 때까지 기다릴 수 있게 코루틴)
//            로딩 중에는 덮개가 투명해도 입력을 막아, 아래에 먼저 열린 목적지 씬 버튼이 눌리지 않게 한다
// [실패]     "불러오지 못했어요" 안내 + [타이틀로] (타이틀도 실패면 [게임 종료]) 버튼을 띄우고 누르면 알린다
//            버튼은 프리팹에 직렬화한 영구 연결 1개(OnFailureButtonClicked)로만 받는다. 코드로 리스너를 붙이지 않는다
//            한 번 누르면 버튼을 잠가 같은 요청이 반복되지 않게 한다
// =====================================================================
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>로딩 화면 표시. 로드·씬 전환은 하지 않는다.</summary>
    public sealed class LoadingScreenView : MonoBehaviour
    {
        [Header("그림·제목·팁"), SerializeField] private Image illustration;
        /// <summary>목적지 제목 (예: 층계참 식당, 아침 귀환).</summary>
        [SerializeField] private TMP_Text headingText;
        /// <summary>목적지 제목 글씨 그림 (타이틀 로고처럼 빛 번짐이 들어간 PNG). 있으면 headingText 대신 보인다.</summary>
        [SerializeField] private Image headingImage;
        /// <summary>제목 그림 크기 배율. 그림은 2배 해상도로 만들었으므로 0.5.</summary>
        [SerializeField] private float headingImageScale = 0.5f;
        [SerializeField] private TMP_Text tipText;

        /// <summary>진행 바 채움 (9-slice 그림). 진행률만큼 오른쪽 끝을 늘인다.</summary>
        [Header("진행 바"), SerializeField] private Image barFill;
        /// <summary>채움 끝을 따라 움직이는 냄비. 진행 바 영역 안의 자식이어야 한다.</summary>
        [SerializeField] private RectTransform potIcon;
        [SerializeField] private RectTransform barArea;
        /// <summary>실제 진행률을 따라가는 속도(초당 비율). 클수록 바로 따라붙는다.</summary>
        [SerializeField] private float followSpeed = 1.5f;
        [SerializeField] private float potBobHeight = 6f;
        [SerializeField] private float potBobSpeed = 6f;
        /// <summary>불씨 깜빡임 크기 (배율 폭).</summary>
        [SerializeField] private float emberFlicker = 0.15f;
        [SerializeField] private float fullFlashSeconds = 0.45f;
        [SerializeField] private Color fullFlashColor = new Color(1f, 0.95f, 0.75f, 1f);

        [Header("페이드"), SerializeField] private Image fadeOverlay;

        [Header("실패 안내"), SerializeField] private GameObject failurePanel;
        [SerializeField] private TMP_Text failureMessage;
        [SerializeField] private Button failureButton;
        [SerializeField] private TMP_Text failureButtonLabel;

        private float targetProgress;
        private float shownProgress;
        private bool quitInsteadOfTitle;
        private Vector2 potBasePos;
        private Color barBaseColor;
        private float flashT = -1f;

        /// <summary>실패 안내의 [타이틀로]를 눌렀다.</summary>
        public event Action ReturnToTitleClicked;
        /// <summary>실패 안내의 [게임 종료]를 눌렀다 (타이틀도 열 수 없을 때).</summary>
        public event Action QuitClicked;

        /// <summary>화면에 보이는 진행률(0~1). 바가 실제 진행률을 다 따라잡았는지 확인할 때 쓴다.</summary>
        public float ShownProgress => shownProgress;

        private void Awake()
        {
            potBasePos = potIcon.anchoredPosition;
            barBaseColor = barFill.color;
            failurePanel.SetActive(false);
        }

        /// <summary>실패 안내 버튼의 영구 연결 대상. 안내 종류에 맞는 이벤트를 한 번만 보낸다.</summary>
        public void OnFailureButtonClicked()
        {
            if (!failurePanel.activeInHierarchy || !failureButton.interactable)
                return;
            // 씬 전환이 시작되기 전 연타로 같은 요청이 여러 번 가지 않게 잠근다.
            failureButton.interactable = false;
            if (quitInsteadOfTitle) QuitClicked?.Invoke();
            else ReturnToTitleClicked?.Invoke();
        }

        /// <summary>목적지 그림과 팁 한 줄을 보여 주고 진행 바를 0부터 시작한다.</summary>
        public void Show(Sprite picture, string tip) => Show(picture, null, null, tip);

        /// <summary>목적지 그림·제목(글자)·팁 한 줄을 보여 주고 진행 바를 0부터 시작한다.</summary>
        public void Show(Sprite picture, string heading, string tip) => Show(picture, null, heading, tip);

        /// <summary>목적지 그림·제목·팁 한 줄을 보여 주고 진행 바를 0부터 시작한다.
        /// 제목 그림(headingArt)이 있으면 그림을, 없으면 제목 글자를 보여 준다.</summary>
        public void Show(Sprite picture, Sprite headingArt, string heading, string tip)
        {
            bool useArt = headingArt != null && headingImage != null;
            if (headingImage != null)
            {
                headingImage.sprite = headingArt;
                headingImage.gameObject.SetActive(useArt);
                if (useArt)
                    headingImage.rectTransform.sizeDelta = headingArt.rect.size * headingImageScale;
            }
            if (headingText != null)
            {
                headingText.text = heading ?? string.Empty;
                headingText.gameObject.SetActive(!useArt && !string.IsNullOrEmpty(heading));
            }
            illustration.sprite = picture;
            illustration.enabled = picture != null;
            tipText.text = tip ?? string.Empty;
            targetProgress = shownProgress = 0f;
            failurePanel.SetActive(false);
            fadeOverlay.raycastTarget = true;
            ApplyProgress();
        }

        /// <summary>실제 진행률(0~1)을 알려 준다. 줄어드는 값은 무시한다.</summary>
        public void SetProgress(float progress)
        {
            targetProgress = Mathf.Max(targetProgress, Mathf.Clamp01(progress));
        }

        /// <summary>검은 덮개를 alpha까지 seconds 동안 바꾼다. 끝날 때까지 기다리려면 yield return 한다.</summary>
        public IEnumerator FadeTo(float alpha, float seconds)
        {
            float start = fadeOverlay.color.a;
            fadeOverlay.raycastTarget = true; // 페이드 중 클릭 막기
            for (float t = 0f; t < seconds; t += Time.unscaledDeltaTime)
            {
                SetFade(Mathf.Lerp(start, alpha, t / seconds));
                yield return null;
            }
            SetFade(alpha);
            // 로딩 화면이 떠 있는 동안에는 아래에 미리 열린 목적지 씬의 버튼이 눌리지 않게 계속 막는다.
            fadeOverlay.raycastTarget = true;
            // 목표 투명도를 한 프레임 실제로 그린 뒤 끝낸다. 바로 끝내면 호출자(UIManager)가 같은 프레임에 로딩을 닫아
            // 완전히 검은 화면이 한 번도 보이지 않고 0.99 상태에서 넘어간다 (Loading 페이드 테스트 간헐 실패 원인)
            yield return null;
        }

        /// <summary>바로 덮개 투명도를 정한다 (화면이 처음 켜질 때 검은 상태로 시작하기 등).</summary>
        public void SetFade(float alpha)
        {
            var c = fadeOverlay.color;
            c.a = alpha;
            fadeOverlay.color = c;
        }

        /// <summary>실패 안내를 띄운다. canReturnToTitle이 false면 [게임 종료]를 보여 준다.</summary>
        public void ShowFailure(bool canReturnToTitle)
        {
            quitInsteadOfTitle = !canReturnToTitle;
            failureMessage.text = canReturnToTitle
                ? "불러오지 못했어요.\n<size=75%>타이틀로 돌아가 다시 시도해 주세요.</size>"
                : "불러오지 못했어요.\n<size=75%>게임을 다시 실행해 주세요.</size>";
            failureButtonLabel.text = canReturnToTitle ? "타이틀로" : "게임 종료";
            failureButton.interactable = true;
            failurePanel.SetActive(true);
            // 실패 안내는 검은 덮개 위에 떠야 하므로 덮개가 클릭을 막지 않게 한다.
            fadeOverlay.raycastTarget = false;
            // 키보드 Enter로도 누를 수 있게 선택한다. EventSystem이 없으면 Unity가 무시한다.
            failureButton.Select();
        }

        private void Update()
        {
            // 실제 진행률을 부드럽게 따라가기 (로딩이 순간적으로 끝나도 바가 차오르는 게 보이게)
            float before = shownProgress;
            shownProgress = Mathf.MoveTowards(shownProgress, targetProgress, followSpeed * Time.unscaledDeltaTime);
            if (before < 1f && shownProgress >= 1f) flashT = 0f; // 다 찬 순간 한 번 반짝
            if (flashT >= 0f) { flashT += Time.unscaledDeltaTime; if (flashT >= fullFlashSeconds) flashT = -1f; }
            ApplyProgress();
        }

        private void ApplyProgress()
        {
            // 채움 그림을 자르지 않고 길이만 늘여서 양끝 둥근 모양을 지킨다
        var fillRect = barFill.rectTransform;
        fillRect.anchorMin = new Vector2(0f, 0f);
        fillRect.anchorMax = new Vector2(shownProgress, 1f);
        fillRect.offsetMin = fillRect.offsetMax = Vector2.zero;
        barFill.enabled = shownProgress > 0.01f;
            // 냄비는 채워진 끝 위에, 살짝 들썩이며
            float width = barArea.rect.width;
            float bob = Mathf.Abs(Mathf.Sin(Time.unscaledTime * potBobSpeed)) * potBobHeight;
            potIcon.anchoredPosition = new Vector2(potBasePos.x + width * shownProgress, potBasePos.y + bob);
            // 불씨가 일렁이듯 크기가 조금씩 흔들린다 (두 사인을 섞어 규칙적으로 안 보이게)
            float flicker = Mathf.Sin(Time.unscaledTime * 13f) * 0.6f + Mathf.Sin(Time.unscaledTime * 29f) * 0.4f;
            potIcon.localScale = Vector3.one * (1f + emberFlicker * flicker);
            float flash = flashT >= 0f ? Mathf.Sin(Mathf.PI * flashT / fullFlashSeconds) : 0f;
            barFill.color = Color.Lerp(barBaseColor, fullFlashColor, flash);
        }
    }
}
