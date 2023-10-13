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
    [SerializeField] Slider xpSlider;
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

    void Start()
    {
        if (playerNumber == 0)
            playerNumber = 1;
    }

    void Update()
    {
        if (uIManager.players.Count == 0)
            return;
        #region Main Tab
        #region Character Stats
        playerName.text = uIManager.players[playerNumber - 1].playerName;
        classType.text = uIManager.players[playerNumber - 1].stats._class.ToString();
        race.text = uIManager.players[playerNumber - 1].stats._race.ToString();
        level.text = uIManager.players[playerNumber - 1].level.ToString();
        xp.text = uIManager.players[playerNumber - 1].currentXp + "/" + uIManager.players[playerNumber - 1].xpToGo;
        xpSlider.maxValue = uIManager.players[playerNumber - 1].xpToGo;
        xpSlider.value = uIManager.players[playerNumber - 1].currentXp;
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
        #endregion
    }

    //For Test Buttons
    public void LevelUp(int player)
    {
        uIManager.players[player - 1].AddXp(uIManager.players[player - 1].xpToGo);
    }

    public void AddXp(int player)
    {
        int xpAmount = 10;
        uIManager.players[player - 1].AddXp(xpAmount);
    }
}
