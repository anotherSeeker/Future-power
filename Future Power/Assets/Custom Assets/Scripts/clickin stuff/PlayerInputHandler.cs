using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")] 
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")] 
    [SerializeField] private string click = "Click";
    [SerializeField] private string position = "Position";
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";

    private InputAction clickAction;
    private InputAction positionAction;
    private InputAction moveAction;
    private InputAction lookAction;


    public enum clickValues
    {
        None,
        Started,
        Held
    }

    public Vector2 moveInput {get; private set;}
    public Vector2 lookInput {get; private set;}
    public clickValues clickInput {get; private set;}
    
    


    public static PlayerInputHandler Instance {get; private set;}

    private void OnEnable()
    {   
        clickAction.Enable(); 
        positionAction.Enable();
        moveAction.Enable();
        lookAction.Enable();
    }

    private void OnDisable()
    {   
        clickAction.Disable(); 
        positionAction.Disable();
        moveAction.Disable();
        lookAction.Disable();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        clickAction     = playerControls.FindActionMap(actionMapName).FindAction(click);
        positionAction  = playerControls.FindActionMap(actionMapName).FindAction(position);
        moveAction      = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction      = playerControls.FindActionMap(actionMapName).FindAction(look); 

        clickAction.Enable(); 
        positionAction.Enable();
        moveAction.Enable();
        lookAction.Enable();
    }

    private void Update()
    {
        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        moveAction.performed    += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled     += context => moveInput = Vector2.zero;

        lookAction.performed    += context => lookInput = context.ReadValue<Vector2>();
        lookAction.canceled     += context => lookInput = Vector2.zero;

        clickAction.performed   += context => clickInput = clickValues.Held;
        clickAction.started     += context => clickInput = clickValues.Started;
        clickAction.canceled    += context => clickInput = clickValues.None;
    }
}
