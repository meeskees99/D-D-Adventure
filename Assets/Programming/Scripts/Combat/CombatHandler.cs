using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    InitiativeHandler initiativeHandler = InitiativeHandler.instance;
    [SerializeField] public static CombatHandler instance;
    public GameObject curTarget;
    GameObject currentAttacker;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void CheckAttack(GameObject attacker, GameObject victim, WeaponObject weaponUsed)
    {
        var victimClass = victim.GetComponent<EntityClass>();
        //check distance between attacker and victim and compare that to the weapons effective range
        float distance = Vector3.Distance(attacker.transform.position, victim.transform.position);
        if (distance < weaponUsed.maxDistance)
        {
            if (distance > weaponUsed.minDistance)
            {
                Debug.Log("Target in Range");
                if (victimClass.armorClass < UnityEngine.Random.Range(1, 20))
                {
                    victimClass.TakeDamage(UnityEngine.Random.Range(1, weaponUsed.maxDamageRange));
                }
            }
            else
            {
                Debug.Log("Target too close");
                if (victimClass.armorClass < DisadvantageRoll())
                {
                    victimClass.TakeDamage(UnityEngine.Random.Range(1, weaponUsed.maxDamageRange));
                }
            }  
        }
        else
        {
            //no hit
            Debug.Log("target too far away");
        }
        victim.GetComponent<Outline>().enabled = false;
    }
    
    public int DisadvantageRoll()
    {
        int roll1 = UnityEngine.Random.Range(1, 20);
        int roll2 = UnityEngine.Random.Range(1, 20);
        return (int)MathF.Min(roll1, roll2);
    }
    public int AdvantageRoll()
    {
        int roll1 = UnityEngine.Random.Range(1, 20);
        int roll2 = UnityEngine.Random.Range(1, 20);
        return (int)MathF.Max(roll1, roll2);
    }

    public void SelectTarget(GameObject victim)
    {
        if (curTarget != null)
        {
            curTarget.GetComponent<Outline>().enabled = false;
        }
        
        if (victim.TryGetComponent(out Outline outline))
        {
           outline.enabled = true;
        }
        else
        {
            victim.AddComponent<Outline>();
            victim.GetComponent<Outline>().enabled = true;
        }
        curTarget = victim;
        //enable attack select UI
    }

    public void SelectWeapon(int newWeaponValue)
    {
        currentAttacker.TryGetComponent(out EntityClass entity);
        entity.currentWeapon = entity.weaponOptions[newWeaponValue];
        CheckAttack(currentAttacker, curTarget, entity.currentWeapon);
    }
}
