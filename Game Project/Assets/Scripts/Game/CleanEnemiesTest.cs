using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanEnemiesTest : MonoBehaviour
{
    public EnemySpawner cleanEnemies;

    void Start()
    {
        cleanEnemies = GameObject.Find("Spawner").GetComponent<EnemySpawner>(); 
    }

    public void CleanOnTrigger()
    {

        cleanEnemies.ExplodeEnemies();
    }
}
