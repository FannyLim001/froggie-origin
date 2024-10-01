using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public AudioSource audioSource;  // Reference to AudioSource component

    public GameObject response;

    private Queue<string> sentences;
    private Queue<AudioClip> voiceOvers;

    void Awake()
    {
        sentences = new Queue<string>();
        voiceOvers = new Queue<AudioClip>();
    }

    void Start()
    {
        AudioManager.instance.SetVolume(0.2f);
        response.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation with " + dialogue.speaker);

        sentences.Clear();
        voiceOvers.Clear();

        // Enqueue sentences and voice-over clips
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        foreach (AudioClip clip in dialogue.voiceOver)
        {
            voiceOvers.Enqueue(clip);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        AudioClip clip = voiceOvers.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, clip));  // Pass the voice clip to the coroutine
    }

    IEnumerator TypeSentence(string sentence, AudioClip clip)
    {
        dialogueText.text = "";

        // Play the voice-over clip
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }

        float clipLength = clip != null ? clip.length : 0f;
        int sentenceLength = sentence.Length;

        // Calculate the delay per character based on voice-over duration
        float typeDelay = sentenceLength > 0 ? clipLength / sentenceLength : 0.05f;

        // Type the sentence character by character with the calculated delay
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeDelay);
        }

        // Wait for the voice-over to finish before moving to the next sentence
        while (audioSource.isPlaying)
        {
            yield return null;
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation");
        response.SetActive(true);
        StartCoroutine(StartAdventure());
    }

    IEnumerator StartAdventure()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("LevelTransition");
    }
}
