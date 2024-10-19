using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomDestinationSetter : MonoBehaviour
{

    public Vector2 areaMin = new Vector2(-10f, -10f);
    public Vector2 areaMax = new Vector2(10f, 10f);
    public float spawnHeight = 1f;
    public float spawnRadius = 0.5f;
    public int maxAttempts = 50;

    void Start()
    {
        GenerateRandomPos();
        InvokeRepeating("GenerateRandomPos", 0, 5);
    }

    private void GenerateRandomPos()
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 spawnPosition = GetRandomPosition();

            if (IsValidSpawnPosition(spawnPosition))
            {

                GetComponent<AIDestinationSetter>().target.position = spawnPosition;
                break;
            }
        }

    }

    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(areaMin.x, areaMax.x);
        float randomZ = Random.Range(areaMin.y, areaMax.y);
        return new Vector3(randomX, spawnHeight, randomZ);
    }

    bool IsValidSpawnPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position + new Vector3(0, spawnHeight, 0), spawnRadius);
        return colliders.Length == 0;
    }
}
