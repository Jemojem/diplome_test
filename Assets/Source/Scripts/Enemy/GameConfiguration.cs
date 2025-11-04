using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create GameConfiguration", fileName = "GameConfiguration", order = 0)]
public class GameConfiguration : ScriptableObject
{
    [field: Header("Shop")]
    [field: SerializeField] public float DefaultShopPriceMultiplier { get; private set; } = 1f;

    [field: Header("Enemy")]
    [field: SerializeField] public float DefaultEnemyDamageMultiplier { get; private set; } = 1f;
    [field: SerializeField] public float DefaultEnemyHealthMultiplier { get; private set; } = 1f;
    [field: SerializeField] public float DefaultEnemySpeedMultiplier { get; private set; } = 1f;
    [field: SerializeField] public float DefaultEnemySpawnCooldown { get; private set; } = 1f;

    [field: Header("Wave")]
    [field: SerializeField] public int DefaultWaveCount { get; private set; } = 5;
    [field: SerializeField] public float DefaultWaveDuration { get; private set; } = 60f;

    [field: Header("Loot")]
    [field: SerializeField] public int DefaultLuckyChest { get; private set; } = 10;
    [field: SerializeField] public int DefaultLevelReward { get; private set; } = 100;

    [field: Header("Level Progression")]
    [field: SerializeField] public LevelProgressionSettings LevelProgression { get; private set; }

    [System.Serializable]
    public class LevelProgressionSettings
    {
        [field: Header("Shop")]
        [field: SerializeField] public ParameterIncrease ShopPriceMultiplierIncrease { get; private set; }

        [field: Header("Enemy")]
        [field: SerializeField] public ParameterIncrease EnemyDamageMultiplierIncrease { get; private set; }
        [field: SerializeField] public ParameterIncrease EnemyHealthMultiplierIncrease { get; private set; }
        [field: SerializeField] public ParameterIncrease EnemySpeedMultiplierIncrease { get; private set; }
        [field: SerializeField] public ParameterIncrease EnemySpawnCooldownIncrease { get; private set; }

        [field: Header("Wave")]
        [field: SerializeField] public ParameterIncrease WaveCountIncrease { get; private set; }
        [field: SerializeField] public ParameterIncrease WaveDurationIncrease { get; private set; }

        [field: Header("Loot")]
        [field: SerializeField] public ParameterIncrease LuckyChestIncrease { get; private set; }
        [field: SerializeField] public ParameterIncrease LevelRewardIncrease { get; private set; }
    }

    [System.Serializable]
    public class ParameterIncrease
    {
        [field: SerializeField] public IncreaseType Type { get; private set; } = IncreaseType.Additive;
        [field: SerializeField] public float Value { get; private set; } = 0f;

        public enum IncreaseType
        {
            Additive,
            Multiplicative
        }
    }
}

