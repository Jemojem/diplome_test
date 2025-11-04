using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private GameParamSystem gameParamSystem;

    [Header("UI Elements")] [SerializeField]
    private TMP_Text waveText;

    [SerializeField] private TMP_Text timerText;

    [Header("Animation Settings")] [SerializeField]
    private float fadeDuration = 0.5f;

    public int currentLevel;

    [SerializeField] private float waveDisplayTime = 1.5f;
    private Tween currentTween;

    private void Awake()
    {
        waveText.alpha = 0f;
        timerText.alpha = 0f;
        if (gameParamSystem == null)
            gameParamSystem = FindObjectOfType<GameParamSystem>();
    }

    public void StartWave(int waveNumber)
    {
        currentTween?.Kill();

        waveText.text = $"ВОЛНА {waveNumber}";

        var waveDuration = gameParamSystem != null ? gameParamSystem.WaveDuration : 60f;
        Sequence seq = DOTween.Sequence();
        SetTimer(waveDuration);
        // Появление волны с пульсацией
        seq.AppendCallback(() =>
        {
            waveText.alpha = 1f;
            waveText.transform.localScale = Vector3.zero;
        });
        seq.Append(waveText.transform.DOScale(1f, 0.6f).SetEase(Ease.OutBack));
        seq.AppendCallback(() =>
        {
            timerText.alpha = 1f;
            timerText.transform.localScale = Vector3.zero;
        });
        seq.Append(timerText.transform.DOScale(1f, 0.6f).SetEase(Ease.OutBack));
        seq.OnComplete(() =>
        {
            StartCoroutine(SpawnEnemy());
            StartCoroutine(TimerCoroutine());
        });
        currentTween = seq;
    }

    public void HideTexts()
    {
        currentTween?.Kill();
        Sequence seq = DOTween.Sequence();
        seq.Append(waveText.DOFade(0f, fadeDuration));
        seq.Join(timerText.DOFade(0f, fadeDuration));
    }

    public void SetTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StartGame()
    {
        currentLevel++;
        if (gameParamSystem != null)
            gameParamSystem.ApplyLevelProgression();
        StartWave(currentLevel);
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            var spawnCooldown = gameParamSystem != null ? gameParamSystem.EnemySpawnCooldown : 1f;
            yield return new WaitForSeconds(spawnCooldown);
            spawner.SpawnEnemy();
        }
    }

    private IEnumerator TimerCoroutine()
    {
        var waveDuration = gameParamSystem != null ? gameParamSystem.WaveDuration : 60f;
        var waveDurationInt = Mathf.RoundToInt(waveDuration);
        for (int i = 0; i < waveDurationInt; i++)
        {
            SetTimer(waveDuration - i);
            yield return new WaitForSeconds(1f);
        }

        HideTexts();
        StopAllCoroutines();
        foreach (var enemyState in FindObjectsByType<EnemyStateMachine>(FindObjectsSortMode.None))
        {
            enemyState.TakeDamage(1000,true);
        }

        var levelReward = gameParamSystem != null ? gameParamSystem.LevelReward : 0;
        if (levelReward > 0 && CoinCounter.Instance != null)
        {
            CoinCounter.Instance.SpawnFlyingCoin(Vector3.zero, levelReward);
        }

        spawner.SpawnShop();
    }
}