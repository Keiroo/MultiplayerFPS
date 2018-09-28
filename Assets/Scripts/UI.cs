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

        StartCoroutine(FindLocalPlayer());
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
            Color cb = readyButton.GetComponent<Image>().color;

            if (cb != null)
            {
                if (!localPlayer.IsReady)
                {
                    localPlayer.IsReady = true;
                    cb = readyColor;
                }
                else
                {
                    localPlayer.IsReady = false;
                    cb = notReadyColor;
                }
                readyButton.GetComponent<Image>().color = cb;
            }
        }
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
