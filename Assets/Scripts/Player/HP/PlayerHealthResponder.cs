using UnityEngine;
using Dreamteck.Forever;
using MoreMountains.Feedbacks;
using TMPro;
using Game.Animation;

public class PlayerHealthResponder : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerHealth health;
    [SerializeField] private Runner runner;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationManagerSO animationSO;

    [Header("Visuals & Feedback")]
    [SerializeField] private MMF_Player damageFeedback;
    [SerializeField] private GameObject reviveScreen;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private TextMeshProUGUI HPCountText;

    private PlayerAnimationManager _animationManager;

    private void Awake()
    {
        _animationManager = new PlayerAnimationManager(animator, animationSO);
    }

    private void OnEnable()
    {
        if (health == null) return;

        health.OnDamaged += HandleDamage;
        health.OnDied += HandleDeath;
        health.OnRevived += HandleRevive;
    }

    private void OnDisable()
    {
        if (health == null) return;

        health.OnDamaged -= HandleDamage;
        health.OnDied -= HandleDeath;
        health.OnRevived -= HandleRevive;
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
            ShowGameOverScreen();
            reviveScreen?.SetActive(false);
        }

        runner.enabled = false;
        _animationManager.SetFalling(true);
        _animationManager.SetAlive(false);   
        UpdateUI();
    }

    private void HandleRevive()
    {
        reviveScreen?.SetActive(false);
        runner.enabled = true;

        _animationManager.SetFalling(false);
        _animationManager.SetAlive(true);    
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
