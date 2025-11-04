using System;
using DG.Tweening;
using StateMachine;
using StateMachine.Conditions;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyStateMachine : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnemyConfiguration enemyConfiguration;
    [SerializeField] protected Animator animator;
    [SerializeField] protected BackflipObject flipObject;
    [SerializeField] protected GameObject hitEffect;
    [SerializeField] protected GameObject droppedLoot;
    [SerializeField] protected GameObject chest;

    protected Transform player;
    protected StateMachine.StateMachine stateMachine;
    protected EnemyAnimatorController enemyAnimatorController;
    protected EnemyGetHitState flip;
    protected int currentHealth;
    protected EnemyAttackState enemyAttackState;
    protected CharacterParamSystem characterParamSystem;
    protected GameParamSystem gameParamSystem;

    public event Action OnEnemyDead;
    public Transform Target => player;

    public void Initialize(Transform player, CharacterParamSystem characterParamSystem, GameParamSystem gameParamSystem)
    {
        this.characterParamSystem = characterParamSystem;
        this.gameParamSystem = gameParamSystem;
        this.player = player;
        var healthMultiplier = gameParamSystem != null ? gameParamSystem.EnemyHealthMultiplier : 1f;
        currentHealth = Mathf.RoundToInt(enemyConfiguration.Health * healthMultiplier);
        InitializeStateMachine();
    }

    protected virtual void InitializeStateMachine()
    {
        enemyAnimatorController = new EnemyAnimatorController(animator);

        var damageMultiplier = gameParamSystem != null ? gameParamSystem.EnemyDamageMultiplier : 1f;
        var speedMultiplier = gameParamSystem != null ? gameParamSystem.EnemySpeedMultiplier : 1f;

        var idle = new EnemyAnimationState(EnemyAnimationType.Idle, enemyAnimatorController);
        var spawn = new EnemyAnimationState(EnemyAnimationType.Spawn, enemyAnimatorController);
        var walk = new EnemyWalkState(enemyConfiguration, enemyAnimatorController, transform, player, speedMultiplier);
        flip = new EnemyGetHitState(flipObject);

        flip.AddTransition(new StateTransition(idle, new TemporaryCondition(flipObject.FlipDuration)));

        enemyAttackState = new EnemyAttackState(enemyConfiguration, enemyAnimatorController, transform, player, damageMultiplier);

        DOVirtual.DelayedCall(0.6f, () => { CanTakeDamage = true; });
        spawn.AddTransition(new StateTransition(idle,
            new TemporaryCondition(enemyAnimatorController.GetAnimationDuration(EnemyAnimationType.Spawn))));

        idle.AddTransition(new StateTransition(walk, new FuncCondition(() => player != null &&
                                                                             Vector2.Distance(transform.position,
                                                                                 player.position) >
                                                                             enemyConfiguration.AttackRange)));

        walk.AddTransition(new StateTransition(enemyAttackState,
            new FuncCondition(() =>
                player != null && Vector2.Distance(transform.position, player.position) <=
                enemyConfiguration.AttackRange)));

        var attackFinish = new TemporaryCondition(
            Mathf.Max(
                enemyAnimatorController.GetAnimationDuration(EnemyAnimationType.Attack),
                enemyConfiguration.AttackCooldown));

        enemyAttackState.AddTransition(new StateTransition(walk, attackFinish));

        stateMachine = new StateMachine.StateMachine(spawn);
    }

    private void Update()
    {
        stateMachine.Tick();
    }

    public void OnAttackHit()
    {
        enemyAttackState?.OnAttackHit();
    }

    public bool CanTakeDamage { get; protected set; }

    public void TakeDamage(int amount, bool isPermanent = false, bool isPet = false)
    {
        if (isPermanent)
        {
            OnEnemyDead?.Invoke();
            Destroy(gameObject);
            return;
        }

        if (CanTakeDamage == false) return;
        var analitycs = FindAnyObjectByType<GameAnalytics>();
        if (isPet)
        {
            analitycs.DamagePets++;
        }
        else
        {
            analitycs.DamagePlayer++;
        }

        var isCrit = characterParamSystem.CritChance > Random.Range(0, 100);
        if (isCrit)
        {
            amount *= 2;
        }

        currentHealth -= amount;
        var hit = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(hit, 3f);
        DamagePopupSpawner.Instance.Show(transform.position, amount, isCrit ? Color.red : Color.white);
        stateMachine.SetState(flip);
        if (currentHealth <= 0)
        {
            analitycs.KilledEnemy++;
            CanTakeDamage = false;
            stateMachine.SetState(new State());
            enemyAnimatorController.SetTrigger(EnemyAnimationType.Dead);

            DOVirtual.DelayedCall(0.2f, () =>
            {
                if (Random.Range(0, 100) > enemyConfiguration.DropChance)
                {
                    Instantiate(droppedLoot, transform.position, Quaternion.identity);
                }

                var luckyChest = gameParamSystem != null ? gameParamSystem.LuckyChest : 0;
                if (Random.Range(0, 100) < luckyChest)
                {
                    Instantiate(chest, transform.position, Quaternion.identity);
                }
            });
            DOVirtual.DelayedCall(0.4f, () =>
            {
                OnEnemyDead?.Invoke();
                Destroy(gameObject);
            });
        }
    }
}