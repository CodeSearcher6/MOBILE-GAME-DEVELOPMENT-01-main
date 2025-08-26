using UnityEngine;
using Dreamteck.Forever;

public class PlayerMovementManager
{
    private readonly Runner _runner;
    private readonly float _joystickSpeed;
    private readonly float _offsetSpeed;
    private readonly float _maxInputMagnitude;
    private readonly float _smoothFactor;
    private readonly float _edgeFalloff;

    public PlayerMovementManager(Runner runner, float joystickSpeed, float offsetSpeed, float maxInputMagnitude, float smoothFactor = 10f, float edgeFalloff = 0.5f)
    {
        _runner = runner;
        _joystickSpeed = joystickSpeed;
        _offsetSpeed = offsetSpeed;
        _maxInputMagnitude = maxInputMagnitude;
        _smoothFactor = smoothFactor;
        _edgeFalloff = edgeFalloff;
    }

    public void Move(Vector2 input)
    {
        Vector2 clampedInput = Vector2.ClampMagnitude(input, _maxInputMagnitude);

        // Нелінійна чутливість
        float sensitivity = Mathf.Pow(Mathf.Abs(clampedInput.x), 1.5f);
        float targetX = Mathf.Sign(clampedInput.x) * sensitivity * _joystickSpeed;

        float currentX = _runner.motion.offset.x;
        float newX;

        if (clampedInput == Vector2.zero)
        {
            newX = Mathf.Lerp(currentX, 0f, Time.deltaTime * _smoothFactor);
        }
        else
        {
            newX = Mathf.Lerp(currentX, targetX, Time.deltaTime * _smoothFactor);
        }

        float clampedX = Mathf.Clamp(newX, -3f, 3f);
        if (Mathf.Abs(clampedX) > 2.5f)
        {
            clampedX = Mathf.Lerp(clampedX, Mathf.Sign(clampedX) * 3f, _edgeFalloff);
        }

        _runner.motion.offset = new Vector2(clampedX, _runner.motion.offset.y);
    }

    public bool IsRunning => Mathf.Abs(_runner.motion.offset.x) > 0.01f;
}
