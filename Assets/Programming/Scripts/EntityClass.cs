using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using Unity.Collections;
public class EntityClass : NetworkBehaviour
{
    public ClassSheet stats;
    public CombatHandler combatHandler = CombatHandler.instance;
    public bool isPlayer;
    public WeaponObject currentWeapon;
    public WeaponObject[] weaponOptions;
    [Header("Death Saves")]
    public int deathSaveAttempts;
    [SerializeField] int saves;
    [SerializeField] int fails;

    [Header("Stats")]
    public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxHitPoints = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> hitPoints = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> level = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentXp = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> xpToGo = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> armorClass = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> initiativeBonus = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> speed = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> proficiency = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> strength = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> dexterity = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> constitution = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> intelligence = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> wisdom = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> charisma = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Stats Race Bonus")]
    public int strenghtBonus;
    public int
        dexterityBonus,
        constitutionBonus,
        intelligenceBonus,
        wisdomBonus,
        charismaBonus;

    void Start()
    {
        if (!IsOwner) { return; }

        if (stats._class == Class.Knight)
        {
            stats.XpToGo = 50;
            stats.MaxHitPoints = 12;
            stats.HitPoints = stats.MaxHitPoints;
            stats.InitiativeBonus = 3;
            stats.Proficiency = 2;
            stats.Speed = 30;
            stats.ArmorClass = 14;
            stats.Strength = 14;
            stats.Dexterity = 16;
            stats.Constitution = 15;
            stats.Intelligence = 11;
            stats.Wisdom = 13;
            stats.Charisma = 9;
        }
        else if (stats._class == Class.Ranger)
        {
            stats.XpToGo = 50;
            stats.MaxHitPoints = 11;
            stats.HitPoints = stats.MaxHitPoints;
            stats.InitiativeBonus = 3;
            stats.Proficiency = 2;
            stats.Speed = 35;
            stats.ArmorClass = 14;
            stats.Strength = 12;
            stats.Dexterity = 17;
            stats.Constitution = 13;
            stats.Intelligence = 10;
            stats.Wisdom = 15;
            stats.Charisma = 8;
        }

        playerName.Value = stats.CharacterName;
        maxHitPoints.Value = stats.MaxHitPoints;
        level.Value = stats.Level;
        currentXp.Value = stats.CurrentXP;
        xpToGo.Value = stats.XpToGo;
        hitPoints.Value = stats.HitPoints;
        armorClass.Value = stats.ArmorClass;
        initiativeBonus.Value = stats.InitiativeBonus;
        speed.Value = stats.Speed;
        strength.Value = stats.Strength;
        proficiency.Value = stats.Proficiency;
        constitution.Value = stats.Constitution;
        dexterity.Value = stats.Dexterity;
        intelligence.Value = stats.Intelligence;
        wisdom.Value = stats.Wisdom;
        charisma.Value = stats.Charisma;

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

    private void Update()
    {

    }
    public void TakeDamage(int damage)
    {
        hitPoints.Value -= damage;
        if (hitPoints.Value <= 0)
        {
            Debug.Log(name + " has died!");
            hitPoints.Value = 0;
            // NetworkManager.Singleton.SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
            // Die();
        }
    }
    public void Heal(int amount)
    {
        hitPoints.Value += amount;
        if (hitPoints.Value > maxHitPoints.Value)
        {
            hitPoints.Value = maxHitPoints.Value;
        }
    }
    public void LevelUp(int extraXp)
    {
        level.Value++;
        xpToGo.Value = Mathf.RoundToInt(xpToGo.Value * 1.2f);
        currentXp.Value = extraXp;
        //xpToGo = formule gebasseer op lvl
        //increase stats
    }
    public void AddXp(int amount)
    {
        currentXp.Value += amount;
        if (currentXp.Value >= xpToGo.Value)
        {
            int extraXp = currentXp.Value - xpToGo.Value;
            LevelUp(extraXp);
        }
    }

    System.Random random = new System.Random();
    public void DeathSave(int throwValue)
    {
        if (hitPoints.Value <= 0)
        {
            if (throwValue == 0)
            {
                int _throwValue = random.Next(1, 20);
                if (_throwValue > 10)
                {
                    saves++;
                    if (saves == 3)
                    {
                        hitPoints.Value = 1;
                        print("You saved yourself! You now have one health");
                    }
                }
                else
                {
                    fails++;
                    if (fails == 3)
                    {
                        Die();
                    }
                }
            }
            else
            {
                if (throwValue > 10)
                {
                    saves++;
                    if (saves == 3)
                    {
                        hitPoints.Value = 1;
                        print("You saved yourself! You now have one health");
                        saves = 0;
                        fails = 0;
                    }
                }
                else
                {
                    fails++;
                    if (fails == 3)
                    {
                        Die();
                        saves = 0;
                        fails = 0;
                    }
                }
            }

        }
        else
        {
            print("No deathsave necessary");
        }



    }
    public void Die()
    {
        print("You Died! You missed 3 death saves!");
        DieServerRpc();
    }

    [ServerRpc]//joe moeder
    public void DieServerRpc(ServerRpcParams serverRpcParams = default)
    {
        level.Value = 0;
        currentXp.Value = 0;
        xpToGo.Value = 50;
        hitPoints.Value = maxHitPoints.Value;
    }


}