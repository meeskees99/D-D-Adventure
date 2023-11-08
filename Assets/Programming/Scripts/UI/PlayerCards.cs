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
    public TMP_Text characternameText;

    public Button nextCharacterButton;
    public Button previousCharacterButton;

    public Button lockInButton;


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
        playerNameText.text = state.IsLockedIn ? $"Player {state.ClientId}" : $"Player {state.ClientId}\n(Picking...)";
        visuals.SetActive(true);
        // Debug.LogError($"Set visuals active for {gameObject.name}.");
    }

    public void DisableDisplay()
    {
        visuals.SetActive(false);
    }
}
