using UnityEngine;
using Game.Models;
using JSAM;
using MoreMountains.Feedbacks;
using VContainer;
public class MyCollectibleScript : MonoBehaviour
{
    public CollectibleTypes CollectibleType;
    private IScoreService scoreService;
    private const string PLAYER_TAG = "Player";
    private CollectibleData data;
    private CollectibleFeedbackManager feedbackManager;

    [Inject]
    public void Init(CollectibleData collectibleData, IScoreService scoreService, CollectibleFeedbackManager feedbackManager)
    {
        data = collectibleData;
        this.scoreService = scoreService;
        this.feedbackManager = feedbackManager;
    }

    public void Init(CollectibleData collectibleData, IScoreService scoreService)
    {
        data = collectibleData;
        this.scoreService = scoreService;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            if (data.collectFeedback != null)
                data.collectFeedback.PlayFeedbacks(transform.position);

            feedbackManager.Play(data.Type, transform.position);
            Debug.Log($"Playing feedback for {data.Type}");

            Collect();
        }
    }

    private void Collect()
    {
        Debug.Log($"Picked up {data.Type}, +{data.Value} points");
        AudioManager.PlaySound(AudioLibrarySounds.CollectilbleSFX);
        scoreService.AddScore(data.Type);

        if (data.CollectEffect != null)
            Instantiate(data.CollectEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
