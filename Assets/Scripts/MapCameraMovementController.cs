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
        AudioManager.instance.PlaySong("GameMap");
        Invoke(nameof(SpawnAykaAndConnie), 0.8f);
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
        float cameraXLimit = GetComponent<Camera>().ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
        //Instantiate(Ayka_Map_Pf, new Vector2(transform.position.x - 1.3f - 3f, 0f), Quaternion.identity);
        //Instantiate(Connie_Map_Pf, new Vector2(transform.position.x - 2.6f - 3f, 0f), Quaternion.identity);
        Instantiate(Ayka_Map_Pf, new Vector2(transform.position.x - cameraXLimit, 0f), Quaternion.identity);
        Instantiate(Connie_Map_Pf, new Vector2(transform.position.x - cameraXLimit - 1.3f, 0f), Quaternion.identity);
    }

    public void LoadActiveLevel()
    {
        gameManager.LoadActiveLevel();
    }
}
