using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SetPlayerInfo : NetworkBehaviour
{
    [SerializeField] List<ForceMovement> myEntities;

    ForceMovement[] m;
    [Tooltip("Time until it moves on to check all MouseLook's found")]
    [SerializeField] float time = 3f;

    bool camtargetSet;
    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            m = FindObjectsOfType<ForceMovement>();
            print($"Time until Set CamTarget: {time}");
        }
        else if (!camtargetSet)
        {
            camtargetSet = true;
            print($"ForceMovement Found In M: {m.Length}");
            for (int i = 0; i < m.Length; i++)
            {
                print($"Is Owner of Movment{i}: {m[i].IsOwner}");
                if (!myEntities.Contains(m[i]) && m[i].IsOwner)
                {
                    myEntities.Add(m[i]);
                    myEntities[i].GetComponent<MouseLook>().CamTarget = myEntities[i].gameObject.transform.GetChild(1);
                    myEntities[i].GetComponent<MouseLook>().CamTarget = myEntities[i].gameObject.transform.GetChild(1);
                    myEntities[i].GetComponent<MouseLook>().VirCam.Follow = myEntities[i].gameObject.transform.GetChild(1);
                    myEntities[i].GetComponent<MouseLook>().VirCam.LookAt = myEntities[i].gameObject.transform.GetChild(1);
                }

            }
        }
    }
}
