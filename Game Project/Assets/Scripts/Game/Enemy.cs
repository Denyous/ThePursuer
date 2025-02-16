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

    public static event System.Action<GameObject> OnEnemyDied;

    private GameObject target;
    private Animator animator;

    Rigidbody2D rb;
    float input;

    private GameOver GameOverScreen;
    private GameController GameController;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        input = Input.GetAxis("Horizontal");
    }

    public void GameOver(int score)
    {
        GameOverScreen.Setup(score);
    }

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
    public void Die(bool explodeBuff, bool kamikaze, bool Flare)
    {
        OnEnemyDied?.Invoke(this.gameObject);
        GameController.HandleCollision(transform.position, explodeBuff, kamikaze, Flare);
    }

    public void ChangeColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }

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

    void OnParticleCollision()
    {
        Die(false, false, true);
    }

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
}
