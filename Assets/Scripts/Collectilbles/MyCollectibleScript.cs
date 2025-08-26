using UnityEngine;
using Game.Models;
using JSAM;
using MoreMountains.Feedbacks;
public class MyCollectibleScript : MonoBehaviour
{
    public CollectibleTypes CollectibleType;
    public GameObject collectEffect;
    [SerializeField] private MMF_Player feedbacks;
    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            feedbacks?.PlayFeedbacks(transform.position);
            Collect();
        }
    }

    private void Collect()
    {
        AudioManager.PlaySound(AudioLibrarySounds.CollectilbleSFX);
        MyScoreManager.Instance.HandleCollectible(this);

        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
