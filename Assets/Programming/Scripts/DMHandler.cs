using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMHandler : MonoBehaviour
{
    Transform camTarget;
    InitiativeHandler initiativeHandler;
    private bool controlNPCMode;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (initiativeHandler.DMTurn)
        {
            if (!controlNPCMode)
            {
                ChangeToDMCameraView(initiativeHandler.characters);
                controlNPCMode = true;
            }
        }
    }
    public void ChangeToDMCameraView(List<GameObject> objects)
    {
        camTarget = initiativeHandler.characters[initiativeHandler.currentTurnNmbr].transform;
        // Check if there are any objects in the list
        if (objects.Count == 0)
        {
            //Debug.LogWarning("No objects in the list.");
            return;
        }

        // Calculate the bounds that encompass all objects in the list
        Bounds bounds = new Bounds(objects[0].transform.position, Vector3.zero);
        foreach (GameObject obj in objects)
        {
            bounds.Encapsulate(obj.transform.position);
        }

        // Calculate the camera position and size
        Vector3 cameraPosition = bounds.center;
        cameraPosition.y = 20; // Keep the camera at the same y-coordinate


        // Set the camera position and orthographic size
        camTarget.transform.position = cameraPosition;
    }

    public void PosistionDMForCombat()
    {
        camTarget.transform.localPosition = new Vector3(0, 0.56f, 0);
    }
}
