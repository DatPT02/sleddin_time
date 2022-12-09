using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrazyGames;

public class ShopManager : MonoBehaviour
{
    [Header("Interactables")]
    public Button previousButton;
    public Button nextButton;
    public Button buyButton;
    public Button selectButton;
    public TextMeshProUGUI selectText;
    public TextMeshProUGUI priceTag;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public GameObject PowerShop;
    [Header("Ad UI")]
    public Button adButton;
    public Image coinImage;
    public TextMeshProUGUI adDisplayText;
    public GameObject AdRewardDisplay;
    public int adAvailableTimeHours = 12;
    public int rewardAmount;
    
    [Header("Audio")]
    public AudioSource buyAudio;
    public AudioSource selectAudio;
    private int currentCar = 0;
    bool adAvailable = true;

    Player myPlayer;
    PlayerData myPlayerData;
    void Start()
    {
        myPlayer = FindObjectOfType<Player>();
        myPlayerData = FindObjectOfType<PlayerData>();
        adDisplayText.text = rewardAmount.ToString();
        AdRewardDisplay.SetActive(false);
    }

    void Update()
    {
        if(DateTime.Compare(DateTime.Now, DateTime.Parse(PlayerPrefs.GetString("targetAdAvailableTime", DateTime.Now.ToString()))) <= 0)
        {
            adAvailable = false;
            adButton.interactable = false;
            AdAvailableTimeCount();
        }
        else {
            adAvailable = true;
            adButton.interactable = true;
        }
    }

    public void OpenShop()
    {
        myPlayer = FindObjectOfType<Player>();
        myPlayerData = FindObjectOfType<PlayerData>();
        PowerShop.SetActive(false);
        currentCar = myPlayerData.SelectedCar;
        DisplayRide(myPlayerData.SelectedCar);
    }

    public void CloseShop()
    {
        myPlayer = FindObjectOfType<Player>();
        myPlayerData = FindObjectOfType<PlayerData>();
        DisplayRide(myPlayerData.SelectedCar);
        myPlayer.SetRide(getCurrentRide());

        for (int i = 0; i <transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void DisplayRide(int ride_index)
    {

        previousButton.interactable = (ride_index > 0);
        nextButton.interactable = (ride_index < transform.childCount - 1);

        nameText.text = transform.GetChild(ride_index).gameObject.name;
        descriptionText.text = transform.GetChild(ride_index).gameObject.GetComponent<RideSelection>().getDescriptionText();

        for (int i = 0; i <transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == ride_index);
        }


        if(transform.GetChild(ride_index).gameObject.GetComponent<RideSelection>().isUnlocked == true)
        {
            selectButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);

            if(ride_index == myPlayerData.SelectedCar)
            {
                selectText.text = "Selected";
                selectButton.interactable = false;
            }
            else 
            {
                selectText.text = "Select";
                selectButton.interactable = true;
            }
        }
        else
        {
            selectButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);

            if(myPlayerData.Coin < transform.GetChild(currentCar).GetComponent<RideSelection>().Price) {
                buyButton.interactable = false;
                priceTag.color = Color.red;
            }
            else
            {
                buyButton.interactable = true;
                priceTag.color = Color.green;
            }

            priceTag.text = transform.GetChild(ride_index).GetComponent<RideSelection>().Price.ToString();
        }
    }

    public void changeRide(int change)
    {
        currentCar += change;
        DisplayRide(currentCar);
    }

    public GameObject getCurrentRide()
    {
        for (int i = 0; i <transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeInHierarchy)
            {
                return transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    public void selectRide()
    {
        selectAudio.volume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        selectAudio.Play();
        myPlayerData.SelectedCar = currentCar;
        DisplayRide(myPlayerData.SelectedCar);
    }

    public void buyRide()
    {
        //AudioSource.PlayClipAtPoint(buyAudio,FindObjectOfType<Player>().transform.position, PlayerPrefs.GetFloat("SfxVolume", 1f));
        buyAudio.volume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        buyAudio.Play();
        transform.GetChild(currentCar).GetComponent<RideSelection>().isUnlocked = true;
        selectRide();
        myPlayerData.Coin = -transform.GetChild(currentCar).GetComponent<RideSelection>().Price;
    }

    public void OpenPowerShop()
    {
        PowerShop.SetActive(true);
        PowerShop.transform.Find("PowerShopManager").GetComponent<PowerShopManager>().openShop();
    }

    public void ClosePowerShop()
    {
        PowerShop.SetActive(false);
    }



    public void playAd()
    {
        CrazyAds.Instance.beginAdBreakRewarded(() =>{
            myPlayerData.GetComponent<AudioSource>().Pause();
            adComplete();
            }, null);
        DateTime now = DateTime.Now;

        DateTime target = now.AddHours(adAvailableTimeHours);
        PlayerPrefs.SetString("targetAdAvailableTime", target.ToString());

        adAvailable = false;
    }

    private void AdAvailableTimeCount()
    {
        DateTime current = DateTime.Now;
        DateTime targetTime = DateTime.Parse(PlayerPrefs.GetString("targetAdAvailableTime", DateTime.Now.ToString()));
        TimeSpan remainingTime = calculateRemainningTime(current, targetTime);

        if(remainingTime.Hours >= adAvailableTimeHours && remainingTime.Minutes > 0)
        {
            adAvailable = true;
            adButton.interactable = true;
            coinImage.gameObject.SetActive(true);
            adDisplayText.text = rewardAmount.ToString();
            PlayerPrefs.SetString("targetAdAvailableTime", DateTime.Now.ToString());
            return;
        }

        if(DateTime.Compare(current, targetTime) <= 0) 
        {
            adDisplayText.text = remainingTime.ToString();
            coinImage.gameObject.SetActive(false);
        }
        else 
        {
            adAvailable = true;
            adButton.interactable = true;
            coinImage.gameObject.SetActive(true);
            adDisplayText.text = rewardAmount.ToString();
        }
    }

    TimeSpan calculateRemainningTime(DateTime from, DateTime to)
    {
        from = new DateTime(from.Year, from.Month, from.Day , from.Hour, from.Minute, from.Second, 0);
        to = new DateTime(to.Year, to.Month, to.Day , to.Hour, to.Minute, to.Second, 0);
        TimeSpan diff = to - from;
        return diff;
    }
    void adComplete()
    {
        myPlayerData.GetComponent<AudioSource>().UnPause();
        AdRewardDisplay.SetActive(true);
        for(int i = 0; i < AdRewardDisplay.transform.childCount; i ++)
        {
            if(AdRewardDisplay.transform.GetChild(i).GetComponent<TextMeshProUGUI>())
            {
                AdRewardDisplay.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = rewardAmount.ToString();
            }
        }

        StartCoroutine(addCoin(300));
    }

    IEnumerator addCoin(int value)
    {
        while(value > 0)
        {
            value --;
            myPlayerData.Coin = 1;
            yield return null;
        }
    }
}
