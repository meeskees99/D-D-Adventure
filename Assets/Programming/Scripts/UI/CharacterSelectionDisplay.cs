using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;


public class CharacterSelectionDisplay : NetworkBehaviour
{
    [SerializeField] Characters characterDataBase;
    // [SerializeField] Transform characterHolder;
    // [SerializeField] CharacterSelectButton selectButtonPrefab;
    [SerializeField] PlayerCards[] playerCards;

    [SerializeField] ClassSheet[] characters;
    [SerializeField] Button previousCharacterButton;
    [SerializeField] Button nextCharacterButton;
    int characterIndex = -1;

    // [SerializeField] GameObject characterInfoPanel;
    [SerializeField] TMP_Text characterNameText;
    // [SerializeField] Transform introSpawnpoint;
    [SerializeField] TMP_InputField joinCodeInputField;
    [SerializeField] Button lockInButton;

    [Header("DM Shizzle")]
    [SerializeField] Button startButton;
    // [SerializeField] TMP_Text dmJoinedText;

    GameObject introPrefabInstance;
    // List<CharacterSelectButton> characterButtons = new();

    NetworkList<CharacterSelection> players;

    HostManager hostManager = HostManager.Instance;

    private void Awake()
    {
        players = new NetworkList<CharacterSelection>();
        startButton.onClick.AddListener(hostManager.StartGame);
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            if (!IsHost)
            {
                characters = characterDataBase.GetAllCharacters();
                // lockInButton.gameObject.SetActive(true);

                // for (int i = 0; i < characters.Length; i++)
                // {
                //     var selectButtonInstance = Instantiate(selectButtonPrefab, characterHolder.GetChild(i));
                //     selectButtonInstance.SetCharacter(this, characters[i]);
                //     characterButtons.Add(selectButtonInstance);
                // }

                lockInButton = playerCards[NetworkManager.Singleton.LocalClient.ClientId - 1].lockInButton;
                nextCharacterButton = playerCards[NetworkManager.Singleton.LocalClient.ClientId - 1].nextCharacterButton;
                previousCharacterButton = playerCards[NetworkManager.Singleton.LocalClient.ClientId - 1].previousCharacterButton;
                characterNameText = playerCards[NetworkManager.Singleton.LocalClient.ClientId - 1].characternameText;
                nextCharacterButton.onClick.AddListener(NextCharacter);
                previousCharacterButton.onClick.AddListener(PreviousCharacter);
                lockInButton.onClick.AddListener(LockIn);
                NextCharacter();
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

        if (IsHost)
        {
            joinCodeInputField.text = HostManager.Instance.JoinCode;
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
    bool updateButtons = true;
    private void Update()
    {
        if (!IsHost)
        {
            var mapDropdown = FindObjectOfType<TMP_Dropdown>();
            if (mapDropdown != null)
                mapDropdown.gameObject.SetActive(false);
        }
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
        #region Disable Buttons
        if (updateButtons)
        {
            updateButtons = false;
            if (NetworkManager.Singleton.LocalClientId == 1)
            {  //Disable Buttons from opisite player
                // Debug.LogError($"This is player {NetworkManager.Singleton.LocalClientId}, disabling buttons for playercard 2");
                playerCards[1].nextCharacterButton.gameObject.SetActive(false);
                playerCards[1].previousCharacterButton.gameObject.SetActive(false);
                playerCards[1].lockInButton.gameObject.SetActive(false);
            }
            else if (NetworkManager.Singleton.LocalClientId == 2)
            {   //Disable Buttons from opisite player
                // Debug.LogError($"This is player {NetworkManager.Singleton.LocalClientId}, disabling buttons for playercard 1");
                playerCards[0].nextCharacterButton.gameObject.SetActive(false);
                playerCards[0].previousCharacterButton.gameObject.SetActive(false);
                playerCards[0].lockInButton.gameObject.SetActive(false);
            }
            else
            {   //Disable buttons for DM
                // Debug.LogError($"This is player {NetworkManager.Singleton.LocalClientId}, disabling buttons for playercards 1 and 2");
                playerCards[0].nextCharacterButton.gameObject.SetActive(false);
                playerCards[0].previousCharacterButton.gameObject.SetActive(false);
                playerCards[0].lockInButton.gameObject.SetActive(false);

                playerCards[1].nextCharacterButton.gameObject.SetActive(false);
                playerCards[1].previousCharacterButton.gameObject.SetActive(false);
                playerCards[1].lockInButton.gameObject.SetActive(false);
            }
        }
        #endregion
    }

    public void Select(ClassSheet character)
    {
        for (int i = 0; i < players.Count; i++)
        {
            // if (i == 0)
            //     continue;
            if (players[i].ClientId != NetworkManager.Singleton.LocalClientId || players[i].ClientId == 0) { continue; }
            if (players[i].IsLockedIn) { return; }
            if (players[i].CharacterId == character.Id) { return; }
        }

        characterNameText.text = character.CharacterName;

        // characterInfoPanel.SetActive(true);

        if (introPrefabInstance != null)
        {
            Destroy(introPrefabInstance);
        }

        if (character.IntroPrefab != null)
        {
            // introPrefabInstance = Instantiate(character.IntroPrefab, introSpawnpoint);
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
                    players[i].IsLockedIn,
                    players[i].IsDungeonMaster
                );
            }
            if (i == 0)
            {
                players[i] = new CharacterSelection(
                    players[i].ClientId,
                    players[i].CharacterId,
                    players[i].IsLockedIn,
                    true
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

        foreach (var player in players)
        {
            if (!player.IsLockedIn && !player.IsDungeonMaster) { return; }
        }

        foreach (var player in players)
        {
            HostManager.Instance.SetCharacter(player.ClientId, player.CharacterId);
        }

    }

    void HandlePlayersStateChanged(NetworkListEvent<CharacterSelection> changeEvent)
    {
        for (int i = 0; i < playerCards.Length + 1; i++)
        {
            if (i == 0)
            {
                // dmJoinedText.text = "Joined";
                continue;
            }

            if (players.Count > i)
            {
                Debug.LogError($"Updated Display Of PlayerCard {i - 1} for player {players[i].ClientId}.");
                playerCards[i - 1].UpdateDisplay(players[i]);
            }
            else
            {
                // Debug.LogError($"Disabled Display Of PlayerCard {i - 1}.");
                playerCards[i - 1].DisableDisplay();
            }
        }
    }

    public void NextCharacter()
    {
        if (characterIndex == characterDataBase.GetAllCharacters().Length - 1)
        {
            characterIndex = 0;
        }
        else
        {
            characterIndex++;
        }
        print(characterIndex + " < character index");
        Select(characters[characterIndex]);
        if (players.Count > 1)
            playerCards[0].UpdateDisplay(players[1]);
        if (players.Count > 2)
            playerCards[1].UpdateDisplay(players[2]);
    }

    public void PreviousCharacter()
    {
        if (characterIndex == 0)
        {
            characterIndex = characterDataBase.GetAllCharacters().Length - 1;
        }
        else
        {
            characterIndex--;
        }
        Select(characters[characterIndex]);
        playerCards[0].UpdateDisplay(players[1]);
        playerCards[1].UpdateDisplay(players[2]);
    }
}
