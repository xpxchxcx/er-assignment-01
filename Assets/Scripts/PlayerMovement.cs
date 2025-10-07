using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 0.4f;
    [SerializeField] private float movementSpeed = 5f;

    [Header("Camera Reference")]
    [SerializeField] private Transform cam;

    [Header("Ground Check Settings")]
    [SerializeField] private Vector3 boxCastSize = new Vector3(0.5f, 0.1f, 0.5f); // half extents of box
    [SerializeField] private float groundCheckDistance = 0.2f; // how far below to check
    [SerializeField] private LayerMask groundMask; // assign "Ground" layer in Inspector

    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        GroundCheck();

        Debug.Log($"Grounded: {isGrounded}");

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // get forward/right relative to camera
        Vector3 forward = cam.forward; forward.y = 0f; forward.Normalize();
        Vector3 right = cam.right; right.y = 0f; right.Normalize();

        // movement vector
        Vector3 moveDir = (forward * v + right * h).normalized;

        // set velocity (X/Z only)
        Vector3 targetVelocity = moveDir * movementSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void GroundCheck()
    {
        // start position slightly above player bottom
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.BoxCast(origin, boxCastSize, Vector3.down, Quaternion.identity, groundCheckDistance, groundMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        Gizmos.matrix = Matrix4x4.TRS(origin, Quaternion.identity, Vector3.one);
        Gizmos.DrawWireCube(Vector3.down * groundCheckDistance, boxCastSize * 2);
    }
}
