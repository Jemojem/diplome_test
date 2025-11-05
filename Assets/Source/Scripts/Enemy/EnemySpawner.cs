using System.Linq;
using DG.Tweening;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Настройки спауна")] 
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private GameObject robotPrefab;

    [SerializeField] private CharacterParamSystem characterParamSystem;
    [SerializeField] private GameParamSystem gameParamSystem;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float minDistanceToPlayer = 3f;
    [SerializeField] private Transform player;
    [SerializeField] private int maxTries = 30;
    [SerializeField] private ShopSystem shopSystem;
    [SerializeField] private GameManager gameManager;
    private int spawnIndex = 0;
    [SerializeField] private float lateralJitter = 1f;

    public void Drop()
    {
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || player == null) return;
        Vector2 spawnPosition = GetDirectionalSpawnPosition();
        var prefab = enemyPrefab;
        if (gameManager.currentLevel > 1)
        {
            prefab = Random.Range(0,10) < 2 ? magicPrefab : prefab;
        }
        if (gameManager.currentLevel > 2)
        {
            prefab = Random.Range(0,10) < 3 ? robotPrefab : prefab;
        }
        var enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);
        var enemyState = enemy.GetComponent<EnemyStateMachine>();
        if (gameParamSystem == null)
            gameParamSystem = FindObjectOfType<GameParamSystem>();
        enemyState.Initialize(player, characterParamSystem, gameParamSystem);
    }

    public void SpawnShop()
    {
        Instantiate(shopSystem.gameObject, player.position, Quaternion.identity);
    }

    private Vector2 GetDirectionalSpawnPosition()
    {
        Vector2 center = transform.position;
        Vector2 dir;

        switch (spawnIndex % 4)
        {
            case 0:
                dir = Vector2.left;
                break; // слева
            case 1:
                dir = Vector2.right;
                break; // справа
            case 2:
                dir = Vector2.up;
                break; // сверху
            default:
                dir = Vector2.down;
                break; // снизу
        }

        // перпендикуляр для небольшого смещения (чтобы позиции не были абсолютно одинаковыми)
        Vector2 perp = new Vector2(-dir.y, dir.x);
        float jitter = Random.Range(-lateralJitter, lateralJitter);

        Vector2 spawnPos = center + dir * spawnRadius + perp * jitter;

        // увеличиваем индекс для следующего спавна (чтобы цикл продолжался)
        spawnIndex = (spawnIndex + 1) % 4;

        // если случайно получившаяся позиция слишком близко к игроку — сдвинем её на minDistanceToPlayer от игрока
        if (player != null && Vector2.Distance(spawnPos, player.position) < minDistanceToPlayer)
        {
            Vector2 fromPlayer = (spawnPos - (Vector2)player.position).normalized;
            if (fromPlayer == Vector2.zero) fromPlayer = dir; // защита на случай совпадения точек
            spawnPos = (Vector2)player.position + fromPlayer * minDistanceToPlayer;
        }

        return spawnPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, minDistanceToPlayer);
        }
    }
}