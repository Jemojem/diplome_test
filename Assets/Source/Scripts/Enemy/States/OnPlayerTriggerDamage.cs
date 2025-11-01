using UnityEngine;

public class OnPlayerTriggerDamage : MonoBehaviour
{
    [SerializeField] private EnemyConfiguration _enemyConfiguration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(_enemyConfiguration.Damage);
            Destroy(gameObject);
        }
    }
}