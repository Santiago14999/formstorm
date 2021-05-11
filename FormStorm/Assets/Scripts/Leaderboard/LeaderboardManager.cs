using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    public delegate void WebRequestCallback(bool state, string result);

    private GameManager _gameManager;

    public static LeaderboardManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void TryUploadNewBest(int score)
    {
        StartCoroutine(TryUploadCoroutine(score));
    }

    IEnumerator TryUploadCoroutine(int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", _gameManager.DeviceID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.3/(2)/getrequest.php", form))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError && !www.isHttpError)
            {
                if (www.downloadHandler.text == "1")
                {
                    WWWForm uploadForm = new WWWForm();
                    uploadForm.AddField("user_id", _gameManager.DeviceID);
                    uploadForm.AddField("score", score);

                    using (UnityWebRequest wwwUpload = UnityWebRequest.Post("http://192.168.0.3/(2)/uploadscore.php", uploadForm))
                    {
                        yield return wwwUpload.SendWebRequest();
                    }
                }
            }
        }
    }

    public void HasId(WebRequestCallback callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", _gameManager.DeviceID);

        StartCoroutine(PostRequestCoroutine("http://e92313mj.beget.tech/getrequest.php", form, callback));
    }

    public void LoadBoard(WebRequestCallback callback)
    {
        StartCoroutine(GetRequestCoroutine("http://e92313mj.beget.tech/leaderboard.php", callback));
    }

    public void CreateDatabaseUser(string username, WebRequestCallback callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", _gameManager.DeviceID);
        form.AddField("username", username);
        form.AddField("score", PlayerPrefs.GetInt("Score"));

        StartCoroutine(PostRequestCoroutine("http://e92313mj.beget.tech/uploaduser.php", form, callback));
    }

    public void UploadScore(WebRequestCallback callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", _gameManager.DeviceID);
        // Get score from score controller
        int score = PlayerPrefs.GetInt("Score");
        form.AddField("score", score);

        StartCoroutine(PostRequestCoroutine("http://e92313mj.beget.tech/uploadscore.php", form, callback));
    }

    private IEnumerator PostRequestCoroutine(string uri, WWWForm form, WebRequestCallback callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            www.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                callback?.Invoke(false, www.error);
            }
            else
            {
                callback?.Invoke(true, www.downloadHandler.text);
            }
        }
    }

    private IEnumerator GetRequestCoroutine(string uri, WebRequestCallback callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            www.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                callback?.Invoke(false, www.error);
            }
            else
            {
                callback?.Invoke(true, www.downloadHandler.text);
            }
        }
    }
}
