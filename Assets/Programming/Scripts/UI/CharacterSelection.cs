using Unity.Netcode;
using System;

public struct CharacterSelection : INetworkSerializable, IEquatable<CharacterSelection>
{
    public ulong ClientId;
    public int CharacterId;
    public bool IsLockedIn;
    public bool IsDungeonMaster;

    public CharacterSelection(ulong clientId, int characterId = -1, bool isLockedIn = false, bool isDungeonMaster = false)
    {
        ClientId = clientId;
        CharacterId = characterId;
        IsLockedIn = isLockedIn;
        IsDungeonMaster = isDungeonMaster;
    }
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref CharacterId);
        serializer.SerializeValue(ref IsLockedIn);
        serializer.SerializeValue(ref IsDungeonMaster);
    }

    public bool Equals(CharacterSelection other)
    {
        return ClientId == other.ClientId &&
            CharacterId == other.CharacterId &&
            IsLockedIn == other.IsLockedIn &&
            IsDungeonMaster == other.IsDungeonMaster;
    }
}
