using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level3Controller : MonoBehaviour
{
    [Header("Ayka Properties")]
    public Sprite aykaSpriteImage;
    public GameObject aykaGameObject;
    private Rigidbody2D aykaRb2d;
    private Transform aykaTransform;
    private Animator aykaAnimator;
    public float aykaMovementSpeed;
    private AudioSource aykaVoice;

    [Header("Connie Properties")]
    public Sprite connieSpriteImage;
    public GameObject connieGameObject;
    private Rigidbody2D connieRb2d;
    private Transform connieTransform;
    private Animator connieAnimator;
    public float connieMovementSpeed;
    private AudioSource connieVoice;

    [Header("Ejercicio")]
    public GameObject blackOverlay;
    public GameObject totalBlackOverlay;
    public GameObject statisticsExercise;

    [Header("General Level Properties")]
    [HideInInspector] public bool firstTime = false;
    [HideInInspector] public bool firstTimeExerciseTutorial = false;
    public LevelCarrotCounter lvlCarrotCounter;
    public Button lvlPauseButton;
    public Button lvlConnieHelperButton;
    public LevelCameraController levelCameraController;
    public EndScreensController endScreensController;
    public LevelProgressBarController lvlProgressBarController;
    public int timesConnieHelperButton;
    private LevelDynamicGenerator levelDynamicGen;
    private GameObject[] enemyGameObjectsArray;
    private int indexForEnemies = -1;

    private Camera mainCamera;
    private int aykaDirX = 0;
    private bool aykaDizzy = false;
    private int connieDirX = 0;
    private bool connieHurt = false;

    private void Start()
    {
        mainCamera = Camera.main;
        SetAykaAndConniePrivateProperties();
        levelDynamicGen = GetComponent<LevelDynamicGenerator>();
        aykaVoice = AudioManager.instance.characterVoices[0].source;
        connieVoice = AudioManager.instance.characterVoices[1].source;
        /* Events */
        StatisticsExercise statisticsExerciseComponent = statisticsExercise.GetComponent<StatisticsExercise>();
        //equivalenceExerciseComponent.OnErrorTutorial += StartSecondCinematic;
        //equivalenceExerciseComponent.OnWinTutorial += StartThirdCinematic;
        statisticsExerciseComponent.OnError += StartErrorCinematic;
        statisticsExerciseComponent.OnWin += StartWinCinematic;
        StartCoroutine(Level1Cinematic1());
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
    }

    public IEnumerator Level1Cinematic1()
    {
        Debug.Log("Here goes a nice cinematic :(");
        /* JUST FOR NOW, DELETE LATER OR MOVE TO LAST CINEMATIC */
        lvlConnieHelperButton.gameObject.SetActive(false); blackOverlay.SetActive(false); statisticsExercise.SetActive(false);
        totalBlackOverlay.SetActive(false);
        aykaGameObject.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(1.5f);
        // Move connie to ayka
        connieDirX = 1; while (connieTransform.position.x < aykaTransform.position.x - 1.3f) { yield return null; }
        connieDirX = 0; aykaGameObject.GetComponent<SpriteRenderer>().flipX = false;
        connieTransform.position = new Vector2(aykaTransform.position.x - 1.3f, connieTransform.position.y);
        yield return new WaitForSeconds(0.6f);
        levelCameraController.gameObject.SetActive(true);
        enemyGameObjectsArray = levelDynamicGen.GetEnemyGameObjectsArray();
        StartCoroutine(MoveToNextEnemy());
    }

    public IEnumerator ActivateExerciseUI()
    {
        yield return new WaitForSeconds(0.8f); blackOverlay.SetActive(true); statisticsExercise.SetActive(true);
        if (indexForEnemies < timesConnieHelperButton)
        {
            lvlConnieHelperButton.gameObject.SetActive(true);
        }
    }

    public IEnumerator MoveToNextEnemy()
    {
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

    public IEnumerator Level01ErrorAction()
    {
        // ocultar el ui de juego y hacer que el enemigo robe una zanahoria
        yield return new WaitForSeconds(0.8f);
        blackOverlay.GetComponent<Animator>().SetInteger("state", 1); statisticsExercise.GetComponent<Animator>().SetInteger("state", 1);
        if (lvlConnieHelperButton.gameObject.activeInHierarchy) { lvlConnieHelperButton.gameObject.SetActive(false); }
        /* Poner mareado a Ayka */
        yield return new WaitForSeconds(1f); aykaDizzy = true; yield return new WaitForSeconds(0.8f);
        // Traer componente del enemigo en cuestion
        EnemyController enemController = enemyGameObjectsArray[indexForEnemies].GetComponent<EnemyController>();
        Transform enemTransform = enemyGameObjectsArray[indexForEnemies].transform;
        enemController.isEnabled = true; enemController.enemyDirX = -1;
        while (enemTransform.position.x > aykaTransform.position.x - 0.1f) { yield return null; }
        enemController.enemyDirX = 0;
        connieHurt = true; yield return new WaitForSeconds(0.5f); connieHurt = false;
        lvlCarrotCounter.DiminishOneCarrot(new Vector2(connieTransform.position.x + 0.8f, connieTransform.position.y)); yield return new WaitForSeconds(0.3f);
        // Mover a la zarigueya hasta el final izquierdo de la pantalla y destruirla
        float positionXCamera = mainCamera.transform.position.x;
        float widthOfCamera = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - positionXCamera;
        enemController.enemyDirX = -1; while (enemTransform.position.x > (positionXCamera - widthOfCamera) - 1.4f) { yield return null; }
        enemController.enemyDirX = 0; enemController.enemyNotDead = false;
        aykaDizzy = false; yield return new WaitForSeconds(0.3f);
        StartCoroutine(MoveToNextEnemy());
    }

    public IEnumerator Level01WinAction()
    {
        // ocultar el ui de juego
        yield return new WaitForSeconds(0.8f); lvlConnieHelperButton.gameObject.SetActive(false);
        blackOverlay.GetComponent<Animator>().SetInteger("state", 1); statisticsExercise.GetComponent<Animator>().SetInteger("state", 1);
        if (lvlConnieHelperButton.gameObject.activeInHierarchy) { lvlConnieHelperButton.gameObject.SetActive(false); }
        yield return new WaitForSeconds(1.5f);
        // Get animator and explode enemy
        enemyGameObjectsArray[indexForEnemies].GetComponent<Animator>().SetInteger("state", 2);
        yield return new WaitForSeconds(1.2f); StartCoroutine(MoveToNextEnemy());
    }

    void StartErrorCinematic() { StartCoroutine(Level01ErrorAction()); }
    void StartWinCinematic() { StartCoroutine(Level01WinAction()); }

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

    void SetAykaAndConniePrivateProperties()
    {
        if (!aykaGameObject && !connieGameObject)
        {
            Debug.LogError("Ayka and Connie game objects not setted in inspector"); return;
        }
        aykaRb2d = aykaGameObject.GetComponent<Rigidbody2D>(); aykaTransform = aykaGameObject.transform;
        aykaAnimator = aykaGameObject.GetComponent<Animator>();
        connieRb2d = connieGameObject.GetComponent<Rigidbody2D>(); connieTransform = connieGameObject.transform;
        connieAnimator = connieGameObject.GetComponent<Animator>();
    }
}
