using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    private float spriteWidth;
    private float spriteHeight;


    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        spriteWidth = spriteRenderer.sprite.texture.width / spriteRenderer.sprite.pixelsPerUnit;
        spriteHeight = spriteRenderer.sprite.texture.height / spriteRenderer.sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x,
                                          deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;

        float distanceX = Mathf.Abs(cameraTransform.position.x - transform.position.x);
        float distanceY = Mathf.Abs(cameraTransform.position.y - transform.position.y);

        if (distanceX >= spriteWidth)
        {
            float offsetPositionX = distanceX % spriteWidth;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }

        if (distanceY >= spriteHeight)
        {
            float offsetPositionY = distanceY % spriteHeight;
            transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPositionY);
        }
    }

}
