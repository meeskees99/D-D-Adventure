using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingleEntityClass : MonoBehaviour
{
    public ClassSheet stats;
    public CombatHandler combatHandler = CombatHandler.instance;
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
    private void Update()
    {
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
    public void TakeDamage(int damage)
    {
        this.hitPoints -= damage;
        if (this.hitPoints <= 0)
        {
            Debug.Log(name + " has died!");
            Destroy(this);
        }
    }
    public void Heal(int amount)
    {
        this.hitPoints += amount;
        if (this.hitPoints > this.maxHitPoints)
        {
            this.hitPoints = this.maxHitPoints;
        }
    }
    public void LevelUp()
    {
        level++;
        currentXp = 0;
        //xpToGo = formule gebasseer op lvl
        //increase stats
    }
}