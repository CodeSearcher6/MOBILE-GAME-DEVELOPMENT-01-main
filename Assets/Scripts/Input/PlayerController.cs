using System;
using Dreamteck.Forever;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Runner _basicRunner;
        [SerializeField] private float _joystickspeed = 2f;
        [SerializeField] private float _maxInputMagnitude = 3f;
        [SerializeField] private float _offsetSpeed = 2f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private Vector2 _targetVector;
        private InputController _inputController;
        private AnimationManager _animationManager;
        private bool _isJumping;
        private float _verticalVelocity;
        private const int LevelWidth = 4;
        private const float MovementThreshold = 0.01f;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            _animationManager = new AnimationManager(animator);
            _targetVector = Vector2.up;
            _inputController = new();
            _inputController.SubscribeEvents();
            SubscribeEvents();
        }

        private void Update()
        {
            HandleMovement();
            UpdateJump();
        }
        private void UpdateJump()
        {
            if (_isJumping)
            {
                _verticalVelocity += _gravity * Time.deltaTime;

                Vector2 currentOffset = _basicRunner.motion.offset;
                currentOffset.y += _verticalVelocity * Time.deltaTime;
                _basicRunner.motion.offset = currentOffset;

                // Перевірка приземлення
                if (_basicRunner.motion.offset.y <= 0f)
                {
                    _basicRunner.motion.offset = new Vector2(currentOffset.x, 0f);
                    _verticalVelocity = 0f;
                    _isJumping = false;

                    bool isRunning = Mathf.Abs(_basicRunner.motion.offset.x) > MovementThreshold;
                    _animationManager.SetRunning(isRunning);
                }
            }
        }
        private void HandleMovement()
        {
            Vector2 clampedInput = Vector2.ClampMagnitude(_targetVector, _maxInputMagnitude);
            Vector2 target = new Vector2(clampedInput.x * _joystickspeed, _basicRunner.motion.offset.y);
            bool isRunning = Mathf.Abs(_basicRunner.motion.offset.x) > MovementThreshold;
            _animationManager.SetRunning(isRunning);
            _basicRunner.motion.offset = Vector2.MoveTowards(_basicRunner.motion.offset, target, Time.deltaTime * _offsetSpeed);
        }

        public class AnimationManager
        {
            private readonly Animator _animator;
            private bool _isRunning;

            [SerializeField] private string isRunningParam = "IsRunning";
            [SerializeField] private string jumpTrigger = "IsJumping";

            public bool IsRunning => _isRunning;

            public AnimationManager(Animator animator)
            {
                _animator = animator;
            }

            public void SetRunning(bool running)
            {
                if (_isRunning == running) return;
                _animator.SetBool(isRunningParam, running);
                _isRunning = running;
            }

            public void TriggerJump()
            {
                _animator.SetTrigger(jumpTrigger);
            }
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
            _animationManager.TriggerJump();
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
