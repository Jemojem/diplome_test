using StateMachine;
using UnityEngine;

public class EnemyAttackState : State
{
    private readonly EnemyConfiguration config;
    private readonly EnemyAnimatorController animatorController;
    private readonly Transform self;
    private readonly Transform target;
    private readonly float damageMultiplier;

    private float attackWindowTimer;

    public EnemyAttackState(EnemyConfiguration config, EnemyAnimatorController animatorController, Transform self,
        Transform target, float damageMultiplier = 1f)
    {
        this.config = config;
        this.animatorController = animatorController;
        this.self = self;
        this.target = target;
        this.damageMultiplier = damageMultiplier;
    }

    public override void OnStateEnter()
    {
        animatorController.SetTrigger(EnemyAnimationType.Attack);
        attackWindowTimer = animatorController.GetAnimationDuration(EnemyAnimationType.Attack);
        if (attackWindowTimer < config.AttackCooldown)
            attackWindowTimer = config.AttackCooldown;
    }

    public override void Tick()
    {
        attackWindowTimer -= Time.deltaTime;
    }

    public void OnAttackHit()
    {
        if (target == null) return;

        var damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            var damage = Mathf.RoundToInt(config.Damage * damageMultiplier);
            damageable.TakeDamage(damage);
        }
    }
}