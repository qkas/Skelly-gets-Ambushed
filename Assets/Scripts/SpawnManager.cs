using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;

    private float waveEndDelay = 2f;
    private bool coroutineStarted = false;

    public int startEnemyCount = 5;
    private int waveCount = 1;
    private int enemiesToSpawn;

    private float xRange = 9f;
    private float zRange = 9f;
    
    void Start()
    {
        enemiesToSpawn = startEnemyCount;
        SpawnEnemies();
    }

    void Update()
    {
        // if all enemies are dead, start new wave after delay
        if (allEnemiesDead() && !coroutineStarted)
        {
            // start new wave after delay
            StartCoroutine(delayNextWave(waveEndDelay));
        }
    }

    private bool allEnemiesDead()
    {
        // check if all enemies are dead
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (!enemyScript.isDead)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator delayNextWave(float time)
    {
        coroutineStarted = true;
        yield return new WaitForSeconds(time);

        // destroy dead enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // start new wave
        waveCount++;
        SpawnEnemies();
        coroutineStarted = false;
    }

    private void SpawnEnemies()
    {
        // Spawn new wave of enemies
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // generate spawn position
            Vector3 spawnPos = new Vector3(Random.Range(-xRange, xRange), 0, Random.Range(-zRange, zRange));

            // generate spawn rotation
            Vector3 direction = (player.transform.position - spawnPos).normalized;
            Quaternion spawnRotation = Quaternion.LookRotation(direction);

            // spawn enemy
            Instantiate(enemyPrefab, spawnPos, spawnRotation);
        }

        enemiesToSpawn++;
    }
}
