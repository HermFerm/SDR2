using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip startGameClip;   

    private bool isStarting = false;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    public void PlayGame()
    {
        if (isStarting) return;   
        isStarting = true;

        if (audioSource != null && startGameClip != null)
        {
            audioSource.PlayOneShot(startGameClip);

            Invoke(nameof(LoadNextScene), startGameClip.length);
        }
        else
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
