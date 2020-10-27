using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterAnimator), typeof(RagdollController))]
public class CharacterBehavior : MonoBehaviour
{  
    [Header("Выбери состояние поведения персонажа: ")]
    [Tooltip("Если выбираешь MovementAlongPath то убедись что установил ссылку на путь в CharacterMovement")]
    [SerializeField] private StateBehavior _stateBehavior;
    [SerializeField] private TypeCharacter _typeCharacter;
    [SerializeField] private CharacterMovement _characterMovement;

    private CharacterAnimator _characterAnimator;      
    private RagdollController _ragdollController;
    private bool _isDied;    

    public UnityAction<TypeCharacter> Died;//подписываемся в GameSession -> IncreaseNumerDiedCharacter();

    private void Awake()
    {
        _characterAnimator = GetComponent<CharacterAnimator>();        
        _ragdollController = GetComponent<RagdollController>();        
    }

    private void Start()
    {        
        _characterAnimator.SetAnimationForCharacterBehavior(_stateBehavior);
        if (_stateBehavior != StateBehavior.MovementAlongPath)
        {
            _characterMovement.enabled = false;
        }       
    }

    /*
     * Поведение: "Смерть"
     * Метод используется при столкновении со снарядом или при взрыве бочки из другого скрипта
     * - статус персонажа изменяется на "мертв"
     * - запускается событие "умер"
     * - отключается компонент отвечающий за передвижение персонажа по сцене
     */
    public void Die(Vector3 directionFalls)
    {
        if (_isDied == false)
        {
            _isDied = true;
            Died?.Invoke(_typeCharacter);
            _characterMovement.enabled = false;
            EnableDollBehavior(directionFalls);
        }        
    }

    /*
     * Поведение: "Встать"
     */
    public void StandUp()
    {
        if (_isDied == false)
        {
            StartCoroutine(TimerStandUp());
        }        
    }

    public GameObject GetCharacterMovement()
    {
        return _characterMovement.gameObject;
    }

    /*
    * Поведение: "Падать"
    */
    public void ToFall()
    {
        EnableDollBehavior(Vector3.zero);
    }

    /*
     * "Включить поведение куклы"
     * Реализация тряпичной куклы:
     * - анимация прекращается
     * - отключается кинематика у всех ridgidbody для симуляции рагдола
     * Принимает параметр Vector3 - направление полета куклы, если необходимо просто 
     * упасть без направления полета когда в тело не прилетает снаряд тогда задаем Vector3.zero
     */
    private void EnableDollBehavior(Vector3 directionFalls)
    {
        _characterAnimator.GetComponent<Animator>().enabled = false;
        _ragdollController.RigidbodyIsKinematicOff(directionFalls);
    }

    private IEnumerator TimerStandUp()
    {        
        yield return new WaitForSeconds(2);
        _characterAnimator.GetComponent<Animator>().enabled = true;
        _ragdollController.RigidbodyIsKinematicOn();
        _characterAnimator.SetAnimationForCharacterBehavior(_stateBehavior);        
    }
}
