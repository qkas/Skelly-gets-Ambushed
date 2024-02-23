using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;
    private float spawnDistance = 5f;
    private float xRange = 10f;
    private float zRange = 10f;
    private int enemyCount = 5;
    private int enemiesAlive;

    void Start()
    {
        SpawnEnemies();
    }

    void Update()
    {
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemiesAlive == 0)
        {
            SpawnEnemies();
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos;
            float distance;
            do
            {
                spawnPos = new Vector3(Random.Range(-xRange, xRange), 0, Random.Range(-zRange, zRange));
                distance = Vector3.Distance(spawnPos, player.transform.position);
            } while (distance < spawnDistance);

            // generate spawn rotation
            Vector3 direction = (player.transform.position - spawnPos).normalized;
            Quaternion spawnRotation = Quaternion.LookRotation(direction);

            // spawn enemy
            Instantiate(enemyPrefab, spawnPos, spawnRotation);
        }
        enemyCount++;
    }
}
