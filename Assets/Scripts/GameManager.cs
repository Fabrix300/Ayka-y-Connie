using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isFirstTimePlaying;
    public int activeLevel;
    public int carrotsPerLevel;

    public GameLevel[] gameLevelList;
    public float transitionsTime;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
        FeedGameLevelList();
    }

    private void Start()
    {
        if (isFirstTimePlaying)
        {
            LoadSceneByName("level0");
        }
        else
        {
            LoadSceneByName("GameMap");
        }
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
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        GameObject levelLoader = GameObject.Find("LevelLoader");
        if (levelLoader)
        {
            levelLoader.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Start");
        }
        yield return new WaitForSeconds(transitionsTime);
        SceneManager.LoadSceneAsync(sceneName);
    }

}
