using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _targetPosition;
    private NavMeshAgent _agent;
    [SerializeField] private int _currentPointNumber;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();        
    }

    private void Start()
    {
        _currentPointNumber = 0;
    }

    private void FixedUpdate()
    {        
        _agent.SetDestination(_targetPosition[_currentPointNumber].position);

        if (_agent.remainingDistance == 0 && _agent.SetDestination(_targetPosition[_currentPointNumber].position))
        {
            Debug.Log("Установленна точка №" + _currentPointNumber);
            Debug.Log("Расстояние:" + _agent.remainingDistance);
            _currentPointNumber = (_currentPointNumber + 1) % _targetPosition.Length;
            Debug.Log("Следующая точка установленна №" + _currentPointNumber);
        }                
    }
}
