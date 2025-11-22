using UnityEngine;

public class P1Manager : MonoBehaviour
{
    InputManager inputManager;
    P1Locomotion p1Locomotion;
    Animator animator;

    [Header("Flags")]
    public bool isInteracting = false;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        p1Locomotion = GetComponent<P1Locomotion>();
        animator = GetComponent<Animator>();
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
    }
}
