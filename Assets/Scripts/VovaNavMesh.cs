using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VovaNavMesh : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransofrm;
    private NavMeshAgent navMeshAgent;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        navMeshAgent.destination = movePositionTransofrm.position;
    }
}
