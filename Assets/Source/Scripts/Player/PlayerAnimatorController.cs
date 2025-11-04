using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController
{
    private readonly Animator _animator;
    private readonly Dictionary<PlayerAnimationType, int> _animationHashes = new Dictionary<PlayerAnimationType, int>();

    public PlayerAnimatorController(Animator animator)
    {
        _animator = animator;
        foreach (PlayerAnimationType type in Enum.GetValues(typeof(PlayerAnimationType)))
        {
            _animationHashes.Add(type, Animator.StringToHash(type.ToString()));
        }
    }

    public void SetBool(PlayerAnimationType type, bool value)
    {
        _animator.SetBool(_animationHashes[type], value);
    }

    public void SetTrigger(PlayerAnimationType type)
    {
        _animator.SetTrigger(_animationHashes[type]);
    }

    public float GetAnimationDuration(PlayerAnimationType animationType)
    {
        RuntimeAnimatorController controller = _animator.runtimeAnimatorController;
        foreach (var clip in controller.animationClips)
        {
            if (clip.name == animationType.ToString())
            {
                return clip.length;
            }
        }

        //Debug.LogError($"Animation clip {animationType} doesn't exist");
        return 0;
    }
}