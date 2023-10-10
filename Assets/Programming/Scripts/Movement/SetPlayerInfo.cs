using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class SetPlayerInfo : NetworkBehaviour
{
    [SerializeField] List<ForceMovement> myEntities;
    [SerializeField] MouseLook MouseLook;
    ForceMovement[] m;
    // [Tooltip("Time until it moves on to check all MouseLook's found")]
    // [SerializeField] float time = 3f;

    bool camtargetSet;
    // Update is called once per frame
    private void Start()
    {
        MouseLook = FindObjectOfType<MouseLook>();
    }
    void Update()
    {
        if (!camtargetSet)
        {
            SetCamTarget();
        }
    }

    void SetCamTarget()
    {
        m = FindObjectsOfType<ForceMovement>();
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
                    MouseLook.CamTarget = myEntities[e].gameObject.transform.GetChild(1);
                    MouseLook.VirCam.Follow = myEntities[e].gameObject.transform.GetChild(1);
                    MouseLook.VirCam.LookAt = myEntities[e].gameObject.transform.GetChild(1);
                    MouseLook.cameras[1].GetComponent<CinemachineVirtualCamera>().Follow = myEntities[e].gameObject.transform.GetChild(2);
                    MouseLook.cameras[1].GetComponent<CinemachineVirtualCamera>().LookAt = myEntities[e].gameObject.transform.GetChild(2);
                }
            }
        }
        camtargetSet = true;
    }
}
