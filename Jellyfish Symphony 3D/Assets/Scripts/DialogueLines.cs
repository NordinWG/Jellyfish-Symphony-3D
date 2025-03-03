using UnityEngine;

public class DialogueLines : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager.DialogueLines = new[]
        {
            new[]
            {
                ("Sander", "Listen, I NEED that drownie. Like, *DESPERATELY* need it."),
                ("Sander", "I woke up today, and the FIRST thing on my mind? That DAMN drownie."),
                ("Sander", "Not just any drownie—the *WEED drownie*. The one I’ve been DREAMING about."),
                ("Sander", "I can already taste it. That SOFT, CHOCOLATEY perfection. I NEED it NOW."),
                ("Sander", "ONE bite. That’s all I’m asking. Just ONE, and I’ll be at peace."),
                ("Sander", "Everything would be *BETTER* with that drownie. WHY is that so hard to understand?"),
                ("Sander", "Every second without it is DRIVING me INSANE. Do you GET it?"),
                ("Sander", "You’ve got it, DON’T you? I can feel it. YOU have the power to make this RIGHT."),
                ("Sander", "I’m not asking for MUCH here. I’m not being DRAMATIC. I just NEED that drownie."),
                ("Sander", "PLEASE. I’m losing my MIND. Hand it over before I lose it COMPLETELY."),
                ("Alira", "What do you mean..?"),
                ("Sander", "I NEED that drownie. That MAGICAL DROWNIE. get IT to me or I will never be able to rest, EVER."),
                ("Alira", "O-okay..? I will bring you the drownie..."),
                ("[HIDE]", "")
            }
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            dialogueManager.StartDialogue(dialogueManager.DialogueLines[0]);
        }
    }
}