using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using static UnityEngine.EventSystems.EventTrigger;

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
    


    [SerializeField] private int enemySpawnLimit = 10; 

    public List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private GameOver GameOverScreen;

    private Camera m_camera;

    private void Start()
    {
        Enemy.OnEnemyDied += HandleEnemyKilled;
        InvokeRepeating("SpawnEnemy", 1.0f, 2.0f);
        InvokeRepeating("SpawnStars", 1.0f, 3.5f);
        InvokeRepeating("SpawnSlowTimePowerUp", 1.0f, 15.0f);
        InvokeRepeating("SpawnShieldPowerUp", 1.0f, 15.0f);
        m_camera = FindObjectOfType<Camera>();
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
            UnityEngine.Object.Destroy(enemy);
            enemies.Remove(enemy);
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
                    enemy1.SetSpeed(45f);
                }
                else
                {
                    enemy1.SetSpeed(30f);
                }
            }
        }
    }

    public void ExplodeEnemies()
    {
        List<GameObject> enemyRefList = new List<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            bool isVisible = CheckVisibility(enemy);

            if (isVisible)
            {
                enemyRefList.Add(enemy);
            }
        }

        foreach (var enemy1 in enemyRefList)
        {
            Enemy enemyScript = enemy1.GetComponent<Enemy>();
            enemyScript.Die(true, false, false);
            enemies.Remove(enemy1);
        }
    }

    public void KamikazeEnemies()
    {
        float seconds = 3;
        bool Kamikaze = true;
        bool MoreThanOneEnemy = CheckMoreThanOneEnemyVisible();
        GameObject enemy1 = GetClosestObjectInList(target, enemies);


        foreach (GameObject enemy in enemies)
        {
            bool isVisible = CheckVisibility(enemy);

            if (isVisible && Kamikaze && MoreThanOneEnemy)
            {
                if (enemy != enemy1)
                {
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    enemyScript.SetSpeed(60f);
                    enemyScript.SetSteer(30f);
                    enemyScript.SetTarget(enemy1);
                    enemyScript.ChangeColor();
                    Kamikaze = false;
                    StartCoroutine(SelfDestroy(seconds, enemy));
                }
            }
        }
    }

    //
    private IEnumerator SelfDestroy(float sec, GameObject enemy)
    {
        yield return new WaitForSeconds(sec);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.Die(false, true, false);
    }


    public GameObject GetClosestObjectInList(GameObject target, List<GameObject> findList)
    {
        GameObject newFind = new GameObject();

        Vector3 targetLocation = target.transform.position;
        float distance = 0;
        foreach (GameObject find in findList)
        {
            Vector3 findLocation = find.transform.position;

            float distanceNew = Vector3.Distance(targetLocation, findLocation);

            if (distance == 0 || distance > distanceNew)
            {
                distance = distanceNew;
                newFind = find;
            }
        }
        return newFind;
    }

    public bool CheckMoreThanOneEnemyVisible()
    {
        List<GameObject> VisibleEnemy = new List<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            bool isVisible = CheckVisibility(enemy);

            if (isVisible)
            {
                VisibleEnemy.Add(enemy);
            }
        }

        if (VisibleEnemy.Count > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool CheckVisibility(GameObject gameObject)
    {
        Vector3 screenPos = m_camera.WorldToScreenPoint(gameObject.transform.position);
        bool onScreen = screenPos.x > 0f && screenPos.x < UnityEngine.Screen.width && screenPos.y > 0f && screenPos.y < UnityEngine.Screen.height;

        if (onScreen && gameObject.GetComponent<SpriteRenderer>().isVisible)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator SlowDown()
    {
        SlowDownEnemies(true);
        yield return new WaitForSeconds(10);
        SlowDownEnemies(false);
    }
}

