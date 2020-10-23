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

    private void OnTriggerEnter(Collider other)
    {
        //Снаряд влетел в триггер персонажа:
        if (other.gameObject.GetComponent<Projectile>())
        {
            //Если снаряд запущен и персонаж еще не мертв
            if (other.gameObject.GetComponent<Projectile>().GetStatusLaunch() == true && _isDied == false)
            {                
                Die(other.gameObject.GetComponent<Projectile>().GetVelocity());
                //EnableDollMode(other.gameObject.GetComponent<Projectile>().GetVelocity());
            }

            /*
             *Если снаряд не был запущен(валяется на платформе)
             *или персонаж взаимодействия мерт, то ничего не происходит
             */
        }

        //Если живой персонаж улетел со сцены
        if (other.gameObject.tag == "CheckingFall" && _isDied == false)
        {
            Die(Vector3.zero);            
        }
    }

    /*
     * "Умереть"
     * Метод используется при столкновении со снарядом или при взрыве бочки из другого скрипта
     * - статус персонажа изменяется на "мертв"
     * - запускается событие "умер"
     * - отключается компонент отвечающий за передвижение персонажа по сцене
     */
    public void Die(Vector3 directionFalls)
    {
        _isDied = true;
        Died?.Invoke(_typeCharacter);        
        _characterMovement.enabled = false;
        EnableDollMode(directionFalls);
    }

    /*
     * "Включить режим куклы"
     * Реализация тряпичной куклы:
     * - анимация прекращается
     * - отключается кинематика у всех ridgidbody для симуляции рагдола
     */
    private void EnableDollMode(Vector3 directionFalls)
    {
        _characterAnimator.AnimationOff();
        _ragdollController.RigidbodyIsKinematicOff(directionFalls);
    }
}
