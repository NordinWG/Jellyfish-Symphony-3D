using UnityEngine;
using System.Collections;

public class DialogueLines : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName;
    [TextArea(3, 10)]
    public string[] dialogueLines;
    public float interactionRange = 2f;
    public AudioClip npcVoice;

    [Header("References")]
    public DialogueManager dialogueManager;
    public PlayerMovement PlayerMovement;

    private Transform player;
    private bool isPlayerInRange;
    public bool IsDialogueActive { get; private set; }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        IsDialogueActive = false;
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            isPlayerInRange = distance <= interactionRange;

            if (isPlayerInRange && Input.GetKeyDown(KeyCode.T) && !IsDialogueActive)
            {
                StartDialogue();
            }
        }
    }

    void StartDialogue()
    {
        if (dialogueLines.Length == 0) return;

        PlayerMovement.enabled = false;

        (string speaker, string dialogue)[] lines = new (string, string)[dialogueLines.Length];
        for (int i = 0; i < dialogueLines.Length; i++)
        {
            lines[i] = (npcName, dialogueLines[i]);
        }

        dialogueManager.StartDialogue(lines, npcVoice, this);
        IsDialogueActive = true;
    }

    public void EndDialogue()
    {
        IsDialogueActive = false;
        PlayerMovement.enabled = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}