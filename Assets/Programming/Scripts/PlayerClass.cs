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
        // maxHitPoints = stats.maxHitPoints;
        // hitPoints = stats.hitPoints;
        // armorClass = stats.armorClass;
        // initiativeBonus = stats.initiativeBonus;
        // speed = stats.speed;
        // proficiency = stats.proficiency;
        // strength = stats.strength;
        // dexterity = stats.dexterity;
        // constitution = stats.constitution;
        // intelligence = stats.intelligence;
        // wisdom = stats.wisdom;
        // charisma = stats.charisma;
    }
}
