using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public float fadeDuration = 0.5f;
    private float masterVolume = 1f;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to play new music with fading and loop control
    public void PlayNewMusic(AudioClip newClip, bool shouldLoop = true)
    {
        StartCoroutine(FadeOutAndIn(newClip, shouldLoop));
    }

    private IEnumerator FadeOutAndIn(AudioClip newClip, bool shouldLoop)
    {
        // Fade out the current music
        yield return StartCoroutine(FadeOut());

        // Change to the new music and set the loop property
        audioSource.clip = newClip;
        audioSource.loop = shouldLoop; // Set loop based on parameter
        audioSource.Play();

        // Fade in the new music
        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }

    private IEnumerator FadeIn()
    {
        float startVolume = 0.0f;
        audioSource.volume = startVolume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, masterVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = masterVolume;
    }

    // Method to set the volume from code
    public void SetVolume(float volume)
    {
        masterVolume = Mathf.Clamp(volume, 0f, 1f); // Clamp the volume between 0 and 1
        audioSource.volume = masterVolume;
    }

    // Method to get the current volume level
    public float GetVolume()
    {
        return masterVolume;
    }
}
