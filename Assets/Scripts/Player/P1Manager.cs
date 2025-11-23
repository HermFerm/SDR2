using UnityEngine;
using UnityEngine.InputSystem;

public class P1Manager : MonoBehaviour
{
    InputManager inputManager;
    public P1Locomotion p1Locomotion;
    Transform grafik;
    public Animator animator;
    P1AttackManager p1AttackManager;
    public AnimationManager animationManager;
    public Rigidbody playerRigidbody;

    [Header("Flags")]
    public bool isInteracting = false;
    public bool aimingIsUp = false;
    public bool aimingIsDown = false;
    public bool canInput = true;
    public bool p1 = true;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        p1Locomotion = GetComponent<P1Locomotion>();
        grafik = transform.Find("Grafik");
        animator = grafik.GetComponent<Animator>();
        p1AttackManager = GetComponent<P1AttackManager>();
        animationManager = GetComponent<AnimationManager>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (p1)
        {
            WorldManager.Instance.p1 = this;
        }
        else
        {
            WorldManager.Instance.p2 = this;
        }
    }

    private void Update()
    {
        if (canInput)
            inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        p1Locomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting");
        p1Locomotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", p1Locomotion.isGrounded);
    }

    public void HandleAttacks(bool dash = false)
    {
        if (!dash)
        {
            animationManager.PlayTargetAnimation("Slicing", true);
            p1AttackManager.HandleSlice(aimingIsUp, aimingIsDown);
        }
        else
        {
            p1AttackManager.HandleDash(aimingIsUp, aimingIsDown); 
        }
    }

    public void HandleDeath()
    {
        WorldManager.Instance.deathPosition = transform;
        WorldManager.Instance.deadP = this;
        WorldManager.Instance.PerformGameOver();
        Destroy(gameObject);       
    }
}
