using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public GameObject blackHolePrefab; // Karadelik prefabı

    public GameObject tapMessageUI;

    private bool isPlacing = false;

    void Start()
    {
        tapMessageUI.SetActive(false);
    }

    void Update()
    {
        if (isPlacing && Input.GetMouseButtonDown(0)) // Sol tık ile yer seç
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0; // 2D olduğu için Z'yi sıfırla

            GameObject blackHoleRef = Instantiate(blackHolePrefab, spawnPosition, Quaternion.identity); // Karadelik oluştur

            isPlacing = false;
            Time.timeScale = 1f; // Oyun devam etsin
            tapMessageUI.SetActive(false);
            StartCoroutine(destroyBlackHoleAfterXSeconds(blackHoleRef));
        }
    }

    private IEnumerator destroyBlackHoleAfterXSeconds(GameObject blackHole)
    {
        yield return new WaitForSeconds(1);
        Object.Destroy(blackHole);
    }

    public void StartPlacement()
    {
        isPlacing = true;
        Time.timeScale = 0f; // Oyun duraklatılır
        tapMessageUI.SetActive(true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Object.DestroyObject(collision.gameObject);
        }
    }
}
