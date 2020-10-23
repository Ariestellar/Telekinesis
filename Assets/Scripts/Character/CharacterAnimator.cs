using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator _characterAnimator;

    private void Awake()
    {
        _characterAnimator = GetComponent<Animator>();
    }

    public void SetAnimationForCharacterBehavior(StateBehavior stateBehavior)
    {
        _characterAnimator.SetTrigger(Convert.ToString(stateBehavior));
    }

    public void AnimationOff()
    {
        _characterAnimator.enabled = false;
    }
}
