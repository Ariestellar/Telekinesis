using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Backlight))]
[RequireComponent(typeof(LineRenderer))]
public class Projectile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Transform _platform;//На сцене
    [SerializeField] private AimMode _aimMode;//Висит на камере    
    
    [Header("Сила запуска")]
    [Range(2000, 5000)] [SerializeField] private float _pushForce;  
    
    //Сделать машину состояний    
    [SerializeField] private bool _isLaunch;//был ли объект запущен?
    [SerializeField] private bool _readyToLaunch;//готов ли объект к запуску?

    private Rigidbody _rigidbody;    
    private Backlight _backLight;
    private LevitationMode _levitationMode;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();        
        _backLight = GetComponent<Backlight>();
        _levitationMode = GetComponent<LevitationMode>();        
    }

    public void OnPointerDown(PointerEventData eventData)//При щелчке и зажатии на объекте 
    {        
        if (_isLaunch == false)
        {
            _levitationMode.enabled = true;
            _backLight.enabled = true;

            _levitationMode.ObjectReachedHand += ReadyToLaunch;
        }        
    }

    public void OnPointerUp(PointerEventData eventData)//При отпускании зажатия
    {        
        _levitationMode.enabled = false;
        _backLight.enabled = false;

        if (_isLaunch == false) 
        {
            if (_readyToLaunch)//Если объект готов к запуску 
            {                
                _aimMode.enabled = false;

                _isLaunch = true;
                
                Vector3 directionPush = Vector3.Normalize(_aimMode.GetMarkerPosition() - transform.position);
                _rigidbody.AddForce(directionPush * _pushForce);                
            }                        
        }                       
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            if (transform.parent != _platform.transform)
            {
                transform.parent = _platform.transform;
            }            
        }        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")//Если снаряд отрывается от платформы
        {
            if (_levitationMode.enabled == false)//и не левитирует 
            {
                transform.parent = null;//то родителя у него нет
            }            
        }
    }

    private void ReadyToLaunch()
    {
        _aimMode.enabled = true;//Включаем прицел
        _readyToLaunch = true;
        _backLight.SetMaterialReadyToLaunch();//Изменяем цвет материала      
    }
}
