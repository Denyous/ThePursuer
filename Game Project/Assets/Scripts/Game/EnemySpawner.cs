using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] private GameObject StarPrefab;
    [SerializeField] private GameObject SlowTimePrefab;
    [SerializeField] private GameObject ShieldPrefab;
    private GameObject Star;
    private GameObject SlowTimePU;
    [SerializeField] private Camera MainCamera;


    private GameObject PrefabRef;

    [SerializeField] private GameObject target;
    [SerializeField] private Transform minSpawn;
    [SerializeField] private Transform maxSpawn;
    [SerializeField] private Transform minSpawnStar;
    [SerializeField] private Transform maxSpawnStar;


    [SerializeField] private int enemySpawnLimit = 3; 

    public List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private GameOver GameOverScreen;


    private void Start()
    {
        Enemy.OnEnemyDied += HandleEnemyKilled;
        InvokeRepeating("SpawnEnemy", 1.0f, 2.0f);
        InvokeRepeating("SpawnStars", 1.0f, 3.5f);
        InvokeRepeating("SpawnSlowTimePowerUp", 1.0f, 15.0f);
        InvokeRepeating("SpawnShieldPowerUp", 1.0f, 15.0f);
    }

    private void SpawnShieldPowerUp()
    {
        Instantiate(ShieldPrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
    }
    private void SpawnSlowTimePowerUp()
    {
        Instantiate(SlowTimePrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
    }
    private void SpawnStars()
    {
        Star = Instantiate(StarPrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
        CollectibleController controller = Star.GetComponent<CollectibleController>();

        controller.SetPlayer(target);
    }

    private void SpawnEnemy()
    {
        if (enemies.Count < enemySpawnLimit && target != null)
        {
            PrefabRef = Instantiate(enemyPrefab, SelectEnemySpawnPoint(), Quaternion.identity);

            Enemy enemyScript = PrefabRef.GetComponent<Enemy>();

            enemyScript.SetTarget(target);
            enemyScript.SetGameOverScreen(GameOverScreen);

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

    private Vector3 SelectEnemySpawnPoint()
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

    private Vector3 SelectSpawnPointForPUandStars()
    {
        Vector3 spawnPoint = new Vector3();

        spawnPoint.x = UnityEngine.Random.Range(minSpawnStar.position.x, maxSpawnStar.position.x);
        spawnPoint.y = UnityEngine.Random.Range(minSpawnStar.position.y, maxSpawnStar.position.y);
        return spawnPoint;
    }

    private void SlowDownEnemies(bool SlowDown)
    {
        foreach (GameObject enemy in enemies)
        {
            Enemy enemy1 = enemy.GetComponent<Enemy>();

            if (enemy1 != null)
            {
                if (SlowDown != true)
                {
                    enemy1.speed = 45f;
                }
                else
                {
                    enemy1.speed = 30f;
                }
            }
        }
    }

    public IEnumerator SlowDown()
    {
        SlowDownEnemies(true);
        yield return new WaitForSeconds(10);
        SlowDownEnemies(false);
    }
}

