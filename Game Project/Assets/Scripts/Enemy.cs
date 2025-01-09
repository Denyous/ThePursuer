using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;
    [SerializeField] private GameObject EnemyPreFab;
    private GameObject gameObject;
    private bool isTriggered = false;

    private void Start()
    {
        gameObject = Instantiate(EnemyPreFab, transform.position, Quaternion.Euler(0, 0, 0));
        gameObject.transform.SetParent(transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isTriggered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isTriggered = false;
        }
    }
    private void Update()
    {
        if (!isTriggered)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}
