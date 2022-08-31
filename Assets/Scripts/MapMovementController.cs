using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovementController : MonoBehaviour
{
    public float[] horizontalLevelPoints;
    public int activeLevel;
    public float smoothFactor;

    private void Start()
    {
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
        /*mainCamera.transform.position = new Vector3
            (horizontalLevelPoints[activeLevel],
            mainCamera.transform.position.y,
            mainCamera.transform.position.z
            );*/
    }

    public void GoToPreviowsLevelPoint()
    {
        if (activeLevel == 0) return;
        activeLevel -= 1;
    }
}
