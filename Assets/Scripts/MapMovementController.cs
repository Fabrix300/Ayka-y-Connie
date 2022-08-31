using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapMovementController : MonoBehaviour
{
    public float[] horizontalLevelPoints;
    public float smoothFactor;

    private int activeLevel;

    private void Start()
    {
        activeLevel = GameManager.instance.activeLevel;
        Debug.Log(activeLevel);
        transform.position = new Vector3
            (horizontalLevelPoints[activeLevel],
            transform.position.y,
            transform.position.z
            );
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position, new Vector3(
                horizontalLevelPoints[activeLevel],
                transform.position.y, 
                transform.position.z),
            smoothFactor * Time.fixedDeltaTime
            );
    }

    public void GoToNextLevelPoint()
    {
        if (activeLevel == horizontalLevelPoints.Length - 1) return;
        activeLevel += 1;
    }

    public void GoToPreviowsLevelPoint()
    {
        if (activeLevel == 0) return;
        activeLevel -= 1;
    }
}
