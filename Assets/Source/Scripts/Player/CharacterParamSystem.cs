using System;
using UnityEngine;

public class CharacterParamSystem : MonoBehaviour
{
    [SerializeField] public PlayerConfiguration playerConfiguration;

    // Абсолютные бонусы
    private float speedBonus;
    private float speedProjectileBonus;
    private float healthBonus;
    private float attackSpeedBonus;
    private float attackRangeBonus;
    private int attackDamageBonus;
    private int attachDamageBonus;
    private int critChanceBonus;
    private int armorBonus;
    private int missBonus;
    private int regenerationBonus;
    private int luckyBonus;
    private int moneyPerLevelBonus;
    private int luckyChestBonus;
    private float spawnProjectileCooldownBonus;
    private float projectileAmountBonus;

    // Процентные бонусы (в процентах, например 10 = +10%)
    private float speedPercentBonus;
    private float speedPercentProjectileBonus;
    private float healthPercentBonus;
    private float attackSpeedPercentBonus;
    private float attackRangePercentBonus;
    private float attackDamagePercentBonus;
    private float attachDamagePercentBonus;
    private float critChancePercentBonus;
    private float armorPercentBonus;
    private float missPercentBonus;
    private float regenerationPercentBonus;
    private float luckyPercentBonus;
    private float moneyPerLevelPercentBonus;
    private float luckyChestPercentBonus;
    private float spawnProjectileCooldownPercentBonus;
    private float projectileAmountPercentBonus;

    // Геттеры с бонусами (базовое значение + абсолютный бонус) * (1 + процентный бонус/100)
    public float Speed => (playerConfiguration.DefaultSpeed + speedBonus) * (1 + speedPercentBonus / 100f);

    public float ProjectileSpeed => (playerConfiguration.DefaultProjectileSpeed + speedProjectileBonus) *
                                    (1 + speedPercentBonus / 100f);

    public float AttackSpeed => (playerConfiguration.DefaultAttackSpeed + attackSpeedBonus) *
                                (1 + attackSpeedPercentBonus / 100f);

    public float AttackRange => (playerConfiguration.DefaultAttachRange + attackRangeBonus) *
                                (1 + attackRangePercentBonus / 100f);

    public bool CanAttackMelee => playerConfiguration.CanAttackMelee;
    public bool CanAttackDistance => playerConfiguration.CanAttackDistance;
    public int Health => Mathf.RoundToInt((playerConfiguration.DefaultHealth + healthBonus) *
                                          (1 + healthPercentBonus / 100f));

    public int AttackDamage => Mathf.RoundToInt((playerConfiguration.DefaultAttackDamage + attackDamageBonus) *
                                                (1 + attackDamagePercentBonus / 100f));

    public int AttachDamage => Mathf.RoundToInt((playerConfiguration.DefaultAttachDamage + attachDamageBonus) *
                                                (1 + attachDamagePercentBonus / 100f));

    public int CritChance => Mathf.RoundToInt((playerConfiguration.DefaultCritChance + critChanceBonus) *
                                              (1 + critChancePercentBonus / 100f));

    public int Armor =>
        Mathf.RoundToInt((playerConfiguration.DefaultArmore + armorBonus) * (1 + armorPercentBonus / 100f));

    public int Miss => Mathf.RoundToInt((playerConfiguration.DefaultMiss + missBonus) * (1 + missPercentBonus / 100f));

    public int Regeneration => Mathf.RoundToInt((playerConfiguration.DefaultRegeneration + regenerationBonus) *
                                                (1 + regenerationPercentBonus / 100f));

    public int Lucky =>
        Mathf.RoundToInt((playerConfiguration.DefaultLucky + luckyBonus) * (1 + luckyPercentBonus / 100f));

    public int MoneyPerLevel => Mathf.RoundToInt((playerConfiguration.DefaultMoneyPerLevel + moneyPerLevelBonus) *
                                                 (1 + moneyPerLevelPercentBonus / 100f));

    public int LuckyChest => Mathf.RoundToInt((playerConfiguration.DefaultLuckyChest + luckyChestBonus) *
                                              (1 + luckyChestPercentBonus / 100f));

