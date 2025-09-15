using UnityEngine;

namespace Game.Animation
{
    [CreateAssetMenu(fileName = "AnimationManagerSO", menuName = "Game/AnimationManager")]
    public class AnimationManagerSO : ScriptableObject
    {
        [Header("Animator Parameters")]
        public string isRunningParam = "IsRunning";
        public string isJumpingParam = "IsJumping";
        public string isFallingParam = "IsFalling";
        public string isAliveParam = "IsAlive";  
    }
}
