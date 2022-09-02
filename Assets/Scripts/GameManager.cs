using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public bool isFirstTimePlaying = false;
    [HideInInspector] public int activeLevel = 0;
    [HideInInspector] public int carrotsPerLevel = 8;
    [HideInInspector] public float transitionsTime = 1f;
    [HideInInspector] public GameObject loadingScreenGameObject;

    public GameLevel[] gameLevelList;
    

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
        FeedGameLevelList();
    }

    private void Start()
    {
        if (isFirstTimePlaying) LoadSceneByName("Level0");
        else LoadSceneByName("GameMap");
    }

    void FeedGameLevelList()
    {
        // if game doesnt have saved info...
        gameLevelList = new GameLevel[2]
        {
            new GameLevel("Nivel 0", "Introducción", carrotsPerLevel, carrotsPerLevel, true, false),
            new GameLevel("Nivel 1", "Equivalencias", carrotsPerLevel, carrotsPerLevel, false, false)
        };
    }

    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadSceneByNameAsynchronously(sceneName));
    }

    IEnumerator LoadSceneByNameAsynchronously(string sceneName)
    {
        GameObject levelLoader = GameObject.Find("LevelLoader");
        if (levelLoader) { levelLoader.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Start"); } //loadingScreen = levelLoader.transform.Find("LoadingScreen").gameObject;
        yield return new WaitForSeconds(transitionsTime);
        if (loadingScreenGameObject) loadingScreenGameObject.SetActive(true);

        Slider progressBar = loadingScreenGameObject.transform.Find("ProgressBar").GetComponent<Slider>();
        TMP_Text progressText = loadingScreenGameObject.transform.Find("ProgressText").GetComponent<TMP_Text>();

        AsyncOperation loadingOperation =  SceneManager.LoadSceneAsync(sceneName);

        while (!loadingOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress / .9f);
            progressBar.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }
    }

    public void LoadActiveLevel()
    {
        if (gameLevelList[activeLevel].unlocked)
        {
            StartCoroutine(LoadSceneByNameAsynchronously("Level" + activeLevel));
        }
        
    }
}
