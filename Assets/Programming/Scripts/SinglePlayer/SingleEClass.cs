using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleEClass : MonoBehaviour
{
    public bool isTurn;
    public ClassSheet classSheet;
    public WeaponObject currentWeapon;
    public WeaponObject[] weaponOptions;

    public int initiative;
    public int maxHitPoints;
    public int hitPoints;
    // Start is called before the first frame update
    public float movementSpeed = 5f;
    public float mouseSensitivity = 2f;

    private CharacterController characterController;
    public Camera playerCamera;
    private float verticalRotation = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        if (isTurn && Cursor.lockState == CursorLockMode.Locked)
        {
            // Player Movement
            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");

            Vector3 movement = transform.forward * verticalMovement + transform.right * horizontalMovement;
            characterController.SimpleMove(movement * movementSpeed);

            // Player Rotation
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Debug.Log(name + " has died!");
            hitPoints = 0;

            // Die();
        }
    }
    public void Heal(int amount)
    {
        hitPoints += amount;
        if (hitPoints > maxHitPoints)
        {
            hitPoints = maxHitPoints;
        }
    }
}
