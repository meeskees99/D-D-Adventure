using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private CharacterSheet stats;
    [SerializeField] private ForceMovement movement;
    [SerializeField] private MouseLook mouse;
    [Header("Stats")]
    [SerializeField] private int maxHitPoints;
    [SerializeField]
    private int
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

    private void SetStats()
    {
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
}
