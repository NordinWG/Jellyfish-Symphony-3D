using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerText;
    public Image arrowImage;
    public float typingSpeed;
    public float spaceCooldown;

    public AudioSource audioSource;
    public AudioClip defaultVoice;
    public AudioClip sanderVoice;
    public AudioClip aliraVoice;

    public (string speaker, string dialogue)[][] DialogueLines;
    private Queue<(string speaker, string dialogue)> dialogueQueue = new Queue<(string speaker, string dialogue)>();
    private bool isTyping;
    private bool canAdvance;
    private float defaultTypingSpeed;

    public GameObject dialogueCanvas;

    void Start()
    {
        defaultTypingSpeed = typingSpeed;
        arrowImage?.gameObject.SetActive(false);
        dialogueCanvas?.SetActive(false);
    }

    public void StartDialogue((string speaker, string dialogue)[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        dialogueQueue.Clear();
        foreach (var line in lines) dialogueQueue.Enqueue(line);

        dialogueCanvas?.SetActive(true);
        NextLine();
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            isTyping = false;
            canAdvance = true;
            var currentLine = dialogueQueue.Peek();
            dialogueText.text = currentLine.dialogue;
            speakerText.text = currentLine.speaker;
            audioSource.Stop();
            return;
        }

        if (dialogueQueue.Count == 0)
        {
            dialogueCanvas?.SetActive(false);
            return;
        }

        StartCoroutine(TypeLine(dialogueQueue.Dequeue()));
    }

    IEnumerator TypeLine((string speaker, string dialogue) line)
    {
        isTyping = true;
        dialogueText.text = "";
        speakerText.text = line.speaker;

        if (line.speaker == "[HIDE]")
        {
            dialogueCanvas?.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            isTyping = false;
            canAdvance = true;
            arrowImage?.gameObject.SetActive(true);
            yield break;
        }

        PlayVoice(line.speaker);

        foreach (char letter in line.dialogue)
        {
            dialogueText.text += letter;
            typingSpeed = Input.GetKey(KeyCode.Space) ? 0.01f : defaultTypingSpeed;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        canAdvance = true;
        audioSource.Stop();
        arrowImage?.gameObject.SetActive(true);
        typingSpeed = defaultTypingSpeed;
    }

    void PlayVoice(string speaker)
    {
        if (audioSource.isPlaying) return;

        audioSource.clip = speaker switch
        {
            "Sander" when sanderVoice != null => sanderVoice,
            "Alira" when aliraVoice != null => aliraVoice,
            _ => defaultVoice
        };

        if (audioSource.clip != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= spaceCooldown && (canAdvance || !isTyping))
        {
            canAdvance = false;
            arrowImage?.gameObject.SetActive(false);
            NextLine();
        }
    }
}