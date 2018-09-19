using System.Collections;
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

    private Player localPlayer;

    private void Start()
    {
        // Enable ready button if match hasn't started
        if (readyButton != null)
        {
            if (!GameManager.MatchStarted)
                readyButton.SetActive(true);
            else readyButton.SetActive(false);
        }

        // Find local player object
        foreach (Player player in GameManager.GetPlayers())
        {
            if (player.GetComponent<PlayerSetup>().isLocalPlayer)
            {
                localPlayer = player;
            }
        }
    }

    private void Update()
    {
        // If match started or can start, disable ready button
        if (GameManager.MatchStarted || GameManager.CanStart)
        {
            readyButton.SetActive(false);
        }
    }

    public void OnReadyButtonClick()
    {
        if (!GameManager.MatchStarted &&
            !GameManager.CanStart)
        {
            ColorBlock cb = readyButton.GetComponent<Button>().colors;
            if (!localPlayer.IsReady)
            {
                localPlayer.IsReady = true;
                cb.normalColor = readyColor;
            }
            else
            {
                localPlayer.IsReady = false;                
                cb.normalColor = notReadyColor;
            }
            readyButton.GetComponent<Button>().colors = cb;
        }
    }
}
