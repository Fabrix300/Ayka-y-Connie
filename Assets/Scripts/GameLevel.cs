using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameLevel
{
    public string levelName;
    public string levelTitle;
    public int carrotsLeft;
    public int totalCarrots;
    public bool unlocked;
    public bool hasBeenPlayed;
}
