using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    [SerializeField] private GameObject target;

    public int enemySpawnLimit = 3; 
    private int enemiesSpawned = 0; 

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1.0f, 2.0f); 
    }

    void SpawnEnemy()
    {
        if (enemiesSpawned < enemySpawnLimit && target != null)
        {
            enemyPrefab = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            Enemy enemyScript = enemyPrefab.GetComponent<Enemy>();

            enemyScript.SetTarget(target.transform);

            enemiesSpawned++;
        }
    }
    

    public int GetEnemiesSpawned()
    {
        return enemiesSpawned;
    }
}

