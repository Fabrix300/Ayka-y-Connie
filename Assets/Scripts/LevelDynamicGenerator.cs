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

    private void Start()
    {
        totalNumberOfElements = GameManager.instance.carrotsPerLevel;
        GenerateTerrainAndEnemies();
    }

    public void GenerateTerrainAndEnemies()
    {
        for (int i = 0; i < totalNumberOfElements; i++)
        {
            //instanciar terreno
            GameObject terrainGO = Instantiate(terrainPreFab, gridParent);
            terrainGO.transform.position = new Vector2(startingPoint + (i*separationOffset), 0f);
            //instanciar enemigo
            GameObject opossumGO = Instantiate(enemyPreFab, enemyGOParent);
            opossumGO.transform.position = new Vector2(startingPoint + enemySeparationOffset + (i * separationOffset), enemyPreFab.transform.position.y);
        }
    }
}
