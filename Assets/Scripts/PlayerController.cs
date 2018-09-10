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
    private float maxCameraAngle = 89f;
    [SerializeField]
    private float jumpHeight = 1f;
    [SerializeField]
    private float jumpForce = 1f;
    [SerializeField]
    private float jumpCooldown = 1f;
    [SerializeField]
    private float airVelocityForce = 1f;
    [SerializeField]
    private float upAirVelocityForce = 1f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float camRotation = 0f;
    private Vector3 jumpVector = Vector3.zero;
    private Vector3 airVelocity = Vector3.zero;
    private Rigidbody rb;
    private bool isGrounded;
    private bool jumpedWithoutVelocity;
    private float jumpCooldownTimer = 0f;
    private Vector3 lastJumpVector = Vector3.zero;
    private float currCamRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Reset model rotation
        gameObject.transform.rotation = Quaternion.Euler(
            new Vector3(0, gameObject.transform.rotation.eulerAngles.y, 0));

        // Cooldown timer
        if (isGrounded) jumpCooldownTimer += Time.deltaTime;

        // Get movement
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
        else
        {
            velocity = Vector3.zero;
        }
        
        rotation = new Vector3(0f, rotY, 0f) * mouseSensitivity;
        camRotation = rotX * mouseSensitivity;

        // Calculate jump vector
        if (Input.GetButton("Jump") && isGrounded && jumpCooldownTimer >= jumpCooldown)
        {
            if (velocity == Vector3.zero)
            {
                jumpedWithoutVelocity = true;
                jumpVector = Vector3.up / 2f;
            }
            else
            {
                jumpedWithoutVelocity = false;
                jumpVector = ((Vector3.up * jumpHeight) + velocity).normalized;
            }
            isGrounded = false;

            lastJumpVector = jumpVector;
        }
        else jumpVector = Vector3.zero;

        // If in air, weaken the move force
        if (!isGrounded)
        {
            // Calculate basic velocity
            airVelocity = ((vecHorizontal + vecVertical).normalized * (movSpeed / 5.0f)).normalized;

            // Disable moving in the jump direction
            if (!jumpedWithoutVelocity)
            {
                // Check if player move in jump direction
                float angle = Vector3.Angle(new Vector3(lastJumpVector.x, 0, lastJumpVector.z), airVelocity);
                if (angle < 90)
                {
                    // Calculate rotation direction from jump vector
                    Vector3 vectorLeft = Quaternion.Euler(0f, -90f, 0f) * lastJumpVector;
                    Vector3 vectorRight = Quaternion.Euler(0f, 90f, 0f) * lastJumpVector;

                    float angleLeft = Vector3.SignedAngle(new Vector3(vectorLeft.x, 0, vectorLeft.z), airVelocity, Vector3.up);
                    float angleRight = Vector3.SignedAngle(new Vector3(vectorRight.x, 0, vectorRight.z), airVelocity, Vector3.up);

                    // Rotate vector and calculate length of vector by angle:
                    // [90,0] == [0,1]
                    if (Mathf.Abs(angleLeft) < Mathf.Abs(angleRight))
                    {
                        airVelocity = Quaternion.Euler(0f, -angleLeft, 0f) * airVelocity;
                        airVelocity *= CalcVectorLength(angleLeft);
                    }
                    else if (Mathf.Abs(angleLeft) > Mathf.Abs(angleRight))
                    {
                        airVelocity = Quaternion.Euler(0f, -angleRight, 0f) * airVelocity;
                        airVelocity *= CalcVectorLength(angleRight);
                    }
                    else airVelocity = Vector3.zero;
                }                
            }
        }
        else airVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        // Apply player movement, rotation and camera rotation
        if (velocity != Vector3.zero && isGrounded)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        else if (velocity != Vector3.zero)
        {
            rb.AddForce(velocity / 0.5f, ForceMode.Force);
        }
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (cam != null)
        {
            currCamRotation -= camRotation;
            currCamRotation = Mathf.Clamp(currCamRotation, -maxCameraAngle, maxCameraAngle);
            cam.transform.localEulerAngles = new Vector3(currCamRotation, 0f, 0f);
        }

        // Apply jump
        if (jumpVector != Vector3.zero)
        {
            rb.AddForce(jumpVector * jumpForce, ForceMode.Impulse);            
        }

        // When in air, use AddForce instead of MovePosition
        if (airVelocity != Vector3.zero && !isGrounded)
        {
            if (jumpedWithoutVelocity)
            {
                rb.AddForce(airVelocity * upAirVelocityForce, ForceMode.Force);
            }
            else rb.AddForce(airVelocity * airVelocityForce, ForceMode.Force);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    // Reset timer when hitting ground
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCooldownTimer = 0f;
        }
    }

    private float CalcVectorLength(float angle)
    {
        return 1 - (Mathf.Abs(angle) / 90f);
    }
}
