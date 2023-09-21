using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForceMovement : MonoBehaviour
{
    [Header("Movement")]
    float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float groundDrag;
    // [SerializeField] MouseLook mouseLook;
    [SerializeField] GameObject playerModel;
    bool isMoving;
    [Tooltip("Maximum distance this character can travel in one turn")]
    [SerializeField] float moveDistance;
    [SerializeField] float distanceMovedThisTurn;
    [Tooltip("Multiply Distance Moved This Turn by this value")]
    [SerializeField] float moveMultiplier = 3;
    [SerializeField] float rotationSpeed = 7;

    [SerializeField] Slider movementSlider;

    public bool isFighting { get; set; }
    public bool isTurn { get; set; }
    [Header("Turn Based Movement")]
    [SerializeField] bool canMove;
    float distanceToGo;


    [Header("Jumping")]
    [Tooltip("How high the player will jump")]
    [SerializeField] float jumpForce;
    [Tooltip("The time a player has to wait before being able to jump again")]
    [SerializeField] float jumpCooldown;
    [Tooltip("Speed will multiply by this whilst airborn")]
    [SerializeField] float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] bool grounded;

    [Header("Slope Check")]
    [SerializeField] float maxSlopeAngle;
    [SerializeField] float minSlopeAngle;
    RaycastHit slopeHit;

    [SerializeField] Transform orientation;
    [SerializeField] TMP_Text speedTxt;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rb;

    [SerializeField] float raycastLenght = 0.51f;

    [SerializeField] bool wallWalk;

    public MovementState state;
    public enum MovementState
    {
        walk, run, air
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        readyToJump = true;
    }

    void Update()
    {
        //speedTxt.text = "Speed: " + rb.velocity.magnitude.ToString("0");
        // Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f, whatIsGround);
        isMoving = rb.velocity.magnitude > 0.1;
        if (canMove)
            MyInput();
        if (!isFighting)
            canMove = true;
        SpeedControl();
        StateHandler();
        // Handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
        if (isFighting)
        {
            #region Fight movement
            if (isTurn)
            {
                float moveAmount = rb.velocity.magnitude;
                distanceToGo = moveDistance - distanceMovedThisTurn;
                movementSlider.transform.gameObject.SetActive(true);
                movementSlider.maxValue = moveDistance;
                movementSlider.value = distanceToGo;
                distanceMovedThisTurn += moveAmount * moveMultiplier * Time.deltaTime;

                if (distanceMovedThisTurn < moveDistance)
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                    print("No more walking points available");
                }
            }
            else
            {
                movementSlider.transform.gameObject.SetActive(false);
                distanceMovedThisTurn = 0;
                canMove = false;
            }
            #endregion
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
            MovePlayer();
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //When To Jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void StateHandler()
    {
        //Mode - Running
        if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.run;
            moveSpeed = sprintSpeed;
        }
        //Mode - Walking
        else if (grounded)
        {
            state = MovementState.walk;
            moveSpeed = walkSpeed;
        }
        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }
    [SerializeField] Transform shootPos;

    void MovePlayer()
    {
        // Calculalte move direction
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Vector3 location = transform.position - moveDir;
        location.y -= transform.position.y;
        Debug.DrawRay(transform.position, location, Color.green);
        if (isMoving)
        {
            moveDir.y = 0f;
            if (moveDir != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(moveDir, Vector3.up);
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }

        RaycastHit hit;
        Debug.DrawRay(shootPos.position, moveDir, Color.red);
        if (Physics.Raycast(shootPos.position, moveDir, out hit, raycastLenght))
        {
            wallWalk = true;
        }
        else
        {
            wallWalk = false;
        }
        if (wallWalk)
        {
            rb.AddForce(Vector3.down * 10f, ForceMode.Force);
            return;
        }
        // On slope
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 40f, ForceMode.Force);
            }
        }


        //On ground
        else if (grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10, ForceMode.Force);
        //In air
        else if (!grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Force);

        // Turn off gravity while on slope
        rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        //Limit speed on slope
        if (OnSlope())
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        //Limit speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Limit the velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

    }

    void Jump()
    {
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void ResetJump()
    {
        readyToJump = true;
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0 && angle > minSlopeAngle;
        }

        return false;
    }

    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }

}
