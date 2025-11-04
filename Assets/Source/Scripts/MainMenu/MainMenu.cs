using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CameraController2D _cameraController;
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup _mainMenuCanvasGroupMainGame;
    [SerializeField] private CanvasGroup _selectGameCanvasGroup;
    [SerializeField] private Button _closeSelectGameButton;
    [SerializeField] private Button _startGame;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SelectLevelManager _selectLevelManager;

    private void Awake()
    {
        _selectGameCanvasGroup.gameObject.SetActive(false);
        _selectLevelManager.Initialize();
        _closeSelectGameButton.onClick.AddListener(() => _selectGameCanvasGroup.DOFade(0, 0.3f).OnComplete(() =>
        {
            _selectGameCanvasGroup.gameObject.SetActive(false);
        }));
        _selectLevelManager.Play += () =>
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_mainMenuCanvasGroup.DOFade(0, 1f));
            float valFloat = _mainCamera.orthographicSize;
            sequence.Append(DOTween.To(() => valFloat, x => valFloat = x, 7, 1f).OnUpdate(() =>
            {
                _mainCamera.orthographicSize = valFloat;
            }));
            sequence.AppendCallback(() =>
            {
                _mainMenuCanvasGroup.gameObject.SetActive(false);
                _mainCanvas.gameObject.SetActive(true);
                _cameraController.enabled = true;
            });
            sequence.Append(
                _mainMenuCanvasGroupMainGame.DOFade(1, 0.6f).OnComplete(() => { _gameManager.StartGame(); }));
        };
        _startGame.onClick.AddListener(() =>
        {
            _selectGameCanvasGroup.gameObject.SetActive(true);
            _selectGameCanvasGroup.DOFade(1, 0.3f).OnComplete(() => { });
        });
    }
}

[Serializable]
public class SelectLevelManager
{
    [SerializeField] private GameConfiguration _easy;
    [SerializeField] private GameConfiguration _medium;
    [SerializeField] private GameConfiguration _hard;
    [SerializeField] private GameParamSystem _paramSystem;
    [SerializeField] private Button _easyb;
    [SerializeField] private Button _mediumb;
    [SerializeField] private Button _hardb;

    public event Action Play;

    public void Initialize()
    {
        _easyb.onClick.AddListener(() =>
        {
            _paramSystem.gameConfiguration = _easy;
            Play?.Invoke();
        });
        _mediumb.onClick.AddListener(() =>
        {
            _paramSystem.gameConfiguration = _medium;
            Play?.Invoke();
        });
        _hardb.onClick.AddListener(() =>
        {
            _paramSystem.gameConfiguration = _hard;
            Play?.Invoke();
        });
    }
}