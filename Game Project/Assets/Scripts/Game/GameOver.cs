using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public TMP_Text PointText;
    public GameObject UI;
    public GameObject pauseIcon;

    
    public void Setup(int score)
    {
        UI.SetActive(false);
        pauseIcon.SetActive(false);

        gameObject.SetActive(true);
        PointText.text = score.ToString() + " Stars";      
        
    }

   
}
