using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceTravelled;

    private void Update()
    {
        _distanceTravelled += _speed * Time.deltaTime;
        transform.position = _pathCreator.path.GetPointAtDistance(_distanceTravelled);
        transform.rotation = _pathCreator.path.GetRotationAtDistance(_distanceTravelled);
    }
}
