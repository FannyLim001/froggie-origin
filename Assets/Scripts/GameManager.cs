using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int totalEnemy;

    public GameObject enemyPlacement;
    public GameObject bossPrefab;
    public GameObject bossPlacement;
    public GameObject bossSFX;
    public AudioClip bossClip;

    public float elapsedTime;

    public GameObject walkTutorial;
    public GameObject attackTutorial;
    public GameObject jumpTutorial;

    public GameObject winPanel;
    public GameObject losePanel;

    public GameObject bossBattle;

    public AudioClip finishAudio;
    public AudioClip loseAudio;

    void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }

    void Start()
    {
        AudioManager.instance.SetVolume(0.5f);
        ResetUI();
        // Initialize totalEnemy with the number of enemies at the start
        totalEnemy = enemyPlacement.transform.childCount;
        Debug.Log("Total enemies at start: " + totalEnemy);

        StartCoroutine(showTutorial());
    }

    void ResetUI()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        bossBattle.SetActive(false);
        walkTutorial.SetActive(false);
        attackTutorial.SetActive(false);
        jumpTutorial.SetActive(false);
    }

    IEnumerator showTutorial()
    {
        // Show walk tutorial for 3 seconds
        walkTutorial.SetActive(true);
        yield return new WaitForSeconds(3f);
        walkTutorial.SetActive(false);

        // Show attack tutorial for 3 seconds
        attackTutorial.SetActive(true);
        yield return new WaitForSeconds(3f);
        attackTutorial.SetActive(false);

        // Show jump tutorial for 3 seconds
        jumpTutorial.SetActive(true);
        yield return new WaitForSeconds(3f);
        jumpTutorial.SetActive(false);

        // Tutorial finished, you can trigger other events if needed
        Debug.Log("Tutorial finished");
    }

    // Call this method when an enemy is defeated
    public void OnEnemyDefeated()
    {
        totalEnemy--;
        Debug.Log("Remaining enemies: " + totalEnemy);

        // Trigger the boss battle if all enemies are defeated
        if (totalEnemy == 0)
        {
            BossBattle();
        }
    }

    void BossBattle()
    {
        bossBattle.SetActive(true);

        AudioManager.instance.PlayNewMusic(bossClip);

        bossSFX.GetComponent<AudioSource>().Play();

        // Instantiate the boss as a child of the bossPlacement
        GameObject boss = Instantiate(bossPrefab, bossPlacement.transform.position, Quaternion.Euler(0, 180, 0));
        boss.transform.SetParent(bossPlacement.transform);
        Debug.Log("Boss Battle Started!");
    }

    public void OnBossDefeated()
    {
        winPanel.SetActive(true);
        Debug.Log("Boss defeated, changing scene...");
        // Change the scene when the boss is defeated
        StartCoroutine(AllEnemyDie());
    }

    public void PlayerDie()
    {
        losePanel.SetActive(true);
        StartCoroutine(PlayerDied());
    }

    IEnumerator PlayerDied()
    {
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        ResetUI();
        AudioManager.instance.PlayNewMusic(loseAudio, false);
        SceneManager.LoadScene("Retry");
    }

    IEnumerator AllEnemyDie()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.instance.PlayNewMusic(finishAudio, false);
        SceneController.Instance.LoadNextLevel();
    }

    // Optional: Use Update if you need to monitor or trigger additional events
    void Update()
    {
        // Example: Additional checks or updates can go here if necessary
        elapsedTime += Time.deltaTime;
        PlayerPrefs.SetFloat("ResultTimer", elapsedTime);
    }
}
