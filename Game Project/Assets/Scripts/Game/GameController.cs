using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public int Exploded = 0;

    [SerializeField] private GameObject ExplosionPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void ResetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Creating explosion for pooledEnemies 
    public void HandleCollision(Vector3 position, bool enemiesCollideWithEachOther, bool BlackHole)
    {
        if (enemiesCollideWithEachOther)
        {
            if (Exploded % 2 != 0)
            {
                Instantiate(ExplosionPrefab, position, Quaternion.identity);
                Exploded++;
            }
            else
            {
                Exploded++;
            }
        }
        else if (BlackHole)
        {
            Exploded = 0;
        }
        else
        {
            Exploded = 0;
            Instantiate(ExplosionPrefab, position, Quaternion.identity);
        }
    }
}
