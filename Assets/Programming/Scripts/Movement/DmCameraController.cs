using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using UnityEngine.EventSystems;
using Cinemachine;
using Unity.Mathematics;

public class DmCameraController : NetworkBehaviour
{
    public MouseLook mouseLook;
    [SerializeField] CinemachineVirtualCamera virCam;
    public CinemachineVirtualCamera VirCam { get { return virCam; } set { virCam = value; } }
    [SerializeField] GameObject cinemachineCam;
    [SerializeField] GameObject cinemachineZoomCam;
    public Transform camTarget;

    LayerMask UILayer;

    [SerializeField] Transform orientation;
    [Header("Zoom Settings")]
    [SerializeField] float scrollSpeed;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [Header("Move Settings")]
    [SerializeField] float mouseSensitivity;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float wallCheckRaycastLenght = 5;

    float _horizontalInput;
    float _verticalInput;
    float _scroll;

    float _rotation;

    bool _multipleKeys;
    float _finalMoveSpeed;


    Vector3 _moveDir;


    private void Start()
    {
        if (IsHost)
        {
            if (cinemachineCam != null)
                cinemachineCam.SetActive(false);
            if (cinemachineZoomCam != null)
                cinemachineZoomCam.SetActive(false);
            GetComponent<CinemachineBrain>().enabled = false;
            transform.rotation = Quaternion.Euler(new Vector3(50f, 0f, 0f));
            UILayer = LayerMask.NameToLayer("UI");
            camTarget = Camera.main.transform;
        }
        else
        {
            this.enabled = false;
        }

    }
    private void Update()
    {
        // print(IsPointerOverUIElement() ? "Over UI" : "Not over UI");
        MyInput();
        _moveDir = transform.forward * _horizontalInput + transform.right * _verticalInput;
        _moveDir.y = 0;
        if (Physics.Raycast(transform.position, _moveDir, out RaycastHit hit, wallCheckRaycastLenght))
        {
            print("You cant hit a wall!");
            return;
        }
        else
        {
            MoveCamera();
        }
        // float horizontalInput = Input.GetAxis("Horizontal");
        // float verticalInput = Input.GetAxis("Vertical");

        // Vector3 newPosition = transform.position + new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime;

        // // Set the Y position to a constant level
        // newPosition.y = transform.position.y;

        // // Move the camera to the new position
        // transform.position = newPosition;

        // if (Physics.Raycast(transform.position, transform.forward, out RaycastHit target, Mathf.Infinity))
        // {
        //     if (target.transform.TryGetComponent(out EntityClass entity))
        //     {
        //         transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        //         //maybe pull up new UI to view entity info
        //     }
        // }
    }

    void MyInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        _scroll = Input.GetAxis("Mouse ScrollWheel");

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        _rotation += mouseX;

        _multipleKeys = Input.GetKey("w") && Input.GetKey("d") || Input.GetKey("w") && Input.GetKey("a") || Input.GetKey("s") && Input.GetKey("d") || Input.GetKey("s") && Input.GetKey("a");
        _finalMoveSpeed = _multipleKeys ? moveSpeed : moveSpeed / 0.75f;


    }

    void MoveCamera()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(orientation.forward * _finalMoveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(-orientation.forward * _finalMoveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(orientation.right * _finalMoveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(-orientation.right * _finalMoveSpeed * Time.deltaTime, Space.World);
        }

        Vector3 pos = transform.position;

        pos.y -= _scroll * 100 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        // pos.x = Mathf.Clamp(pos.x, minX, maxX);
        // pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        // transform.Translate(_moveDir * moveSpeed * Time.deltaTime, Space.World);

        transform.position = pos;
        transform.rotation = Quaternion.Euler(50, _rotation, 0).normalized;
        // orientation.rotation = Quaternion.Euler(-50f, 0f, 0f);

    }
    public void PositionCameraForObjects(List<GameObject> objects)
    {
        GetComponent<CinemachineBrain>().enabled = false;
        GetComponent<MouseLook>().enabled = false;
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

    public void PosistionCameraForCombat()
    {
        GetComponent<CinemachineBrain>().enabled = true;
        var mouseLook = GetComponent<MouseLook>();
        mouseLook.enabled = true;
        mouseLook.ActivateCamera(0);
    }
    #region UI Check
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
    #endregion
}
