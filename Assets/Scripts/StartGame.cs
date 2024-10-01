using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private string installKeyFile;
    public AudioClip nextAudio;

    void Start()
    {
        installKeyFile = Path.Combine(Application.persistentDataPath, "InstallKey.txt");

        // If this is a new installation or reinstallation
        if (!File.Exists(installKeyFile))
        {
            // Write a new install key
            File.WriteAllText(installKeyFile, System.Guid.NewGuid().ToString());
            // Reset PlayerPrefs for a clean state
            PlayerPrefs.DeleteAll();
        }
    }

    public void PlayGame()
    {
        // If it's the user's first time playing after install
        if (PlayerPrefs.GetInt("FirstTime", 1) == 1)
        {
            PlayerPrefs.SetInt("FirstTime", 0);
            SceneManager.LoadScene("Cutscene");
        }
        else
        {
            SceneManager.LoadScene("ArcSelect");
        }

        AudioManager.instance.PlayNewMusic(nextAudio);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
