using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

[System.Serializable]

public class DialogueCharacter
{
    public string name;
    public Sprite icon;
    public AudioClip CharacterVoice;

    [UnityEngine.Range(0f, 0.75f)] public float typingSpeed;
}

[System.Serializable]
public class introStuff
{
    public GameObject BlackScreen;
    public AudioClip Explosion;
    public AudioSource audioS;
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
    public bool isIntro;
    public bool activatedIntro;
    public IntroCutscene Intro;
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }

    public void Start()
    {
        if (isIntro)
        {
            StartCoroutine(nameof(IntroSequence));
        }
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

    public IEnumerator IntroSequence()
    {
        activatedIntro = true;
        Intro.BlackScreen.SetActive(true);
        yield return new WaitForSeconds(3);
        Intro.audioS.PlayOneShot(Intro.Explosion, 1f);
        Destroy(Intro.BlackScreen);
        yield return new WaitForSeconds(0f);
        TriggerDialogue();
        yield return new WaitForSeconds(1f);
        DialogueManager.instance.DisplayNextDialogueLine();
        yield return new WaitForSeconds(1f);
        DialogueManager.instance.DisplayNextDialogueLine();
        yield return new WaitForSeconds(1f);
        DialogueManager.instance.EndDialogue();
        yield return new WaitForSeconds(0f);
        Destroy(DialogueManager.instance);
    }
}
