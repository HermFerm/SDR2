using UnityEngine;

public class P1AttackManager : MonoBehaviour
{
    P1Manager p1Manager;

    public GameObject sliceSplash1;
    public GameObject spawnPoint;
    public GameObject spawnRotation;

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
}
