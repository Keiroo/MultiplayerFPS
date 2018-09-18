using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private const string PLAYER_PREFIX = "Player";

    [SerializeField]
    private int pointsToWin = 100;
    [SerializeField]
    private float pointsGainSpeed = 1f;
    [SerializeField]
    private GameObject spawns;

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private static GameManager instance;
    private GameManager() { }

    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = new GameManager();
            return instance;
        }
    }
    public int PointsToWin
    {
        get
        {
            return pointsToWin;
        }
    }
    public float PointsGainSpeed
    {
        get
        {
            return pointsGainSpeed;
        }
    }
    public GameObject Spawns
    {
        get
        {
            Debug.Log("Returning NM");
            Debug.Log(spawns);
            return spawns;
        }
    }

    public static void AddPlayer(string netID, Player player)
    {
        string playerID = PLAYER_PREFIX + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
    }

    public static Player GetPlayerByID(string playerID)
    {
        return players[playerID];
    }

    public static List<Player> GetPlayers()
    {
        List<Player> playersList = new List<Player>();

        foreach (string playerID in players.Keys)
        {
            playersList.Add(players[playerID]);
        }

        return playersList;
    }

    public static void DeletePlayer(string playerID)
    {
        players.Remove(playerID);
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(300, 10, 200, 500));
        GUILayout.BeginVertical();
        foreach(string playerID in players.Keys)
        {
            GUILayout.Label(playerID + " | " + players[playerID].Points);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
