using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapCarrotCounter : MonoBehaviour
{
    public TMP_Text carrotCounterText;
    
    void Start()
    {
        GameManager gameManager = GameManager.instance;
        int allLevelsTotalCarrots = gameManager.carrotLivesPerLevel * gameManager.gameLevelList.Length;
        int allLevelsCurrentCarrots = 0;
        foreach (GameLevel gL in gameManager.gameLevelList)
        {
            if (gL.hasBeenPlayed) { allLevelsCurrentCarrots += gL.carrotsLeft; }
        }
        carrotCounterText.text = allLevelsCurrentCarrots + "/" + allLevelsTotalCarrots;
    }
}