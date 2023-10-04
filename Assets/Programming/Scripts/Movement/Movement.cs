using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    // [Header("Movement Settings")]
    // [SerializeField] float movementSpeed;
    // [Tooltip("Maximum distance this character can travel in one turn")]
    // [SerializeField] float moveDistance;
    // [SerializeField] float distanceMovedThisTurn;
    // [Tooltip("Multiply Distance Moved This Turn by this value")]
    // [SerializeField] float moveMultiplier = 3;
    // [SerializeField] bool isMoving;

    // Vector3 v3;
    // float distanceToGo;
    // float zMovement;
    // float xMovement;

    // [Header("Turn Based Movement")]
    // [SerializeField] bool isFighting;
    // [SerializeField] bool isTurn;

    // [SerializeField] Slider movementSlider;

    // [Header("Body rotation")]
    // [SerializeField] MouseLook mouseLook;
    // [SerializeField] float turningSmoothness;
    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (isFighting)
    //     {
    //         #region Fight movement
    //         if (isTurn)
    //         {
    //             if (v3.z < 0)
    //             {
    //                 zMovement = v3.z * v3.z;
    //             }
    //             else
    //             {
    //                 zMovement = v3.z;
    //             }
    //             if (v3.x < 0)
    //             {
    //                 xMovement = v3.x * v3.x;
    //             }
    //             else
    //             {
    //                 xMovement = v3.x;
    //             }
    //             float moveAmount = zMovement + xMovement;
    //             distanceToGo = moveDistance - distanceMovedThisTurn;
    //             movementSlider.transform.gameObject.SetActive(true);
    //             movementSlider.maxValue = moveDistance;
    //             movementSlider.value = distanceToGo;
    //             distanceMovedThisTurn += moveAmount * moveMultiplier * Time.deltaTime;

    //             if (distanceMovedThisTurn < moveDistance)
    //             {
    //                 Move();
    //             }
    //             else
    //             {
    //                 isTurn = false;
    //                 print("No more walking points available");
    //             }
    //         }
    //         else
    //         {
    //             movementSlider.transform.gameObject.SetActive(false);
    //             distanceMovedThisTurn = 0;
    //         }
    //         #endregion
    //     }
    //     else
    //     {
    //         Move();
    //     }
    //     #region Only rotate body while moving

    //     isMoving = v3 != new Vector3(0, 0, 0);
    //     #endregion

    // }

    // void Move()
    // {
    //     float horizontal = Input.GetAxis("Horizontal");
    //     float vertical = Input.GetAxis("Vertical");

    //     v3.x = horizontal;
    //     v3.z = vertical;

    //     transform.Translate(v3 * movementSpeed * Time.deltaTime);
    //     // transform.rotation = mouseLook.transform.rotation;
    //     if (isMoving)
    //     {
    //         // transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0, mouseLook.GetYRotation, 0), turningSmoothness);
    //         // Quaternion.Euler(0, mouseLook.GetYRotation, 0);
    //     }
    // }
}
