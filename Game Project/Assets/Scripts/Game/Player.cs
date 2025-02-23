using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using Unity.Mathematics;

public class Player : MonoBehaviour
{

    [SerializeField] private float Speed = 35f;
    [SerializeField] private float Steer = 1.5f;
    [SerializeField] private GameObject Explosion;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private FixedJoystick Joystick;
    [SerializeField] private float RotationSpeed = 3f;
    [SerializeField] private GameObject flares;
    [SerializeField] private Transform flareLocation;

    private EnemySpawner Spawner;

    private int Score = 0;


    Rigidbody2D rb;
    private float targetAngle;
    public bool Shield;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Spawner = FindAnyObjectByType<EnemySpawner>();
    }



    //Handling collision for Shield
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (Shield != true)
            {
                Instantiate(Explosion, transform.position, UnityEngine.Quaternion.identity);
            }
        }
    }

    //Handling collisions for Stars, Shield, Slow Time, Kamikaze
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Star"))
        {
            CollectibleController controller =
                collision.gameObject.GetComponent<CollectibleController>();

            controller.Kill();
            Score++;
            ScoreText.text = Score.ToString();
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {
            Object.Destroy(collision.gameObject);
            Shield = true;
        }
        else if (collision.gameObject.CompareTag("SlowTime"))
        {
            Object.Destroy(collision.gameObject);
            StartCoroutine(Spawner.SlowDown());
        }
        else if (collision.gameObject.CompareTag("Kamikaze"))
        {
            Object.Destroy(collision.gameObject);
        }
    }

    //Player movement
    private void FixedUpdate()
    {
        float x = Joystick.Horizontal;
        float y = Joystick.Vertical;

        if (Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f)
        {
            targetAngle = Mathf.Atan2(y, x) * Mathf.Rad2Deg - 90f; 
        }

        UnityEngine.Quaternion currentRotation = transform.rotation;

        UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.
            Euler(0, 0, targetAngle);

        transform.rotation = UnityEngine.Quaternion.Lerp
            (currentRotation, targetRotation, RotationSpeed * Time.fixedDeltaTime / Steer);

        rb.velocity = transform.up * Speed * Time.fixedDeltaTime * 10f;
    }
    
    //Getting score to show in game over screen
    public int GetScore()
    {
        return Score;
    }

    public void CreateFlares()
    {
        UnityEngine.Quaternion currentRotation = transform.rotation;

        Instantiate(flares, flareLocation.position, currentRotation);
    }
}
