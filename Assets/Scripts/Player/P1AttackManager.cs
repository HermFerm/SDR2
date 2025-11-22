using UnityEngine;

public class P1AttackManager : MonoBehaviour
{
    P1Manager p1Manager;

    public GameObject sliceSplash1;

    private void Awake()
    {
        p1Manager = GetComponent<P1Manager>();
    }

    public void HandleSlice()
    {
        GameObject splash;
        Vector3 splashPosition;
        splash = Instantiate(sliceSplash1);
        splashPosition = this.transform.position;
        splashPosition.x += 1;
        splash.transform.position = splashPosition;

    }
}
