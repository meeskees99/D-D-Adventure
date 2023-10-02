using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMHandler : MonoBehaviour
{
    InitiativeHandler initiativeHandler;
    private bool controlNPCMode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(initiativeHandler.DMTurn)
        {
            if(!controlNPCMode)
            {
                ChangeDMCameraView();
                controlNPCMode = true;
            }
        }
    }

    private void ChangeDMCameraView()
    {

    }
}
