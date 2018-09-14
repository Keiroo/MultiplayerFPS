using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float range = 20f;
    [SerializeField]
    private float shootPower = 1f;

    private Transform camTrans;
    private bool shoot = false;
    Rigidbody rb;
    private Vector3 shootVector = Vector3.zero;

    private void Start()
    {
        camTrans = cam.transform;        
    }

    private void Update()
    {        
        bool btnPressed = Input.GetButtonDown("Fire1");

        if (btnPressed)
        {
            RaycastHit hit;
            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, range))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    Shoot(hit.transform.gameObject);
                }
            }
        }
    }


    private void Shoot(GameObject obj)
    {
        rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            shootVector = (obj.transform.position - transform.position).normalized;
            shoot = true;
        }
        else
        {
            Debug.Log("ShotgunShoot: Rigidbody not found");
        }
    }

    private void FixedUpdate()
    {
        if (shoot)
        {
            rb.AddForce(shootVector * shootPower, ForceMode.VelocityChange);
            rb = null;
            shoot = false;
        }
    }
}
