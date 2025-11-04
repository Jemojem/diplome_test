using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create PlayerConfiguration", fileName = "PlayerConfiguration", order = 0)]
public class PlayerConfiguration : ScriptableObject
{
    [field: SerializeField] public float DefaultSpeed { get; private set; }
    [field: Header("Projectile")]
    [field: SerializeField] public float DefaultProjectileSpeed { get; private set; }
    [field: SerializeField] public float DefaultSpawnProjectileCooldown { get; private set; }
    [field: SerializeField] public float DefaultProjectileAmount { get; private set; }
    [field: Header("Health")]
    [field: SerializeField] public float DefaultHealth { get; private set; }
    [field: Header("Attack and damage")]
    [field: SerializeField] public float DefaultAttackSpeed { get; private set; }
    [field: SerializeField] public float DefaultAttachRange { get; private set; }
    [field: SerializeField] public int DefaultAttackDamage { get; private set; }
    [field: SerializeField] public int DefaultAttachDamage { get; private set; }
    [field: SerializeField] public int DefaultCritChance { get; private set; }
    [field: Header("Armore and miss")]
    [field: SerializeField] public int DefaultArmore { get; private set; }
    [field: SerializeField] public int DefaultMiss { get; private set; }
    [field: Header("Regeneration and lucky")]
    [field: SerializeField] public int DefaultRegeneration { get; private set; }
    [field: SerializeField] public int DefaultLucky { get; private set; }
    [field: SerializeField] public int DefaultMoneyPerLevel { get; private set; }
    public bool CanAttackMelee = true;
    public bool CanAttackDistance = true;
}