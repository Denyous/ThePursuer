using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebulaParallax : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector2 movementRatio = new Vector2(0.5f, 0.5f); // How much the nebula moves relative to the player
    private Vector3 initialOffset;
    public RectTransform canvasRect; // Reference to the canvas's RectTransform

    void Start()
    {
        initialOffset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = player.position + initialOffset;
        newPosition.x *= movementRatio.x;
        newPosition.y *= movementRatio.y;

        Vector3[] canvasCorners = new Vector3[4];
        canvasRect.GetWorldCorners(canvasCorners);

        float minX = canvasCorners[0].x; // Bottom-left corner X
        float maxX = canvasCorners[2].x; // Top-right corner X
        float minY = canvasCorners[0].y; // Bottom-left corner Y
        float maxY = canvasCorners[2].y; // Top-right corner Y

        // Clamp the position to stay within the canvas bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;
    }
}
