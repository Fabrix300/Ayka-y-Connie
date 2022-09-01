using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapCameraMovementController : MonoBehaviour
{
    public float[] horizontalLevelPoints;
    public float smoothFactor;

    public GameObject Connie_Map_Pf;
    public GameObject Ayka_Map_Pf;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        transform.position = new Vector3
            (horizontalLevelPoints[gameManager.activeLevel],
            transform.position.y,
            transform.position.z
            );
        Invoke(nameof(SpawnAykaAndConnie), 1f);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position, new Vector3(
                horizontalLevelPoints[gameManager.activeLevel],
                transform.position.y, 
                transform.position.z),
            smoothFactor * Time.fixedDeltaTime
            );
    }

    public void GoToNextLevelPoint()
    {
        if (gameManager.activeLevel == horizontalLevelPoints.Length - 1) return;
        gameManager.activeLevel += 1;
    }

    public void GoToPreviowsLevelPoint()
    {
        if (gameManager.activeLevel == 0) return;
        gameManager.activeLevel -= 1;
    }

    void SpawnAykaAndConnie()
    {
        Instantiate(Ayka_Map_Pf, new Vector2(transform.position.x - 1.3f - 3f, 0f), Quaternion.identity);
        Instantiate(Connie_Map_Pf, new Vector2(transform.position.x - 2.6f - 3f, 0f), Quaternion.identity);
    }
}
