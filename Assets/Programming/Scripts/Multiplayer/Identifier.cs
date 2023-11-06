using UnityEngine;
using Unity.Netcode;
using System;
using System.Linq;

public class Identifier : NetworkBehaviour
{
    // public PlayerId playerId;
    public NetworkVariable<ulong> playerId = new NetworkVariable<ulong>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> isTurn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> isEnemy = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<int> initiative = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> turnNumber = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    PlayerInfoManager playerInfoManager;

    bool initialized;

    [ServerRpc(RequireOwnership = false)]
    void SetIdServerRpc(ulong id, ServerRpcParams serverRpcParams = default)
    {
        print($"Player Id Set to {id}");
        playerId.Value = id;
    }

    void Update()
    {
        playerInfoManager = FindObjectOfType<PlayerInfoManager>();
        if (playerInfoManager != null && !initialized && playerInfoManager.allEntities.Length >= 2)
        {
            initialized = true;
            if (IsOwner && !IsHost)
            {
                print($"Setting Client Id as {NetworkManager.Singleton.LocalClient.ClientId}");
                SetIdServerRpc(NetworkManager.Singleton.LocalClient.ClientId);
            }
            else if (IsOwnedByServer && IsHost)
            {
                isEnemy.Value = true;
                int enemyCount = 0;
                ulong numberToGive;
                if (playerInfoManager == null) return;
                foreach (Identifier entity in playerInfoManager.allEntities)
                {
                    if (entity.isEnemy.Value && entity.playerId.Value == 0)
                    {
                        enemyCount++;
                        numberToGive = (ulong)NetworkManager.Singleton.ConnectedClientsList.Count - 1 + (ulong)enemyCount;
                        foreach (Identifier enemy in playerInfoManager.allEntities)
                        {
                            if (enemy.playerId.Value == numberToGive)
                            {
                                numberToGive++;
                            }
                        }
                        entity.playerId.Value = numberToGive;
                        print($"Setting Client Id for {entity.gameObject.name} as {numberToGive}");
                    }
                }
            }
        }
    }
}


// [System.Serializable]
// public struct PlayerId : INetworkSerializable, IEquatable<PlayerId>
// {
//     public Identifier identifier;
//     public ulong Id;
//     public bool IsDungeonMaster;
//     public bool IsTurn;

//     public PlayerId(ulong id, bool isDungeonMaster = false, bool isTurn = false)
//     {
//         Id = id;
//         IsDungeonMaster = isDungeonMaster;
//         IsTurn = isTurn;
//     }

//     public bool Equals(PlayerId other)
//     {
//         return Id == other.Id &&
//         IsDungeonMaster == other.IsDungeonMaster &&
//         IsTurn == other.IsTurn;

//     }

//     public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
//     {
//         serializer.SerializeValue(ref Id);
//         serializer.SerializeValue(ref IsDungeonMaster);
//         serializer.SerializeValue(ref IsTurn);
//     }
// }
