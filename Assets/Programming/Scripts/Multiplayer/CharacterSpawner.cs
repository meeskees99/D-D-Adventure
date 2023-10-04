using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterSpawner : NetworkBehaviour
{
    [SerializeField] Characters characterDataBase;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        foreach (var client in HostManager.Instance.ClientData)
        {
            var character = characterDataBase.GetCharacterById(client.Value.characterId);
            if (character != null)
            {
                var spawnPos = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
                var characterInstance = Instantiate(character.PlayerPrefab, spawnPos, Quaternion.identity);
                characterInstance.SpawnAsPlayerObject(client.Value.clientId);
                characterInstance.GetComponent<EntityClass>().isPlayer = true;
                SetPlayerObserver(characterInstance.GetComponent<EntityClass>());
            }
        }
    }

    void SetPlayerObserver(EntityClass charInstance)
    {
        charInstance.isPlayer = true;
    }
}
