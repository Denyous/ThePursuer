using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public GameObject blackHolePrefab; // Karadelik prefabı

    public GameObject tapMessageUI;

    private GamePanelSetting pauseIcon;

    public GameObject UI;

    [SerializeField] GameObject player;

    private Player playerScript;

    private bool isPlacing = false;

    public AudioManager audioManager;

    public AudioSource musicSource;

    public SpriteRenderer blackholeSprite;


    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        tapMessageUI.SetActive(false);
        pauseIcon = GetComponent<GamePanelSetting>();
        playerScript = player.GetComponent<Player>();
    }

    void Update()
    {
        if (isPlacing && Input.GetMouseButtonDown(0)) // Sol tık ile yer seç
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0; // 2D olduğu için Z'yi sıfırla

            GameObject blackHoleRef = Instantiate(blackHolePrefab, spawnPosition, Quaternion.identity); // Karadelik oluştur

            audioManager.SFXPlayOneShot(audioManager.blackHoleInstantiationSFX); // blackhole yerleştirme sfx

            isPlacing = false;
            musicSource.UnPause();
            Time.timeScale = 1f; // Oyun devam etsin
            tapMessageUI.SetActive(false);
            //pauseIcon.pauseIcon.SetActive(true);
            UI.SetActive(true);
        }
    }

    public void StartPlacement()
    {
        if (playerScript.getBlackHoleCount() > 0)
        {
            isPlacing = true;
            Time.timeScale = 0f; // Oyun duraklatılır
            
            musicSource.Pause();
            
            tapMessageUI.SetActive(true);
            //pauseIcon.pauseIcon.SetActive(false);
            UI.SetActive(false);

            playerScript.decreaseBlackHoleCount();
            playerScript.adjustBuffUiText(blackholeSprite, playerScript.blackHoleCount, true);
        }
    }
}
