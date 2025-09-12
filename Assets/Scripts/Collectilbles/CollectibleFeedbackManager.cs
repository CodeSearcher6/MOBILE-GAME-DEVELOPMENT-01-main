using UnityEngine;
using MoreMountains.Feedbacks;
using Game.Models;
public class CollectibleFeedbackManager : MonoBehaviour
{
    [SerializeField] private MMF_Player coinFeedback;
    [SerializeField] private MMF_Player gemFeedback;

    public void Play(CollectibleTypes type, Vector3 position)
    {
        switch (type)
        {
            case CollectibleTypes.Coin:
            case CollectibleTypes.Star:
                coinFeedback?.PlayFeedbacks(position);
                break;
            case CollectibleTypes.Gem:
                gemFeedback?.PlayFeedbacks(position);
                break;
        }
    }
}
