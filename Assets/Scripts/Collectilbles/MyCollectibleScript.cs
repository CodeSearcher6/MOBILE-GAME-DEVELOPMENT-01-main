using UnityEngine;
using Game.Models;
using JSAM;
using MoreMountains.Feedbacks;
public class MyCollectibleScript : MonoBehaviour
{
    public CollectibleTypes CollectibleType;

    private const string PLAYER_TAG = "Player";
    private CollectibleData data;

    public void Init(CollectibleData collectibleData)
    {
        data = collectibleData;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            if (data.collectFeedback != null)
                data.collectFeedback.PlayFeedbacks(transform.position);

            Collect();
        }
    }

    private void Collect()
    {
        Debug.Log($"Picked up {data.Type}, +{data.Value} points");
        AudioManager.PlaySound(AudioLibrarySounds.CollectilbleSFX);
        MyScoreManager.Instance.AddScore(data.Value);

        if (data.CollectEffect != null)
            Instantiate(data.CollectEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
