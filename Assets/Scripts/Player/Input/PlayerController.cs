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
        [SerializeField] private PlayerHealth health;

        [Header("Movement Settings")]
        [SerializeField] private float laneChangeSpeed = 10f;
        [SerializeField] private float laneOffset = 1f;
        private bool laneChangedThisFrame = false;
        private float previousInputX = 0f;


        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity = -9.8f;


        private PlayerMovementManager movement;
        private PlayerStrafeManager strafeManager;

        private PlayerAnimationManager playerAnimator;
        private InputController input;

        private Vector2 currentInput;

        private void Awake()
        {
            movement = new PlayerMovementManager(
                runner, laneOffset, laneChangeSpeed, jumpForce, gravity
            );

            var health = GetComponent<PlayerHealth>();
            health.OnDied += () =>
            {
                enabled = false;
                playerAnimator.SetFalling(true);
                playerAnimator.SetAlive(false);
            };

            health.OnRevived += () =>
            {
                enabled = true;
                playerAnimator.SetFalling(false);
                playerAnimator.SetAlive(true);
            };

            playerAnimator = new PlayerAnimationManager(animator, animationSO);
            strafeManager = new PlayerStrafeManager(animator);

            input = new InputController();
            input.SubscribeEvents();

            input.MovementRecieved += OnMovementReceived;
            input.MovementEnded += OnMovementEnded;
            input.JumpPerformed += OnJumpPerformed;
        }


        private bool wasJumping = false;

        private void Update()
        {
            input.Update();
            laneChangedThisFrame = false;
            movement.Move();
            playerAnimator.UpdateRunning(movement.IsRunning);
            strafeManager.UpdateStrafe(currentInput);

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

            if (laneChangedThisFrame) return;

            if (dir.x < -0.5f && previousInputX >= -0.5f && movement.MoveLeft())
            {
                laneChangedThisFrame = true;
            }
            else if (dir.x > 0.5f && previousInputX <= 0.5f && movement.MoveRight())
            {
                laneChangedThisFrame = true;
            }

            previousInputX = dir.x;
        }

        private void OnMovementEnded() => currentInput = Vector2.zero;
        private void OnJumpPerformed() => movement.Jump();

        private void OnDestroy()
        {
            input.Dispose();
        }
    }
}
