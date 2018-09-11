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

    private Rigidbody rb;
    private bool rayVisible = false;
    private Vector3 swingObjectPos = Vector3.zero;
    private bool playerHooked = false;
    private Vector3 pullVector = Vector3.zero;
    private float velocityMagnitude = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update ()
    {
        bool btnPressed = Input.GetButtonDown("Fire2");

        if (btnPressed)
        {
            if (rayVisible)
            {
                rayVisible = false;
                playerHooked = false;
            }
            else
            {
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "SwingObject")
                    {
                        rayVisible = true;
                        swingObjectPos = hit.collider.gameObject.transform.position;

                        // Activate grapling hook
                        if (!playerHooked)
                        {
                            playerHooked = true;

                            // Calculate pull vector and current velocity magnitude
                            pullVector = (swingObjectPos - transform.position);
                            velocityMagnitude = Vector3.Magnitude(rb.velocity);

                            // Apply new velocity vector and normalize pull vector
                            // for future calculations
                            rb.velocity = pullVector * velocityChangeForce;
                            pullVector = pullVector.normalized;

                            // If old magnitude is greater, apply it to new vector
                            float magnitude = Vector3.Magnitude(rb.velocity);
                            if (velocityMagnitude > magnitude)
                            {
                                // Check if new magnitude isn't exceeding max magnitude
                                if (velocityMagnitude < maxVelocityMagnitude)
                                    rb.velocity = rb.velocity.normalized * velocityMagnitude;
                            }
                        }
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
            
            pullVector = (swingObjectPos - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, swingObjectPos);
            if (distance < pullMinDistance)
            {
                playerHooked = false;
                rayVisible = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerHooked)
        {
            rb.AddForce(pullVector * pullForce, ForceMode.VelocityChange);
        }
    }
}
