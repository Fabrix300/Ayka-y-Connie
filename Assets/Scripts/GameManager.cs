using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isFirstTimePlaying;
    public int activeLevel;
    public int enemiesPerLevel;
    public int carrotLivesPerLevel;
    public string persistentPath;

    public GameLevel[] gameLevelList;

    private readonly float transitionsTime = 1f;
    //public GameObject loadingScreenGameObject;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "saveData.json";
        FeedGameLevelList();
    }

    private void Start()
    {
        //FeedGameLevelList();
        if (isFirstTimePlaying) { LoadSceneByName("Level0"); }
        else { LoadSceneByName("GameMap"); }
    }

    void FeedGameLevelList()
    {
        // if game does have saved info...
        if (System.IO.File.Exists(persistentPath))
        {
            //Debug.Log("Hay datos");
            LoadData();
        }
        else
        {
            //Debug.Log("No hay datos");
            gameLevelList = new GameLevel[2]
            {
                new GameLevel("Nivel 0", "Introducción", carrotLivesPerLevel, carrotLivesPerLevel, true, false),
                new GameLevel("Nivel 1", "Equivalencias", carrotLivesPerLevel, carrotLivesPerLevel, false, false)
            };
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadSceneByNameAsynchronously(sceneName));
    }

    IEnumerator LoadSceneByNameAsynchronously(string sceneName)
    {
        GameObject levelLoader = GameObject.Find("LevelLoader");
        if (!levelLoader) { Debug.LogWarning("No 'levelLoader' game object available in current scene."); yield break; }
        levelLoader.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(transitionsTime);
        GameObject loadingScreenGameObject = levelLoader.transform.GetChild(1).gameObject;
        loadingScreenGameObject.SetActive(true);

        Slider progressBar = loadingScreenGameObject.transform.Find("ProgressBar").GetComponent<Slider>();
        TMP_Text progressText = loadingScreenGameObject.transform.Find("ProgressText").GetComponent<TMP_Text>();
        progressBar.value = 0;
        progressText.text = "0%";

        AsyncOperation loadingOperation =  SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone)
        {
            yield return new WaitForEndOfFrame();
            float progress = Mathf.Clamp01(loadingOperation.progress / .9f);
            progressBar.value = progress;
            progressText.text = (progress * 100f).ToString("N0") + "%";
            if (loadingOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                loadingOperation.allowSceneActivation = true;
            }
            yield return null;
        }
        AudioManager.instance.PlaySong(sceneName);
    }

    public void LoadActiveLevel()
    {
        //if (gameLevelList[activeLevel].unlocked) StartCoroutine(LoadSceneByNameAsynchronously("Level" + activeLevel));
        StartCoroutine(LoadSceneByNameAsynchronously("Level" + activeLevel));
    }

    public void LoadData()
    {
        using StreamReader rd = new StreamReader(persistentPath);
        string json = rd.ReadToEnd();

        gameLevelList = JsonUtility.FromJson<SaveDataObj>(json).gameLevelList;
    }

    public void SaveData()
    {
        SaveDataObj sDO = new(gameLevelList);
        string json = JsonUtility.ToJson(sDO);
        File.Delete(persistentPath);
        using StreamWriter writer = new StreamWriter(persistentPath);
        writer.Write(json);
    }
}
