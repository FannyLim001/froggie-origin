using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public AudioClip playLevel;
    public AudioClip startMenu;

    public void SelectLevel(int level)
    {
        AudioManager.instance.PlayNewMusic(playLevel);
        SceneManager.LoadScene("Level" + level);
    }

    public void Back()
    {
        AudioManager.instance.PlayNewMusic(startMenu);
        SceneManager.LoadScene("StartMenu");
    }
}
