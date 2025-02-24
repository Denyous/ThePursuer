using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public GameObject blackHolePrefab; // Karadelik prefabı
    
    private bool isPlacing = false;

    void Start()
    {
       
    }

    void Update()
    {
        if (isPlacing && Input.GetMouseButtonDown(0)) // Sol tık ile yer seç
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0; // 2D olduğu için Z'yi sıfırla

            Instantiate(blackHolePrefab, spawnPosition, Quaternion.identity); // Karadelik oluştur

            isPlacing = false;
            Time.timeScale = 1f; // Oyun devam etsin
        }
    }

   public void StartPlacement()
    {
        isPlacing = true;
        Time.timeScale = 0f; // Oyun duraklatılır
    }
}
