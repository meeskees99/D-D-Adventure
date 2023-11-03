using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Unity.Netcode;
using UnityEditor;

public class MouseLook : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField] float mouseSensitivity = 500;
    [SerializeField] float maxLookUpDegrees = 60;
    [SerializeField] float maxLookDownDegrees = 80;
    [SerializeField] GameObject playerObject;
    public Transform CamTarget { get; set; }
    [SerializeField] Transform tempCamTarget;
    [SerializeField] CinemachineVirtualCamera virCam;
    public CinemachineVirtualCamera VirCam { get { return virCam; } set { virCam = value; } }
    bool isDm;

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
        if (IsHost)
            isDm = true;

        if (isDm) return;

        Cursor.lockState = CursorLockMode.Locked;

        if (CamTarget == null)
            CamTarget = tempCamTarget;
    }
    // Update is called once per frame
    void Update()
    {
        if (isDm) return;
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
                ChangeBodyRotation(xRotation, yRotation);
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
            LayerMask prevLayer = CamTarget.parent.gameObject.layer;
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (!zoomedIn && combbatHandler.currentAttacker != null)
                {
                    ActivateCamera(1);
                    zoomedIn = true;
                    Debug.Log("zooming");
                    
                }
                if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1))
                {
                    if(combbatHandler.currentAttacker != null)
                    {
                        if(CamTarget.parent.gameObject.layer != 2)
                        {
                            prevLayer = CamTarget.parent.gameObject.layer;
                            CamTarget.parent.gameObject.layer = 2;
                        }
                        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit target, Mathf.Infinity))
                        {
                            if (target.transform.gameObject.TryGetComponent(out EntityClass _))
                            {
                                Debug.DrawRay(Camera.main.transform.position, target.point, Color.blue, 5f);
                                Debug.Log("Hit: " + target.transform.name);
                                combbatHandler.SelectTarget(target.transform.gameObject);
                            }
                        }
                        Debug.Log("hit nothing");
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                if (zoomedIn)
                {
                    //Debug.Log("triggering toggle");
                    ActivateCamera(0);
                    zoomedIn = false;
                    CamTarget.parent.gameObject.layer = prevLayer;
                }
            }
        }

    }
    
    public void ChangeBodyRotation(float xRotation, float yRotation)
    {
        CamTarget.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void ActivateCamera(int index)
    {
        if (isDm) return;
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
        if (isDm) return;
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
        if (isDm) return;
        CamTarget.transform.localPosition = new Vector3(0, 0.56f, 0);
    }
}