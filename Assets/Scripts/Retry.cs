using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retry : MonoBehaviour
{
    public AudioClip battleSound;
    public void RetryLevel()
    {
        string previousSceneName = PlayerPrefs.GetString("PreviousScene", null);
        AudioManager.instance.PlayNewMusic(battleSound);
        SceneController.Instance.Retry(previousSceneName);
    }
}
