using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class TurnOrderUIManager : MonoBehaviour
{
    InitiativeHandler initiativeHandler;

    [Header("Active player")]
    [SerializeField] Image activePlayerIcon;
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Slider heatlhSlider;

    [Header("Panels")]
    [SerializeField] GameObject activePlayerPanel;
    [SerializeField] GameObject combatPanel;
    [SerializeField] GameObject turnOrderPanel;

    [Header("Info")]
    [SerializeField] List<TurnOrderUI> playersUI = new();
    [SerializeField] List<Identifier> entities = new();
    [SerializeField] List<Identifier> initOrder = new();
    [SerializeField] TMP_Text turnNumberTxt;

    bool startedCombat;
    // Start is called before the first frame update
    void Start()
    {
        if (initiativeHandler == null)
            initiativeHandler = FindObjectOfType<InitiativeHandler>();

        var _players = FindObjectsOfType<TurnOrderUI>();
        for (int i = 0; i < _players.Length; i++)
        {
            playersUI.Add(_players[i]);
        }
        playersUI.Reverse();

        if (combatPanel != null)
            combatPanel.SetActive(false);
    }

    float turnOrderRefreshTime = 3f;
    float elapsedTime;
    // Update is called once per frame
    void Update()
    {
        if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
        }
        else if (startedCombat)
        {
            elapsedTime = turnOrderRefreshTime;
            UpdateTurnOrder();
            print("Updated TurnUI");
        }
    }

    public void UpdateTurnOrder()
    {
        if (!startedCombat)
        {
            startedCombat = true;
            InitializeCombat();
        }
        //turnNumberTxt.text = initiativeHandler.currentTurnNmbr.ToString();

        for (int i = 0; i < playersUI.Count; i++)
        {
            if (i >= initOrder.Count)
            {
                playersUI[i].gameObject.SetActive(false);
                playersUI[i].playerIcon.sprite = null;
                // players[i].initiativeText.text = "";
            }
            else
            {
                if (initOrder[i].gameObject.GetComponent<EntityClass>().hitPoints.Value <= 0)
                {
                    playersUI[i].gameObject.SetActive(false);
                    playersUI[i].playerIcon.sprite = null;
                    // players[i].initiativeText.text = "";
                    playersUI.RemoveAt(i);
                }
                else
                {
                    playersUI[i].playerIcon.sprite = initOrder[i].gameObject.GetComponent<EntityClass>().stats.Icon;
                    // players[i].initiativeText.text = initiativeHandler.initiativeOrder[i].initiative.ToString();
                    playersUI[i].gameObject.SetActive(true);
                }

            }

        }
        if (playersUI.Count > 0)
        {
            activePlayerIcon.sprite = playersUI[0].playerIcon.sprite;
            playerNameText.text = $"{initOrder[0].gameObject.GetComponent<EntityClass>().stats.CharacterName}";
            healthText.text = initOrder[0].gameObject.GetComponent<EntityClass>().hitPoints.Value + "/" + initOrder[0].gameObject.GetComponent<EntityClass>().maxHitPoints.Value;
            heatlhSlider.maxValue = initOrder[0].gameObject.GetComponent<EntityClass>().maxHitPoints.Value;
            heatlhSlider.value = initOrder[0].gameObject.GetComponent<EntityClass>().hitPoints.Value;
        }
        else
        {
            turnOrderPanel.GetComponent<Animator>().SetTrigger("Combat");
            startedCombat = false;
        }
    }

    public void ToggleCurrentPlayerPanel()
    {
        activePlayerPanel.SetActive(!activePlayerPanel.activeSelf);
    }

    public void InitializeCombat()
    {
        turnOrderPanel.GetComponent<Animator>().SetTrigger("Combat");

        Identifier[] identifiers = FindObjectsOfType<Identifier>();

        foreach (var item in identifiers)
        {
            if (!entities.Contains(item))
            {
                entities.Add(item);
            }
        }

        foreach (var item in entities)
        {
            for (int i = 0; i < initOrder.Count; i++)
            {
                if (initOrder[i].initiative.Value >= item.initiative.Value)
                {
                    initOrder.Insert(i, item);
                }
                else
                {
                    if (i == entities.Count)
                        initOrder.Add(item);
                    else
                    {
                        continue;
                    }
                }
                continue;
            }
            if (initOrder.Count == 0)
            {
                initOrder.Add(item);
            }
        }
    }

    public void EndTurnButton()
    {
        EndTurn(NetworkManager.Singleton.LocalClient.ClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndTurn(ulong clientID, ServerRpcParams serverRpcParams = default)
    {
        initiativeHandler.EndTurn(clientID);
    }
}
