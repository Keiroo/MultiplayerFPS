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
    private GameObject netManager;
    private List<GameObject> spawnPoints;

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

        netManager = GameManager.Instance.Spawns;
        spawnPoints = new List<GameObject>();
        StartCoroutine(LateStart(1f));
    }

    private void Update()
    {
        if (transform.position.y < -10f)
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
        bool spawnFound = false;
        int spawnID = 0;

        if (spawnPoints.Count > 0)
        {
            Debug.Log("Spawns more than 0");
            do
            {
                spawnID = UnityEngine.Random.Range(0, spawnPoints.Count);
                spawnFound = true;
                List<Player> players = GameManager.GetPlayers();

                foreach (Player player in players)
                {
                    if (player.transform.position == spawnPoints[spawnID].transform.position)
                    {
                        spawnFound = false;
                    }
                }
            } while (!spawnFound);
        }
        
        if (spawnFound)
        {
            Debug.Log("Resetting position");
            rb.velocity = Vector3.zero;
            transform.position = spawnPoints[spawnID].transform.position;
        }

    }

    private void ResetValues()
    {
        gotShot = false;
        shootVector = Vector3.zero;
        shootPower = 0f;
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (netManager != null)
        {
            Debug.Log("NM found");
            foreach (Transform child in netManager.transform)
            {
                spawnPoints.Add(child.gameObject);
            }
        }
        else
        {
            Debug.Log("NM not found");
        }
    }
}
