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

    public void HandleCollision(Vector3 position)
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
}
