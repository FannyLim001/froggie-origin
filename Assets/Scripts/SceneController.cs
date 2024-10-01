using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of SceneController exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Persist across scenes if needed
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void Retry(string scene)
    {
        SceneManager.LoadScene(scene); // Replace with the name of your retry scene
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("FinishLevel"); // Replace with the name of your finish level scene
    }
}
