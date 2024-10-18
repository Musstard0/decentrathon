using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public Transform pointA;              
    public Transform pointB;             
    public float moveSpeed = 2f;          
    private Transform targetPoint;        
    private bool movingToB = true;        

    void Start()
    {
        targetPoint = pointA;            
    }

    void Update()
    {
        MoveTowardsTarget();            

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (movingToB)
            {
                targetPoint = pointA;
                movingToB = false;
            }
            else
            {
                targetPoint = pointB;
                movingToB = true;
            }
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
    }

   
}
