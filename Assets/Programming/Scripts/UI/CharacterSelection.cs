using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public struct CharacterSelection : INetworkSerializable, IEquatable<CharacterSelection>
{
    public ulong ClientId;
    public int CharacterId;

    public CharacterSelection(ulong clientId, int characterId = -1)
    {
        ClientId = clientId;
        CharacterId = characterId;
    }
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref CharacterId);
    }

    public bool Equals(CharacterSelection other)
    {
        return ClientId == other.ClientId &&
            CharacterId == other.CharacterId;
    }
}
