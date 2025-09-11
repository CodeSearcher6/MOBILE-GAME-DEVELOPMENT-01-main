using UnityEngine;
using Dreamteck.Forever;
using Game.Animation;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Runner runner;
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationManagerSO animationSO;

        [Header("Movement Settings")]
        [SerializeField] private float joystickSpeed = 20f;

        [SerializeField] private float smoothFactor = 10f;
        [SerializeField] private float maxOffset = 5f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity = -9.8f;


        private PlayerMovementManager movement;
        private PlayerAnimationManager playerAnimator;
        private InputController input;

        private Vector2 currentInput;

        private void Awake()
        {
            movement = new PlayerMovementManager(
             runner, joystickSpeed, maxOffset, jumpForce, gravity, smoothFactor
          );

            playerAnimator = new PlayerAnimationManager(animator, animationSO);

            input = new InputController();
            input.SubscribeEvents();

            input.MovementRecieved += OnMovementReceived;
            input.MovementEnded += OnMovementEnded;
            input.JumpPerformed += OnJumpPerformed;
        }

        private bool wasJumping = false;

        private void Update()
        {
            movement.Move(currentInput);
            playerAnimator.UpdateRunning(movement.IsRunning);

            if (movement.IsJumping && !wasJumping)
            {
                playerAnimator.StartJump();
                wasJumping = true;
                Debug.Log("Jump started");
            }

            if (!movement.IsJumping && wasJumping)
            {
                playerAnimator.EndJump();
                wasJumping = false;
                Debug.Log("Jump ended");
            }
        }


        private void OnMovementReceived(Vector2 dir)
        {
            currentInput = Vector2.ClampMagnitude(dir, 1f);
            Debug.Log($"Movement input: {currentInput}");
        }

        private void OnMovementEnded() => currentInput = Vector2.zero;
        private void OnJumpPerformed() => movement.Jump();

        private void OnDestroy()
        {
            input.Dispose();
        }
    }
}
