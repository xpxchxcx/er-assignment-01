using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[AddComponentMenu("Camera-Control/ Mouse Look")]
public class MouseLook : MonoBehaviour
{
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;

    public float minY = -60f;
    public float maxY = 60f;
    public float minX = -60f;
    public float maxX = -60f;

    private bool isMouseActive = true;
    private float rotY = 0f;


    public GameObject playerGameObj;
    public GameObject cameraGameObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        cameraGameObj.transform.position = playerGameObj.transform.position;
        Vector2 delta = Mouse.current.delta.ReadValue();
        float mouseX = delta.x;
        float mouseY = delta.y;

        if (isMouseActive)
        {
            float rotX = cameraGameObj.transform.localEulerAngles.y + mouseX * sensitivityX;
            rotY += mouseY * sensitivityY;
            rotY = Mathf.Clamp(rotY, minY, maxY);
            rotX += mouseX * sensitivityX;
            cameraGameObj.transform.localEulerAngles = new Vector3(-rotY, rotX, 0);

        }

        if (Input.GetKey(KeyCode.Escape))
        {
            isMouseActive = !isMouseActive;
            if (isMouseActive)
            {
                Debug.Log("Mouse active!");
            }
            else
            {
                Debug.Log("Mouse inactive");
            }
        }
    }
}
