using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour, IWindow
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    public void OnWindowOpened()
    {
        _musicSlider.value = AudioManager.Instance.MusicVolume;
        _sfxSlider.value = AudioManager.Instance.SFXVolume;
    }
}
