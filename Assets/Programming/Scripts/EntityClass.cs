using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityClass : MonoBehaviour
{
    public ClassSheet stats;
    [SerializeField] private ForceMovement movement;
    [SerializeField] private MouseLook mouse;
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
    public int
        strenghtBonus,
        dexterityBonus,
        constitutionBonus,
        intelligenceBonus,
        wisdomBonus,
        charismaBonus;

    bool statsSet;
    private void SetStats()
    {
        statsSet = true;
        level = stats.Level;
        currentXp = stats.CurrentXP;
        xpToGo = stats.XpToGo;
        isPlayer = stats.isPlayer;
        maxHitPoints = stats.MaxHitPoints;
        hitPoints = stats.HitPoints;
        armorClass = stats.ArmorClass;
        initiativeBonus = stats.InitiativeBonus;
        speed = stats.Speed;
        proficiency = stats.Proficiency;
        strength = stats.Strength;
        dexterity = stats.Dexterity;
        constitution = stats.Constitution;
        intelligence = stats.Intelligence;
        wisdom = stats.Wisdom;
        charisma = stats.Charisma;
    }

    private void Update()
    {
        // if (stats._class == Class.Goblin)
        // {
        //     stats.characterName = "Goblin";
        //     stats.MaxHitPoints = 7;
        //     stats.HitPoints = 7;
        //     stats.Proficiency = 2;
        //     stats.Speed = 30;
        //     stats.InitiativeBonus = 2;
        //     stats.ArmorClass = 15;
        //     stats.Strength = 8;
        //     stats.Dexterity = 14;
        //     stats.Constitution = 10;
        //     stats.Intelligence = 10;
        //     stats.Wisdom = 8;
        //     stats.Charisma = 8;
        //     if (!statsSet)
        //         SetStats();
        // }
        if (stats._class == Class.Knight)
        {
            stats.characterName = "Knight";
            stats.MaxHitPoints = 12;
            stats.HitPoints = 12;
            stats.Proficiency = 2;
            stats.Speed = 30;
            stats.InitiativeBonus = 3;
            stats.ArmorClass = 14;
            stats.Strength = 14 + strenghtBonus;
            stats.Dexterity = 16 + dexterityBonus;
            stats.Constitution = 15 + constitutionBonus;
            stats.Intelligence = 11 + intelligenceBonus;
            stats.Wisdom = 13 + wisdomBonus;
            stats.Charisma = 9 + charismaBonus;
            if (!statsSet)
                SetStats();
        }
        else if (stats._class == Class.Ranger)
        {
            stats.characterName = "Ranger";
            stats.MaxHitPoints = 11;
            stats.HitPoints = 1;
            stats.Proficiency = 2;
            stats.Speed = 35;
            stats.InitiativeBonus = 3;
            stats.ArmorClass = 14;
            stats.Strength = 12;
            stats.Dexterity = 17;
            stats.Constitution = 13;
            stats.Intelligence = 10;
            stats.Wisdom = 15;
            stats.Charisma = 8;
            if (!statsSet)
                SetStats();
        }
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
}
