using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomDestinationSetter : MonoBehaviour
{
    void Start()
    {
        GenerateRandomPos();
        InvokeRepeating("GenerateRandomPos", 0, 5);
    }

    private void GenerateRandomPos()
    {
        GetComponent<AIDestinationSetter>().target.position = new Vector3(Random.Range(-20, 1000), 0, Random.Range(-20, 1000));
    }
}
