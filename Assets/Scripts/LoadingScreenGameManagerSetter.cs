using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenGameManagerSetter : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.loadingScreenGameObject = gameObject;
        gameObject.SetActive(false);
    }
}
