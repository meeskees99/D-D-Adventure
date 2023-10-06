using UnityEngine;
using Unity.Netcode;
using System;

public class Identifier : MonoBehaviour
{
    public PlayerId playerId;
}


[System.Serializable]
public struct PlayerId : INetworkSerializable, IEquatable<PlayerId>
{
    public ulong Id;
    public bool IsDungeonMaster;
    public bool IsTurn;

    public PlayerId(ulong id, bool isDungeonMaster = false, bool isTurn = false)
    {
        Id = id;
        IsDungeonMaster = isDungeonMaster;
        IsTurn = isTurn;
    }

    public bool Equals(PlayerId other)
    {
        return Id == other.Id &&
        IsDungeonMaster == other.IsDungeonMaster &&
        IsTurn == other.IsTurn;

    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Id);
        serializer.SerializeValue(ref IsDungeonMaster);
        serializer.SerializeValue(ref IsTurn);
    }
}
