using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons")]
public class WeaponObject : ScriptableObject
{
    public string weaponName;
    public float minDistance;
    public float maxDistance;
    public int maxDamageRange;
}
