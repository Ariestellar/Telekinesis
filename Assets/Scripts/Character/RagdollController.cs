using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public List<Rigidbody> RigidbodiesRagdoll = new List<Rigidbody>();    

    private void Awake()
    {
        RigidbodiesRagdoll.AddRange(GetComponentsInChildren<Rigidbody>());
        RigidbodyIsKinematicOn();
    }

    public void RigidbodyIsKinematicOn()
    {
        for (int i = 0; i < RigidbodiesRagdoll.Count; i++)
        {            
            RigidbodiesRagdoll[i].isKinematic = true;            
        }
    }

    public void RigidbodyIsKinematicOff(Vector3 velocity)
    {
        for (int i = 0; i < RigidbodiesRagdoll.Count; i++)
        {            
            RigidbodiesRagdoll[i].isKinematic = false;            
        }

        foreach (var rigidbody in RigidbodiesRagdoll)
        {
            rigidbody.AddForce(velocity * 2, ForceMode.Impulse);
        }
    }
}
