using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnOrderUIManager : MonoBehaviour
{
    InitiativeHandler initiativeHandler;

    [Header("Active player")]
    [SerializeField] Image activePlayerIcon;
    [SerializeField] TMP_Text activePlayerText;

    [Header("Panels")]
    [SerializeField] GameObject activePlayerPanel;
    [SerializeField] GameObject combatPanel;

    [Header("Info")]
    [SerializeField] List<TurnOrderUI> players = new();
    [SerializeField] TMP_Text turnNumberTxt;
    // Start is called before the first frame update
    void Start()
    {
        if (initiativeHandler == null)
            initiativeHandler = FindObjectOfType<InitiativeHandler>();

        var _players = FindObjectsOfType<TurnOrderUI>();
        for (int i = 0; i < _players.Length; i++)
        {
            players.Add(_players[i]);
        }
        players.Reverse();

        combatPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateTurnOrder()
    {
        turnNumberTxt.text = initiativeHandler.currentTurnNmbr.ToString();

        for (int i = 0; i < players.Count; i++)
        {
            if (i >= initiativeHandler.initiativeOrder.Count)
            {
                players[i].gameObject.SetActive(false);
                players[i].playerIcon.sprite = null;
                players[i].initiativeText.text = "";
                continue;
            }
            else
            {
                players[i].playerIcon.sprite = initiativeHandler.initiativeOrder[i].character.GetComponent<EntityClass>().stats.Icon;
                players[i].initiativeText.text = initiativeHandler.initiativeOrder[i].initiative.ToString();
                players[i].gameObject.SetActive(true);
            }

        }

        activePlayerIcon.sprite = players[0].playerIcon.sprite;
        activePlayerText.text = $"Current Player:\n{initiativeHandler.initiativeOrder[0].character.GetComponent<EntityClass>().stats.CharacterName}";
    }

    public void ToggleCurrentPlayerPanel()
    {
        activePlayerPanel.SetActive(!activePlayerPanel.activeSelf);
    }
}
