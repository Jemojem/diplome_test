using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _loseButton;
    [SerializeField] private GameAnalytics _gameAnalytics;

    [SerializeField] private TMP_Text _amountMoney;
    [SerializeField] private TMP_Text _damagePlayer;
    [SerializeField] private TMP_Text _damagePets;
    [SerializeField] private TMP_Text _amountPets;
    [SerializeField] private TMP_Text _killedEnemy;
    [SerializeField] private CanvasGroup _gameCanvas;
    private bool _isShowed;

    public void Show()
    {
        if(_isShowed) return;
        _isShowed = true;
        _amountMoney.text = _gameAnalytics.TakedMoney.ToString();
        _damagePlayer.text ="Урон игрока "+ _gameAnalytics.DamagePlayer.ToString();
        _damagePets.text ="Урон петов "+ _gameAnalytics.DamagePets.ToString();
        _amountPets.text =_gameAnalytics.AmountPets.ToString();
        _killedEnemy.text ="Убито врагов "+ _gameAnalytics.KilledEnemy.ToString();
        gameObject.SetActive(true);
        var currentValue = 1f;
        _gameCanvas.DOFade(0, 1f).SetUpdate(true);
        DOTween.To(() => currentValue, x => currentValue = x, 0f, 1f).OnUpdate(() => { Time.timeScale = currentValue; })
            .SetUpdate(true).OnComplete(() =>
            {
                _loseButton.onClick.AddListener(() =>
                {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                });
                _canvasGroup.DOFade(1, 0.4f).SetUpdate(true);
            });
    }
}