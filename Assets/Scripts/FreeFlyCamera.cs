using UnityEngine;

public class FreeFlyCamera : MonoBehaviour
{
    public float moveSpeed = 10f;              
    public float lookSpeed = 2f;               
    public float sprintMultiplier = 2f;        
    public float sensitivity = 0.1f;          
    public float upDownSpeed = 5f;             

    private float yaw = 0f;                   
    private float pitch = 0f;                 

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();                     
        HandleMovement();                      

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleMouseLook()
    {
        yaw += Input.GetAxis("Mouse X") * lookSpeed;
        pitch -= Input.GetAxis("Mouse Y") * lookSpeed;

        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); 
        float moveZ = Input.GetAxis("Vertical");   
        float moveY = 0f;

        if (Input.GetKey(KeyCode.Space)) 
        {
            moveY = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftControl)) 
        {
            moveY = -1f;
        }

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ + transform.up * moveY;

        float currentSpeed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

        transform.position += moveDirection * currentSpeed * Time.deltaTime;
    }
}
