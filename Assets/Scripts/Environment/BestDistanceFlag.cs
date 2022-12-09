using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestDistanceFlag : MonoBehaviour
{
    public float checkDistance = 1f;

    Player player;
    PlayerData playerData;
    bool highScoreAvailable = true;
    bool highScorebreak = false;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        playerData = FindObjectOfType<PlayerData>();

        if(playerData.HighScore == 0){ 
            highScoreAvailable = false;
            gameObject.SetActive(false);
        }
        else{
            transform.position = player.transform.position + new Vector3(0,-1, playerData.HighScore);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z - player.transform.position.z < checkDistance)
        {
            if(highScoreAvailable && !highScorebreak) 
            {
                highScorebreak = true;
            }
        }
    }
}
