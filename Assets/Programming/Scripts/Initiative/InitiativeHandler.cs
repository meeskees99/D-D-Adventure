using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class InitiativeHandler : NetworkBehaviour
{
    public static InitiativeHandler instance;
    public CombatHandler combatHandler = CombatHandler.instance;
    public int currentTurnNmbr;
    public int currentRound;
    public bool DMTurn;
    public GameObject DMObject;
    public List<GameObject> characters;
    public List<Initiative> initiativeOrder;
    public GameManager gameManager;
    public PlayerInfoManager playerInfoManager;
    public TurnOrderUIManager turnOrderUIManager;

    MouseLook mouseLook;


    [Header("Settings")]
    [SerializeField] GameObject combatPanel;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        gameManager = FindObjectOfType<GameManager>();
        playerInfoManager = FindObjectOfType<PlayerInfoManager>();
        turnOrderUIManager = FindObjectOfType<TurnOrderUIManager>();
        mouseLook = FindObjectOfType<MouseLook>();

        if (IsHost)
        {
            DMObject = Camera.main.transform.gameObject;
        }
    }

    public void SetInitiativeOrder()
    {
        if (IsServer)
        {
            currentRound = 0;
            currentTurnNmbr = 0;

            EntityClass[] combatEntities = FindObjectsOfType<EntityClass>();
            foreach (var entity in combatEntities)
            {
                if (!characters.Contains(entity.gameObject))
                {
                    characters.Add(entity.gameObject);
                }
            }
            if (initiativeOrder != null)
            {
                if (initiativeOrder.Count != 0)
                {
                    initiativeOrder.Clear();
                }
                foreach (var character in characters)
                {
                    var count = 0;
                    int _initiative = character.GetComponent<EntityClass>().initiative.Value + character.GetComponent<EntityClass>().initiativeBonus.Value;
                    if (initiativeOrder.Count != 0)
                    {
                        foreach (var item in initiativeOrder)
                        {
                            if (_initiative >= item.initiative)
                            {
                                var initiativeInstance = new Initiative { character = character };
                                initiativeInstance.initiative = _initiative;
                                initiativeOrder.Insert(count, initiativeInstance);
                                initiativeOrder[count].character.GetComponent<Identifier>().initiative.Value = initiativeInstance.initiative;
                                break;
                            }
                            else
                            {
                                count++;
                                if (count == initiativeOrder.Count)
                                {
                                    var initiativeInstance = new Initiative { character = character };
                                    initiativeInstance.initiative = _initiative;
                                    initiativeOrder.Add(initiativeInstance);
                                    initiativeOrder[count].character.GetComponent<Identifier>().initiative.Value = _initiative;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        var initiativeInstance = new Initiative { character = character };
                        initiativeInstance.initiative = _initiative;
                        initiativeOrder.Add(initiativeInstance);
                    }
                    Debug.Log("The amount of characters is: " + initiativeOrder.Count);
                    Debug.Log("Initiative of " + character + " is: " + _initiative);
                }
            }

        }

        StartCombat();
        foreach (var instance in initiativeOrder)
        {
            if (instance.character.TryGetComponent(out ForceMovement forceMovement))
            {
                if (forceMovement.isTurn)
                    continue;
            }
            else
            {
                if (instance.character.GetComponent<EntityClass>().isPlayer)
                {
                    if (mouseLook != null)
                        mouseLook.PositionCameraForObjects(characters);
                }
            }
        }
        turnOrderUIManager.UpdateTurnOrder();
    }
    public void AddInitiatives()
    {

    }
    private void StartCombat()
    {
        currentTurnNmbr = 0;
        Debug.Log($"Current turn number: {currentTurnNmbr}");
        initiativeOrder.ElementAt(currentTurnNmbr).character.TryGetComponent(out ForceMovement forceMovement);
        if (forceMovement != null)
        {
            forceMovement.isTurn = true;
        }
        if (combatPanel != null)
        {
            combatPanel.SetActive(true);
        }
        if (IsHost)
        {
            DMObject.GetComponent<DmCameraController>().PosistionCameraForCombat();
            if (instance.DMTurn)
            {
                DMObject.GetComponent<DmCameraController>().enabled = false;
                DMObject.GetComponent<MouseLook>().enabled = true;
            }
        }

    }
    public void EndTurn(ulong clientId)
    {
        if (initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponent<Identifier>().playerId.Value != clientId) { return; }

        initiativeOrder.ElementAt(currentTurnNmbr).character.TryGetComponent(out ForceMovement playerMovement);
        if (playerMovement != null)
        {
            playerMovement.isTurn = false;
        }
        if (initiativeOrder[currentTurnNmbr].character.GetComponent<EntityClass>().isPlayer == true)
        {
            if (mouseLook != null)
            {
                mouseLook.PositionCameraForObjects(characters);
                mouseLook.enabled = false;
            }
        }
        else
        {
            initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponent<ForceMovement>().enabled = false;
        }

        currentTurnNmbr++;
        if (currentTurnNmbr > initiativeOrder.Count - 1)
        {
            currentTurnNmbr = 0;
            currentRound++;
        }
        Debug.Log($"Current turn number: {currentTurnNmbr}");
        initiativeOrder.ElementAt(currentTurnNmbr).character.TryGetComponent(out playerMovement);
        if (playerMovement != null)
        {
            playerMovement.isTurn = true;
            if (initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponent<EntityClass>().isPlayer == false || initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponent<Identifier>().isEnemy.Value == true)
            {
                DMTurn = true;
                DMObject.GetComponent<DmCameraController>().enabled = false;
                DMObject.GetComponent<MouseLook>().enabled = true;
                initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponent<ForceMovement>().enabled = true;
            }
            else
            {
                DMTurn = false;
                DMObject.GetComponent<DmCameraController>().enabled = true;
                DMObject.GetComponent<MouseLook>().enabled = false;
                if (mouseLook != null)
                {
                    mouseLook.PosistionCameraForCombat();
                    mouseLook.enabled = true;
                }
            }
        }
        initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponent<Identifier>().turnNumber.Value = currentTurnNmbr;
        playerInfoManager.NextTurn(initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponent<Identifier>().playerId.Value);
        turnOrderUIManager.UpdateTurnOrder();
        if (combatHandler.curTarget != null)
        {
            combatHandler.curTarget.TryGetComponent(out Outline outline);
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }
}

[Serializable]
public class Initiative
{
    public GameObject character;
    public int initiative;
}