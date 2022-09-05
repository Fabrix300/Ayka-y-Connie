using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCarrotCounter : MonoBehaviour
{
    public TMP_Text levelCarrotCounterText;

    private int totalCarrots;
    private int currentCarrots;
    //private GameLevel actualGameLevel;

    private void Start()
    {
        //GameManager gameManager = GameManager.instance;
        //actualGameLevel = GameManager.instance.gameLevelList[GameManager.instance.activeLevel];
        totalCarrots = GameManager.instance.carrotsPerLevel;
        currentCarrots = totalCarrots;
        levelCarrotCounterText.text = currentCarrots + "/" + totalCarrots;
    }

    // pensar como sera el aumento o disminucion de las zanahorias.
}
