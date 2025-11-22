using UnityEngine;
using UnityEngine.Audio;

public class P1Locomotion : MonoBehaviour
{
    P1Manager p1Manager;
    InputManager inputManager;
    AnimationManager animationManager;

    public Vector3 moveDirection;
    public GameObject lookDirection;
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
    public float rotationSpeed = 15;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Player Audio")]
    public AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip moveClip;


    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        p1Manager = GetComponent<P1Manager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        animationManager = GetComponent<AnimationManager>();

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
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
        HandleRotation();
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

        bool isMovingHorizontally = Mathf.Abs(inputManager.horizontalInput) > 0.01f && isGrounded && !isJumping;
        HandleMoveSound(isMovingHorizontally);
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        Vector3 lookVector = Vector3.zero;

        targetDirection = cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.z = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;

        lookVector = cameraObject.up * inputManager.verticalInput;
        lookVector += cameraObject.right * inputManager.horizontalInput;
        lookVector.Normalize();
        lookVector.z = 0;

        if (lookVector == Vector3.zero)
            lookVector = lookDirection.transform.forward;

        Quaternion lookRotation = Quaternion.LookRotation(lookVector);
        Quaternion lookDRotation = Quaternion.Slerp(lookDirection.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        lookDirection.transform.rotation = lookDRotation;

        if (lookDirection.transform.localRotation.eulerAngles.x == 270)
        {
            p1Manager.aimingIsUp = true;
            p1Manager.aimingIsDown = false;
        }
        else if (lookDirection.transform.localRotation.eulerAngles.x == 90)
        {
            p1Manager.aimingIsUp = false;
            p1Manager.aimingIsDown = true;
        }
        else
        {
            p1Manager.aimingIsUp = false;
            p1Manager.aimingIsDown = false;
        }
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
            if (!isGrounded && !p1Manager.isInteracting)
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

            PlayJumpSound();

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

    #region Sounds
    void PlayJumpSound()
    {
        if (audioSource != null && jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip);
        }
    }
    void HandleMoveSound(bool isMoving)
    {
        if (audioSource == null || moveClip == null)
            return;

        if (isMoving)
        {
            if (!audioSource.isPlaying || audioSource.clip != moveClip)
            {
                audioSource.clip = moveClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying && audioSource.clip == moveClip)
            {
                audioSource.Stop();
                audioSource.loop = false;
                audioSource.clip = null;
            }
        }
    }
    #endregion
}
