using System;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueLines : MonoBehaviour
{
    [Header("NPC Settings")] // koptekst
    public string npcName;
    [TextArea(3, 10)] // groter tekst vak in unity
    public string[] dialogueLines; // array van strings
    public float interactionRange;
    public AudioClip npcVoice;

    [Header("References")]
    public DialogueManager dialogueManager;
    public PlayerMovement PlayerMovement;

    private Transform player;
    private bool isPlayerInRange;
    public bool IsDialogueActive { get; private set; } // andere class kan krijgen maar niet aanpassen

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // niet gevonden = null, oftw geen error
        IsDialogueActive = false;
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position); // berekent afstand tussen objecten
            isPlayerInRange = distance <= interactionRange; // <= kleiner dan of gelijk aan

            if (isPlayerInRange && Input.GetKeyDown(KeyCode.T) && !IsDialogueActive) // && is en
            {
                StartDialogue();
            }
        }
    }

    void StartDialogue()
    {
        if (dialogueLines.Length == 0) return; // length kijkt hoeveel elementen in array staan, als array leeg dus 0 dan stopt deze void

        PlayerMovement.enabled = false;

        // (string speaker, string dialogue) is een tuple type, [] geeft aan dat je een array van tuples wilt maken, naam van array is lines
        // maakt array van tuples met de speaker en dialogue tekst, gebaseerd op lengte van dialogueLines
        // tuple is verzameling van meerdere waarden van verschillende types
        (string speaker, string dialogue)[] lines = new (string, string)[dialogueLines.Length];

        // for start een loop waarmee je iets meerdere keren kan uitvoeren
        // zolang i kleiner is dan het aantal elementen in dialogueLines, blijft de loop lopen
        // i++ verhoogd i met 1 elke keer als de code uitgevoerd wordt
        
        for (int i = 0; i < dialogueLines.Length; i++)
        {
            // de loop vult elke index van de lines array met een tuple bestaande uit de npc-naam en de bijbehorende dialoogregel
            // de npc-naam blijft hetzelfde voor elke regel, alleen de tekst verandert per herhaling
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