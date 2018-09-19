using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    private const string PLAYER_PREFIX = "Player";

    [SerializeField]
    private int pointsToWin = 100;
    [SerializeField]
    private float pointsGainSpeed = 1f;
    [SerializeField]
    private float timeToStart = 3f;
    [SerializeField]
    private float minPlayers = 2f;

    [SyncVar]
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private static GameManager instance;
    private static bool canStart = false;
    private static bool matchStarted = false;

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
    public static bool MatchStarted
    {
        get
        {
            return matchStarted;
        }
    }
    public static bool CanStart
    {
        get
        {
            return canStart;
        }
    }

    private void Update()
    {
        if (!matchStarted)
        {
            if (!canStart)
            {
                if (players.Count >= minPlayers)
                {
                    canStart = true;
                    foreach (string playerID in players.Keys)
                    {
                        if (!players[playerID].IsReady) canStart = false;                        
                    }
                }

                if (canStart)
                {
                    StartCoroutine(StartMatch());
                }
            }
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

        Debug.Log(players.Count);

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

    private IEnumerator StartMatch()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yield return new WaitForSeconds(timeToStart);

        foreach (string playerID in players.Keys)
        {
            players[playerID].GetComponent<PlayerSetup>().EnableLocalComponents();
        }

        matchStarted = true;
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
