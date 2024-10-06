using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class basicTestCam : MonoBehaviour 
{
    [Header("Speed")]
    [SerializeField] private float flySpeed = 5.0f;

    [Header("Look Sens")]
    [SerializeField] private float sensitivity = 2.0f;
    [SerializeField] private float upDownRange = 80.0f;

    private Vector3 currentMovementVector = new Vector3(0,0,0);
    private float vertRotation;

    private Camera mainCamera;
    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        mainCamera = Camera.main;
        inputHandler = PlayerInputHandler.Instance;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void FireScreenRay()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo))
        {
           Debug.Log(hitInfo.collider.gameObject.name);
        }
    }

    private void HandleMovement()
    {
        Vector3 inputDirection = new Vector3(inputHandler.moveInput.x, 0f, inputHandler.moveInput.y);

        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        worldDirection.Normalize();

        currentMovementVector.x = worldDirection.x * flySpeed;
        currentMovementVector.z = worldDirection.z * flySpeed;

        transform.position += currentMovementVector;
    }

    private void HandleRotation()
    {
        float horizRotation = inputHandler.lookInput.x * sensitivity;
        transform.Rotate(0,horizRotation,0);

        vertRotation -= inputHandler.lookInput.y * sensitivity;
        vertRotation = Mathf.Clamp(vertRotation, -upDownRange, upDownRange);
        Quaternion.Euler(vertRotation, 0, 0);

        transform.Rotate(0,0,0);
    }
}

        //transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        //transform.position += Vector3.up * Input.GetAxis("Jump") * speed * Time.deltaTime;       