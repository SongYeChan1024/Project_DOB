// ESC는 게임 베이스의 조작과 별도로 UI 씬에서 읽으며, 세계 입력보다 먼저 일시정지를 적용한다.
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
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                menu.HandleEscape();
        }
    }
}
