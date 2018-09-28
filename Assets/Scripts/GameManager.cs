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
    private static bool canStart = false;
    private static bool matchStarted = false;

    public static int PointsToWin { get; private set; }
    public static float PointsGainSpeed { get; private set; }
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

    private void Start()
    {
        PointsToWin = pointsToWin;
        PointsGainSpeed = pointsGainSpeed;
    }

    private void Update()
    {
        if (!matchStarted)
        {
            CmdCheckIfCanStartMatch();
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

    [Command]
    private void CmdCheckIfCanStartMatch()
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
                CmdStartMatch();
            }
        }
    }

    [Command]
    private void CmdStartMatch()
    {
        Debug.Log("Start match command");
        // Debug: write players ready state
        foreach (string playerID in players.Keys)
        {
            Debug.Log(playerID + " " + players[playerID].IsReady);
        }

        // First, execute start match on sever
        StartCoroutine(StartMatchCoroutine());
        // Then, order clients to do the same
        RpcStartMatch();
    }

    [ClientRpc]
    private void RpcStartMatch()
    {
        Debug.Log("Start match RPC");
        StartCoroutine(StartMatchCoroutine());
    }

    private IEnumerator StartMatchCoroutine()
    {
        Debug.Log("Start match coroutine");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yield return new WaitForSeconds(timeToStart);

        foreach (string id in players.Keys)
        {
            if (players[id].isLocalPlayer)
            {
                Debug.Log("localPlayer found");
                players[id].GetComponent<PlayerSetup>().EnableLocalComponents();
            }
                
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
