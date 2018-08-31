using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float movSpeed = 10f;
    [SerializeField]
    private float mouseSensitivity = 3f;
    [SerializeField]
    private float jumpHeight = 1f;
    [SerializeField]
    private float jumpForce = 1f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 camRotation = Vector3.zero;
    private Vector3 jumpVector = Vector3.zero;
    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");
        float rotY = Input.GetAxisRaw("Mouse X");
        float rotX = Input.GetAxisRaw("Mouse Y");

        // Calculate player movement, rotation and camera rotation
        Vector3 vecHorizontal = transform.right * movX;
        Vector3 vecVertical = transform.forward * movY;

        // Check if player is on the ground
        if (isGrounded)
        {
            velocity = (vecHorizontal + vecVertical).normalized * movSpeed;
        }
        
        rotation = new Vector3(0f, rotY, 0f) * mouseSensitivity;
        camRotation = new Vector3(rotX, 0f, 0f) * mouseSensitivity;

        // Calculate jump vector
        if (Input.GetButton("Jump") && isGrounded)
        {
            if (velocity == Vector3.zero)
            {
                jumpVector = Vector3.up;
            }
            else
            {
                jumpVector = ((Vector3.up * jumpHeight) + velocity.normalized).normalized;
            }
            isGrounded = false;
        }
        else jumpVector = Vector3.zero;
    }

    private void FixedUpdate()
    {
        // Apply player movement, rotation and camera rotation
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (cam != null)
        {
            cam.transform.Rotate(-camRotation);
        }

        // Apply jump
        if (jumpVector != Vector3.zero)
        {
            rb.AddForce(jumpVector * jumpForce, ForceMode.Impulse);            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
