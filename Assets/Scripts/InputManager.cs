using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IInputProvider
{
    private InputSystem_Actions playerInputActions;
    private Vector2 movementInput;

    public Vector2 MovementInput => movementInput;
    public bool IsMovementPressed => movementInput.magnitude > 0;

    private void Awake()
    {
        playerInputActions = new InputSystem_Actions();
        
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}