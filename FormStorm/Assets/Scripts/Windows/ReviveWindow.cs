using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviveWindow : MonoBehaviour, IWindow
{
    [SerializeField] private float _timeToDecision;
    [SerializeField] private Image _reviveBar;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private Coroutine _coroutine;
    private WindowsManager _windowsManager;
    private AdController _adController;
    private ScoreController _scoreController;

    private void Start()
    {
        _windowsManager = WindowsManager.Instance;
        _adController = AdController.Instance;
    }

    public void OnWindowOpened()
    {
        _reviveBar.fillAmount = 1f;
        _coroutine = StartCoroutine(ReviveBarCoroutine());
        if (_scoreController == null)
            _scoreController = ScoreController.Instance;

        _scoreText.text = "SCORE: " + _scoreController.Score.ToString();
    }

    public void Revive()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (!_adController.ShowRewardedVideo())
            Skip();
    }

    public void Skip()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _windowsManager.OpenWindow(WindowsManager.WindowType.Lost);
    }

    IEnumerator ReviveBarCoroutine()
    {
        float startTime = Time.time;
        while(Time.time < startTime + _timeToDecision)
        {
            _reviveBar.fillAmount = (startTime + _timeToDecision - Time.time) / _timeToDecision;
            yield return null;
        }

        _reviveBar.fillAmount = 0;
        _coroutine = null;
        Skip();
    }
}
