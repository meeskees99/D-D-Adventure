using UnityEngine;
using Unity.Netcode;
using System;

public class Identifier : NetworkBehaviour
{
    // public PlayerId playerId;
    public NetworkVariable<ulong> playerId = new NetworkVariable<ulong>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isTurn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isDungeonMaster = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        if (IsOwner)
            SetId(NetworkManager.Singleton.LocalClientId);
    }

    void SetId(ulong id)
    {
        playerId.Value = id;
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
