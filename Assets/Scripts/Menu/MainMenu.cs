using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Scroll Reveal")]
    public RectTransform scrollRoot;   // dein ScrollReveal
    public float targetWidth = 1920f;  // Endbreite
    public float slideDuration = 0.5f; // Dauer der Animation

    private bool isStarting = false;
    private float originalHeight;

    private void Awake()
    {
        Debug.Log("[MainMenu] Awake");

        if (scrollRoot != null)
        {
            originalHeight = scrollRoot.sizeDelta.y;

            // Startzustand: Breite 0
            var size = scrollRoot.sizeDelta;
            size.x = 0f;
            size.y = originalHeight;
            scrollRoot.sizeDelta = size;

            Debug.Log("[MainMenu] scrollRoot initial size = " + scrollRoot.sizeDelta);
        }
        else
        {
            Debug.LogWarning("[MainMenu] scrollRoot ist NICHT gesetzt!");
        }
    }

    public void PlayGame()
    {
        Debug.Log("[MainMenu] PlayGame() called");

        if (isStarting) return;
        isStarting = true;

        StartCoroutine(ScrollAndLoad());
    }

    private IEnumerator ScrollAndLoad()
    {
        Debug.Log("[MainMenu] ScrollAndLoad started");

        if (scrollRoot == null)
        {
            Debug.LogError("[MainMenu] scrollRoot ist null, breche ab.");
            yield break;
        }

        float t = 0f;

        // Animation über Time.unscaledDeltaTime, falls Time.timeScale evtl. 0 ist
        while (t < slideDuration)
        {
            t += Time.unscaledDeltaTime;
            float normalized = Mathf.Clamp01(t / slideDuration);

            scrollRoot.sizeDelta = new Vector2(
                Mathf.Lerp(0f, targetWidth, normalized),
                originalHeight
            );

            yield return null;
        }

        // zum Schluss hart auf Zielwert setzen
        scrollRoot.sizeDelta = new Vector2(targetWidth, originalHeight);
        Debug.Log("[MainMenu] Scroll fertig, final size = " + scrollRoot.sizeDelta);

        // Szene laden
        var current = SceneManager.GetActiveScene();
        int nextIndex = current.buildIndex + 1;
        Debug.Log("[MainMenu] Lade Szene Index " + nextIndex);
        SceneManager.LoadScene(nextIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
