using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField] private GameObject IconPrefab;
    private Camera Camera;

    private GameObject icon;
    private GameObject Player;
    private bool isVisible = false;

    [SerializeField] private float ScreenBorder;
    [SerializeField] private float VisibleDistance;

    private void Awake()
    {
        Camera = Camera.main;
    }


    private void Update()
    {
        if (Player != null)
        {
            float distance = GetDistanceToPlayer(Player.transform);

            Vector3 viewportPosition = Camera.WorldToViewportPoint(transform.position);

            if (distance < VisibleDistance && icon == null)
            {
                if (!isVisible)
                {
                    icon = Instantiate(IconPrefab, GetIconPosition(), Quaternion.identity);
                }
            }
            else if(distance > VisibleDistance && icon != null)
            {
               Object.Destroy(icon);
            }

            if (icon != null)
            {
                UpdateIconPosition(icon, viewportPosition, Camera);
            }
        }
    }

    public void SetPlayer(GameObject player)
    {
        Player = player;
    }
    private Vector3 GetIconPosition()
    {
        Vector3 starPosition = Camera.WorldToViewportPoint(transform.position);

        Vector3 iconPosition = new Vector3();

        if (starPosition.x < 0f || starPosition.y < 0f)
        {
            if (starPosition.x < 0f)
            {
                iconPosition = new Vector3(0f, transform.position.y);
            }
            else
            {
                iconPosition = new Vector3(transform.position.x, 0f);
            }
        }

        if (starPosition.x > 1f || starPosition.y > 1f)
        {
            if (starPosition.x > 1f)
            {
                iconPosition = new Vector3(1f, transform.position.y);
            }
            else
            {
                iconPosition = new Vector3(transform.position.x, 1f);
            }
        }

        return iconPosition;
    }
    public float GetDistanceToPlayer(Transform playerTransform)
    {
        return Vector3.Distance(playerTransform.position, transform.position);
    }
    void UpdateIconPosition(GameObject icon, Vector3 viewportPosition, Camera camera)
    {
        Vector3 screenPosition = new Vector3();

        if (viewportPosition.x < 0) // Left side
        {
            screenPosition.x = ScreenBorder;
        }
        else if (viewportPosition.x > 1) // Right side
        {
            screenPosition.x = Screen.width - ScreenBorder;
        }
        else
        {
            screenPosition.x = viewportPosition.x * Screen.width;
        }

        if (viewportPosition.y < 0) // Bottom side
        {
            screenPosition.y = ScreenBorder;
        }
        else if (viewportPosition.y > 1) // Top side
        {
            screenPosition.y = Screen.height - ScreenBorder;
        }
        else
        {
            screenPosition.y = viewportPosition.y * Screen.height;
        }

        Vector3 iconPosition = 
            camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 1));
        icon.transform.position = iconPosition;
    }
    private void OnBecameVisible()
    {
        if (icon != null)
        {
            Object.Destroy(icon);
        }
        isVisible = true;
    }
    private void OnBecameInvisible()
    {
        isVisible = false;
    }
    public void Kill()
    {
        Object.Destroy(gameObject);
        if (icon != null)
        {
            Object.Destroy(icon);
        }
    }
}
