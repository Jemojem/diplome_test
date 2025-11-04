using System;
using StateMachine;
using UnityEngine;

public class EnemyWalkState : State
{
    private readonly EnemyConfiguration config;
    private readonly EnemyAnimatorController animatorController;
    private readonly Transform self;
    private readonly Transform target;
    private readonly float scale;
    private readonly float speedMultiplier;

    public EnemyWalkState(EnemyConfiguration config, EnemyAnimatorController animatorController, Transform self,
        Transform target, float speedMultiplier = 1f)
    {
        this.config = config;
        this.animatorController = animatorController;
        this.self = self;
        this.target = target;
        this.scale = self.localScale.x;
        this.speedMultiplier = speedMultiplier;
    }

    public override void OnStateEnter()
    {
        animatorController.SetBool(EnemyAnimationType.Walk, true);
    }

    public override void OnStateExit()
    {
        animatorController.SetBool(EnemyAnimationType.Walk, false);
    }

    public override void Tick()
    {
        if (target == null) return;

        Vector3 dir = (target.position - self.position);
        dir.z = 0f;
        if (dir.sqrMagnitude > 0.0001f)
        {
            dir = dir.normalized;
            self.position += dir * (config.MoveSpeed * speedMultiplier * Time.deltaTime);
        }

        if (dir.x > 0.01f)
        {
            self.localScale = new Vector3(scale, scale, scale);
        }
        else if (dir.x < -0.01f)
        {
            self.localScale = new Vector3(-scale, scale, scale);
        }
    }
}