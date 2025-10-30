using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Vector2 range;
    [SerializeField] private CharacterParamSystem characterParamSystem;
    [SerializeField] private Animator animator;
    private bool canAttach = true;

    private void OnDrawGizmosSelected()
    {
        if (pivot == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pivot.position, range * characterParamSystem.AttackRange);
    }

    private void FixedUpdate()
    {
        
        if (canAttach == false || characterParamSystem.CanAttackMelee == false) return;
        var foundTargets = GetFoundColliders();
        if (foundTargets.Length == 0) return;
        foreach (var foundTarget in foundTargets)
        {
            foundTarget.GetComponent<IDamageable>().TakeDamage(characterParamSystem.AttachDamage);
            canAttach = false;
            animator.Play("AttackMelee",1);
            DOVirtual.DelayedCall(characterParamSystem.AttackSpeed, () => { canAttach = true; });
        }
    }

    private Collider2D[] GetFoundColliders()
    {
        var colliders = Physics2D.OverlapBoxAll(pivot.position, range * characterParamSystem.AttackRange, 10)
            .Where(t =>
            {
                var damageable = t.GetComponent<IDamageable>();
                return damageable is { CanTakeDamage: true };
            }).ToArray();
        return colliders;
    }
}