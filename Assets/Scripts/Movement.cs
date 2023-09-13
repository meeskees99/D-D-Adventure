using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    Vector3 v3;

    [SerializeField] float moveDistance;
    [SerializeField] float distanceMovedThisTurn;
    [Tooltip("Multiply Distance Moved This Turn by this value")]
    [SerializeField] float moveMultiplier = 5;

    [SerializeField] bool isTurn;
    [SerializeField] Slider movementSlider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isTurn)
        {
            movementSlider.transform.gameObject.SetActive(true);
            movementSlider.maxValue = moveDistance;
            distanceMovedThisTurn += transform.GetComponent<Rigidbody>().velocity.magnitude * moveMultiplier * Time.deltaTime;

            if (distanceMovedThisTurn < moveDistance)
            {
                Move();
            }
            else
            {
                isTurn = false;
                print("No more walking points available");
            }
        }
        else
        {
            movementSlider.transform.gameObject.SetActive(false);
            distanceMovedThisTurn = 0;
        }

    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        v3.x = horizontal;
        v3.z = vertical;

        transform.Translate(v3 * movementSpeed * Time.deltaTime);
    }
}
