using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class EscKey : MonoBehaviour {

    [SerializeField]
    private string sceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (sceneName == "Exit" || sceneName == "Quit") Application.Quit();
            else SceneManager.LoadScene(sceneName);
        }
    }
}
