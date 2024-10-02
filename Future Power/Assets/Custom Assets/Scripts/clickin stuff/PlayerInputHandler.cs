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
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";

    private InputAction clickAction;
    private InputAction moveAction;
    private InputAction lookAction;
    
    public Vector2 moveInput {get; private set;}
    public Vector2 lookInput {get; private set;}
    public bool clickInput {get; private set;}

    public static PlayerInputHandler Instance {get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        clickAction = playerControls.FindActionMap(actionMapName).FindAction(click);
        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
    }

    void RegisterInputActions()
    {
        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;

        lookAction.performed += context => lookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => lookInput = Vector2.zero;

        clickAction.performed += context => clickInput = true;
        clickAction.canceled += context => clickInput = false;
    }

    private void OnEnable()
    {   
        clickAction.Enable(); 
        moveAction.Enable();
        lookAction.Enable();
    }

     private void OnDisable()
    {   
        clickAction.Disable(); 
        moveAction.Disable();
        lookAction.Disable();
    }
}
