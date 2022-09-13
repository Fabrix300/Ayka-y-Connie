using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCarrotCounter : MonoBehaviour
{
    public TMP_Text levelCarrotCounterText;
    public GameObject carrotActionCanvas;

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

    public void DiminishOneCarrot(Vector2 positionToInstantiateEffect)
    {
        Instantiate(carrotActionCanvas, positionToInstantiateEffect, Quaternion.identity);
        currentCarrots--;
        levelCarrotCounterText.text = currentCarrots + "/" + totalCarrots;
    }
}
