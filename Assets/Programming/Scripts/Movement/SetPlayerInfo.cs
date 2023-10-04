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
                    for (int e = 0; e < myEntities.Count; e++)
                    {
                        print($"MyEntities[e] = {myEntities[e]}");
                        myEntities[e].GetComponent<MouseLook>().CamTarget = myEntities[e].gameObject.transform.GetChild(1);
                        myEntities[e].GetComponent<MouseLook>().CamTarget = myEntities[e].gameObject.transform.GetChild(1);
                        myEntities[e].GetComponent<MouseLook>().VirCam.Follow = myEntities[e].gameObject.transform.GetChild(1);
                        myEntities[e].GetComponent<MouseLook>().VirCam.LookAt = myEntities[e].gameObject.transform.GetChild(1);
                    }
                }

            }
        }
    }
}
