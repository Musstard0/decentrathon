using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public enum NPCState
    {
        Idle,
        Interacting,
        PlayingAnimation,
        Waiting
    }

    public NPCState currentState = NPCState.Idle;
    public float wanderRadius = 10f;
    public float interactionRange = 2f;
    public Animator animator;
    public Renderer npcRenderer;
    public Text stateText;

    private Vector3 destination;
    private float actionTimer = 0f;
    private float moveSpeed = 2f;

    private Color idleColor = Color.green;
    private Color interactingColor = Color.blue;
    private Color playingAnimationColor = Color.yellow;
    private Color waitingColor = Color.red;

    void Start()
    {
        SetStateText();
        StartCoroutine(ActivityRoutine());
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
        else if (Random.value < 0.1f)
        {
            StartCoroutine(PlayRandomAnimation());
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
            SetNewWanderDestination();
        }
        MoveToDestination();
    }

    private void SetNewWanderDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        destination = new Vector3(randomPoint.x, transform.position.y, randomPoint.y) + transform.position;
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

    private IEnumerator PlayRandomAnimation()
    {
        currentState = NPCState.PlayingAnimation;
        SetStateText();
        SetColor(playingAnimationColor);

        AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
        string randomAnimation = clips[Random.Range(0, clips.Length)].clip.name;
        animator.SetTrigger(randomAnimation);

        yield return new WaitForSeconds(Random.Range(2f, 5f));

        currentState = NPCState.Waiting;
        SetStateText();
        SetColor(waitingColor);
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
}
