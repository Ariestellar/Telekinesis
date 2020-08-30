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
    [SerializeField] private Transform _platform;
    [SerializeField] private Transform _hand;    
    [SerializeField] private AimMode _aimMode;//Висит на камере    
    [SerializeField] private Backlight _backLight;
    
    //Режим левитации:
    [Header("Скорость притягивания")]
    [Range(10, 100)][SerializeField] private float _telekinesSpeed;
    [Header("Скорость вращения")]
    [Range(0, 0.1f)][SerializeField] private float _speedRotate;
    private Vector3 _axisRotate;//ось вращения рандомно высчитывается в OnPointerDown используется в FixedUpdate
    private Vector3 _direction;//направление притягивания высчитывается в OnPointerDown используется в FixedUpdate

    [Header("Сила запуска")]
    [Range(2000, 5000)] [SerializeField] private float _pushForce;  
    
    //Сделать машину состояний
    [SerializeField] private bool _isObjectInHand;//находится ли объект в руке?
    [SerializeField] private bool _isLevitation;//находится ли объект в состоянии левитации?
    [SerializeField] private bool _isLaunch;//был ли объект запущен?
    private Rigidbody _rigidbody;
    private GameObject _parent;
    private LineRenderer _lineRenderer;    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _backLight = GetComponent<Backlight>();
    }

    [System.Obsolete]//Устаревшую модель заменить
    private void FixedUpdate()
    {
        if (_isLevitation && _isObjectInHand == false && _parent && _isLaunch == false)//Если объект в состоянии движения(т.е зажата клавиша) И объект не в руке И родитель есть (создается как только клавиша зажата)
        {
            transform.position = _parent.transform.position;//Позиция объекта приравнивается к позиции родителя ля перемещения и вращения одновременного(родитель перемещается а объект отдельно может крутиться)
            _parent.transform.Translate(_direction * Time.deltaTime * _telekinesSpeed);//Перемещаем объект
            _lineRenderer.SetPosition(0, _parent.transform.position);//Линия рендера всегда подправляется к родителю
            
            if (Vector3.Magnitude(_hand.position) - Vector3.Magnitude(transform.position) <= 0.1f)//Если точка руки достигнута
            {
                _isObjectInHand = true;//Задаем состояние объекта - "В руке"    
                _aimMode.enabled = true;//Включаем прицел
                _backLight.enabled = true;
                _backLight.SetMaterialReadyToLaunch();                
            }
        }

        if ((_isLevitation || _isObjectInHand) && _isLaunch == false)//если в левитации ИЛИ руках И не был запущен:
        {            
            transform.RotateAroundLocal(_axisRotate, _speedRotate);//крутим объект по оси Y             
        }
    }

    public void OnPointerDown(PointerEventData eventData)//При зажатии на объекте 
    {
             

        if (_isObjectInHand == false && _isLaunch == false)//Если не в руке и не был запущен (несчитается если он при притягивании отваливался) 
        {
            _parent = new GameObject("Parent");//Создаем нового родителя болванку (Чтобы он перемещался к руке а сам объект мог вращаться вокруг себя не мешая координации перемещения родителя)
            _parent.transform.position = transform.position;//Родителю даем координаты объекта
            transform.parent = _parent.transform;//Назаначаем нового, только что созданного родителя объекту

            _direction = (_hand.position - _parent.transform.position).normalized;// Вычисляем направления притяжения к точке руки
            _axisRotate = GetAxisRotate();//Получить случайную ось вращения объетка при левитации   

            _lineRenderer.enabled = true;//Включаем линию телекинеза и назначаем ей точки отрисовки
            _lineRenderer.SetPosition(0, _parent.transform.position);
            _lineRenderer.SetPosition(1, _hand.transform.position);
            _backLight.enabled = true;//включаем подсветку телекинеза на объекте

            _rigidbody.useGravity = false;//Отключаем гравитацию
            _isLevitation = true;//Задаем состояние объекта - "Левитация"            
        }
    }

    public void OnPointerUp(PointerEventData eventData)//При отпускании зажатия
    {
        _backLight.enabled = false;//подсветка выкл
        _lineRenderer.enabled = false;//линия телекинеза выкл     
        _isLevitation = false;//Объект меняет состояние на "не в движении"

        if (_isObjectInHand == false)//Если не в руке
        {
            _rigidbody.useGravity = true;
            transform.parent = null;            
            Destroy(_parent);
        }
        else //Иначе объект запускается 
        {                        
            if (_aimMode.enabled == true)//если режим прицела был включен 
            {
                _isLaunch = true;
                _isObjectInHand = false;
                _rigidbody.useGravity = true;
                Vector3 directionPush = Vector3.Normalize(_aimMode.GetMarkerPosition() - transform.position);
                _rigidbody.AddForce(directionPush * _pushForce);                
                _aimMode.enabled = false;//выключаем прицел
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
            if (_isLevitation == false)//и не левитирует 
            {
                transform.parent = null;//то родителя у него нет
            }            
        }
    }

    private Vector3 GetAxisRotate()
    {
        Vector3 axisRotate;
        int randomNumber = UnityEngine.Random.Range(1, 5);

        switch (randomNumber)
        {            
            case 1:
                axisRotate = Vector3.right + Vector3.up;
                break;
            case 2:
                axisRotate = Vector3.down + Vector3.right;
                break;
            case 3:
                axisRotate = Vector3.left + Vector3.down;
                break;
            case 4:
                axisRotate = Vector3.up + Vector3.right;
                break;
            default:
                axisRotate = Vector3.up;
                break;
        }
        return axisRotate;
    }
}
