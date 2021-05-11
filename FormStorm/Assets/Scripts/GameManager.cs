using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event System.Action OnGameStart = delegate { };
    public static event System.Action OnGameEnd = delegate { };

    [SerializeField] private string _deviceID;
    public string DeviceID { get => _deviceID; }

    private bool _isSecondAttempt;
    private WindowsManager _windowsManager;
    private AdController _adController;
    private FigureSpawner _figureSpawner;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
#if !UNITY_EDITOR
        _deviceID = SystemInfo.deviceUniqueIdentifier;
#endif
    }

    private void Start()
    {
        _windowsManager = WindowsManager.Instance;
        _adController = AdController.Instance;
        _figureSpawner = FigureSpawner.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnGameEnd();
            OpenMenu();
        }
    }

    public void Play()
    {
        if (_adController.ShouldShowAd())
            return;

        _isSecondAttempt = false;
        _windowsManager.OpenWindow(WindowsManager.WindowType.Game);
        OnGameStart();
    }

    public void Revive()
    {
        _isSecondAttempt = true;
        _windowsManager.OpenWindow(WindowsManager.WindowType.Game);
        _figureSpawner.Revive();
    }

    public void LoseChoise()
    {
        OnGameEnd();
        if (!_isSecondAttempt && _adController.IsAdReady())
        {
            _windowsManager.OpenWindow(WindowsManager.WindowType.Revive);
        }
        else
        {
            _windowsManager.OpenWindow(WindowsManager.WindowType.Lost);
        }
    }

    public void OpenMenu()
    {
        _windowsManager.OpenWindow(WindowsManager.WindowType.Menu);
    }

    public void OpenLeaderboard()
    {
        _windowsManager.OpenWindow(WindowsManager.WindowType.Leaderboard);
    }

    public void OpenSettings()
    {
        _windowsManager.OpenWindow(WindowsManager.WindowType.Settings);
    }

}
