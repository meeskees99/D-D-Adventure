using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            if (other.transform.GetComponent<ForceMovement>().isFighting == true)
            {
                return;
            }
            other.transform.GetComponent<ForceMovement>().isFighting = true;
            other.transform.GetChild(0).GetComponent<MouseLook>().inBattle = true;
            for (int i = 0; i < enemies.Length; i++)
            {
                initiativeHandler.characters.Add(enemies[i].gameObject);
            }
            initiativeHandler.characters.Add(other.gameObject);
            initiativeHandler.SetInitiativeOrder();
        }
    }
}
