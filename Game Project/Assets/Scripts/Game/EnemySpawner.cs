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
    //Spawn points for enemies
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


    //Enemy spawn logic variables
    [SerializeField] private int enemySpawnLimit = 10; 
    public List<GameObject> enemies = new List<GameObject>();


    //This is for setting game over screen for enemy script and game over screen logic handled there
    [SerializeField] private GameOver GameOverScreen;
    [SerializeField] private Button kamikazeButton;

    private Player playerScript;

    //Camera is required for determining is there a more than one enemy on the screen
    private Camera m_camera;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //This is an event on enemy killed
        Enemy.OnEnemyDied += HandleEnemyKilled;
        InvokeRepeating("SpawnEnemy", 1.0f, 1f);
        //InvokeRepeating("SpawnStars", 1.0f, 4f);
        //InvokeRepeating("SpawnSpeedBuffPowerUp", 1.0f, 4f);
        //InvokeRepeating("SpawnShieldPowerUp", 1.0f, 4f);
        //InvokeRepeating("SpawnKamikazeBuff", 1.0f, 4f);
        InvokeRepeating("SpawnBlackHoleBuff", 1.0f, 1f);
        //InvokeRepeating("SpawnFlareBuff", 1.0f, 4f);
        //InvokeRepeating("SpawnExplodeBuff", 1.0f, 4f);

        m_camera = FindObjectOfType<Camera>();
        playerScript = target.GetComponent<Player>();
    }


    #region Spawners
    //Spawn Shield Speed Up Stars and Enemies
    private void SpawnShieldPowerUp()
    {
        Instantiate(ShieldPrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
    }
    private void SpawnSpeedBuffPowerUp()
    {
        Instantiate(SpeedBoostPrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
    }
    private void SpawnKamikazeBuff()
    {
        Instantiate(KamikazePrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
    }
    private void SpawnBlackHoleBuff()
    {
        Instantiate(BlackHolePrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
    }
    private void SpawnFlareBuff()
    {
        Instantiate(FlarePrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
    }

    private void SpawnExplodeBuff()
    {
        Instantiate(ExplodePrefab, SelectSpawnPointForPUandStars(), Quaternion.identity);
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
    #endregion


    #region Enemy Functions
    //Destroy enemy objects and removing them from the list
    private void HandleEnemyKilled(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            UnityEngine.Object.Destroy(enemy);
            enemies.Remove(enemy);
        }
    }
    //Add enemies to the list
    public void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
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


    //Get collectible spawn point according to the star spawn point objects in the unity
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
            bool MoreThanOneEnemy = CheckMoreThanOneEnemyVisible();
            GameObject enemy1 = GetClosestObjectInList(target, enemies);

            foreach (GameObject enemy in enemies)
            {
                bool isVisible = CheckVisibility(enemy);

                if (isVisible && Kamikaze && MoreThanOneEnemy)
                {
                    if (enemy != enemy1)
                    {
                        audioManager.SFXPlay(audioManager.kamizkaze);
                        
                        Enemy enemyScript = enemy.GetComponent<Enemy>();
                        enemyScript.SetSpeed(60f);
                        enemyScript.SetSteer(30f);
                        enemyScript.SetTarget(enemy1);
                        enemyScript.ActivateKamikaze(KamikazeEffect);
                        Kamikaze = false;
                        StartCoroutine(SelfDestroy(seconds, enemy));
                    }
                }
            }

            playerScript.decreaseKamikazeCount();
            playerScript.adjustBuffUiText(playerScript.kamikazeText, playerScript.kamikazeCount);
        }
    }

    #endregion


   

    #region Explode buff functions
    public void ExplodeEnemies()
    {
        audioManager.SFXPlay(audioManager.laser);
        GameObject explode = Instantiate(ExplodeEffect, transform.position, Quaternion.identity);
        explode.transform.SetParent(transform);
    }
    private IEnumerator SelfDestroy(float sec, GameObject enemy)
    {
        yield return new WaitForSeconds(sec);
        if (enemy != null)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.Die(false, true, false);
        }
    }
    public GameObject GetClosestObjectInList(GameObject target, List<GameObject> findList)
    {
        GameObject newFind = new GameObject();

        Vector3 targetLocation = target.transform.position;
        float distance = 0;
        foreach (GameObject find in findList)
        {
            if (find != null)
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
    //If there is more than one enemy in the screen when player press the button explode them all
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
    #endregion

    #endregion

}

