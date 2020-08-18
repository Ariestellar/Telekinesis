using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class SubjectInteraction : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform _hand;
    private float _pushForce = 5000;
    private float _telekinesisForce = 2000;
    [SerializeField] private bool _isObjectInHand;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Arm")
        {
            _isObjectInHand = true;
            _rigidbody.isKinematic = true;
            other.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isObjectInHand == false)
        {
            transform.parent = null;
            _rigidbody.useGravity = false;
            _rigidbody.AddForce((_hand.position - transform.position).normalized * _telekinesisForce);
        }
        else
        {
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            _rigidbody.AddForce(_hand.forward * _pushForce);
        }               
    }
}
