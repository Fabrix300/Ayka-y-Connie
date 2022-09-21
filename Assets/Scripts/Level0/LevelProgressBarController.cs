using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelProgressBarController : MonoBehaviour
{
    public TMP_Text completedExercisesText;

    private Slider levelProgressSlider;
    private Animator levelProgressSliderAnimator;

    private void Start()
    {
        levelProgressSlider = GetComponent<Slider>();
        levelProgressSliderAnimator = GetComponent<Animator>();
    }

    public void AppearLevelProgressBarAndUpdate(int indexOfEnemy)
    {
        int index = indexOfEnemy;
        StartCoroutine(FillEnergySliderPlayer(index));
        levelProgressSliderAnimator.SetTrigger("Start");
    }

    public IEnumerator FillEnergySliderPlayer(int indexOfEnemy)
    {
        completedExercisesText.text = (indexOfEnemy) + "/" + GameManager.instance.enemiesPerLevel;
        float target = 1/ (float)GameManager.instance.enemiesPerLevel * indexOfEnemy;
        float value = levelProgressSlider.value;
        float currentTime = 0f;
        float timeToLerp = 0.8f;
        while (currentTime < timeToLerp)
        {
            levelProgressSlider.value = Mathf.Lerp(value, target, currentTime / timeToLerp);
            currentTime += Time.deltaTime;
            yield return null;
        }
        //levelProgressSlider.value = target;
    }
}
