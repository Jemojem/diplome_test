using UnityEngine;

public class OnPlayerTriggerDamage : MonoBehaviour
{
    [SerializeField] private EnemyConfiguration _enemyConfiguration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var gameParamSystem = FindObjectOfType<GameParamSystem>();
            var damageMultiplier = gameParamSystem != null ? gameParamSystem.EnemyDamageMultiplier : 1f;
            var damage = Mathf.RoundToInt(_enemyConfiguration.Damage * damageMultiplier);
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}