// ESC는 게임 베이스의 조작과 별도로 UI 씬에서 읽으며, 세계 입력보다 먼저 일시정지를 적용한다.
// Esc 순서 (홍준우 2026-11-24): ESC 메뉴가 열려 있으면 메뉴가 받는다 → 준비 · 요리 화면이 열려 있으면 그 화면이 닫히도록 넘긴다
//   → 아무것도 없으면 ESC 메뉴를 연다. (화면 쪽 Presenter가 같은 프레임에 스스로 닫는다)
using DOB.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DOB.Input
{
    [DefaultExecutionOrder(-150), RequireComponent(typeof(BasementPauseMenu))]
    public sealed class BasementPauseInput : MonoBehaviour
    {
        private BasementPauseMenu menu;

        private void Awake() => menu = GetComponent<BasementPauseMenu>();

        private void Update()
        {
            if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame)
                return;
            if (!BasementPauseMenu.IsPaused && UIManager.IsScreenOpen)
                return;
            menu.HandleEscape();
        }
    }
}
