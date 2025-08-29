using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private GameObject PrefabRef;
    private GameObject Star;
    public AudioManager audioManager;


    //Player
    [SerializeField] private GameObject target;
    //Spawn points for pooledEnemies
    [SerializeField] private Transform minSpawn;
    [SerializeField] private Transform maxSpawn;
    //Spawn points for collectibles
    [SerializeField] private Transform minSpawnStar;
    [SerializeField] private Transform maxSpawnStar;

    //Collectible Prefabs
    [SerializeField] private GameObject StarPrefab;
    [SerializeField] private GameObject SpeedBoostPrefab;
    [SerializeField] private GameObject ShieldPrefab;
    [SerializeField] private GameObject KamikazePrefab;
    [SerializeField] private GameObject BlackHolePrefab;
    [SerializeField] private GameObject FlarePrefab;
    [SerializeField] private GameObject ExplodePrefab;


    //Kamikaze Effect Particle System
    [SerializeField] private GameObject KamikazeEffect;
    [SerializeField] private GameObject ExplodeEffect;

    [SerializeField] private SpriteRenderer kamikazeSprite;
    [SerializeField] private SpriteRenderer explodeSprite;

    //Enemy spawn logic variables
    [SerializeField] private int pooledObjectLimit = 10; 
    private List<GameObject> pooledEnemies = new List<GameObject>();
    private List<GameObject> pooledStars = new List<GameObject>();
    private List<GameObject> pooledShields = new List<GameObject>();
    private List<GameObject> pooledKamikazes = new List<GameObject>();
    private List<GameObject> pooledBlackHoles = new List<GameObject>();
    private List<GameObject> pooledFlares = new List<GameObject>();
    private List<GameObject> pooledExplodes = new List<GameObject>();
    private List<GameObject> pooledSpeedBuffs = new List<GameObject>();

    //This is for setting game over screen for enemy script and game over screen logic handled there
    [SerializeField] private GameOver GameOverScreen;
    [SerializeField] private Button kamikazeButton;


    private Player playerScript;
    private CollectibleController controller;

    //Camera is required for determining is there a more than one enemy on the screen
    private Camera m_camera;

    private void Start()
    {
        controller = FindAnyObjectByType<CollectibleController>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //This is an event on enemy killed
        Enemy.OnEnemyDied += HandleEnemyKilled;

        for (int i = 0; i < pooledObjectLimit; i++)
        {
            SpawnBlackHoleBuff();
            SpawnEnemy();
            SpawnFlareBuff();
            SpawnExplodeBuff();
            SpawnKamikazeBuff();
            SpawnSpeedBuffPowerUp();
            SpawnStars();
            SpawnShieldPowerUp();
        }

        InvokeRepeating("SpawnPooledEnemies", 1f, 2f);
        InvokeRepeating("SpawnPooledStars", 1f, 3f);

        InvokeRepeating("SpawnPooledShields", 1f, 5f);
        InvokeRepeating("SpawnPooledSpeedBuffs", 1f, 5f);

        InvokeRepeating("SpawnPooledKamikazes", 1f, 5f);
        InvokeRepeating("SpawnPooledFlares", 1f, 7f);

        InvokeRepeating("SpawnPooledExplodes", 1f, 10f);
        InvokeRepeating("SpawnPooledBlackHoles", 1f, 1f);


        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerScript = target.GetComponent<Player>();
    }
    

    #region Spawners
    //Spawn Shield Speed Up Stars and Enemies
    private void SpawnShieldPowerUp()
    {
        GameObject obj = Instantiate(ShieldPrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
        obj.SetActive(false);
        pooledShields.Add(obj);
    }
    private void SpawnSpeedBuffPowerUp()
    {
        GameObject obj = Instantiate(SpeedBoostPrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
        obj.SetActive(false);
        pooledSpeedBuffs.Add(obj);
    }
    private void SpawnKamikazeBuff()
    {
        GameObject obj = Instantiate(KamikazePrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
        obj.SetActive(false);
        pooledKamikazes.Add(obj);
    }
    private void SpawnBlackHoleBuff()
    {
        GameObject obj = Instantiate(BlackHolePrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
        obj.SetActive(false);
        pooledBlackHoles.Add(obj);
    }
    private void SpawnFlareBuff()
    {
        GameObject obj = Instantiate(FlarePrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
        obj.SetActive(false);
        pooledFlares.Add(obj);
    }
    private void SpawnExplodeBuff()
    {
        GameObject obj = Instantiate(ExplodePrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
        obj.SetActive(false);
        pooledExplodes.Add(obj);
    }
    private void SpawnStars()
    {
        Star = Instantiate(StarPrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
        
        Star.SetActive(false);
        pooledStars.Add(Star);
        controller.AddToStars(Star);
    }
    private void SpawnEnemy()
    {
        PrefabRef = Instantiate(enemyPrefab, SelectEnemySpawnPoint(), Quaternion.identity);

        Enemy enemyScript = PrefabRef.GetComponent<Enemy>();

        enemyScript.SetTarget(target);
        enemyScript.SetGameOverScreen(GameOverScreen);
        PrefabRef.SetActive(false);
        RegisterEnemy(PrefabRef);
    }
    #endregion

    #region SpawnPooledObjects

    private void SpawnPooledStars()
    {
        GameObject star = GetPooledObject(pooledStars);

        if (star != null)
        {
            star.transform.position = SelectSpawnPointForPUandStars();
            star.SetActive(true);
        }
    }
    private void SpawnPooledShields()
    {
        GameObject shield = GetPooledObject(pooledShields);

        if (shield != null)
        {
            shield.transform.position = SelectSpawnPointForPUandStars();
            shield.SetActive(true);
        }
    }
    private void SpawnPooledSpeedBuffs()
    {
        GameObject speedBuff = GetPooledObject(pooledSpeedBuffs);

        if (speedBuff != null)
        {
            speedBuff.transform.position = SelectSpawnPointForPUandStars();
            speedBuff.SetActive(true);
        }
    }
    private void SpawnPooledKamikazes()
    {
        GameObject kamikaze = GetPooledObject(pooledKamikazes);

        if (kamikaze != null)
        {
            kamikaze.transform.position = SelectSpawnPointForPUandStars();
            kamikaze.SetActive(true);
        }
    }
    private void SpawnPooledFlares()
    {
        GameObject flare = GetPooledObject(pooledFlares);

        if (flare != null)
        {
            flare.transform.position = SelectSpawnPointForPUandStars();
            flare.SetActive(true);
        }
    }
    private void SpawnPooledBlackHoles()
    {
        GameObject blackHole = GetPooledObject(pooledBlackHoles);

        if (blackHole != null)
        {
            blackHole.transform.position = SelectSpawnPointForPUandStars();
            blackHole.SetActive(true);
        }
    }
    private void SpawnPooledExplodes()
    {
        GameObject explode = GetPooledObject(pooledExplodes);

        if (explode != null)
        {
            explode.transform.position = SelectSpawnPointForPUandStars();
            explode.SetActive(true);
        }
    }
    private void SpawnPooledEnemies()
    {
        GameObject enemy = GetPooledObject(pooledEnemies);

        if (enemy != null)
        {
            enemy.transform.position = SelectEnemySpawnPoint();
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.SetTarget(target);
            enemyScript.enabled = true;
            enemyScript.DeActivateKamikaze();
            enemy.SetActive(true);
        }
    }
    private GameObject GetPooledObject(List<GameObject> pooledObjects)
    {
        foreach (var pooledObject in pooledObjects)
        {
            if (!pooledObject.activeInHierarchy)
            {
                return pooledObject;
            }
        }
        return null;
    }

    #endregion

    #region Enemy Functions
    //Destroy enemy objects and removing them from the list
    private void HandleEnemyKilled(GameObject enemy)
    {
        enemy.SetActive(false);
    }
    //Add pooledEnemies to the list
    public void RegisterEnemy(GameObject enemy)
    {
        pooledEnemies.Add(enemy);
        controller.AddToEnemies(enemy);
    }
    //Get enemy spawn point according to the spawn point objects in the unity 
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

    #endregion


    //Get collectible spawn point according to the enemy spawn point objects in the unity
    private Vector3 SelectSpawnPointForPUandStars()
    {
        Vector3 spawnPoint = new Vector3();

        spawnPoint.x = UnityEngine.Random.Range(minSpawnStar.position.x, maxSpawnStar.position.x);
        spawnPoint.y = UnityEngine.Random.Range(minSpawnStar.position.y, maxSpawnStar.position.y);
        return spawnPoint;
    }


    #region PowerUp Effects

    #region Slow Down Enemies
    private void SlowDownEnemies(bool SlowDown)
    {
        foreach (GameObject enemy in pooledEnemies)
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
    public IEnumerator SlowDown()
    {
        SlowDownEnemies(true);
        yield return new WaitForSeconds(10);
        SlowDownEnemies(false);
    }
    #endregion





    //2 power ups use this function (kamikaze and explode)
    public bool CheckVisibility(GameObject gameObject)
    {
        if (gameObject != null)
        {
            if (gameObject.GetComponent<SpriteRenderer>().isVisible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }



    #region Kamikaze
    public void KamikazeEnemies()
    {
        if (playerScript.getKamikazeCount() > 0)
        {
            float seconds = 3;
            bool Kamikaze = true;
            GameObject enemy1 = GetClosestObjectInList(target, pooledEnemies);
            bool moreThanOne = CheckIfMoreThanOneEnemyVisible(pooledEnemies);

            foreach (GameObject enemy in pooledEnemies)
            {
                bool isVisible = enemy.GetComponent<SpriteRenderer>().isVisible;

                if (isVisible && Kamikaze)
                {
                    if (enemy != enemy1 && moreThanOne)
                    {
                        Enemy enemyScript = enemy.GetComponent<Enemy>();
                        enemyScript.SetSpeed(90f);
                        enemyScript.SetSteer(30f);
                        enemyScript.SetTarget(enemy1);
                        enemyScript.ActivateKamikaze(KamikazeEffect);
                        Kamikaze = false;
                        StartCoroutine(SelfDestroy(seconds, enemy));
                    }
                    else if (enemy == enemy1 && !moreThanOne)
                    {
                        Enemy enemyScript = enemy.GetComponent<Enemy>();
                        enemyScript.SetTarget(target);
                        enemyScript.SetSpeed(60f);
                        enemyScript.SetSteer(25f);
                        Kamikaze = false;
                        enemyScript.Die(false, true, false, false, false);
                    }
                }
            }

            playerScript.decreaseKamikazeCount();
            playerScript.adjustBuffUiText(kamikazeSprite, playerScript.kamikazeCount, false);
        }
    }

    #endregion


    private bool CheckIfMoreThanOneEnemyVisible(List<GameObject> enemies)
    {
        int counter = 0;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                Vector3 viewPos = Camera.main.WorldToViewportPoint(enemy.transform.position);

                if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
                {
                    counter++;
                    if (counter > 1)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    #region Explode buff functions
    public void ExplodeEnemies()
    {
        audioManager.SFXPlayOneShot(audioManager.laserSFX);
        GameObject explode = Instantiate(ExplodeEffect, transform.position, Quaternion.identity);
        explode.transform.SetParent(transform);

        playerScript.decreaseExplodeCount();
        playerScript.adjustBuffUiText(explodeSprite, playerScript.explodeCount, false);
    }
    private IEnumerator SelfDestroy(float sec, GameObject enemy)
    {
        yield return new WaitForSeconds(sec);
        if (enemy != null)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.SetTarget(target);
            enemyScript.SetSpeed(60f);
            enemyScript.SetSteer(25f);
            enemyScript.Die(false, true, false, false, false);
        }
    }
    public GameObject GetClosestObjectInList(GameObject target, List<GameObject> findList)
    {
        GameObject newFind = new GameObject();

        Vector3 targetLocation = target.transform.position;
        float distance = 0;
        foreach (GameObject find in findList)
        {
            if (find != null && find.activeInHierarchy)
            {
                Vector3 findLocation = find.transform.position;

                float distanceNew = Vector3.Distance(targetLocation, findLocation);

                if (distance == 0 || distance > distanceNew)
                {
                    distance = distanceNew;
                    newFind = find;
                }
            }
        }
        return newFind;
    }
    #endregion

    #endregion
}

