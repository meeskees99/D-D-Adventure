using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class InitiativeHandler : MonoBehaviour
{
    public CharacterSheetHandler playerTab;
    public int currentTurnNmbr;
    public List<CharacterSheet> characters;
    public List<Initiative> initiativeOrder = new();



    public void Start()
    {
        SetInitiativeOrder();
        foreach (var player in characters)
        {

        }
    }
    private void SetInitiativeOrder()
    {
        if(initiativeOrder != null)
        {
            if(initiativeOrder.Count != 0)
            {
                initiativeOrder.Clear();
            }
            foreach (var character in characters)
            {
                var count = 0;
                int initiative = UnityEngine.Random.Range(1, 20) + character.initiativeBonus;
                if (initiativeOrder.Count != 0)
                {
                    foreach (var item in initiativeOrder)
                    {
                        if(initiative >= item.initiative)
                        {
                            var initiativeInstance = new Initiative {character = character,initiative = initiative};
                            initiativeOrder.Insert(count, initiativeInstance);
                            break;
                        }
                        else
                        {
                            count++;
                            if(count == initiativeOrder.Count)
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
                Debug.Log("Initiative of "+ character + " is: " + initiative);
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
        initiativeOrder.ElementAt(currentTurnNmbr).character.characterModel.GetComponent<Renderer>().material.color = Color.green;

    }
    public void EndTurn()
    {
        initiativeOrder[currentTurnNmbr].character.characterModel.GetComponent<Renderer>().material.color = Color.red;
        currentTurnNmbr++;
        if(currentTurnNmbr > initiativeOrder.Count-1)
        {
            currentTurnNmbr = 0;
        }
        Debug.Log(currentTurnNmbr);
        initiativeOrder.ElementAt(currentTurnNmbr).character.characterModel.GetComponent<Renderer>().material.color = Color.green;
    }
    
}
[Serializable]
public class Initiative
{
    public CharacterSheet character;
    [SerializeField]
    public int initiative;
}