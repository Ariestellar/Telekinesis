using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    [SerializeField] private int _numberOfFallen;

    private void OnTriggerExit(Collider other)
    {
        _numberOfFallen += 1;
    }
}
