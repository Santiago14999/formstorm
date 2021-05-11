using TMPro;
using UnityEngine;

public class MenuWindow : MonoBehaviour, IWindow
{
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Score"))
            PlayerPrefs.SetInt("Score", 0);

        OnWindowOpened();
    }

    public void OnWindowOpened()
    {
        _scoreText.text = $"BEST: {PlayerPrefs.GetInt("Score")}";
        SetLabelColor();
    }

    private void SetLabelColor()
    {
        int h = Random.Range(0, 12) * 30;
        _label.color = Color.HSVToRGB((float)h / 360f, 1f, 1f);
    }
}
