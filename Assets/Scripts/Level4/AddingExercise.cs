using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AddingExercise : MonoBehaviour
{
    public ValueSwipeSelector valueSwipeSelector;
    public ValueSwipeSelector denominatorValueSwipeSelector;
    public ValueSwipeSelector wholeValueSwipeSelector;
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

    private Level4Controller levelController;
    private int[] usedMusicalFigures = new int[4] { 0, 0, 0, 0 };
    private float[] musicalFiguresValues = { 1f, 0.500f, 0.250f, 0.125f };

    private void OnEnable()
    {
        levelController = FindObjectOfType<Level4Controller>();
        int musicalFiguresUsedLimit = UnityEngine.Random.Range(2, 9);
        int counter = 0;
        bool stop = false;
        while (!stop && counter < musicalFiguresUsedLimit)
        {
            int musicalFigure = UnityEngine.Random.Range(0, 3);
            float sum = 0f;
            for (int i = 0; i < usedMusicalFigures.Length; i++) { sum += usedMusicalFigures[i] * musicalFiguresValues[i]; }
            if (sum + musicalFiguresValues[musicalFigure] <= 2f)
            {
                usedMusicalFigures[musicalFigure] += 1;
                Instantiate(musicalFiguresPrefabs[musicalFigure], musicalFiguresParent.transform);
                counter++;
                float sum2 = 0f;
                for (int i = 0; i < usedMusicalFigures.Length; i++) { sum2 += usedMusicalFigures[i] * musicalFiguresValues[i]; }
                if (sum2 == 2f)
                {
                    stop = true;
                }
            }
        }
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
        string valueNumerator = valueSwipeSelector.swipeSelectorValues[Mathf.Abs(valueSwipeSelector.GetSelected() - (valueSwipeSelector.swipeSelectorValues.Length - 1))].text;
        int valueSelected = 0;
        if(valueNumerator != "-") { valueSelected = int.Parse(valueSwipeSelector.swipeSelectorValues[Mathf.Abs(valueSwipeSelector.GetSelected() - (valueSwipeSelector.swipeSelectorValues.Length - 1))].text); }
        Debug.Log("valueSelected" + valueSelected);
        int denominatorValueSelected = int.Parse(denominatorValueSwipeSelector.swipeSelectorValues[Mathf.Abs(denominatorValueSwipeSelector.GetSelected() - (denominatorValueSwipeSelector.swipeSelectorValues.Length - 1))].text);
        Debug.Log("denominatorValueSelected" + denominatorValueSelected);
        int wholeValueSelected = int.Parse(wholeValueSwipeSelector.swipeSelectorValues[Mathf.Abs(wholeValueSwipeSelector.GetSelected() - (wholeValueSwipeSelector.swipeSelectorValues.Length - 1))].text);
        float sumOfUsed = 0;
        for (int i = 0; i < usedMusicalFigures.Length; i++) { sumOfUsed += usedMusicalFigures[i] * musicalFiguresValues[i]; }
        float sumOfAnswer = wholeValueSelected + ((float)valueSelected / (float)denominatorValueSelected);
        Debug.Log("sumOfAnswer: " + sumOfAnswer);
        ColorBlock cB = checkButton.colors;
        if (sumOfAnswer == sumOfUsed)
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
