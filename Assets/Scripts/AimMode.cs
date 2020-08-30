using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
public class AimMode : MonoBehaviour
{
    [SerializeField] private GameObject _markerPrefab; 
    
    private GameObject _marker; 
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _marker = Instantiate(_markerPrefab);
        _marker.SetActive(false);
    }

    private void OnEnable()
    {
        //_marker = Instantiate(_markerPrefab);
        _marker.SetActive(true);
    }

    private void OnDisable()
    {
        //Destroy(_marker);
        _marker.SetActive(false);
    }

    public Vector3 GetMarkerPosition()
    {
        return _marker ? _marker.transform.position : Vector3.zero;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray rayAttack = _camera.ScreenPointToRay(Input.mousePosition);//Проверить или изменить мышь и тач

        if (Physics.Raycast(rayAttack, out hit))
        {
            if (hit.transform.gameObject.tag == "Platform")
            {
                _marker.transform.position = hit.point;
            }
        }               
    }    
}
