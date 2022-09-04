using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level0Controller : MonoBehaviour
{
    public Sprite aykaSpriteImage;
    public Sprite connieSpriteImage;
    public Rigidbody2D aykaRb2d;
    public Transform aykaTransform;
    public Animator aykaAnimator;
    public Rigidbody2D connieRb2d;
    public Transform connieTransform;
    public Animator connieAnimator;
    public float aykaMovementSpeed;
    public float connieMovementSpeed;

    public Animator dialogueBoxAnimator;
    public Animator continueDialogueButtonAnimator;
    public Image dialogueBoxCharacterImage;
    public TMP_Text dialogueBoxCharacterName;
    public TMP_Text dialogueBoxSentenceBox;

    //private bool inDialogue = false;
    private bool continueDialogue = false;
    private int index = 0;
    private IEnumerator typeSentenceCoroutine;
    private int aykaDirX = 0;
    private int connieDirX = 0;

    private void Start()
    {
        FeedDialoguesArrays();
        MoveConnieToLimitOfCamera();
        StartCoroutine(Level0Actions());
    }

    private void FixedUpdate()
    {
        aykaRb2d.velocity = new Vector2(aykaDirX * aykaMovementSpeed * Time.fixedDeltaTime, aykaRb2d.velocity.y);
        AykaUpdateAnimation();
        connieRb2d.velocity = new Vector2(connieDirX * connieMovementSpeed * Time.fixedDeltaTime, connieRb2d.velocity.y);
        ConnieUpdateAnimation();
    }

    public IEnumerator Level0Actions()
    {
        yield return new WaitForSeconds(1.5f);
        DisplayCompleteDialogueUI();   continueDialogue = false;
        DisplayDialogueSentence(aykaMonologue, index);
        while (!continueDialogue) yield return null;
        StopCoroutine(typeSentenceCoroutine);
        index++;   continueDialogue = false;
        DisplayDialogueSentence(aykaMonologue, index);
        while (!continueDialogue) yield return null;
        StopCoroutine(typeSentenceCoroutine);
        continueDialogue = false;   index = 0;
        HideCompleteDialogueUI();
        // CONNIE APPEARS
        yield return new WaitForSeconds(.8f);
        connieDirX = -1;
        while (connieTransform.position.x > 2f) yield return null;
        connieDirX = 0;
        /* STARTING CONNIEDIALOGUE1 */
        DisplayCompleteDialogueUI(); continueDialogue = false;
        DisplayDialogueSentence(connieDialogue1, index);
        while (!continueDialogue) yield return null;
        StopCoroutine(typeSentenceCoroutine);
        index++; continueDialogue = false;
        DisplayDialogueSentence(connieDialogue1, index);
        while (!continueDialogue) yield return null;
        StopCoroutine(typeSentenceCoroutine);
        continueDialogue = false; index = 0;
        HideCompleteDialogueUI();
    }

    void DisplayDialogueSentence(Dialogue dialogue, int index)
    {
        dialogueBoxCharacterImage.sprite = dialogue.characterImage;
        dialogueBoxCharacterName.text = dialogue.characterName;
        //dialogueBoxSentenceBox.text = dialogue.sentences[index];
        typeSentenceCoroutine = TypeSentence(dialogue.sentences[index], 0.05f);
        StartCoroutine(typeSentenceCoroutine);
    }

    IEnumerator TypeSentence(string sentence, float timeBetweenLetters)
    {
        dialogueBoxSentenceBox.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueBoxSentenceBox.text += letter;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
    }

    void AykaUpdateAnimation()
    {
        int state;
        if (aykaDirX > 0f)
        {
            //sR.flipX = false;
            aykaTransform.rotation = Quaternion.Euler(0, 0, 0);
            state = 1;
        }
        else if (aykaDirX < 0f)
        {
            //sR.flipX = true;
            aykaTransform.rotation = Quaternion.Euler(0, 180, 0);
            state = 1;
        }
        else
        {
            //sR.flipX = false;
            state = 0;
        }
        aykaAnimator.SetInteger("state", state);
    }

    void ConnieUpdateAnimation()
    {
        int state;
        if (connieDirX > 0f)
        {
            //sR.flipX = false;
            connieTransform.rotation = Quaternion.Euler(0, 0, 0);
            state = 1;
        }
        else if (connieDirX < 0f)
        {
            //sR.flipX = true;
            connieTransform.rotation = Quaternion.Euler(0, 180, 0);
            state = 1;
        }
        else
        {
            //sR.flipX = false;
            state = 0;
        }
        connieAnimator.SetInteger("state", state);
    }

    void DisplayCompleteDialogueUI()
    {
        dialogueBoxAnimator.SetInteger("state", 1);
        continueDialogueButtonAnimator.SetInteger("state", 1);
        continueDialogueButtonAnimator.gameObject.GetComponent<Button>().interactable = true;
    }

    void HideCompleteDialogueUI()
    {
        dialogueBoxAnimator.SetInteger("state", 2);
        continueDialogueButtonAnimator.gameObject.GetComponent<Button>().interactable = false;
        continueDialogueButtonAnimator.SetInteger("state", 2);
    }

    public void OnContinueDialogue()
    {
        continueDialogue = true;
    }

    void MoveConnieToLimitOfCamera()
    {
        connieTransform.position = new Vector2((Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x) + 1f, connieRb2d.gameObject.transform.position.y);
    }

    private Dialogue aykaMonologue;
    private Dialogue connieDialogue1;

    void FeedDialoguesArrays()
    {
        aykaMonologue = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "¡Hola querido jugador!", "¿Cómo estás?..." });
        connieDialogue1 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "AAAAAAAAAAAA", "¡Señor zorro por favor ayúdeme!" });
    }
}
