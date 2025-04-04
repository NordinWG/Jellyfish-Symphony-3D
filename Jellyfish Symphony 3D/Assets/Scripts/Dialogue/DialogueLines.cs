using System;
using UnityEngine;

public class DialogueLines : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName;
    [TextArea(3, 10)]
    public string[] dialogueLines;
    public float interactionRange;
    public AudioClip npcVoice;

    [Header("References")]
    public DialogueManager dialogueManager;
    public PlayerMovement PlayerMovement;

    [Header("Quest Items")]
    public Item shellItem;
    public Item staffReward;

    private Transform player;
    private bool isPlayerInRange;
    public bool IsDialogueActive { get; private set; }
    private bool hasStartedQuest = false;

    void Awake()
    {
        if (QuestManager.instance == null)
        {
            Debug.LogError("QuestManager instance is null! Ensure QuestManager is in the scene.");
        }
        else if (QuestManager.instance.quests.Count == 0)
        {
            QuestManager.instance.quests.Add(new QuestManager.Quest(
                "Find 10 Shells", "Collect 10 shells for the crabs.", shellItem, 10, staffReward));
            Debug.Log("Initialized 'Find 10 Shells' quest in QuestManager.");
        }
    }

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

            if (isPlayerInRange)
            {
                Debug.Log($"In range. Canvas active: {dialogueManager.dialogueCanvas.activeSelf}, Dialogue active: {IsDialogueActive}, Quest started: {hasStartedQuest}");
            }

            if (isPlayerInRange && Input.GetKeyDown(KeyCode.T))
            {
                // 🔧 Only start dialogue if quest has NOT started
                if (!IsDialogueActive && !dialogueManager.dialogueCanvas.activeSelf && !hasStartedQuest)
                {
                    Debug.Log("Starting dialogue with NPC (no active dialogue, canvas off, and quest not started).");
                    StartDialogue();
                }
                else if (!dialogueManager.dialogueCanvas.activeSelf && hasStartedQuest)
                {
                    Debug.Log("Player pressed T after dialogue ended and quest started, attempting to turn in quest.");
                    TryTurnInQuests();
                }
                else
                {
                    Debug.Log($"Player pressed T, but conditions not met. Canvas active: {dialogueManager.dialogueCanvas.activeSelf}, Dialogue active: {IsDialogueActive}");
                }
            }
        }

        if (isPlayerInRange)
        {
            QuestManager.instance.CheckQuestProgress();
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

        if (!hasStartedQuest)
        {
            Debug.Log("Dialogue ended, starting 'Find 10 Shells' quest for the first time.");
            QuestManager.instance.StartQuest("Find 10 Shells");
            hasStartedQuest = true;
        }
        else
        {
            Debug.Log("Dialogue ended, but 'Find 10 Shells' quest was already started.");
        }
    }

    void TryTurnInQuests()
    {
        Debug.Log("Attempting to turn in 'Find 10 Shells' quest.");
        if (QuestManager.instance.TurnInQuest("Find 10 Shells"))
        {
            Debug.Log("Completed 'Find 10 Shells' quest and received the Staff!");
            hasStartedQuest = false; // Remove this line if you don't want to restart quest ever again
        }
        else
        {
            Debug.Log("Failed to turn in 'Find 10 Shells' quest. Check if it's active and completed.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
