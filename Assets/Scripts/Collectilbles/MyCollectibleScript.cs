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
            if (data.feedbacks != null)
                data.feedbacks.PlayFeedbacks(transform.position);

            Collect();
        }
    }

    private void Collect()
    {
        Debug.Log($"Picked up {data.type}, +{data.value} points");
        AudioManager.PlaySound(AudioLibrarySounds.CollectilbleSFX);
        MyScoreManager.Instance.AddScore(data.value);

        if (data.collectEffect != null)
            Instantiate(data.collectEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
