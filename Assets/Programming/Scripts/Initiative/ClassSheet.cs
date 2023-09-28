using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
// using UnityEditor.PackageManager.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class ClassSheet : ScriptableObject
{
    [Header("Character Info")]
    [SerializeField] int id = -1;
    [SerializeField] string characterName;
    [SerializeField] Sprite icon;

    public int Id => id;
    public string CharacterName => characterName;
    public Sprite Icon => icon;


    public bool isPlayer;

    public Class _class;
    public Race _race;
    public int Level { get; set; }
    public int CurrentXP { get; set; }
    public int XpToGo { get; set; }
    public int MaxHitPoints { get; set; }
    public int HitPoints { get; set; }
    public int Proficiency { get; set; }
    public int Speed { get; set; }
    public int InitiativeBonus { get; set; }
    public int ArmorClass { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }

}
[SerializeField]
public enum Class
{
    Knight,
    Ranger
}
public enum Race
{
    Human,
    Elf,
    Goblin
}

