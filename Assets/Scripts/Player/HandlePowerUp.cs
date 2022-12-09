using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandlePowerUp : MonoBehaviour
{
    Player myPlayer;
    GameManager myGameManager;
    [Header("Power Up Display")]
    public Image powerUpImage;
    public Sprite coinSprite;
    public Sprite magnetSprite;
    public Sprite shieldSprite;
    public Slider PowerUpTimeSlider;
    public ParticleSystem invulnerablittyFX;
    public ParticleSystem magnetFX;
    public ParticleSystem doubleCoinFX;
    private float magnetRadius = 20f;
    float countDownValue = 0f;
    bool isActiveBuff = false;

    bool magnetActive = false;
    bool doubleCoinActive = false;
    bool isPaused = false;
    bool isCountdown = false;
    // Start is called before the first frame update
    void Start()
    {
        PowerUpTimeSlider.gameObject.SetActive(false);
        powerUpImage.gameObject.SetActive(false);
        myPlayer = GetComponent<Player>();
        myGameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(magnetActive) UseMagnet();

        if(myPlayer.hasDied) {
            invulnerablittyFX.Stop();
            doubleCoinFX.Stop();
            magnetFX.Stop();
        }

        if(!isCountdown) isPaused = !myPlayer.isMovable;
    }

    private void UseMagnet()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, magnetRadius);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.GetComponent<Coin>())
            {
                collider.gameObject.GetComponent<Coin>().isMagneted = true;
            }
        }
    }

    public void DoubleCoin(float duration)
    {
        powerUpImage.sprite = coinSprite;
        StartCoroutine(DoubleCoinBuff(duration));
        StartCoroutine(CountDown(duration));
    }

    public void ImvulnerabilityShield(float duration)
    {
        powerUpImage.sprite = shieldSprite;
        StartCoroutine(ShieldBuff(duration));
        StartCoroutine(CountDown(duration));
    }

    public void MagnetBuff(float duration)
    {
        powerUpImage.sprite = magnetSprite;
        StartCoroutine(Magnet(duration));
        StartCoroutine(CountDown(duration));
    }

    IEnumerator Magnet(float duration)
    {
        magnetActive = true;
        if(!myPlayer.hasDied) magnetFX.Play();
        float buffTime = duration;

        while(buffTime > 0){
            buffTime -= Time.deltaTime;
            while(isPaused) {
                yield return null;
            }
            yield return null;
        }

        magnetActive = false;
        magnetFX.Stop();
    }

    IEnumerator ShieldBuff(float duration)
    {
        myPlayer.isInvulnerable = true;
        if(!myPlayer.hasDied) invulnerablittyFX.Play();

        float buffTime = duration;

        while(buffTime > 0){
            buffTime -= Time.deltaTime;
            while(isPaused) {
                yield return null;
            }
            yield return null;
        }

        myPlayer.isInvulnerable = false;
        invulnerablittyFX.Stop();
    }

    IEnumerator DoubleCoinBuff(float duration)
    {
        doubleCoinActive = true;
        if(!myPlayer.hasDied) doubleCoinFX.Play();
        myGameManager.CoinInCreaseAmount = 2;

        float buffTime = duration;

        while(buffTime > 0){
            buffTime -= Time.deltaTime;
            while(isPaused) {
                yield return null;
            }
            yield return null;
        }

        myGameManager.CoinInCreaseAmount = 1;
        doubleCoinActive = false;
        doubleCoinFX.Stop();
    }

    IEnumerator CountDown(float countTime)
    {
        isActiveBuff = true;
        PowerUpTimeSlider.gameObject.SetActive(true);
        powerUpImage.gameObject.SetActive(true);
        countDownValue = countTime;
        while(countDownValue > 0f)
        {
            countDownValue -= Time.deltaTime;
            PowerUpTimeSlider.value = countDownValue / countTime;

            while(isPaused) {
                yield return null;
            }
            
            yield return null;
        }
        PowerUpTimeSlider.gameObject.SetActive(false);
        powerUpImage.gameObject.SetActive(false);
        isActiveBuff = false;
    }

    public bool ActiveBuff
    {
        get
        {
            return isActiveBuff;
        }
    }

    public bool IsPaused
    {
        get 
        {
            return isPaused;
        }
        set 
        {
            isPaused = value;
        }
    }

    public bool IsCountdown
    {
        get 
        {
            return isCountdown;
        }
        set{
            isCountdown = value;
        }
    }
}
