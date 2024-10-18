using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class basicTestCam : MonoBehaviour 
{
    [SerializeField] private float flySpeed = 5.0f;
    [SerializeField] private bool useDebugFly = false;
    //[SerializeField] private float sensitivity = 0.2f;
    //[SerializeField] private float upDownRange = 80.0f;
    [SerializeField] private float clickHoldDelay = 0.5f;
    [SerializeField] private String clickableLayerName = "Clickable";
    [SerializeField] private GameObject cameraFlyPoints;


    private Boolean clickHeld = false;
    private float clickHeldStartTime = 0;
    private Vector2 initialClickLocation;

    private Vector3 currentMovementVector = new Vector3(0,0,0);
    private GameObject interactTarget;
    private Camera mainCamera;
    private PlayerInputHandler inputHandler;
    private static TMP_Dropdown activeDropdown = null;



    private void Start()
    {
        mainCamera = Camera.main;
        inputHandler = PlayerInputHandler.Instance;
        SetupClickActions();
    }

    void Update()
    {
        HandleMovement();
        updateHoldClick();
    }
    private void HandleMovement()
    {
        if (useDebugFly)
        {
            Vector3 inputDirection = new Vector3(inputHandler.moveInput.x, 0f, inputHandler.moveInput.y);

            Vector3 worldDirection = transform.TransformDirection(inputDirection);
            worldDirection.Normalize();

            currentMovementVector.x = worldDirection.x * flySpeed * Time.deltaTime;
            currentMovementVector.z = worldDirection.z * flySpeed * Time.deltaTime;

            transform.position += currentMovementVector;
        }
        else
        {
            updatePositionLerp();
        }
    }

    private bool RaycastFindClickable()
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

    private void SetupClickActions()
    {        
        inputHandler.clickAction.started    += OnClick;
        inputHandler.clickAction.canceled   += OnCancelClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        clickHeld = true;
        clickHeldStartTime = Time.time;
        initialClickLocation = new Vector2(Mouse.current.position.ReadValue().x,Mouse.current.position.ReadValue().y );
        Debug.Log("started");

        if (RaycastFindClickable())
        {
            //if (activeDropdown == null)
            {
                if (interactTarget.GetComponent<GeneratorNode3d>())
                {
                    activeDropdown = interactTarget.GetComponent<GeneratorNode3d>().onClick(activeDropdown);
                }
                if (interactTarget.GetComponent<ConsumerNode>())
                {
                    interactTarget.GetComponent<ConsumerNode>().onClick();
                }
                return;
            }
        }

        /*if (activeDropdown != null)
        {
            //clicking while a canvas is up will hide the canvas
            GameObject canvas = activeDropdown.transform.parent.gameObject;
            canvas.SetActive(false);

            activeDropdown = null;
        }*/
    }

    private void OnCancelClick(InputAction.CallbackContext context)
    {
        clickHeld = false;
        interactTarget = null;
        Debug.Log("canceled");
    }

    private void updateHoldClick()
    {
        if (clickHeld)
        {
           
            float clickDuration = Time.time-clickHeldStartTime; 
            if (clickDuration > clickHoldDelay)
            {
                if (interactTarget)
                {
                    if (interactTarget.GetComponent<GeneratorDial>())
                    {
                        
                        Vector2 currentLocation = new Vector2(Mouse.current.position.ReadValue().x,Mouse.current.position.ReadValue().y);
                        
                        float distance = Mathf.Abs(currentLocation.x-initialClickLocation.x) + Mathf.Abs(currentLocation.y-initialClickLocation.y);
                        distance = Mathf.Clamp(distance, 0, 500);

                        if (currentLocation.x-initialClickLocation.x < 0)
                            distance *=-1;

                        bool isSlowSpin = false;

                        interactTarget.GetComponent<GeneratorDial>().SpinDial(isSlowSpin, distance);
                    }
                }
            }
        }
    }

    public void updatePositionLerp()
    {

    }
}   