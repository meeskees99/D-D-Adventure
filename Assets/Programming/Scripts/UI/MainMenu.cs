using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string lobbyScene = "Lobby";

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(lobbyScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        // NetworkManager.Singleton.SceneManager.LoadScene(lobbyScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
