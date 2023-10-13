using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSheetUIManager : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] GameObject backgroundPanel;
    [SerializeField] GameObject playerOneSheet;
    [SerializeField] GameObject playerTwoSheet;
    [SerializeField] Animator sheetAnimator;
    [Header("Player")]
    public List<EntityClass> players = new();
    [SerializeField] bool sheet1Active;
    [SerializeField] bool sheet2Active;
    [SerializeField] List<EntityClass> allEntities;
    [SerializeField] bool allPlayersChecked;
    bool playersSet;

    [Header("Player One Subsections")]
    [SerializeField] GameObject p1MainPanel;
    [SerializeField] GameObject p1CombatPanel;
    [SerializeField] GameObject p1DeathPanel;
    [SerializeField] GameObject p1InventoryPanel;
    [SerializeField] GameObject p1NotesPanel;


    [Header("Player Two Subsections")]
    [SerializeField] GameObject p2MainPanel;
    [SerializeField] GameObject p2CombatPanel;
    [SerializeField] GameObject p2DeathPanel;
    [SerializeField] GameObject p2InventoryPanel;
    [SerializeField] GameObject p2NotesPanel;

    void Update()
    {
        allPlayersChecked = allEntities.Count == FindObjectsOfType<EntityClass>().Length;
        EntityClass[] Sheet = FindObjectsOfType<EntityClass>();
        if (!allPlayersChecked)
        {
            for (int i = 0; i < Sheet.Length; i++)
            {
                if (!allEntities.Contains(Sheet[i]))
                {
                    allEntities.Add(Sheet[i]);
                }
            }
        }
        else if (!playersSet)
        {
            SetPlayers();
        }
    }

    void SetPlayers()
    {
        playersSet = true;
        for (int i = 0; i < allEntities.Count; i++)
        {
            if (allEntities[i].isPlayer)
            {
                players.Add(allEntities[i]);
            }
        }

    }

    public void TogglePlayerOneSheet()
    {
        if (sheet2Active)
        {
            playerTwoSheet.SetActive(false);
        }
        else
        {
            SelectMainPanel(true);
            sheetAnimator.SetTrigger("Toggle");
        }

        sheet1Active = !sheet1Active;
        sheet2Active = false;
        playerOneSheet.SetActive(sheet1Active);

    }
    public void TogglePlayerTwoSheet()
    {
        if (sheet1Active)
        {
            playerOneSheet.SetActive(false);
        }
        else
        {
            SelectMainPanel(false);
            sheetAnimator.SetTrigger("Toggle");
        }

        sheet2Active = !sheet2Active;
        sheet1Active = false;
        playerTwoSheet.SetActive(sheet2Active);

    }



    #region Character Sheet Subsections
    public void SelectMainPanelButton()
    {
        if (sheet1Active)
        {
            p1MainPanel.SetActive(true);
            p1CombatPanel.SetActive(false);
            p1DeathPanel.SetActive(false);
            p1InventoryPanel.SetActive(false);
            p1NotesPanel.SetActive(false);
        }
        else
        {
            p2MainPanel.SetActive(true);
            p2CombatPanel.SetActive(false);
            p2DeathPanel.SetActive(false);
            p2InventoryPanel.SetActive(false);
            p2NotesPanel.SetActive(false);
        }
    }
    void SelectMainPanel(bool isP1)
    {
        if (sheet1Active || isP1)
        {
            p1MainPanel.SetActive(true);
            p1CombatPanel.SetActive(false);
            p1DeathPanel.SetActive(false);
            p1InventoryPanel.SetActive(false);
            p1NotesPanel.SetActive(false);
        }
        else
        {
            p2MainPanel.SetActive(true);
            p2CombatPanel.SetActive(false);
            p2DeathPanel.SetActive(false);
            p2InventoryPanel.SetActive(false);
            p2NotesPanel.SetActive(false);
        }
    }

    public void SelectCombatPanel()
    {
        if (sheet1Active)
        {
            p1MainPanel.SetActive(false);
            p1CombatPanel.SetActive(true);
            p1DeathPanel.SetActive(false);
            p1InventoryPanel.SetActive(false);
            p1NotesPanel.SetActive(false);
        }
        else
        {
            p2MainPanel.SetActive(false);
            p2CombatPanel.SetActive(true);
            p2DeathPanel.SetActive(false);
            p2InventoryPanel.SetActive(false);
            p2NotesPanel.SetActive(false);
        }
    }

    public void SelectDeathPanel()
    {
        if (sheet1Active)
        {
            p1MainPanel.SetActive(false);
            p1CombatPanel.SetActive(false);
            p1DeathPanel.SetActive(true);
            p1InventoryPanel.SetActive(false);
            p1NotesPanel.SetActive(false);
        }
        else
        {
            p2MainPanel.SetActive(false);
            p2CombatPanel.SetActive(false);
            p2DeathPanel.SetActive(true);
            p2InventoryPanel.SetActive(false);
            p2NotesPanel.SetActive(false);
        }
    }

    public void SelectInventoryPanel()
    {
        if (sheet1Active)
        {
            p1MainPanel.SetActive(false);
            p1CombatPanel.SetActive(false);
            p1DeathPanel.SetActive(false);
            p1InventoryPanel.SetActive(true);
            p1NotesPanel.SetActive(false);
        }
        else
        {
            p2MainPanel.SetActive(false);
            p2CombatPanel.SetActive(false);
            p2DeathPanel.SetActive(false);
            p2InventoryPanel.SetActive(true);
            p2NotesPanel.SetActive(false);
        }
    }

    public void SelectNotesPanel()
    {
        if (sheet1Active)
        {
            p1MainPanel.SetActive(false);
            p1CombatPanel.SetActive(false);
            p1DeathPanel.SetActive(false);
            p1InventoryPanel.SetActive(false);
            p1NotesPanel.SetActive(true);
        }
        else
        {
            p2MainPanel.SetActive(false);
            p2CombatPanel.SetActive(false);
            p2DeathPanel.SetActive(false);
            p2InventoryPanel.SetActive(false);
            p2NotesPanel.SetActive(true);
        }
    }

    #endregion
}
