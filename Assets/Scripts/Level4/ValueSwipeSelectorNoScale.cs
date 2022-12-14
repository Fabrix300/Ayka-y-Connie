using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueSwipeSelectorNoScale : MonoBehaviour
{
    public Scrollbar scrollBar;

    [HideInInspector] public TMP_Text[] swipeSelectorValues;

    private float scrollPos = 0;
    private float[] pos;
    private float distance;
    public int selected;

    // Start is called before the first frame update
    void Start()
    {
        swipeSelectorValues = new TMP_Text[transform.childCount];
        int index = 0;
        foreach (Transform child in transform)
        {
            swipeSelectorValues[index] = child.GetComponent<TMP_Text>();
            index++;
        }
        pos = new float[transform.childCount];
        distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++) { pos[i] = distance * i; }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollBar.value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    scrollBar.value = Mathf.Lerp(scrollBar.value, pos[i], 0.1f);
                    selected = i;
                    //Debug.Log(selected + " " + transform.GetChild((transform.childCount - 1) - selected).name);
                    /*transform.GetChild(transform.childCount - 1 - selected).localScale = Vector2.Lerp(
                    transform.GetChild(transform.childCount - 1 - selected).localScale,
                    new Vector2(1f, 1f),
                    0.1f
                    );*/
                }
                /*if (i != selected)
                {
                    transform.GetChild(transform.childCount - 1 - i).localScale = Vector2.Lerp(
                            transform.GetChild(transform.childCount - 1 - i).localScale,
                            new Vector2(0.8f, 0.8f),
                            0.1f
                            );
                }*/
            }
        }
    }

    public int GetSelected() { return selected; }
}
