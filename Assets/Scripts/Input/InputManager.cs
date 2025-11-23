using UnityEngine;
using UnityEngine.InputSystem;



public class InputManager : MonoBehaviour
{
    P1Inputs p1Inputs;
    P1Locomotion p1Locomotion;
    P1Manager p1Manager;
    AnimationManager animationManager;

    [Header("P1")]
    public Vector2 movementInput;
    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool spaceInput;
    public bool sliceAttackInput;
    public bool dashAttackInput;

    [Header("P2")]
    public Vector2 movementInput2;
    private float moveAmount2;
    public float verticalInput2;
    public float horizontalInput2;

    public bool spaceInput2;
    public bool sliceAttackInput2;
    public bool dashAttackInput2;

    private void OnEnable()
    {
        if (p1Inputs == null)
        {
            p1Inputs = new P1Inputs();

            p1Inputs.Actions.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            p1Inputs.Actions.Movement.canceled += i => movementInput = Vector2.zero;

            p1Inputs.Actions.Space.performed += i => spaceInput = true;
            p1Inputs.Actions.SliceAttack.performed += i => sliceAttackInput = true;
            p1Inputs.Actions.DashAttack.performed += i => dashAttackInput = true;

            p1Inputs.Actions2.Movement.performed += i => movementInput2 = i.ReadValue<Vector2>();
            p1Inputs.Actions2.Movement.canceled += i => movementInput2 = Vector2.zero;

            p1Inputs.Actions2.Space.performed += i => spaceInput2 = true;
            p1Inputs.Actions2.SliceAttack.performed += i => sliceAttackInput2 = true;
            p1Inputs.Actions2.DashAttack.performed += i => dashAttackInput2 = true;
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
        p1Manager = GetComponent<P1Manager>();
        animationManager = GetComponent<AnimationManager>();
    }

    public void HandleAllInputs()
    {
        if (p1Manager.p1)
        {
            HandleMovementInput();
            HandleJumpingInput();
            HandleAttackInputs();
        }
        else
        {
            HandleMovementInput2();
            HandleJumpingInput2();
            HandleAttackInputs2();
        }

    }

    void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput));
        animationManager.UpdateAnimatorValues(moveAmount, 0);
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
            p1Manager.HandleAttacks();
        }
        else if (dashAttackInput)
        {
            dashAttackInput = false;
            p1Manager.HandleAttacks(true);
        }
    }

    void HandleMovementInput2()
    {
        verticalInput2 = movementInput2.y;
        horizontalInput2 = movementInput2.x;
        moveAmount2 = Mathf.Clamp01(Mathf.Abs(horizontalInput2));
        animationManager.UpdateAnimatorValues(moveAmount2, 0);
    }

    void HandleJumpingInput2()
    {
        if (spaceInput2)
        {
            spaceInput2 = false;
            p1Locomotion.HandleJumping();
        }
    }

    void HandleAttackInputs2()
    {
        if (sliceAttackInput2)
        {
            sliceAttackInput2 = false;
            p1Manager.HandleAttacks();
        }
        else if (dashAttackInput2)
        {
            dashAttackInput2 = false;
            p1Manager.HandleAttacks(true);
        }
    }

}
