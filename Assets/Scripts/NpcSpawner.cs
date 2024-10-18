using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NpcSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private GameObject _spawnPrefab;

    [SerializeField]
    private int _spawnCount = 10;

    private SphereCollider _sphereCollider;
    public float FixedHeight => _target.position.y; // Фиксированная высота

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {

    }
}
