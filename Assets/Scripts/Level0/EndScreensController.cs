using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreensController : MonoBehaviour
{
    public LevelCarrotCounter lvlCarrotCounter;
    public GameObject LivesIndicatorPreFab;
    public GameObject blackOverlay;
    public GameObject winScreen;
    public GameObject loseScreen;
    public Color notObtainedLiveIndicatorIconColor;
    public TMP_Text winMessageText;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        blackOverlay.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    public void ShowWinScreen()
    {
        int totalCarrots = gameManager.carrotLivesPerLevel;
        Transform livesParent = winScreen.transform.GetChild(0).GetChild(2);
        for (int i = 0; i < totalCarrots; i++)
        {
            if (i + 1 <= lvlCarrotCounter.GetCurrentCarrots())
            {
                Instantiate(LivesIndicatorPreFab, livesParent);
            }
            else
            {
                GameObject liveIndicatorGO = Instantiate(LivesIndicatorPreFab, livesParent);
                liveIndicatorGO.GetComponent<Image>().color = notObtainedLiveIndicatorIconColor;
            }
        }
        switch (lvlCarrotCounter.GetCurrentCarrots())
        {
            case 1:
                {
                    string[] oneCarrotMessages = new string[3] 
                    { "?Puedes hacerlo mejor!", "?Puedes mejorar!", "?Siempre puedes reintentarlo!" };
                    winMessageText.text = oneCarrotMessages[Random.Range(0, oneCarrotMessages.Length-1)];
                    break;
                }
            case 2:
                {
                    string[] twoCarrotsMessages = new string[4]
                    { "?Bien!", "?Casi glorioso!", "?Casi perfecto!", "?Vas por buen camino!" };
                    winMessageText.text = twoCarrotsMessages[Random.Range(0, twoCarrotsMessages.Length - 1)];
                    break;
                }
            case 3:
                {
                    string[] twoCarrotsMessages = new string[5]
                    { "?Excelente!", "?Glorioso!", "?Que pro!", "?Magn?fico!", "?Incre?ble!" };
                    winMessageText.text = twoCarrotsMessages[Random.Range(0, twoCarrotsMessages.Length - 1)];
                    break;
                }
        }
        blackOverlay.SetActive(true);
        winScreen.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        blackOverlay.SetActive(true);
        loseScreen.SetActive(true);
    }

    public void WinSaveGameState()
    {
        GameLevel gL = gameManager.gameLevelList[gameManager.activeLevel];
        if (gL.hasBeenPlayed)
        {
            if (lvlCarrotCounter.GetCurrentCarrots() > gL.carrotsLeft)
            {
                gL.carrotsLeft = lvlCarrotCounter.GetCurrentCarrots();
            }
        }
        else
        {
            gL.hasBeenPlayed = true;
            gL.carrotsLeft = lvlCarrotCounter.GetCurrentCarrots();
        }
        if (gameManager.activeLevel + 1 < gameManager.gameLevelList.Length)
        {
            gameManager.gameLevelList[gameManager.activeLevel + 1].unlocked = true;
        }
        gameManager.SaveData();
    }

    public void LoseSaveGameState()
    {
        GameLevel gL = gameManager.gameLevelList[gameManager.activeLevel];
        if (!gL.hasBeenPlayed)
        {
            gL.carrotsLeft = lvlCarrotCounter.GetCurrentCarrots();
            gL.hasBeenPlayed = true;
        }
        gameManager.SaveData();
    }

    public void WinGoBackToMapAction() { gameManager.LoadSceneByName("GameMap"); }
    public void WinRestartAction() { gameManager.LoadActiveLevel(); }
    public void LoseGoBackToMapAction() { gameManager.LoadSceneByName("GameMap"); }
    public void LoseRestartAction() { gameManager.LoadActiveLevel(); }
}
