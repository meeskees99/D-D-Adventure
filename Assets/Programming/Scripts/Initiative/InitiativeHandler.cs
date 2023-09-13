using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class InitiativeHandler : MonoBehaviour
{
    public List<CharacterSheet> characters;
    public SortedList<GameObject, int> initiativeOrder;
    public void Start()
    {
        SetInitiativeOrder();
    }

    private void SetInitiativeOrder()
    {
        if(initiativeOrder.Count != 0)
        {
            initiativeOrder.Clear();
        }
        
        foreach(var character in characters)
        {
            int initiative = UnityEngine.Random.Range(0, 20) + character.initiativeBonus;
            initiativeOrder.Add(character.characterModel, initiative);
            Debug.Log(initiativeOrder);
        }
    }
}
