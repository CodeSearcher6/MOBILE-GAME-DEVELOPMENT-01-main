using UnityEngine;
using Game.Models;
using System.Collections.Generic;
using TMPro;

public class MyScoreManager : MonoBehaviour
{
    public static MyScoreManager Instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;

    private Dictionary<CollectibleTypes, int> scoreValues = new()
    {
        { CollectibleTypes.Coin, 1 },
        { CollectibleTypes.Gem, 5 },
        { CollectibleTypes.Star, 10 }
    };

    public void HandleCollectible(MyCollectibleScript collectible)
    {
        if (scoreValues.TryGetValue(collectible.CollectibleType, out int value))
        {
            AddScore(value);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;

        if (scoreText != null)
            scoreText.text = $"Score: {score}";
        else
            Debug.LogWarning("Score Text is not assigned!");

        Debug.Log("Current Score: " + score);
    }

    private void OnEnable()
    {
        MyCollectibleScript.OnCollected += HandleCollectible;
    }

    private void OnDisable()
    {
        MyCollectibleScript.OnCollected -= HandleCollectible;
    }
}
