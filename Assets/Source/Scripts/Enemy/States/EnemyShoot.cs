using StateMachine;
using UnityEngine;

public class EnemyShoot : State
{
    private readonly Transform _player;
    private readonly Transform _enemy;
    private readonly GameObject _projectilePrefab;
    private readonly EnemyAnimatorController _animatorController;
    private float shootForce = 10f;

    public EnemyShoot(Transform player, Transform enemy, GameObject projectilePrefab,
        EnemyAnimatorController animatorController)
    {
        _player = player;
        _enemy = enemy;
        _projectilePrefab = projectilePrefab;
        _animatorController = animatorController;
    }

    public override void OnStateEnter()
    {
        _animatorController.SetTrigger(EnemyAnimationType.Attack);
        Shoot();
    }

    private void Shoot()
    {
        var projectile = GameObject.Instantiate(_projectilePrefab, _enemy.position, Quaternion.identity);
        Vector2 direction = (_player.position - _enemy.position).normalized;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * shootForce;
        }

        GameObject.Destroy(projectile, 3);
    }
}