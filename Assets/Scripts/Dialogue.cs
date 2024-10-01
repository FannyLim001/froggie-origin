using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string speaker;

    [TextArea(3,10)]
    public string[] sentences;

    public AudioClip[] voiceOver;
}
