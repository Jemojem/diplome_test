using DG.Tweening;
using StateMachine;
using StateMachine.Conditions;
using UnityEngine;

public class EnemyMagicStateMachine : EnemyStateMachine
{
    [SerializeField] private GameObject projectile;

    protected override void InitializeStateMachine()
    {
        enemyAnimatorController = new EnemyAnimatorController(animator);

        var speedMultiplier = gameParamSystem != null ? gameParamSystem.EnemySpeedMultiplier : 1f;

        var idle = new EnemyAnimationState(EnemyAnimationType.Idle, enemyAnimatorController);
        var spawn = new EnemyAnimationState(EnemyAnimationType.Spawn, enemyAnimatorController);
        var walk = new EnemyMoveAroundPlayerState(enemyConfiguration, player, transform, enemyAnimatorController, speedMultiplier);
        var attackState = new EnemyShoot(player, transform, projectile, enemyAnimatorController);
        flip = new EnemyGetHitState(flipObject);

        flip.AddTransition(new StateTransition(idle, new TemporaryCondition(flipObject.FlipDuration)));

        DOVirtual.DelayedCall(0.6f, () => { CanTakeDamage = true; });
        spawn.AddTransition(new StateTransition(idle,
            new TemporaryCondition(enemyAnimatorController.GetAnimationDuration(EnemyAnimationType.Spawn))));

        idle.AddTransition(new StateTransition(walk, new FuncCondition(() => player != null &&
                                                                             Vector2.Distance(transform.position, player.position) >
                                                                             enemyConfiguration.AttackRange)));
        walk.AddTransition(new StateTransition(attackState,
            new TemporaryCondition(enemyConfiguration.AttackCooldown)));
        attackState.AddTransition(new StateTransition(walk, new TemporaryCondition(2f)));
        stateMachine = new StateMachine.StateMachine(spawn);
    }
}