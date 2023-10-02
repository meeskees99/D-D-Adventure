using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InitiativeHandler : MonoBehaviour
{
    public int currentTurnNmbr;
    public bool DMTurn;
    public List<GameObject> characters;
    public List<Initiative> initiativeOrder = new();

    public void SetInitiativeOrder()
    {
        if (initiativeOrder != null)
        {
            if (initiativeOrder.Count != 0)
            {
                initiativeOrder.Clear();
            }
            foreach (var character in characters)
            {
                var count = 0;
                int initiative = UnityEngine.Random.Range(1, 20) + character.GetComponent<EntityClass>().initiativeBonus;
                if (initiativeOrder.Count != 0)
                {
                    foreach (var item in initiativeOrder)
                    {
                        if (initiative >= item.initiative)
                        {
                            var initiativeInstance = new Initiative { character = character, initiative = initiative };
                            initiativeOrder.Insert(count, initiativeInstance);
                            break;
                        }
                        else
                        {
                            count++;
                            if (count == initiativeOrder.Count)
                            {
                                var _initiativeInstance = new Initiative { character = character, initiative = initiative };
                                initiativeOrder.Add(_initiativeInstance);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var initiativeInstance = new Initiative { character = character, initiative = initiative };
                    initiativeOrder.Add(initiativeInstance);
                }
                Debug.Log("The amount of characters is: " + initiativeOrder.Count);
                Debug.Log("Initiative of " + character + " is: " + initiative);
            }
        }
        StartCombat();
    }
    public void AddInitiatives()
    {

    }
    private void StartCombat()
    {
        currentTurnNmbr = 0;
        Debug.Log(currentTurnNmbr);
        initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponent<ForceMovement>().isTurn = true;
    }
    public void EndTurn()
    {
        ForceMovement playerMovement;
        initiativeOrder.ElementAt(currentTurnNmbr).character.TryGetComponent(out playerMovement);
        if (playerMovement != null)
        {
            playerMovement.isTurn = false;
        }
        if (initiativeOrder[currentTurnNmbr].character.GetComponent<EntityClass>().isPlayer == true)
        {
            initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponentInChildren<MouseLook>().PositionCameraForObjects(characters);
            initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponentInChildren<MouseLook>().enabled = false;
        }
        
        currentTurnNmbr++;
        if (currentTurnNmbr > initiativeOrder.Count - 1)
        {
            currentTurnNmbr = 0;
        }
        initiativeOrder.ElementAt(currentTurnNmbr).character.TryGetComponent(out playerMovement);
        if (playerMovement != null)
        {
            playerMovement.isTurn = true;
            if(initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponent<EntityClass>().isPlayer == false)
            {
                DMTurn = true;
            }
            else
            {
                DMTurn = false;
                initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponentInChildren<MouseLook>().PosistionCameraForCombat();
                initiativeOrder.ElementAt(currentTurnNmbr).character.GetComponentInChildren<MouseLook>().enabled = true;
            }
        }
    }

}
[Serializable]
public class Initiative
{
    public GameObject character;
    [SerializeField]
    public int initiative;
}