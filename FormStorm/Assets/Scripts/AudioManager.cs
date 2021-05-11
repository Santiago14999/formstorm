using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private AudioClip _popSound;
    [SerializeField] private AudioClip _spawnSound;
    [SerializeField] private AudioClip _lostSound;
    [SerializeField] private AudioClip _newRecordSound;

    public float MusicVolume { get; private set; }
    public float SFXVolume { get; private set; }

    private AudioSource _source;
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadMusicVolume();
        LoadSFXVolume();
    }

    public void SetMusicVolume(float value)
    {
        _mixer.SetFloat("musicVol", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
        MusicVolume = value;
    }

    public void LoadMusicVolume()
    {
        float value = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : .5f;
        _mixer.SetFloat("musicVol", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        MusicVolume = value;
    }

    public void SetSFXVolume(float value)
    {
        _mixer.SetFloat("sfxVol", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
        SFXVolume = value;
    }

    public void LoadSFXVolume()
    {
        float value = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : .5f;
        _mixer.SetFloat("sfxVol", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        SFXVolume = value;
    }

    public void PlayPopSound()
    {
        _source.pitch = Random.Range(0.8f, 1.2f);
        _source.clip = _popSound;
        _source.Play();
    }

    public void PlaySpawnSound()
    {
        _source.pitch = Random.Range(0.6f, 1.2f);
        _source.clip = _spawnSound;
        _source.Play();
    }

    public void PlayLostSound()
    {

        _source.pitch = 1f;
        _source.clip = _lostSound;
        _source.Play();
    }

    public void PlayNewRecordSound()
    {
        _source.pitch = 1f;
        _source.clip = _newRecordSound;
        _source.Play();
    }
}
