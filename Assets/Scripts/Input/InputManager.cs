using UnityEngine;

public class InputManager : MonoBehaviour
{
    P1Inputs p1Inputs;
    P1Locomotion p1Locomotion;

    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;

    public bool spaceInput;

    private void OnEnable()
    {
        if (p1Inputs == null)
        {
            p1Inputs = new P1Inputs();

            p1Inputs.Actions.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            p1Inputs.Actions.Space.performed += i => spaceInput = true;
        }

        p1Inputs.Enable();
    }

    private void OnDisable()
    {
        p1Inputs.Disable();
    }

    private void Awake()
    {
        p1Locomotion = GetComponent<P1Locomotion>();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleJumpingInput();
    }

    void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }

    void HandleJumpingInput()
    {
        if (spaceInput)
        {
            spaceInput = false;
            p1Locomotion.HandleJumping();
        }
    }
}
