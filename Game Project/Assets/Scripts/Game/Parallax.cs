using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    public Transform player;  // Reference to the player
    public Transform middle;  // The center background tile
    public Transform top, bottom, left, right, topleft, topright, bottomleft, bottomright; // The surrounding tiles

    private Vector2 bgSize;  // The size of each background tile

    void Start()
    {
        // Calculate background tile size based on the middle tile's renderer
        SpriteRenderer sr = middle.GetComponent<SpriteRenderer>();
        bgSize = sr.bounds.size;

        RepositionTiles();
    }

    void FixedUpdate()
    {
        // Find the player's position relative to the middle tile
        Vector3 playerPos = player.position;
        Vector3 middlePos = middle.position;

        // Check if the player moves outside the middle tile
        float offsetX = playerPos.x - middlePos.x;
        float offsetY = playerPos.y - middlePos.y;

        if (Mathf.Abs(offsetX) > bgSize.x / 6 || Mathf.Abs(offsetY) > bgSize.y / 6)
        {
            RepositionTiles();
        }
    }

    void RepositionTiles()
    {
        // Determine which tile the player is currently in
        Transform newMiddle = GetClosestTile();

        // Move the middle tile to the new position
        Vector3 newMiddlePos = newMiddle.position;
        middle.position = newMiddlePos;

        // Reposition all surrounding tiles
        top.position = newMiddlePos + new Vector3(0, bgSize.y, 0);
        bottom.position = newMiddlePos + new Vector3(0, -bgSize.y, 0);
        left.position = newMiddlePos + new Vector3(-bgSize.x, 0, 0);
        right.position = newMiddlePos + new Vector3(bgSize.x, 0, 0);

        topleft.position = newMiddlePos + new Vector3(-bgSize.x, bgSize.y, 0);
        topright.position = newMiddlePos + new Vector3(bgSize.x, bgSize.y, 0);

        bottomleft.position = newMiddlePos + new Vector3(-bgSize.x, -bgSize.y, 0);
        bottomright.position = newMiddlePos + new Vector3(bgSize.x, -bgSize.y, 0);
    }

    Transform GetClosestTile()
    {
        // Finds the closest tile to the player (which will become the new middle tile)
        Transform closest = middle;
        float minDistance = Vector3.Distance(player.position, middle.position);
        minDistance += 100;

        Transform[] tiles = { top, bottom, left, right, bottomleft, bottomright, topleft, topright };

        foreach (Transform tile in tiles)
        {
            float distance = Vector3.Distance(player.position, tile.position);
            if (distance < minDistance)
            {
                closest = tile;
                minDistance = distance;
            }
        }

        return closest;
    }

}
