using UnityEngine;

namespace Game.Animation
{
    public class PlayerAnimationManager
    {
        private readonly Animator _animator;
        private readonly AnimationManagerSO _animationSO;
        private bool _isRunning;

        public PlayerAnimationManager(Animator animator, AnimationManagerSO animationSO)
        {
            _animator = animator;
            _animationSO = animationSO;
        }

        public void StartJump() =>
            _animator.SetBool(_animationSO.isJumpingParam, true);

        public void EndJump() =>
            _animator.SetBool(_animationSO.isJumpingParam, false);

        public void UpdateRunning(bool isRunningNow)
        {
            if (_isRunning == isRunningNow) return;
            _isRunning = isRunningNow;
            _animator.SetBool(_animationSO.isRunningParam, isRunningNow);
        }

        public void SetFalling(bool falling) =>
            _animator.SetBool(_animationSO.isFallingParam, falling);

        public void SetAlive(bool alive) =>
            _animator.SetBool(_animationSO.isAliveParam, alive);  
    }
}
