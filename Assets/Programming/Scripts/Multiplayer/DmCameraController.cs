using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmCameraController : MonoBehaviour
{
    public MouseLook mouseLook;
    private float moveSpeed = 20.0f; // Adjust the speed of camera movement
    private void Start()
    {
        //mouseLook = this.gameObject.GetComponent<MouseLook>();
        //check if player Controlling Camera is the DM
        //if its not DM disable this script
        //this.gameObject.SetActive(false);
    }
    private void Update()
    {
        // Get input for camera movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate new position
        Vector3 newPosition = transform.position + new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime;

        // Set the Y position to a constant level
        newPosition.y = transform.position.y;

        // Move the camera to the new position
        transform.position = newPosition;

        if (Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out RaycastHit target, Mathf.Infinity))
        {
            if (target.transform.TryGetComponent(out EntityClass entity))
            {
                this.transform.position = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
                //maybe pull up new UI to view entity info
            }
        }
    }
}
