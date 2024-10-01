using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public float transitionTime = 3f; // Time to wait before transitioning to the next scene
    public AudioClip nextAudio;

    void Start()
    {
        // Start the transition to the next scene
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        // Wait for the transition time or animation
        yield return new WaitForSeconds(transitionTime);

        // Load the next scene (gameplay scene)
        SceneManager.LoadScene("Level1"); // Replace with your actual gameplay scene name
        AudioManager.instance.PlayNewMusic(nextAudio);
    }
}
