using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapLevelInfoLoader : MonoBehaviour
{
    public TMP_Text levelName;
    public TMP_Text levelTitle;
    public TMP_Text carrotsText;
    public GameObject lockPanel;
    public int levelNumber;

    private void Start()
    {
        GameLevel gL = GameManager.instance.gameLevelList[levelNumber];
        if (gL.unlocked)
        {
            lockPanel.SetActive(false);
            levelName.text = gL.levelName;
            levelTitle.text = gL.levelTitle;
            if (gL.hasBeenPlayed) carrotsText.text = gL.carrotsLeft + "/" + gL.totalCarrots;
            else carrotsText.text = "-/" + gL.totalCarrots;
        }
        else
        {
            lockPanel.SetActive(true);
            levelName.transform.parent.gameObject.SetActive(false);
        }
    }
}
