using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool Exploded = false;
    private int ExplodeCount = 0;

    [SerializeField] private GameObject ExplosionPrefab;
    public void ResetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void HandleCollision(Vector3 position, bool ExplodeBuff, bool Kamikaze, bool Flare)
    {
        if (!ExplodeBuff && !Kamikaze && !Flare)
        {
            if (!Exploded && ExplodeCount == 0)
            {
                Instantiate(ExplosionPrefab, position, Quaternion.identity);
                Exploded = true;
                ExplodeCount++;
            }
            else
            {
                ExplodeCount = 0;
                Exploded = false;
            }
        }
        else if (Kamikaze)
        {
            Instantiate(ExplosionPrefab, position, Quaternion.identity);
        }
        else if (ExplodeBuff)
        {
            Instantiate(ExplosionPrefab, position, Quaternion.identity);
        }
        else if (Flare)
        {
            Instantiate(ExplosionPrefab, position, Quaternion.identity);
        }
    }
}
