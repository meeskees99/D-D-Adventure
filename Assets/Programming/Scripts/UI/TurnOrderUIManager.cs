using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnOrderUIManager : MonoBehaviour
{
    InitiativeHandler initiativeHandler;

    [SerializeField] TMP_Text turnNumberTxt;
    [SerializeField] int currentTurnNmbr;

    [SerializeField] List<TurnOrderUI> players = new();
    // Start is called before the first frame update
    void Start()
    {
        if (initiativeHandler == null)
            initiativeHandler = FindObjectOfType<InitiativeHandler>();

        for (int i = 0; i < transform.childCount; i++)
        {
            players.Add(transform.GetChild(i).GetComponent<TurnOrderUI>());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateTurnOrder()
    {
        currentTurnNmbr++;
        turnNumberTxt.text = currentTurnNmbr.ToString();

        for (int i = 0; i < players.Count; i++)
        {

        }
        for (int i = 0; i < players.Count; i++)
        {
            if (i > initiativeHandler.initiativeOrder.Count)
            {
                players[i].gameObject.SetActive(false);
                continue;
            }
            else
            {
                players[i].gameObject.SetActive(true);
            }
            players[i].playerIcon.sprite = initiativeHandler.initiativeOrder[initiativeHandler.currentTurnNmbr].character.GetComponent<EntityClass>().stats.Icon;
            players[i].initiativeText.text = initiativeHandler.initiativeOrder[initiativeHandler.currentTurnNmbr].initiative.ToString();
        }
    }
}
