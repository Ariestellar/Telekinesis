using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator _characterAnimator;

    public void SetAnimationForCharacterBehavior(StateBehavior stateBehavior)
    {
        _characterAnimator.SetTrigger(Convert.ToString(stateBehavior));
    }
}
