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
    [SerializeField] private Aim _aim;    
    [SerializeField] private Backlight _backLight;    

    [Header("Скорость притягивания")]
    [Range(10, 100)][SerializeField] private float _telekinesSpeed;
    [Header("Скорость вращения")]
    [Range(0, 0.1f)][SerializeField] private float _speedRotate;
    private Vector3 _direction;
    private float _pushForce = 5000;    
    //Сделать машину состояний
    [SerializeField] private bool _isObjectInHand;
    [SerializeField] private bool _isLevitation;
    [SerializeField] private bool _isFreeFlight;
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
        if (_isLevitation && _isObjectInHand == false && _parent && _isFreeFlight == false)//Если объект в состоянии движения(т.е зажата клавиша) И объект не в руке И родитель есть (создается как только клавиша зажата)
        {
            transform.position = _parent.transform.position;//Позиция объекта приравнивается к позиции родителя ля перемещения и вращения одновременного(родитель перемещается а объект отдельно может крутиться)
            _parent.transform.Translate(_direction * Time.deltaTime * _telekinesSpeed);//Перемещаем объект
            _lineRenderer.SetPosition(0, _parent.transform.position);//Линия рендера всегда подправляется к родителю
            
            if (Vector3.Magnitude(_hand.position) - Vector3.Magnitude(transform.position) <= 0.1f)//Если точка руки достигнута
            {
                _isObjectInHand = true;//Задаем состояние объекта - "В руке"    
                _aim.enabled = true;//Включаем прицел
                _backLight.enabled = true;
                _backLight.SetMaterialReadyToLaunch();                
            }
        }

        if ((_isLevitation || _isObjectInHand) && _isFreeFlight == false)//если в движении ИЛИ руках
        {
            transform.RotateAroundLocal(Vector3.up, _speedRotate);//крутим объект по оси Y пока объект не на земле
        }
    }

    public void OnPointerDown(PointerEventData eventData)//При зажатии на объекте 
    {
        if (_isObjectInHand == false && _isFreeFlight == false)//Если не в руке 
        {            
            _parent = new GameObject("Parent");//Создаем нового родителя болванку (Чтобы он перемещался к руке а сам объект мог вращаться вокруг себя не мешая координации перемещения родителя)
            _parent.transform.position = transform.position;//Родителю даем координаты объекта
            transform.parent = _parent.transform;//Назаначаем нового, только что созданного родителя объекту
            _direction = (_hand.position - _parent.transform.position).normalized;// Вычисляем направления притяжения к точке руки

            _lineRenderer.enabled = true;//Включаем линию телекинеза и назначаем ей точки отрисовки
            _lineRenderer.SetPosition(0, _parent.transform.position);
            _lineRenderer.SetPosition(1, _hand.transform.position);
            _backLight.enabled = true;//включаем подсветку телекинеза на объекте

            _rigidbody.useGravity = false;//Отключаем гравитацию
            _isLevitation = true;//Задаем состояние объекта - "Левитация"            
        } //Далее реализовать: Иначе если объект уже в руке то логику создания цели и перемещения этой цели по полю а при отпускании выстрел объектом        
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
            if (_aim.enabled == true)
            {
                _isFreeFlight = true;
                _isObjectInHand = false;
                _rigidbody.useGravity = true;
                Vector3 directionPush = Vector3.Normalize(_aim.GetMarkerPosition() - transform.position);
                _rigidbody.AddForce(directionPush * _pushForce);                
                _aim.enabled = false;//выключаем прицел
            }           
        }       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            transform.parent = _platform.transform;
        }        
    }
}
