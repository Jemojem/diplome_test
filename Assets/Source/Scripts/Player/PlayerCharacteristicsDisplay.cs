using TMPro;
using UnityEngine;

public class PlayerCharacteristicsDisplay : MonoBehaviour
{
    [SerializeField] private PlayerConfiguration _playerConfiguration;
    [SerializeField] private TMP_Text _characteristicsText;

    public void UpdateCharacteristics()
    {
        if (_playerConfiguration == null || _characteristicsText == null)
            return;

        var description = BuildCharacteristicsDescription();
        _characteristicsText.text = description;
    }

    private string BuildCharacteristicsDescription()
    {
        var config = _playerConfiguration;
        var lines = new System.Text.StringBuilder();

        if (config.DefaultSpeed != 0)
            lines.AppendLine($"Скорость: {config.DefaultSpeed}");
        
        if (config.DefaultHealth != 0)
            lines.AppendLine($"Здоровье: {config.DefaultHealth}");
        
        if (config.DefaultProjectileSpeed != 0)
            lines.AppendLine($"Скорость снаряда: {config.DefaultProjectileSpeed}");
        
        if (config.DefaultSpawnProjectileCooldown != 0)
            lines.AppendLine($"Кудаун создания снаряда: {config.DefaultSpawnProjectileCooldown}");
        
        if (config.DefaultProjectileAmount != 0)
            lines.AppendLine($"Количество снарядов: {config.DefaultProjectileAmount}");
        
        if (config.DefaultAttackSpeed != 0)
            lines.AppendLine($"Скорость атаки: {config.DefaultAttackSpeed}");
        
        if (config.DefaultAttachRange != 0)
            lines.AppendLine($"Дистанция атаки: {config.DefaultAttachRange}");
        
        if (config.DefaultAttackDamage != 0)
            lines.AppendLine($"Урон атаки: {config.DefaultAttackDamage}");
        
        if (config.DefaultAttachDamage != 0)
            lines.AppendLine($"Урон ближнего боя: {config.DefaultAttachDamage}");
        
        if (config.DefaultCritChance != 0)
            lines.AppendLine($"Шанс критического удара: {config.DefaultCritChance}%");
        
        if (config.DefaultArmore != 0)
            lines.AppendLine($"Броня: {config.DefaultArmore}");
        
        if (config.DefaultMiss != 0)
            lines.AppendLine($"Шанс уклонения: {config.DefaultMiss}%");
        
        if (config.DefaultRegeneration != 0)
            lines.AppendLine($"Регенерация: {config.DefaultRegeneration}");
        
        if (config.DefaultLucky != 0)
            lines.AppendLine($"Удача: {config.DefaultLucky}");
        
        if (config.DefaultMoneyPerLevel != 0)
            lines.AppendLine($"Деньги за уровень: {config.DefaultMoneyPerLevel}");
        
        if (config.DefaultLuckyChest != 0)
            lines.AppendLine($"Удача сундука: {config.DefaultLuckyChest}");
        
        if (config.CanAttackMelee)
            lines.AppendLine("Может атаковать ближний бой");
        
        if (config.CanAttackDistance)
            lines.AppendLine("Может атаковать дистанционно");

        return lines.ToString();
    }

    private void OnValidate()
    {
        if (_playerConfiguration != null && _characteristicsText != null)
            UpdateCharacteristics();
    }
}
