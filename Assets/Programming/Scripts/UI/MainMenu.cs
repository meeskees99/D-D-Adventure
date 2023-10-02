using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainMenu : MonoBehaviour
{

    public void StartHost()
    {
        ServerManager.Instance.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
