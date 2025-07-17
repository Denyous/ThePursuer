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
    private GameObject closestEnemy;
    private GameObject enemyIcon;
    private List<GameObject> stars = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private float ScreenBorder;
    [SerializeField] private float VisibleDistance;

    private void Start()
    {
        Camera = Camera.main;
        Player = transform.parent.gameObject;
        enemyIcon = Instantiate(enemyIndicatorPrefab, GetIconPosition(closestEnemy), Quaternion.identity);
        enemyIcon.SetActive(false);
        icon = Instantiate(collectiblePrefab, GetIconPosition(nearestStar), Quaternion.identity);
        icon.SetActive(false);
    }

    private void LateUpdate()
    {
        nearestStar = GetClosestGameObject(Player.transform, stars);

        if (Player != null && nearestStar != null && nearestStar.activeInHierarchy)
        {
            Vector3 viewportPos = Camera.WorldToViewportPoint(nearestStar.transform.position);
            bool isStarVisible = viewportPos.x > 0.05f && viewportPos.x < 1.05f && viewportPos.y > 0.05f && viewportPos.y < 1.05f;

            if (!isStarVisible)
            {
                icon.SetActive(true);
                UpdateIconPosition(icon, nearestStar, viewportPos, Camera);
            }
            else if (icon != null)
            {
                icon.SetActive(false);
            }
        }

        closestEnemy = GetClosestGameObject(Player.transform, enemies);

        if (Player != null && closestEnemy != null && closestEnemy.activeInHierarchy)
        {
            Vector3 viewportPos = Camera.WorldToViewportPoint(closestEnemy.transform.position);
            bool isEnemyVisible = viewportPos.x > 0.05f && viewportPos.x < 1.05f && viewportPos.y > 0.05f && viewportPos.y < 1.05f;

            if (!isEnemyVisible)
            {
                enemyIcon.SetActive(true);
                UpdateIconPosition(enemyIcon, closestEnemy, viewportPos, Camera);
            }
            else if (enemyIcon != null)
            {
                enemyIcon.SetActive(false);
            }
        }
    }

    #region Handling Icon Positions on the side of the screen
    public void AddToStars(GameObject star)
    {
        stars.Add(star);
    }
    public void AddToEnemies(GameObject star)
    {
        enemies.Add(star);
    }
    private Vector3 GetIconPosition(GameObject obj)
    {
        if (obj == null)
        {
            return Vector3.zero;
        }

        Vector3 objPosition = Camera.WorldToViewportPoint(nearestStar.transform.position);
        Vector3 iconPosition = new Vector3();

        if (objPosition.x < 0f || objPosition.y < 0f)
        {
            if (objPosition.x < 0f)
            {
                iconPosition = new Vector3(0f, obj.transform.position.y);
            }
            else
            {
                iconPosition = new Vector3(obj.transform.position.x, 0f);
            }
        }

        if (objPosition.x > 1f || objPosition.y > 1f)
        {
            if (objPosition.x > 1f)
            {
                iconPosition = new Vector3(1f, obj.transform.position.y);
            }
            else
            {
                iconPosition = new Vector3(obj.transform.position.x, 1f);
            }
        }

        return iconPosition;
    }

    public GameObject GetClosestGameObject(Transform subject, List<GameObject> objectList)
    {
        float minDistance = 0f;
        GameObject gameObject = null;

        foreach (GameObject obj in objectList)
        {
            Vector3 viewportPos = Camera.WorldToViewportPoint(obj.transform.position);
            bool isVisible = viewportPos.z > 0 && viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1;

            if (obj != null && obj.activeInHierarchy)
            {
                float distance = Vector3.Distance(subject.position, obj.transform.position);
                if (distance < minDistance || minDistance == 0f)
                {
                    if (!isVisible)
                    {
                        minDistance = distance;
                        gameObject = obj;
                    }
                }
            }
        }
        if (gameObject != null)
        {
            return gameObject;
        }
        return null;
    }

    void UpdateIconPosition(GameObject icon, GameObject closestObject, Vector3 viewportPosition, Camera camera)
    {
        Vector3 screenPosition = new Vector3();

        if (viewportPosition.x < 0) 
        {
            screenPosition.x = ScreenBorder;
        }
        else if (viewportPosition.x > 1) 
        {
            screenPosition.x = Screen.width - ScreenBorder;
        }
        else
        {
            screenPosition.x = viewportPosition.x * Screen.width;
        }

        if (viewportPosition.y < 0) 
        {
            screenPosition.y = ScreenBorder;
        }
        else if (viewportPosition.y > 1)
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

        Vector3 directionToCollectible = closestObject.transform.position - icon.transform.position;

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
