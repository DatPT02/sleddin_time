using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerShopManager : MonoBehaviour
{
    public GameObject powerUpManager;

    [Header("Object Display")]
    public GameObject objectDisplay;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public GameObject priceDisplay;
    public TextMeshProUGUI priceTag;
    public GameObject levelBar;
    public Button buyButton;
    public Button nextButton;
    public Button previousButton;
    private int currentObjectIndex = 0;

    Transform powerUpManagerTransform;
    PowerUpObject[] powerUps;

    void Start()
    {
        //powerUpManagerTransform = powerUpManager.transform;
        //powerUps = powerUpManagerTransform.GetComponentsInChildren<PowerUpObject>();
    }
    
    public void openShop()
    {
        powerUpManagerTransform = powerUpManager.transform;
        powerUps = powerUpManagerTransform.GetComponentsInChildren<PowerUpObject>();
        DisplayItem(currentObjectIndex);
    }

    void DisplayItem(int itemIndex) {
        previousButton.interactable = (itemIndex > 0);
        nextButton.interactable = (itemIndex < powerUps.Length - 1);

        GameObject PowerObject = powerUpManagerTransform.GetChild(itemIndex).gameObject;
        PowerUpObject PowerUp = PowerObject.GetComponent<PowerUpObject>();

        PowerUp.setShopItemElements(levelBar, buyButton, priceTag);

        nameText.text = PowerObject.transform.name;
        descriptionText.text = PowerUp.getDescripiton();
        if(PowerUp.isMaxLeveled()){
            priceDisplay.SetActive(false);
        }
        else{
            priceDisplay.SetActive(true);
            priceTag.gameObject.SetActive(true);
            priceTag.text = PowerUp.getPrice().ToString();
        }

        for(int i = 0; i < objectDisplay.transform.childCount; i ++)
        {
            Destroy(objectDisplay.transform.GetChild(i).gameObject);
        }

        GameObject objDisplay = Instantiate(PowerUp.getObjectDisplay(), objectDisplay.transform.position, Quaternion.identity);
        objDisplay.transform.SetParent(objectDisplay.transform, false);

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => {
            PowerUp.UpgradePower();
            PowerUp.setShopItemElements(levelBar, buyButton, priceTag);
        });
    }

    public void changeObject(int change){

        currentObjectIndex += change;
        DisplayItem(currentObjectIndex);
    }
    
}
