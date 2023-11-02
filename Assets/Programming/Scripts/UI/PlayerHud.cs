using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] EntityClass myClass;

    [Header("Stats")]
    [SerializeField] TMP_Text classTxt;
    [SerializeField] TMP_Text levelTxt;
    [SerializeField] TMP_Text healthTxt;
    [SerializeField] Slider healthSlider;

    [Header("Ability Scores")]
    [SerializeField] TMP_Text strenghtTxt;
    // [SerializeField] TMP_Text strenghtBonus;
    [SerializeField] TMP_Text dexterityTxt;
    // [SerializeField] TMP_Text dexterityBonus;
    [SerializeField] TMP_Text constitutionTxt;
    // [SerializeField] TMP_Text constitutionBonus;
    [SerializeField] TMP_Text intelligenceTxt;
    // [SerializeField] TMP_Text intelligenceBonus;
    [SerializeField] TMP_Text wisdomTxt;
    // [SerializeField] TMP_Text wisdomBonus;
    [SerializeField] TMP_Text charismaTxt;
    // [SerializeField] TMP_Text charismaBonus;
    [SerializeField] TMP_Text armorClassTxt;

    [Header("Death Saves")]
    [SerializeField] GameObject deathSavePanel;
    int saveAmount;
    [SerializeField] GameObject[] saves;
    int failAmount;
    [SerializeField] GameObject[] fails;
    [SerializeField] TMP_Text rolledText;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (myClass == null)
        {
            EntityClass[] classToGet = FindObjectsOfType<EntityClass>();
            foreach (EntityClass entity in classToGet)
            {
                print($"Is owner of {entity.gameObject.name}: {entity.IsOwner}");
                if (entity.IsOwner)
                {
                    myClass = entity;
                }
            }
        }
        else
        {
            if (myClass.IsServer)
            {
                enabled = false;
            }
            if (myClass.hitPoints.Value <= 0)
            {
                deathSavePanel.SetActive(true);
            }
            else
            {
                deathSavePanel.SetActive(false);
            }
            #region Player Stats
            #region Stats
            classTxt.text = myClass.playerName;
            levelTxt.text = "Level " + myClass.level.Value;
            healthTxt.text = myClass.hitPoints.Value + "/" + myClass.maxHitPoints.Value;
            healthSlider.maxValue = myClass.maxHitPoints.Value;
            healthSlider.value = myClass.hitPoints.Value;
            #endregion
            #region Ability Scores
            strenghtTxt.text = myClass.strenghtBonus.ToString();
            dexterityTxt.text = myClass.dexterityBonus.ToString();
            constitutionTxt.text = myClass.constitutionBonus.ToString();
            intelligenceTxt.text = myClass.intelligenceBonus.ToString();
            wisdomTxt.text = myClass.wisdomBonus.ToString();
            charismaTxt.text = myClass.charismaBonus.ToString();
            armorClassTxt.text = myClass.armorClass.ToString();
            #endregion
            #endregion
            #region Weapon Stats


            #endregion
        }

    }

    public void TakeDamage(int amount)
    {
        myClass.TakeDamage(amount);
    }

    public void Heal(int amount)
    {
        myClass.Heal(amount);
    }

    public void LevelUp()
    {
        myClass.LevelUp(0);
    }

    System.Random random = new System.Random();
    public void TryDeathSave()
    {
        int throwValue = random.Next(1, 20);
        rolledText.text = "Rolled :" + throwValue;
        if (throwValue > 10)
        {
            saves[saveAmount].SetActive(true);
            saveAmount++;
            if (saveAmount == 3)
            {
                for (int i = 0; i < saves.Length; i++)
                {
                    fails[i].SetActive(false);
                    saves[i].SetActive(false);
                    failAmount = 0;
                    saveAmount = 0;
                    rolledText.text = "";
                }
            }
        }
        else
        {
            fails[failAmount].SetActive(true);
            failAmount++;
            if (failAmount == 3)
            {
                for (int i = 0; i < fails.Length; i++)
                {
                    fails[i].SetActive(false);
                    saves[i].SetActive(false);
                    failAmount = 0;
                    saveAmount = 0;
                    rolledText.text = "";
                }
            }
        }
        myClass.DeathSave(throwValue);
    }
}
