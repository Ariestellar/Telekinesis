using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backlight : MonoBehaviour
{
    [SerializeField] private GameObject _backLightPrefab;
    [SerializeField] private Material _materialReadyToLaunch;
    //Если меш на другом объекте то указать
    [SerializeField] private MeshFilter _meshFilter;
    private GameObject _backLight;

    private void Awake()
    {
        if (_meshFilter == null)
        {
            _meshFilter = GetComponent<MeshFilter>();
        }
        _backLight = Instantiate(_backLightPrefab, transform);
        _backLight.GetComponent<MeshFilter>().mesh = _meshFilter.mesh;
        _backLight.SetActive(false);
    }

    private void OnEnable()
    {
        _backLight.SetActive(true);
    }

    private void OnDisable()
    {
        _backLight.SetActive(false);
    }

    public void SetMaterialReadyToLaunch()
    {
        _backLight.GetComponent<MeshRenderer>().material = _materialReadyToLaunch;
    }
}
