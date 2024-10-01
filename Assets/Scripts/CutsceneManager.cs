using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // If using TextMeshPro for the loading text

public class CutsceneManager : MonoBehaviour
{
    public GameObject[] panels; // Array to hold all the panels
    public Button nextButton;   // Reference to the Next button
    private int currentPanelIndex = 0; // To track which panel is currently active

    public AudioSource sfx;
    public Image loading;       // Image for the loading screen
    public TMP_Text loadingText;    // Reference to the loading text (child of Image)
    public float fadeSpeed = 1.0f; // Speed of fade effect for loading screen

    public AudioClip nextAudio;

    void Start()
    {
        // Hide all panels except the first one
        for (int i = 1; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        // Initially hide the loading screen and its text
        SetAlpha(loading, 0);
        SetAlpha(loadingText, 0);

        // Add the button click listener to advance the cutscene
        nextButton.onClick.AddListener(ShowNextPanel);
    }

    void Update()
    {
        // Check if the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextPanel();
        }
    }

    void ShowNextPanel()
    {
        // Hide the current panel
        panels[currentPanelIndex].SetActive(false);

        sfx.Play();

        // Increment to the next panel
        currentPanelIndex++;

        // If we've reached the last panel, do something (like ending the cutscene)
        if (currentPanelIndex >= panels.Length)
        {
            EndCutscene();
        }
        else
        {
            // Show the next panel
            panels[currentPanelIndex].SetActive(true);
        }
    }

    void EndCutscene()
    {
        nextButton.gameObject.SetActive(false);

        // Start loading the next scene asynchronously with a loading screen
        StartCoroutine(LoadNextSceneAsync());
    }

    IEnumerator LoadNextSceneAsync()
    {
        // Fade in the loading screen and its text
        yield return StartCoroutine(FadeLoadingScreen(true));

        // Optional: A brief delay before starting the scene load
        yield return new WaitForSeconds(0.5f);

        // Start loading the next scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ShortStory"); // Replace with your next scene name

        AudioManager.instance.PlayNewMusic(nextAudio);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Scene is loaded, optionally fade out the loading screen and its text
        yield return StartCoroutine(FadeLoadingScreen(false));
    }

    IEnumerator FadeLoadingScreen(bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1.0f : 0.0f;
        float startAlpha = loading.color.a;

        // Fade the loading image and text alpha over time
        for (float t = 0; t < 1.0f; t += Time.deltaTime * fadeSpeed)
        {
            float blend = Mathf.Clamp01(t);
            SetAlpha(loading, Mathf.Lerp(startAlpha, targetAlpha, blend));
            SetAlpha(loadingText, Mathf.Lerp(startAlpha, targetAlpha, blend)); // Fade the text

            yield return null;
        }

        // Ensure the final alpha is set
        SetAlpha(loading, targetAlpha);
        SetAlpha(loadingText, targetAlpha);
    }

    // Helper function to set alpha for Image
    void SetAlpha(Image img, float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }

    // If using TextMeshPro instead of Unity's default Text:
    void SetAlpha(TMP_Text txt, float alpha)
    {
        Color color = txt.color;
        color.a = alpha;
        txt.color = color;
    }
}
