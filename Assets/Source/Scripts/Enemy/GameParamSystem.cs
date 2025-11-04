using UnityEngine;

public class GameParamSystem : MonoBehaviour
{
    [SerializeField] public GameConfiguration gameConfiguration;

    // Абсолютные бонусы
    private float shopPriceMultiplierBonus;
    private float enemyDamageMultiplierBonus;
    private float enemyHealthMultiplierBonus;
    private float enemySpeedMultiplierBonus;
    private float enemySpawnCooldownBonus;
    private int waveCountBonus;
    private float waveDurationBonus;
    private int luckyChestBonus;
    private int levelRewardBonus;

    // Процентные бонусы (в процентах, например 10 = +10%)
    private float shopPriceMultiplierPercentBonus;
    private float enemyDamageMultiplierPercentBonus;
    private float enemyHealthMultiplierPercentBonus;
    private float enemySpeedMultiplierPercentBonus;
    private float enemySpawnCooldownPercentBonus;
    private float waveCountPercentBonus;
    private float waveDurationPercentBonus;
    private float luckyChestPercentBonus;
    private float levelRewardPercentBonus;

    // Геттеры с бонусами (базовое значение + абсолютный бонус) * (1 + процентный бонус/100)
    public float ShopPriceMultiplier => (gameConfiguration.DefaultShopPriceMultiplier + shopPriceMultiplierBonus) *
                                        (1 + shopPriceMultiplierPercentBonus / 100f);

    public float EnemyDamageMultiplier => (gameConfiguration.DefaultEnemyDamageMultiplier + enemyDamageMultiplierBonus) *
                                          (1 + enemyDamageMultiplierPercentBonus / 100f);

    public float EnemyHealthMultiplier => (gameConfiguration.DefaultEnemyHealthMultiplier + enemyHealthMultiplierBonus) *
                                          (1 + enemyHealthMultiplierPercentBonus / 100f);

    public float EnemySpeedMultiplier => (gameConfiguration.DefaultEnemySpeedMultiplier + enemySpeedMultiplierBonus) *
                                        (1 + enemySpeedMultiplierPercentBonus / 100f);

    public float EnemySpawnCooldown => (gameConfiguration.DefaultEnemySpawnCooldown + enemySpawnCooldownBonus) *
                                       (1 + enemySpawnCooldownPercentBonus / 100f);

    public int WaveCount => Mathf.RoundToInt((gameConfiguration.DefaultWaveCount + waveCountBonus) *
                                            (1 + waveCountPercentBonus / 100f));

    public float WaveDuration => (gameConfiguration.DefaultWaveDuration + waveDurationBonus) *
                                (1 + waveDurationPercentBonus / 100f);

    public int LuckyChest => Mathf.RoundToInt((gameConfiguration.DefaultLuckyChest + luckyChestBonus) *
                                              (1 + luckyChestPercentBonus / 100f));

    public int LevelReward => Mathf.RoundToInt((gameConfiguration.DefaultLevelReward + levelRewardBonus) *
                                              (1 + levelRewardPercentBonus / 100f));

    // Методы для добавления абсолютных бонусов
    public void AddShopPriceMultiplierBonus(float bonus) => shopPriceMultiplierBonus += bonus;
    public void AddEnemyDamageMultiplierBonus(float bonus) => enemyDamageMultiplierBonus += bonus;
    public void AddEnemyHealthMultiplierBonus(float bonus) => enemyHealthMultiplierBonus += bonus;
    public void AddEnemySpeedMultiplierBonus(float bonus) => enemySpeedMultiplierBonus += bonus;
    public void AddEnemySpawnCooldownBonus(float bonus) => enemySpawnCooldownBonus += bonus;
    public void AddWaveCountBonus(int bonus) => waveCountBonus += bonus;
    public void AddWaveDurationBonus(float bonus) => waveDurationBonus += bonus;
    public void AddLuckyChestBonus(int bonus) => luckyChestBonus += bonus;
    public void AddLevelRewardBonus(int bonus) => levelRewardBonus += bonus;

