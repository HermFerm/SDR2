using UnityEngine;
using UnityEngine.InputSystem;



public class InputManager : MonoBehaviour
{
    P1Inputs p1Inputs;
    P1Locomotion p1Locomotion;
    P1AttackManager p1AttackManager;

    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;

    public bool spaceInput;
    public bool sliceAttackInput;

    private void OnEnable()
    {
        if (p1Inputs == null)
        {
            p1Inputs = new P1Inputs();

            p1Inputs.Actions.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            p1Inputs.Actions.Movement.canceled += i => movementInput = Vector2.zero;

            p1Inputs.Actions.Space.performed += i => spaceInput = true;
            p1Inputs.Actions.SliceAttack.performed += i => sliceAttackInput = true;
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
        p1AttackManager = GetComponent<P1AttackManager>();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleJumpingInput();
        HandleAttackInputs();
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

    void HandleAttackInputs()
    {
        if (sliceAttackInput)
        {
            sliceAttackInput = false;
            p1AttackManager.HandleSlice();
        }
    }

}
