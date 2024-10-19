using UnityEngine;

public class FreeFlyCamera : MonoBehaviour
{
    public float moveSpeed = 10f;              // Скорость перемещения камеры
    public float lookSpeed = 2f;               // Скорость вращения камеры (мышью)
    public float sprintMultiplier = 2f;        // Множитель скорости при нажатии Shift
    public float sensitivity = 0.1f;           // Чувствительность мыши
    public float upDownSpeed = 5f;             // Скорость вертикального перемещения

    private float yaw = 0f;                    // Горизонтальный поворот
    private float pitch = 0f;                  // Вертикальный поворот

    void Start()
    {
        // Скрываем курсор и блокируем его в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();                     // Обработка вращения камеры
        HandleMovement();                      // Обработка перемещения камеры

        // Нажатие Esc возвращает курсор
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Метод для вращения камеры мышью
    void HandleMouseLook()
    {
        // Получаем смещение мыши
        yaw += Input.GetAxis("Mouse X") * lookSpeed;
        pitch -= Input.GetAxis("Mouse Y") * lookSpeed;

        // Ограничиваем угол наклона вверх/вниз
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        // Вращаем камеру по осям X и Y
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }

    // Метод для перемещения камеры
    void HandleMovement()
    {
        // Получаем ввод от клавиш WASD и стрелок
        float moveX = Input.GetAxis("Horizontal"); // Лево/право
        float moveZ = Input.GetAxis("Vertical");   // Вперед/назад
        float moveY = 0f;

        // Управление вертикальным перемещением (вверх/вниз)
        if (Input.GetKey(KeyCode.E)) // Подъем
        {
            moveY = 1f;
        }
        else if (Input.GetKey(KeyCode.Q)) // Спуск
        {
            moveY = -1f;
        }

        // Перемещение с учетом направления камеры
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ + transform.up * moveY;

        // Увеличение скорости при зажатом Shift
        float currentSpeed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

        // Применяем движение к позиции камеры
        transform.position += moveDirection * currentSpeed * Time.deltaTime;
    }
}
