using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioSource audioSource;
    public AudioClip hoverClip;
    public AudioClip clickClip;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponentInParent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && hoverClip != null)
            audioSource.PlayOneShot(hoverClip);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource != null && clickClip != null)
            audioSource.PlayOneShot(clickClip);
    }
}
