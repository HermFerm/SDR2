using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    public Transform deathPosition;
    public P1Manager p1;
    public P1Manager p2;
    public P1Manager deadP;
    public GameObject deathSplash;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PerformGameOver()
    {
        if (p1 != deadP)
        {
            p1.canInput = false;
            p1.endGame = true;
        }

        if (p2 != deadP)
        {
            p2.canInput = false;
            p2.endGame = true;
        }

        GameObject ds = Instantiate(deathSplash);
        ds.transform.position = deathPosition.position;
        ds.transform.rotation = deathPosition.rotation;
        ds.transform.localScale = deathPosition.localScale;
    }



}
