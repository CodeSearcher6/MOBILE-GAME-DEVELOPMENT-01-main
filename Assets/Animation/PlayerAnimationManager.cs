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

        public void TriggerJump()
        {
            _animationSO.TriggerJump(_animator);
        }

        public void UpdateRunning(bool isRunningNow)
        {
            _animationSO.SetRunning(_animator, ref _isRunning, isRunningNow);
        }
    }
}