    public float SpawnProjectileCooldown =>
        (playerConfiguration.DefaultSpawnProjectileCooldown + spawnProjectileCooldownBonus) *
        (1 + spawnProjectileCooldownPercentBonus / 100f);

    public float ProjectileAmount => (playerConfiguration.DefaultProjectileAmount + projectileAmountBonus) *
                                     (1 + projectileAmountPercentBonus / 100f);

    // Методы для добавления абсолютных бонусов
    public void AddSpeedBonus(float bonus) => speedBonus += bonus;
    public void AddAttackSpeedBonus(float bonus) => attackSpeedBonus += bonus;
    public void AddAttackRangeBonus(float bonus) => attackRangeBonus += bonus;
    public void AddAttackDamageBonus(int bonus) => attackDamageBonus += bonus;
    public void AddAttachDamageBonus(int bonus) => attachDamageBonus += bonus;
    public void AddCritChanceBonus(int bonus) => critChanceBonus += bonus;
    public void AddArmorBonus(int bonus) => armorBonus += bonus;
    public void AddMissBonus(int bonus) => missBonus += bonus;
    public void AddRegenerationBonus(int bonus) => regenerationBonus += bonus;
    public void AddLuckyBonus(int bonus) => luckyBonus += bonus;
    public void AddMoneyPerLevelBonus(int bonus) => moneyPerLevelBonus += bonus;
    public void AddLuckyChestBonus(int bonus) => luckyChestBonus += bonus;
    public void AddSpawnProjectileCooldownBonus(float bonus) => spawnProjectileCooldownBonus += bonus;
    public void AddProjectileAmountBonus(float bonus) => projectileAmountBonus += bonus;

    // Методы для добавления процентных бонусов
    public void AddSpeedPercentBonus(float percent) => speedPercentBonus += percent;
    public void AddAttackSpeedPercentBonus(float percent) => attackSpeedPercentBonus += percent;
    public void AddAttackRangePercentBonus(float percent) => attackRangePercentBonus += percent;
    public void AddAttackDamagePercentBonus(float percent) => attackDamagePercentBonus += percent;
    public void AddAttachDamagePercentBonus(float percent) => attachDamagePercentBonus += percent;
    public void AddCritChancePercentBonus(float percent) => critChancePercentBonus += percent;
    public void AddArmorPercentBonus(float percent) => armorPercentBonus += percent;
    public void AddMissPercentBonus(float percent) => missPercentBonus += percent;
    public void AddRegenerationPercentBonus(float percent) => regenerationPercentBonus += percent;
    public void AddLuckyPercentBonus(float percent) => luckyPercentBonus += percent;
    public void AddMoneyPerLevelPercentBonus(float percent) => moneyPerLevelPercentBonus += percent;
    public void AddLuckyChestPercentBonus(float percent) => luckyChestPercentBonus += percent;
    public void AddSpawnProjectileCooldownPercentBonus(float percent) => spawnProjectileCooldownPercentBonus += percent;
    public void AddProjectileAmountPercentBonus(float percent) => projectileAmountPercentBonus += percent;

    public void AddHealthBonus(float bonus)
    {
        healthBonus += bonus;
        FindObjectOfType<PlayerHealth>().RecalculateHealth();
    }

    public void AddHealthPercentBonus(float percent)
    {
        healthPercentBonus += percent;
        FindObjectOfType<PlayerHealth>().RecalculateHealth();
    }

    // Методы для сброса всех бонусов
    public void ResetAllBonuses()
    {
        // Сброс абсолютных бонусов
        speedBonus = attackSpeedBonus = attackRangeBonus = spawnProjectileCooldownBonus = projectileAmountBonus = 0f;
        attackDamageBonus = attachDamageBonus = critChanceBonus = armorBonus =
            missBonus = regenerationBonus = luckyBonus = moneyPerLevelBonus = luckyChestBonus = 0;

        // Сброс процентных бонусов
        speedPercentBonus = attackSpeedPercentBonus = attackRangePercentBonus =
            attackDamagePercentBonus = attachDamagePercentBonus = critChancePercentBonus =
                armorPercentBonus = missPercentBonus = regenerationPercentBonus =
                    luckyPercentBonus = moneyPerLevelPercentBonus = luckyChestPercentBonus =
                        spawnProjectileCooldownPercentBonus = projectileAmountPercentBonus = 0f;
    }
}