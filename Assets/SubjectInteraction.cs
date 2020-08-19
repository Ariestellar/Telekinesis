using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class SubjectInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Transform _plane;
    [SerializeField] private Transform _hand;
    [SerializeField] private GameObject _lightTelekines;
    [SerializeField] private Material _lightRedy;

    [Header("Скорость притягивания")]
    [Range(10, 100)][SerializeField] private float _telekinesSpeed;
    [Header("Скорость вращения")]
    [Range(0, 0.1f)][SerializeField] private float _speedRotate;
    private Vector3 _direction;
    //private float _pushForce = 5000;    
    private bool _isObjectInHand;
    private bool _isMoveObject;
    private Rigidbody _rigidbody;
    private GameObject _parent;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    [System.Obsolete]//Устаревшую модель заменить
    private void FixedUpdate()
    {
        if (_isMoveObject && _isObjectInHand == false && _parent)//Если объект в состоянии движения(т.е зажата клавиша) И объект не в руке И родитель есть (создается как только клавиша зажата)
        {
            transform.position = _parent.transform.position;//Позиция объекта приравнивается к позиции родителя ля перемещения и вращения одновременного(родитель перемещается а объект отдельно может крутиться)
            _parent.transform.Translate(_direction * Time.deltaTime * _telekinesSpeed);//Перемещаем объект
            _lineRenderer.SetPosition(0, _parent.transform.position);//Линия рендера всегда подправляется к родителю
            
            if (Vector3.Magnitude(_hand.position) - Vector3.Magnitude(transform.position) <= 0.1f)//Если точка руки достигнута
            {
                _isObjectInHand = true;//Задаем состояние объекта - "В руке"                
                _lightTelekines.SetActive(true);//делаем свечение постоянным(до этого оно отключается когда мы прекращаем держат луч)
                _lightTelekines.GetComponent<MeshRenderer>().material = _lightRedy;//Подключаем свечение другим светом 
            }
        }

        if (_isMoveObject || _isObjectInHand)//если в движении ИЛИ руках
        {
            transform.RotateAroundLocal(Vector3.up, _speedRotate);//крутим объект по оси Y пока объект не на земле
        }
    }

    public void OnPointerDown(PointerEventData eventData)//При зажатии на объекте 
    {
        if (_isObjectInHand == false)//Если не в руке 
        {
            transform.parent = null;//отцепляем от предыдущего родителя коим является земля
            _parent = new GameObject("Parent");//Создаем нового родителя болванку (Чтобы он перемещался к руке а сам объект мог вращаться вокруг себя не мешая координации перемещения родителя)
            _parent.transform.position = transform.position;//Родителю даем координаты объекта
            transform.parent = _parent.transform;//Назаначаем нового, только что созданного родителя объекту
            _direction = (_hand.position - _parent.transform.position).normalized;// Вычисляем направления притяжения к точке

            _lineRenderer.enabled = true;//Включаем линию телекинеза и назначаем ей точки отрисовки
            _lineRenderer.SetPosition(0, _parent.transform.position);
            _lineRenderer.SetPosition(1, _hand.transform.position);
            _lightTelekines.SetActive(true);//включаем подсветку телекинеза на объекте

            _rigidbody.useGravity = false;//Отключаем гравитацию
            _isMoveObject = true;//Задаем состояние объекта - "В движении"            
        } //Далее реализовать: Иначе если объект уже в руке то логику создания цели и перемещения этой цели по полю а при отпускании выстрел объектом        
    }

    public void OnPointerUp(PointerEventData eventData)//При отпускании зажатия
    {
        _lineRenderer.enabled = false;//Линия телекинеза больше не рисуется       
        _isMoveObject = false;//Объект меняет состояние на "не в движении"

        if (_isObjectInHand == false)//Если не в руке
        {
            _rigidbody.useGravity = true;
            transform.parent = _plane;
            _lightTelekines.SetActive(false);            
            Destroy(_parent);
        }//Иначе объект запускается
        
    }

    
}
