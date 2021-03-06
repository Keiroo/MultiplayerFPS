﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    [SerializeField]
    private GameObject readyButton;
    [SerializeField]
    private Color readyColor;
    [SerializeField]
    private Color notReadyColor;
    [SerializeField]
    private GameObject startText;
    [SerializeField]
    private string startingText;
    [SerializeField]
    private string afterStartText;
    [SerializeField]
    private float afterStartTextDuration = 1f;
    [SerializeField]
    private string endMatchText;

    private Player localPlayer;
    private bool matchStarted;

    private void Start()
    {
        // Check if joined to started match
        if (GameManager.MatchStarted) matchStarted = true;
        else matchStarted = false;

        // Enable ready button if match hasn't started
        if (readyButton != null)
        {
            if (!matchStarted) readyButton.SetActive(true);
            else readyButton.SetActive(false);
        }

        StartCoroutine(FindLocalPlayer());
    }

    private void Update()
    {
        // If joined to not started match, enable button and text
        if (!matchStarted)
        {
            // If match started or can start, disable ready button
            if (GameManager.MatchStarted || GameManager.CanStart)
            {
                readyButton.SetActive(false);
            }

            // If match is starting, show startingText
            if (GameManager.CanStart)
            {
                ShowStartingText();
            }

            // If match started, show afterStartText
            if (GameManager.MatchStarted)
            {
                StartCoroutine(ShowAfterStartText());
            }
        }

        if (GameManager.MatchEnded)
        {
            ShowEndMatchText();
        }
    }

    public void OnReadyButtonClick()
    {
        if (!GameManager.MatchStarted &&
            !GameManager.CanStart)
        {
            Color cb = readyButton.GetComponent<Image>().color;

            if (cb != null)
            {
                if (!localPlayer.IsReady)
                {
                    localPlayer.CmdPlayerReady(true);
                    cb = readyColor;
                }
                else
                {
                    localPlayer.CmdPlayerReady(false);
                    cb = notReadyColor;
                }
                readyButton.GetComponent<Image>().color = cb;
            }
        }
    }

    private void ShowEndMatchText()
    {
        startText.GetComponent<Text>().text = endMatchText;

        // Find player with most points
        List<Player> players = GameManager.GetPlayers();
        int bestPlayerID = -1;

        for (int i = 0; i < players.Count; i++)
        {
            if (bestPlayerID == -1)
            {
                bestPlayerID = i;
            }
            else if (players[i].Points > players[bestPlayerID].Points)
            {
                bestPlayerID = i;
            }
        }

        // If found best player, add score to text
        if (bestPlayerID != -1)
        {
            startText.GetComponent<Text>().text +=
                "\n Winner: " +
                players[bestPlayerID].name;
        }

        startText.SetActive(true);
    }

    private void ShowStartingText()
    {
        startText.GetComponent<Text>().text = startingText;
        startText.SetActive(true);
    }

    private IEnumerator ShowAfterStartText()
    {
        startText.GetComponent<Text>().text = afterStartText;
        startText.SetActive(true);
        yield return new WaitForSeconds(afterStartTextDuration);
        startText.SetActive(false);

        // Disable unnecessary if checks in Update()
        matchStarted = true;
    }

    private IEnumerator FindLocalPlayer()
    {
        do
        {
            // Find local player object
            foreach (Player player in GameManager.GetPlayers())
            {
                if (player.GetComponent<PlayerSetup>().isLocalPlayer)
                {
                    localPlayer = player;
                }
                Debug.Log(localPlayer);
            }
            yield return null;
        } while (localPlayer == null);
    }
}
