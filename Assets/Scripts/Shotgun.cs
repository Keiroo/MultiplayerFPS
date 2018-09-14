using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shotgun : NetworkBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private float range = 20f;
    [SerializeField]
    private float shootPower = 1f;
    [SerializeField]
    private float shootCooldown = 1f;

    private Transform camTrans;
    private float CDTimer = 0f;

    private void Start()
    {
        camTrans = cam.transform;        
    }

    private void Update()
    {
        CDTimer += Time.deltaTime;

        bool btnPressed = Input.GetButtonDown("Fire1");

        if (btnPressed)
        {
            if (CDTimer > shootCooldown)
            {
                Shoot();
                CDTimer = 0f;
            }                
        }
    }    

    [Client]
    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, range, mask))
        {
            GameObject target = hit.transform.gameObject;
            if (target.tag == "Player")
            {
                Debug.Log(target.name + " found");
                CmdShoot(target.name);
            }
        }        
    }

    [Command]
    private void CmdShoot(string targetID)
    {
        Debug.Log("CmdShoot called");
        Player player = GameManager.GetPlayerByID(targetID);
        Debug.Log(gameObject.name + " " + transform.position);
        Debug.Log(player.transform.name + " " + player.transform.position);
        Vector3 shootVector = (player.transform.position - transform.position).normalized;
        player.GetShot(shootVector, shootPower);
    }
}
