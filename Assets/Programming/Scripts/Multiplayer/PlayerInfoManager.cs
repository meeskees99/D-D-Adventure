using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInfoManager : NetworkBehaviour
{
    InitiativeHandler initiativeHandler;
    NetworkList<PlayerId> players;

    private void Awake()
    {
        players = new NetworkList<PlayerId>();
    }

    private void Start()
    {
        initiativeHandler = FindObjectOfType<InitiativeHandler>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            players.OnListChanged += HandlePlayersStateChanged;
        }
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            players.OnListChanged -= HandlePlayersStateChanged;
        }
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        players.Add(new PlayerId(clientId));
    }
    void HandleClientDisconnected(ulong clientId)
    {
        if (!IsServer) return;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Id == clientId)
            {
                players.RemoveAt(i);
                break;
            }
        }
    }

    void HandlePlayersStateChanged(NetworkListEvent<PlayerId> changeEvent)
    {
        if (!IsServer) return;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Id == 0)
            {
                players[i] = new PlayerId(
                    players[i].Id,
                    true,
                    players[i].IsTurn
                );
            }
            else
            {
                players[i] = new PlayerId(
                    players[i].Id,
                    false,
                    players[i].IsTurn
                                );
            }
        }
    }

    public void NextTurn(ulong playerId)
    {
        NextTurnServerRPC(playerId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextTurnServerRPC(ulong playerId, ServerRpcParams serverRpcParams = default)
    {
        print($"Next turn for player {playerId}");
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Id == serverRpcParams.Receive.SenderClientId)
            {
                players[i] = new PlayerId(
                    players[i].Id,
                    players[i].IsDungeonMaster,
                    false
                );
                continue;
            }
            if (players[i].Id == playerId)
            {
                players[i] = new PlayerId(
                    players[i].Id,
                    players[i].IsDungeonMaster,
                    true
                );
            }
        }
    }
}
