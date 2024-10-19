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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            SpawnObject();
        }

        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
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
                GameObject npc = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                Reference.AddNPC(npc);
                break;
            }
        }
    }

    void RemoveObject()
    {
        if (Reference.GetAllNPCs().Count > 0)
        {
            GameObject npcToRemove = Reference.GetAllNPCs()[Reference.GetAllNPCs().Count - 1];
            Reference.RemoveNPC(npcToRemove);
            Destroy(npcToRemove);
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
        Collider[] colliders = Physics.OverlapSphere(position + new Vector3(0, spawnHeight,0), spawnRadius);
        return colliders.Length == 0;
    }
}

public static class Reference
{
    public static List<GameObject> NPCs = new List<GameObject>();

    public static void AddNPC(GameObject npc)
    {
        if (!NPCs.Contains(npc))
        {
            NPCs.Add(npc);
        }
    }

    public static void RemoveNPC(GameObject npc)
    {
        if (NPCs.Contains(npc))
        {
            NPCs.Remove(npc);
        }
    }

    public static List<GameObject> GetAllNPCs()
    {
        return NPCs;
    }
}
