using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector2 areaMin = new Vector2(-10f, -10f);
    public Vector2 areaMax = new Vector2(10f, 10f);
    public float spawnHeight = 1f;
    public float spawnRadius = 0.5f;
    public int maxAttempts = 50;
    public int numberOfObjectsToSpawn = 10;

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        int spawnedCount = 0;

        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Vector3 spawnPosition = GetRandomPosition();

                if (IsValidSpawnPosition(spawnPosition))
                {
                    GameObject npc = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                    spawnedCount++;

                    // Add spawned NPC to the global Reference list
                    Reference.AddNPC(npc);
                    break;
                }
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
        Collider[] colliders = Physics.OverlapSphere(position += new Vector3(0, 1, 0), spawnRadius);
        if (colliders.Length > 0)
        {
            return false;
        }

        return true; // If no collisions, spawn is valid
    }
}

public static class Reference
{
    // Static list to store NPCs globally
    public static List<GameObject> NPCs = new List<GameObject>();

    // Method to add an NPC to the list
    public static void AddNPC(GameObject npc)
    {
        if (!NPCs.Contains(npc))
        {
            NPCs.Add(npc);
        }
    }

    // Method to remove an NPC from the list
    public static void RemoveNPC(GameObject npc)
    {
        if (NPCs.Contains(npc))
        {
            NPCs.Remove(npc);
        }
    }

    // Method to get all NPCs
    public static List<GameObject> GetAllNPCs()
    {
        return NPCs;
    }
}