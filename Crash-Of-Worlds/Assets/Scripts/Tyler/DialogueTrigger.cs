using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
    public AudioClip CharacterVoice;

    //[Range(0.1f, 1f)] public float typingSpeed;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (Input.GetKey("e") && !DialogueManager.instance.isDialogueActive)
            {
                TriggerDialogue();
            }
        }
    }
}
