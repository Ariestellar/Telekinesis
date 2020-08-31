using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _targetPosition;
    private NavMeshAgent _agent;
    private int _currentPointNumber;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _currentPointNumber = 0;
    }

    private void FixedUpdate()
    {        
        _agent.destination = _targetPosition[_currentPointNumber].position;
        if (_agent.remainingDistance == 0)
        {            
            _currentPointNumber += 1;
            if (_currentPointNumber == _targetPosition.Length)
            {
                _currentPointNumber = 0;
            }
            Debug.Log("Установленна следующая точка №" + _currentPointNumber);
        }
    }
}
