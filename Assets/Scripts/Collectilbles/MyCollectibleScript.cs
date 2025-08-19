using UnityEngine;
using Game.Models;
using JSAM;

public class MyCollectibleScript : MonoBehaviour
{
    public CollectibleTypes CollectibleType;
    public float rotationSpeed = 50f;
    public GameObject collectEffect;

    public static event System.Action<MyCollectibleScript> OnCollected;
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (CollectibleType == CollectibleTypes.Coin)
        {
            if (collectEffect)
                Instantiate(collectEffect, transform.position, Quaternion.identity);

            AudioManager.PlaySound(AudioLibrarySounds.CollectilbleSFX);
            OnCollected?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
