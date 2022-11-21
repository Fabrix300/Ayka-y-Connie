using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpScreensPageController : MonoBehaviour
{
    public GameObject[] pages;
    public TMP_Text pageIndicator;
    public Button prevPageButton;
    public Button nextPageButton;

    private int activeHelpScreen;
    private GameManager gameManager = GameManager.instance;
    private List<GameObject> actualPages;

    private void Start()
    {
        actualPages = new List<GameObject>();
        for (int i = 0; i < gameManager.gameLevelList.Length; i++)
        {
            if (gameManager.gameLevelList[i].unlocked) {
                if (pages[i]) { actualPages.Add(pages[i]); }
            }
        }
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == activeHelpScreen);
        }
        activeHelpScreen = gameManager.activeLevel;
        prevPageButton.gameObject.SetActive(activeHelpScreen != 0);
        nextPageButton.gameObject.SetActive(activeHelpScreen != actualPages.Count - 1);
        pageIndicator.text = (activeHelpScreen + 1) + "/" + actualPages.Count;
        for(int i = 0; i < actualPages.Count; i++) {
            actualPages[i].SetActive(i == activeHelpScreen);
        }
    }

    public void goToPrevPage()
    {
        if (activeHelpScreen == 0) return;
        activeHelpScreen -= 1;
        prevPageButton.gameObject.SetActive(activeHelpScreen != 0);
        nextPageButton.gameObject.SetActive(activeHelpScreen != actualPages.Count - 1);
        pageIndicator.text = (activeHelpScreen + 1) + "/" + actualPages.Count;
        for (int i = 0; i < actualPages.Count; i++)
        {
            actualPages[i].SetActive(i == activeHelpScreen);
        }
    }

    public void goToNextPage()
    {
        if (activeHelpScreen == actualPages.Count - 1) return;
        activeHelpScreen += 1;
        prevPageButton.gameObject.SetActive(activeHelpScreen != 0);
        nextPageButton.gameObject.SetActive(activeHelpScreen != actualPages.Count - 1);
        pageIndicator.text = (activeHelpScreen + 1) + "/" + actualPages.Count;
        for (int i = 0; i < actualPages.Count; i++)
        {
            actualPages[i].SetActive(i == activeHelpScreen);
        }
    }
}
