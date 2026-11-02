// =====================================================================
// TitleView — 타이틀 화면 표시와 입력
// 역할   : 메뉴 버튼·로딩 표시·새로 시작 확인창·설정 창·실패 안내를 보여 주고,
//          버튼 입력을 이벤트로 알린다. 씬 이동·저장 확인은 하지 않는다.
// 호출자 : TitlePresenter (메뉴 구성, 잠금, 확인창, 실패 안내 요청)
// 호출   : 없음 (이벤트로만 알림)
// 상태   : 화면에 보이는 것만 가진다. 게임 상태는 갖지 않는다.
// =====================================================================
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>타이틀 메뉴를 표시하고 입력을 이벤트로 보낸다. 씬 전환과 저장 판단은 Presenter가 한다.</summary>
    public sealed class TitleView : MonoBehaviour
    {
        /// <summary>새로 시작 버튼.</summary>
        [Header("메뉴"), SerializeField] private Button newGameButton;
        /// <summary>이어하기 버튼. 저장이 없으면 숨긴다.</summary>
        [SerializeField] private Button continueButton;
        /// <summary>설정 창을 여는 버튼.</summary>
        [SerializeField] private Button settingsButton;
        /// <summary>게임 종료 버튼.</summary>
        [SerializeField] private Button quitButton;

        /// <summary>씬 전환 요청 뒤 보이는 "불러오는 중" 표시.</summary>
        [Header("상태 표시"), SerializeField] private GameObject loadingIndicator;
        /// <summary>씬 전환 요청이 거절됐을 때 보이는 안내.</summary>
        [SerializeField] private GameObject loadFailedMessage;

        /// <summary>저장이 있을 때 새로 시작 전에 띄우는 확인창.</summary>
        [Header("새로 시작 확인창"), SerializeField] private GameObject confirmPopup;
        [SerializeField] private Button confirmYesButton;
        [SerializeField] private Button confirmNoButton;

        /// <summary>설정 창. 지금은 열고 닫기만 한다(설정 항목은 07·08번 작업).</summary>
        [Header("설정 창"), SerializeField] private GameObject settingsPanel;
        [SerializeField] private Button settingsCloseButton;

        /// <summary>새로 시작을 눌렀다.</summary>
        public event Action NewGameClicked;
        /// <summary>확인창에서 새로 시작을 확정했다.</summary>
        public event Action NewGameConfirmed;
        /// <summary>이어하기를 눌렀다.</summary>
        public event Action ContinueClicked;
        /// <summary>나가기를 눌렀다.</summary>
        public event Action QuitClicked;

        private void Awake()
        {
            newGameButton.onClick.AddListener(() => NewGameClicked?.Invoke());
            continueButton.onClick.AddListener(() => ContinueClicked?.Invoke());
            quitButton.onClick.AddListener(() => QuitClicked?.Invoke());
            confirmYesButton.onClick.AddListener(() => NewGameConfirmed?.Invoke());
            confirmNoButton.onClick.AddListener(() => ShowConfirm(false));

            // 설정 창 열고 닫기는 게임 상태와 무관한 화면 동작이라 View 안에서 처리한다.
            settingsButton.onClick.AddListener(() => settingsPanel.SetActive(true));
            settingsCloseButton.onClick.AddListener(() => settingsPanel.SetActive(false));

            loadingIndicator.SetActive(false);
            loadFailedMessage.SetActive(false);
            confirmPopup.SetActive(false);
            settingsPanel.SetActive(false);
        }

        /// <summary>저장 여부에 맞춰 메뉴를 구성한다. 저장이 없으면 이어하기를 숨긴다.</summary>
        public void ShowMenu(bool hasSave)
        {
            continueButton.gameObject.SetActive(hasSave);
            SetBusy(false);
        }

        /// <summary>전환 요청 중이면 모든 버튼을 잠그고 로딩 표시를 켠다.</summary>
        public void SetBusy(bool busy)
        {
            newGameButton.interactable = !busy;
            continueButton.interactable = !busy;
            settingsButton.interactable = !busy;
            quitButton.interactable = !busy;
            loadingIndicator.SetActive(busy);
            if (busy)
                loadFailedMessage.SetActive(false);
        }

        /// <summary>새로 시작 확인창을 열거나 닫는다.</summary>
        public void ShowConfirm(bool show)
        {
            confirmPopup.SetActive(show);
        }

        /// <summary>전환 요청이 거절됐음을 알린다. 잠금은 Presenter가 SetBusy(false)로 푼다.</summary>
        public void ShowLoadFailed()
        {
            loadFailedMessage.SetActive(true);
        }
    }
}
