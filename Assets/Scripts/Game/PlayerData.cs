using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    int score = 0;
    int coin = 0;
    bool isDay = true;
    bool replay = false;
    // bool openShop = false;

    void Start()
    {
        if(FindObjectsOfType<PlayerData>().Length > 1)
        {
            Destroy(gameObject);
        }
        else DontDestroyOnLoad(gameObject);
    }

    public int Score
    {
        get 
        {
            return score;
        }
        set 
        {
            score = value;
        }
    }

    public int HighScore
    {
        get
        {
            return PlayerPrefs.GetInt("HighScore", 0);
        }
        set
        {
            PlayerPrefs.SetInt("HighScore", value);
        }
    }

    public int CollectedCoin
    {
        get
        {
            return coin;
        }
        set
        {
            coin = value;
        }
    }

    public int Coin
    {
        get 
        {
            return PlayerPrefs.GetInt("PlayerCoin",0);
        }
        set 
        {
            PlayerPrefs.SetInt("PlayerCoin", PlayerPrefs.GetInt("PlayerCoin",0) + value);
        }
    }

    public int SelectedCar
    {
        get
        {
            return PlayerPrefs.GetInt("SelectedCar",0);
        }
        set
        {
            PlayerPrefs.SetInt("SelectedCar", value);
        }
    }
    
    public bool isDayTime
    {
        get
        {
            return isDay;
        }
        set
        {
            isDay = value;
        }
    }

    public bool Replay
    {
        get 
        {
            return replay;
        }
        set
        {
            replay = value;
        }
    }

    // public bool OpenShop
    // {
    //     get 
    //     {
    //         return openShop;
    //     }
    //     set
    //     {
    //         openShop = value;
    //     }
    // }

}
