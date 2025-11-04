using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyConfiguration _enemyConfiguration;

    public void Damage()
    {
        var gameParamSystem = FindObjectOfType<GameParamSystem>();
        var damageMultiplier = gameParamSystem != null ? gameParamSystem.EnemyDamageMultiplier : 1f;
        var damage = Mathf.RoundToInt(_enemyConfiguration.Damage * damageMultiplier);
        FindAnyObjectByType<PlayerHealth>().TakeDamage(damage);
    }
}