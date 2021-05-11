using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private int _gamesToAd = 3;

    private int _gameWithoutAd;
    private WindowsManager _windowsManager;
    private GameManager _gameManager;
    private Coroutine _bannerCoroutine;

    public static AdController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Advertisement.Initialize("3758981", false);
        Advertisement.AddListener(this);
        if (PlayerPrefs.HasKey("GameWithoutAd"))
            _gameWithoutAd = PlayerPrefs.GetInt("GameWithoutAd");

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        if (_bannerCoroutine == null)
            _bannerCoroutine = StartCoroutine(ShowBannerWhenInitialized());
    }

    private void Start()
    {
        _windowsManager = WindowsManager.Instance;
        _gameManager = GameManager.Instance;
    }

    public bool ShowRewardedVideo()
    {
        if (!Advertisement.IsReady())
            return false;

        _gameWithoutAd = 0;
        Advertisement.Show("rewardedVideo");
        return true;
    }

    public bool ShowVideo()
    {
        if (!Advertisement.IsReady())
            return false;

        _gameWithoutAd = 0;
        Advertisement.Show("video");
        return true;
    }

    private IEnumerator ShowBannerWhenInitialized()
    {
        while(!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(.5f);
        }
        Advertisement.Banner.Show("banner");
    }


    public bool IsAdReady() => Advertisement.IsReady();
    public bool ShouldShowAd()
    {
        if (!Advertisement.IsReady())
            return false;

        if (_gameWithoutAd >= _gamesToAd)
        {
            ShowVideo();
            return true;
        }

        _gameWithoutAd++;
        PlayerPrefs.SetInt("GameWithoutAd", _gameWithoutAd);
        return false;
    }

    public void OnUnityAdsDidError(string message)
    {
        _windowsManager.OpenWindow(WindowsManager.WindowType.Menu);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == "rewardedVideo")
        {
            if (showResult == ShowResult.Finished)
                _gameManager.Revive();
            else
                _windowsManager.OpenWindow(WindowsManager.WindowType.Lost);
            
        }
        else if (placementId == "video")
        {
            _gameManager.Play();
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsReady(string placementId)
    {

    }
}
