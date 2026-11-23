// 영업 입력: 프로젝트의 Player/Move와 Player/Interact를 읽어 공용 손님 서비스에 전달한다.
// 원본 입력 자산과 UI는 전역 액션 수명을 공유하므로 이 어댑터는 액션을 직접 끄지 않는다.
using DOB.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DOB.Input
{
    /// <summary>프로젝트의 기존 WASD 이동과 E 상호작용 바인딩을 사용한다.</summary>
    public sealed class BasementPlayerInput : FirstGuestControls
    {
        private InputAction move;
        private InputAction interact;

        private void OnEnable()
        {
            var actions = InputSystem.actions;
            move = actions?.FindAction("Player/Move");
            interact = actions?.FindAction("Player/Interact");
            if (move == null || interact == null)
                Debug.LogError("[DOB] 프로젝트 전역 입력 자산에 Player/Move와 Player/Interact가 필요합니다.", this);
        }

        /// <inheritdoc/>
        public override Vector2 Move => isActiveAndEnabled && move != null ? move.ReadValue<Vector2>() : Vector2.zero;
        /// <inheritdoc/>
        public override bool InteractPressed => isActiveAndEnabled && interact != null && interact.WasPressedThisFrame();
    }
}
