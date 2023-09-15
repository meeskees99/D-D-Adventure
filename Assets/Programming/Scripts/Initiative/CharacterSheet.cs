using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet : MonoBehaviour 
{
    [Header("Character Info")]
    public string charaterName;
    public GameObject characterModel;
    public enum Sexes
    {
        Male,
        Female
    }
    public Sexes sex;
    public enum Class
    {
        Knight,
        Ranger
    }
    public Class _class;
    public int maxHitPoints,
        hitPoints,
        proficiency,
        walking,
        initiativeBonus,
        armorClass;

    [Header("Stats")]
    public int strength;
    public int dexterity,
        constitution,
        intelligence,
        wisdom,
        charisma;

    public void Start()
    {
        
    }

}
