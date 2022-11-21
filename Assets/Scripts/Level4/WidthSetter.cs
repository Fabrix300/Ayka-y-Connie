using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidthSetter : MonoBehaviour
{
    public RectTransform targetTransform;

    private RectTransform selfTransform;

    // Start is called before the first frame update
    void Start()
    {
        selfTransform = GetComponent<RectTransform>();
        StartCoroutine(Testing());
    }

    private IEnumerator Testing()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log(targetTransform.sizeDelta.x);
        selfTransform.sizeDelta = new Vector2(targetTransform.sizeDelta.x, selfTransform.sizeDelta.y);
    }
}
