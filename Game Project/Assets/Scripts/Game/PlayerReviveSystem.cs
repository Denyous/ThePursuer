using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReviveSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject gameOverScreen;
    public GameObject reviveButton;
    public GameObject UI;
    

    public bool reviveUsed = false;

    private Vector3 deathPosition; // ✅ Eklendi

    private void Update()
    {
        if (player != null)
        {
            PlayerDiedLoaction();
        }
    }
    public void OnReviveButtonPressed()
    {
        UıSettings();
        RevivePlayer();
    }

   

    public void RevivePlayer()
    {
        player.SetActive(true);
        player.transform.position = deathPosition; // ✅ Aynı noktada doğ
        UI.SetActive(true);
    }

    void UıSettings()
    {
        reviveUsed = true;

        GameStats.deathCount = 0;

        gameOverScreen.SetActive(false);

        if (!reviveUsed)
        {
            reviveButton.SetActive(true);
        }
        else
        {
            reviveButton.SetActive(false);
        }
    }

    public void PlayerDiedLoaction()
    {
        deathPosition = player.transform.position; // ✅ Ölüm konumunu kaydet
    }
}


