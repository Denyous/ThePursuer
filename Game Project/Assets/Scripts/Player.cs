using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float steer = 30f;
    [SerializeField] private GameObject Explosion;

    private Animator animator;

    Rigidbody2D rb;
    float input;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        input = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
        }
    }


    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed * Time.fixedDeltaTime * 10f;
        rb.angularVelocity = -input * steer * 10f;
    }

}
