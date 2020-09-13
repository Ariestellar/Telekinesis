using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLook : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _renderer;       

    public void SetMaterial(Material material)
    {
        _renderer.material = material;
    }
}
