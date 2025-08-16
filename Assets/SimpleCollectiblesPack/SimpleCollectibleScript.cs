using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour
{
	public static event Action<SimpleCollectibleScript> OnCollected;


	public enum CollectibleTypes { NoType, Type1, Type2, Type3, Type4, Type5 }; // you can replace this with your own labels for the types of collectibles in your game!

	public CollectibleTypes CollectibleType; // this gameObject's type

	public bool rotate; // do you want it to rotate?

	public float rotationSpeed;

	public AudioClip collectSound;

	public GameObject collectEffect;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		if (rotate)
			transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Collect();
		}
	}

	public void Collect()
	{
		if (collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if (collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);

		//Below is space to add in your code for what happens based on the collectible type


		if (CollectibleType == CollectibleTypes.Type1)
		{


		}
		OnCollected?.Invoke(this);
		Destroy(gameObject);
	}
}
