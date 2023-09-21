using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class CharacterSheet : ScriptableObject
{
    [Header("Character Info")]
    public string characterName;
    public GameObject characterModel;


    public void OnInspectorGUI()
    {
        Debug.Log("Yes");
        Class newValue = (Class)EditorGUILayout.EnumPopup(_class);
        if (newValue != _class)
        {
            _class = newValue;
            if (_class == Class.Goblin)
            {
                characterName = "Goblin";
                MaxHitPoints = 7;
                HitPoints = 7;
                Proficiency = 2;
                Speed = 30;
                InitiativeBonus = 2;
                ArmorClass = 15;
                Strength = 8;
                Dexterity = 14;
                Constitution = 10;
                Intelligence = 10;
                Wisdom = 8;
                Charisma = 8;
            }
            else if (_class == Class.Knight)
            {
                characterName = "Knight";
                MaxHitPoints = 12;
                HitPoints = 12;
                Proficiency = 2;
                Speed = 30;
                InitiativeBonus = 3;
                ArmorClass = 14;
                Strength = 14;
                Dexterity = 16;
                Constitution = 15;
                Intelligence = 11;
                Wisdom = 13;
                Charisma = 9;
            }
        }
    }


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

