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
    [Header("Player")]
    public List<EntityClass> players = new();

    [SerializeField] List<EntityClass> allEntities;
    [SerializeField] bool allPlayersChecked;
    bool playersSet;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        print($"All entities count = {allEntities.Count}. All players Count = {players.Count}.");
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

        if (playerOneSheet.activeSelf || playerTwoSheet.activeSelf)
        {
            backgroundPanel.SetActive(true);
        }
        else
        {
            backgroundPanel.SetActive(false);
        }
    }

    public void TogglePlayerOneSheet()
    {
        playerOneSheet.SetActive(!playerOneSheet.activeSelf);
        playerTwoSheet.SetActive(false);
    }
    public void TogglePlayerTwoSheet()
    {
        playerOneSheet.SetActive(false);
        playerTwoSheet.SetActive(!playerTwoSheet.activeSelf);
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
}
