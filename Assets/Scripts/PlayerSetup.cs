using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] Components;
    GameObject MainCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            // Disable all remote player components
            foreach (Behaviour component in Components)
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
