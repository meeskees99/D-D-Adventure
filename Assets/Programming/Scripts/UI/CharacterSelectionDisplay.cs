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

    [Header("DM Shizzle")]
    [SerializeField] Button startButton;
    [SerializeField] TMP_Text dmJoinedText;

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
            if (!IsHost)
            {
                ClassSheet[] allCharacters = characterDataBase.GetAllCharacters();

                for (int i = 0; i < allCharacters.Length; i++)
                {
                    var selectButtonInstance = Instantiate(selectButtonPrefab, characterHolder.GetChild(i));
                    selectButtonInstance.SetCharacter(this, allCharacters[i]);
                    characterButtons.Add(selectButtonInstance);
                }
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

            startButton.gameObject.SetActive(true);
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

    private void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (i == 0)
                continue;
            if (!players[i].IsLockedIn)
            {
                break;
            }
            startButton.interactable = true;
        }
    }

    public void Select(ClassSheet character)
    {
        for (int i = 0; i < players.Count; i++)
        {
            // if (i == 0)
            //     continue;
            if (players[i].ClientId != NetworkManager.Singleton.LocalClientId) { continue; }
            if (players[i].IsLockedIn) { return; }
            if (players[i].CharacterId == character.Id) { return; }
        }

        characterNameText.text = character.CharacterName;

        characterInfoPanel.SetActive(true);

        if (introPrefabInstance != null)
        {
            Destroy(introPrefabInstance);
        }

        if (character.IntroPrefab != null)
        {
            introPrefabInstance = Instantiate(character.IntroPrefab, introSpawnpoint);
        }

        SelectServerRPC(character.Id);
    }
    [ServerRpc(RequireOwnership = false)]
    void SelectServerRPC(int characterId, ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            //     if (i == 0)
            //         continue;
            if (players[i].ClientId == serverRpcParams.Receive.SenderClientId)
            {
                players[i] = new CharacterSelection(
                    players[i].ClientId,
                    characterId,
                    players[i].IsLockedIn
                );
            }
        }
    }

    public void LockIn()
    {
        LockInServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    public void LockInServerRPC(ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId == serverRpcParams.Receive.SenderClientId)
            {
                if (!characterDataBase.IsCharacterIdValid(players[i].CharacterId)) { return; }
                players[i] = new CharacterSelection(
                    players[i].ClientId,
                    players[i].CharacterId,
                    true
                );
            }
        }
    }

    void HandlePlayersStateChanged(NetworkListEvent<CharacterSelection> changeEvent)
    {
        for (int i = 0; i < playerCards.Length + 1; i++)
        {
            if (i == 0)
            {
                dmJoinedText.text = "Joined";
                continue;
            }
            if (players.Count > i)
            {
                playerCards[i - 1].UpdateDisplay(players[i]);
            }
            else
            {
                playerCards[i - 1].DisableDisplay();
            }
        }
    }
}
