using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

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
    public EndScreensController endScreensController;
    public LevelProgressBarController lvlProgressBarController;
    public GameObject skipCinematicButton;
    public int timesConnieHelperButton;
    private LevelDynamicGenerator levelDynamicGen;
    private GameObject[] enemyGameObjectsArray;
    private int indexForEnemies = -1;

    private Camera mainCamera;
    private bool continueDialogue = false;
    //private int indexForDialogue = 0;
    private IEnumerator typeSentenceCoroutine;
    private IEnumerator level01Cinematic;
    private IEnumerator processDialogueGroup;
    private IEnumerator processDialogueObjectSentences;
    private bool isTypeSentenceCoroutineRunning;
    private bool mainThreadStopped = false;
    private bool secondThreadStopped = false;
    private int aykaDirX = 0;
    private bool aykaDizzy = false;
    private int connieDirX = 0;
    private bool connieHurt = false;
    private int opossumDirX = 0;

    private void Start()
    {
        mainCamera = Camera.main;
        levelDynamicGen = GetComponent<LevelDynamicGenerator>();
        FeedDialoguesArrays();
        aykaVoice = AudioManager.instance.characterVoices[0].source;
        connieVoice = AudioManager.instance.characterVoices[1].source;
        opossumVoice = AudioManager.instance.characterVoices[2].source;
        AssociationExercise associationExerciseComponent = associationExercise.GetComponent<AssociationExercise>();
        associationExerciseComponent.OnErrorTutorial += StartSecondCinematic;
        associationExerciseComponent.OnWinTutorial += StartThirdCinematic;
        associationExerciseComponent.OnError += StartErrorCinematic;
        associationExerciseComponent.OnWin += StartWinCinematic;
        level01Cinematic = Level01Cinematic();
        StartCoroutine(level01Cinematic);
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
            opossumRb2d.velocity = new Vector2(opossumDirX * opossumMovementSpeed * Time.fixedDeltaTime, opossumRb2d.velocity.y);
            OpossumUpdateAnimation();
        }
    }

    public IEnumerator Level01Cinematic()
    {
        skipCinematicButton.SetActive(false); blackOverlay.SetActive(false); associationExercise.SetActive(false);
        totalBlackOverlay.SetActive(false); connieHelperIndicatorOverlay.SetActive(false); connieHelpIndicatorTutorial.SetActive(false);
        lvlCarrotCounter.gameObject.SetActive(false); lvlPauseButton.gameObject.SetActive(false); lvlConnieHelperButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        if (GameManager.instance.gameLevelList[GameManager.instance.activeLevel].hasBeenPlayed) skipCinematicButton.SetActive(true);
        /* STARTING AYKA MONOLOGUE */
        DisplayCompleteDialogueUI();
        mainThreadStopped = true;
        processDialogueGroup = ProcessDialogueGroup(dialogueGroup1); StartCoroutine(processDialogueGroup);
        while (mainThreadStopped) { yield return null; }
        HideCompleteDialogueUI();
        // CONNIE APPEARS
        InstantiateAndSetConniePreFab(); yield return new WaitForSeconds(.8f);
        connieDirX = -1;  while (connieTransform.position.x > 1.2f) { yield return null; }
        connieDirX = 0; connieRb2d.AddForce(Vector2.up * 200);
        /* STARTING dialogueGroup2 */
        DisplayCompleteDialogueUI(); 
        mainThreadStopped = true; 
        processDialogueGroup = ProcessDialogueGroup(dialogueGroup2); StartCoroutine(processDialogueGroup);
        while (mainThreadStopped) { yield return null; }
        HideCompleteDialogueUI();
        /* HIDING SKIP CINEMATIC BUTTON */
        if (skipCinematicButton.activeInHierarchy) skipCinematicButton.GetComponent<Animator>().SetInteger("state", 1);
        /* OPOSSUM SCARES CONNIE THEN CONNIE GOES BEHIND AYKA AND CAMERA MOVES TO CENTER BOTH */
        InstatiateAndSetOpossumPreFab(); yield return new WaitForSeconds(.8f); opossumDirX = -1;
        while (opossumTransform.position.x > 2.8f) { yield return null; } opossumDirX = 0;
        yield return new WaitForSeconds(0.3f); connieTransform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(0.5f); connieRb2d.AddForce(Vector2.up * 200);
        yield return new WaitForSeconds(0.6f); connieDirX = -1;
        while (connieTransform.position.x > -1.3f) { yield return null; }
        connieDirX = 0; connieTransform.rotation = Quaternion.Euler(0, 0, 0);
        /* dialogueGroup3  */
        DisplayCompleteDialogueUI();
        mainThreadStopped = true;
        processDialogueGroup = ProcessDialogueGroup(dialogueGroup3); StartCoroutine(processDialogueGroup);
        while (mainThreadStopped) { yield return null; }
        HideCompleteDialogueUI(); yield return new WaitForSeconds(0.8f); lvlCarrotCounter.gameObject.SetActive(true);
        lvlPauseButton.gameObject.SetActive(true); blackOverlay.SetActive(true); associationExercise.SetActive(true);
    }

    public IEnumerator Level01Cinematic2()
    {
        firstTime = false;
        yield return new WaitForSeconds(0.8f);
        blackOverlay.GetComponent<Animator>().SetInteger("state", 1); associationExercise.GetComponent<Animator>().SetInteger("state", 1);
        /* Poner mareado a Ayka */ yield return new WaitForSeconds(1f); aykaDizzy = true;
        DisplayCompleteDialogueUI();
        // dialogueGroup4
        mainThreadStopped = true; 
        processDialogueGroup = ProcessDialogueGroup(dialogueGroup4); StartCoroutine(processDialogueGroup);
        while (mainThreadStopped) { yield return null; }
        HideCompleteDialogueUI();
        /* MOVE OPOSSUM */
        yield return new WaitForSeconds(0.8f);
        opossumDirX = -1; while (opossumTransform.position.x > aykaTransform.position.x - 0.1f) { yield return null; } opossumDirX = 0;
        connieHurt = true; yield return new WaitForSeconds(0.5f); connieHurt = false;
        opossumDirX = 1; while (opossumTransform.position.x < 2.6f) { yield return null; } opossumDirX = 0; opossumTransform.rotation = Quaternion.Euler(0, 0, 0);
        aykaDizzy = false;
        DisplayCompleteDialogueUI();
        // dialogueGroup5
        mainThreadStopped = true;
        processDialogueGroup = ProcessDialogueGroup(dialogueGroup5); StartCoroutine(processDialogueGroup);
        while (mainThreadStopped) { yield return null; }
        HideCompleteDialogueUI();
        yield return new WaitForSeconds(0.8f); blackOverlay.SetActive(true); associationExercise.SetActive(true);
        yield return new WaitForSeconds(0.5f); lvlConnieHelperButton.gameObject.SetActive(true);
        connieHelperIndicatorOverlay.SetActive(true); connieHelpIndicatorTutorial.SetActive(true);
    }

    public IEnumerator Level01Cinematic3Win()
    {
        firstTime = false; firstTimeExerciseTutorial = false; yield return new WaitForSeconds(0.5f);
        lvlConnieHelperButton.gameObject.SetActive(false); blackOverlay.GetComponent<Animator>().SetInteger("state", 1);
        associationExercise.GetComponent<Animator>().SetInteger("state", 1);
        yield return new WaitForSeconds(1.8f);
        DisplayCompleteDialogueUI();
        // dialogueGroup6
        mainThreadStopped = true;
        processDialogueGroup = ProcessDialogueGroup(dialogueGroup6); StartCoroutine(processDialogueGroup);
        while (mainThreadStopped) { yield return null; }
        HideCompleteDialogueUI(); yield return new WaitForSeconds(1f);
        opossumNotDead = false;
        yield return new WaitForSeconds(1.5f);
        DisplayCompleteDialogueUI(); continueDialogue = false;
        // dialogueGroup7
        mainThreadStopped = true; 
        processDialogueGroup = ProcessDialogueGroup(dialogueGroup7); StartCoroutine(processDialogueGroup);
        while (mainThreadStopped) { yield return null; }
        HideCompleteDialogueUI();
        totalBlackOverlay.SetActive(true); yield return new WaitForSeconds(1f);
        totalBlackOverlay.GetComponent<Animator>().SetInteger("state", 1); yield return new WaitForSeconds(1.2f);
        levelCameraController.gameObject.SetActive(true);
        enemyGameObjectsArray = levelDynamicGen.GetEnemyGameObjectsArray();
        StartCoroutine(MoveToNextEnemy());
    }

    public IEnumerator MoveToNextEnemy()
    {
        // chequear si es el ultimo index del array y si es pues ganas el nivel
        indexForEnemies++;
        int currentCarrots = lvlCarrotCounter.GetCurrentCarrots();
        if (currentCarrots == 0) 
        { endScreensController.ShowLoseScreen(); endScreensController.LoseSaveGameState(); }
        else if (indexForEnemies == enemyGameObjectsArray.Length) 
        {
            lvlProgressBarController.AppearLevelProgressBarAndUpdate(indexForEnemies);
            yield return new WaitForSeconds(1.2f); endScreensController.ShowWinScreen();
            endScreensController.WinSaveGameState();
        }
        else
        {
            if (indexForEnemies > 0) { lvlProgressBarController.AppearLevelProgressBarAndUpdate(indexForEnemies); }
            // aca empezamos a mover a los personajes y tal, los detenemos hasta llegar a las zarigueyas
            aykaDirX = 1; connieDirX = 1;
            float xPositionOfEnemyGameObject = enemyGameObjectsArray[indexForEnemies].transform.position.x;
            while (aykaTransform.position.x < xPositionOfEnemyGameObject - 3) yield return null;
            aykaDirX = 0; connieDirX = 0;
            // Activar ejercicio
            StartCoroutine(ActivateExerciseUI());
        }
    }

    public IEnumerator ActivateExerciseUI()
    {
        yield return new WaitForSeconds(0.8f); blackOverlay.SetActive(true); associationExercise.SetActive(true);
        if (indexForEnemies < timesConnieHelperButton)
        {
            lvlConnieHelperButton.gameObject.SetActive(true);
        }
    }

    public IEnumerator Level01ErrorAction()
    {
        // ocultar el ui de juego y hacer que la zarigueya robe una zanahoria
        yield return new WaitForSeconds(0.8f);
        blackOverlay.GetComponent<Animator>().SetInteger("state", 1); associationExercise.GetComponent<Animator>().SetInteger("state", 1);
        if (lvlConnieHelperButton.gameObject.activeInHierarchy) { lvlConnieHelperButton.gameObject.SetActive(false); }
        /* Poner mareado a Ayka */
        yield return new WaitForSeconds(1f); aykaDizzy = true; yield return new WaitForSeconds(0.8f);
        // Traer componente de la zarigueya en cuestion
        EnemyController enemController = enemyGameObjectsArray[indexForEnemies].GetComponent<EnemyController>();
        Transform enemTransform = enemyGameObjectsArray[indexForEnemies].transform;
        enemController.isEnabled = true; enemController.enemyDirX = -1;
        while (enemTransform.position.x > aykaTransform.position.x - 0.1f) { yield return null; }
        enemController.enemyDirX = 0;
        connieHurt = true; yield return new WaitForSeconds(0.5f); connieHurt = false;
        lvlCarrotCounter.DiminishOneCarrot(new Vector2(connieTransform.position.x + 0.8f, connieTransform.position.y)); yield return new WaitForSeconds(0.3f);
        // Mover a la zarigueya hasta el final izquierdo de la pantalla y destruirla (TODO)
        float positionXCamera = mainCamera.transform.position.x;
        float widthOfCamera =  mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - positionXCamera;
        enemController.enemyDirX = -1; while (enemTransform.position.x > (positionXCamera - widthOfCamera) - 1.4f) { yield return null; }
        enemController.enemyDirX = 0; enemController.enemyNotDead = false;
        aykaDizzy = false; yield return new WaitForSeconds(0.3f);
        StartCoroutine(MoveToNextEnemy());
    }

    public IEnumerator Level01WinAction()
    {
        // ocultar el ui de juego
        yield return new WaitForSeconds(0.5f); lvlConnieHelperButton.gameObject.SetActive(false);
        blackOverlay.GetComponent<Animator>().SetInteger("state", 1); associationExercise.GetComponent<Animator>().SetInteger("state", 1);
        if (lvlConnieHelperButton.gameObject.activeInHierarchy) { lvlConnieHelperButton.gameObject.SetActive(false); }
        yield return new WaitForSeconds(1.5f);
        // Get animator and explode enemy
        enemyGameObjectsArray[indexForEnemies].GetComponent<Animator>().SetInteger("state", 2);
        yield return new WaitForSeconds(1.2f); StartCoroutine(MoveToNextEnemy());
    }

    public IEnumerator SkipCoroutineCinematic(GameObject skipCinematicButton)
    {
        /*Stop the coroutines*/
        StopCoroutine(level01Cinematic); StopCoroutine(typeSentenceCoroutine); StopCoroutine(processDialogueObjectSentences);
        StopCoroutine(processDialogueGroup);
        HideCompleteDialogueUI();
        skipCinematicButton.GetComponent<Button>().interactable = false;
        skipCinematicButton.GetComponent<Animator>().SetInteger("state", 1);
        firstTime = false; firstTimeExerciseTutorial = false; continueDialogue = false;
        /* setting everithing back to normal */
        mainThreadStopped = false; secondThreadStopped = false;
        aykaDirX = 0; aykaDizzy = false; connieDirX = 0; connieHurt = false; opossumDirX = 0;
        /* ********************************* */
        totalBlackOverlay.SetActive(true);
        yield return new WaitForSeconds(1f);
        lvlCarrotCounter.gameObject.SetActive(true); lvlPauseButton.gameObject.SetActive(true);
        if (opossumTransform) Destroy(opossumTransform.gameObject);
        if (connieTransform) connieTransform.SetPositionAndRotation(new Vector2(-1.3f, connieTransform.position.y), Quaternion.Euler(0, 0, 0));
        else { InstantiateAndSetConniePreFab(); connieTransform.SetPositionAndRotation(new Vector2(-1.3f, connieTransform.position.y), Quaternion.Euler(0, 0, 0)); }
        totalBlackOverlay.GetComponent<Animator>().SetInteger("state", 1); yield return new WaitForSeconds(1.2f);
        levelCameraController.gameObject.SetActive(true);
        enemyGameObjectsArray = levelDynamicGen.GetEnemyGameObjectsArray();
        StartCoroutine(MoveToNextEnemy());
    }

    void StartSecondCinematic() { StartCoroutine(Level01Cinematic2()); }
    void StartThirdCinematic() { StartCoroutine(Level01Cinematic3Win()); }
    void StartErrorCinematic() { StartCoroutine(Level01ErrorAction()); }
    void StartWinCinematic() { StartCoroutine(Level01WinAction()); }
    public void HideConnieHelpIndicator() { connieHelperIndicatorOverlay.SetActive(false); connieHelpIndicatorTutorial.SetActive(false); }
    public void SkipCinematic(GameObject skipCinematicButton)
    {
        GameObject gO = skipCinematicButton; StartCoroutine(SkipCoroutineCinematic(gO));
    }

    IEnumerator ProcessDialogueGroup(Dialogue[] dialogueGroup)
    {
        for (int i = 0; i < dialogueGroup.Length; i++)
        {
            continueDialogue = false; secondThreadStopped = true;
            processDialogueObjectSentences = ProcessDialogueObjectSentences(dialogueGroup[i]);
            StartCoroutine(processDialogueObjectSentences);
            while (secondThreadStopped) { yield return null; }
        }
        mainThreadStopped = false;
    }

    IEnumerator ProcessDialogueObjectSentences(Dialogue dialogue)
    {
        continueDialogue = false;
        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            DisplayDialogueSentence(dialogue, i);
            while (!continueDialogue) yield return null;
            if (isTypeSentenceCoroutineRunning)
            {
                continueDialogue = false; StopCoroutine(typeSentenceCoroutine);
                dialogueBoxSentenceBox.text = dialogue.sentences[i];
                while (!continueDialogue) yield return null;
            }
            continueDialogue = false;
        }
        secondThreadStopped = false;
    }

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
        continueDialogueButtonAnimator.SetInteger("state", 2);
        continueDialogueButtonAnimator.gameObject.GetComponent<Button>().interactable = false;
    }

    public void OnContinueDialogue() { continueDialogue = true; }
    // Creo que esta funcion la usa el slider que cuenta los enemigos que vas venciendo.
    public int GetIndexForEnemies() { return indexForEnemies; }

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

    private Dialogue[] dialogueGroup1;
    private Dialogue[] dialogueGroup2;
    private Dialogue[] dialogueGroup3;
    private Dialogue[] dialogueGroup4;
    private Dialogue[] dialogueGroup5;
    private Dialogue[] dialogueGroup6;
    private Dialogue[] dialogueGroup7;

    void FeedDialoguesArrays()
    {
        Dialogue aykaMonologue = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "¡Que bonito día!", "Espero nadie me moleste..." });
        dialogueGroup1 = new Dialogue[1] { aykaMonologue };
        /* ------------------------------------------------------- */
        Dialogue connieDialogue1 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "AAAAAAAAAAAA", "¡Señor zorro por favor ayúdeme!" });
        Dialogue aykaDialogue1 = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "¿Qué pasa coneja?", "¿A qué vienen esas prisas?" });
        Dialogue connieDialogue2 = new Dialogue("Connie", connieSpriteImage, new string[3]{
            "Estaba recolectando zanahorias y...", "¡Los animales malvados me las quieren robar!",
            "Tiene que ayudarme por favor"});
        dialogueGroup2 = new Dialogue[3] { connieDialogue1, aykaDialogue1, connieDialogue2 };
        /* ------------------------------------------------------- */
        Dialogue opossumDialogue1 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[2]{
            "Vamos coneja...", "No hagas esto más difícil..." });
        Dialogue connieDialogue3 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "¡Él es de los malos!", "¿Me ayudará señor zorro?" });
        Dialogue aykaDialogue2 = new Dialogue("Ayka", aykaSpriteImage, new string[2]{ 
            "Está bien, te ayudaré", "Pero no recuerdo bien los conceptos musicales" });
        Dialogue connieDialogue4 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "Usted tranquilo, yo nerviosa", "¡Lo ayudaré!" });
        dialogueGroup3 = new Dialogue[4] { opossumDialogue1, connieDialogue3, aykaDialogue2, connieDialogue4 };
        /* ------------------------------------------------------- */
        Dialogue aykaDialogue3 = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "Oh no, ¡Fallé!", "Ahora me siento algo mareado..." });
        Dialogue connieDialogue5 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "Nooo ¡Señor zorro!", "¡Ahora él podrá robarme una zanahoria!" });
        Dialogue opossumDialogue2 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[2]{
            "Muajajaja", "Esto será pan comido..." });
        dialogueGroup4 = new Dialogue[3] { aykaDialogue3, connieDialogue5, opossumDialogue2 };
        /* ------------------------------------------------------- */
        Dialogue opossumDialogue3 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[2]{
            "¿Qué?", "¿Una roca?" });
        Dialogue connieDialogue6 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "¡Ah! ¡Qué suerte!", "¡Robaste una roca que tenía guardada! jajaja" });
        Dialogue opossumDialogue4 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[2]{
            "Coneja suertuda...", "No importa, ¡aún no resuelven mi ejercicio!" });
        Dialogue aykaDialogue4 = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "¡Es cierto!", "Ahora sí coneja, echame una mano" });
        Dialogue connieDialogue7 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "Sí señor zorro", "¡Vamos allá!" });
        dialogueGroup5 = new Dialogue[5] { opossumDialogue3, connieDialogue6, opossumDialogue4, aykaDialogue4, connieDialogue7 };
        /* ------------------------------------------------------- */
        Dialogue opossumDialogue5 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[3]{
            "Resolvieron mi ejercicio...", "Siento que...", "debo irme..." });
        Dialogue aykaDialogue5 = new Dialogue("Ayka", aykaSpriteImage, new string[1]{
            "Ehh... ¿irte?" });
        Dialogue opossumDialogue6 = new Dialogue("Zarigüeya", opossumSpriteImage, new string[1]{
            "adiós" });
        dialogueGroup6 = new Dialogue[3] { opossumDialogue5, aykaDialogue5, opossumDialogue6 };
        /* ------------------------------------------------------- */
        Dialogue aykaDialogue6 = new Dialogue("Ayka", aykaSpriteImage, new string[3]{
            "¡Oh! Se fue", "Parece que resolviendo sus ejercicios te dejan en paz", "Bueno... ¡mucha suerte coneja! Adiós" });
        Dialogue connieDialogue8 = new Dialogue("Connie", connieSpriteImage, new string[2]{
            "¡Noo! no puede dejarme señor zorro", "Le pido que me ayude a volver a mi casa con mi madre" });
        Dialogue aykaDialogue7 = new Dialogue("Ayka", aykaSpriteImage, new string[5]{
            "primero, ya basta con lo de señor zorro...", "Me llamo Ayka", "Y te acompaño si me pagas con...", "¡cerezas!", "Me gustan las cerezas jeje" });
        Dialogue connieDialogue9 = new Dialogue("Connie", connieSpriteImage, new string[3]{
            "Un gusto Ayka, yo soy Connie", "Y está bien, tengo cerezas en casa y si me llevas...", "¡Todas son para ti!" });
        Dialogue aykaDialogue8 = new Dialogue("Ayka", aykaSpriteImage, new string[2]{
            "¡Genial! ya nos vamos entendiendo", "Así que... ¡Pongámonos en marcha!" });
        Dialogue connieDialogue10 = new Dialogue("Connie", connieSpriteImage, new string[1]{
            "¡Vamos!" });
        dialogueGroup7 = new Dialogue[6]
        {
           aykaDialogue6, connieDialogue8, aykaDialogue7, connieDialogue9, aykaDialogue8, connieDialogue10
        };
        /* ------------------------------------------------------- */
    }
}