using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Objective : MonoBehaviour {

    private int pointsToWin;
    private float pointsGainSpeed;
    private List<Player> players;
    private float timer;

    private void Start()
    {
        players = new List<Player>();
        pointsToWin = GameManager.PointsToWin;
        pointsGainSpeed = GameManager.PointsGainSpeed;
    }

    private void Update()
    {
        if (players.Count == 1)
        {
            Debug.Log("One player detected");
            timer += Time.deltaTime;
            if (timer > pointsGainSpeed)
            {
                Debug.Log("Adding point");
                players.First().AddPoint();
                timer = 0f;
            }                
        }
        else timer = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            players.Add(other.gameObject.GetComponent<Player>());
        }
        Debug.Log("Player added");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (players.Contains(player))
            {
                players.Remove(player);
                Debug.Log("Player removed");
            }
            else
            {
                Debug.LogError("Objective: Player wasn't added on enter: " +
                    player.gameObject.name);
            }
        }
    }
}
