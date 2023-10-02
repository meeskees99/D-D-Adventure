using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform camTarget;
    [SerializeField] Transform camPos;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float maxLookUpDegrees;
    [SerializeField] float defaultCameraView;

    [Header("In battle")]
    [SerializeField] GameObject virCam;
    [SerializeField] Transform battleCamPos;
    [SerializeField] Transform battleCamTarget;
    public bool inBattle;

    float yRotation;
    public float GetYRotation
    {
        get
        {
            return yRotation;
        }
        set
        {
            yRotation = value;
        }
    }
    float xRotation;

    bool isLocked;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        isLocked = Cursor.lockState == CursorLockMode.Locked;
        #region Mouse movement

        if (inBattle)
        {
            virCam.GetComponent<CinemachineVirtualCamera>().transform.position = battleCamPos.position;
            virCam.GetComponent<CinemachineVirtualCamera>().LookAt = battleCamTarget;
        }
        else
        {
            virCam.GetComponent<CinemachineVirtualCamera>().transform.position = camPos.position;
            virCam.GetComponent<CinemachineVirtualCamera>().LookAt = camTarget;
        }
        if (isLocked)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -maxLookUpDegrees, 90f);

            camTarget.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
        else
        {
            //idk
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isLocked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void PositionCameraForObjects(List<GameObject> objects)
    {
        // Check if there are any objects in the list
        if (objects.Count == 0)
        {
            Debug.LogWarning("No objects in the list.");
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
        float cameraSize = Mathf.Max(bounds.size.x, bounds.size.z) / 2f;

        // Set the camera position and orthographic size
        camTarget.transform.position = cameraPosition;
        Camera.main.orthographicSize = cameraSize;
    }

    public void PosistionCameraForCombat()
    {
        camTarget.transform.localPosition = new Vector3(0, 0.56f, 0);
        Camera.main.orthographicSize = 10;
    }
}
