using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggers : MonoBehaviour
{
    [SerializeField] bool combatTrigger;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (combatTrigger)
        {
            if (other.transform.GetComponent<ForceMovement>().IsFighting == true)
            {
                return;
            }
            other.transform.GetComponent<ForceMovement>().IsFighting = true;
        }
    }
}
