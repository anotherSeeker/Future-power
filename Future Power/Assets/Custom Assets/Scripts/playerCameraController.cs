using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class playerCameraController : MonoBehaviour 
{
    [SerializeField] private float flySpeed = 0.5f;
    [SerializeField] private float rotSpeed = 45.0f;
    [SerializeField] private float moveTime = 1; 

    private Vector3 currentMovementVector = new Vector3(0,0,0);

    private Vector3 flyVelocity = new Vector3();
    private float rotVelocityX = 0;
    private float rotVelocityY = 0;
    private float rotVelocityZ = 0;
    
    [SerializeField] private Canvas inGameUI;
    [SerializeField] private Canvas tutorialUI;
    
    private bool isGameStartPath = true;
    private bool isTutorialPath = false;
    private  Boolean isTraversingPath = false;
    private int finalTargetPoint = 0;
    private int currentTargetPoint = 0;
    [SerializeField] private bool useDebugFly = false;
    //[SerializeField] private float sensitivity = 0.2f;
    //[SerializeField] private float upDownRange = 80.0f;
    [SerializeField] private float clickHoldDelay = 0.5f;
    [SerializeField] private String clickableLayerName = "Clickable";
    [SerializeField] private String blockingLayerName = "UI";
    [SerializeField] private GameObject gameCameraFlyPoints;
    [SerializeField] private GameObject menuCameraFlyPoints;


    private bool isSlowDial = false;

    private List<Transform> menuPoints = new List<Transform>();
    private List<Transform> gamePoints = new List<Transform>();
    private List<Transform> tutorialPoints = new List<Transform>();

    private bool clickHeld = false;
    private float clickHeldStartTime = 0;
    private Vector2 initialClickLocation;

    
    private GameObject interactTarget;
    private Camera mainCamera;
    private PlayerInputHandler inputHandler;
    private static TMP_Dropdown activeDropdown = null;

    //used to track if we should register our input callbacks
    private Boolean shouldReregister = false;

    private void Start()
    {
        //sets up actions and postions the camera in the starting location
        mainCamera = Camera.main;

        inputHandler = PlayerInputHandler.Instance;
        SetupClickActions();

        defaultPosition();
    }

    void OnEnable()
    {
        //setsup actions after being disabled
        if (shouldReregister)
        {
            SetupClickActions();
            shouldReregister = false;
        }
    }

    void OnDisable()
    {
        unregisterClickActions();
        shouldReregister = true;
    }

    void Update()
    {
        /*if (isGameStartPath)
        {
            //inGameUI.enabled = false;
        }
        else
        {
            //inGameUI.enabled = true;       
        }*/
        
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
            updateCameraPos();
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
            Debug.Log(hitInfo.collider.gameObject.name);
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
        //reloading the scene results in these actions being added as a callback on click twice, we only want these happening the once when clicked    
        inputHandler.clickAction.started    += OnClick;
        inputHandler.clickAction.canceled   += OnCancelClick;

        inputHandler.dialSpeedAction.performed    += context => isSlowDial = true;
        inputHandler.dialSpeedAction.canceled     += context => isSlowDial = false;
    }

    private void unregisterClickActions()
    {
        //reloading the scene results in these actions being added as a callback on click twice, we only want these happening the once when clicked    
        inputHandler.clickAction.started    -= OnClick;
        inputHandler.clickAction.canceled   -= OnCancelClick;

        inputHandler.dialSpeedAction.performed    -= context => isSlowDial = true;
        inputHandler.dialSpeedAction.canceled     -= context => isSlowDial = false;
    }


    private void OnClick(InputAction.CallbackContext context)
    {
        clickHeld = true;
        clickHeldStartTime = Time.time;
        initialClickLocation = new Vector2(Mouse.current.position.ReadValue().x,Mouse.current.position.ReadValue().y );
        Debug.Log("started");
        //tests if we're hovering the ui and if
        if (IsPointerBlocked())
        {
            Debug.Log("Pointer is blocked");
            return;
        }
        if (RaycastFindClickable())
        {
            {
                
                if (interactTarget.GetComponent<GeneratorDial>())
                {
                    interactTarget.GetComponent<GeneratorDial>().selected();
                }
                if (interactTarget.GetComponent<GeneratorNode3d>())
                {
                    activeDropdown = interactTarget.GetComponent<GeneratorNode3d>().onClick(activeDropdown);
                }
                if (interactTarget.GetComponent<ConsumerNode>())
                {
                    interactTarget.GetComponent<ConsumerNode>().onClick();
                }
                if (interactTarget.GetComponent<resetButton>())
                {
                    interactTarget.GetComponent<resetButton>().onClick();
                }
                return;
            }
        }
    }
    
    private void OnCancelClick(InputAction.CallbackContext context)
    {
        if (interactTarget)
        {
            if (interactTarget.GetComponent<GeneratorDial>())
            {
                interactTarget.GetComponent<GeneratorDial>().deselected();
            }
        }

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

                        interactTarget.GetComponent<GeneratorDial>().SpinDial(isSlowDial, distance);
                    }
                }
            }
        }
    }

    public void updateTargetPoint()
    {
        if (currentTargetPoint < finalTargetPoint)
            currentTargetPoint++;
        else if (currentTargetPoint > finalTargetPoint)
            currentTargetPoint--;
    }

    public void startGamePointsMove()
    {
        isTraversingPath = true;

        if (finalTargetPoint == 0)
        {
            //arrays start at 0 count starts at 1
            if (isGameStartPath)
                finalTargetPoint = menuPoints.Count-1;
            else
                finalTargetPoint = gamePoints.Count-1;
        }
        else
        {
            finalTargetPoint = 0;
        }
    }

    public void startTutorialMove()
    {
        isTraversingPath = true;
        isTutorialPath = true;

        if (finalTargetPoint == 0)
        {
            //arrays start at 0 count starts at 1
            if (isGameStartPath)
                finalTargetPoint = menuPoints.Count-1;
            else
                finalTargetPoint = gamePoints.Count-1;
        }
        else
        {
            finalTargetPoint = 0;
        }
    }

    public void updateCameraPos()
    {
        //if we are using the initial move to start the game or using a mid game move
        if (isGameStartPath && isTraversingPath)
        {
            //if we're at the target point stop, if we've hit one point along the path, we can continue
            if ((transform.position - menuPoints[currentTargetPoint].position).magnitude < 0.05)
            {
                //if we're on our final target point and our rotation is done, return, else we start moving towards the next point
                if (currentTargetPoint == finalTargetPoint && (transform.rotation.eulerAngles - menuPoints[currentTargetPoint].rotation.eulerAngles).magnitude < 0.05)
                {
                    transform.position = menuPoints[currentTargetPoint].position;
                    transform.rotation = menuPoints[currentTargetPoint].rotation;

                    isTraversingPath = false;
                    isGameStartPath = false;

                    //reset this so that from now on we use GameFlyPoints instead of menuPoints
                    //also enable the in game ui
                    currentTargetPoint = 0;
                    finalTargetPoint = 0;
                    //we have other ui elements for the tutorial
                    if (!isTutorialPath)
                        inGameUI.gameObject.SetActive(true);
                    else
                        tutorialUI.gameObject.SetActive(true);
                    return;
                }
                else
                    updateTargetPoint();
            }

            doMove(menuPoints);
        }
        else if (isTraversingPath)
        {
            //if we're at the target point stop, if we've hit one point along the path, we can continue
            if ((transform.position - gamePoints[currentTargetPoint].position).magnitude < 0.05)
            {
                //if we're on our final target point and our rotation is done, return, else we start moving towards the next point
                //if we're on our final target point and our rotation is done, return, else we start moving towards the next point
                if (currentTargetPoint == finalTargetPoint && (transform.rotation.eulerAngles - gamePoints[currentTargetPoint].rotation.eulerAngles).magnitude < 0.05)
                {
                    transform.position = gamePoints[currentTargetPoint].position;
                    transform.rotation = gamePoints[currentTargetPoint].rotation;

                    isTraversingPath = false;
                    return;
                }
                else
                    updateTargetPoint();
            }

            doMove(gamePoints);
        }
    }

    private void doMove(List<Transform> points)
    {
        float usedMoveTime = moveTime;
        if (isGameStartPath)
                usedMoveTime = moveTime*1.5f;
        else if (currentTargetPoint != finalTargetPoint)
                usedMoveTime = moveTime/2;

        //move from current position towards gamePoints[currentTargetPoint].position governed by flyspeed and deltatime so we're frame independent
        transform.position = Vector3.SmoothDamp(transform.position, points[currentTargetPoint].position, ref flyVelocity, usedMoveTime, rotSpeed);
        
        //simple rotate to face target position
        float x = Mathf.SmoothDampAngle(transform.eulerAngles.x, points[currentTargetPoint].eulerAngles.x, ref rotVelocityX, usedMoveTime, rotSpeed);
        float y = Mathf.SmoothDampAngle(transform.eulerAngles.y, points[currentTargetPoint].eulerAngles.y, ref rotVelocityY, usedMoveTime, rotSpeed);
        float z = Mathf.SmoothDampAngle(transform.eulerAngles.z, points[currentTargetPoint].eulerAngles.z, ref rotVelocityZ, usedMoveTime, rotSpeed);
        
        transform.rotation = Quaternion.Euler(x, y, z);
    }

    public void defaultPosition()
    {
        for (int i=0; i<menuCameraFlyPoints.transform.childCount; i++)
        {
            menuPoints.Add(menuCameraFlyPoints.transform.GetChild(i));
        }
        for (int i=0; i<gameCameraFlyPoints.transform.childCount; i++)
        {
            gamePoints.Add(gameCameraFlyPoints.transform.GetChild(i));
        }

        transform.position = menuPoints[0].position;
        transform.rotation = menuPoints[0].rotation;
    }








    public bool IsPointerBlocked()
    {
        return IsPointerBlocked(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerBlocked(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer(blockingLayerName))
                return true;
        }
        return false;
    }

     static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}   