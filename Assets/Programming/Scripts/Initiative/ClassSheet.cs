using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class ClassSheet : ScriptableObject
{
    [Header("Character Info")]
    public string characterName;
    public bool isPlayer;

    [SerializeField] public Class _class;
    [SerializeField] public int MaxHitPoints { get; set; }
    [SerializeField] public int HitPoints { get; set; }
    [SerializeField] public int Proficiency { get; set; }
    [SerializeField] public int Speed { get; set; }
    [SerializeField] public int InitiativeBonus { get; set; }
    [SerializeField] public int ArmorClass { get; set; }
    [SerializeField] public int Strength { get; set; }
    [SerializeField] public int Dexterity { get; set; }
    [SerializeField] public int Constitution { get; set; }
    [SerializeField] public int Intelligence { get; set; }
    [SerializeField] public int Wisdom { get; set; }
    [SerializeField] public int Charisma { get; set; }
}
[SerializeField]
public enum Class
{
    Goblin,
    Knight,
    Ranger
}

