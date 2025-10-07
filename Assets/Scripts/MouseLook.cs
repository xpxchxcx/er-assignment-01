using UnityEngine;

public class CameraPlayerController : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivityX = 2f;
    public float mouseSensitivityY = 2f;
    public float minMouseY = -60f;
    public float maxMouseY = 60f;

    [Header("Camera Keyboard Settings")]
    public float panSpeed = 50f;
    public float fovAdjustSpeed = 30f;
    public float minFOV = 20f;
    public float maxFOV = 90f;
    public float verticalOffset = 2f;

    [Header("Player Settings")]
    public float playerSpeed = 5f;
    public GameObject playerGameObj;
    public Camera cameraComponent;

    private bool isMouseActive = true;

    // Camera rotation variables
    private float pitch = 0f;
    private float cameraYaw = 0f;

    void Start()
    {
        if (cameraComponent == null)
            cameraComponent = Camera.main;

        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        HandleCamera();
        HandlePlayerMovement();
    }

    void HandleCamera()
    {
        // --- Mouse look ---
        if (isMouseActive)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, minMouseY, maxMouseY);
            cameraYaw += mouseX;
        }

        // --- Keyboard pan (camera only) ---
        if (Input.GetKey(KeyCode.Q)) cameraYaw -= panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) cameraYaw += panSpeed * Time.deltaTime;

        // --- Keyboard FOV ---
        if (Input.GetKey(KeyCode.LeftShift)) cameraComponent.fieldOfView -= fovAdjustSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftControl)) cameraComponent.fieldOfView += fovAdjustSpeed * Time.deltaTime;
        cameraComponent.fieldOfView = Mathf.Clamp(cameraComponent.fieldOfView, minFOV, maxFOV);

        // --- Camera position above player ---
        Vector3 camPos = playerGameObj.transform.position + Vector3.up * verticalOffset;
        cameraComponent.transform.position = camPos;

        // --- Apply camera rotation ---
        cameraComponent.transform.rotation = Quaternion.Euler(pitch, cameraYaw, 0f);

        // --- Toggle mouse look ---
        if (Input.GetKeyDown(KeyCode.Escape))
            isMouseActive = !isMouseActive;
    }

    void HandlePlayerMovement()
    {
        float h = 0f;
        float v = 0f;

        // --- Only WASD controls movement ---
        if (Input.GetKey(KeyCode.W)) v += 1f;
        if (Input.GetKey(KeyCode.S)) v -= 1f;
        if (Input.GetKey(KeyCode.A)) h -= 1f;
        if (Input.GetKey(KeyCode.D)) h += 1f;

        if (Mathf.Approximately(h, 0f) && Mathf.Approximately(v, 0f))
            return;

        // --- Player movement relative to camera forward ---
        Vector3 forward = cameraComponent.transform.forward;
        Vector3 right = cameraComponent.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * v + right * h;
        playerGameObj.transform.position += moveDir * playerSpeed * Time.deltaTime;
    }
}
