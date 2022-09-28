using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDynamicGenerator : MonoBehaviour
{
    public GameObject terrainPreFab;
    public GameObject enemyPreFab;
    public Transform gridParent;
    public Transform enemyGOParent;
    public float startingPoint;
    public float separationOffset;
    public float enemyStartingPoint;

    private int totalNumberOfElements;
    private GameObject[] enemyGameObjectsArray;

    private void Start()
    {
        totalNumberOfElements = GameManager.instance.enemiesPerLevel;
        GenerateTerrainAndEnemies();
    }

    public void GenerateTerrainAndEnemies()
    {
        enemyGameObjectsArray = new GameObject[totalNumberOfElements];
        for (int i = 0; i < totalNumberOfElements; i++)
        {
            //instanciar terreno
            GameObject terrainGO = Instantiate(terrainPreFab, gridParent);
            terrainGO.transform.position = new Vector2(startingPoint + (i*separationOffset), 0f);
            //instanciar enemigo
            GameObject enemyGO = Instantiate(enemyPreFab, enemyGOParent);
            //float positionX = enemyStartingPoint + enemySeparationOffset +(i * separationOffset);
            float positionX = enemyStartingPoint + (i * separationOffset);
            enemyGO.transform.position = new Vector2(positionX, enemyPreFab.transform.position.y);
            enemyGameObjectsArray[i] = enemyGO;
        }
    }

    public GameObject[] GetEnemyGameObjectsArray()
    {
        return enemyGameObjectsArray;
    }
}
