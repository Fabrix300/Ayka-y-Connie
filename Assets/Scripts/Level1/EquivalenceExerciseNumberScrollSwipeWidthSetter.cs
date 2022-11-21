using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquivalenceExerciseNumberScrollSwipeWidthSetter : MonoBehaviour
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
        Debug.Log(RectTransformUtility.CalculateRelativeRectTransformBounds(transformParent, transformTarget).size.y);
        rectSelf.sizeDelta = new Vector2(RectTransformUtility.CalculateRelativeRectTransformBounds(transformParent, transformTarget).size.x, rectSelf.sizeDelta.y);
    }
}
