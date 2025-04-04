using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleFunctionality : MonoBehaviour
{
    public float pullStrength = 5f;       
    public float orbitStrength = 3f;     
    public float duration = 3f;
    public float destroyRadius = 0.5f;

    private List<Rigidbody2D> enemiesInRange = new List<Rigidbody2D>();

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
                Destroy(enemyRb.gameObject);
                Enemy enemyScript = enemyRb.gameObject.GetComponent<Enemy>();
                enemyScript.Die(false, false, false);
                continue;
            }

            Vector2 normalizedDirection = directionToCenter.normalized;
            
            Vector2 tangent = new Vector2(-normalizedDirection.y, normalizedDirection.x);

            Vector2 force = normalizedDirection * pullStrength + tangent * orbitStrength;

            enemyRb.velocity = force;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null && !enemiesInRange.Contains(enemyRb))
            {
                enemiesInRange.Add(enemyRb);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemiesInRange.Remove(enemyRb);
            }
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
