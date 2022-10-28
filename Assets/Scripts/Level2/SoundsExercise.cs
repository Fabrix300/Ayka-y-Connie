using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundsExercise : MonoBehaviour
{
    public MusicalFigureInfo[] musicalFiguresInfoArray;
    public event Action OnErrorTutorial;
    public event Action OnError;
    public event Action OnWin;
    public event Action OnWinTutorial;

    [Header("Exercise UI Elements")]
    public Button checkButton;
    public TMP_Text checkButtonText;
    public Color correctColor;
    public Color wrongColor;
    public Button[] musicalFiguresButtons;
    public RectTransform playingGuideParent;
    public RectTransform playingGuide;

    private Level1Controller levelController;
    private readonly string[] numberValuesTexts = new string[9]
    {
        "Una", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Ocho", "Nueve"
    };
    private AudioManager audioManager = AudioManager.instance;

    private void OnEnable()
    {
        //RectTransformUtility.CalculateRelativeRectTransformBounds(transformParent, transformTarget).size.y
        Debug.Log(playingGuideParent.sizeDelta.x);
        checkButton.interactable = true;
        checkButtonText.color = new Color(1f, 1f, 1f, 1f);
        checkButtonText.text = "Comprobar";
        /*make the buttons disabled until sound is played*/
        foreach (Button b in musicalFiguresButtons) { b.interactable = false; }
        /*Get Random Number to get random note and duration*/
        int randomNote = UnityEngine.Random.Range(0, audioManager.musicalNotes.Length);
        int randomDuration = UnityEngine.Random.Range(0, audioManager.musicalNotes[randomNote].musicalNotesAccordingToFigures.Length);
        /*Create Function to assign to the button to play the random note length*/
        /*in the function set the white bar to move according to the sound*/
        /*work on logic to select a button and set a "selected" variable in this script*/
        /*check if the one selected is the sound obtained*/
    }

    public void PlaySound()
    {
        MoveToPos(new Vector2(), 2f);
    }

    public void MoveToPos(Vector2 position, float timeToMove)
    {
        StartCoroutine(MoveToPosition(position, timeToMove));
    }

    public IEnumerator MoveToPosition(Vector2 position, float timeToMove)
    {
        Vector2 currentPos = playingGuide.anchoredPosition;
        Debug.Log(currentPos);
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            //transform.position = Vector3.Lerp(currentPos, position, t);
            playingGuide.anchoredPosition = Vector2.Lerp(currentPos, position, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        playingGuide.anchoredPosition = new Vector2(-480, 0);
    }
}
