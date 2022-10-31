using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundsExercise : MonoBehaviour
{
    //public MusicalFigureInfo[] musicalFiguresInfoArray;
    public event Action OnErrorTutorial;
    public event Action OnError;
    public event Action OnWin;
    public event Action OnWinTutorial;

    [Header("Exercise UI Elements")]
    public Button checkButton;
    public TMP_Text checkButtonText;
    public Color correctColor;
    public Color wrongColor;
    public Color musicalFigureButtonSelectedColor;
    public Color musicalFigureButtonDefaultColor;
    public Color durationHolderColorDefault;
    public Color durationHolderColorPassed;
    public Button[] musicalFiguresButtons;
    public RectTransform playingGuideParent;
    public RectTransform playingGuide;
    public Image[] soundDurationHolders;
    public Button askedSoundButton;
    public TMP_Text musicalFigureSelectedText;

    private Level2Controller levelController;
    private readonly string[] musicalFiguresNames = new string[4] { "redonda", "blanca", "negra", "corchea" };
    private readonly int[] durationsOfMusicalFigures = new int[4] { 8, 4, 2, 1 };
    private AudioManager audioManager = AudioManager.instance;
    private AudioSource pickedAudioSource;
    private int randomDuration;
    private int valueSelected = 0;

    private void OnEnable()
    {
        levelController = FindObjectOfType<Level2Controller>();
        /*make the buttons disabled until sound is played*/
        foreach (Button b in musicalFiguresButtons) { b.interactable = false; }
        /*Get Random Number to get random note and duration*/
        int randomNote = UnityEngine.Random.Range(0, audioManager.musicalNotes.Length);
        randomDuration = UnityEngine.Random.Range(0, audioManager.musicalNotes[randomNote].musicalNotesAccordingToFigures.Length);
        pickedAudioSource = audioManager.musicalNotes[randomNote].musicalNotesAccordingToFigures[randomDuration].source;
        /*Create Function to assign to the button to play the random note length*/
        /*in the function set the white bar to move according to the sound*/
        /*work on logic to select a button and set a "selected" variable in this script*/
        /*check if the one selected is the sound obtained*/
    }

    private void OnDisable()
    {
        playingGuide.anchoredPosition = new Vector2(7.5f, 0f);
        foreach (Image i in soundDurationHolders) { i.color = durationHolderColorDefault; }
        for (int i = 0; i < musicalFiguresButtons.Length; i++)
        {
            ColorBlock cB = musicalFiguresButtons[i].colors;
            cB.normalColor = musicalFigureButtonDefaultColor;
            cB.selectedColor = musicalFigureButtonDefaultColor;
            cB.highlightedColor = musicalFigureButtonDefaultColor;
            musicalFiguresButtons[i].colors = cB;
        }
        valueSelected = 0;
        checkButton.interactable = true;
        checkButtonText.color = new Color(1f, 1f, 1f, 1f);
        checkButtonText.text = "Comprobar";
        musicalFigureSelectedText.text = "()";
    }

    public void PlaySound()
    {
        foreach (Image i in soundDurationHolders) { i.color = durationHolderColorDefault; }
        playingGuide.anchoredPosition = new Vector2(7.5f, 0f);
        pickedAudioSource.Play();
        MoveToPos(new Vector2(playingGuideParent.sizeDelta.x - 7.5f, 0f), 2f);
    }

    public void MoveToPos(Vector2 position, float timeToMove)
    {
        askedSoundButton.interactable = false;
        StartCoroutine(MoveToPosition(position, timeToMove));
    }

    public IEnumerator MoveToPosition(Vector2 position, float timeToMove)
    {
        Vector2 currentPos = playingGuide.anchoredPosition;
        var t = 0f;
        int count = 1;
        float breakPoint = (playingGuideParent.sizeDelta.x - 7.5f)/8f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            //transform.position = Vector3.Lerp(currentPos, position, t);
            playingGuide.anchoredPosition = Vector2.Lerp(currentPos, position, t);
            if(playingGuide.anchoredPosition.x >= breakPoint * count) {
                if (count < durationsOfMusicalFigures[randomDuration]+1) { soundDurationHolders[count - 1].color = durationHolderColorPassed; }
                count++; 
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        askedSoundButton.interactable = true;
        if (musicalFiguresButtons[0].interactable == false) { foreach (Button b in musicalFiguresButtons) { b.interactable = true; } }
    }

    public void SelectMusicalFigureButton(int index)
    {
        //valueSelected = value;
        for (int i = 0; i < musicalFiguresButtons.Length; i++)
        {
            ColorBlock cB = musicalFiguresButtons[i].colors;
            cB.normalColor = musicalFigureButtonDefaultColor;
            cB.selectedColor = musicalFigureButtonDefaultColor;
            cB.highlightedColor = musicalFigureButtonDefaultColor;
            musicalFiguresButtons[i].colors = cB;
        }
        ColorBlock cBSelected = musicalFiguresButtons[index].colors;
        cBSelected.normalColor = musicalFigureButtonSelectedColor;
        cBSelected.selectedColor = musicalFigureButtonSelectedColor;
        cBSelected.highlightedColor = musicalFigureButtonSelectedColor;
        musicalFiguresButtons[index].colors = cBSelected;
        valueSelected = durationsOfMusicalFigures[index];
        // show name according to selected
        musicalFigureSelectedText.text = "(una " + musicalFiguresNames[index] + ")";
    }

    public void CheckAnswer()
    {
        if (valueSelected != 0)
        {
            ColorBlock cB = checkButton.colors;
            if (valueSelected == durationsOfMusicalFigures[randomDuration])
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
}
