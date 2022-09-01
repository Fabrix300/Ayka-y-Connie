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
        levelName.text = gL.levelName;
        levelTitle.text = gL.levelTitle;
        carrotsText.text = gL.carrotsLeft+"/"+gL.totalCarrots;
        if (gL.unlocked) lockPanel.SetActive(false);
        else lockPanel.SetActive(true);
    }
}
