using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Header("Camera")]
    public GameObject mainCamera;
    public GameObject shopCamera;
    public GameObject collisionCamera;
    [Header("Canvas")]
    public GameObject mainCanvas;
    public GameObject shopCanvas;
    public GameObject topCanvas;

    GameManager myGameManager;
    ShopManager myShopManager;
    Player myPlayer;
    PlayerData myPlayerData;

    void Start()
    {
        myGameManager = GetComponent<GameManager>();
        myShopManager = FindObjectOfType<ShopManager>();
        myPlayer = FindObjectOfType<Player>();
        myPlayerData = FindObjectOfType<PlayerData>();

        if(myPlayerData.Replay){
            setMainCameraActive();
        }
        else {
            setShopCameraActive();
        }

        //setMainCameraActive();
    }

    public void setMainCameraActive()
    {
        if(myShopManager) myShopManager.CloseShop();
        mainCamera.SetActive(true);
        shopCamera.SetActive(false);
        collisionCamera.SetActive(false);

        StartCoroutine(setGameMenu());
    }

    public void setShopCameraActive()
    {
        if(myShopManager) myShopManager.OpenShop();
        mainCamera.SetActive(false);
        shopCamera.SetActive(true);
        collisionCamera.SetActive(false);

        StartCoroutine(setShopMenu());
    }

    public void setCollisionCameraActive()
    {
        collisionCamera.SetActive(true);
        mainCamera.SetActive(false);
        shopCamera.SetActive(false);
    }

    IEnumerator setGameMenu()
    {
        shopCanvas.SetActive(false);
        topCanvas.SetActive(false);

        while(!isMainCameraActive)
        {
            yield return null;
        }

        mainCanvas.SetActive(true);
    }

    IEnumerator setShopMenu()
    {
        mainCanvas.SetActive(false);

        while(!isShopCameraActive)
        {
            yield return null;
        }

        shopCanvas.SetActive(true);
        topCanvas.SetActive(true);
    }

    public bool isShopCameraActive
    {
        get
        {
            return Vector3.Distance(Camera.main.gameObject.transform.position, shopCamera.transform.position) < 0.1f;
        }
    }

    public bool isMainCameraActive
    {
        get
        {
            return Vector3.Distance(Camera.main.gameObject.transform.position, mainCamera.transform.position) < 0.1f;
        }
    }
}
