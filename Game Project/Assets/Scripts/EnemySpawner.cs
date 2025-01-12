using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    [SerializeField] private GameObject target;
    [SerializeField] private Transform minSpawn;
    [SerializeField] private Transform maxSpawn;

    [SerializeField] private int enemySpawnLimit = 15; 
    private int enemiesSpawned = 0; 

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1.0f, 2.0f);
    }
    
    void SpawnEnemy()
    {
        if (enemiesSpawned < enemySpawnLimit && target != null)
        {
            enemyPrefab = Instantiate(enemyPrefab, SelectSpawnPoint(), Quaternion.identity);

            Enemy enemyScript = enemyPrefab.GetComponent<Enemy>();

            enemyScript.SetTarget(target.transform);

            enemiesSpawned++;
        }
    }
    
    private Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;

        bool spawnVerticalEdge = Random.Range(0f, 1f) > 0.5f;

        if (spawnVerticalEdge)
        {
            spawnPoint.y = Random.Range(minSpawn.position.y, maxSpawn.position.y);

            if (Random.Range(0f, 1f) > 0.5f)
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
            spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);

            if (Random.Range(0f, 1f) > 0.5f)
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
    public int GetEnemiesSpawned()
    {
        return enemiesSpawned;
    }
}

