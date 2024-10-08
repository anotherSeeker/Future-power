using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class basicTestCam : MonoBehaviour 
{
    [Header("Speed")]
    [SerializeField] private float flySpeed = 5.0f;

    [Header("Look Sens")]
    [SerializeField] private float sensitivity = 0.2f;
    [SerializeField] private float upDownRange = 80.0f;

    private Vector3 currentMovementVector = new Vector3(0,0,0);
    private float vertRotation;

    private GameObject interactTarget;

    private Camera mainCamera;
    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        //mainCamera = Camera.main;
        inputHandler = PlayerInputHandler.Instance;
    }

    void Update()
    {
        HandleMovement();
        PlayerInputHandler.clickValues clickState = ClickState();
        if (clickState != PlayerInputHandler.clickValues.None)
        {
            Debug.Log("AAAAA");
            FireScreenRay();
        }

        HandleRotation();
    }

    private PlayerInputHandler.clickValues ClickState()
    {
        PlayerInputHandler.clickValues value = inputHandler.clickInput;
        return value;
    }

    void FireScreenRay()
    {
        LayerMask mask = LayerMask.GetMask();

        Ray cameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        bool hitSomething = Physics.Raycast(cameraRay, out RaycastHit hitInfo, 10);

        if (hitSomething)
        {
            Debug.DrawRay(cameraRay.origin, cameraRay.direction*10, Color.green, 2, false);
            Debug.Log(hitInfo.collider.gameObject.name);
        }
        else
        {
             Debug.DrawRay(cameraRay.origin, cameraRay.direction*10, Color.red, 2, false);
        }
    }

    private void HandleMovement()
    {
        Vector3 inputDirection = new Vector3(inputHandler.moveInput.x, 0f, inputHandler.moveInput.y);

        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        worldDirection.Normalize();

        currentMovementVector.x = worldDirection.x * flySpeed * Time.deltaTime;
        currentMovementVector.z = worldDirection.z * flySpeed * Time.deltaTime;

        transform.position += currentMovementVector;
    }

    private void HandleRotation()
    {
        //we don't actually want this behaviour
        /*float horizRotation = inputHandler.lookInput.x * sensitivity;
        transform.Rotate(0,horizRotation,0);

        vertRotation -= inputHandler.lookInput.y * sensitivity;
        vertRotation = Mathf.Clamp(vertRotation, -upDownRange, upDownRange);
        Quaternion.Euler(vertRotation, 0, 0);

        transform.Rotate(0,0,0);*/
    }
}   