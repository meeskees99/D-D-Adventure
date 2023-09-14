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
                int initiative = UnityEngine.Random.Range(1, 20) + character.initiativeBonus;
                initiativeOrder.Add((character.characterModel, initiative));
                Debug.Log(initiativeOrder);
            }
        }
    }
}