using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 45f;
    [SerializeField] private float steer = 15f;

    //This is an event to be handled in EnemySpawner script
    public static event System.Action<GameObject> OnEnemyDied;

    //Player
    private GameObject target;

    Rigidbody2D rb;

    private GameOver GameOverScreen;

    //Game Controller for creating explosion prefabs
    private GameController GameController;


    private GameObject KamikazeEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameController = FindObjectOfType<GameController>();

    }



    //Showing game over screen
    public void GameOver(int score)
    {
        GameOverScreen.Setup(score);
    }


    #region Setting Enemy Variables
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void SetSteer(float steer)
    {
        this.steer = steer;
    }
    public void SetGameOverScreen(GameOver gameOverScreen)
    {
        GameOverScreen = gameOverScreen;
    }
    #endregion



    //Killing enemies
    public void Die(bool explodeBuff, bool kamikaze, bool Flare)
    {
        OnEnemyDied?.Invoke(this.gameObject);
        GameController.HandleCollision(transform.position, explodeBuff, kamikaze, Flare);
    }



    //Change color for kamikaze buff
    public void ChangeColor(GameObject ParticleSystem)
    {
        GameObject newParticle = Instantiate(ParticleSystem, transform.position, Quaternion.identity);
        newParticle.transform.SetParent(transform);
    }



    #region Handling Collisions
    //Handling enemy collisions with player, other enemies, and flares
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = target.gameObject.GetComponent<Player>();

            if (player != null && player.Shield == false)
            {
                int score = player.GetScore();

                Object.Destroy(target.gameObject);
                Object.Destroy(this.gameObject);

                GameOver(score);
            }
            else
            {

                player.Shield = false;

                player.ChangeShieldState();

                Object.Destroy(this.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Die(false, false, false);
        }
        else if (collision.gameObject.CompareTag("Flare"))
        {
            Die(false, false, false);
        }
    }

    //Flare particle collision
    void OnParticleCollision()
    {
        Die(false, false, true);
    }

    #endregion



    #region Follow Player Logic
    //Enemies follow player
    private void FixedUpdate()
    {
        if (target != null)
        {
            rb.velocity = transform.up * speed * Time.fixedDeltaTime * 10f;
            Vector2 direction = (target.transform.position - transform.position).normalized;
            float rotationSteer = Vector3.Cross(transform.up, direction).z;
            rb.angularVelocity = rotationSteer * steer * 10f;
        }
    }
    #endregion
}
