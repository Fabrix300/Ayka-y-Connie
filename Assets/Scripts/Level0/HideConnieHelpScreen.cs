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

    public void ShowConnieHelpScreenGO()
    {
        connieHelpScreens.SetActive(true);
    }
}
