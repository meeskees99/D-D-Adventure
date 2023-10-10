using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class ForceMovement : NetworkBehaviour
{
    [Header("Movement")]
    float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] GameObject playerModel;
    bool isMoving;
    [Tooltip("Maximum distance this character can travel in one turn")]
    [SerializeField] float moveDistance;
    [SerializeField] float distanceMovedThisTurn;
    [Tooltip("Multiply Distance Moved This Turn by this value")]
    [SerializeField] float moveMultiplier = 3;
    [SerializeField] float rotationSpeed = 7;

    [SerializeField] Slider movementSlider;

    [Header("Turn Based Movement")]
    [SerializeField] bool canMove;
    public bool isFighting { get; set; }
    public bool isTurn { get; set; }
    public float distanceToGo { get; private set; }


    [Header("Jumping")]
    [Tooltip("How high the player will jump")]
    [SerializeField] float jumpForce = 4f;
    [Tooltip("The time a player has to wait before being able to jump again")]
    [SerializeField] float jumpCooldown = 0.25f;
    [Tooltip("Speed will multiply by this whilst airborn")]
    [SerializeField] float airMultiplier = 0.4f;
    bool readyToJump;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheckPos;
    [SerializeField] LayerMask whatIsGround = 6;
    [SerializeField] bool overrideGrounded;
    [SerializeField] bool grounded;
    [SerializeField] float groundCheckRayLenght = 0.4f;

    [Header("Slope Check")]
    [SerializeField] float maxSlopeAngle = 50f;
    [SerializeField] float minSlopeAngle = 2f;
    RaycastHit slopeHit;
    [SerializeField] Transform shootPos;

    [Header("")]
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
        WALK, RUN, AIR
    }

    void Start()
    {
        if (!IsOwner)
        {
            this.enabled = false;
        }
        rb = GetComponent<Rigidbody>();
        movementSlider = FindObjectOfType<Slider>();
        readyToJump = true;
        if (groundCheckPos == null)
        {
            groundCheckPos = transform;
        }
        raycastLenght = 0.51f;
    }

    void Update()
    {
        if (speedTxt != null)
        {
            speedTxt.text = "Speed: " + rb.velocity.magnitude.ToString("0");
        }
        // Ground Check
        RaycastHit groundRay;
        Physics.Raycast(groundCheckPos.position, Vector3.down, out groundRay, groundCheckRayLenght, whatIsGround);
        if (!overrideGrounded)
            grounded = Physics.Raycast(groundCheckPos.position, Vector3.down, groundCheckRayLenght, whatIsGround);
        Debug.DrawRay(groundCheckPos.position, groundRay.point, Color.green);

        isMoving = rb.velocity.magnitude > 0.1;

        if (canMove)
            MyInput();

        if (!isFighting)
            canMove = true;
        SpeedControlServerRpc(moveSpeed);
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
                if (movementSlider != null)
                {
                    movementSlider.transform.gameObject.SetActive(true);
                    movementSlider.maxValue = moveDistance;
                    movementSlider.value = distanceToGo;
                }

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
                if (movementSlider != null)
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
            state = MovementState.RUN;
            moveSpeed = sprintSpeed;
        }
        //Mode - Walking
        else if (grounded)
        {
            state = MovementState.WALK;
            moveSpeed = walkSpeed;
        }
        // Mode - Air
        else
        {
            state = MovementState.AIR;
        }
    }

    void MovePlayer()
    {
        // Calculalte move direction
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDir.y = 0;
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
            MovePlayerServerRPC(true, moveSpeed, moveDir, airMultiplier, wallWalk);
        // rb.AddForce(moveDir.normalized * moveSpeed * 10, ForceMode.Force);
        //In air
        else if (!grounded)
            MovePlayerServerRPC(false, moveSpeed, moveDir, airMultiplier, wallWalk);
        // rb.AddForce(moveDir.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Force);

        // Turn off gravity while on slope
        rb.useGravity = !OnSlope();
    }

    [ServerRpc(RequireOwnership = false)]
    public void MovePlayerServerRPC(bool ground, float moveSpeed, Vector3 moveDir, float airMultiplier, bool wallWalk, ServerRpcParams serverRpcParams = default)
    {
        if (wallWalk)
        {
            rb.AddForce(Vector3.down * 10f, ForceMode.Force);
            return;
        }
        // // On slope
        // if (OnSlope())
        // {
        //     rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);

        //     if (rb.velocity.y > 0)
        //     {
        //         rb.AddForce(Vector3.down * 40f, ForceMode.Force);
        //     }
        // }
        else if (ground)
            rb.AddForce(moveDir.normalized * moveSpeed * 10, ForceMode.Force);
        else if (!ground)
            rb.AddForce(moveDir.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Force);

        // rb.useGravity = !OnSlope();

    }
    [ServerRpc(RequireOwnership = false)]
    public void SpeedControlServerRpc(float moveSpeed, ServerRpcParams serverRpcParams = default)
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
    // [ServerRpc(RequireOwnership = false)]
    public void Jump()
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
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, groundCheckRayLenght))
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

    public void StartCombat()
    {
        isFighting = true;
        canMove = false;
    }

    public void EndCombat()
    {
        isFighting = false;
        canMove = true;
        isTurn = false;
    }

}
