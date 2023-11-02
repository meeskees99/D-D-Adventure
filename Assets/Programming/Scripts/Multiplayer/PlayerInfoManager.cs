using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInfoManager : NetworkBehaviour
{
    InitiativeHandler initiativeHandler;
    public List<Identifier> players = new();
    public Identifier[] allEntities;

    // public NetworkVariable<int>

    public List<Identifier> entitiesList = new();


    private void Start()
    {
        initiativeHandler = FindObjectOfType<InitiativeHandler>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            // HandleClientConnected(NetworkManager.Singleton.LocalClientId);
            Identifier[] identifiers = FindObjectsOfType<Identifier>();
            foreach (Identifier entity in identifiers)
            {
                if (!players.Contains(entity) && entity.isEnemy.Value == false)
                    players.Add(entity);
            }

            // foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            // {
            //     HandleClientConnected(client.ClientId);
            // }
        }

    }

    public override void OnNetworkDespawn()
    {
        // HandleClientDisconnected(NetworkManager.Singleton.LocalClientId);
    }

    void HandleClientConnected(ulong clientId)
    {
        // if (!IsServer) return;
        // for (int i = 0; i < players.Count; i++)
        // {
        //     if (players[i].playerId.Value == 0)
        //     {
        //         // players[i].playerId.Value = clientId;
        //         players[i].isDungeonMaster.Value = true;
        //     }
        //     else
        //     {
        //         players[i].isDungeonMaster.Value = false;
        //     }
        // }
    }


    private void Update()
    {
        allEntities = FindObjectsOfType<Identifier>();
        for (int i = 0; i < allEntities.Length; i++)
        {
            if (!entitiesList.Contains(allEntities[i]))
            {
                entitiesList.Add(allEntities[i]);
            }
        }
    }
    void HandleClientDisconnected(ulong clientId)
    {
        if (!IsServer) return;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerId.Value == clientId)
            {
                players.RemoveAt(i);
                break;
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
        print($"Next turn for player {playerId}. Ended turn for player {serverRpcParams.Receive.SenderClientId}.");
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerId.Value == serverRpcParams.Receive.SenderClientId)
            {
                // players[i] = new PlayerId(
                //     players[i].Id,
                //     players[i].IsDungeonMaster,
                //     false
                // );
                // continue;
                players[i].isTurn.Value = false;
            }
            if (players[i].playerId.Value == playerId)
            {
                // players[i] = new PlayerId(
                //     players[i].Id,
                //     players[i].IsDungeonMaster,
                //     true
                // );
                players[i].isTurn.Value = true;
            }
        }


    }
}
