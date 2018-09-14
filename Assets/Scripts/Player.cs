using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class Player : NetworkBehaviour {

    private bool gotShot = false;
    private Vector3 shootVector = Vector3.zero;
    private float shootPower = 0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (gotShot)
        {
            Debug.Log("Adding force to " + gameObject.name);
            Debug.Log(shootVector);
            rb.AddForce(shootVector * shootPower, ForceMode.VelocityChange);
            Debug.Log(rb.velocity);
            ResetValues();
        }
    }

    public void GetShot(Vector3 shootVector, float shootPower)
    {
        Debug.Log("GetShot called");
        this.shootVector = shootVector;
        this.shootPower = shootPower;
        gotShot = true;
    }

    private void ResetValues()
    {
        gotShot = false;
        shootVector = Vector3.zero;
        shootPower = 0f;
    }
}
