using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private const string PLAYER_PREFIX = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

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
            GUILayout.Label(playerID + " | " + players[playerID].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
