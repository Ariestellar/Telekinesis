using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorMainCharacter : MonoBehaviour
{
    [SerializeField] private GameObject _spiritBall;//временная неочивидная зависимость(класс управляет анимациями героя и + доп объектом)
    [SerializeField] private Animator _animatorCharacter;
    private Animator _animatorMovement;


    private void Awake()
    {
        _animatorMovement = GetComponent<Animator>();
    }

    public void FlyingMainMenu()
    {
        _animatorMovement.SetTrigger("MovementMainMenu");
        _animatorCharacter.SetTrigger("Flying");
    }

    //Событие срабатывает в конце анимации "MovementMainMenu"
    public void PlayIdleCharacter()
    {
        _spiritBall.SetActive(true);
        _animatorMovement.SetTrigger("PlayGame");
        _animatorCharacter.SetTrigger("Idle");
    }
}


