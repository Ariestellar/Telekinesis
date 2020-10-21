using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public List<Rigidbody> GetRigidbodies = new List<Rigidbody>();

    private void Awake()
    {
        GetRigidbodies.AddRange(GetComponentsInChildren<Rigidbody>());
        RigidbodyIsKinematicOn();
    }

    public void RigidbodyIsKinematicOn()
    {
        for (int i = 1; i < GetRigidbodies.Count; i++)
        {
            //GetRigidbodies[i].gameObject.GetComponent<Collider>().enabled = false;
            GetRigidbodies[i].isKinematic = true;            
        }
    }

    public void RigidbodyIsKinematicOff()
    {
        for (int i = 1; i < GetRigidbodies.Count; i++)
        {
            //GetRigidbodies[i].gameObject.GetComponent<Collider>().enabled = true;
            GetRigidbodies[i].isKinematic = false;
        }

    }
}
