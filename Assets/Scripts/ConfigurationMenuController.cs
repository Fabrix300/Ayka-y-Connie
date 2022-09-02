using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationMenuController : MonoBehaviour
{
    public Animator configMenuOverlayAnim;
    public Animator configMenuPanelAnim;

    private void Start()
    {
        if(gameObject.activeSelf) gameObject.SetActive(false);
    }

    public void OpenConfigMenu()
    {
        gameObject.SetActive(true);
    }

    public void CloseConfigMenu()
    {
        configMenuPanelAnim.SetInteger("state", 1);
        configMenuOverlayAnim.SetInteger("state", 1);
    }
}
