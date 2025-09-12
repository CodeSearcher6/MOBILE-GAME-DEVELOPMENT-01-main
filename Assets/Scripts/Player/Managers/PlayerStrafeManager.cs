using UnityEngine;

public class PlayerStrafeManager
{
    private readonly Animator _animator;
    private readonly float _strafeThreshold;
    private bool _isStrafingLeft;
    private bool _isStrafingRight;

    public PlayerStrafeManager(Animator animator, float strafeThreshold = 0.5f)
    {
        _animator = animator;
        _strafeThreshold = strafeThreshold;
    }

    public void UpdateStrafe(Vector2 input)
    {
        bool left = input.x < -_strafeThreshold;
        bool right = input.x > _strafeThreshold;

        //Debug.Log($"Strafe check: input.x = {input.x}, left = {left}, right = {right}");

        if (left != _isStrafingLeft)
        {
            _animator.SetBool("StrafeLeft", left);
            Debug.Log("StrafeLeft set to " + left);
            _isStrafingLeft = left;
        }

        if (right != _isStrafingRight)
        {
            _animator.SetBool("StrafeRight", right);
            Debug.Log("StrafeRight set to " + right);
            _isStrafingRight = right;
        }
    }

    public void ResetStrafe()
    {
        _animator.SetBool("StrafeLeft", false);
        _animator.SetBool("StrafeRight", false);
        _isStrafingLeft = false;
        _isStrafingRight = false;
    }
}
