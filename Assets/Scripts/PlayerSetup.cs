using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] Components;
    GameObject MainCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (Behaviour component in Components)
            {
                component.enabled = false;
            }
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

    private void OnDisable()
    {
        if (MainCamera != null)
        {
            MainCamera.SetActive(true);
        }
    }

}
