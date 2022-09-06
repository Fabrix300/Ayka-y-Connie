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

    [Header("Extra Characters GameObjects")]
    public GameObject opossumPreFab;
    private GameObject opossumGO;

    [Header("Dialogue Box Properties")]
    public Animator dialogueBoxAnimator;
    public Animator continueDialogueButtonAnimator;
    public Image dialogueBoxCharacterImage;
    public TMP_Text dialogueBoxCharacterName;
    public TMP_Text dialogueBoxSentenceBox;

    [Header("General Level Properties")]
    public LevelCarrotCounter lvlCarrotCounter;
    public Button lvlPauseButton;
    [Range(1,5)] public int dialogueSoundFrequencyLevel;
    public float dialogueTimeBetweenLetters;
    [Range(-3, 3)] public float dialogueMinPitch;
    [Range(-3, 3)] public float dialogueMaxPitch;

    //private bool inDialogue = false;
    private bool continueDialogue = false;
    private int index = 0;
    private IEnumerator typeSentenceCoroutine;
    private bool isTypeSentenceCoroutineRunning;
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
        lvlCarrotCounter.gameObject.SetActive(false);
        lvlPauseButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        /* STARTING AYKA MONOLOGUE */
        DisplayCompleteDialogueUI();   continueDialogue = false;
        DisplayDialogueSentence(aykaMonologue, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaMonologue.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++;   continueDialogue = false;
        DisplayDialogueSentence(aykaMonologue, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaMonologue.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false;   index = 0;
        HideCompleteDialogueUI();
        // CONNIE APPEARS
        yield return new WaitForSeconds(.8f);
        connieDirX = -1;
        while (connieTransform.position.x > 2f) yield return null;
        connieDirX = 0;
        /* STARTING CONNIE DIALOGUE 1 */
        DisplayCompleteDialogueUI(); continueDialogue = false;
        DisplayDialogueSentence(connieDialogue1, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue1.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(connieDialogue1, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue1.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index = 0;
        /* FINISHING CONNIE DIALOGUE 1 */
        /* STARTING AYKA DIALOGUE 1 */
        DisplayDialogueSentence(aykaDialogue1, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue1.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue1, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue1.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index = 0;
        /* FINISHING AYKA DIALOGUE 1 */
        /* STARTING CONNIE DIALOGUE 2 */
        DisplayDialogueSentence(connieDialogue2, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue2.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(connieDialogue2, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue2.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index++;
        DisplayDialogueSentence(connieDialogue2, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue2.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;
        /* FINISHING CONNIE DIALOGUE 2 */
        /* INSTANTIATE OPOSSUM AND SCARE CONNIE THEN CONNIE GOES BEHIND AYKA AND CAMERA MOVES TO CENTER BOTH */
        opossumGO = Instantiate(
            opossumPreFab,
            new Vector3((Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x) + 1f, 0f, 0f),
            Quaternion.identity
            );
        /**/
        HideCompleteDialogueUI();
        yield return new WaitForSeconds(0.8f); lvlCarrotCounter.gameObject.SetActive(true); lvlPauseButton.gameObject.SetActive(true);
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
        isTypeSentenceCoroutineRunning = true;
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
        isTypeSentenceCoroutineRunning = false;
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

    public void OnContinueDialogue() { continueDialogue = true; }

    void MoveConnieToLimitOfCamera() { connieTransform.position = new Vector2((Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x) + 1f, connieRb2d.gameObject.transform.position.y); }

    private Dialogue aykaMonologue;
    private Dialogue connieDialogue1;
    private Dialogue aykaDialogue1;
    private Dialogue connieDialogue2;

    void FeedDialoguesArrays()
    {
        aykaMonologue = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "�Que bonito d�a!", "Espero nadie me moleste..." });
        connieDialogue1 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "AAAAAAAAAAAA", "�Se�or zorro por favor ay�deme!" });
        aykaDialogue1 = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "�Qu� pasa coneja?", "A qu� vienen esas prisas..." });
        connieDialogue2 = new Dialogue("Connie", connieSpriteImage, new string[3]{
            "Estaba recolectando zanahorias y...", "�Los animales malvados me las quieren robar!",
            "Tiene que ayudarme se�or zorro, por favor :("});
    }
}
