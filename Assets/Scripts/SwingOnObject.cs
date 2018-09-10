using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingOnObject : MonoBehaviour {

    [SerializeField]
    private Camera cam;

    private bool rayVisible = false;
    private Vector3 swingObjectPos = Vector3.zero;

    private void Start()
    {
    }

    private void Update ()
    {
        bool btnPressed = Input.GetButtonDown("Fire2");

        if (btnPressed)
        {
            if (rayVisible)
            {
                rayVisible = false;
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
                    }
                        
                }
            }
        }

        if (rayVisible)
        {
            Debug.DrawRay(transform.position, swingObjectPos - transform.position, Color.green);
        }

        Debug.Log(rayVisible);
    }
}
