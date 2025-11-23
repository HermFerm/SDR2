using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1AttackManager : MonoBehaviour
{
    P1Manager p1Manager;

    public GameObject sliceSplash1;
    public GameObject spawnPoint;
    public GameObject spawnRotation;

    [Header("Dash Specs")]
    Coroutine dashCo;
    public float dashSpeed = 5;
    public float dashingTime = 1;

    private void Awake()
    {
        p1Manager = GetComponent<P1Manager>();
    }

    public void HandleSlice(bool up = false, bool down = false)
    {
        GameObject splash;
        Vector3 splashPosition;
        splash = Instantiate(sliceSplash1);
        Transform grafic = splash.transform.Find("Grafic");
        splashPosition = spawnPoint.transform.position;
        splash.transform.position = splashPosition;
        splash.transform.rotation = spawnRotation.transform.rotation;

        float yEuler = grafic.transform.localRotation.eulerAngles.y;
        float zEuler = grafic.transform.localRotation.eulerAngles.z;

        if (up || down)
        {
            if (grafic != null)
            {
                grafic.localRotation = Quaternion.Euler(90, yEuler, zEuler);
            }
        }

        Transform collidersDamage = grafic.Find("CollidersDamage");

        foreach (Transform child in collidersDamage)
        {
            DamageCollider dc = child.GetComponent<DamageCollider>();
            dc.owner = p1Manager;
        }

    }

    public void HandleDash(bool up = false, bool down = false)
    {
        p1Manager.canInput = false;

        if (dashCo != null)
        {
            StopCoroutine(dashCo);
        }
        dashCo = StartCoroutine(DashingCo());
    }

    private IEnumerator DashingCo()
    {
        Vector3 dashDirection = p1Manager.p1Locomotion.lookDirection.transform.forward;
        dashDirection.Normalize();
        dashDirection *= dashSpeed;

        float forceX = dashDirection.x;
        float forceY = dashDirection.y;
        float forceZ = dashDirection.z;
        float dashTimer = 0;

        p1Manager.animator.SetBool("isDashingNow", true);

        p1Manager.animationManager.PlayTargetAnimation("Dash", true);

        while (dashTimer <= dashingTime)
        {
            p1Manager.playerRigidbody.AddForce(forceX, forceY, forceZ, ForceMode.VelocityChange);
            dashTimer += Time.deltaTime;
            yield return null;
        }

        p1Manager.animator.SetBool("isDashingNow", false);
        p1Manager.canInput = true;
    }
}
