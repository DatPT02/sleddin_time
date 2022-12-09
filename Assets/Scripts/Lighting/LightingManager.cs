using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LightingManager : MonoBehaviour
{
    public Vector3 dayLightRotation;
    public Vector3 nightLightRotation;
    public Light displayLight;
    public GameObject ShopCanvas;
    public Button LightButton;
    public Image buttonImage;
    public Sprite DaySprite;
    public Sprite NightSprite;
    public float rotationSpeed;

    bool isDay = true;
    bool isrotating = false;
    Vector3 rotation;
    Player player;
    PlayerData playerData;
    GameManager myGameManager;

    GameObject playerHeadlight;
    void Start()
    {
        player = FindObjectOfType<Player>();
        playerData = FindObjectOfType<PlayerData>();
        myGameManager = FindObjectOfType<GameManager>();
        isDay = playerData.isDayTime;
        //playerHeadlight = player.getPlayerSledgeHeadlight();
    }

    void Update()
    {
        if(playerHeadlight == null && !ShopCanvas.activeInHierarchy && myGameManager.GameHasStarted) 
            playerHeadlight = player.getPlayerSledgeHeadlight();

        if(!isDay){
            if(ShopCanvas.activeInHierarchy && !isrotating)
            {
                displayLight.enabled = true;
            } 
            else if(displayLight.isActiveAndEnabled)
            {
                displayLight.enabled = false;
                //playerHeadlight = player.getPlayerSledgeHeadlight();
                if(playerHeadlight != null) playerHeadlight.SetActive(true);
            }
        }

        if(!isrotating)
        {
            if(isDay)
            {
                displayLight.enabled = false;
                GetComponent<Light>().enabled = true;
                if(playerHeadlight!= null) playerHeadlight.SetActive(false);
                transform.rotation = Quaternion.Euler(dayLightRotation);
                //LightButton.GetComponent<Image>().sprite = NightSprite;
                buttonImage.sprite = NightSprite;
            }else 
            {
                GetComponent<Light>().enabled = false;
                if(playerHeadlight!= null) playerHeadlight.SetActive(true);
                transform.rotation = Quaternion.Euler(nightLightRotation);
                //LightButton.GetComponent<Image>().sprite = DaySprite;
                buttonImage.sprite = DaySprite;
            }
        }
    }


    public void setLight()
    {
        if(!ShopCanvas.activeInHierarchy) playerHeadlight = player.getPlayerSledgeHeadlight();

        LightButton.interactable = false;
        isDay = !isDay;
        playerData.isDayTime = isDay;
        if(isDay) 
        {
            GetComponent<Light>().enabled = true;
            if(playerHeadlight != null)playerHeadlight.SetActive(false);
            displayLight.enabled = false;
            rotation = dayLightRotation;
            //LightButton.GetComponent<Image>().sprite = NightSprite;
            buttonImage.sprite = NightSprite;
        }
        else
        { 
            rotation = nightLightRotation;
            //LightButton.GetComponent<Image>().sprite = DaySprite;
            buttonImage.sprite = DaySprite;
        }

        StartCoroutine(rotateLight());

    }

    IEnumerator rotateLight()
    {
        isrotating = true;
        while(Mathf.Abs(transform.eulerAngles.x - rotation.x) > 1f)
        {
            transform.Rotate(new Vector3(1,0,0) * rotationSpeed * Time.deltaTime);
            yield return null;
        }

        LightButton.interactable = true;
        isrotating = false;
    }
}
