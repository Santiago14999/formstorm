using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    public int Score { get; private set; }

    public static ScoreController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStart += ResetScore;
    }

    public void AddScore(int value)
    {
        Score += value;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        _scoreText.text = Score.ToString();
    }

    private void ResetScore()
    {
        Score = 0;
        UpdateScoreUI();
    }
}
