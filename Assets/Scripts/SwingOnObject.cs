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
    private float velocityChangeForce = 1f;

    private Rigidbody rb;
    private bool rayVisible = false;
    private Vector3 swingObjectPos = Vector3.zero;
    private bool playerHooked = false;
    Vector3 pullVector = Vector3.zero;

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
                            pullVector = (swingObjectPos - transform.position).normalized;                            
                            rb.velocity = pullVector * velocityChangeForce;                            
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
            Debug.Log(distance);
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
