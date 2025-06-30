using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static int deathCount = 0;
    [SerializeField] GameObject interstitalAds;
    


    private void Update()
    {
        if (deathCount == 3)
        {
            deathCount = 0;
            interstitalAds.SetActive(true);
        }
    }

    public static void AddDeath()
    {
        deathCount++;
      
        Debug.Log("Ölüm Sayısı: " + deathCount);
    }

  

    public void SkipButton()
    {
        interstitalAds.SetActive(false);
    }
}
