using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;
using System;
public class EntityClass : NetworkBehaviour
{
    public ClassSheet stats;
    public CombatHandler combatHandler = CombatHandler.instance;
    public string playerName;
    public bool isPlayer;

    [Header("Stats")]
    [SerializeField] private int maxHitPoints;
    public int
        level,
        currentXp,
        xpToGo,
        hitPoints,
        armorClass,
        initiativeBonus,
        speed,
        proficiency,
        strength,
        dexterity,
        constitution,
        intelligence,
        wisdom,
        charisma;

    [Header("Stats Race Bonus")]
    public int strenghtBonus;
    public int
        dexterityBonus,
        constitutionBonus,
        intelligenceBonus,
        wisdomBonus,
        charismaBonus;

    void Start()
    {
        if (PlayerPrefs.GetInt("FirstTime") == 1)
        {
            if (stats._class == Class.Knight)
            {
                stats.XpToGo = 50;
                stats.MaxHitPoints = 12;
                stats.HitPoints = stats.MaxHitPoints;
                stats.InitiativeBonus = 3;
                stats.Proficiency = 2;
                stats.Speed = 30;
                stats.ArmorClass = 14;
                stats.Strength = 14;
                stats.Dexterity = 16;
                stats.Constitution = 15;
                stats.Intelligence = 11;
                stats.Wisdom = 13;
                stats.Charisma = 9;
            }
            else if (stats._class == Class.Ranger)
            {
                stats.XpToGo = 50;
                stats.MaxHitPoints = 11;
                stats.HitPoints = stats.MaxHitPoints;
                stats.InitiativeBonus = 3;
                stats.Proficiency = 2;
                stats.Speed = 35;
                stats.ArmorClass = 14;
                stats.Strength = 12;
                stats.Dexterity = 17;
                stats.Constitution = 13;
                stats.Intelligence = 10;
                stats.Wisdom = 15;
                stats.Charisma = 8;
            }


            PlayerPrefs.SetInt("FirstTime", 1);
        }
        // if (stats._class == Class.Knight)
        // {
        //     stats.MaxHitPoints = 12;
        //     stats.HitPoints = stats.MaxHitPoints;
        //     stats.InitiativeBonus = stats.i
        //     stats.Speed
        //     stats.ArmorClass =
        //     stats.Proficiency
        //     stats.Constitution
        //     stats.Intelligence
        //     stats.Wisdom
        //     stats.Charisma
        // }
        // else if (stats._class == Class.Ranger)
        // {

        //     stats.MaxHitPoints = 17;
        //     stats.Level
        //     stats.CurrentXP
        //     stats.XpToGo
        //     stats.HitPoints
        //     stats.InitiativeBonus
        //     stats.Speed
        //     stats.ArmorClass
        //     stats.Proficiency
        //     stats.Constitution
        //     stats.Intelligence
        //     stats.Wisdom
        //     stats.Charisma
        // }
        // else if (stats._class == Class.)
        // {

        //     stats.MaxHitPoints = 17;
        //     stats.Level
        //     stats.CurrentXP
        //     stats.XpToGo
        //     stats.HitPoints
        //     stats.ArmorClass
        //     stats.InitiativeBonus
        //     stats.Speed
        //     stats.Proficiency
        //     stats.Constitution
        //     stats.Intelligence
        //     stats.Wisdom
        //     stats.Charisma
        // }
        playerName = stats.CharacterName;
        maxHitPoints = stats.MaxHitPoints;
        level = stats.Level;
        currentXp = stats.CurrentXP;
        xpToGo = stats.XpToGo;
        hitPoints = stats.HitPoints;
        armorClass = stats.ArmorClass;
        initiativeBonus = stats.InitiativeBonus;
        speed = stats.Speed;
        strength = stats.Strength;
        proficiency = stats.Proficiency;
        constitution = stats.Constitution;
        dexterity = stats.Dexterity;
        intelligence = stats.Intelligence;
        wisdom = stats.Wisdom;
        charisma = stats.Charisma;

        if (stats._race == Race.Human)
        {
            strenghtBonus = 1;
            dexterityBonus = 1;
            constitutionBonus = 1;
            intelligenceBonus = 1;
            wisdomBonus = 1;
            charismaBonus = 1;
        }
        else if (stats._race == Race.Elf)
        {
            strenghtBonus = 0;
            dexterityBonus = 2;
            constitutionBonus = 0;
            intelligenceBonus = 0;
            wisdomBonus = 1;
            charismaBonus = 0;
        }
        else if (stats._race == Race.Goblin)
        {
            strenghtBonus = 0;
            dexterityBonus = 2;
            constitutionBonus = 1;
            intelligenceBonus = 0;
            wisdomBonus = 0;
            charismaBonus = 0;
        }
    }

    private void Update()
    {

    }
    public void TakeDamage(int damage)
    {
        this.hitPoints -= damage;
        if (this.hitPoints <= 0)
        {
            Debug.Log(name + " has died!");
            Destroy(this);
        }
    }
    public void Heal(int amount)
    {
        this.hitPoints += amount;
        if (this.hitPoints > this.maxHitPoints)
        {
            this.hitPoints = this.maxHitPoints;
        }
    }
    public void LevelUp(int extraXp)
    {
        level++;
        xpToGo = Mathf.RoundToInt(xpToGo * 1.2f);
        currentXp = extraXp;
        //xpToGo = formule gebasseer op lvl
        //increase stats
    }
    public void AddXp(int amount)
    {
        currentXp += amount;
        if (currentXp >= xpToGo)
        {
            int extraXp = currentXp - xpToGo;
            LevelUp(extraXp);
        }
    }
}