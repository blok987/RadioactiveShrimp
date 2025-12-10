using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
    public AudioSource VoiceSFX;

    private Queue<DialogueLine> lines = new();

    public bool isDialogueActive = false;

    //[Range(0.1f, 1f)] public float typingSpeed;
    

    public Animator anim;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;

        anim.Play("show");

        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines) 
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            VoiceSFX.PlayOneShot(dialogueLine.character.CharacterVoice, 0.7f);
            dialogueArea.text += letter;
            yield return new WaitForSeconds(dialogueLine.character.typingSpeed);
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        anim.Play("hide");
    }
}