    // Методы для добавления процентных бонусов
    public void AddShopPriceMultiplierPercentBonus(float percent) => shopPriceMultiplierPercentBonus += percent;
    public void AddEnemyDamageMultiplierPercentBonus(float percent) => enemyDamageMultiplierPercentBonus += percent;
    public void AddEnemyHealthMultiplierPercentBonus(float percent) => enemyHealthMultiplierPercentBonus += percent;
    public void AddEnemySpeedMultiplierPercentBonus(float percent) => enemySpeedMultiplierPercentBonus += percent;
    public void AddEnemySpawnCooldownPercentBonus(float percent) => enemySpawnCooldownPercentBonus += percent;
    public void AddWaveCountPercentBonus(float percent) => waveCountPercentBonus += percent;
    public void AddWaveDurationPercentBonus(float percent) => waveDurationPercentBonus += percent;
    public void AddLuckyChestPercentBonus(float percent) => luckyChestPercentBonus += percent;
    public void AddLevelRewardPercentBonus(float percent) => levelRewardPercentBonus += percent;

    // Методы для сброса всех бонусов
    public void ResetAllBonuses()
    {
        shopPriceMultiplierBonus = enemyDamageMultiplierBonus = enemyHealthMultiplierBonus =
            enemySpeedMultiplierBonus = enemySpawnCooldownBonus = waveDurationBonus = 0f;
        waveCountBonus = luckyChestBonus = levelRewardBonus = 0;

        shopPriceMultiplierPercentBonus = enemyDamageMultiplierPercentBonus = enemyHealthMultiplierPercentBonus =
            enemySpeedMultiplierPercentBonus = enemySpawnCooldownPercentBonus = waveCountPercentBonus =
                waveDurationPercentBonus = luckyChestPercentBonus = levelRewardPercentBonus = 0f;
    }

    // Применение увеличения параметров при повышении уровня
    public void ApplyLevelProgression()
    {
        if (gameConfiguration?.LevelProgression == null)
            return;

        var progression = gameConfiguration.LevelProgression;

        ApplyParameterIncrease(progression.ShopPriceMultiplierIncrease, ref shopPriceMultiplierBonus, ref shopPriceMultiplierPercentBonus);
        ApplyParameterIncrease(progression.EnemyDamageMultiplierIncrease, ref enemyDamageMultiplierBonus, ref enemyDamageMultiplierPercentBonus);
        ApplyParameterIncrease(progression.EnemyHealthMultiplierIncrease, ref enemyHealthMultiplierBonus, ref enemyHealthMultiplierPercentBonus);
        ApplyParameterIncrease(progression.EnemySpeedMultiplierIncrease, ref enemySpeedMultiplierBonus, ref enemySpeedMultiplierPercentBonus);
        ApplyParameterIncrease(progression.EnemySpawnCooldownIncrease, ref enemySpawnCooldownBonus, ref enemySpawnCooldownPercentBonus);
        ApplyParameterIncrease(progression.WaveCountIncrease, ref waveCountBonus, ref waveCountPercentBonus);
        ApplyParameterIncrease(progression.WaveDurationIncrease, ref waveDurationBonus, ref waveDurationPercentBonus);
        ApplyParameterIncrease(progression.LuckyChestIncrease, ref luckyChestBonus, ref luckyChestPercentBonus);
        ApplyParameterIncrease(progression.LevelRewardIncrease, ref levelRewardBonus, ref levelRewardPercentBonus);
    }

    private void ApplyParameterIncrease(GameConfiguration.ParameterIncrease increase, ref float bonus, ref float percentBonus)
    {
        if (increase == null)
            return;

        if (increase.Type == GameConfiguration.ParameterIncrease.IncreaseType.Additive)
            bonus += increase.Value;
        else
            percentBonus += increase.Value;
    }

    private void ApplyParameterIncrease(GameConfiguration.ParameterIncrease increase, ref int bonus, ref float percentBonus)
    {
        if (increase == null)
            return;

        if (increase.Type == GameConfiguration.ParameterIncrease.IncreaseType.Additive)
            bonus += Mathf.RoundToInt(increase.Value);
        else
            percentBonus += increase.Value;
    }
}

