using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms;

public class Enemy : MonoBehaviour
{
    //Variables of the Enemy itself
    [SerializeField] private float speed = 45f;
    [SerializeField] private float steer = 15f;
    [SerializeField] private GameObject rocketFire;
    private GameObject fire;
    private GameObject kamikazeEffect;
    Rigidbody2D rb;
    private AudioManager audioManager;

    //This is an event to be handled in EnemySpawner script
    public static event System.Action<GameObject> OnEnemyDied;

    //Player
    private GameObject target;

    private GameOver GameOverScreen;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        fire = Instantiate(rocketFire, transform, false);
        fire.transform.localPosition = new Vector3(0f, -2.1f, 1f);
    }

    #region Follow Player Logic
    //Enemies follow player
    private void FixedUpdate()
    {
        if (target != null)
        {
            rb.velocity = speed * Time.fixedDeltaTime * 15f * transform.up;
            Vector2 direction = (target.transform.position - transform.position).normalized;
            float rotationSteer = Vector3.Cross(transform.up, direction).z;
            rb.angularVelocity = rotationSteer * steer * 10f;
        }
    }
    #endregion

    //Showing game over screen
    public void GameOver(int score, int highscore)
    {
        GameOverScreen.Setup(score, highscore);
    }


    #region Setting Variables for Outside Use

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



    //Killing pooledEnemies
    public void Die(bool explodeBuff, bool kamikaze, bool Flare, bool BlackHole, bool Shield)
    {
        if (kamikazeEffect != null)
        {
            kamikazeEffect.SetActive(false);
        }
        OnEnemyDied?.Invoke(this.gameObject);
        if (!kamikaze && !explodeBuff && !Flare && !BlackHole && !Shield)
        {
            GameController.instance.HandleCollision(transform.position, true, false);
        }
        else
        {
            if (BlackHole)
            {
                GameController.instance.HandleCollision(transform.position, false, true);
            }
            else
            {
                GameController.instance.HandleCollision(transform.position, false, false);   
            }
        }
    }



    public void ActivateKamikaze(GameObject ParticleSystem)
    {
        kamikazeEffect = Instantiate(ParticleSystem, transform.position, Quaternion.identity);
        kamikazeEffect.transform.SetParent(transform);
        kamikazeEffect.SetActive(true);
    }

    public void DeActivateKamikaze()
    {
        if (kamikazeEffect != null)
        {
            kamikazeEffect.SetActive(false);
        }
    }
    #region Handling Triggers

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("kalkan"))
        {
            Player player = target.gameObject.GetComponent<Player>();
            
            player.Shield = false;

            audioManager.SFXPlay(audioManager.shieldOff);
            audioManager.SFXPlay(audioManager.explosion);

            player.ChangeShieldState();

            Die(false, false, false, false, true);
        }
    }
    #endregion
    #region Handling Collisions
    //Handling enemy collisions with player, other pooledEnemies, and flares
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Player>(out Player player))
            {
                if (player != null && player.Shield == false)
                {
                    //int score = player.GetScore();
                    //int highscore = PlayerPrefs.GetInt("HighScore", 0);
                    
                    //if (highscore < score)
                    //{
                    //    highscore = score;
                    //    PlayerPrefs.SetInt("HighScore", highscore);
                    //}

                    //player.CreateExplosion();
                    //Object.Destroy(target.gameObject);
                    //Object.Destroy(gameObject);
                    //GameOver(score, highscore);
                }
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            audioManager.SFXPlay(audioManager.explosion);
            Die(false, false, false, false, false);
        }
    }

    //Flare particle collision
    void OnParticleCollision()
    {
        audioManager.SFXPlay(audioManager.explosion);
        Die(false, false, true, false, false);
    }

    #endregion
}
