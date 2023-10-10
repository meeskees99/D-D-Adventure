using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using TMPro;

public class HostManager : NetworkBehaviour
{
    public static HostManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] int maxConnections = 3;
    [SerializeField] string lobbyScene = "Lobby";
    [SerializeField] string gameScene = "Game";
    [SerializeField] TMP_Dropdown mapSelect;

    private bool gameHasStarted;
    public Dictionary<ulong, ClientData> ClientData { get; private set; }

    public string JoinCode { get; private set; }

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

    void Update()
    {
        // if (!IsHost)
        // {
        //     mapSelect.gameObject.SetActive(false);
        //     return;
        // }
        if (mapSelect == null)
        {
            mapSelect = FindObjectOfType<TMP_Dropdown>();
        }
        else if (gameScene != mapSelect.options[mapSelect.value].text)
        {
            gameScene = mapSelect.options[mapSelect.value].text;
            UpdateMapSelectDisplayClientRpc(mapSelect.value);
        }
    }

    public async void StartHost()
    {
        Allocation _allocation;

        try
        {
            _allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay Create allocation request failed {e.Message}");
            throw;
        }

        Debug.Log($"server: {_allocation.ConnectionData[0]} {_allocation.ConnectionData[1]}");
        Debug.Log($"server: {_allocation.AllocationId}");

        try
        {
            JoinCode = await RelayService.Instance.GetJoinCodeAsync(_allocation.AllocationId);
        }
        catch
        {
            Debug.LogError("Relay get join code request failed");
            throw;
        }

        var relayServerData = new RelayServerData(_allocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

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

        _ = NetworkManager.Singleton.SceneManager.LoadScene(lobbyScene, LoadSceneMode.Single);
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
    [ClientRpc]
    public void UpdateMapSelectDisplayClientRpc(int index, ClientRpcParams clientRpcParams = default)
    {
        mapSelect.value = index;
        mapSelect.RefreshShownValue();
        print("Updated Map Selection");
    }
}
