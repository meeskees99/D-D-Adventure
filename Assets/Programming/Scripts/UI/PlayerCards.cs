using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCards : MonoBehaviour
{
    [SerializeField] Characters characterDataBase;
    [SerializeField] GameObject visuals;
    [SerializeField] Image characterIconImage;
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] TMP_Text characternameText;

    public void UpdateDisplay(CharacterSelection state)
    {
        if (state.CharacterId != -1)
        {
            var character = characterDataBase.GetCharacterById(state.CharacterId);
            characterIconImage.sprite = character.Icon;
            characterIconImage.enabled = true;
            characternameText.text = character.CharacterName;
        }
        else
        {
            characterIconImage.enabled = false;
        }
        playerNameText.text = $"Player {state.ClientId}";
        visuals.SetActive(true);
    }

    public void DisableDisplay()
    {
        visuals.SetActive(false);
    }
}
