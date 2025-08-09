using System;
using Dreamteck.Forever;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        private InputController _inputController;
        [SerializeField] private Animator animator;
        [SerializeField] private Runner _basicRunner;
        [SerializeField] private float _joystickspeed = 2f;
        [SerializeField] private float _maxInputMagnitude = 3f;
        [SerializeField] private float _offsetSpeed = 2f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _gravity = 9.8f;
        [SerializeField] private Vector2 _targetVector;

        private float _verticalVelocity;
        private bool _isJumping;
        private const int LevelWidth = 4;

        private enum AnimationState { Idle, Running, Jumping }
        private AnimationState _currentState;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            _targetVector = Vector2.up;
            _inputController = new();
            _inputController.SubscribeEvents();
            SubscribeEvents();
        }

        private void Update()
        {
            HandleMovement();
            HandleAnimationState();
        }

        private void HandleMovement()
        {
            Vector2 clampedInput = Vector2.ClampMagnitude(_targetVector, _maxInputMagnitude);
            Vector2 target = new Vector2(clampedInput.x * _joystickspeed, _basicRunner.motion.offset.y);

            if (_isJumping)
            {
                _verticalVelocity -= _gravity * Time.deltaTime;
                target.y += _verticalVelocity * Time.deltaTime;

                if (target.y <= 0f)
                {
                    target.y = 0f;
                    _verticalVelocity = 0f;
                    _isJumping = false;
                }
            }

            _basicRunner.motion.offset = Vector2.MoveTowards(_basicRunner.motion.offset, target, Time.deltaTime * _offsetSpeed);
        }

        private void HandleAnimationState()
        {
            bool isMoving = Mathf.Abs(_basicRunner.motion.offset.x) > 0.01f;

            if (_isJumping)
            {
                SetAnimationState(AnimationState.Jumping);
            }
            else if (isMoving)
            {
                SetAnimationState(AnimationState.Running);
            }
        }

        private void SetAnimationState(AnimationState newState)
        {
            if (newState == _currentState) return;

            switch (newState)
            {
                case AnimationState.Running:
                    animator.Play("Run");
                    break;
                case AnimationState.Jumping:
                    animator.Play("Jump");
                    break;
            }

            _currentState = newState;
        }

        public void SubscribeEvents()
        {
            _inputController.MovementRecieved += OnMovementReceived;
            _inputController.MovementEnded += OnMovementEnded;
            _inputController.JumpPerformed += OnJumpPerformed;
        }

        private void UnsubscribeEvents()
        {
            _inputController.MovementRecieved -= OnMovementReceived;
            _inputController.MovementEnded -= OnMovementEnded;
            _inputController.JumpPerformed -= OnJumpPerformed;
        }

        private void OnMovementReceived(Vector2 movement)
        {
            Debug.Log("Movement received: " + movement);
            _targetVector = new Vector2(Mathf.Clamp(_targetVector.x + movement.x, -LevelWidth, LevelWidth), 2f);
        }

        private void OnMovementEnded()
        {
            _targetVector = Vector2.zero;
            Debug.Log("Movement ended");
        }

        private void OnJumpPerformed()
        {
            if (_isJumping) return;

            _verticalVelocity = _jumpForce;
            _isJumping = true;
            SetAnimationState(AnimationState.Jumping);
            Debug.Log("Jump triggered");
        }

        private void OnEscapeButtonPressed()
        {
            Debug.Log("Escape button pressed");
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
            _inputController.Dispose();
        }
    }
}
