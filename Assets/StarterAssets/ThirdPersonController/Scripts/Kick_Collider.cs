using UnityEngine;

public class Kick_Collider : MonoBehaviour
{
    [SerializeField] private float kickForce = 25f;
    [SerializeField] private ForceMode forceMode = ForceMode.Impulse; 
    
    

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision with: " + other.collider.name);
        

        Rigidbody rb = other.collider.attachedRigidbody;

        if (other.collider.CompareTag("Player") && rb != null)
        {
            rb.freezeRotation = true;
        }

        if (rb != null)
        {
            rb.AddForce(Vector3.up * kickForce, forceMode);
            Debug.Log("Player collided with cube");
        }

    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Debug.Log("Player exited cube collider");
        }
    }
}
