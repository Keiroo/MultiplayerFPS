using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    private const string PLAYER_PREFIX = "Player";

    [SerializeField]
    private int pointsToWin = 100;
    [SerializeField]
    private float pointsGainSpeed = 1f;
    [SerializeField]
    private float timeToStart = 3f;
    [SerializeField]
    private float minPlayers = 2f;

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static int PointsToWin { get; private set; }
    public static float PointsGainSpeed { get; private set; }
    public static bool MatchStarted { get; private set; }
    public static bool CanStart { get; private set; }

    private void Start()
    {
        PointsToWin = pointsToWin;
        PointsGainSpeed = pointsGainSpeed;
    }

    private void Update()
    {
        if (!MatchStarted)
        {
            if (!CanStart)
            {
                if (players.Count >= minPlayers)
                {
                    Debug.Log("Enough players to start");
                    CanStart = true;
                    foreach (string playerID in players.Keys)
                    {
                        if (!players[playerID].IsReady) CanStart = false;                        
                    }
                }

                if (CanStart)
                {
                    Debug.Log("Starting match");
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
        Debug.Log("Starting match coroutine");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yield return new WaitForSeconds(timeToStart);

        foreach (string playerID in players.Keys)
        {
            players[playerID].GetComponent<PlayerSetup>().EnableLocalComponents();
        }

        MatchStarted = true;
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
