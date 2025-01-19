#nullable enable

using Core;
using UnityEngine;

namespace Kumorikuma.Gameplay {
    /*Simple player movement controller, based on character controller component,
    with footstep system based on check the current texture of the component*/
    public class PlayerController : ManagedMonoBehaviour {
        public Animator? Animator;
        [NonNullField] public GameObject PlayerModel;

        //Private movement variables
        private Vector3 _Velocity; // Used for handling jumping
        private CharacterController _CharacterController = null!;
        Quaternion _TargetRotation = Quaternion.identity;
        
        private PlayerInputManager _PlayerInputManager = null!;
        private GameConfig _GameConfig = null!;

        private void Awake() {
            _PlayerInputManager = Main.Instance.PlayerInputManager;
            _GameConfig = Main.Instance.GameConfig;
            
            _CharacterController = GetComponent<CharacterController>();
            _Velocity.y = -2f;
        }
        
        public override void Process() {
            // Initiate jump.
            bool jumpPressed = _PlayerInputManager.Inputs.JumpInputAction.WasPerformedThisFrame();
            if (jumpPressed && _CharacterController.isGrounded) {
                _Velocity.y = Mathf.Sqrt( _GameConfig.JumpForce * -2f * _GameConfig.Gravity);
            }

            Vector3 inputMoveVector = _PlayerInputManager.Inputs.MoveVector3d;
            
            // TODO: Handle camera moving

            // Using KBM controls, there's a specific button to walk.
            // Otherwise, use the amount that user is pushing stick.
            bool isWalkKeyHeld = _PlayerInputManager.Inputs.WalkInputAction.WasPressedThisFrame();
            bool isWalking = isWalkKeyHeld || inputMoveVector.magnitude < 0.5f;
            float moveSpeed = isWalking ? _GameConfig.WalkSpeed : _GameConfig.RunSpeed;
            // Character should move in the direction of the camera
            Vector3 worldMoveDirection = inputMoveVector;
            // Y component should be 0
            worldMoveDirection.y = 0;
            worldMoveDirection = worldMoveDirection.normalized;
            Vector3 absoluteMoveVector = worldMoveDirection * (moveSpeed * Time.deltaTime);
            bool isMoving = absoluteMoveVector.magnitude > 0;
            float forwardSpeed = isMoving ? moveSpeed : 0;
            if (isMoving) {
                // Face the character in the direction of movement
                _TargetRotation = Quaternion.Euler(0,
                    Mathf.Atan2(worldMoveDirection.x, worldMoveDirection.z) * Mathf.Rad2Deg, 0);
            }

            if (Animator != null) {
                Animator.SetBool("IsRunning", isMoving);
                Animator.SetBool("IsMovingForward", isMoving);
                Animator.SetBool("IsMovingBackward", false);
                Animator.SetBool("IsMovingLeft", false);
                Animator.SetBool("IsMovingRight", false);

                // Animator.SetFloat("HasHorizontalMovement", worldMoveDirection.x);
                Animator.SetFloat("MoveX", worldMoveDirection.x);
                Animator.SetFloat("MoveZ", worldMoveDirection.z);

                // Animator.SetBool("IsMovingForward", worldMoveDirection.z > 0);
                // Animator.SetBool("IsMovingBackward", worldMoveDirection.z < 0);
                // Animator.SetBool("IsMovingLeft", worldMoveDirection.x < 0);
                // Animator.SetBool("IsMovingRight", worldMoveDirection.x > 0);
            }

            // Turn the player incrementally towards the direction of movement
            PlayerModel.transform.rotation = Quaternion.RotateTowards(PlayerModel.transform.rotation, _TargetRotation,
                _GameConfig.TurnSpeed * Time.deltaTime);

            // CharacterController.Move should only be called once, see:
            // https://forum.unity.com/threads/charactercontroller-isgrounded-unreliable-or-bad-code.373492/
            _CharacterController.Move(_Velocity * Time.deltaTime + absoluteMoveVector);

            // Update velocity from gravity
            _Velocity.y += _GameConfig.Gravity * Time.deltaTime;
        }
    }
}
