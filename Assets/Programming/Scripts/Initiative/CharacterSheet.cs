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
    [SerializeField] public int maxHitPoints { get; set; }
    [SerializeField] public int hitPoints { get; set; }
    [SerializeField] public int proficiency { get; set; }
    [SerializeField] public int speed { get; set; }
    [SerializeField] public int initiativeBonus { get; set; }
    [SerializeField] public int armorClass { get; set; }

    [Header("Stats")]
    [SerializeField] private int strength;
    [SerializeField] private int dexterity,
        constitution,
        intelligence,
        wisdom,
        charisma;
}
