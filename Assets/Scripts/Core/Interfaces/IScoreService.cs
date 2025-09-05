using System;
using Game.Models;

public interface IScoreService
{
    int CurrentScore { get; }
    int HighScore { get; }

    void AddScore(int amount);
    void AddScore(CollectibleTypes type);

    event Action OnScoreChanged;
}
