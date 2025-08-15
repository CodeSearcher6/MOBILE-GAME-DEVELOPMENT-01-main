using UnityEngine;

public class Jump_Trigger : MonoBehaviour
{
    [SerializeField] private float jumpForce = 100f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                Vector3 jumpVector = Vector3.up * jumpForce * Time.deltaTime;
                controller.Move(jumpVector);
                Debug.Log("Player entered JumpPad");
            }

        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited JumpPad");
        }
    }
}
