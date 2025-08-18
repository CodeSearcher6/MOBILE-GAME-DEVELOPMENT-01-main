using UnityEngine;
using System.Collections.Generic;
using TMPro;

public enum CollectibleTypes { Type1, Gem, Star }


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private int score = 0;
    private Dictionary<SimpleCollectibleScript.CollectibleTypes, int> scoreValues = new()
    {
        { SimpleCollectibleScript.CollectibleTypes.Type1, 1 },
    };

    private void HandleCollectible(SimpleCollectibleScript collectible)
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
        SimpleCollectibleScript.OnCollected += HandleCollectible;
    }

    private void OnDisable()
    {
        SimpleCollectibleScript.OnCollected -= HandleCollectible;
    }

}


