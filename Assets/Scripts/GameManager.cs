using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public int activeLevel;
    public int carrotsPerLevel;

    public GameLevel[] gameLevelList;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
        FeedGameLevelList();
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

}
