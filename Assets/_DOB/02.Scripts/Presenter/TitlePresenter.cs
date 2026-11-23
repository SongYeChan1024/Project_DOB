// =====================================================================
// TitlePresenter — 타이틀 화면과 씬 전환을 잇는다
// 역할   : TitleView의 입력을 받아 저장 여부 확인, 새로 시작 확인, 씬 전환 요청, 종료를 처리한다.
// 호출자 : TitleView 이벤트 (NewGameClicked, NewGameConfirmed, ContinueClicked, QuitClicked)
// 호출   : 일차 저장소, BasementComposition.EnterDay, TitleView 표시 메서드
// 상태   : 전환 요청을 보냈는지(isRequesting)만 가진다. 저장·세션 상태는 갖지 않는다.
// =====================================================================
using System;
using System.IO;
using DOB.Systems;
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>타이틀 메뉴 입력을 씬 전환 요청으로 옮긴다. 게임 규칙·저장 내용은 계산하지 않는다.</summary>
    public sealed class TitlePresenter : MonoBehaviour
    {
        /// <summary>연결할 타이틀 화면.</summary>
        [SerializeField] private TitleView view;

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
            view.ShowMenu(false);
            if (TryReadLatest(out var saved))
                view.ShowMenu(saved != null, saved == null ? null : BasementPauseMenu.Format(saved));
        }

        private void OnNewGameClicked()
        {
            if (isRequesting || !TryReadLatest(out var saved))
                return;
            if (saved != null)
                view.ShowConfirm(true);
            else
                EnterDay(null);
        }

        private void OnNewGameConfirmed()
        {
            view.ShowConfirm(false);
            EnterDay(null);
        }

        private void OnContinueClicked()
        {
            if (isRequesting || !TryReadLatest(out var saved))
                return;
            if (saved == null)
            {
                view.ShowMenu(false);
                view.ShowLoadFailed();
                return;
            }
            EnterDay(saved);
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

        private void EnterDay(FirstServiceSaveData checkpoint)
        {
            if (isRequesting || SceneLoadService.IsLoading)
                return;

            isRequesting = true;
            view.SetBusy(true);
            if (!BasementComposition.EnterDay(checkpoint))
            {
                isRequesting = false;
                view.SetBusy(false);
                view.ShowLoadFailed();
            }
        }

        // 표시와 실제 클릭 사이에 바뀐 슬롯도 반영한다. 저장이 없을 때는 새 게임을 열 수 있다.
        private bool TryReadLatest(out FirstServiceSaveData saved)
        {
            try
            {
                BasementComposition.OpenSaves().TryLoadLatest(out saved);
                return true;
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                saved = null;
                Debug.LogWarning("[DOB] 타이틀에서 저장 기록을 읽을 수 없습니다. " + e.Message, this);
                view.ShowLoadFailed();
                return false;
            }
        }
    }
}
