using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmCameraController : MonoBehaviour
{
    public MouseLook mouseLook;
    private void Start()
    {
        mouseLook = this.gameObject.GetComponent<MouseLook>();
        //check if player Controlling Camera is the DM
        //if its not DM disable this script
        //this.gameObject.SetActive(false);
    }
    private void Update()
    {
        //else make DM able to move camera with WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        this.transform.Translate(new Vector3(horizontal, this.transform.position.y , vertical));
        
        if(Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out RaycastHit target, Mathf.Infinity))
        {
            if(target.transform.TryGetComponent(out EntityClass entity))
            {
                this.transform.position = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
                //maybe pull up new UI to view entity info
            }
        }
    }
}
