using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsExercise : MonoBehaviour
{
    public ValueSwipeSelector valueSwipeSelector;
    public ValueSwipeSelector denominatorValueSwipeSelector;
    public event Action OnErrorTutorial;
    public event Action OnError;
    public event Action OnWin;
    public event Action OnWinTutorial;

    [Header("Exercise UI Elements")]
    public Button checkButton;
    public TMP_Text checkButtonText;
    public Color correctColor;
    public Color wrongColor;
    public GameObject musicalFiguresParent;
    public GameObject[] musicalFiguresPrefabs;
    public TMP_Text askedMusicalFigureText;

    private Level3Controller levelController;
    private int musicalFiguresUsedLimit;
    private int[] usedMusicalFigures = new int[4] {0,0,0,0};
    private string[] musicalFiguresNames = new string[4] { "Redondas", "Blancas", "Negras", "Corcheas" };
    private int askedMusicalFigure;
     
    private void OnEnable()
    {
        levelController = FindObjectOfType<Level3Controller>();
        /* generar las figuras random */
        //obtener numero de figuras a usar
        musicalFiguresUsedLimit = UnityEngine.Random.Range(2, 9);
        int[] musicalFiguresUsed = new int[musicalFiguresUsedLimit];
        for (int i = 0; i < musicalFiguresUsed.Length; i++)
        {
            musicalFiguresUsed[i] = UnityEngine.Random.Range(0, 4);
            Instantiate(musicalFiguresPrefabs[musicalFiguresUsed[i]], musicalFiguresParent.transform);
        }
        //denominator.text = musicalFiguresUsedLimit.ToString();
        /* chequear cuales se estan usando y preguntar en base a eso */
        for (int i = 0; i < musicalFiguresUsed.Length; i++)
        {
            usedMusicalFigures[musicalFiguresUsed[i]] += 1;
        }
        askedMusicalFigure = 4;
        while (askedMusicalFigure == 4)
        {
            int selectedMaybe = UnityEngine.Random.Range(0, 4);
            if (usedMusicalFigures[selectedMaybe] != 0)
            {
                askedMusicalFigure = selectedMaybe;
            }
        }
        askedMusicalFigureText.text = musicalFiguresNames[askedMusicalFigure];
    }

    private void OnDisable()
    {
        foreach (Transform child in musicalFiguresParent.transform)
        {
            Destroy(child.gameObject);
        }
        checkButton.interactable = true;
        checkButtonText.color = new Color(1f, 1f, 1f, 1f);
        checkButtonText.text = "Comprobar";
        usedMusicalFigures = new int[4] { 0, 0, 0, 0 };
    }

    public void CheckAnswer()
    {
        int valueSelected = int.Parse(valueSwipeSelector.swipeSelectorValues[Mathf.Abs(valueSwipeSelector.GetSelected() - (valueSwipeSelector.swipeSelectorValues.Length - 1))].text);
        int denominatorValueSelected = int.Parse(denominatorValueSwipeSelector.swipeSelectorValues[Mathf.Abs(denominatorValueSwipeSelector.GetSelected() - (denominatorValueSwipeSelector.swipeSelectorValues.Length - 1))].text);
        ColorBlock cB = checkButton.colors;
        if (valueSelected == usedMusicalFigures[askedMusicalFigure] && denominatorValueSelected == musicalFiguresUsedLimit)
        {
            checkButtonText.text = "Correcto";
            checkButtonText.color = new Color(0f, 0.3f, 0f, 1f);
            cB.disabledColor = correctColor;
            checkButton.colors = cB;
            checkButton.interactable = false;

            if (levelController.firstTimeExerciseTutorial) { OnWinTutorial?.Invoke(); }
            else { OnWin?.Invoke(); }
        }
        else
        {
            checkButtonText.text = "Incorrecto";
            checkButtonText.color = new Color(0.3f, 0f, 0f, 1f);
            cB.disabledColor = wrongColor;
            checkButton.colors = cB;
            checkButton.interactable = false;
            if (levelController.firstTime) { OnErrorTutorial?.Invoke(); }
            else if (!levelController.firstTimeExerciseTutorial) { OnError?.Invoke(); }
        }
    }
}