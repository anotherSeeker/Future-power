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

    public static Boolean isSetup = false;

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
        //moveAction.Disable();
        //lookAction.Disable();
    }


    //action setup from action map, uses instances to check if we're reloading the scene and if we are don't setup again
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            clickAction = playerControls.FindActionMap(actionMapName).FindAction(click);
            moveAction  = playerControls.FindActionMap(actionMapName).FindAction(move);
            lookAction  = playerControls.FindActionMap(actionMapName).FindAction(look); 
            dialSpeedAction = playerControls.FindActionMap(actionMapName).FindAction(dialSpeed);

            clickAction.Enable();
            dialSpeedAction.Enable(); 
            moveAction.Enable();
            lookAction.Enable();
        }
        else
        {
            isSetup = true;
            Destroy(gameObject);
        }

        
    }
}
