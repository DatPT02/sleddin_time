using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpObject : MonoBehaviour
{
    //public ParticleSystem pickupFX;
    public AudioSource pickupSFX;
    public PowerUp powerUp;
    public Sprite powerUpImage;
    public string description;
    public GameObject objectDisplay;
    public AudioSource buyAudio;

    public float[] buffTime;
    public bool unlocked = false;
    
    public int maxUpgradeLevel = 6;
    public int upgradeLevel = 0;

    public int[] price;

    PlayerData myPlayerData;
    Player myPlayer;

    void Start()
    {
        myPlayerData = FindObjectOfType<PlayerData>();
        myPlayer = FindObjectOfType<Player>();
        upgradeLevel = PlayerPrefs.GetInt(transform.name, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //pickupFX.gameObject.transform.position = transform.position;
            //pickupFX.Play();
            //AudioSource.PlayClipAtPoint(pickupSFX, myPlayer.transform.position, PlayerPrefs.GetFloat("SfxVolume", 1f));
            pickupSFX.volume = PlayerPrefs.GetFloat("SfxVolume", 1f);
            pickupSFX.Play();
            //Debug.Log(pickupSFX.isPlaying);
            powerUp.ApplyPowerUp(other.gameObject, buffTime[upgradeLevel-1]);
            gameObject.SetActive(false);
        }
    }

    public bool isUnlocked
    {
        get
        {
            if(upgradeLevel > 0) unlocked = true;
            return unlocked;
        }
        set{
            unlocked = value;
        }
    }

    public void UpgradePower()
    {
        if(!isMaxLeveled()) {
            //AudioSource.PlayClipAtPoint(buyAudio,FindObjectOfType<Player>().transform.position, PlayerPrefs.GetFloat("SfxVolume", 1f));
            buyAudio.volume = PlayerPrefs.GetFloat("SfxVolume", 1f);
            buyAudio.Play();
            myPlayerData.Coin = -getPrice();
            upgradeLevel ++;
            PlayerPrefs.SetInt(transform.name, upgradeLevel);
        }
        if(upgradeLevel > 0) unlocked = true;
    }

    public int getPrice()
    {
        return price[upgradeLevel];
    }

    public bool isMaxLeveled()
    {
        return upgradeLevel == maxUpgradeLevel;
    }

    public Sprite getPowerUpSprite()
    {
        return powerUpImage;
    }

    public GameObject getObjectDisplay()
    {
        return objectDisplay;
    }

    public string getDescripiton()
    {
        return description;
    }

    public void setShopItemElements(GameObject levelBar, Button buyButton, TextMeshProUGUI priceTag)
    {
        TextMeshProUGUI buyText = buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        buyText.text = upgradeLevel == 0 ? "Unlock" : "Upgrade";
        buyButton.interactable = true;

        if(!isMaxLeveled()){
            if(myPlayerData.Coin >= getPrice()) {
                buyText.color = Color.green;
            }
            else {
                buyText.color = Color.red;
                buyButton.interactable = false;
            }

            priceTag.text = getPrice().ToString();
        }else {
            priceTag.gameObject.SetActive(false);
            buyButton.interactable = false;
            buyText.text = "Max Level";
            buyText.color = Color.white;
        }

        for(int i=0; i < maxUpgradeLevel; i ++)
        {
            levelBar.transform.GetChild(i).GetComponent<Image>().color = Color.gray;
        }

        for(int i=0; i < upgradeLevel; i ++)
        {
            levelBar.transform.GetChild(i).GetComponent<Image>().color = Color.green;
        }
    }
}
