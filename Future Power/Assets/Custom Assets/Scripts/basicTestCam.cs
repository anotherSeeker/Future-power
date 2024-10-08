using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class basicTestCam : MonoBehaviour 
{
    [SerializeField] private float flySpeed = 5.0f;
    [SerializeField] private float sensitivity = 0.2f;
    [SerializeField] private float upDownRange = 80.0f;
    [SerializeField] private float clickHeldDelay = 1.0f;
    [SerializeField] private String clickableLayerName = "Clickable";



    private Vector3 currentMovementVector = new Vector3(0,0,0);
    private GameObject interactTarget;
    private Camera mainCamera;
    private PlayerInputHandler inputHandler;

    public enum clickValues
    {
        None,
        Started,
        Held
    }

    private void Awake()
    {
        inputHandler = PlayerInputHandler.Instance;
        SetupClickActions();
    }

    void Update()
    {
        HandleMovement();
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

    bool RaycastFindClickable()
    {
        int raycastLength = 5;
        LayerMask mask = LayerMask.GetMask(clickableLayerName);

        Ray cameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        bool hitSomething = Physics.Raycast(cameraRay, out RaycastHit hitInfo, raycastLength, mask);

        if (hitSomething)
        {
            interactTarget = hitInfo.transform.gameObject;

            Debug.DrawRay(cameraRay.origin, cameraRay.direction*raycastLength, Color.green, 2, false);
            //Debug.Log(hitInfo.collider.gameObject.name);
        }
        else
        {
            //Debug.Log(hitInfo.distance);
            Debug.DrawRay(cameraRay.origin, cameraRay.direction*raycastLength, Color.red, 2, false);
        }

        return hitSomething;
    }


    void SetupClickActions()
    {        
        inputHandler.clickAction.started    += OnClick;
        inputHandler.clickAction.performed  += OnClickHeld;
        inputHandler.clickAction.canceled   += context => interactTarget = null;
    }

    void OnClick(InputAction.CallbackContext context)
    {
        if (RaycastFindClickable())
        {
            if (interactTarget.GetComponent<GeneratorNode3d>())
            {
                interactTarget.GetComponent<GeneratorNode3d>().onClick();
            }
            if (interactTarget.GetComponent<ConsumerNode>())
            {
                interactTarget.GetComponent<ConsumerNode>().onClick();
            }
        }
    }

    void OnClickHeld(InputAction.CallbackContext context)
    {
        if (context.duration > clickHeldDelay) 
            Debug.Log("Held");
    }
}   