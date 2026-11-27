// =====================================================================
// TitlePresenter — 타이틀 화면과 씬 전환을 잇는다
// 역할   : TitleView의 입력을 받아 저장 여부 확인, 새로 시작 확인, 씬 전환 요청, 종료를 처리한다.
// 호출자 : TitleView 이벤트 (NewGameClicked, NewGameConfirmed, ContinueClicked, DemoReplayClicked, DemoReplayConfirmed, QuitClicked)
// 호출   : 일차 저장소, BasementComposition.EnterDay, TitleView 표시 메서드 (전환 전 FadeOutThen으로 화면을 어둡게)
// 상태   : 전환 요청을 보냈는지(isRequesting)만 가진다. 저장·세션 상태는 갖지 않는다.
//
// 「엔딩 날 다시 하기」(시안 24, #57 데모 완료 슬롯)
//   [시작]   데모 완료 슬롯이 있으면 메뉴를 보인다 (일차 · 저장 날짜)
//   [누름]   슬롯을 다시 읽어 있으면 확인창, 없으면 메뉴를 숨기고 실패 안내
//   [확정]   슬롯을 다시 읽어 그 데이터로 EnterDay. 「그 하루만 하고 끝」은 GameManager가 슬롯을 보고 처리한다
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
            view.DemoReplayClicked += OnDemoReplayClicked;
            view.DemoReplayConfirmed += OnDemoReplayConfirmed;
            view.QuitClicked += OnQuitClicked;
        }

        private void OnDisable()
        {
            view.NewGameClicked -= OnNewGameClicked;
            view.NewGameConfirmed -= OnNewGameConfirmed;
            view.ContinueClicked -= OnContinueClicked;
            view.DemoReplayClicked -= OnDemoReplayClicked;
            view.DemoReplayConfirmed -= OnDemoReplayConfirmed;
            view.QuitClicked -= OnQuitClicked;
        }

        private void Start()
        {
            view.ShowMenu(false);
            view.ShowDemoReplay(null);
            if (TryReadLatest(out var saved))
                view.ShowMenu(saved != null, saved == null ? null : ShortSaveLine(saved));
            if (TryReadDemo(out var demo) && demo != null)
                view.ShowDemoReplay(ShortSaveLine(demo));
        }

        private void OnDemoReplayClicked()
        {
            if (isRequesting || !TryReadDemo(out var demo))
                return;
            if (demo == null)
            {
                view.ShowDemoReplay(null);
                view.ShowLoadFailed();
                return;
            }
            view.ShowDemoConfirm(true, demo.Day);
        }

        private void OnDemoReplayConfirmed()
        {
            view.ShowDemoConfirm(false);
            if (isRequesting || !TryReadDemo(out var demo))
                return;
            if (demo == null)
            {
                view.ShowDemoReplay(null);
                view.ShowLoadFailed();
                return;
            }
            EnterDay(demo);
        }

        // 타이틀은 짧게 「n일차 · yy.MM.dd」만 보인다. ESC 불러오기 목록은 시각까지 보이는 BasementPauseMenu.Format을 그대로 쓴다.
        private static string ShortSaveLine(FirstServiceSaveData saved) =>
            $"{saved.Day}일차 · {saved.SavedAtUtc.ToLocalTime():yy.MM.dd}";

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
            // 화면이 어두워진 뒤 전환을 요청한다. 실패하면 잠금 · 덮개를 풀고 안내한다
            view.FadeOutThen(() =>
            {
                if (BasementComposition.EnterDay(checkpoint)) return;
                isRequesting = false;
                view.SetBusy(false);
                view.ShowLoadFailed();
            });
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

        // 데모 완료 슬롯. 없으면 saved = null로 true (메뉴만 숨김), 읽기 오류면 false.
        private bool TryReadDemo(out FirstServiceSaveData saved)
        {
            try
            {
                if (!BasementComposition.OpenSaves().TryLoad(FirstServiceSaveSlot.DemoComplete, out saved))
                    saved = null;
                return true;
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                saved = null;
                Debug.LogWarning("[DOB] 타이틀에서 데모 완료 기록을 읽을 수 없습니다. " + e.Message, this);
                return false;
            }
        }
    }
}
