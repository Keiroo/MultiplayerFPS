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
    private int points = 0;

    public int Points
    {
        get
        {
            return points;
        }
    }

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
            RpcAddForce(shootVector * shootPower, ForceMode.VelocityChange);
            Debug.Log(rb.velocity);
            ResetValues();
        }
    }
    
    public void AddPoint()
    {
        points++;
        Debug.Log("Point added");
    }

    public void ResetPoints()
    {
        points = 0;
    }

    public void GetShot(Vector3 shootVector, float shootPower)
    {
        Debug.Log("GetShot called");
        this.shootVector = shootVector;
        this.shootPower = shootPower;
        gotShot = true;
    }

    [ClientRpc]
    public void RpcAddForce(Vector3 force, ForceMode mode)
    {
        rb.AddForce(force, mode);
    }

    private void ResetValues()
    {
        gotShot = false;
        shootVector = Vector3.zero;
        shootPower = 0f;
    }
}
