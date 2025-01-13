using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public TMP_Text PointText;

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        PointText.text = score.ToString() + " Stars";
    }
}
