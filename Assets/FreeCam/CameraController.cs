using Frank;
using UnityEngine;

public enum CameraType
{
    Free,
    Lock
}

public class CameraController : MonoBehaviour
{
    public CameraType type;
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float fastSpeedMultiplier = 3f;

    [Header("Mouse Settings")]
    public float lookSensitivity = 2f;

    private float yaw;
    private float pitch;

    // Thêm các biến để giới hạn vùng di chuyển
    [Header("Movement Limit")]
    public Vector3 minPosition = new Vector3(-10, 1, -10);
    public Vector3 maxPosition = new Vector3(10, 10, 10);

    private void Start()
    {
        Vector3 rot = transform.eulerAngles;
        yaw = rot.y;
        pitch = rot.x;
        xRotation = rot.x;
        yRotation = rot.y;
    }

    private void Update()
    {
        if (type == CameraType.Free)
        {
            HandleMouseLook();
            HandleMovement();
        }
        else // Lock
        {
            if (isModeAroundTarget)
                MouseInput();
            else
                HandleClickAndDrag();
        }
    }

    void LateUpdate()
    {
        if (type == CameraType.Lock && isModeAroundTarget && target)
        {
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            Vector3 dir = new Vector3(0, 0, -distance);
            transform.position = target.position + rotation * dir;
            transform.LookAt(target);
        }

        ClampPosition();
    }

    public void SetType(CameraType type)
    {
        this.type = type;
    }

    private void HandleMouseLook()
    {
        if (!Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -85f, 85f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void HandleMovement()
    {
        if (!Input.GetMouseButton(1)) return;

        Vector3 dir = Vector3.zero;
        dir += transform.forward * Input.GetAxis("Vertical");
        dir += transform.right * Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.E)) dir += Vector3.up;
        if (Input.GetKey(KeyCode.Q)) dir += Vector3.down;

        float speed = Input.GetKey(KeyCode.LeftShift)
            ? moveSpeed * fastSpeedMultiplier
            : moveSpeed;

        transform.position += dir * speed * Time.deltaTime;
    }

    // Hàm giới hạn vị trí camera
    private void ClampPosition()
    {
        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, minPosition.x, maxPosition.x);
        clamped.y = Mathf.Clamp(clamped.y, minPosition.y, maxPosition.y);
        clamped.z = Mathf.Clamp(clamped.z, minPosition.z, maxPosition.z);
        transform.position = clamped;
    }

    public Transform target;              // Object để quan sát
    public float distance;           // Khoảng cách ban đầu
    public float zoomSpeed = 1f;
    public float minDistance;
    public float maxDistance;

    public float rotationSpeed = 5f;
    public float panSpeed = 0.1f;

    private float currentX = 0f;
    private float currentY = 0f;

    private Vector3 lastMousePosition;
    public float sensitivity = 200f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private bool isDragging = false;
    public bool isModeAroundTarget;

    public void OnCameraAroundTarget(Transform target, float distance)
    {
        SetType(CameraType.Lock);
        this.target = target;
        this.distance = distance;
        isModeAroundTarget = true;
    }

    public void OnClickAndDrag(Vector3 targetPosition, Vector3 angles)
    {
        SetType(CameraType.Lock);
        transform.position = targetPosition;
        transform.localEulerAngles = angles;
        Vector3 a = transform.eulerAngles;
        xRotation = a.x;
        yRotation = a.y;
        isModeAroundTarget = false;
    }

    private void HandleClickAndDrag()
    {
        // Nhấn giữ chuột phải để xoay
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if (isDragging)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // giới hạn nhìn lên/xuống

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    private void MouseInput()
    {
        // ===== Xử lý xoay bằng chuột (click chuột phải) =====
        if (Input.GetMouseButton(1)) // Chuột phải
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            currentX += mouseX * rotationSpeed;
            currentY -= mouseY * rotationSpeed;
            currentY = Mathf.Clamp(currentY, -85, 85);
        }

        // ===== Xử lý zoom bằng scroll chuột =====
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // ===== Di chuyển (pan) bằng chuột giữa =====
        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 right = transform.right;
            Vector3 up = transform.up;

            Vector3 pan = (-right * delta.x + -up * delta.y) * panSpeed * Time.deltaTime * 100f;
            target.position += pan;
        }
        lastMousePosition = Input.mousePosition;
    }

    // Vẽ Gizmo để nhìn rõ vùng giới hạn di chuyển
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = (minPosition + maxPosition) * 0.5f;
        Vector3 size = maxPosition - minPosition;
        Gizmos.DrawWireCube(center, size);
    }
}