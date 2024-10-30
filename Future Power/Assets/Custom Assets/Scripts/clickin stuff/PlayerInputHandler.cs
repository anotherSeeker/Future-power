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
    [SerializeField] private string dialSpeed = "DialSpeed";
    [SerializeField] private string click = "Click";

    //Move and look were for debug purposes
    [SerializeField] private string move = "Move";
    
    [SerializeField] private string look = "Look";

    public InputAction clickAction;
    public InputAction dialSpeedAction;

    //Move and look were for debug purposes
    public InputAction moveAction;
    public InputAction lookAction;

    public Vector2 moveInput {get; private set;}
    public Vector2 lookInput {get; private set;}

    public int DialSpeedInput {get; private set;}

    public static PlayerInputHandler Instance {get; private set;}

    private void OnEnable()
    {   
        clickAction.Enable(); 
        dialSpeedAction.Enable();
        //moveAction.Enable();
        //lookAction.Enable();
    }

    private void OnDisable()
    {   
        clickAction.Disable(); 
        dialSpeedAction.Disable();  
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

        clickAction = playerControls.FindActionMap(actionMapName).FindAction(click);
        moveAction  = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction  = playerControls.FindActionMap(actionMapName).FindAction(look); 
        dialSpeedAction = playerControls.FindActionMap(actionMapName).FindAction(dialSpeed);

        clickAction.Enable();
        dialSpeedAction.Enable(); 
        moveAction.Enable();
        lookAction.Enable();
    }

    private void Update()
    {
        RegisterMoveAndLookActions();
    }

    void RegisterMoveAndLookActions()
    {
        moveAction.performed    += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled     += context => moveInput = Vector2.zero;

        lookAction.performed    += context => lookInput = context.ReadValue<Vector2>();
        lookAction.canceled     += context => lookInput = Vector2.zero;
    }
}
