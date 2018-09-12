using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SwingOnObject : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float pullForce = 1f;
    [SerializeField]
    private float pullMinDistance = 1f;
    [SerializeField]
    private float velocityChangeForce = 0.1f;
    [SerializeField]
    private float maxVelocityMagnitude = 100f;
    [SerializeField]
    private float maxHookDistance = 100f;
    [SerializeField]
    private float maxPlayerSpeed = 100f;

    private Rigidbody rb;
    private Transform camTrans;
    private bool rayVisible = false;
    private Vector3 swingObjectPos = Vector3.zero;
    private bool playerHooked = false;
    private Vector3 pullVector = Vector3.zero;
    private float velocityMagnitude = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        camTrans = cam.transform;
    }

    private void Update ()
    {
        bool btnPressed = Input.GetButtonDown("Fire2");

        if (btnPressed)
        {
            if (rayVisible)
            {
                CutHook();
            }
            else
            {
                //Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));                
                RaycastHit hit;

                if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, maxHookDistance))
                {
                    if (hit.collider.gameObject.tag == "SwingObject")
                    {
                        rayVisible = true;
                        swingObjectPos = hit.collider.gameObject.transform.position;

                        // Activate grapling hook
                        if (!playerHooked)
                        {
                            playerHooked = true;
                            Pull();
                        }
                        Debug.Log(Vector3.Magnitude(rb.velocity));
                    }
                }
            }
        }

        // Draw ray for debugging
        if (rayVisible)
        {
            Debug.DrawRay(transform.position, swingObjectPos - transform.position, Color.green);
        }

        // Pull player until he reaches min. distance to object
        if (playerHooked)
        {
            CutHookByMinDistance();
        }
    }

    private void FixedUpdate()
    {
        if (playerHooked)
        {
            rb.AddForce(pullVector * pullForce, ForceMode.VelocityChange);
        }

        // Constaint max player speed
        if (Vector3.Magnitude(rb.velocity) > maxPlayerSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxPlayerSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reset saved velocity magnitude and
        // and cut hook when hit ground
        if (collision.gameObject.tag == "Ground")
        {
            velocityMagnitude = 0f;
            CutHook();
        }
    }

    private void Pull()
    {
        // Calculate pull vector and current velocity magnitude
        pullVector = (swingObjectPos - transform.position) * velocityChangeForce;
        velocityMagnitude = Vector3.Magnitude(rb.velocity);

        // If old magnitude is greater, apply it to new vector
        float magnitude = Vector3.Magnitude(pullVector);
        if (velocityMagnitude > magnitude)
            pullVector = pullVector.normalized * velocityMagnitude;

        // Check if new magnitude isn't exceeding max magnitude
        magnitude = Vector3.Magnitude(pullVector);
        if (magnitude > maxVelocityMagnitude)
            pullVector = pullVector.normalized * maxVelocityMagnitude;

        // Apply new velocity vector and normalize pull vector
        // for future calculations
        rb.velocity = pullVector;
    }

    private void CutHookByMinDistance()
    {
        // Recalculate pull vector (for better pull precision)
        pullVector = (swingObjectPos - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, swingObjectPos);
        if (distance < pullMinDistance)
        {
            CutHook();
        }
    }

    private void CutHook()
    {
        rayVisible = false;
        playerHooked = false;
    }
}
