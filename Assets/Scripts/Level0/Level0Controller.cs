using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level0Controller : MonoBehaviour
{
    public Sprite aykaSpriteImage;
    public Sprite connieSpriteImage;

    public Animator dialogueBoxAnimator;
    public Animator continueDialogueButtonAnimator;
    public Image dialogueBoxCharacterImage;
    public TMP_Text dialogueBoxCharacterName;
    public TMP_Text dialogueBoxSentenceBox;

    private Dialogue aykaMonologue;
    private bool inDialogue = false;
    private bool continueDialogue = false;
    private int index = 0;

    private void Start()
    {
        FeedDialoguesArrays();
        StartCoroutine(Level0Actions());
    }

    void FeedDialoguesArrays()
    {
        aykaMonologue = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "¡Hola!", 
            "¿Cómo estás?..."
        });
    }

    public IEnumerator Level0Actions()
    {
        dialogueBoxAnimator.SetInteger("state", 1);
        continueDialogueButtonAnimator.SetInteger("state", 1);
        continueDialogue = false;
        DisplayDialogueSentence(aykaMonologue, index);
        while (!continueDialogue) yield return null;
        index++;
        continueDialogue = false;
        DisplayDialogueSentence(aykaMonologue, index);
        while (!continueDialogue) yield return null;
        continueDialogue = false;
        index = 0;
        /* FINISHING AYKA MONOLOGUE */
        dialogueBoxAnimator.SetInteger("state", 2);
        continueDialogueButtonAnimator.SetInteger("state", 2);
    }

    void DisplayDialogueSentence(Dialogue dialogue, int index)
    {
        dialogueBoxCharacterImage.sprite = dialogue.characterImage;
        dialogueBoxCharacterName.text = dialogue.characterName;
        dialogueBoxSentenceBox.text = dialogue.sentences[index];
    }

    public void OnContinueDialogue()
    {
        continueDialogue = true;
    }
}
