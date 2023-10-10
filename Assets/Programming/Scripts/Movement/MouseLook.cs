using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Unity.Netcode;

public class MouseLook : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField] float mouseSensitivity = 500;
    [SerializeField] float maxLookUpDegrees = 60;
    [SerializeField] float maxLookDownDegrees = 80;
    public Transform CamTarget { get; set; }
    [SerializeField] Transform tempCamTarget;
    [SerializeField] CinemachineVirtualCamera virCam;
    public CinemachineVirtualCamera VirCam { get { return virCam; } set { virCam = value; } }


    [Header("In battle")]
    public bool inBattle;
    [SerializeField] CombatHandler combbatHandler = CombatHandler.instance;

    [Header("target Selecting")]
    bool zoomedIn;
    public GameObject[] cameras;

    public float yRotation { get; private set; }
    float xRotation;

    bool isLocked;
    // Start is called before the first frame update
    void Start()
    {
        // if (!IsOwner) { this.enabled = false; }
        Cursor.lockState = CursorLockMode.Locked;
        if (CamTarget == null)
            CamTarget = tempCamTarget;
    }
    // Update is called once per frame
    void Update()
    {
        isLocked = Cursor.lockState == CursorLockMode.Locked;
        #region Mouse movement

        if (isLocked)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -maxLookUpDegrees, maxLookDownDegrees);
            if (CamTarget == null)
            {
                Debug.LogError("CamTarget Not Assigned");
            }
            else
            {
                CamTarget.rotation = Quaternion.Euler(xRotation, yRotation, 0);
                ChangeBodyRotationServerRPC(xRotation, yRotation);
            }
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
        if (inBattle)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (!zoomedIn)
                {
                    ActivateCamera(1);
                    zoomedIn = true;
                    Debug.Log("zooming");
                }
                if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1))
                {
                    if (Physics.Raycast(cameras[1].transform.position, cameras[1].transform.forward, out RaycastHit target, Mathf.Infinity))
                    {
                        Debug.DrawRay(cameras[1].transform.position, target.point, Color.blue, 100f);
                        Debug.Log("Hit: " + target.transform.name);
                        target.transform.TryGetComponent(out Outline outline);
                        outline.enabled = true;
                        combbatHandler.CheckAttack(this.gameObject, target.transform.gameObject, 0, 5);
                    }
                    Debug.Log("hit nothing");
                }
                
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                if (zoomedIn)
                {
                    //Debug.Log("triggering toggle");
                    ActivateCamera(0);
                    zoomedIn = false;
                }
            }
        }

    }
    [ServerRpc(RequireOwnership = false)]
    public void ChangeBodyRotationServerRPC(float xRotation, float yRotation, ServerRpcParams serverRpcParams = default)
    {
        CamTarget.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void ActivateCamera(int index)
    {
        //Debug.Log("toggling Camera's");
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i == index)
            {
                cameras[i].SetActive(true);
                //Debug.Log("enabled camera " + cameras[i].name);
            }
            else
            {
                cameras[i].SetActive(false);
                //Debug.Log("disabled camera " + cameras[i].name);
            }
        }
    }

    public void PositionCameraForObjects(List<GameObject> objects)
    {
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
        CamTarget.transform.position = cameraPosition;
    }

    public void PosistionCameraForCombat()
    {
        CamTarget.transform.localPosition = new Vector3(0, 0.56f, 0);
    }
}