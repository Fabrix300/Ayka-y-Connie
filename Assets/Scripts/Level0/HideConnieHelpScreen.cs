using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideConnieHelpScreen : MonoBehaviour
{
    public GameObject connieHelpScreens;

    public void HideConnieHelpScreenGO()
    {
        connieHelpScreens.SetActive(false);
    }

    public void ToggleConnieHelpScreenGO()
    {
        if(!connieHelpScreens.activeInHierarchy) connieHelpScreens.SetActive(true);
        else connieHelpScreens.SetActive(false);
    }
}
