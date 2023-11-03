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
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Slider heatlhSlider;

    [Header("Panels")]
    [SerializeField] GameObject activePlayerPanel;
    [SerializeField] GameObject combatPanel;
    [SerializeField] GameObject turnOrderPanel;

    [Header("Info")]
    [SerializeField] List<TurnOrderUI> players = new();
    [SerializeField] TMP_Text turnNumberTxt;

    bool startedCombat;
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

        if (combatPanel != null)
            combatPanel.SetActive(false);
    }

    float turnOrderRefreshTime = 3f;
    float elapsedTime;
    // Update is called once per frame
    void Update()
    {
        if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
        }
        else if (startedCombat)
        {
            elapsedTime = turnOrderRefreshTime;
            UpdateTurnOrder();
            print("Updated TurnUI");
        }
    }

    public void UpdateTurnOrder()
    {
        if (!startedCombat)
        {
            startedCombat = true;
            InitializeCombat();
        }
        //turnNumberTxt.text = initiativeHandler.currentTurnNmbr.ToString();

        for (int i = 0; i < players.Count; i++)
        {
            if (i >= initiativeHandler.initiativeOrder.Count)
            {
                players[i].gameObject.SetActive(false);
                players[i].playerIcon.sprite = null;
                // players[i].initiativeText.text = "";
            }
            else
            {
                if (initiativeHandler.initiativeOrder[i].character.GetComponent<EntityClass>().hitPoints.Value <= 0)
                {
                    players[i].gameObject.SetActive(false);
                    players[i].playerIcon.sprite = null;
                    // players[i].initiativeText.text = "";
                    players.RemoveAt(i);
                }
                else
                {
                    players[i].playerIcon.sprite = initiativeHandler.initiativeOrder[i].character.GetComponent<EntityClass>().stats.Icon;
                    // players[i].initiativeText.text = initiativeHandler.initiativeOrder[i].initiative.ToString();
                    players[i].gameObject.SetActive(true);
                }

            }

        }
        if (players.Count > 0)
        {
            activePlayerIcon.sprite = players[0].playerIcon.sprite;
            playerNameText.text = $"{initiativeHandler.initiativeOrder[0].character.GetComponent<EntityClass>().stats.CharacterName}";
            healthText.text = initiativeHandler.initiativeOrder[0].character.GetComponent<EntityClass>().hitPoints.Value + "/" + initiativeHandler.initiativeOrder[0].character.GetComponent<EntityClass>().maxHitPoints.Value;
            heatlhSlider.maxValue = initiativeHandler.initiativeOrder[0].character.GetComponent<EntityClass>().maxHitPoints.Value;
            heatlhSlider.value = initiativeHandler.initiativeOrder[0].character.GetComponent<EntityClass>().hitPoints.Value;
        }
        else
        {
            turnOrderPanel.GetComponent<Animator>().SetTrigger("Combat");
            startedCombat = false;
        }
    }

    public void ToggleCurrentPlayerPanel()
    {
        activePlayerPanel.SetActive(!activePlayerPanel.activeSelf);
    }

    public void InitializeCombat()
    {
        turnOrderPanel.GetComponent<Animator>().SetTrigger("Combat");
    }
}
