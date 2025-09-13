using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 5;
    public int currentHP;

    public TextMeshProUGUI HPCountText;
    public GameObject reviveScreen;
    
    public MMF_Player damageFeedback;
    public Animator animator;

    private bool isFalling = false;
    private bool isAlive = true;

    void Start()
    {
        UpdateUI();
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered with: " + other.name);

        if (!isAlive || isFalling)
        {
            Debug.Log("Blocked by state: isAlive=" + isAlive + ", isFalling=" + isFalling);
            return;
        }

        if (other.CompareTag("Obstacle"))
        {
            HandleCollision();
        }
    }


    public void HandleCollision()
    {
        Debug.Log("Player hit an obstacle!");
        damageFeedback.PlayFeedbacks();

        if (currentHP > 1)
        {
            currentHP--;
            UpdateUI();
        }
        else
        {
            StartFallSequence();
        }
    }

    public void StartFallSequence()
    {
        isFalling = true;
        isAlive = false;
        animator.SetBool("isFalling", true);
        Invoke(nameof(ShowReviveScreen), 1.5f);
    }

    public void ShowReviveScreen()
    {
        reviveScreen.SetActive(true);
    }

    public void OnReviveButtonPressed()
    {
        Debug.Log("Revive button pressed");
        currentHP = Mathf.Max(currentHP - 1, 0);
        UpdateUI();

        reviveScreen.SetActive(false);
        isAlive = true;
        isFalling = false;
        animator.SetBool("isAlive", true);
        animator.SetBool("isFalling", false);
        animator.Play("Run");

    }

    public void UpdateUI()
    {
        HPCountText.text = $"HP: {currentHP}";
    }
}
