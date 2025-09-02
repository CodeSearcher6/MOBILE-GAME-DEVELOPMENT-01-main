using UnityEngine;
using TMPro;
using VContainer;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private IScoreService scoreService;

    [Inject]
    public void Construct(IScoreService scoreService)
    {
        this.scoreService = scoreService;
    }

    private void Start()
    {
        Debug.Log("ScoreUI стартував");
        Debug.Log("Current score: " + scoreService.CurrentScore);

        scoreService.OnScoreChanged += RefreshUI;
        RefreshUI();
    }

    private void OnDestroy()
    {
        scoreService.OnScoreChanged -= RefreshUI;
    }

    private void RefreshUI()
    {
        Debug.Log("Оновлення UI: " + scoreService.CurrentScore);

        scoreText.text = $"Score: {scoreService.CurrentScore}";
        highScoreText.text = $"High Score: {scoreService.HighScore}";
    }
}
