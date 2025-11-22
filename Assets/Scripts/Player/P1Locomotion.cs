using UnityEngine;

public class P1Locomotion : MonoBehaviour
{
    P1Manager p1Manager;
    InputManager inputManager;
    AnimationManager animationManager;

    Vector3 moveDirection;
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

    [Header("Movement Speeds")]
    public float movementSpeed = 7;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        p1Manager = GetComponent<P1Manager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        animationManager = GetComponent<AnimationManager>();
    }

    public void HandleAllMovement()
    {
        /*if (p1Manager.isInteracting)
            return;*/

        HandleMovement();
        HandleFallingAndLanding();
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

        if (!isGrounded)
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
}
