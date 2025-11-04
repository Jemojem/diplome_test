using StateMachine;
using UnityEngine;

public class EnemyMoveAroundPlayerState : State
{
    private Transform player; // Ссылка на игрока
    private readonly EnemyConfiguration _enemyConfiguration;
    private Transform enemy; // Ссылка на игрока
    private readonly EnemyAnimatorController _animatorController;
    private readonly float speedMultiplier;
    private float orbitRadius = 4f; // Радиус движения вокруг игрока
    private float waitTime = 1f; // Пауза между сменой точек

    private Vector2 targetPoint;
    private float waitTimer;
    private Vector3 originalScale;

    public EnemyMoveAroundPlayerState(EnemyConfiguration enemyConfiguration, Transform player, Transform enemy, EnemyAnimatorController animatorController, float speedMultiplier = 1f)
    {
        this.player = player;
        _enemyConfiguration = enemyConfiguration;
        this.enemy = enemy;
        _animatorController = animatorController;
        this.speedMultiplier = speedMultiplier;
    }

    public override void OnStateEnter()
    {
        originalScale = enemy.localScale;
        _animatorController.SetBool(EnemyAnimationType.Walk,true);
        PickNewPoint();
    }

    public override void OnStateExit()
    {
        _animatorController.SetBool(EnemyAnimationType.Walk,false);
    }

    public override void Tick()
    {
        Update();
    }

    void Update()
    {
        if (player == null) return;
        enemy.position =
            Vector2.MoveTowards(enemy.position, targetPoint, _enemyConfiguration.MoveSpeed * speedMultiplier * Time.deltaTime);
        if (Vector2.Distance(enemy.position, targetPoint) < 0.1f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                PickNewPoint();
                waitTimer = 0f;
            }
        }

        if (player.position.x > enemy.position.x)
            enemy.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else
            enemy.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }

    private void PickNewPoint()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * orbitRadius;
        targetPoint = (Vector2)player.position + offset;
    }
}