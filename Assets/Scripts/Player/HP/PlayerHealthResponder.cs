using UnityEngine;
using Dreamteck.Forever;
using MoreMountains.Feedbacks;
using TMPro;

public class PlayerHealthResponder : MonoBehaviour
{
    [Header("Visuals & Feedback")]
    [SerializeField] private Runner runner;
    [SerializeField] private Animator animator;
    [SerializeField] private MMF_Player damageFeedback;
    [SerializeField] private GameObject reviveScreen;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private TextMeshProUGUI HPCountText;

    private PlayerHealth health;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();

        health.OnDamaged += HandleDamage;
        health.OnDied += HandleDeath;
        health.OnRevived += HandleRevive;

        UpdateUI();
    }

    private void HandleDamage()
    {
        damageFeedback?.PlayFeedbacks();
        runner.enabled = false;
        reviveScreen?.SetActive(true);
        UpdateUI();
    }

    private void HandleDeath()
    {
        if (health.GetCurrentHP() <= 0)
        {
            ShowGameOverScreen(); // HP вже 0 — кінець
            reviveScreen?.SetActive(false);

        }

        runner.enabled = false;
        animator.SetBool("isFalling", true);
        animator.SetBool("isAlive", false);
        UpdateUI();
    }


    private void HandleRevive()
    {
        reviveScreen?.SetActive(false);
        runner.enabled = true;

        animator.SetBool("isFalling", false);
        animator.SetBool("isAlive", true);
        UpdateUI();
    }
    private void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            Debug.Log("Game Over — no more revives left.");
            runner.enabled = false;
        }
    }

    private void UpdateUI()
    {
        if (HPCountText != null)
            HPCountText.text = $"HP: {health.GetCurrentHP()}";
    }
}
