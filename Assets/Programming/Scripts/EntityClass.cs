using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityClass : MonoBehaviour
{
    [SerializeField] private ClassSheet stats;
    [SerializeField] private ForceMovement movement;
    [SerializeField] private MouseLook mouse;
    [SerializeField] string playerName;
    public bool isPlayer;
    [Header("Stats")]
    [SerializeField] private int maxHitPoints;
    [SerializeField]
    public int
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
    bool statsSet;
    private void SetStats()
    {
        statsSet = true;
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
        if (stats._class == Class.Goblin)
        {
            stats.characterName = "Goblin";
            stats.MaxHitPoints = 7;
            stats.HitPoints = 7;
            stats.Proficiency = 2;
            stats.Speed = 30;
            stats.InitiativeBonus = 2;
            stats.ArmorClass = 15;
            stats.Strength = 8;
            stats.Dexterity = 14;
            stats.Constitution = 10;
            stats.Intelligence = 10;
            stats.Wisdom = 8;
            stats.Charisma = 8;
            if (!statsSet)
                SetStats();
        }
        else if (stats._class == Class.Knight)
        {
            stats.characterName = "Knight";
            stats.MaxHitPoints = 12;
            stats.HitPoints = 12;
            stats.Proficiency = 2;
            stats.Speed = 30;
            stats.InitiativeBonus = 3;
            stats.ArmorClass = 14;
            stats.Strength = 14;
            stats.Dexterity = 16;
            stats.Constitution = 15;
            stats.Intelligence = 11;
            stats.Wisdom = 13;
            stats.Charisma = 9;
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
    }
}
