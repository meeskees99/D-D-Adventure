using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    InitiativeHandler initiativeHandler;
    [SerializeField] public static CombatHandler instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void CheckAttack(GameObject attacker, GameObject victim, float minDistance, float maxDistance)
    {
        var victimClass = victim.GetComponent<EntityClass>();
        //check distance between attacker and victim and compare that to the weapons effective range
        float distance = Vector3.Distance(attacker.transform.position, victim.transform.position);
        if (distance < maxDistance)
        {
            if (distance > minDistance)
            {
                Debug.Log("Target in Range");
                if (victimClass.armorClass < Random.Range(1, 20))
                {
                    victimClass.TakeDamage(Random.Range(1, 8));

                }
                else
                {
                    Debug.Log("Target to close");
                    if (victimClass.armorClass < DisadvantageRoll())
                    {
                        victimClass.TakeDamage(Random.Range(1, 8));
                    }
                }
            }
            else
            {
                //no hit
                Debug.Log("target to far away");
            }
        }
    }
        public int DisadvantageRoll()
        {
            int roll1 = Random.Range(1, 20);
            int roll2 = Random.Range(1, 20);
            if (roll1 > roll2)
            {
                return roll2;
            }
            else
            {
                return roll1;
            }
        }
        public int AdvantageRoll(int max, int _damage)
        {
            int roll1 = Random.Range(1, 20);
            int roll2 = Random.Range(1, 20);
            if (roll1 < roll2)
            {
                return roll2;
            }
            else
            {
                return roll1;
            }
        }
    }
