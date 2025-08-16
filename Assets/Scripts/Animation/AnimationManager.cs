using UnityEngine;

namespace Game.Animation
{
    [CreateAssetMenu(fileName = "AnimationManagerSO", menuName = "Game/AnimationManager")]
    public class AnimationManagerSO : ScriptableObject
    {
        [SerializeField] private string isRunningParam = "IsRunning";
        [SerializeField] private string jumpTrigger = "IsJumping";

        public void SetRunning(Animator animator, ref bool currentState, bool running)
        {
            if (currentState == running) return;
            animator.SetBool(isRunningParam, running);
            currentState = running;
            Debug.Log("Running state set to: " + running);

        }

        public void TriggerJump(Animator animator)
        {
            animator.SetTrigger(jumpTrigger);
            Debug.Log("Jump triggered");

        }
    }
}
