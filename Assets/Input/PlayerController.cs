using System;
using Dreamteck.Forever;
using UnityEngine;
namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        private InputController _inputController;
        [SerializeField] private Runner _basicRunner; 
        [SerializeField] private float _joystickspeed = 2f; 
        [SerializeField] private float _maxInputMagnitude = 3f;
        [SerializeField] private float _offsetLerpSpeed = 2f;
        
        private Vector2 _targetVector;
        private const int LevelWidth = 4;

        private void Update()
        {
            Vector2 clampedInput = Vector2.ClampMagnitude(_targetVector, _maxInputMagnitude);
            Vector2 target = new Vector2(clampedInput.x * _joystickspeed, _basicRunner.motion.offset.y);

            _basicRunner.motion.offset = Vector2.MoveTowards( _basicRunner.motion.offset,target,Time.deltaTime * _offsetLerpSpeed);

        }
        private void Awake()
        {
            _inputController = new();
            _inputController.SubscribeEvents();
            SubscribeEvents();
        }

        public void SubscribeEvents()
        {
            _inputController.MovementRecieved += OnMovementReceived;
            _inputController.MovementEnded += OnMovementEnded;
        }

        private void UnsubscribeEvent()
        {
            _inputController.MovementRecieved -= OnMovementReceived;
            _inputController.MovementEnded -= OnMovementEnded;
        }

        private void OnMovementReceived(Vector2 movement)
        {
            Debug.Log("Movement received: " + movement);
            _targetVector = new Vector2(Mathf.Clamp(_targetVector.x + movement.x, -LevelWidth, LevelWidth), 2f);
        }
        private void OnMovementEnded()
        {
            _targetVector = Vector2.zero;
            // Handle movement end logic here
            Debug.Log("Movement ended");
        }

        private void OnEscapeButtonPressed()
        {

            // Handle escape button logic here
            Debug.Log("Escape button pressed");
        }

        private void OnDestroy()
        {
            UnsubscribeEvent();
            _inputController.Dispose();
        }
    }
}