using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;
using UnityEngine.Experimental.XR.Interaction;
using Unity.VisualScripting;
using UnityEngine.UIElements;


public class Player : MonoBehaviour
{
    [SerializeField] private GameObject Explosion;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private FixedJoystick Joystick;
    [SerializeField] private GameObject flares;
    [SerializeField] private Transform flareLocation;
    [SerializeField] private GameObject shieldVisual;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject SpeedBuffAnimation;
    [SerializeField] private GameObject Animation;
    [SerializeField] private GameObject controllerGameObject;
    [SerializeField] private GameObject spawnerGameObject;
    [SerializeField] private SpriteRenderer kamikazeSprite;
    [SerializeField] private UnityEngine.UI.Button flareButton;
    [SerializeField] private UnityEngine.UI.Button kamikazeButton;
    [SerializeField] private UnityEngine.UI.Button blackHoleButton;
    [SerializeField] private UnityEngine.UI.Button explodeButton;
    [SerializeField] private SpriteRenderer flareSprite;
    [SerializeField] private SpriteRenderer explodeSprite;
    [SerializeField] private SpriteRenderer blackholeSprite;
    [SerializeField] private Sprite zeroBlackHoleUIBuffSprite;
    [SerializeField] private Sprite oneBlackHoleUIBuffSprite;
    [SerializeField] private Sprite zeroUIBuffSprite;
    [SerializeField] private Sprite oneUIBuffSprite;
    [SerializeField] private Sprite twoUIBuffSprite;
    [SerializeField] private Sprite threeUIBuffSprite;
    [SerializeField] private float Steer = 1.5f;
    [SerializeField] private float Speed = 35f;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float boostSpeed = 50f;
    [SerializeField] private float notBoostSpeed = 35f;
    [SerializeField] private float RotationSpeed = 3f;
    private AudioSource playerShipSFX;
    public int blackHoleCount = 0;
    public int flareCount = 0;
    public int kamikazeCount = 0;
    public int explodeCount = 0;
    private float originalVolume;
    private float originalPitch;

    private AudioManager audioManager;
    private EnemySpawner Spawner;
    private CollectibleController controller;
    private int Score = 0;
    private float targetAngle;
    public bool Shield;
    private float speedUpDuration = 0f;
    private bool isSpeedUp = false;
    private float inputX;
    private float inputY;
    private bool isBoost;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        Spawner = spawnerGameObject.GetComponent<EnemySpawner>();
        controller = controllerGameObject.GetComponent<CollectibleController>();
        playerShipSFX = GetComponent<AudioSource>();
        adjustBuffUiText(blackholeSprite, blackHoleCount, true);
        adjustBuffUiText(flareSprite, flareCount, false);
        adjustBuffUiText(kamikazeSprite, kamikazeCount, false);
        adjustBuffUiText(explodeSprite, explodeCount, false);
        originalVolume = playerShipSFX.volume;
        originalPitch = playerShipSFX.pitch;

