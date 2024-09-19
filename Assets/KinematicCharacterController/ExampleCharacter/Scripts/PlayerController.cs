using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;

namespace KinematicCharacterController.Examples
{
    public class PlayerController : MonoBehaviour
    {
        public ExampleCharacterController Character;
        public ExampleCharacterCamera CharacterCamera;
        public Animator animator; // Компонент Animator для управления анимациями

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private void Start()
        {
            // Инициализация камеры
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Обработка атаки
              
            }

            HandleCharacterInput();
            HandleAnimations();
        }

        private void LateUpdate()
        {
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection =
                    Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation *
                    CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3
                    .ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            float mouseLookAxisUp = 0;
            float mouseLookAxisRight = 0;
            if (Input.GetMouseButton(1))
            {
                mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
                mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
            }


            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
            scrollInput = 0f;
#endif

            CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs
            {
                MoveAxisForward = Input.GetAxisRaw(VerticalInput),
                MoveAxisRight = Input.GetAxisRaw(HorizontalInput),
                CameraRotation = CharacterCamera.Transform.rotation,
                JumpDown = Input.GetKeyDown(KeyCode.Space),
                CrouchDown = Input.GetKeyDown(KeyCode.C),
                CrouchUp = Input.GetKeyUp(KeyCode.C)
            };

            Character.SetInputs(ref characterInputs);
        }

        private void HandleAnimations()
        {
            float moveForward = Input.GetAxisRaw(VerticalInput);
            float moveRight = Input.GetAxisRaw(HorizontalInput);

            bool isMoving = moveForward != 0 || moveRight != 0;

            // Если игрок двигается, переключаемся на анимацию бега (Stay = 1)
            if (isMoving)
            {
                animator.SetInteger("Stay", 1);
            }
            // Если игрок не двигается, переключаемся на анимацию idle (Stay = 0)
            else
            {
                animator.SetInteger("Stay", 0);
            }
        }
    }
}