using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")] 
    [SerializeField] private String actionMapName = "Player";

    [Header("Action Name References")] 
    [SerializeField] private String move = "Move";
    [SerializeField] private String look = "Look";
    [SerializeField] private String flyUp = "Up";
    [SerializeField] private String flyDown = "Down";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction flyUpAction;
    private InputAction flyDownAction;
    
    public Vector2 MoveInput {get; private set;}
    public Vector2 InputLook {get; private set;}
    public bool inputFlyUp {get; private set;}
    public bool inputFlyDown {get; private set;}

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

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
    }
}
