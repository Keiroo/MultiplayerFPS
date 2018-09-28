using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
[RequireComponent(typeof(Rigidbody))]
public class Player : NetworkBehaviour {

    [Header("Respawn settings")]
    [SerializeField]
    private float minHeight = -30f;
    [SerializeField]
    private float respawnX = 100f;
    [SerializeField]
    private float respawnY = 2f;
    [SerializeField]
    private float respawnZ = 100f;

    [SyncVar]
    private int points = 0;

    private bool gotShot = false;
    private Vector3 shootVector = Vector3.zero;
    private float shootPower = 0f;
    private Rigidbody rb;
    private Vector3 respawnPoint;

    public int Points
    {
        get
        {
            return points;
        }
    }

    [SyncVar]
    public bool IsReady = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        respawnPoint = new Vector3(respawnX, respawnY, respawnZ);

        if (GameManager.MatchStarted) IsReady = true;
    }

    private void Update()
    {
        if (transform.position.y < minHeight)
            ResetPlayerPos();
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

    private void ResetPlayerPos()
    {
        rb.velocity = Vector3.zero;
        transform.position = respawnPoint;
    }

    private void ResetValues()
    {
        gotShot = false;
        shootVector = Vector3.zero;
        shootPower = 0f;
    }
}
