using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool Exploded = false;

    [SerializeField] private GameObject ExplosionPrefab;
    public void ResetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Creating explosion for enemies 
    public void HandleCollision(Vector3 position, bool ExplodeBuff, bool Kamikaze, bool Flare)
    {
        //if (!ExplodeBuff && !Kamikaze && !Flare)
        //{
        //    Instantiate(ExplosionPrefab, position, Quaternion.identity);
        //}
        //else if (Kamikaze)
        //{
        //    Instantiate(ExplosionPrefab, position, Quaternion.identity);
        //}
        //else if (ExplodeBuff)
        //{
        //    Instantiate(ExplosionPrefab, position, Quaternion.identity);
        //}
        //else if (Flare)
        //{
        //    Instantiate(ExplosionPrefab, position, Quaternion.identity);
        //}

        Instantiate(ExplosionPrefab, position, Quaternion.identity);
    }
}
