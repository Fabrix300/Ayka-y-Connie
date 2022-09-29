using System;
using UnityEngine;

public class EquivalenceExercise : MonoBehaviour
{
    public int timesToFail;
    public int timesToWin;
    public event Action OnErrorTutorial;
    public event Action OnError;
    public event Action OnWin;
    public event Action OnWinTutorial;

    private int timesFailing = 0;
    private int timesWinning = 0;
    private Level1Controller levelController;

    private void OnEnable()
    {
        timesWinning = 0;
        timesFailing = 0;
        levelController = FindObjectOfType<Level1Controller>();
    }
}
