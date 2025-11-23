using UnityEngine;

public class P1Manager : MonoBehaviour
{
    InputManager inputManager;
    P1Locomotion p1Locomotion;
    Transform grafik;
    Animator animator;
    P1AttackManager p1AttackManager;
    AnimationManager animationManager;

    [Header("Flags")]
    public bool isInteracting = false;
    public bool aimingIsUp = false;
    public bool aimingIsDown = false;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        p1Locomotion = GetComponent<P1Locomotion>();
        grafik = transform.Find("Grafik");
        animator = grafik.GetComponent<Animator>();
        p1AttackManager = GetComponent<P1AttackManager>();
        animationManager = GetComponent<AnimationManager>();
    }

    private void Update()
    {
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

    public void HandleAttacks()
    {
        animationManager.PlayTargetAnimation("Slicing", true);
        p1AttackManager.HandleSlice(aimingIsUp, aimingIsDown);       
    }

    public void HandleDeath()
    {
        Destroy(this.gameObject);
    }
}
