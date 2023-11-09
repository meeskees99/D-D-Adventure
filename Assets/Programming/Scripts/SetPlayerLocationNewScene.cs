using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerLocationNewScene : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform location;

    bool locationSet;

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<ForceMovement>().gameObject;
        }
        else if (!locationSet && player.GetComponent<ForceMovement>().IsOwner)
        {
            player.transform.position = location.position;
            locationSet = true;
        }
    }
}
