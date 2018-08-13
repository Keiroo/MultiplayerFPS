using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float movSpeed = 10f;
    [SerializeField]
    private float mouseSensitivity = 3f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 camRotation = Vector3.zero;
    private Rigidbody rb;

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

        Vector3 vecHorizontal = transform.right * movX;
        Vector3 vecVertical = transform.forward * movY;

        velocity = (vecHorizontal + vecVertical).normalized * movSpeed;
        rotation = new Vector3(0f, rotY, 0f) * mouseSensitivity;
        camRotation = new Vector3(rotX, 0f, 0f) * mouseSensitivity;
    }

    private void FixedUpdate()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null)
        {
            cam.transform.Rotate(-camRotation);
        }
    }
}
