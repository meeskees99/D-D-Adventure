using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class CharacterSpawner : NetworkBehaviour
{
    [SerializeField] Characters characterDataBase;
    [SerializeField] Transform spawnTransform;
    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        foreach (var client in HostManager.Instance.ClientData)
        {
            var character = characterDataBase.GetCharacterById(client.Value.characterId);
            if (character != null)
            {
                var spawnPos = new Vector3(spawnTransform.position.x + Random.Range(-3f, 3f), spawnTransform.position.y, spawnTransform.position.z + Random.Range(-3f, 3f));
                var characterInstance = Instantiate(character.PlayerPrefab, spawnPos, Quaternion.identity);
                characterInstance.SpawnAsPlayerObject(client.Value.clientId);
                characterInstance.ChangeOwnership(client.Value.clientId);
                characterInstance.GetComponent<EntityClass>().isPlayer = true;
                SetPlayer(characterInstance);
            }
        }
    }


    void SetPlayer(NetworkObject charInstance)
    {
        charInstance.GetComponent<EntityClass>().isPlayer = true;
    }
}
