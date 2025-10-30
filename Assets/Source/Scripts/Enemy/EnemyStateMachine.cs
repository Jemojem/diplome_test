using System;
using DG.Tweening;
using StateMachine;
using StateMachine.Conditions;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyStateMachine : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyConfiguration enemyConfiguration;
    [SerializeField] private Animator animator;
    [SerializeField] private BackflipObject flipObject;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject droppedLoot;
    [SerializeField] private GameObject chest;

    private Transform player;
    private StateMachine.StateMachine stateMachine;
    private EnemyAnimatorController enemyAnimatorController;
    private EnemyGetHitState flip;
    private int currentHealth;
    private EnemyAttackState enemyAttackState;
    private CharacterParamSystem characterParamSystem;

    public event Action OnEnemyDead;
    public Transform Target => player;

    public void Initialize(Transform player, CharacterParamSystem characterParamSystem)
    {
        this.characterParamSystem = characterParamSystem;
        this.player = player;
        currentHealth = enemyConfiguration.Health;
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        enemyAnimatorController = new EnemyAnimatorController(animator);

        var idle = new EnemyAnimationState(EnemyAnimationType.Idle, enemyAnimatorController);
        var spawn = new EnemyAnimationState(EnemyAnimationType.Spawn, enemyAnimatorController);
        var walk = new EnemyWalkState(enemyConfiguration, enemyAnimatorController, transform, player);
        flip = new EnemyGetHitState(flipObject);

        flip.AddTransition(new StateTransition(idle, new TemporaryCondition(flipObject.FlipDuration)));

        enemyAttackState = new EnemyAttackState(enemyConfiguration, enemyAnimatorController, transform, player);

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

    public bool CanTakeDamage { get; private set; }

    public void TakeDamage(int amount , bool isPermanent = false, bool isPet = false)
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
        var isCrit = characterParamSystem.CritChance > Random.Range(0,100);
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

                if (Random.Range(0, 100) < characterParamSystem.LuckyChest)
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