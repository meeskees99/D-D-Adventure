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
    [SerializeField] GameObject cinemachineCam;
    [SerializeField] GameObject cinemachineZoomCam;

    LayerMask UILayer;
    [Header("Zoom Settings")]
    [SerializeField] float scrollSpeed;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [Header("Move Settings")]
    [SerializeField] float mouseSensitivity;
    [SerializeField] float moveSpeed = 20.0f;
    [SerializeField] float wallCheckRaycastLenght = 5;

    float _horizontalInput;
    float _verticalInput;
    float _scroll;

    float _rotation;


    Vector3 _moveDir;


    private void Start()
    {
        if (IsHost)
        {
            cinemachineCam.SetActive(false);
            cinemachineZoomCam.SetActive(false);
            GetComponent<CinemachineBrain>().enabled = false;
            transform.rotation = Quaternion.Euler(new Vector3(50f, 0f, 0f));
            UILayer = LayerMask.NameToLayer("UI");

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

        _rotation = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        bool multipleKeys = Input.GetKey("w") && Input.GetKey("d") || Input.GetKey("w") && Input.GetKey("a") || Input.GetKey("s") && Input.GetKey("d") || Input.GetKey("s") && Input.GetKey("a");
        float finalMoveSpeed = multipleKeys ? moveSpeed : moveSpeed / 0.75f;

        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * finalMoveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * finalMoveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * finalMoveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * finalMoveSpeed * Time.deltaTime, Space.World);
        }
    }

    void MoveCamera()
    {


        Vector3 pos = transform.position;

        pos.y -= _scroll * 100 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        // pos.x = Mathf.Clamp(pos.x, minX, maxX);
        // pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        // transform.Translate(_moveDir * moveSpeed * Time.deltaTime, Space.World);

        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
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
