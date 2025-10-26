using StateMachine;
using UnityEngine;

public class PetAttackState : State
{
    private readonly PetParamSystem paramSystem;
    private readonly PetAnimatorController animatorController;
    private readonly Transform pet;
    private Transform target;
    private readonly SpriteRenderer spriteRenderer;
    private float attackWindowTimer;
    private float lastFlipTime;
    private const float flipCooldown = 0.1f;

    public PetAttackState(PetParamSystem paramSystem, PetAnimatorController animatorController, Transform pet, Transform target)
    {
        this.paramSystem = paramSystem;
        this.animatorController = animatorController;
        this.pet = pet;
        this.target = target;
        this.spriteRenderer = pet.GetComponent<SpriteRenderer>();
    }

    public override void OnStateEnter()
    {
        animatorController.SetTrigger(PetAnimationType.Attack);
        attackWindowTimer = animatorController.GetAnimationDuration(PetAnimationType.Attack);
        if (attackWindowTimer < paramSystem.AttackCooldown)
            attackWindowTimer = paramSystem.AttackCooldown;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public override void Tick()
    {
        if (target == null) return;

        // Только атакуем, не двигаемся
        var dir = (target.position - pet.position).normalized;
        UpdateFacing(dir);
        attackWindowTimer -= Time.deltaTime;
    }

    private void UpdateFacing(Vector3 direction)
    {
        if (spriteRenderer == null) return;
        
        // Защита от быстрого переключения
        if (Time.time - lastFlipTime < flipCooldown) return;

        if (direction.x > 0.01f && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
            lastFlipTime = Time.time;
        }
        else if (direction.x < -0.01f && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
            lastFlipTime = Time.time;
        }
    }

    public void OnAttackHit()
    {
        if (target == null) return;

        var damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(paramSystem.Damage, isPet:true);
        }
    }
}
