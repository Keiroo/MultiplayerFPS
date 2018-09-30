using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] RemoteComponents;
    [SerializeField]
    private Behaviour[] LocalComponents;

    private GameObject MainCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            // Disable all remote player components
            foreach (Behaviour component in RemoteComponents)
            {
                component.enabled = false;
            }

            // Change player layer to remote
            gameObject.layer = LayerMask.NameToLayer("RemotePlayer");
        }
        else
        {
            MainCamera = Camera.main.gameObject;
            if (MainCamera != null)
            {
                MainCamera.SetActive(false);
            }

            if (!GameManager.MatchStarted)
            {
                // Disable all local player components on the start
                DisableLocalComponents();
            }            
        }
    }

    public void EnableLocalComponents()
    {
        foreach (Behaviour component in LocalComponents)
        {
            component.enabled = true;
        }
    }

    public void DisableLocalComponents()
    {
        foreach (Behaviour component in LocalComponents)
        {
            component.enabled = false;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Register player with unique ID
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.AddPlayer(netID, player);
    }

    private void OnDisable()
    {
        if (MainCamera != null)
        {
            MainCamera.SetActive(true);
        }
        GameManager.DeletePlayer(gameObject.name);
    }

}
