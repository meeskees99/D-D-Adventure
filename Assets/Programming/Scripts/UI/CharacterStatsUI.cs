using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterStatsUI : MonoBehaviour
{
    [SerializeField] CharacterSheetUIManager uIManager;
    [Range(1, 2)]
    [SerializeField] int playerNumber;
    [Header("Character Stats")]
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text classType;
    [SerializeField] TMP_Text race;
    [SerializeField] TMP_Text level;
    [SerializeField] TMP_Text xp;
    [Header("Extra")]
    [SerializeField] TMP_Text profB;
    [SerializeField] TMP_Text armorClass;
    [SerializeField] TMP_Text initiative;
    [SerializeField] TMP_Text speed;
    [Header("Ability Scores")]
    [SerializeField] TMP_Text strenghtNor;
    [SerializeField] TMP_Text strenghtBonus;
    [SerializeField] TMP_Text dexterityNor;
    [SerializeField] TMP_Text dexterityBonus;
    [SerializeField] TMP_Text constitutionNor;
    [SerializeField] TMP_Text constitutionBonus;
    [SerializeField] TMP_Text intelligenceNor;
    [SerializeField] TMP_Text intelligenceBonus;
    [SerializeField] TMP_Text wisdomNor;
    [SerializeField] TMP_Text wisdomBonus;
    [SerializeField] TMP_Text charismaNor;
    [SerializeField] TMP_Text charismaBonus;
    // Update is called once per frame
    void Update()
    {
        if (uIManager.players.Count == 0)
            return;
        #region Character Stats
        playerName.text = uIManager.players[playerNumber - 1].name;
        classType.text = uIManager.players[playerNumber - 1].stats._class.ToString();
        race.text = uIManager.players[playerNumber - 1].stats._race.ToString();
        level.text = uIManager.players[playerNumber - 1].level.ToString();
        xp.text = uIManager.players[playerNumber - 1].currentXp + "/" + uIManager.players[playerNumber - 1].xpToGo;
        #endregion
        #region Extra
        profB.text = uIManager.players[playerNumber - 1].proficiency.ToString();
        armorClass.text = uIManager.players[playerNumber - 1].armorClass.ToString();
        initiative.text = uIManager.players[playerNumber - 1].initiativeBonus.ToString();
        speed.text = uIManager.players[playerNumber - 1].speed.ToString();
        #endregion
        #region Ability Scores
        strenghtNor.text = uIManager.players[playerNumber - 1].strength.ToString();
        strenghtBonus.text = uIManager.players[playerNumber - 1].strenghtBonus.ToString();
        dexterityNor.text = uIManager.players[playerNumber - 1].dexterity.ToString();
        dexterityBonus.text = uIManager.players[playerNumber - 1].dexterityBonus.ToString();
        constitutionNor.text = uIManager.players[playerNumber - 1].constitution.ToString();
        constitutionBonus.text = uIManager.players[playerNumber - 1].constitutionBonus.ToString();
        intelligenceNor.text = uIManager.players[playerNumber - 1].intelligence.ToString();
        intelligenceBonus.text = uIManager.players[playerNumber - 1].intelligenceBonus.ToString();
        wisdomNor.text = uIManager.players[playerNumber - 1].wisdom.ToString();
        wisdomBonus.text = uIManager.players[playerNumber - 1].wisdomBonus.ToString();
        charismaNor.text = uIManager.players[playerNumber - 1].charisma.ToString();
        charismaBonus.text = uIManager.players[playerNumber - 1].charismaBonus.ToString();
        #endregion
    }
}
