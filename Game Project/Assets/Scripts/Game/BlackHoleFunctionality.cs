using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleFunctionality : MonoBehaviour
{
    [SerializeField] private float pullStrength = 5f;
    [SerializeField] private float orbitStrength = 3f;     
    [SerializeField] private float duration = 10f;
    [SerializeField] private float destroyRadius = 0.5f;

    private List<Rigidbody2D> enemiesInRange = new List<Rigidbody2D>();
    private List<Rigidbody2D> enemiesToRemove = new List<Rigidbody2D>();

    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody2D enemyRb in enemiesInRange)
        {
            if (enemyRb == null) continue;

            Vector2 directionToCenter = (Vector2)transform.position - enemyRb.position;
            float distance = directionToCenter.magnitude;

            if (distance < destroyRadius)
            {
                Enemy enemyScript = enemyRb.gameObject.GetComponent<Enemy>();
                enemyScript.Die(false, false, false, true, false);
                enemiesToRemove.Add(enemyRb);
                continue;
            }

            Vector2 normalizedDirection = directionToCenter.normalized;
            
            Vector2 tangent = new Vector2(-normalizedDirection.y, normalizedDirection.x);

            Vector2 force = normalizedDirection * pullStrength + tangent * orbitStrength;

            enemyRb.velocity = force;
        }

        foreach (var enemy in enemiesToRemove)
        {
            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null && !enemiesInRange.Contains(enemyRb))
            {
                enemiesInRange.Add(enemyRb);
                enemyRb.GetComponent<Enemy>().enabled = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null && !enemiesInRange.Contains(enemyRb))
            {
                enemiesInRange.Add(enemyRb);
                enemyRb.GetComponent<Enemy>().enabled = false;
            }
        }
    }


    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(duration);
        foreach (var enemyRb in enemiesInRange)
        {
            if (enemyRb == null) continue;

            Enemy enemyScript = enemyRb.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.enabled = true;
                enemyScript.Die(false, false, false, true, false);
            }
        }

        Object.Destroy(gameObject);
    }
}
