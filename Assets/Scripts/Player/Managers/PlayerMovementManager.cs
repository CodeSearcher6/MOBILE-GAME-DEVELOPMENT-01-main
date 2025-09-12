using UnityEngine;
using Dreamteck.Forever;

public class PlayerMovementManager
{
    private readonly Runner _runner;
    private readonly float _laneOffset;   // Відстань між доріжками
    private readonly float _laneChangeSpeed;
    private readonly float _jumpForce;
    private readonly float _gravity;

    private int _currentLane = 0;  // -1 = ліва, 0 = середина, 1 = права
    private float _verticalVelocity;
    private bool _isJumping;

    public PlayerMovementManager(Runner runner, float laneOffset, float laneChangeSpeed, float jumpForce, float gravity)
    {
        _runner = runner;
        _laneOffset = laneOffset;
        _laneChangeSpeed = laneChangeSpeed;
        _jumpForce = jumpForce;
        _gravity = gravity;
    }

    public void Move()
    {
        float targetX = _currentLane * _laneOffset;
        float newX = Mathf.MoveTowards(_runner.motion.offset.x, targetX, _laneChangeSpeed * Time.deltaTime);

        float newY = _runner.motion.offset.y;
        if (_isJumping)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
            newY += _verticalVelocity * Time.deltaTime;

            if (newY <= 0f)
            {
                newY = 0f;
                _isJumping = false;
                _verticalVelocity = 0f;
            }
        }

        _runner.motion.offset = new Vector2(newX, newY);
    }

    public bool MoveLeft()
    {
        if (_currentLane > -1)
        {
            _currentLane--;
            return true;
        }
        return false;
    }

    public bool MoveRight()
    {
        if (_currentLane < 1)
        {
            _currentLane++;
            return true;
        }
        return false;
    }

    public void Jump()
    {
        if (_isJumping) return;
        _isJumping = true;
        _verticalVelocity = _jumpForce;
    }

    public bool IsRunning => Mathf.Abs(_runner.motion.offset.x) > 0.01f;
    public bool IsJumping => _isJumping;
}
