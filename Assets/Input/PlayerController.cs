using UnityEngine;
using Dreamteck.Forever;
using Game.Animation;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationManagerSO animationManagerSO;
    [SerializeField] private Runner runner;

    [Header("Movement Settings")]
    [SerializeField] private float joystickSpeed = 2f;
    [SerializeField] private float maxInputMagnitude = 3f;
    [SerializeField] private float offsetSpeed = 2f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    private InputController inputController;
    private PlayerMovementManager movementManager;
    private PlayerJumpManager jumpManager;
    private PlayerAnimationManager animationManager;

    private Vector2 targetVector;

    private void Awake()
    {
        inputController = new();
        inputController.SubscribeEvents();

        movementManager = new PlayerMovementManager(runner, joystickSpeed, offsetSpeed, maxInputMagnitude);
        jumpManager = new PlayerJumpManager(runner, gravity, jumpForce);
        animationManager = new PlayerAnimationManager(animator, animationManagerSO);

        SubscribeEvents();
    }

    private void Update()
    {
        movementManager.Move(targetVector);
        jumpManager.UpdateJump();
    }

    private void SubscribeEvents()
    {
        inputController.MovementRecieved += OnMovementPerformed;
        inputController.JumpPerformed += OnJumpPerformed;
        jumpManager.Landed += OnLanded;
    }

    private void UnsubscribeEvents()
    {
        inputController.MovementRecieved -= OnMovementPerformed;
        inputController.JumpPerformed -= OnJumpPerformed;
        jumpManager.Landed -= OnLanded;
    }

    private void OnMovementPerformed(Vector2 movement)
    {
        Debug.Log("Movement received: " + movement);
        targetVector = new Vector2(Mathf.Clamp(targetVector.x + movement.x, -4, 4), 2f);
    }

    private void OnJumpPerformed()
    {
        jumpManager.Jump();
        animationManager.TriggerJump();
    }

    private void OnLanded()
    {
        animationManager.UpdateRunning(movementManager.IsRunning);
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
        inputController.Dispose();
    }
}
