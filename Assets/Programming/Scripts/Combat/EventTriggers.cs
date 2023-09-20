using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggers : MonoBehaviour
{
    [SerializeField] InitiativeHandler initiativeHandler;
    [SerializeField] bool combatTrigger;
    [SerializeField] GameObject[] enemies;
    public GameObject[] Enemies
    {
        get
        {
            return enemies;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        initiativeHandler = FindObjectOfType<InitiativeHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (combatTrigger)
        {
            if (other.transform.GetComponent<ForceMovement>().IsFighting == true)
            {
                return;
            }
            other.transform.GetComponent<ForceMovement>().IsFighting = true;
            other.transform.GetComponent<MouseLook>().inBattle = true;
            for (int i = 0; i < enemies.Length; i++)
            {
                initiativeHandler.characters.Add(enemies[i].GetComponent<CharacterSheet>());
            }
            initiativeHandler.characters.Add(other.GetComponent<CharacterSheet>());
            initiativeHandler.SetInitiativeOrder();
        }
    }
}
