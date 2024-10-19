using Pathfinding;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public enum NPCState
    {
        Idle,
        Interacting,
        Waiting
    }

    public NPCState currentState = NPCState.Idle;
    public float wanderRadius = 10f;
    public float interactionRange = 2f;
    public Renderer npcRenderer;
    public TextMeshProUGUI stateText;

    public Vector2 areaMin = new Vector2(-10f, -10f);
    public Vector2 areaMax = new Vector2(10f, -10f);
    public float spawnHeight = 1f;
    public float spawnRadius = 0.5f;
    public int maxAttempts = 50;

    private Vector3 destination;
    private float actionTimer = 0f;
    private float moveSpeed = 2f;

    private Color idleColor = Color.green;
    private Color interactingColor = Color.blue;
    private Color waitingColor = Color.red;

    void Start()
    {
        SetStateText();
        StartCoroutine(ActivityRoutine());

        // √енераци€ первой случайной цели дл€ перемещени€
        GenerateRandomPos();
        InvokeRepeating("GenerateRandomPos", 0, 5);  // ѕериодическое обновление цели перемещени€
    }

    void Update()
    {
        if (currentState == NPCState.Idle)
        {
            Wander();
        }
        else if (currentState == NPCState.Waiting)
        {
            UpdateWaitingState();
        }
    }

    IEnumerator ActivityRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            switch (currentState)
            {
                case NPCState.Idle:
                    HandleIdleState();
                    break;

                case NPCState.Interacting:
                    yield return HandleInteractingState();
                    break;
            }
            yield return null;
        }
    }

    private void HandleIdleState()
    {
        if (CheckForNearbyNPC())
        {
            actionTimer = Random.Range(5f, 10f);
            currentState = NPCState.Waiting;
        }
    }

    private IEnumerator HandleInteractingState()
    {
        yield return new WaitForSeconds(Random.Range(5f, 10f));
        currentState = NPCState.Waiting;
        SetStateText();
        SetColor(waitingColor);
    }

    private void Wander()
    {
        if (Vector3.Distance(transform.position, destination) < 0.2f)
        {
            GenerateRandomPos();
        }
        MoveToDestination();
    }

    private void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);
    }

    private bool CheckForNearbyNPC()
    {
        foreach (GameObject npc in Reference.GetAllNPCs())
        {
            if (Vector3.Distance(transform.position, npc.transform.position) < interactionRange)
            {
                currentState = NPCState.Interacting;
                StartCoroutine(LookAtEachOther(npc.transform));
                SetStateText();
                SetColor(interactingColor);
                return true;
            }
        }
        return false;
    }

    private IEnumerator LookAtEachOther(Transform otherNPC)
    {
        RotateTowards(otherNPC);

        yield return new WaitForSeconds(Random.Range(5f, 10f));

        currentState = NPCState.Waiting;
        SetStateText();
        SetColor(waitingColor);
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.rotation = lookRotation;
        target.rotation = Quaternion.LookRotation(-direction);
    }

    private void UpdateWaitingState()
    {
        actionTimer -= Time.deltaTime;
        if (actionTimer <= 0f)
        {
            currentState = NPCState.Idle;
            SetStateText();
            SetColor(idleColor);
        }
    }

    private void SetStateText()
    {
        stateText.text = currentState.ToString();
    }

    private void SetColor(Color color)
    {
        npcRenderer.material.color = color;
    }

    // Ћогика генерации случайных позиций
    private void GenerateRandomPos()
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 spawnPosition = GetRandomPosition();

            if (IsValidSpawnPosition(spawnPosition))
            {
                destination = spawnPosition;
                break;
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(areaMin.x, areaMax.x);
        float randomZ = Random.Range(areaMin.y, areaMax.y);
        return new Vector3(randomX, spawnHeight, randomZ);
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position + new Vector3(0, spawnHeight, 0), spawnRadius);
        return colliders.Length == 0;
    }
}
