// =====================================================================
// LoadingScreenPresenter — 로딩 화면 데이터와 화면 표시를 잇는다
// 역할   : 목적지 이름으로 LoadingScreenContent에서 그림·제목·팁을 골라 LoadingScreenView에 넘기고,
//          진행률·페이드·실패 안내 요청을 화면에 전달한다. 실패 버튼 응답은 UIManager에 알린다.
// 호출자 : UIManager (Show, SetProgress, FadeIn, FadeOut, ShowFailure, HasCaughtUp)
// 호출   : LoadingScreenView, LoadingScreenContent, UIManager (등록·해제·실패 응답)
// 상태   : 없음. 씬 로드·게임 진행·저장은 하지 않는다.
//
// 의사코드
// [켜질 때]  화면의 실패 버튼 이벤트를 구독하고 UIManager에 자신을 등록한다
// [꺼질 때]  구독을 해제하고 등록을 푼다 (Loading 씬이 언로드되면 자동으로 꺼진다)
// [보이기]   목적지의 그림·제목 그림·제목·무작위 팁 한 줄을 보여 주고 검은 화면에서 시작한다
// [실패]     검은 덮개를 걷고 실패 안내를 띄운다
// =====================================================================
using System.Collections;
using DOB.Define;
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>로딩 화면 내용과 진행 표시를 연결한다. 로드 요청이나 게임 상태를 변경하지 않는다.</summary>
    public sealed class LoadingScreenPresenter : MonoBehaviour
    {
        /// <summary>같은 Loading 씬에 배치한 로딩 화면(PF_LoadingScreen).</summary>
        [SerializeField] private LoadingScreenView view;

        /// <summary>목적지별 그림·팁과 최소 표시·페이드 시간 데이터.</summary>
        [SerializeField] private LoadingScreenContent content;

        // 진행 바가 이 값 이상이면 끝까지 찬 것으로 본다. 부동소수 오차를 감안한 값이다.
        private const float CaughtUpThreshold = 0.999f;

        /// <summary>화면이 완전히 보인 뒤 최소한 머무는 시간(초).</summary>
        public float MinimumDisplaySeconds => content ? content.MinimumDisplaySeconds : 0f;

        /// <summary>진행 바가 끝까지 차 보이는지.</summary>
        public bool HasCaughtUp => !view || view.ShownProgress >= CaughtUpThreshold;

        private float FadeSeconds => content ? content.FadeSeconds : 0f;

        /// <summary>에디터 씬 작성 과정에서 화면과 데이터를 연결한다.</summary>
        public void Configure(LoadingScreenView screen, LoadingScreenContent data)
        {
            view = screen;
            content = data;
        }

        private void OnEnable()
        {
            if (view)
            {
                view.ReturnToTitleClicked += UIManager.HandleLoadingReturnToTitle;
                view.QuitClicked += UIManager.HandleLoadingQuit;
            }
            UIManager.RegisterLoadingScreen(this);
        }

        private void OnDisable()
        {
            if (view)
            {
                view.ReturnToTitleClicked -= UIManager.HandleLoadingReturnToTitle;
                view.QuitClicked -= UIManager.HandleLoadingQuit;
            }
            UIManager.UnregisterLoadingScreen(this);
        }

        /// <summary>목적지의 그림·제목·팁을 보이고 검은 화면에서 시작한다.</summary>
        public void Show(string destination)
        {
            if (!view)
                return;
            if (content)
                view.Show(content.GetIllustration(destination), content.GetHeadingImage(destination),
                    content.GetHeading(destination), content.GetRandomTip(destination));
            else
                view.Show(null, null, null, null);
            view.SetFade(1f);
        }

        /// <summary>실제 진행률(0~1)을 화면에 전달한다.</summary>
        public void SetProgress(float progress)
        {
            if (view) view.SetProgress(progress);
        }

        /// <summary>검은 화면에서 로딩 화면이 나타난다. 끝날 때까지 기다리려면 yield return 한다.</summary>
        public IEnumerator FadeIn() => Fade(0f);

        /// <summary>로딩 화면이 검게 사라진다. 끝날 때까지 기다리려면 yield return 한다.</summary>
        public IEnumerator FadeOut() => Fade(1f);

        /// <summary>실패 안내를 띄운다. canReturnToTitle이 false면 [게임 종료]를 보인다.</summary>
        public void ShowFailure(bool canReturnToTitle)
        {
            if (!view)
                return;
            // 페이드 도중 실패해도 안내가 검은 덮개에 가리지 않게 덮개를 걷는다.
            view.SetFade(0f);
            view.ShowFailure(canReturnToTitle);
        }

        private IEnumerator Fade(float alpha)
        {
            if (view)
                yield return view.FadeTo(alpha, FadeSeconds);
        }
    }
}
