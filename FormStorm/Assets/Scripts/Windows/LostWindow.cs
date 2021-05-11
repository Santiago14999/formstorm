using TMPro;
using UnityEngine;

public class LostWindow : MonoBehaviour, IWindow
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    public void OnWindowOpened()
    {
        int score = ScoreController.Instance.Score;
        if (PlayerPrefs.GetInt("Score") < score)
        {
            PlayerPrefs.SetInt("Score", score);
            LeaderboardManager.Instance.UploadScore(null);
            _scoreText.text = "NEW BEST: " + score;
            AudioManager.Instance.PlayNewRecordSound();
        }
        else
            _scoreText.text = "SCORE: " + score;
    }
}
