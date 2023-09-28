using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;


public class CharacterSelectionDisplay : NetworkBehaviour
{
    [SerializeField] Characters characterDataBase;
    [SerializeField] Transform characterHolder;
    [SerializeField] CharacterSelectButton selectButtonPrefab;
    [SerializeField] PlayerCards[] playerCards;
    [SerializeField] GameObject characterInfoPanel;
    [SerializeField] TMP_Text characterNameText;
    [SerializeField] Transform introSpawnpoint;
    [SerializeField] Button lockInButton;

    GameObject introPrefabInstance;
    List<CharacterSelectButton> characterButtons = new();

    NetworkList<CharacterSelection> players;

    private void Awake()
    {
        players = new NetworkList<CharacterSelection>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            ClassSheet[] allCharacters = characterDataBase.GetAllCharacters();

            for (int i = 0; i < allCharacters.Length; i++)
            {
                var selectButtonInstance = Instantiate(selectButtonPrefab, characterHolder.GetChild(i));
                selectButtonInstance.SetCharacter(this, allCharacters[i]);
                characterButtons.Add(selectButtonInstance);
            }

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
        players.Add(new CharacterSelection(clientId));
    }
    void HandleClientDisconnected(ulong clientId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId == clientId)
            {
                players.RemoveAt(i);
                break;
            }
        }
    }

    public void Select(ClassSheet character)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != NetworkManager.Singleton.LocalClientId) { continue; }
        }

        characterNameText.text = character.CharacterName;

        characterInfoPanel.SetActive(true);

        SelectServerRPC(character.Id);
    }
    [ServerRpc(RequireOwnership = false)]
    void SelectServerRPC(int characterId, ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId == serverRpcParams.Receive.SenderClientId)
            {
                players[i] = new CharacterSelection(
                    players[i].ClientId,
                    characterId
                );
            }
        }
    }

    void HandlePlayersStateChanged(NetworkListEvent<CharacterSelection> changeEvent)
    {
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (players.Count > i)
            {
                playerCards[i].UpdateDisplay(players[i]);
            }
            else
            {
                playerCards[i].DisableDisplay();
            }
        }
    }
}
