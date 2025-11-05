using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Button _openPauseButton;
    [SerializeField] private Button _closePauseButton;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private CanvasGroup _pauseCanvasGroup;
    [SerializeField] private float _fadeDuration = 0.3f;
    [SerializeField] private SoundConfiguration _soundConfigurations;

    private const string SoundPrefsKey = "SoundVolume";
    private const string MusicPrefsKey = "MusicVolume";
    private float _sound;
    private float _music;
    private bool _isPaused;

    private void Awake()
    {
        LoadSettings();
        SoundManager.SetSoundVolume(_sound);
        SoundManager.SetMusicVolume(_music);
        SoundManager.Initialize(_soundConfigurations);
        InitializeButtons();
        InitializeSliders();
        UpdateCanvasGroupVisibility();
    }

    private void InitializeButtons()
    {
        _openPauseButton.onClick.AddListener(OpenPause);
        _closePauseButton.onClick.AddListener(ClosePause);
    }

    private void InitializeSliders()
    {
        _soundSlider.minValue = 0f;
        _soundSlider.maxValue = 1f;
        _soundSlider.value = _sound;
        _soundSlider.onValueChanged.AddListener(OnSoundChanged);

        _musicSlider.minValue = 0f;
        _musicSlider.maxValue = 1f;
        _musicSlider.value = _music;
        _musicSlider.onValueChanged.AddListener(OnMusicChanged);
    }

    private void UpdateCanvasGroupVisibility()
    {
        _pauseCanvasGroup.alpha = _isPaused ? 1f : 0f;
        _pauseCanvasGroup.interactable = _isPaused;
        _pauseCanvasGroup.blocksRaycasts = _isPaused;
    }

    public void OpenPause()
    {
        if (_isPaused)
            return;
        SoundManager.PlaySound(SoundType.setting);
        _isPaused = true;
        Time.timeScale = 0f;

        _pauseCanvasGroup.interactable = true;
        _pauseCanvasGroup.blocksRaycasts = true;
        _pauseCanvasGroup.DOFade(1f, _fadeDuration).SetUpdate(true);
    }

    public void ClosePause()
    {
        if (!_isPaused)
            return;

        _isPaused = false;
        Time.timeScale = 1f;

        _pauseCanvasGroup.DOFade(0f, _fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            _pauseCanvasGroup.interactable = false;
            _pauseCanvasGroup.blocksRaycasts = false;
        });
    }

    private void OnSoundChanged(float value)
    {
        _sound = value;
        SoundManager.SetSoundVolume(value);
        SaveSettings();
    }

    private void OnMusicChanged(float value)
    {
        _music = value;
        SoundManager.SetMusicVolume(value);
        SaveSettings();
    }

    private void LoadSettings()
    {
        _sound = PlayerPrefs.GetFloat(SoundPrefsKey, 1f);
        _music = PlayerPrefs.GetFloat(MusicPrefsKey, 1f);
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat(SoundPrefsKey, _sound);
        PlayerPrefs.SetFloat(MusicPrefsKey, _music);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        _soundSlider?.onValueChanged.RemoveListener(OnSoundChanged);
        _musicSlider?.onValueChanged.RemoveListener(OnMusicChanged);
        _openPauseButton?.onClick.RemoveAllListeners();
        _closePauseButton?.onClick.RemoveAllListeners();
    }
}