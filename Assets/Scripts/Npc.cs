using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Npc : MonoBehaviour
{
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }


}