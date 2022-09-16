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
    public float enemySeparationOffset;

    private int totalNumberOfElements;
    private float[] enemyPositionsArray;

    private void Start()
    {
        totalNumberOfElements = GameManager.instance.carrotsPerLevel;
        GenerateTerrainAndEnemies();
    }

    public void GenerateTerrainAndEnemies()
    {

        enemyPositionsArray = new float[totalNumberOfElements];
        for (int i = 0; i < totalNumberOfElements; i++)
        {
            //instanciar terreno
            GameObject terrainGO = Instantiate(terrainPreFab, gridParent);
            terrainGO.transform.position = new Vector2(startingPoint + (i*separationOffset), 0f);
            //instanciar enemigo
            GameObject enemyGO = Instantiate(enemyPreFab, enemyGOParent);
            float positionX = startingPoint + enemySeparationOffset + (i * separationOffset);
            enemyGO.transform.position = new Vector2(positionX, enemyPreFab.transform.position.y);
            enemyPositionsArray[i] = positionX;
        }
    }

    public float[] GetEnemyPositionsArray()
    {
        return enemyPositionsArray;
    }
}
