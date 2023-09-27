using UnityEngine.UI;
using UnityEngine;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Button button;

    CharacterSelectionDisplay characterSelect;
    ClassSheet character;

    public void SetCharacter(CharacterSelectionDisplay characterSelect, ClassSheet character)
    {
        iconImage.sprite = character.Icon;

        this.characterSelect = characterSelect;
        this.character = character;
        Debug.Log("Character set");
    }

    public void SelectCharacter()
    {
        characterSelect.Select(character);
    }


}
