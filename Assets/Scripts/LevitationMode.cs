using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevitationMode : MonoBehaviour
{
    [SerializeField] private Transform _hand;//На сцене  
    [Header("Скорость притягивания")]
    [Range(10, 100)] [SerializeField] private float _telekinesSpeed;
    [Header("Скорость вращения")]
    [Range(0, 0.1f)] [SerializeField] private float _speedRotate;
    private Vector3 _axisRotate;//ось вращения рандомно высчитывается в OnPointerDown используется в FixedUpdate
    private Vector3 _direction;//направление притягивания высчитывается в OnPointerDown используется в FixedUpdate

    [SerializeField] private bool _isObjectInHand;//находится ли объект в руке?    

    private Transform _parent;//получить из Projectile при создании
    private Rigidbody _rigidbody;
    private LineRenderer _lineRenderer;//на том же объекте висит  
    public UnityAction ObjectReachedHand;
    public bool IsObjectInHand => _isObjectInHand;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();        
        _rigidbody = GetComponent<Rigidbody>();
        _axisRotate = GetAxisRotate();
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (_isObjectInHand == false)//Если родитель есть
        {
            transform.position = _parent.position;//Позиция объекта приравнивается к позиции родителя ля перемещения и вращения одновременного(родитель перемещается а объект отдельно может крутиться)
            _parent.Translate(_direction * Time.deltaTime * _telekinesSpeed);//Перемещаем объект именно родителя
            _lineRenderer.SetPosition(0, _parent.position);//Линия рендера всегда подправляется к родителю

            if (Vector3.Magnitude(_hand.position) - Vector3.Magnitude(transform.position) <= 0.1f)//Если точка руки достигнута
            {                
                _rigidbody.isKinematic = true;
                ObjectReachedHand?.Invoke();                
                _isObjectInHand = true;//Задаем состояние объекта - "Объект притянулся и находится в руке"
            }
        }
        transform.RotateAroundLocal(_axisRotate, _speedRotate);//крутим объект по двум рандомным осям(Устарело)        
    }
    
    private Vector3 GetAxisRotate()
    {
        Vector3 axisRotate;
        int randomNumber = Random.Range(1, 5);

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

    private void OnEnable()
    {
        _rigidbody.useGravity = false;//Отключаем гравитацию

        GameObject parent = new GameObject("Parent");//Создаем нового родителя болванку (Чтобы он перемещался к руке а сам объект мог вращаться вокруг себя не мешая координации перемещения родителя)
        parent.transform.position = transform.position;//Родителю даем координаты объекта
        _parent = parent.transform;

        transform.parent = _parent.transform;//Назаначаем нового, только что созданного родителя объекту
        _direction = (_hand.position - _parent.transform.position).normalized;// Вычисляем направления притяжения к точке руки
        
        _lineRenderer.enabled = true;//Включаем линию телекинеза и назначаем ей точки отрисовки
        _lineRenderer.SetPosition(0, _parent.transform.position);
        _lineRenderer.SetPosition(1, _hand.transform.position);
    }

    private void OnDisable()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        transform.parent = null;
        Destroy(_parent.gameObject);
        _lineRenderer.enabled = false;
    }
}