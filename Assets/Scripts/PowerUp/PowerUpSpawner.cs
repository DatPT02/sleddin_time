using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject PowerUpManager;
    public float minSpawnDistanceFromPlayer = 500f;
    public float maxSpawnDistanceFromPlayer = 1000f;
    Player player;
    HandlePowerUp PowerUpHandler;
    GameManager myGameManager;
    private float previousPowerUp = -1;
    // Start is called before the first frame update
    void Start()
    {
        myGameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
        PowerUpHandler = FindObjectOfType<HandlePowerUp>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myGameManager.GameHasStarted && hasUnlockedPowerUp()){
            if(!PowerUpHandler.ActiveBuff && GetActiveBuff() == null)
            {
                SpawnNewBuff();
            }    
            else if(GetActiveBuff() != null && PowerUpManager.transform.position.z < player.transform.position.z)
            {
                SpawnNewBuff();
            }
        }
    }

    GameObject GetActiveBuff()
    {
        for(int i=0; i <PowerUpManager.transform.childCount; i++)
        {
            if(PowerUpManager.transform.GetChild(i).gameObject.GetComponent<Light>()) continue;
            if(PowerUpManager.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                return PowerUpManager.transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    void SpawnNewBuff()
    {
        begin:
        int buffIndex = Random.Range(0, PowerUpManager.transform.childCount);
        if(buffIndex == previousPowerUp) goto begin;
        previousPowerUp = buffIndex;

        for(int i=0; i < PowerUpManager.transform.childCount; i++)
        {
            if(PowerUpManager.transform.GetChild(i).gameObject.GetComponent<Light>()) continue;
            if(i == buffIndex && PowerUpManager.transform.GetChild(i).gameObject.GetComponent<PowerUpObject>().isUnlocked)
            {
                PowerUpManager.transform.GetChild(i).gameObject.SetActive(true);
            }
            else PowerUpManager.transform.GetChild(i).gameObject.SetActive(false);
        }
        PowerUpManager.transform.position = GetSpawnPosition();
    }

    public Vector3 GetSpawnPosition()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("PowerSpawnPoint");
        float distanceFromPlayer = Random.Range(minSpawnDistanceFromPlayer, maxSpawnDistanceFromPlayer);
        Vector3 spawnPosition =  new Vector3();
        foreach(var obj in spawnPoints) {
            if(obj.transform.position.z - player.transform.position.z > distanceFromPlayer)
            {
                spawnPosition = obj.transform.position;
                break;
            }
        }
        return spawnPosition;
    }

    private bool hasUnlockedPowerUp()
    {
        for(int i=0; i < PowerUpManager.transform.childCount; i++)
        {
            if(PowerUpManager.transform.GetChild(i).gameObject.GetComponent<Light>()) continue;
            if(PowerUpManager.transform.GetChild(i).gameObject.GetComponent<PowerUpObject>().isUnlocked)
            {
                return true;
            }
        }
        return false;
    }
}
