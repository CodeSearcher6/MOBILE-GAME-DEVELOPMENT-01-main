using Game.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreService : IScoreService
{
    private int score = 0;
    private int highScore = 0;

    public event Action OnScoreChanged;

    public int CurrentScore => score;
    public int HighScore => highScore;

    private readonly Dictionary<CollectibleTypes, int> scoreValues = new()
    {
        { CollectibleTypes.Coin, 1 },
        { CollectibleTypes.Gem, 50 },
        { CollectibleTypes.Star, 10 }
    };

    public ScoreService()
    {
        highScore = SaveManager.LoadHighScore();
    }

    public void AddScore(int amount)
    {
        score += amount;

        if (score > highScore)
        {
            highScore = score;
            SaveManager.SaveHighScore(highScore);
        }

        OnScoreChanged?.Invoke();
    }

    public void AddScore(CollectibleTypes type)
    {
        Debug.Log("ScoreService: додано очки");
        OnScoreChanged?.Invoke();

        if (!scoreValues.TryGetValue(type, out int value)) return;
        AddScore(value);
    }
}
