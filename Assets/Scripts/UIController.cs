using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pathfinding; // For A* pathfinding components
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    public GameObject npcPrefab; // NPC prefab for spawning
    public TextMeshProUGUI debugInfoText;   // UI text element for displaying debug info
    public GameObject debugPanel; // Panel to toggle pathfinding debug info

    public Vector2 areaMin = new Vector2(-10f, -10f); // Area boundaries for NPC spawning
    public Vector2 areaMax = new Vector2(10f, 10f);
    public float spawnHeight = 1f; // Y-coordinate for spawn height
    public float spawnRadius = 0.5f; // Radius for valid spawn check
    public int maxAttempts = 50; // Maximum attempts to find a valid spawn position

    private float deltaTime = 0.0f;

    void Start()
    {
        // Hide debug panel at the start
        debugPanel.SetActive(false);
    }

    void Update()
    {
        // Update framerate and pathfinding info every frame
        UpdateDebugInfo();
    }

    public void SpawnNPC()
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (IsValidSpawnPosition(spawnPosition))
            {
                GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
                Reference.AddNPC(npc); // Add NPC to the global reference list
                break;
            }
        }
    }

    public void DeleteOneNPC()
    {
        var npcs = Reference.GetAllNPCs();
        if (npcs.Count > 0)
        {
            GameObject npcToRemove = npcs[npcs.Count - 1];
            Reference.RemoveNPC(npcToRemove);
            Destroy(npcToRemove);
        }
    }

    public void DeleteAllNPCs()
    {
        var npcs = Reference.GetAllNPCs();
        foreach (var npc in npcs)
        {
            Destroy(npc);
        }
        Reference.ClearNPCList();
    }

    public void ToggleDebugPanel()
    {
        debugPanel.SetActive(!debugPanel.activeSelf);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(areaMin.x, areaMax.x);
        float randomZ = Random.Range(areaMin.y, areaMax.y);
        return new Vector3(randomX, spawnHeight, randomZ);
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position + new Vector3(0, spawnHeight, 0), spawnRadius);
        return colliders.Length == 0; // Valid if no colliders in spawn radius
    }

    private void UpdateDebugInfo()
    {
        // Frame Rate Calculation
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Pathfinding Debug Info - calculate average path computation time
        Seeker[] pathfindingComponents = FindObjectsOfType<Seeker>();
        float totalPathDuration = 0f;
        int pathCount = 0;

        foreach (var seeker in pathfindingComponents)
        {
            var path = seeker.GetCurrentPath();
            if (path != null)
            {
                totalPathDuration += path.duration;
                pathCount++;
            }
        }

        float averagePathDuration = pathCount > 0 ? totalPathDuration / pathCount : 0;

        // Update debug text
        debugInfoText.text = $"FPS: {Mathf.Ceil(fps)}\n" +
                             $"Active Seekers: {pathfindingComponents.Length}\n" +
                             $"Average Path Computation Time: {averagePathDuration:F2} ms\n";
    }
}
