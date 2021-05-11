using TMPro;
using UnityEngine;

public class CreateUser : MonoBehaviour
{
    [SerializeField] private int _maxLength = 16;
    [SerializeField] private int _minLength = 3;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private LeaderboardWindow _leaderboardWindow;

    private LeaderboardManager _leaderboardManager;

    private void Awake()
    {
        _inputField.characterLimit = _maxLength;
    }

    private void Start()
    {
        _leaderboardManager = LeaderboardManager.Instance;
    }

    public void SubmitName()
    {
        if (_inputField.text.Length < _minLength)
        {
            Debug.Log("Username can't be less then " + _minLength);
            ToastManager.Instance.Toast("Username can't be less then " + _minLength, ToastManager.MessageType.Error);
        }
        else if (_inputField.text.Length > _maxLength)
        {
            Debug.Log("Username can't be greater then " + _maxLength);
            ToastManager.Instance.Toast("Username can't be greater then " + _maxLength, ToastManager.MessageType.Error);
        }
        else
        {
            _leaderboardManager.CreateDatabaseUser(_inputField.text, CreateDatabaseUserCallback);
            PlayerPrefs.SetString("Username", _inputField.text);
        }
    }

    private void CreateDatabaseUserCallback(bool state, string result)
    {
        if (state)
        {
            if (result == "1")
            {
                _leaderboardWindow.OpenLeaderboardWindow();
            }
            else
            {
                Debug.Log("Server error. Message: " + result);
            }
        }
        else
        {
            Debug.Log("Create database user error. Message: " + result);
            _leaderboardWindow.ReturnToMenu("Unable to connect to server.");
        }
    }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }
}
