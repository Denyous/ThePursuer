using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 60f;
    [SerializeField] private float steer = 15f;

    public static event System.Action<GameObject> OnEnemyDied;

    private Transform target;
    private Animator animator;

    Rigidbody2D rb;
    float input;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Die()
    {
        OnEnemyDied?.Invoke(this.gameObject);
    }

    private void Update()
    {
        input = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Object.Destroy(target.gameObject);
            Object.Destroy(transform.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            rb.velocity = transform.up * speed * Time.fixedDeltaTime * 10f;
            Vector2 direction = (target.position - transform.position).normalized;
            float rotationSteer = Vector3.Cross(transform.up, direction).z;
            rb.angularVelocity = rotationSteer * steer * 10f;
        }
    }

    
}
