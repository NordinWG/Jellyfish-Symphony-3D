using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; //a

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerText;
    public Image arrowImage;
    public float typingSpeed;
    public float spaceCooldown;

    public AudioSource audioSource;
    public AudioClip defaultVoice;

    private Queue<(string speaker, string dialogue)> dialogueQueue = new Queue<(string speaker, string dialogue)>();
    private bool isTyping;
    private bool canAdvance;
    private float defaultTypingSpeed;
    private float lastSpaceTime;
    private DialogueLines currentNPC;

    public GameObject dialogueCanvas;

    void Start()
    {
        defaultTypingSpeed = typingSpeed;
        arrowImage?.gameObject.SetActive(false);
        dialogueCanvas?.SetActive(false);
    }

    public void StartDialogue((string speaker, string dialogue)[] lines, AudioClip npcVoice, DialogueLines npc)
    {
        if (lines == null || lines.Length == 0) return;

        dialogueQueue.Clear();
        foreach (var line in lines) dialogueQueue.Enqueue(line);

        dialogueCanvas?.SetActive(true);
        audioSource.clip = npcVoice != null ? npcVoice : defaultVoice;
        currentNPC = npc;
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
            if (currentNPC != null)
            {
                currentNPC.EndDialogue();
                currentNPC = null;
            }
            return;
        }

        StartCoroutine(TypeLine(dialogueQueue.Dequeue()));
    }

    IEnumerator TypeLine((string speaker, string dialogue) line)
    {
        isTyping = true;
        dialogueText.text = "";
        speakerText.text = line.speaker;

        if (audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastSpaceTime + spaceCooldown && (canAdvance || !isTyping))
        {
            canAdvance = false;
            arrowImage?.gameObject.SetActive(false);
            lastSpaceTime = Time.time;
            NextLine();
        }
    }
}