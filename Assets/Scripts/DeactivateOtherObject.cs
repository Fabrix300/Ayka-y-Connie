using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOtherObject : MonoBehaviour
{
    public GameObject targetToDeactivate;

    public void DeactivateTargetObject()
    {
        targetToDeactivate.SetActive(false);
    }
}
