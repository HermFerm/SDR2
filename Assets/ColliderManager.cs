using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    Transform landingColliderTrans;
    Transform damageColliderTrans;
    GameObject landingCollider;
    GameObject damageCollider;

    private void Awake()
    {
        landingColliderTrans = transform.Find("Colliders");
        damageColliderTrans = transform.Find("CollidersDamage");
        landingCollider = landingColliderTrans.gameObject;
        damageCollider = damageColliderTrans.gameObject;

        landingCollider.SetActive(false);
        damageCollider.SetActive(true);
    }

    public void ChangeCollider()
    {
        landingCollider.SetActive(true);
        damageCollider.SetActive(false);
    }
}
