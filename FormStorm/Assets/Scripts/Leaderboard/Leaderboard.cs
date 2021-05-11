using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Record _recordPrefab;
    [SerializeField] private Transform _parent;
    [SerializeField] private LeaderboardWindow _leaderboardWindow;

    private LeaderboardManager _leaderboardManager;

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
        if (state)
        {
            if (_leaderboardManager == null)
                _leaderboardManager = LeaderboardManager.Instance;

            UploadScore();
        }
    }

    private void UploadScore()
    {
        if (PlayerPrefs.GetInt("Score") > 0)
            _leaderboardManager.UploadScore(UploadScoreCallback);
        else
            LoadBoard();
    }

    private void UploadScoreCallback(bool state, string result)
    {
        if (state)
        {
            if (result == "1")
            {
                LoadBoard();
            }
            else
            {
                Debug.Log("Server error. HTML message: " + result);
                _leaderboardWindow.ReturnToMenu("Server error.");
            }
        }
        else
        {
            Debug.Log("Upload score error. Message: " + result);
            _leaderboardWindow.ReturnToMenu("Server error.");
        }
    }

    private void LoadBoard()
    {
        _leaderboardManager.LoadBoard(LoadBoardCallback);
    }

    private void LoadBoardCallback(bool state, string result)
    {
        if (state)
        {
            for (int i = 0; i < _parent.childCount; i++)
                Destroy(_parent.GetChild(i).gameObject);

            string localUsername = PlayerPrefs.HasKey("Username") ? PlayerPrefs.GetString("Username") : null;
            foreach (string rec in result.Trim('%').Split('%'))
            {
                string[] values = rec.Split('/');
                int score = int.Parse(values[0]);
                string username = values[1];
                bool isLocalPlayer = false;
                if (localUsername != null && username == localUsername)
                    isLocalPlayer = true;

                Instantiate(_recordPrefab, _parent).Initialize(score, username, isLocalPlayer);
            }
        }
        else
        {
            Debug.Log("Server error. Message: " + result);
            _leaderboardWindow.ReturnToMenu("Server error.");
        }
    }
}
