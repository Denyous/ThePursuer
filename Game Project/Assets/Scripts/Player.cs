using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float steer = 2.2f;
    [SerializeField] private GameObject Explosion;
    [SerializeField] private GameObject StarPreFab;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private FixedJoystick Joystick;
    [SerializeField] private float rotationSpeed = 0.5f;

    private Animator animator;
    private int Score = 0;

    Rigidbody2D rb;
    private float targetAngle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(Explosion, transform.position, UnityEngine.Quaternion.identity);
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
            (currentRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime / steer);

        rb.velocity = transform.up * speed * Time.fixedDeltaTime * 10f;
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
