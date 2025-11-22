using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider triggerCollider;
    public P1Manager owner;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        triggerCollider.gameObject.SetActive(true);
        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        P1Manager pVictim = other.GetComponent<P1Manager>();

        if (pVictim != null)
        {
            if (pVictim != owner)
            {
                pVictim.HandleDeath();
            }
        }
    }
}
