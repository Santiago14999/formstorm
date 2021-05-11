using UnityEngine;

public class LeaderboardWindow : MonoBehaviour, IWindow
{
    [SerializeField] private CreateUser _createUsernameWindow;
    [SerializeField] private Leaderboard _leaderboardWindow;

    private LeaderboardManager _leaderboardManager;

    public void OnWindowOpened()
    {
        if (_leaderboardManager == null)
            _leaderboardManager = LeaderboardManager.Instance;

        UploadScoreOrCreateUser();
    }

    private void UploadScoreOrCreateUser()
    {
        _leaderboardManager.HasId(UploadScoreOrCreateUserCallback);
    }
    private void UploadScoreOrCreateUserCallback(bool state, string result)
    {
        if (state)
        {
            if (result == "0") // Id does not exist in database
            {
                if (PlayerPrefs.GetInt("Score") > 0)
                    OpenCreateUsernameWindow();
                else
                    OpenLeaderboardWindow();
            }
            else if (result == "-1") 
            {
                Debug.Log("Server error: " + result);
                ReturnToMenu("Server error.");
            }
            else // Id exits in database. 'result' contains username
            {
                if (!PlayerPrefs.HasKey("Username"))
                    PlayerPrefs.SetString("Username", result);

                OpenLeaderboardWindow();
            }
        }
        else
        {
            Debug.Log("Upload score or create user error: " + result);
            ReturnToMenu("Unable to connect to server.");
        }
    }

    private void OpenCreateUsernameWindow()
    {
        _leaderboardWindow.SetActive(false);
        _createUsernameWindow.SetActive(true);
    }

    public void OpenLeaderboardWindow()
    {
        _createUsernameWindow.SetActive(false);
        _leaderboardWindow.SetActive(true);
    }
    public void ReturnToMenu(string message)
    {
        _createUsernameWindow.SetActive(false);
        _leaderboardWindow.SetActive(false);
        ToastManager.Instance.Toast(message, ToastManager.MessageType.Error);
        GameManager.Instance.OpenMenu();
    }
}
