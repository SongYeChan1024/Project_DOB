// =====================================================================
// TitlePresenter — 타이틀 화면과 씬 전환을 잇는다
// 역할   : TitleView의 입력을 받아 저장 여부 확인, 새로 시작 확인, 씬 전환 요청, 종료를 처리한다.
// 호출자 : TitleView 이벤트 (NewGameClicked, NewGameConfirmed, ContinueClicked, QuitClicked)
// 호출   : SceneLoadService.RequestLoad, TitleView 표시 메서드
// 상태   : 전환 요청을 보냈는지(isRequesting)만 가진다. 저장·세션 상태는 갖지 않는다.
// =====================================================================
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>타이틀 메뉴 입력을 씬 전환 요청으로 옮긴다. 게임 규칙·저장 내용은 계산하지 않는다.</summary>
    public sealed class TitlePresenter : MonoBehaviour
    {
        /// <summary>연결할 타이틀 화면.</summary>
        [SerializeField] private TitleView view;

        /// <summary>새로 시작의 진입 경로. 오프닝 스토리 위치가 정해지면 바뀐다.</summary>
        [SerializeField] private string newGameDestination = "Tutorial";

        /// <summary>이어하기의 진입 경로. 저장 계약이 정해지면 저장 데이터 기준으로 바뀐다.</summary>
        [SerializeField] private string continueDestination = "Restaurant";

        private bool isRequesting;

        private void OnEnable()
        {
            view.NewGameClicked += OnNewGameClicked;
            view.NewGameConfirmed += OnNewGameConfirmed;
            view.ContinueClicked += OnContinueClicked;
            view.QuitClicked += OnQuitClicked;
        }

        private void OnDisable()
        {
            view.NewGameClicked -= OnNewGameClicked;
            view.NewGameConfirmed -= OnNewGameConfirmed;
            view.ContinueClicked -= OnContinueClicked;
            view.QuitClicked -= OnQuitClicked;
        }

        private void Start()
        {
            view.ShowMenu(HasSaveData());
        }

        private void OnNewGameClicked()
        {
            // 저장이 있으면 기록이 사라지므로 한 번 확인받는다.
            if (HasSaveData())
                view.ShowConfirm(true);
            else
                RequestLoad(newGameDestination);
        }

        private void OnNewGameConfirmed()
        {
            view.ShowConfirm(false);
            RequestLoad(newGameDestination);
        }

        private void OnContinueClicked()
        {
            RequestLoad(continueDestination);
        }

        private void OnQuitClicked()
        {
            if (isRequesting)
                return;
#if UNITY_EDITOR
            // 에디터에서는 Application.Quit이 동작하지 않아 플레이 모드를 멈춘다.
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void RequestLoad(string destination)
        {
            if (isRequesting || SceneLoadService.IsLoading)
                return;

            isRequesting = true;
            view.SetBusy(true);
            SceneLoadService.RequestLoad(destination);

            // RequestLoad는 결과를 돌려주지 않는다. 받아들여지면 IsLoading이 바로 참이 되므로,
            // 거짓이면 사전 검사(경로·빌드 등록)에서 거절된 것으로 보고 메뉴를 되돌린다.
            if (!SceneLoadService.IsLoading)
            {
                isRequesting = false;
                view.SetBusy(false);
                view.ShowLoadFailed();
            }
        }

        // 저장 기능은 아직 없다. 김언호와 저장 계약을 정하면 백엔드 요청으로 바꾼다.
        private bool HasSaveData()
        {
            return false;
        }
    }
}
