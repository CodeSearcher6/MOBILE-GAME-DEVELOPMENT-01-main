using UnityEngine;
using Dreamteck.Forever;
using System;

public class PlayerJumpManager
{
    private readonly Runner _runner;
    private readonly float _gravity;
    private readonly float _jumpForce;

    private float _verticalVelocity;
    private bool _isJumping;

    public event Action Landed;

    public PlayerJumpManager(Runner runner, float gravity, float jumpForce)
    {
        _runner = runner;
        _gravity = gravity;
        _jumpForce = jumpForce;
    }

    public void Jump()
    {
        if (_isJumping) return;

        _verticalVelocity = _jumpForce;
        _isJumping = true;
    }

    public void UpdateJump()
    {
        if (!_isJumping) return;

        _verticalVelocity += _gravity * Time.deltaTime;
        _runner.motion.offset += Vector2.up * _verticalVelocity * Time.deltaTime;

        if (_runner.motion.offset.y <= 0f)
        {
            _runner.motion.offset = new Vector2(_runner.motion.offset.x, 0f);
            _verticalVelocity = 0f;
            _isJumping = false;
            Landed?.Invoke();
        }
    }
}
