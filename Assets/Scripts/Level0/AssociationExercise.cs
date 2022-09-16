using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssociationExercise : MonoBehaviour
{
    public AssociationImageButton[] mFImages;
    public AssociationTextButton[] mFNames;
    public AssociationTextButton[] mFValues;

    public GameObject leftLayout;
    public GameObject rightLayout;
    public GameObject buttonPreFab;

    public Color selectedColor;
    public Color correctColor;
    public Color errorColor;
    public Color defaultColor;

    public int timesToFail;
    public int timesToWin;
    public event Action OnErrorTutorial;
    public event Action OnError;
    public event Action OnWin;
    public event Action OnWinTutorial;

    private int timesFailing = 0;
    private int timesWinning = 0;
    private Level0Controller levelController;
    private readonly List<AssociationButton[]> associationTypeButtonsList = new();
    private AssociationButtonGameObject firstButtonSelected = null;
    private AssociationButtonGameObject secondButtonSelected = null;
    private readonly float timeToFadeColor = 1f;

    private void Start()
    {
        Debug.Log("Start is second");
    }

    private void OnEnable()
    {
        timesFailing = 0;
        levelController = FindObjectOfType<Level0Controller>();
        // Add AssociationTypeButtons to the list
        associationTypeButtonsList.Clear();
        associationTypeButtonsList.Add(mFImages);
        associationTypeButtonsList.Add(mFNames);
        associationTypeButtonsList.Add(mFValues);
        // Choose randomly 2 arrays from the list to know which ones will be on left and right
        int firstArray = UnityEngine.Random.Range(0, associationTypeButtonsList.Count); int secondArray = firstArray;
        while (firstArray == secondArray) secondArray = UnityEngine.Random.Range(0, associationTypeButtonsList.Count);
        // Scramble randomly the elements inside to instantiate the buttons according to that scramble
        int[] firstArrayOrder = Shuffle(new int[]{ 0, 1, 2, 3 });
        int[] secondArrayOrder = Shuffle(new int[] { 0, 1, 2, 3 });
        // Instantiate the buttons
        for (int i = 0; i < associationTypeButtonsList[firstArray].Length; i++)
        {
            AssociationButton aS = associationTypeButtonsList[firstArray][firstArrayOrder[i]];
            GameObject buttonGO = Instantiate(buttonPreFab, leftLayout.transform);
            AssociationButtonGameObject aBGO = buttonGO.GetComponent<AssociationButtonGameObject>();
            aBGO.column = 0; aBGO.associationButtonpair = aS.associationButtonpair;
            buttonGO.GetComponent<Button>().onClick.AddListener(() => CheckButton(aBGO));
            if (aS is AssociationImageButton aIB)
            {
                GameObject imageChildObject = buttonGO.transform.Find("Image").gameObject;
                imageChildObject.SetActive(true);
                imageChildObject.GetComponent<Image>().sprite = aIB.buttonImage;
            }
            else if (aS is AssociationTextButton aTB)
            {
                GameObject imageChildObject = buttonGO.transform.Find("Text").gameObject;
                imageChildObject.SetActive(true);
                imageChildObject.GetComponent<TMP_Text>().text = aTB.buttonText;
            }
        }
        for (int i = 0; i < associationTypeButtonsList[secondArray].Length; i++)
        {
            AssociationButton aS = associationTypeButtonsList[secondArray][secondArrayOrder[i]];
            GameObject buttonGO = Instantiate(buttonPreFab, rightLayout.transform);
            AssociationButtonGameObject aBGO = buttonGO.GetComponent<AssociationButtonGameObject>();
            aBGO.column = 1; aBGO.associationButtonpair = aS.associationButtonpair;
            buttonGO.GetComponent<Button>().onClick.AddListener(() => CheckButton(aBGO));
            if (aS is AssociationImageButton aIB)
            {
                GameObject imageChildObject = buttonGO.transform.Find("Image").gameObject;
                imageChildObject.SetActive(true);
                imageChildObject.GetComponent<Image>().sprite = aIB.buttonImage;
            }
            else if (aS is AssociationTextButton aTB)
            {
                GameObject imageChildObject = buttonGO.transform.Find("Text").gameObject;
                imageChildObject.SetActive(true);
                imageChildObject.GetComponent<TMP_Text>().text = aTB.buttonText;
            }
        }
    }

    private void OnDisable()
    {
        foreach (Transform child in leftLayout.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rightLayout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    int[] Shuffle(int[] array)
    {
        int[] arrayCopy = array;
        for (int i = 0; i < arrayCopy.Length; i++)
        {
            int helper = UnityEngine.Random.Range(0, arrayCopy.Length);
            int temp = arrayCopy[i];
            arrayCopy[i] = arrayCopy[helper];
            arrayCopy[helper] = temp;
        }
        return arrayCopy;
    }

    private void CheckButton(AssociationButtonGameObject aBGO)
    {
        if (!firstButtonSelected)
        {
            firstButtonSelected = aBGO;
            Button firstButtonSelectedButton = firstButtonSelected.gameObject.GetComponent<Button>();
            ColorBlock colorBlock = firstButtonSelectedButton.colors;
            colorBlock.normalColor = selectedColor;
            colorBlock.selectedColor = selectedColor;
            colorBlock.highlightedColor = selectedColor;
            firstButtonSelectedButton.colors = colorBlock;
        }
        else
        {
            if (aBGO.column == firstButtonSelected.column)
            {
                Button firstButtonSelectedButton = firstButtonSelected.gameObject.GetComponent<Button>();
                ColorBlock colorBlock = firstButtonSelectedButton.colors;
                colorBlock.normalColor = defaultColor;
                colorBlock.selectedColor = defaultColor;
                colorBlock.highlightedColor = defaultColor;
                firstButtonSelectedButton.colors = colorBlock;
                firstButtonSelected = aBGO;
                firstButtonSelectedButton = firstButtonSelected.gameObject.GetComponent<Button>();
                colorBlock = firstButtonSelectedButton.colors;
                colorBlock.normalColor = selectedColor;
                colorBlock.selectedColor = selectedColor;
                colorBlock.highlightedColor = selectedColor;
                firstButtonSelectedButton.colors = colorBlock;
            }
            else
            {
                secondButtonSelected = aBGO;
                if (secondButtonSelected.associationButtonpair == firstButtonSelected.associationButtonpair)
                {
                    Button firstButtonSelectedButton = firstButtonSelected.gameObject.GetComponent<Button>();
                    ColorBlock colorBlock = firstButtonSelectedButton.colors;
                    colorBlock.normalColor = correctColor;
                    colorBlock.selectedColor = correctColor;
                    colorBlock.highlightedColor = correctColor;
                    firstButtonSelectedButton.colors = colorBlock;
                    Button secondButtonSelectedButton = secondButtonSelected.gameObject.GetComponent<Button>();
                    colorBlock = secondButtonSelectedButton.colors;
                    colorBlock.normalColor = correctColor;
                    colorBlock.selectedColor = correctColor;
                    colorBlock.highlightedColor = correctColor;
                    secondButtonSelectedButton.colors = colorBlock;
                    StartCoroutine(FadeToColorAndDisableButton(firstButtonSelectedButton,secondButtonSelectedButton,correctColor,Color.gray));
                    firstButtonSelected = null;
                    secondButtonSelected = null;
                    timesWinning++;
                    if (levelController.firstTimeExerciseTutorial) OnWinTutorial?.Invoke();
                    else if (timesWinning == timesToWin && !levelController.firstTimeExerciseTutorial) OnWin?.Invoke();
                }
                else
                {
                    Button firstButtonSelectedButton = firstButtonSelected.gameObject.GetComponent<Button>();
                    ColorBlock colorBlock = firstButtonSelectedButton.colors;
                    colorBlock.normalColor = errorColor;
                    colorBlock.selectedColor = errorColor;
                    colorBlock.highlightedColor = errorColor;
                    firstButtonSelectedButton.colors = colorBlock;
                    Button secondButtonSelectedButton = secondButtonSelected.gameObject.GetComponent<Button>();
                    colorBlock = secondButtonSelectedButton.colors;
                    colorBlock.normalColor = errorColor;
                    colorBlock.selectedColor = errorColor;
                    colorBlock.highlightedColor = errorColor;
                    secondButtonSelectedButton.colors = colorBlock;
                    StartCoroutine(FadeToColor(firstButtonSelectedButton,secondButtonSelectedButton,errorColor,defaultColor));
                    firstButtonSelected = null;
                    secondButtonSelected = null;
                    timesFailing++;
                    if (levelController.firstTime) OnErrorTutorial?.Invoke();
                    else if (timesFailing == timesToFail && !levelController.firstTimeExerciseTutorial) OnError?.Invoke();
                }
            }
        }
    }

    private IEnumerator FadeToColor(Button firstButton, Button secondButton, Color originColor, Color targetColor)
    {
        Button firstButtonCopy = firstButton; Button secondButtonCopy = secondButton;
        ColorBlock firstColorBlock = firstButtonCopy.colors; ColorBlock secondColorBlock = secondButtonCopy.colors;
        float currentTime = 0f;
        while (currentTime < timeToFadeColor)
        {
            Color fadeColor = Color.Lerp(originColor, targetColor, currentTime / timeToFadeColor);
            firstColorBlock.normalColor = fadeColor;
            firstColorBlock.selectedColor = fadeColor;
            firstColorBlock.highlightedColor = fadeColor;
            firstButtonCopy.colors = firstColorBlock;
            secondColorBlock.normalColor = fadeColor;
            secondColorBlock.selectedColor = fadeColor;
            secondColorBlock.highlightedColor = fadeColor;
            secondButtonCopy.colors = secondColorBlock;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeToColorAndDisableButton(Button firstButton, Button secondButton, Color originColor, Color targetColor)
    {
        Button firstButtonCopy = firstButton; Button secondButtonCopy = secondButton;
        ColorBlock firstColorBlock = firstButtonCopy.colors; ColorBlock secondColorBlock = secondButtonCopy.colors;
        float currentTime = 0f;
        while (currentTime < timeToFadeColor)
        {
            Color fadeColor = Color.Lerp(originColor, targetColor, currentTime / timeToFadeColor);
            firstColorBlock.normalColor = fadeColor;
            firstColorBlock.selectedColor = fadeColor;
            firstColorBlock.highlightedColor = fadeColor;
            firstButtonCopy.colors = firstColorBlock;
            secondColorBlock.normalColor = fadeColor;
            secondColorBlock.selectedColor = fadeColor;
            secondColorBlock.highlightedColor = fadeColor;
            secondButtonCopy.colors = secondColorBlock;
            currentTime += Time.deltaTime;
            yield return null;
        }
        firstButtonCopy.interactable = false;
        secondButtonCopy.interactable = false;
    }
}
