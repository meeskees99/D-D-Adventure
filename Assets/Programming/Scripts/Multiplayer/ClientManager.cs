using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [SerializeField] GameObject buttonsUI;
    [SerializeField] GameObject connectingUI;
    public static ClientManager Instance { get; private set; }
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

    public async void StartClient(string joinCode)
    {
        JoinAllocation _allocation;
        connectingUI.SetActive(true);
        buttonsUI.SetActive(false);

        try
        {
            _allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch
        {
            Debug.LogError("Relay get join code request failed");
            buttonsUI.SetActive(true);
            connectingUI.SetActive(false);
            throw;
        }

        Debug.Log($"client: {_allocation.ConnectionData[0]} {_allocation.ConnectionData[1]}");
        Debug.Log($"host: {_allocation.HostConnectionData[0]} {_allocation.HostConnectionData[1]}");
        Debug.Log($"client: {_allocation.AllocationId}");


        var relayServerData = new RelayServerData(_allocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartClient();
    }
}
