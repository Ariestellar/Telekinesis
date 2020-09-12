using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterBehavior : MonoBehaviour
{  
    [Header("Выбери состояние поведения персонажа: ")]
    [Tooltip("Если выбираешь MovementAlongPath то убедись что установил ссылку на путь в CharacterMovement")]
    [SerializeField] private StateBehavior _stateBehavior;
    [SerializeField] private TypeCharacter _typeCharacter;

    private CharacterAnimator _characterAnimator;
    private CharacterMovement _characterMovement;

    public UnityAction<TypeCharacter> Died;//подписываемся в GameSession -> IncreaseNumerDiedCharacter();

    private void Awake()
    {
        _characterAnimator = GetComponent<CharacterAnimator>();
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void Start()
    {        
        _characterAnimator.SetAnimationForCharacterBehavior(_stateBehavior);
        if (_stateBehavior != StateBehavior.MovementAlongPath)
        {
            _characterMovement.enabled = false;
        }       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>())
        {
            Died?.Invoke(_typeCharacter);
            _characterAnimator.SetAnimationForCharacterBehavior(StateBehavior.Die);
            _characterMovement.enabled = false;
        }
    }

}
