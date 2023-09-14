using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class InitiativeHandler : MonoBehaviour
{
    public List<CharacterSheet> characters;
    public List<(GameObject, int)> initiativeOrder = new List<(GameObject, int)>();

    public void Start()
    {
        SetInitiativeOrder();
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
                Debug.Log(initiative);
                if (initiativeOrder.Count != 0)
                {
                    foreach (var item in initiativeOrder)
                    {
                        if(initiative >= item.Item2)
                        {
                            initiativeOrder.Insert(count, (character.characterModel, initiative));
                            break;
                        }
                        else
                        {
                            count++;
                            continue;
                        }
                    }
                }
                else
                {
                    initiativeOrder.Add((character.characterModel, initiative));
                }
                Debug.Log(initiativeOrder);
            }
        }
        StartCombat();
    }
    public void AddInitiatives()
    {

    }
    private void StartCombat()
    {
        int currentTurnNmbr = 0;

    }
}