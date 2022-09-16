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
    private AudioSource aykaVoice;

    [Header("Connie Properties")]
    public Sprite connieSpriteImage;
    public GameObject conniePreFab;
    private Rigidbody2D connieRb2d;
    private Transform connieTransform;
    private Animator connieAnimator;
    public float connieMovementSpeed;
    private AudioSource connieVoice;

    [Header("Extra Characters GameObjects")]
    public Sprite opossumSpriteImage;
    public GameObject opossumPreFab;
    private Rigidbody2D opossumRb2d;
    private Transform opossumTransform;
    private Animator opossumAnimator;
    public float opossumMovementSpeed;
    private bool opossumNotDead = true;
    private AudioSource opossumVoice;

    [Header("Dialogue Box Properties")]
    public Animator dialogueBoxAnimator;
    public Animator continueDialogueButtonAnimator;
    public Image dialogueBoxCharacterImage;
    public TMP_Text dialogueBoxCharacterName;
    public TMP_Text dialogueBoxSentenceBox;

    [Header("Ejercicio")]
    public GameObject blackOverlay;
    public GameObject associationExercise;
    public GameObject connieHelperIndicatorOverlay;
    public GameObject connieHelpIndicatorTutorial;
    public GameObject totalBlackOverlay;

    [Header("General Level Properties")]
    [HideInInspector] public bool firstTime = true;
    [HideInInspector] public bool firstTimeExerciseTutorial = true;
    public LevelCarrotCounter lvlCarrotCounter;
    public Button lvlPauseButton;
    public Button lvlConnieHelperButton;
    [Range(1,5)] public int dialogueSoundFrequencyLevel;
    public float dialogueTimeBetweenLetters;
    [Range(-3, 3)] public float dialogueMinPitch;
    [Range(-3, 3)] public float dialogueMaxPitch;
    public LevelCameraController levelCameraController;

    //private bool inDialogue = false;
    private Camera mainCamera;
    private bool continueDialogue = false;
    private int index = 0;
    private IEnumerator typeSentenceCoroutine;
    private bool isTypeSentenceCoroutineRunning;
    private int aykaDirX = 0;
    private bool aykaDizzy = false;
    private int connieDirX = 0;
    private bool connieHurt = false;
    private int opossumDirX = 0;

    private void Start()
    {
        mainCamera = Camera.main;
        FeedDialoguesArrays();
        //MoveCharactersToLimitOfCamera();
        aykaVoice = AudioManager.instance.characterVoices[0].source;
        connieVoice = AudioManager.instance.characterVoices[1].source;
        opossumVoice = AudioManager.instance.characterVoices[2].source;
        associationExercise.GetComponent<AssociationExercise>().OnErrorTutorial += StartSecondCinematic;
        associationExercise.GetComponent<AssociationExercise>().OnWinTutorial += StartWinCinematic;
        associationExercise.GetComponent<AssociationExercise>().OnError += StartErrorCinematic;
        StartCoroutine(Level01Cinematic());
    }

    private void FixedUpdate()
    {
        if (aykaRb2d)
        {
            aykaRb2d.velocity = new Vector2(aykaDirX * aykaMovementSpeed * Time.fixedDeltaTime, aykaRb2d.velocity.y);
            AykaUpdateAnimation();
        }
        if (connieRb2d)
        {
            connieRb2d.velocity = new Vector2(connieDirX * connieMovementSpeed * Time.fixedDeltaTime, connieRb2d.velocity.y);
            ConnieUpdateAnimation();
        }
        if (opossumRb2d)
        {
            opossumRb2d.velocity = new Vector2(opossumDirX * connieMovementSpeed * Time.fixedDeltaTime, opossumRb2d.velocity.y);
            OpossumUpdateAnimation();
        }
    }

    public IEnumerator Level01Cinematic()
    {
        blackOverlay.SetActive(false); associationExercise.SetActive(false); totalBlackOverlay.SetActive(false);
        connieHelperIndicatorOverlay.SetActive(false); connieHelpIndicatorTutorial.SetActive(false);
        lvlCarrotCounter.gameObject.SetActive(false); lvlPauseButton.gameObject.SetActive(false); lvlConnieHelperButton.gameObject.SetActive(false);
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
        InstantiateAndSetConniePreFab();
        yield return new WaitForSeconds(.8f);
        connieDirX = -1;
        while (connieTransform.position.x > 1.2f) yield return null;
        connieDirX = 0;
        connieRb2d.AddForce(Vector2.up * 200);
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
        HideCompleteDialogueUI();
        /* FINISHING CONNIE DIALOGUE 2 */
        /* OPOSSUM SCARES CONNIE THEN CONNIE GOES BEHIND AYKA AND CAMERA MOVES TO CENTER BOTH */
        InstatiateAndSetOpossumPreFab();
        yield return new WaitForSeconds(.8f); opossumDirX = -1;
        while (opossumTransform.position.x > 2.8f) { yield return null; } opossumDirX = 0;
        yield return new WaitForSeconds(.3f); connieTransform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(0.5f); connieRb2d.AddForce(Vector2.up * 200);
        yield return new WaitForSeconds(0.6f); connieDirX = -1;
        while (connieTransform.position.x > -1.3f) yield return null; connieDirX = 0; connieTransform.rotation = Quaternion.Euler(0, 0, 0);
        /**/
        /* OPOSSUM  */
        DisplayCompleteDialogueUI(); continueDialogue = false;
        DisplayDialogueSentence(opossumDialogue1, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue1.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index++;
        DisplayDialogueSentence(opossumDialogue1, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue1.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index = 0;
        DisplayDialogueSentence(connieDialogue3, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue3.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index++;
        DisplayDialogueSentence(connieDialogue3, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue3.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index = 0;
        DisplayDialogueSentence(aykaDialogue2, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue2.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index++;
        DisplayDialogueSentence(aykaDialogue2, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue2.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index = 0;
        DisplayDialogueSentence(connieDialogue4, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue4.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index++;
        DisplayDialogueSentence(connieDialogue4, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue4.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index = 0;
        /**/
        HideCompleteDialogueUI();
        yield return new WaitForSeconds(0.8f); lvlCarrotCounter.gameObject.SetActive(true); lvlPauseButton.gameObject.SetActive(true);
        blackOverlay.SetActive(true); associationExercise.SetActive(true);
    }

    public IEnumerator Level01Cinematic2()
    {
        firstTime = false;
        yield return new WaitForSeconds(0.8f);
        blackOverlay.GetComponent<Animator>().SetInteger("state", 1); associationExercise.GetComponent<Animator>().SetInteger("state", 1);
        /* Poner mareado a Ayka */
        yield return new WaitForSeconds(1f); aykaDizzy = true;
        /* AYKA DIALOGUE 3 */
        DisplayCompleteDialogueUI(); continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue3, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue3.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue3, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue3.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index = 0;
        /**/
        /* Connie yells that opossum is going to steal a carrot from her */
        DisplayDialogueSentence(connieDialogue5, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue5.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(connieDialogue5, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue5.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index = 0;
        DisplayDialogueSentence(opossumDialogue2, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue2.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index++;
        DisplayDialogueSentence(opossumDialogue2, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue2.sentences[index];
            while (!continueDialogue) yield return null;
        }
        continueDialogue = false; index = 0;
        HideCompleteDialogueUI();
        /* MOVE OPOSSUM */
        yield return new WaitForSeconds(0.8f);
        opossumDirX = -1; while (opossumTransform.position.x > aykaTransform.position.x - 0.1f) { yield return null; } opossumDirX = 0;
        connieHurt = true; yield return new WaitForSeconds(0.5f); connieHurt = false;
        opossumDirX = 1; while (opossumTransform.position.x < 2.6f) { yield return null; } opossumDirX = 0; opossumTransform.rotation = Quaternion.Euler(0, 0, 0);
        aykaDizzy = false;
        DisplayCompleteDialogueUI(); continueDialogue = false;
        DisplayDialogueSentence(opossumDialogue3, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue3.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(opossumDialogue3, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue3.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;
        DisplayDialogueSentence(connieDialogue6, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue6.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(connieDialogue6, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue6.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;
        DisplayDialogueSentence(opossumDialogue4, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue4.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(opossumDialogue4, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue4.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue4, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue4.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue4, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue4.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;
        DisplayDialogueSentence(connieDialogue7, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue7.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(connieDialogue7, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue7.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;
        /***********/
        HideCompleteDialogueUI();
        yield return new WaitForSeconds(0.8f);
        blackOverlay.SetActive(true); associationExercise.SetActive(true);
        yield return new WaitForSeconds(0.5f); lvlConnieHelperButton.gameObject.SetActive(true);
        connieHelperIndicatorOverlay.SetActive(true); connieHelpIndicatorTutorial.SetActive(true);
        // Activar canvas screens que contengan info sobre el valor y los nombres de las figuras musicales.
    }

    public IEnumerator Level01Cinematic3Win()
    {
        firstTimeExerciseTutorial = false;
        yield return new WaitForSeconds(0.5f); lvlConnieHelperButton.gameObject.SetActive(false);
        blackOverlay.GetComponent<Animator>().SetInteger("state", 1); associationExercise.GetComponent<Animator>().SetInteger("state", 1);
        yield return new WaitForSeconds(1.8f);
        DisplayCompleteDialogueUI();
        DisplayDialogueSentence(opossumDialogue5, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue5.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(opossumDialogue5, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue5.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(opossumDialogue5, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue5.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue5, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue5.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;
        DisplayDialogueSentence(opossumDialogue6, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = opossumDialogue6.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;
        HideCompleteDialogueUI(); yield return new WaitForSeconds(1f);
        opossumNotDead = false;
        yield return new WaitForSeconds(1.5f);
        DisplayCompleteDialogueUI();
        DisplayDialogueSentence(aykaDialogue6, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue6.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue6, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue6.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue6, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue6.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;

        DisplayDialogueSentence(connieDialogue8, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue8.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(connieDialogue8, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue8.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;

        DisplayDialogueSentence(aykaDialogue7, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue7.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue7, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue7.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue7, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue7.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue7, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue7.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue7, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue7.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;

        DisplayDialogueSentence(connieDialogue9, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue9.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(connieDialogue9, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue9.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(connieDialogue9, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = connieDialogue9.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;

        DisplayDialogueSentence(aykaDialogue8, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue8.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index++; continueDialogue = false;
        DisplayDialogueSentence(aykaDialogue8, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue8.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;

        DisplayDialogueSentence(connieDialogue10, index);
        while (!continueDialogue) yield return null;
        if (isTypeSentenceCoroutineRunning)
        {
            continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
            dialogueBoxSentenceBox.text = aykaDialogue8.sentences[index];
            while (!continueDialogue) yield return null;
        }
        index = 0; continueDialogue = false;
        HideCompleteDialogueUI();
        totalBlackOverlay.SetActive(true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartPlaying());
    }

    public IEnumerator StartPlaying()
    {
        // aca empezamos a mover a los personajes y tal, los detenemos hasta llegar a las zarigueyas
        totalBlackOverlay.GetComponent<Animator>().SetInteger("state", 1);
        yield return new WaitForSeconds(1.2f);
        LevelDynamicGenerator levelDynamicGen = GetComponent<LevelDynamicGenerator>();
        levelCameraController.gameObject.SetActive(true);
        aykaDirX = 1; connieDirX = 1;
        float[] enemyPositionsArray = levelDynamicGen.GetEnemyPositionsArray();
        for (int i = 0; i < enemyPositionsArray.Length; i++)
        {
            while (aykaTransform.position.x < enemyPositionsArray[i] - 1.5) yield return null;

        }
    }

    public IEnumerator Level01CinematicError()
    {
        // ocultar el ui de juego y hacer que la zarigueya robe una zanahoria
        yield return null;
    }

    void StartSecondCinematic() { StartCoroutine(Level01Cinematic2()); }
    void StartWinCinematic() { StartCoroutine(Level01Cinematic3Win()); }
    void StartErrorCinematic() { StartCoroutine(Level01CinematicError()); }
    public void HideConnieHelpIndicator() { connieHelperIndicatorOverlay.SetActive(false); connieHelpIndicatorTutorial.SetActive(false); }

    void DisplayDialogueSentence(Dialogue dialogue, int index)
    {
        dialogueBoxCharacterImage.sprite = dialogue.characterImage;
        dialogueBoxCharacterName.text = dialogue.characterName;
        //dialogueBoxSentenceBox.text = dialogue.sentences[index];
        typeSentenceCoroutine = TypeSentence( dialogue.sentences[index], dialogueTimeBetweenLetters, dialogue.characterName );
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
                if (letterCount % dialogueSoundFrequencyLevel == 0) aykaVoice.Play();
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
                if (letterCount % dialogueSoundFrequencyLevel == 0) connieVoice.Play();
                dialogueBoxSentenceBox.text += letter;
                yield return new WaitForSeconds(timeBetweenLetters);
                letterCount++;
            }
        }
        else if (characterName == "Zarigüeya")
        {
            foreach (char letter in sentenceCharArray)
            {
                opossumVoice.pitch = Random.Range(dialogueMinPitch, dialogueMaxPitch);
                if (letterCount % dialogueSoundFrequencyLevel == 0) opossumVoice.Play();
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
        else if (aykaDizzy) { state = 2; }
        else state = 0;
        aykaAnimator.SetInteger("state", state);
    }

    void ConnieUpdateAnimation()
    {
        int state;
        if (connieDirX > 0f) { connieTransform.rotation = Quaternion.Euler(0, 0, 0); state = 1; }
        else if (connieDirX < 0f) { connieTransform.rotation = Quaternion.Euler(0, 180, 0); state = 1; }
        else if (connieHurt) { state = 2; }
        else state = 0;
        connieAnimator.SetInteger("state", state);
    }

    void OpossumUpdateAnimation()
    {
        int state;
        if (opossumNotDead)
        {
            if (opossumDirX > 0f) { opossumTransform.rotation = Quaternion.Euler(0, 180, 0); state = 1; }
            else if (opossumDirX < 0f) { opossumTransform.rotation = Quaternion.Euler(0, 0, 0); state = 1; }
            else state = 0;
        }
        else state = 2;
        opossumAnimator.SetInteger("state", state);
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

    void MoveCharactersToLimitOfCamera() 
    { 
        connieTransform.position = new Vector2((mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x) + 1f, connieRb2d.gameObject.transform.position.y);
        opossumTransform.position = new Vector2((mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x) + 2f, opossumRb2d.gameObject.transform.position.y);
    }

    void InstantiateAndSetConniePreFab()
    {
        GameObject connieGO = Instantiate(
            conniePreFab,
            new Vector2((mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x) + 1f, conniePreFab.transform.position.y),
            Quaternion.identity
            );
        connieRb2d = connieGO.GetComponent<Rigidbody2D>();
        connieTransform = connieGO.transform;
        connieAnimator = connieGO.GetComponent<Animator>();
    }

    void InstatiateAndSetOpossumPreFab()
    {
        GameObject opossumGO = Instantiate(
            opossumPreFab,
            new Vector2((mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x) + 1f, opossumPreFab.transform.position.y),
            Quaternion.identity
            );
        opossumRb2d = opossumGO.GetComponent<Rigidbody2D>();
        opossumTransform = opossumGO.transform;
        opossumAnimator = opossumGO.GetComponent<Animator>();
    }

    private Dialogue aykaMonologue;
    private Dialogue connieDialogue1;
    private Dialogue aykaDialogue1;
    private Dialogue connieDialogue2;
    private Dialogue opossumDialogue1;
    private Dialogue connieDialogue3;
    private Dialogue aykaDialogue2;
    private Dialogue connieDialogue4;
    private Dialogue aykaDialogue3;
    private Dialogue connieDialogue5;
    private Dialogue opossumDialogue2;
    private Dialogue opossumDialogue3;
    private Dialogue connieDialogue6;
    private Dialogue opossumDialogue4;
    private Dialogue aykaDialogue4;
    private Dialogue connieDialogue7;
    private Dialogue opossumDialogue5;
    private Dialogue aykaDialogue5;
    private Dialogue opossumDialogue6;
    private Dialogue aykaDialogue6;
    private Dialogue connieDialogue8;
    private Dialogue aykaDialogue7;
    private Dialogue connieDialogue9;
    private Dialogue aykaDialogue8;
    private Dialogue connieDialogue10;

    void FeedDialoguesArrays()
    {
        aykaMonologue = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "¡Que bonito día!", "Espero nadie me moleste..." });
        connieDialogue1 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "AAAAAAAAAAAA", "¡Señor zorro por favor ayúdeme!" });
        aykaDialogue1 = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "¿Qué pasa coneja?", "¿A qué vienen esas prisas?" });
        connieDialogue2 = new Dialogue("Connie", connieSpriteImage, new string[3]{
            "Estaba recolectando zanahorias y...", "¡Los animales malvados me las quieren robar!",
            "Tiene que ayudarme por favor"});
        opossumDialogue1 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[2]{
            "Vamos coneja...", "No hagas esto más difícil..." });
        connieDialogue3 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "¡Él es de los malos!", "¿Me ayudará señor zorro?" });
        aykaDialogue2 = new Dialogue("Ayka", aykaSpriteImage, new string[2]{ 
            "Está bien, te ayudaré", "Pero no recuerdo bien los conceptos musicales" });
        connieDialogue4 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "Usted tranquilo, yo nerviosa", "¡Lo ayudaré!" });
        aykaDialogue3 = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "Oh no, ¡Fallé!", "Ahora me siento algo mareado..." });
        connieDialogue5 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "Nooo ¡Señor zorro!", "¡Ahora él podrá robarme una zanahoria!" });
        opossumDialogue2 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[2]{
            "Muajajaja", "Esto será pan comido..." });
        opossumDialogue3 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[2]{
            "¿Qué?", "¿Una roca?" });
        connieDialogue6 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "¡Ah! ¡Qué suerte!", "¡Robaste una roca que tenía guardada! jajaja" });
        opossumDialogue4 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[2]{
            "Coneja suertuda...", "No importa, ¡aún no resuelven mi ejercicio!" });
        aykaDialogue4 = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "¡Es cierto!", "Ahora sí coneja, echame una mano" });
        connieDialogue7 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "Sí señor zorro", "¡Vamos allá!" });
        opossumDialogue5 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[3]{
            "Resolvieron mi ejercicio...", "Siento que...", "debo irme..." });
        aykaDialogue5 = new Dialogue("Ayka", aykaSpriteImage, new string[1]{
            "Ehh... ¿irte?" });
        opossumDialogue6 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[1]{
            "adiós" });
        aykaDialogue6 = new Dialogue("Ayka", aykaSpriteImage, new string[3]{
            "¡Oh! Se fue", "Parece que resolviendo sus ejercicios te dejan en paz", "Bueno... ¡mucha suerte coneja! Adiós" });
        connieDialogue8 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "¡Noo! no puede dejarme señor zorro", "Le pido que me ayude a volver a mi casa con mi madre" });
        aykaDialogue7 = new Dialogue("Ayka", aykaSpriteImage, new string[5]{
            "primero, ya basta con lo de señor zorro...", "Me llamo Ayka", "Y te acompaño si me pagas con...", "¡cerezas!", "Me gustan las cerezas jeje" });
        connieDialogue9 = new Dialogue("Connie", connieSpriteImage, new string[3]{
            "Un gusto Ayka, yo soy Connie", "Y está bien, tengo cerezas en casa y si me llevas...", "¡Todas son para ti!" });
        aykaDialogue8 = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "¡Genial! ya nos vamos entendiendo", "Así que... ¡Pongámonos en marcha!" });
        connieDialogue10 = new Dialogue("Connie", connieSpriteImage, new string[1]{
            "¡Vamos!" });
    }
}