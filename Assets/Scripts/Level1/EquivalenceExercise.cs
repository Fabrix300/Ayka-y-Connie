using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquivalenceExercise : MonoBehaviour
{
    public MusicalFigureInfo[] musicalFiguresInfoArray;
    public ValueSwipeSelector valueSwipeSelector;
    //public event Action OnErrorTutorial;
    public event Action OnError;
    public event Action OnWin;
    //public event Action OnWinTutorial;

    [Header("Exercise UI Elements")]
    public TMP_Text askedMusicalFigureTextElement;
    public Image askedMusicalFigureImageElement;
    public Image answerMusicalFigureImageElement;

    private Level1Controller levelController;
    private int firstMusicalFigure = 0;
    private int secondMusicalFigure = 0;
    int askedValueFromMusicalFigure = 0;

    private void OnEnable()
    {
        levelController = FindObjectOfType<Level1Controller>();
        /* Get Two different numbers to get the musical figures. */
        firstMusicalFigure = UnityEngine.Random.Range(0, musicalFiguresInfoArray.Length);
        secondMusicalFigure = firstMusicalFigure;
        while (firstMusicalFigure == secondMusicalFigure) secondMusicalFigure = UnityEngine.Random.Range(0, musicalFiguresInfoArray.Length);
        /* Get random value from allowedValues array inside musicalFigureInfo object */
        askedValueFromMusicalFigure = musicalFiguresInfoArray[firstMusicalFigure].allowedValuesWhenAsked[UnityEngine.Random.Range(0, musicalFiguresInfoArray[firstMusicalFigure].allowedValuesWhenAsked.Length)];
        /* Set values in ui elements */
        askedMusicalFigureTextElement.text = askedValueFromMusicalFigure.ToString();
        askedMusicalFigureImageElement.sprite = musicalFiguresInfoArray[firstMusicalFigure].musicalFigureImage;
        answerMusicalFigureImageElement.sprite = musicalFiguresInfoArray[secondMusicalFigure].musicalFigureImage;
    }

    public void CheckAnswer()
    {
        int valueSelected = int.Parse(valueSwipeSelector.swipeSelectorValues[Mathf.Abs(valueSwipeSelector.GetSelected() - (valueSwipeSelector.swipeSelectorValues.Length - 1))].text);
        float answerValue = valueSelected * musicalFiguresInfoArray[secondMusicalFigure].musicalFigureValue;
        float askedValue = askedValueFromMusicalFigure * musicalFiguresInfoArray[firstMusicalFigure].musicalFigureValue;
        if (answerValue == askedValue)
        {
            Debug.Log("win");
        }
        else
        {
            Debug.Log("lose");
        }
    }
}
