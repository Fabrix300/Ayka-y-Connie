using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightSetter : MonoBehaviour
{
    public RectTransform targetTransform;

    private RectTransform selfTransform;

    // Start is called before the first frame update
    void Start()
    {
        selfTransform = GetComponent<RectTransform>();
        selfTransform.sizeDelta = new Vector2(selfTransform.sizeDelta.x, targetTransform.sizeDelta.y);
    }
}
