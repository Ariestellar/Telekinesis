using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    [SerializeField] private CharacterBehavior _characterBehavior;

    //Проверка на выход из триггера
    private void OnTriggerExit(Collider other)
    {
        //потерял опору: падаешь
        if (other.gameObject.tag == "Support")
        {
            _characterBehavior.ToFall();
        }
    }

    //Проверка на вхождение в триггер
    private void OnTriggerEnter(Collider other)
    {
        //Попал на платформу: встать
        if (other.gameObject.tag == "Platform")
        {
            _characterBehavior.GetCharacterMovement().transform.up = other.transform.up;
            _characterBehavior.StandUp();
        }

        //Выпал с платформы: умер
        if (other.gameObject.tag == "CheckingFall")//Триггер расположенный за платформой
        {
            _characterBehavior.Die(Vector3.zero);
        }
    }
}
