using UnityEngine;
using Dreamteck.Forever;

public class PlayerMovementManager
{
    private readonly Runner _runner;
    private readonly float _joystickSpeed;
    private readonly float _offsetSpeed;
    private readonly float _maxInputMagnitude;

    public PlayerMovementManager(Runner runner, float joystickSpeed, float offsetSpeed, float maxInputMagnitude)
    {
        _runner = runner;
        _joystickSpeed = joystickSpeed;
        _offsetSpeed = offsetSpeed;
        _maxInputMagnitude = maxInputMagnitude;
    }

    public void Move(Vector2 input)
    {
        Vector2 clampedInput = Vector2.ClampMagnitude(input, _maxInputMagnitude);
        float targetX = clampedInput.x * _joystickSpeed;
        float newX = Mathf.MoveTowards(_runner.motion.offset.x, targetX, Time.deltaTime * _offsetSpeed);
        newX = Mathf.Clamp(newX, -3f, 3f);
        _runner.motion.offset = new Vector2(newX, _runner.motion.offset.y);
    }

    public bool IsRunning => Mathf.Abs(_runner.motion.offset.x) > 0.01f;
}
