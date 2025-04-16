using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using Unity.Mathematics;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;
using UnityEngine.Experimental.XR.Interaction;
using Unity.VisualScripting;


public class Player : MonoBehaviour
{
    [SerializeField] private GameObject Explosion;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private FixedJoystick Joystick;
    [SerializeField] private GameObject flares;
    [SerializeField] private Transform flareLocation;
    [SerializeField] private GameObject shieldVisual;
    [SerializeField] private GameObject blackHolePreFab;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject SpeedBuffAnimation;
    [SerializeField] private GameObject Animation;


    [SerializeField] private float Steer = 1.5f;
    [SerializeField] private float Speed = 35f;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float boostSpeed = 50f;
    [SerializeField] private float notBoostSpeed = 35f;
    [SerializeField] private float RotationSpeed = 3f;

    AudioManager audioManager;

    public Button flareButton;
    public Button kamikazeButton;
    public Button blackHoleButton;
    public Button explodeButton;

    [SerializeField] private SpriteRenderer kamikazeSprite;
    public SpriteRenderer flareSprite;
    public SpriteRenderer explodeSprite;
    public SpriteRenderer blackholeSprite;

    public Sprite zeroBlackHoleUIBuffSprite;
    public Sprite oneBlackHoleUIBuffSprite;
    public Sprite zeroUIBuffSprite;
    public Sprite oneUIBuffSprite;
    public Sprite twoUIBuffSprite;
    public Sprite threeUIBuffSprite;

    public int blackHoleCount = 0;
    public int flareCount = 0;
    public int kamikazeCount = 0;
    public int explodeCount = 0;


    private bool isBoost;
    private EnemySpawner Spawner;

    private int Score = 0;

    Rigidbody2D rb;
    private float targetAngle;
    public bool Shield;

    private float speedUpDuration = 0f;
    private bool isSpeedUp = false;

    private float inputX;
    private float inputY;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
        rb = GetComponent<Rigidbody2D>();
        Spawner = FindAnyObjectByType<EnemySpawner>();

        adjustBuffUiText(blackholeSprite, blackHoleCount, true);
        adjustBuffUiText(flareSprite, flareCount, false);
        adjustBuffUiText(kamikazeSprite, kamikazeCount, false);
        adjustBuffUiText(explodeSprite, explodeCount, false);

        MakeButtonTransparent(kamikazeButton);
        MakeButtonTransparent(explodeButton);
        MakeButtonTransparent(blackHoleButton);
        MakeButtonTransparent(flareButton);


        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
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

                    audioManager.SFXPlay(audioManager.speedUp);
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
                }
            }
        }
    }
    //Player movement
    private void FixedUpdate()
    {
        //float x = Joystick.Horizontal;
        //float y = Joystick.Vertical;

        //if (Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f)
        //{
        //    targetAngle = Mathf.Atan2(y, x) * Mathf.Rad2Deg - 90f;
        //}

        //UnityEngine.Quaternion currentRotation = transform.rotation;

        //UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.
        //    Euler(0, 0, targetAngle);

        //transform.rotation = UnityEngine.Quaternion.Lerp
        //    (currentRotation, targetRotation, RotationSpeed * Time.fixedDeltaTime / Steer);
        //rb.velocity = transform.up * Speed * Time.fixedDeltaTime * 10f;

        UnityEngine.Quaternion currentRotation = transform.rotation;
        UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = UnityEngine.Quaternion.Lerp(currentRotation, targetRotation, RotationSpeed * Time.deltaTime / Steer);

        rb.MovePosition(rb.position + (UnityEngine.Vector2)(transform.up * Speed * Time.deltaTime));
    }

    //Handling collision for Shield
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (Shield != true)
            {
                AudioListener listener = Camera.main.GetComponent<AudioListener>();
                listener.enabled = true;
                audioManager.SFXPlay(audioManager.explosion);
                Instantiate(Explosion, transform.position, UnityEngine.Quaternion.identity);
            }
        }
    }



    //Handling collisions for Stars, Shield, Slow Time, Kamikaze
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Star"))
        {
            audioManager.SFXPlay(audioManager.collect);
            CollectibleController controller =
                collision.gameObject.GetComponent<CollectibleController>();

            controller.Kill();
            Score++;
            ScoreText.text = Score.ToString();
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {
            
            Object.Destroy(collision.gameObject);
            Shield = true;
            
            audioManager.SFXPlay(audioManager.shield);
            
            ChangeShieldState();
        }
        else if (collision.gameObject.CompareTag("SlowTime"))
        {
            Object.Destroy(collision.gameObject);
            StartCoroutine(Spawner.SlowDown());
        }
        else if (collision.gameObject.CompareTag("SpeedBuff"))
        {
            Object.Destroy(collision.gameObject); // destroying the buff

            if (Speed == notBoostSpeed)
            {
                normalSpeed = Speed; // storing the normal speed for the backup

                isBoost = true;
            }

            speedUpDuration += 4f;
        }
        else if (collision.gameObject.CompareTag("Kamikaze"))
        {
            Object.Destroy(collision.gameObject);

            if (kamikazeCount < 3)
            {
                kamikazeCount++;
                adjustBuffUiText(kamikazeSprite, kamikazeCount, false);
                MakeButtonNonTransparent(kamikazeButton);
            }
        }
        else if (collision.gameObject.CompareTag("Flare"))
        {
            Object.Destroy(collision.gameObject);

            if (flareCount < 3)
            {
                flareCount++;
                adjustBuffUiText(flareSprite, flareCount, false);
                MakeButtonNonTransparent(flareButton);
            }
        }
        else if (collision.gameObject.CompareTag("BlackHole"))
        {
            Object.Destroy(collision.gameObject);

            if (blackHoleCount < 1)
            {
                blackHoleCount++;
                adjustBuffUiText(blackholeSprite, blackHoleCount, true);
                MakeButtonNonTransparent(blackHoleButton);
            }
        }
        else if (collision.gameObject.CompareTag("Explode"))
        {
            Object.Destroy(collision.gameObject);

            if (explodeCount < 3)
            {
                explodeCount++;
                adjustBuffUiText(explodeSprite, explodeCount, false);
                MakeButtonNonTransparent(explodeButton);
            }

            //Spawner.ExplodeEnemies();
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
        return Score;
    }



    //In UI when flare button pressed trigger this function
    public void CreateFlares()
    {
        if (flareCount > 0)
        {
            audioManager.SFXPlay(audioManager.flare);

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


    public void MakeButtonTransparent(Button myButton)
    {
        ColorBlock cb = myButton.colors;
        cb.disabledColor = new Color(cb.disabledColor.r, cb.disabledColor.g, cb.disabledColor.b, 0.3f); // Adjust transparency (0 = invisible, 1 = fully visible)
        myButton.colors = cb;
        myButton.interactable = false;
    }

    public void MakeButtonNonTransparent(Button myButton)
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
