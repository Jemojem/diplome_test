using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Prefab / Targeting")] public GameObject projectilePrefab;
    public string enemyTag = "Enemy";
    [SerializeField] private CharacterParamSystem characterParamSystem;
    [Header("Firing / Ammo")] public int availableProjectiles => (int)characterParamSystem.ProjectileAmount;
    public float cooldown => characterParamSystem.SpawnProjectileCooldown;

    [Header("Curve Parameters")] public float minAmplitude = 0.3f;
    public float maxAmplitude = 1.2f;
    public float minFrequency = 1f;
    public Animator animator;
    public float maxFrequency = 3f;

    private float lastFireTime = -Mathf.Infinity;

    void Update()
    {
        if (Time.time - lastFireTime >= cooldown)
        {
            TryFire();
            lastFireTime = Time.time;
        }
    }

    void TryFire()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab not assigned");
            return;
        }

        // Собираем всех врагов
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies == null || enemies.Length == 0) return;

        // Сортируем врагов по расстоянию до этой турели (ближайшие первыми)
        var sorted = enemies
            .Where(e => e != null)
            .OrderBy(e => Vector3.SqrMagnitude(e.transform.position - transform.position))
            .ToArray();

        if (sorted.Length == 0) return;
        int ammoToUse = Mathf.Clamp(availableProjectiles, 0, availableProjectiles);
        if (ammoToUse <= 0) return;
        if (characterParamSystem.CanAttackDistance)
        {
            animator.Play("AttackDistance", 2);
            DOVirtual.DelayedCall(0.6f, () =>
            {
                if (ammoToUse > 0) SoundManager.PlaySound(SoundType.shot);
                for (int i = 0; i < ammoToUse; i++)
                {
                    GameObject target = sorted[i % sorted.Length]; // round-robin по списку ближайших
                    if (target != null)
                    {
                        SpawnProjectileTowards(target.transform);
                    }
                }
            });
        }
    }

    void SpawnProjectileTowards(Transform target)
    {
        GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        CurvedProjectile proj = go.GetComponent<CurvedProjectile>();
        if (proj == null)
        {
            Debug.LogWarning("Prefab не содержит компонент CurvedProjectile. Добавьте его для корректной работы.");
            Destroy(go);
            return;
        }

        proj.Initialize(characterParamSystem, target,
            Random.Range(minAmplitude, maxAmplitude),
            Random.Range(minFrequency, maxFrequency));
    }
}