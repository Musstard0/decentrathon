using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector2 areaMin = new Vector2(-10f, -10f);
    public Vector2 areaMax = new Vector2(10f, 10f);
    public float spawnHeight = 1f;
    public float spawnRadius = 0.5f;
    public int maxAttempts = 50;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals)) // "+" key
        {
            SpawnObject();
        }
        else if (Input.GetKeyDown(KeyCode.Minus)) // "-" key
        {
            RemoveObject();
        }
    }

    void SpawnObject()
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 spawnPosition = GetRandomPosition();

            if (IsValidSpawnPosition(spawnPosition))
            {
                GameObject newObj = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                spawnedObjects.Add(newObj);
                break;
            }
        }
    }

    void RemoveObject()
    {
        if (spawnedObjects.Count > 0)
        {
            GameObject lastObject = spawnedObjects[spawnedObjects.Count - 1];
            spawnedObjects.RemoveAt(spawnedObjects.Count - 1);
            Destroy(lastObject);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(areaMin.x, spawnHeight, areaMin.y), new Vector3(areaMax.x, spawnHeight, areaMin.y));
        Gizmos.DrawLine(new Vector3(areaMax.x, spawnHeight, areaMin.y), new Vector3(areaMax.x, spawnHeight, areaMax.y));
        Gizmos.DrawLine(new Vector3(areaMax.x, spawnHeight, areaMax.y), new Vector3(areaMin.x, spawnHeight, areaMax.y));
        Gizmos.DrawLine(new Vector3(areaMin.x, spawnHeight, areaMax.y), new Vector3(areaMin.x, spawnHeight, areaMin.y));
    }
}
