using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedData
{
    //List of all players in campaign
    [SerializeField] public List<(int, int, int, int, int, int, int ,int)> playerStats = new List<(int, int, int, int, int, int, int ,int)> ();
    public List<Vector3> entityPos;
}
