using System;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>플레이어 프리팹의 이동 입력을 카메라 기준 평면 이동과 회전으로 적용한다.</summary>
    [DisallowMultipleComponent, RequireComponent(typeof(CharacterController))]
    [DefaultExecutionOrder(10)]
    public sealed class PlayerMovementController : MonoBehaviour
    {
        [Min(0.1f), SerializeField] private float moveSpeed = 3.5f;

        private CharacterController body;
        private FirstGuestControls controls;
        private Camera movementCamera;
        private RestaurantLayout floor;
        private Func<bool> canMove;

        private void Awake()
        {
            body = GetComponent<CharacterController>();
            controls = GetComponent<FirstGuestControls>();
            if (!controls)
            {
                Debug.LogError("[DOB] 플레이어 루트에 이동 입력을 연결하세요.", this);
                enabled = false;
            }
        }

        public void EnterRestaurant(Camera camera, RestaurantLayout layout, Func<bool> allowMovement)
        {
            movementCamera = camera;
            floor = layout;
            canMove = allowMovement;
            enabled = true;
        }

        public void LeaveRestaurant()
        {
            enabled = false;
            movementCamera = null;
            floor = null;
            canMove = null;
        }

        private void Update()
        {
            if (Time.deltaTime <= 0f || !body.enabled || (canMove != null && !canMove()))
                return;
            var input = controls.Move;
            if (input.sqrMagnitude == 0f)
                return;

            var camera = movementCamera ? movementCamera : Camera.main;
            var forward = camera ? Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up) : Vector3.forward;
            if (forward.sqrMagnitude < 0.0001f)
                forward = Vector3.ProjectOnPlane(camera.transform.up, Vector3.up);
            forward.Normalize();
            var right = Vector3.Cross(Vector3.up, forward);
            var direction = Vector3.ClampMagnitude(right * input.x + forward * input.y, 1f);
            var target = transform.position + direction * (moveSpeed * Time.deltaTime);
            if (floor)
                target = floor.ClampToFloor(target, body.radius);
            body.Move(target - transform.position);
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
