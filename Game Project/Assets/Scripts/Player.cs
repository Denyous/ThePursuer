using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float steer = 30f;
    [SerializeField] private GameObject Explosion;
    [SerializeField] private GameObject StarPreFab;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private FixedJoystick Joystick;
    [SerializeField] private float rotationSpeed = 5f;

    private Animator animator;
    private int Score = 0;

    Rigidbody2D rb;
    private Vector2 InputVector;
    private float input;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        input = Joystick.Horizontal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
        }
        else if (collision.gameObject.CompareTag("Star"))
        {
            CollectibleController controller = collision.gameObject.GetComponent<CollectibleController>();
            controller.Kill();            
            Score++;
            ScoreText.text = Score.ToString();
        }
    }


    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed * Time.deltaTime * 10f;

        float rotation = input * 30f;

        transform.rotation = Quaternion.Euler(0f, rotation, rotation);
        Debug.Log($"{transform.rotation}");
    }



    private void PlaceHolder()
    {
        //rb.velocity = 10f * speed * Time.fixedDeltaTime * transform.up;
        //rb.velocity = transform.up * speed * Time.fixedDeltaTime * 10f;

        //float rotation = 10f;

        //float rotationNormalized = InputVector.x * rotation;

        //rb.angularVelocity = InputVector.x * steer * 10f;
        //Debug.Log($"{rotationNormalized}");

        //transform.rotation = Quaternion.Euler(0, 0, -rotationNormalized);


        //transform.Rotate(new Vector3(0, 0, -20) * Time.deltaTime * _speed, Space.World);
    }
    public int GetScore()
    {
        return Score;
    }
}
