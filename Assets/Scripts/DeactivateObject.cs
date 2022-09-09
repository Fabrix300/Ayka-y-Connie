using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject : MonoBehaviour
{
    public void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }
}
