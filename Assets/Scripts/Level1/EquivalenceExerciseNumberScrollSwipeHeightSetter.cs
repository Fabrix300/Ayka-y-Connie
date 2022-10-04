using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EquivalenceExerciseNumberScrollSwipeHeightSetter : MonoBehaviour
{
    public Transform transformParent;
    public Transform transformTarget;

    private RectTransform rectSelf;

    private void Start()
    {
        rectSelf = GetComponent<RectTransform>();
        StartCoroutine(Testing());
    }

    public IEnumerator Testing()
    {
        yield return new WaitForEndOfFrame();
        //Debug.Log(RectTransformUtility.CalculateRelativeRectTransformBounds(transformParent, transformTarget).size.y);
        rectSelf.sizeDelta = new Vector2(rectSelf.sizeDelta.x, RectTransformUtility.CalculateRelativeRectTransformBounds(transformParent, transformTarget).size.y);
    }
}
