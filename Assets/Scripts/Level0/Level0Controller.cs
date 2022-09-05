using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level0Controller : MonoBehaviour
{
    [Header("Ayka Properties")]
    public Sprite aykaSpriteImage;
    public Rigidbody2D aykaRb2d;
    public Transform aykaTransform;
    public Animator aykaAnimator;
    public float aykaMovementSpeed;
    public AudioSource aykaVoice;

    [Header("Connie Properties")]
    public Sprite connieSpriteImage;
    public Rigidbody2D connieRb2d;
    public Transform connieTransform;
    public Animator connieAnimator;
    public float connieMovementSpeed;
    public AudioSource connieVoice;

    [Header("Dialogue Box Properties")]
    public Animator dialogueBoxAnimator;
    public Animator continueDialogueButtonAnimator;
    public Image dialogueBoxCharacterImage;
    public TMP_Text dialogueBoxCharacterName;
    public TMP_Text dialogueBoxSentenceBox;

    [Header("General Level Properties")]
    public LevelCarrotCounter lvlCarrotCounter;
    [Range(1,5)] public int dialogueSoundFrequencyLevel;
    public float dialogueTimeBetweenLetters;
    [Range(-3, 3)] public float dialogueMinPitch;
    [Range(-3, 3)] public float dialogueMaxPitch;

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
        aykaVoice = AudioManager.instance.characterVoices[0].source;
        connieVoice = AudioManager.instance.characterVoices[1].source;
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
        typeSentenceCoroutine = TypeSentence(
            dialogue.sentences[index], 
            dialogueTimeBetweenLetters, 
            dialogue.characterName
            );
        StartCoroutine(typeSentenceCoroutine);
    }

    IEnumerator TypeSentence(string sentence, float timeBetweenLetters, string characterName)
    {
        dialogueBoxSentenceBox.text = "";
        int letterCount = 0;
        char[] sentenceCharArray = sentence.ToCharArray();
        if (characterName == "Ayka")
        {
            foreach (char letter in sentenceCharArray)
            {
                aykaVoice.pitch = Random.Range(dialogueMinPitch, dialogueMaxPitch);
                if (letterCount % dialogueSoundFrequencyLevel == 0)
                {
                    aykaVoice.Play();
                }
                dialogueBoxSentenceBox.text += letter;
                yield return new WaitForSeconds(timeBetweenLetters);
                letterCount++;
            }
        }
        else if (characterName == "Connie")
        {
            foreach (char letter in sentenceCharArray)
            {
                connieVoice.pitch = Random.Range(dialogueMinPitch, dialogueMaxPitch);
                if (letterCount % dialogueSoundFrequencyLevel == 0)
                {
                    connieVoice.Play();
                }
                dialogueBoxSentenceBox.text += letter;
                yield return new WaitForSeconds(timeBetweenLetters);
                letterCount++;
            }
        }
    }

    void AykaUpdateAnimation()
    {
        int state;
        if (aykaDirX > 0f) { aykaTransform.rotation = Quaternion.Euler(0, 0, 0); state = 1; }
        else if (aykaDirX < 0f) { aykaTransform.rotation = Quaternion.Euler(0, 180, 0); state = 1; }
        else state = 0;
        aykaAnimator.SetInteger("state", state);
    }

    void ConnieUpdateAnimation()
    {
        int state;
        if (connieDirX > 0f) { connieTransform.rotation = Quaternion.Euler(0, 0, 0); state = 1; }
        else if (connieDirX < 0f) { connieTransform.rotation = Quaternion.Euler(0, 180, 0); state = 1; }
        else state = 0;
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
            "¡Que bonito día!", "Espero nadie me moleste..." });
        connieDialogue1 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "AAAAAAAAAAAA", "¡Señor zorro por favor ayúdeme!" });
    }
}
