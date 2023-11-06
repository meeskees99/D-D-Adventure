using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class EventTriggers : MonoBehaviour
{
    [SerializeField] InitiativeHandler initiativeHandler;
    [SerializeField] bool combatTrigger;
    [SerializeField] GameObject[] enemies;

    GameObject playerToAdd;

    MouseLook MouseLook;
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
        MouseLook = FindObjectOfType<MouseLook>();
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
            MouseLook.inBattle = true;


            playerToAdd = other.gameObject;
            StartCombatServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartCombatServerRpc(ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            initiativeHandler.characters.Add(enemies[i].gameObject);
        }

        initiativeHandler.characters.Add(playerToAdd);
        initiativeHandler.SetInitiativeOrder();
    }

}
