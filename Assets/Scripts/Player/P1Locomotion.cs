using UnityEngine;

public class P1Locomotion : MonoBehaviour
{
    P1Manager p1Manager;
    InputManager inputManager;
    AnimationManager animationManager;

    public Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("Falling")]
    public float InAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float movementSpeed = 7;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        p1Manager = GetComponent<P1Manager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        animationManager = GetComponent<AnimationManager>();
    }

    private void Update()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else if (coyoteTimeCounter > 0)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    public void HandleAllMovement()
    {
        /*if (p1Manager.isInteracting)
            return;*/

        HandleMovement();
        HandleFallingAndLanding();

        if (isJumping)
        {
            HandleJumpingHeight();
        }
    }

    private void HandleMovement()
    {
        moveDirection = cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.z = 0;
        moveDirection *= movementSpeed;

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.linearVelocity = movementVelocity;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y = raycastOrigin.y + rayCastHeightOffset;

        if (!isGrounded && !isJumping)
        {
            if (!p1Manager.isInteracting)
            {
                animationManager.PlayTargetAnimation("Falling", true);
            }

            InAirTimer = InAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * InAirTimer);
        }

        if (Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if(!isGrounded && !p1Manager.isInteracting)
            {
                animationManager.PlayTargetAnimation("Land", true);
            }

            InAirTimer = 5;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void HandleJumping()
    {
        if (coyoteTimeCounter > 0f)
        {
            animationManager.animator.SetBool("isJumping", true);
            animationManager.PlayTargetAnimation("Jump", false);

            coyoteTimeCounter = 0f;
        }
    }

    void HandleJumpingHeight()
    {
        float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
        Vector3 playerVelocity = moveDirection;
        playerVelocity.y = jumpingVelocity;
        playerRigidbody.linearVelocity = playerVelocity;
    }
}
