using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class CharacterMovement : MonoBehaviour
{
    [Header("Сюда установить ссылку на путь:")]
    [SerializeField] private PathCreator _pathCreator;
    [Header("Скорость передвижения по пути:")]
    [SerializeField] private float _speed;

    private float _distanceTravelled;

    private void Update()
    {
        _distanceTravelled += _speed * Time.deltaTime;
        transform.position = _pathCreator.path.GetPointAtDistance(_distanceTravelled);
        transform.rotation = _pathCreator.path.GetRotationAtDistance(_distanceTravelled);
    }
}
