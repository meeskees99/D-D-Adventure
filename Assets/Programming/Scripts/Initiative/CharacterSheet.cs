using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class CharacterSheet : ScriptableObject
{
    [Header("Character Info")]
    public string charaterName;
    public GameObject characterModel;
    [SerializeField] private enum Class
    {
        None,
        Knight,
        Ranger
    }
    [SerializeField] private Class _class;
    [SerializeField] private int maxHitPoints,
        hitPoints,
        proficiency,
        speed,
        initiativeBonus ,
        armorClass;
    
    [Header("Stats")]
    [SerializeField] private int strength;
    [SerializeField] private int dexterity,
        constitution,
        intelligence,
        wisdom,
        charisma;
}