        MakeButtonTransparent(kamikazeButton);
        MakeButtonTransparent(explodeButton);
        MakeButtonTransparent(blackHoleButton);
        MakeButtonTransparent(flareButton);
    }

    private void Update()
    {
        float newX = Joystick.Horizontal;
        float newY = Joystick.Vertical;

        if (Mathf.Abs(newX) > 0.05f || Mathf.Abs(newY) > 0.05f)
        {
            inputX = newX;
            inputY = newY;
        }

        if (Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputY) > 0.1f)
        {
            targetAngle = Mathf.Atan2(inputY, inputX) * Mathf.Rad2Deg - 90f;
        }

        if (isBoost)
        {
            if (speedUpDuration > 0)
            {
                speedUpDuration -= Time.deltaTime;

                if (!isSpeedUp)
                {
                    isSpeedUp = true;

                    audioManager.SFXPlayOneShot(audioManager.speedUpSFX);
                    Animation.gameObject.SetActive(false);
                    SpeedBuffAnimation.gameObject.SetActive(true);

                    Speed = boostSpeed;
                }

                if (speedUpDuration < 0)
                {
                    isSpeedUp = false;
                    isBoost = false;

                    Speed = normalSpeed;
                    Animation.SetActive(true);
                    SpeedBuffAnimation.SetActive(false);
                    audioManager.SFXPlayOneShot(audioManager.speedDownSFX);
                    playerShipSFX.pitch = originalPitch;
                    playerShipSFX.volume = originalVolume;
                }
            }
        }

        UnityEngine.Quaternion currentRotation = transform.rotation;
        UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = UnityEngine.Quaternion.Lerp(currentRotation, targetRotation, RotationSpeed * Time.deltaTime / Steer);

        transform.position += Speed * Time.deltaTime * transform.up;
    }

    public void CreateExplosion()
    {
        audioManager.SFXPlayOneShot(audioManager.explosionSFX);
        Instantiate(Explosion, new Vector3(transform.position.x, transform.position.y, 3), UnityEngine.Quaternion.identity);
    }

    //Handling collisions for Stars, Shield, Slow Time, Kamikaze
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Star"))
        {
            audioManager.SFXPlayOneShot(audioManager.collectStarSFX);
            controller.Kill(collision.gameObject);
            Score++;
            ScoreText.text = Score.ToString();
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {
            
            collision.gameObject.SetActive(false);
            Shield = true;
            
            audioManager.SFXPlayOneShot(audioManager.shieldUpSFX);
            
            ChangeShieldState();
        }
        else if (collision.gameObject.CompareTag("SlowTime"))
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(Spawner.SlowDown());
        }
        else if (collision.gameObject.CompareTag("SpeedBuff"))
        {
            collision.gameObject.SetActive(false);

            if (Speed == notBoostSpeed)
            {
                normalSpeed = Speed; // storing the normal speed for the backup

                isBoost = true;
            }
            if (playerShipSFX.volume == originalVolume && playerShipSFX.pitch == originalPitch)
            {
                playerShipSFX.volume = 1f;
                playerShipSFX.pitch = originalPitch * 1.2f;
            }
            speedUpDuration += 4f;
        }
        else if (collision.gameObject.CompareTag("Kamikaze"))
        {
            collision.gameObject.SetActive(false);

            if (kamikazeCount < 3)
            {
                kamikazeCount++;
                adjustBuffUiText(kamikazeSprite, kamikazeCount, false);
                MakeButtonNonTransparent(kamikazeButton);
                audioManager.SFXPlayOneShot(audioManager.collectBuffSFX);
            }
        }
        else if (collision.gameObject.CompareTag("Flare"))
        {
            collision.gameObject.SetActive(false);

            if (flareCount < 3)
            {
                flareCount++;
                adjustBuffUiText(flareSprite, flareCount, false);
                MakeButtonNonTransparent(flareButton);
                audioManager.SFXPlayOneShot(audioManager.collectBuffSFX);
            }
        }
        else if (collision.gameObject.CompareTag("BlackHole"))
        {
            collision.gameObject.SetActive(false);

            if (blackHoleCount < 1)
            {
                blackHoleCount++;
                adjustBuffUiText(blackholeSprite, blackHoleCount, true);
                MakeButtonNonTransparent(blackHoleButton);
                audioManager.SFXPlayOneShot(audioManager.collectBuffSFX);
            }
        }
        else if (collision.gameObject.CompareTag("Explode"))
        {
            collision.gameObject.SetActive(false);

            if (explodeCount < 3)
            {
                explodeCount++;
                adjustBuffUiText(explodeSprite, explodeCount, false);
                MakeButtonNonTransparent(explodeButton);
                audioManager.SFXPlayOneShot(audioManager.collectBuffSFX);
            }
        }
    }
 
    //Change Shield Visual according to the Shield bool
    public void ChangeShieldState()
    {
        if (Shield == false)
        {
            
            shieldVisual.SetActive(false);  // Kalkan görselini kapat
        }
        else
        {
            shieldVisual.SetActive(true);  // Kalkan görselini aç
        }
    }

    //Getting score to show in game over screen
    public int GetScore()
    {
        return (int)(Score * GameStats.GetElapsedTime());
    }

    //In UI when flare button pressed trigger this function
    public void CreateFlares()
    {
        if (flareCount > 0)
        {
            audioManager.SFXPlayOneShot(audioManager.flareSFX);

            UnityEngine.Quaternion currentRotation = transform.rotation;

            Instantiate(flares, flareLocation.position, currentRotation);

            decreaseFlareCount();

            adjustBuffUiText(flareSprite, flareCount, false);
        }
    }

    public int getBlackHoleCount()
    {
        return blackHoleCount;
    }

    public int getKamikazeCount()
    {
        return kamikazeCount;
    }

    public int getFlareCount()
    {
        return flareCount;
    }

    public void decreaseKamikazeCount()
    {
        if (kamikazeCount > 0)
        {
            kamikazeCount--;
        }

        if (kamikazeCount == 0)
        {
            MakeButtonTransparent(kamikazeButton);
        }
    }

    public void decreaseBlackHoleCount()
    {
        if (blackHoleCount > 0)
        {
            blackHoleCount--;
        }

        if (blackHoleCount == 0)
        {
            MakeButtonTransparent(blackHoleButton);
        }
    }

    public void decreaseFlareCount()
    {
        if (flareCount > 0)
        {
            flareCount--;
        }

        if (flareCount == 0)
        {
            MakeButtonTransparent(flareButton);
        }
    }

    public void decreaseExplodeCount()
    {
        if (explodeCount > 0)
        {
            explodeCount--;
        }

        if (explodeCount == 0)
        {
            MakeButtonTransparent(explodeButton);
        }
    }

    public void MakeButtonTransparent(UnityEngine.UI.Button myButton)
    {
        ColorBlock cb = myButton.colors;
        cb.disabledColor = new Color(cb.disabledColor.r, cb.disabledColor.g, cb.disabledColor.b, 0.3f); // Adjust transparency (0 = invisible, 1 = fully visible)
        myButton.colors = cb;
        myButton.interactable = false;
    }

    public void MakeButtonNonTransparent(UnityEngine.UI.Button myButton)
    {
        ColorBlock cb = myButton.colors;

        cb.disabledColor = new Color(cb.disabledColor.r, cb.disabledColor.g, cb.disabledColor.b, 1f); 
        myButton.colors = cb;
        myButton.interactable = true;
    }

    public void adjustBuffUiText(SpriteRenderer UI, int value, bool isBlackHole)
    {
        switch (value)
        {
            case 0:
                if (isBlackHole)
                {
                    UI.sprite = zeroBlackHoleUIBuffSprite;
                }
                else
                {
                    UI.sprite = zeroUIBuffSprite;
                }
                break;
            case 1:
                if (isBlackHole)
                {
                    UI.sprite = oneBlackHoleUIBuffSprite;
                }
                else
                {
                    UI.sprite = oneUIBuffSprite;
                }
                break;
            case 2:
                if (!isBlackHole)
                {
                    UI.sprite = twoUIBuffSprite;
                }
                break;
            case 3:
                if (!isBlackHole)
                {
                    UI.sprite = threeUIBuffSprite;
                }
                break;
            default:
                break;
        }
    }
}
