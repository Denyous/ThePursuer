using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private GameObject PrefabRef;

    [SerializeField] private GameObject target;
    [SerializeField] private Transform minSpawn;
    [SerializeField] private Transform maxSpawn;

    [SerializeField] private int enemySpawnLimit = 3; 

    private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        Enemy.OnEnemyDied += HandleEnemyKilled;
        InvokeRepeating("SpawnEnemy", 1.0f, 2.0f);
    }
    
    private void SpawnEnemy()
    {
        if (enemies.Count < enemySpawnLimit && target != null)
        {
            PrefabRef = Instantiate(enemyPrefab, SelectSpawnPoint(), Quaternion.identity);

            Enemy enemyScript = PrefabRef.GetComponent<Enemy>();

            enemyScript.SetTarget(target.transform);

            RegisterEnemy(PrefabRef);
        }
    }

    private void HandleEnemyKilled(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            UnityEngine.Object.Destroy(enemy);
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    private Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;

        bool spawnVerticalEdge = UnityEngine.Random.Range(0f, 1f) > 0.5f;

        if (spawnVerticalEdge)
        {
            spawnPoint.y = UnityEngine.Random.Range(minSpawn.position.y, maxSpawn.position.y);

            if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
            {
                spawnPoint.x = maxSpawn.position.x;
            }
            else
            {
                spawnPoint.x = minSpawn.position.x;
            }
        }
        else
        {
            spawnPoint.x = UnityEngine.Random.Range(minSpawn.position.x, maxSpawn.position.x);

            if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
            {
                spawnPoint.y = maxSpawn.position.y;
            }
            else
            {
                spawnPoint.y = minSpawn.position.y;
            }
        }

        return spawnPoint;
    }



}

