using UnityEngine;

namespace Game.Animation
{
    [CreateAssetMenu(fileName = "AnimationManagerSO", menuName = "Game/AnimationManager")]
    public class AnimationManagerSO : ScriptableObject
    {
        [SerializeField] private string isRunningParam = "IsRunning";
        [SerializeField] private string isJumpingParam = "IsJumping";
        [SerializeField] private string isFallingParam = "isFalling";



        public void Initialize(PlayerHealth health, Animator animator)
        {
            health.OnDied += () => animator.SetBool("isFalling", true);
            health.OnRevived += () => animator.Play("Run");
        }


        public void SetRunning(Animator animator, ref bool currentState, bool running)
        {
            if (animator.GetBool(isJumpingParam)) return; // не оновлюємо, якщо в стрибку

            if (currentState == running) return;

            animator.SetBool(isRunningParam, running);
            currentState = running;
            Debug.Log("Running state set to: " + running);
        }

        public void SetJumping(Animator animator, bool jumping)
        {
            animator.SetBool(isJumpingParam, jumping);
            Debug.Log("Jumping state set to: " + jumping);
        }

        public void SetFalling(Animator animator, bool falling)
        {
            animator.SetBool(isFallingParam, falling);
        }
    }
}
