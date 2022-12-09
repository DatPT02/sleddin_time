using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using CrazyGames;

public class EndSceneManager : MonoBehaviour
{
    [Header("Distance")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI distanceHeaderText;
    [Header("Coin")]
    public TextMeshProUGUI coinText;
    [Header("HighScore")]
    public TextMeshProUGUI highScoreText;
    public GameObject highScoreGameObject;

    PlayerData myPlayerData;
    Player myPlayer;
    GameManager myGameManager;
    GenerateLevel myLevelGenerator;
    void Start()
    {
        myPlayerData = FindObjectOfType<PlayerData>();
        myPlayer = FindObjectOfType<Player>();
        myGameManager = FindObjectOfType<GameManager>();
        myLevelGenerator = FindObjectOfType<GenerateLevel>();
    }

    void Update()
    {
        distanceText.text = myPlayerData.Score.ToString() + "m";
        highScoreText.text = myPlayerData.HighScore.ToString() + "m";
        coinText.text = myPlayerData.CollectedCoin.ToString();
        if(myPlayerData.Score > myPlayerData.HighScore)
        {
            distanceHeaderText.text = "New Best Distance";
            myPlayerData.HighScore = myPlayerData.Score;
            highScoreGameObject.SetActive(false);
        }
    }

    public void Replay(bool replay)
    {
        myPlayerData.Coin = myPlayerData.CollectedCoin;
        myPlayerData.Replay = replay;
        //myPlayerData.OpenShop = !replay;
        SceneManager.LoadScene("MainScene");

        // myPlayer.Replay();
        // myLevelGenerator.ResetGround();
        // myGameManager.startGame();
    }
}
