using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleTimer : MonoBehaviour
{
    private SpriteRenderer Renderer;

    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("MakeInactiveIfNotVisible", 5f, 30f);
    }
    
    private void MakeInactiveIfNotVisible()
    {
        if (!Renderer.isVisible && Renderer != null)
        {
            gameObject.SetActive(false);
        }
    }
}
