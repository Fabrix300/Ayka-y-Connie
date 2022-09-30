using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class EquivalenceExercise : MonoBehaviour
{
    public MusicalFigureInfo[] musicalFiguresInfoArray;
    public ValueSwipeSelector valueSwipeSelector;
    public event Action OnErrorTutorial;
    public event Action OnError;
    public event Action OnWin;
    public event Action OnWinTutorial;

    [Header("Exercise UI Elements")]
    public TMP_Text askedMusicalFigureTextElement;
    public Image askedMusicalFigureImageElement;
    public Image answerMusicalFigureImageElement;
    public Button checkButton;
    public TMP_Text checkButtonText;
    public Color correctColor;
    public Color wrongColor;

    private Level1Controller levelController;
    private int firstMusicalFigure = 0;
    private int secondMusicalFigure = 0;
    private int askedValueFromMusicalFigure = 0;
    private readonly string[] congratsMessages = new string[4] 
    { "�Genial!", "�Buen trabajo!", "�As� se hace!", "�Sigue as�!" };

    private void OnEnable()
    {
        checkButton.interactable = true;
        checkButtonText.color = new Color(1f, 1f, 1f, 1f);
        checkButtonText.text = "Comprobar";
        levelController = FindObjectOfType<Level1Controller>();
        firstMusicalFigure = UnityEngine.Random.Range(0, musicalFiguresInfoArray.Length);
        /* Get random value from allowedValues array inside musicalFigureInfo object */
        askedValueFromMusicalFigure = musicalFiguresInfoArray[firstMusicalFigure].allowedValuesWhenAsked[UnityEngine.Random.Range(0, musicalFiguresInfoArray[firstMusicalFigure].allowedValuesWhenAsked.Length)];
        /* Get the second musical figure and ensure that it is divisible */
        secondMusicalFigure = ChooseSecondMusicalFigure();
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
        ColorBlock cB = checkButton.colors;
        if (answerValue == askedValue)
        {
            checkButtonText.text = congratsMessages[UnityEngine.Random.Range(0, congratsMessages.Length)];
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

    int ChooseSecondMusicalFigure()
    {
        List<int> l = new();
        for (int i = 0; i < musicalFiguresInfoArray.Length; i++) {
            if (
                musicalFiguresInfoArray[firstMusicalFigure].musicalFigureValue
                != musicalFiguresInfoArray[i].musicalFigureValue) 
            {
                float askedValue = askedValueFromMusicalFigure * musicalFiguresInfoArray[firstMusicalFigure].musicalFigureValue;
                if (askedValue % musicalFiguresInfoArray[i].musicalFigureValue == 0)
                {
                    l.Add(i);
                }
            }
        }
        return l[UnityEngine.Random.Range(0, l.Count)];
    }
}
