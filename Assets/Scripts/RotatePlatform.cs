using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    [Range(0, 1)][SerializeField] private float _speed;
    private void Update()
    {
        transform.Rotate(Vector3.up, _speed);
    }
}
