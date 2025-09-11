using UnityEngine;
using Dreamteck.Forever;

public class PlayerMovementManager
{
    private readonly Runner _runner;
    private readonly float _horizontalAcceleration;
    private readonly float _smoothFactor;
    private readonly float _edgeFalloff;
    private readonly float _jumpForce;
    private readonly float _gravity;
    private float _verticalVelocity;
    private bool _isJumping;

    public PlayerMovementManager(
     Runner runner,
     float horizontalAcceleration,
     float smoothFactor,
     float jumpForce,
     float gravity,
     float edgeFalloff = 5f)
    {
        _runner = runner;

        _horizontalAcceleration = horizontalAcceleration;
        _jumpForce = jumpForce;
        _gravity = gravity;
        _edgeFalloff = edgeFalloff;
        _smoothFactor = smoothFactor;
    }


    public void Move(Vector2 input)
    {
        // 🔹 Нормалізація input'у
        Vector2 clampedInput = Vector2.ClampMagnitude(input, 1f);

        // 🔹 Обчислення чутливості з easing'ом
        float sensitivity = Mathf.Pow(Mathf.Abs(clampedInput.x), 1.5f);
        float targetX = Mathf.Sign(clampedInput.x) * sensitivity * _horizontalAcceleration;

        // 🔹 Інерція при зміні напрямку
        float currentX = _runner.motion.offset.x;
        float directionChangeSmooth = Mathf.Lerp(currentX, targetX, Time.deltaTime * _smoothFactor);
        float newX = (clampedInput == Vector2.zero)
            ? Mathf.Lerp(currentX, 0f, Time.deltaTime * _smoothFactor * (Mathf.Abs(currentX) < 1f ? 2f : 1f)) // пружне повернення
            : Mathf.Lerp(currentX, directionChangeSmooth, Time.deltaTime * _smoothFactor);

        // 🔹 Edge falloff з easing'ом
        float clampedX = Mathf.Clamp(newX, -_edgeFalloff, _edgeFalloff);

        if (Mathf.Abs(clampedX) > 2.5f)
        {
            float edgeT = Mathf.InverseLerp(2.5f, 3f, Mathf.Abs(clampedX));
            float eased = Mathf.SmoothStep(0f, 1f, edgeT);
            clampedX = Mathf.Sign(clampedX) * Mathf.Lerp(2.5f, 3f, eased * (1f - _edgeFalloff));
        }

        // 🔹 Застосування offset'у по X
        _runner.motion.offset = new Vector2(clampedX, _runner.motion.offset.y);

        // 🔹 Рух по Y (стрибки)
        if (_isJumping)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
            float newY = _runner.motion.offset.y + _verticalVelocity * Time.deltaTime;

            if (newY <= 0f)
            {
                newY = 0f;
                _isJumping = false;
                _verticalVelocity = 0f;
            }

            _runner.motion.offset = new Vector2(_runner.motion.offset.x, newY);
        }
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

