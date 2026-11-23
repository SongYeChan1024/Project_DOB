// 타이틀에서 최근에 저장한 일차를 표시한다. 새 게임 또는 이어하기를 선택하면 로딩 화면으로 진입한다.
using DOB.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.Manager
{
    public sealed class BasementTitleMenu : MonoBehaviour
    {
        [SerializeField] private Text lastSave;
        [SerializeField] private Button continueButton;

        private void OnEnable() => Refresh();

        private void Refresh()
        {
            continueButton.interactable = false;
            lastSave.text = "저장된 영업이 없어요.";
            BasementPauseMenu.TryIo(() =>
            {
                if (!BasementComposition.OpenSaves().TryLoadLatest(out var saved)) return;
                lastSave.text = BasementPauseMenu.Format(saved);
                continueButton.interactable = true;
            }, error => lastSave.text = error);
        }

        public void NewGame()
        {
            if (!BasementComposition.EnterDay(null))
                lastSave.text = "게임 씬을 열 수 없어요.";
        }

        public void ContinueGame()
        {
            BasementPauseMenu.TryIo(() =>
            {
                if (!BasementComposition.OpenSaves().TryLoadLatest(out FirstServiceSaveData saved))
                {
                    Refresh();
                    return;
                }
                if (!BasementComposition.EnterDay(saved))
                    lastSave.text = "저장한 일차의 씬을 열 수 없어요.";
            }, error => lastSave.text = error);
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
