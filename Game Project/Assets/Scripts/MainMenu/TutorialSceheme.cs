using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSceheme : MonoBehaviour
{
    public Sprite[] tutorialImages;            // 8 tane tutorial sprite'ı
    public Image tutorialImageDisplay;         // Görüntülenecek UI Image
    public Button nextButton;
    public Button previousButton;
    public TextMeshProUGUI pageText;                      // (opsiyonel) "1 / 8" tarzı yazı

    private int currentIndex = 0;

    void Start()
    {
        ShowImage(0);

        nextButton.onClick.AddListener(NextImage);
        previousButton.onClick.AddListener(PreviousImage);
    }

    public void ShowImage(int index)
    {
        tutorialImageDisplay.sprite = tutorialImages[index];

        // Butonları aktif/pasif hale getir
       
        previousButton.interactable = index > 0;
        nextButton.interactable = index < tutorialImages.Length - 1;

        // Sayfa sayısı güncelle
        if (pageText != null)
        {
            pageText.text = $"{index + 1} / {tutorialImages.Length}";
        }
    }

    void NextImage()
    {
        if (currentIndex < tutorialImages.Length - 1)
        {
            currentIndex++;
            ShowImage(currentIndex);
        }
    }

    void PreviousImage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowImage(currentIndex);
        }
    }
}
