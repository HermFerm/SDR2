using UnityEngine;

public class InputManager : MonoBehaviour
{
    P1Inputs p1Inputs;

    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;

    private void OnEnable()
    {
        if (p1Inputs == null)
        {
            p1Inputs = new P1Inputs();

            p1Inputs.Actions.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }

        p1Inputs.Enable();
    }

    private void OnDisable()
    {
        p1Inputs.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }
}
