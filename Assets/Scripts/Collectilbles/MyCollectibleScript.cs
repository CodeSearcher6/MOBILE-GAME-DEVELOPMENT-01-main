using UnityEngine;
using Game.Models;
using JSAM;
public class MyCollectibleScript : MonoBehaviour
{
    public CollectibleTypes CollectibleType;
    public GameObject collectEffect;

    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
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
