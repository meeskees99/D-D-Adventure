using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance { get; private set; }

    [SerializeField] string lobbyScene = "Lobby";
    [SerializeField] string gameScene = "Game";

    private bool gameHasStarted;
    public Dictionary<ulong, ClientData> ClientData { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;

        ClientData = new Dictionary<ulong, ClientData>();

        NetworkManager.Singleton.StartHost();
    }

    void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (ClientData.Count >= 3 || gameHasStarted)
        {
            response.Approved = false;
            return;
        }

        response.Approved = true;
        response.CreatePlayerObject = false;
        response.Pending = false;

        ClientData[request.ClientNetworkId] = new ClientData(request.ClientNetworkId);

        Debug.Log($"Removed Client {request.ClientNetworkId}");
    }

    void OnNetworkReady()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

        NetworkManager.Singleton.SceneManager.LoadScene(lobbyScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    void OnClientDisconnect(ulong clientId)
    {
        if (ClientData.ContainsKey(clientId))
        {
            if (ClientData.Remove(clientId))
            {
                Debug.Log($"Removed Client {clientId}");
            }
        }
    }

    public void SetCharacter(ulong clientId, int characterId)
    {
        if (ClientData.TryGetValue(clientId, out ClientData data))
        {
            data.characterId = characterId;
        }
    }

    public void StartGame()
    {
        gameHasStarted = true;

        NetworkManager.Singleton.SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }
}
