using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using CrazyGames;

public class GameManager : MonoBehaviour
{
    [Header("Text Display")]
    public TextMeshProUGUI collectedCoinText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI playerCoinText;
    public TextMeshProUGUI bestDistanceText;
    [Header("Menu")]
    public Canvas GameMenu;
    public Canvas pauseMenu;
    public Canvas TopMenu;
    public Canvas EndMenu;
    [Header("Start Menu")]
    // public GameObject startText;
    // public GameObject shopButton;
    public Button PauseButton;
    [Header("Setting Menu")]
    public Slider volumeSlider;
    public Slider SfxVolumeSlider;
    [Header("Utility")]
    public int countdownTime = 3;
    Player player;

    int coin = 0;
    int coinIncreaseAmount = 1;
    int distance = 0;
    float initialDistance = 0;
    int startTime = 0;
    PlayerData data;
    CameraSwitch myCameraSwitch;

    bool hasStarted;
    private bool startCondition;

    void Start()
    {
        GameMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        EndMenu.gameObject.SetActive(false);

        hasStarted = false;

        player = FindObjectOfType<Player>();
        data = FindObjectOfType<PlayerData>();
        myCameraSwitch = GetComponent<CameraSwitch>();

        player.isMovable = false;
        initialDistance = player.gameObject.transform.position.z;

        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        SfxVolumeSlider.value = PlayerPrefs.GetFloat("SfxVolume", 1f);
        changeVolume();
        changeSfxVolume();

        playerCoinText.text = PlayerPrefs.GetInt("PlayerCoin",0).ToString();

        if(data.Replay)
        { 
            startGame();
        }
        // else if(data.OpenShop)
        // {
        //     myCameraSwitch.setShopCameraActive();
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameMenu.isActiveAndEnabled){
            collectedCoinText.text = coin.ToString();
            if(player.isMovable) distance = (int)Mathf.Floor((player.gameObject.transform.position.z - initialDistance));
            distanceText.text = distance.ToString() + "m";
            if(distance < data.HighScore) {
                bestDistanceText.text = TravelDistance.ToString() + "m";
            }
            else
            {
                bestDistanceText.transform.parent.gameObject.SetActive(false);
            }
        }

        if(TopMenu.isActiveAndEnabled){
            playerCoinText.text = PlayerPrefs.GetInt("PlayerCoin",0).ToString();
        }

        // #if UNITY_STANDALONE || UNITY_WEBGL
        // bool mouseButtonDown = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
        // if(startText.activeInHierarchy){
        //     if(Input.anyKeyDown && !mouseButtonDown)
        //     {
        //         startGame();
        //     }
        // }
        // #endif
    }

    public void startGame()
    {
        EndMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        TopMenu.gameObject.SetActive(false);

        // startText.SetActive(false);
        // shopButton.SetActive(false);

        StartCoroutine(beginStart());
    }
    
    IEnumerator beginStart()
    {
        myCameraSwitch.setMainCameraActive();
        PauseButton.interactable = false;

        while(!myCameraSwitch.isMainCameraActive)
        {
            yield return null;
        }
        
        startTime = countdownTime;
        countdownText.gameObject.SetActive(true);
        while(startTime > 0){
            countdownText.text = startTime.ToString();
            yield return new WaitForSeconds(1f);
            startTime--;
        }
        
        countdownText.gameObject.SetActive(false);
        PauseButton.interactable = true;
        hasStarted = true;
        CrazyEvents.Instance.GameplayStart();
        player.isMovable = true;
    }

    public void increaseCoinCount()
    {
        coin += coinIncreaseAmount;
    }

    public int CoinInCreaseAmount
    {
        set
        {
            coinIncreaseAmount = value;
        }
    }

    IEnumerator countDown()
    {
        startTime = countdownTime;
        countdownText.gameObject.SetActive(true);
        while(startTime > 0){
            countdownText.text = startTime.ToString();
            yield return new WaitForSeconds(1f);
            startTime--;
        }

        if(!isPauseMenuActive()){
            countdownText.gameObject.SetActive(false);
            player.isMovable = true;
            CrazyEvents.Instance.GameplayStart();
        }

        //FindObjectOfType<HandlePowerUp>().IsPaused = false;
    }

    public bool GameHasStarted
    {
        get
        {
            return hasStarted;
        }
    }
    public void LoadEndScene()
    {
        hasStarted = false;
        CrazyEvents.Instance.GameplayStop();
        data.Score = distance;
        data.CollectedCoin = coin;
        EndMenu.gameObject.SetActive(true);
    }

    public void loadPauseMenu()
    {
        if(player.hasDied) return;

        pauseMenu.gameObject.SetActive(!pauseMenu.isActiveAndEnabled);
        if(hasStarted) GameMenu.gameObject.SetActive(!pauseMenu.isActiveAndEnabled);

        //FindObjectOfType<HandlePowerUp>().IsPaused = true;

        if(pauseMenu.isActiveAndEnabled)
        {
            player.isMovable = false;
            CrazyEvents.Instance.GameplayStop();
        }
        else if(hasStarted) StartCoroutine(countDown());

    }

    public bool isPauseMenuActive()
    {
        return pauseMenu.isActiveAndEnabled;
    }

    public void LoadSettingMenu()
    {
        pauseMenu.gameObject.SetActive(!pauseMenu.isActiveAndEnabled);
    }

    public void changeVolume()
    {
        data.GetComponent<AudioSource>().volume = volumeSlider.value;
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    public void changeSfxVolume()
    {
        PlayerPrefs.SetFloat("SfxVolume", SfxVolumeSlider.value);
    }

    public float TravelDistance
    {
        get
        {
            return (data.HighScore - distance);
        }
    }
}
