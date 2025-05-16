using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private Camera Camera;

    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private GameObject enemyIndicatorPrefab;

    private GameObject icon;
    private GameObject Player;
    private GameObject nearestStar;
    private List<GameObject> stars = new List<GameObject>();

    [SerializeField] private float ScreenBorder;
    [SerializeField] private float VisibleDistance;

    private void Start()
    {
        Camera = Camera.main;
        Player = transform.parent.gameObject;
    }

    private void LateUpdate()
    {
        float distance = GetDistanceToNearestStar(Player.transform, stars);

        if (Player != null && nearestStar != null && nearestStar.activeInHierarchy)
        {
            Vector3 viewportPos = Camera.WorldToViewportPoint(nearestStar.transform.position);
            bool isStarVisible = viewportPos.x > 0.05f && viewportPos.x < 1.05f && viewportPos.y > 0.05f && viewportPos.y < 1.05f;

            if (!isStarVisible)
            {
                if (icon == null)
                {
                    icon = Instantiate(collectiblePrefab, GetIconPosition(), Quaternion.identity);
                }
                icon.SetActive(true);
                UpdateIconPosition(icon, viewportPos, Camera);
            }
            else if (icon != null)
            {
                icon.SetActive(false);
            }
        }
    }

    #region Handling Icon Positions on the side of the screen
    public void AddToStars(GameObject star)
    {
        stars.Add(star);
    }
    private Vector3 GetIconPosition()
    {
        Vector3 starPosition = Camera.WorldToViewportPoint(nearestStar.transform.position);
        Vector3 iconPosition = new Vector3();

        if (starPosition.x < 0f || starPosition.y < 0f)
        {
            if (starPosition.x < 0f)
            {
                iconPosition = new Vector3(0f, nearestStar.transform.position.y);
            }
            else
            {
                iconPosition = new Vector3(nearestStar.transform.position.x, 0f);
            }
        }

        if (starPosition.x > 1f || starPosition.y > 1f)
        {
            if (starPosition.x > 1f)
            {
                iconPosition = new Vector3(1f, nearestStar.transform.position.y);
            }
            else
            {
                iconPosition = new Vector3(nearestStar.transform.position.x, 1f);
            }
        }

        return iconPosition;
    }

    public float GetDistanceToNearestStar(Transform playerTransform, List<GameObject> stars)
    {
        float minDistance = 0f;

        foreach (GameObject star in stars)
        {
            Vector3 viewportPos = Camera.WorldToViewportPoint(star.transform.position);
            bool isVisible = viewportPos.z > 0 && viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1;

            if (star != null && star.activeInHierarchy)
            {
                float distance = Vector3.Distance(playerTransform.position, star.transform.position);
                if (distance < minDistance || minDistance == 0f)
                {
                    if (!isVisible)
                    {
                        minDistance = distance;
                        nearestStar = star;
                    }
                }
            }
        }

        return minDistance;
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

        Vector3 directionToCollectible = nearestStar.transform.position - icon.transform.position;

        directionToCollectible.z = 0; 
        directionToCollectible.Normalize();

        float angle = Mathf.Atan2(directionToCollectible.y, directionToCollectible.x) * Mathf.Rad2Deg;

        icon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
    public void Kill(GameObject star)
    {
        if (stars.Contains(star))
        {
            star.SetActive(false);
        }
        if (icon.activeInHierarchy)
        {
            icon.SetActive(false);
        }
    }
    #endregion
}
