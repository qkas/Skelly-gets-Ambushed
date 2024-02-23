using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;
    private float xRange = 10f;
    private float zRange = 10f;
    private int enemyCount = 5;

    void Start()
    {
        SpawnEnemies();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemies();
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // generate spawn position
            Vector3 spawnPos = new Vector3(Random.Range(-xRange, xRange), 0, Random.Range(-zRange, zRange));

            // generate spawn rotation
            Vector3 direction = (player.transform.position - spawnPos).normalized;
            Quaternion spawnRotation = Quaternion.LookRotation(direction);

            // spawn enemy
            Instantiate(enemyPrefab, spawnPos, spawnRotation);
        }
        enemyCount++;
    }
}
