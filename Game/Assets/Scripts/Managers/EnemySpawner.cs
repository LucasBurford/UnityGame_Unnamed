﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Int to set max amount of enemies in the world
    [SerializeField]
    private int maxEnemies;

    // Floats to act as random number parameters
    [SerializeField]
    private float num1, num2;

    // Bool to determine if it is night time. True = yes, false = no (i.e. day)
    [SerializeField]
    private bool isNight;

    // Enemy prefab
    public GameObject enemyPrefab;

    // List of enemies
    [SerializeField]
    private List<GameObject> enemyList;

    private void Start()
    {
        enemyList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if it is currently night time
        if (isNight)
        {
            // Generate a random number to decide if we should even consider spawning an enemy
            if (GetRandomNumber(num1, num2) <= 10)
            {
                // Decide if we need to spawn any enemies - Based on maxEnemies
                if (enemyList.Count >= maxEnemies)
                {
                    // Don't spawn any enemies
                    Debug.Log("Too many enemies");
                }
                // Else if there are less than max enemies active
                else if (enemyList.Count < maxEnemies)
                {
                    // Spawn an enemy
                    SpawnEnemy();
                }
            }
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// Instantiate an enemy and add it to list
    /// </summary>
    private void SpawnEnemy()
    {
        // Add enemy into the world
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(16, -14, 0), Quaternion.identity);

        // Add enemy to the list
        enemyList.Add(enemy);
    }

    private float GetRandomNumber(float a, float b)
    {
        float x = Random.Range(a, b);

        return x;
    }

    /// <summary>
    /// Set timeOfDay to true or false. Send true if night time, send false if night time
    /// </summary>
    /// <param name="time">Pass in true if day, false if night</param>
    public void ChangeDayOrNight(bool time)
    {
        isNight = time;
    }
}
