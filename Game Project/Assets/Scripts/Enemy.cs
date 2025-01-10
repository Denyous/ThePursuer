using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 60f;
    [SerializeField] private float steer = 15f;
    private Transform target;

    Rigidbody2D rb;
    float input;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed * Time.fixedDeltaTime * 10f;
        Vector2 direction = (target.position - transform.position).normalized;
        float rotationSteer = Vector3.Cross(transform.up, direction).z;
        rb.angularVelocity = rotationSteer * steer * 10f;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
