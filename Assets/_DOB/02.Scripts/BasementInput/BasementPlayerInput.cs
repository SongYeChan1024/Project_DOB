// 프리팹의 PlayerInput이 활성화한 Player 맵에서 이동과 상호작용을 읽는다.
using DOB.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DOB.Input
{
    /// <summary>프로젝트의 기존 WASD 이동과 E 상호작용 바인딩을 사용한다.</summary>
    [DisallowMultipleComponent, RequireComponent(typeof(PlayerInput))]
    public sealed class BasementPlayerInput : FirstGuestControls
    {
        private PlayerInput playerInput;
        private InputAction move;
        private InputAction interact;

        private void OnEnable()
        {
            playerInput = GetComponent<PlayerInput>();
            var actions = playerInput.actions;
            move = actions?.FindAction("Player/Move");
            interact = actions?.FindAction("Player/Interact");
            if (move == null || interact == null)
                Debug.LogError("[DOB] 플레이어의 PlayerInput에 Player/Move와 Player/Interact를 연결하세요.", this);
        }

        /// <inheritdoc/>
        public override Vector2 Move => isActiveAndEnabled && playerInput.inputIsActive && move != null ? move.ReadValue<Vector2>() : Vector2.zero;
        /// <inheritdoc/>
        public override bool InteractPressed => isActiveAndEnabled && playerInput.inputIsActive && interact != null && interact.WasPressedThisFrame();
    }
}
