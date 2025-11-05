using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    [SerializeField] private Button _openPauseButton;
    [SerializeField] private Button _closePauseButton;
    [SerializeField] private Button _restartPauseButton;
    [SerializeField] private CanvasGroup _pauseCanvasGroup;
    [SerializeField] private float _fadeDuration = 0.3f;
    private bool _isPaused;

    private void Awake()
    {
        InitializeButtons();
        UpdateCanvasGroupVisibility();
    }

    private void InitializeButtons()
    {
        _openPauseButton.onClick.AddListener(OpenPause);
        _closePauseButton.onClick.AddListener(ClosePause);
        _restartPauseButton.onClick.AddListener(Restart);
    }

    private void UpdateCanvasGroupVisibility()
    {
        _pauseCanvasGroup.alpha = _isPaused ? 1f : 0f;
        _pauseCanvasGroup.interactable = _isPaused;
        _pauseCanvasGroup.blocksRaycasts = _isPaused;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); Time.timeScale = 1f;
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

    private void OnDestroy()
    {
        _openPauseButton?.onClick.RemoveAllListeners();
        _closePauseButton?.onClick.RemoveAllListeners();
        _restartPauseButton?.onClick.RemoveAllListeners();
    }
}