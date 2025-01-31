using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebulaParallax : MonoBehaviour
{
    public Transform player; // Reference to the player
    public Camera mainCamera; // Reference to the main camera
    public float followSpeed = 2f; // Speed of movement
    public float movementFactor = 0.3f;

    private float minX, maxX, minY, maxY; // Background bounds

    private float camHeight, camWidth;
    private float bgWidth, bgHeight;



    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        camHeight = 2f * mainCamera.orthographicSize;
        camWidth = camHeight * mainCamera.aspect;

        bgWidth = sr.bounds.size.x;
        bgHeight = sr.bounds.size.y;
    }

    void Update()
    {
        Vector3 targetPos = new Vector3(0,0,transform.position.z);

        float cameraLeftSide = mainCamera.transform.position.x - camWidth / 2;
        float cameraRightSide = mainCamera.transform.position.x + camWidth / 2;

        float cameraTopSide = mainCamera.transform.position.y + camHeight / 2;
        float cameraBotSide = mainCamera.transform.position.y - camHeight / 2;

        float bgLeftSide = transform.position.x - bgWidth / 2;
        float bgRightSide = transform.position.x + bgWidth / 2;

        float bgTopSide = transform.position.x + bgHeight / 2;
        float bgBotSide = transform.position.x - bgHeight / 2;

        float checkLeft = cameraLeftSide - bgLeftSide;
        float checkRight = bgRightSide - cameraRightSide;

        float checkTop = bgTopSide - cameraTopSide;
        float checkBot = cameraBotSide - bgBotSide;

        if (checkBot < 0.5)
        {
            minY = cameraBotSide;
            maxY = cameraBotSide;
            minX = bgLeftSide;
            maxX = bgRightSide;

            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

            // Smoothly move the background toward the target position
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
        if (checkTop < 0.5)
        {
            minY = cameraTopSide;
            maxY = cameraTopSide;
            minX = bgLeftSide;
            maxX = bgRightSide;

            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

            // Smoothly move the background toward the target position
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
        if (checkLeft < 0.5)
        {
            minX = cameraLeftSide;
            maxX = cameraLeftSide;
            minY = cameraBotSide;
            maxY = cameraTopSide;

            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

            // Smoothly move the background toward the target position
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
        if (checkRight < 0.5)
        {
            minX = cameraRightSide;
            maxX = cameraRightSide;
            minY = cameraBotSide;
            maxY = cameraTopSide;

            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

            // Smoothly move the background toward the target position
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
    }
}
