using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    public enum WindowType { Game, Lost, Revive, Menu, Settings, Leaderboard };

    [System.Serializable]
    public class Window
    {
        public WindowType windowType;
        public GameObject windowObject;
    }

    [SerializeField] private Window[] _windows;

    public static WindowsManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public void OpenWindow(WindowType windowType)
    {
        foreach(var window in _windows)
        {
            if (window.windowType == windowType)
            {
                window.windowObject.SetActive(true);
                window.windowObject.GetComponent<IWindow>()?.OnWindowOpened();
            }
            else
            {
                window.windowObject.SetActive(false);
            }
        }
    }

}
