using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Record : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _usernameText;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Color _localUserColor;

    public void Initialize(int score, string username, bool localUser)
    {
        _scoreText.text = score.ToString();
        _usernameText.text = username;
        if (localUser)
            _backgroundImage.color = _localUserColor;
    }
}
