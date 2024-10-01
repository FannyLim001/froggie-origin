using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public TMP_Text duration;
    public AudioClip nextAudio;
    // Start is called before the first frame update
    void Start()
    {
        float timerValue = PlayerPrefs.GetFloat("ResultTimer");
        int minutes = Mathf.FloorToInt(timerValue / 60);
        int seconds = Mathf.FloorToInt(timerValue % 60);
        duration.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void NextAdventure()
    {
        AudioManager.instance.PlayNewMusic(nextAudio);
        SceneManager.LoadScene("ArcSelect");
    }
}
